using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Encryption;
using Api.Models;
using Core.DomainModel;
using Core.DomainServices;

namespace Api.Controllers
{
    public class ReportController : ApiController
    {

        private IUnitOfWork Uow { get; }
        private IGenericRepository<DriveReport> DriveReportRepo { get; }
        private IGenericRepository<Token> TokenRepo { get; }

        public ReportController(IUnitOfWork uow, IGenericRepository<DriveReport> driveReportRepo, IGenericRepository<Token> tokenRepo)
        {
            Uow = uow;
            DriveReportRepo = driveReportRepo;
            TokenRepo = tokenRepo;
        }

        public class DriveObject
        {
            public DriveReportViewModel DriveReport { get; set; }
            public TokenViewModel Token { get; set; }
        }


        public IHttpActionResult Post(DriveObject driveObject)
        {
            driveObject.Token = Encryptor.EncryptToken(driveObject.Token);
            var token = TokenRepo.Get(x => x.GuId == driveObject.Token.GuId && x.Status == 1).FirstOrDefault();

            if (token == null)
                return new CustomErrorActionResult(Request, "Token not found", ErrorCodes.TokenNotFound,
                    HttpStatusCode.Unauthorized);

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
                return new CustomErrorActionResult(Request, "Could not save", ErrorCodes.SaveError, HttpStatusCode.BadRequest);
            }
        }


    }
}
