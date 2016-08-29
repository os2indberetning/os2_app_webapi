using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Encryption;
using Api.Models;
using Core.DomainModel;
using Core.DomainModel.Model;
using Core.DomainServices;
using Core.ApplicationServices.Logger;

namespace Api.Controllers
{
    public class ReportController : ApiController
    {

        private IUnitOfWork Uow { get; set; }
        private IGenericRepository<DriveReport> DriveReportRepo { get; set; }
        private IGenericRepository<UserAuth> AuthRepo { get; set; }
       

        public ReportController(IUnitOfWork uow, IGenericRepository<DriveReport> driveReportRepo, IGenericRepository<UserAuth> authRepo)
        {
            Uow = uow;
            DriveReportRepo = driveReportRepo;
            AuthRepo = authRepo;
        }

        public class DriveObject
        {
            public DriveReportViewModel DriveReport { get; set; }
            public AuthorizationViewModel Authorization { get; set; }
        }

        // POST /report
        public IHttpActionResult Post(DriveObject driveObject)
        {
            ILogger _logger = new Logger();
            _logger.Log("Post /report. Object DriveObject.AuthorizationGuid initial: " + driveObject.Authorization.GuId, "api", 3);
            var encryptedGuId = Encryptor.EncryptAuthorization(driveObject.Authorization).GuId;
            var auth = AuthRepo.Get(t => t.GuId == encryptedGuId).FirstOrDefault();
            var DuplicateReportCheck = DriveReportRepo.Get(t => t.Uuid == driveObject.DriveReport.Uuid).Any();

            if (auth == null)
            {
                _logger.Log("Post /report. Invalid authorization", "api", 3);
                return new CustomErrorActionResult(Request, "Invalid authorization", ErrorCodes.InvalidAuthorization,
                    HttpStatusCode.Unauthorized);
            }
            if(auth.ProfileId != driveObject.DriveReport.ProfileId)
            {
                _logger.Log("Post /report. User and drive report user do not match", "api", 3);
                return new CustomErrorActionResult(Request, "User and drive report user do not match", ErrorCodes.ReportAndUserDoNotMatch,
                     HttpStatusCode.Unauthorized);
            }
            if (DuplicateReportCheck)
            {
                _logger.Log("Post /report. Report rejected, duplicate found", "api", 3);
                return new CustomErrorActionResult(Request, "Report rejected, duplicate found", ErrorCodes.DuplicateReportFound, HttpStatusCode.OK);
            }

            try
            {
                driveObject.DriveReport = Encryptor.EncryptDriveReport(driveObject.DriveReport);

                var model = AutoMapper.Mapper.Map<DriveReport>(driveObject.DriveReport);

                DriveReportRepo.Insert(model);
                Uow.Save();

                _logger.Log("Post /report. Before ok", "api", 3);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log("Post /report. Exception Could not save drivereport: " + ex.Message, "api", 3);
                return new CustomErrorActionResult(Request, "Could not save drivereport", ErrorCodes.SaveError, HttpStatusCode.BadRequest);
            }
        }


    }
}
