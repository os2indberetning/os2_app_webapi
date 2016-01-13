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
    public class UserDataController : ApiController
    {
        private IUnitOfWork _uow { get; set; }
        private IGenericRepository<Rate> _rateRepo{ get; set; }
        private IGenericRepository<Token> _tokenRepo { get; set; }

        public UserDataController(IUnitOfWork uow, IGenericRepository<Rate> rateRepo, IGenericRepository<Token> tokenRepo)
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

                UserInfoViewModel ui = new UserInfoViewModel();
                ui.profile = profile;
                ui.rates = AutoMapper.Mapper.Map <List<RateViewModel>> (_rateRepo.Get().ToList());

                return Ok(ui);
            }
            else
            {
                return new CustomErrorActionResult(Request, "Token not found", ErrorCodes.TokenNotFound, HttpStatusCode.Unauthorized);
            }
        }
    }
}
