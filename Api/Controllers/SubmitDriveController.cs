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
using Core.ApplicationServices.Logger;

namespace Api.Controllers
{
    public class SubmitDriveController : BaseController
    {
        private IUnitOfWork _uow { get; set; }
        private IGenericRepository<DriveReport> _driveReportRepo { get; set; }
        private IGenericRepository<Token> _tokenRepo { get; set; }
        private IGenericRepository<Rate> _rateRepo { get; set; }
        

        public SubmitDriveController(IUnitOfWork uow, IGenericRepository<Rate> rateRepo, IGenericRepository<DriveReport> driveReportRepo, IGenericRepository<Token> tokenRepo, ILogger logger) : base(logger)
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
                    ui.rates = AutoMapper.Mapper.Map<List<RateViewModel>>(_rateRepo.Get().Where(x=> x.isActive).ToList());

                    return Ok(ui);
                }
                catch (Exception ex)
                {
                    _logger.Error($"{GetType().Name}, Post(), Could not save drivereport, for user: {driveObject.DriveReport.ProfileId}", ex);
                    return new CustomErrorActionResult(Request, "Could not save", ErrorCodes.SaveError, HttpStatusCode.BadRequest);
                }

            }
            else
            {
                _logger.Debug($"{GetType().Name}, Post(), Token not found ");
                return new CustomErrorActionResult(Request, "Token not found", ErrorCodes.InvalidAuthorization, HttpStatusCode.Unauthorized);
            }
        }
    }
}