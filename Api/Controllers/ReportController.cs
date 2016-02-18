using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Encryption;
using Api.Models;
using Core.DomainModel;
using Core.DomainModel.Model;
using Core.DomainServices;

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
            var encryptedGuId = Encryptor.EncryptAuthorization(driveObject.Authorization).GuId;
            var auth = AuthRepo.Get(t => t.GuId == encryptedGuId).FirstOrDefault();
            
            if (auth == null)
                return new CustomErrorActionResult(Request, "Invalid authorization", ErrorCodes.InvalidAuthorization,
                    HttpStatusCode.Unauthorized);

            if(auth.ProfileId != driveObject.DriveReport.ProfileId)
            {
                return new CustomErrorActionResult(Request, "User and drive report user do not match", ErrorCodes.ReportAndUserDoNotMatch,
                     HttpStatusCode.Unauthorized);
            }

            try
            {
                driveObject.DriveReport = Encryptor.EncryptDriveReport(driveObject.DriveReport);

                var model = AutoMapper.Mapper.Map<DriveReport>(driveObject.DriveReport);

                DriveReportRepo.Insert(model);
                Uow.Save();

                return Ok();
            }
            catch (Exception)
            {
                return new CustomErrorActionResult(Request, "Could not save drivereport", ErrorCodes.SaveError, HttpStatusCode.BadRequest);
            }
        }


    }
}
