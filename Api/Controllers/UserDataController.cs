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
    public class UserDataController : BaseController
    {
        private IUnitOfWork _uow { get; set; }
        private IGenericRepository<Rate> _rateRepo{ get; set; }
        private IGenericRepository<Token> _tokenRepo { get; set; }
        

        public UserDataController(IUnitOfWork uow, IGenericRepository<Rate> rateRepo, IGenericRepository<Token> tokenRepo, ILogger logger) : base(logger)
        {
            _uow = uow;
            _rateRepo = rateRepo;
            _tokenRepo = tokenRepo;
        }

        // POST api/userdata
        public IHttpActionResult Post(TokenViewModel obj)
        {
            obj = Encryptor.EncryptToken(obj);
            Token token = _tokenRepo.Get(t => t.GuId == obj.GuId && t.Status == 1).FirstOrDefault();

            if (token != null)
            {
                var profile = AutoMapper.Mapper.Map<ProfileViewModel>(token.Profile);
                profile = Encryptor.DecryptProfile(profile);
                var currentTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                profile.Employments = profile.Employments.AsQueryable().Where(x => x.StartDateTimestamp < currentTimestamp && (x.EndDateTimestamp > currentTimestamp || x.EndDateTimestamp == 0)).ToList();

                UserInfoViewModel ui = new UserInfoViewModel();
                ui.profile = profile;
                ui.rates = AutoMapper.Mapper.Map <List<RateViewModel>> (_rateRepo.Get().Where(x=> x.isActive).ToList());

                return Ok(ui);
            }
            else
            {
                _logger.Debug($"{GetType().Name}, Post(), Token not found for {obj.GuId}");
                return new CustomErrorActionResult(Request, "Token not found", ErrorCodes.InvalidAuthorization, HttpStatusCode.Unauthorized);
            }
        }
    }
}
