using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.DomainModel;
using Core.DomainServices;
using AutoMapper;
using Api.Models;
using Api.Encryption;

namespace Api.Controllers
{
    public class SubmitDriveController : ApiController
    {
        private IUnitOfWork _uow { get; set; }
        private IGenericRepository<DriveReport> _driveReportRepo { get; set; }
        private IGenericRepository<Token> _tokenRepo { get; set; }
        private IGenericRepository<Rate> _rateRepo { get; set; }

        public SubmitDriveController(IUnitOfWork uow, IGenericRepository<Rate> rateRepo, IGenericRepository<DriveReport> driveReportRepo, IGenericRepository<Token> tokenRepo)
        {
            _uow = uow;
            _driveReportRepo = driveReportRepo;
            _tokenRepo = tokenRepo;
            _rateRepo = rateRepo;
        }

        public class DriveObject
        {
            public DriveReportViewModel DriveReport { get; set; }
            public TokenViewModel Token { get; set; }
        }

        public IHttpActionResult Post(DriveObject driveObject)
        {
            driveObject.Token = Encryptor.EncryptToken(driveObject.Token);
            Token token = _tokenRepo.Get(x => x.GuId == driveObject.Token.GuId && x.Status == 1).FirstOrDefault();

            if (token != null)
            {
                try
                {
                    //Add drivereport
                    driveObject.DriveReport = Encryptor.EncryptDriveReport(driveObject.DriveReport);

                    var model = AutoMapper.Mapper.Map<DriveReport>(driveObject.DriveReport);

                    _driveReportRepo.Insert(model);
                    _uow.Save();

                    //Return user info (optional)
                    var profile = AutoMapper.Mapper.Map<ProfileViewModel>(token.Profile);
                    profile = Encryptor.DecryptProfile(profile);

                    UserInfoViewModel ui = new UserInfoViewModel();
                    ui.profile = profile;
                    ui.rates = AutoMapper.Mapper.Map<List<RateViewModel>>(_rateRepo.Get().ToList());

                    return Ok(ui);
                }
                catch (Exception ex)
                {
                    return new CustomErrorActionResult(Request, "Could not save", ErrorCodes.SaveError, HttpStatusCode.BadRequest);
                }

            }
            else
            {
                return new CustomErrorActionResult(Request, "Token not found", ErrorCodes.TokenNotFound, HttpStatusCode.Unauthorized);
            }
        }
    }
}