﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Encryption;
using Core.DomainModel;
using Core.DomainServices;
using Api.Models;

namespace Api.Controllers
{
    public class SyncWithTokenController : ApiController
    {
        private IUnitOfWork _uow { get; set; }
        private IGenericRepository<Core.DomainModel.Profile> _profileRepo { get; set; }
        private IGenericRepository<Token> _tokenRepo { get; set; }
        private IGenericRepository<Rate> _rateRepo { get; set; }

        public SyncWithTokenController(IUnitOfWork uow, IGenericRepository<Rate> rateRepo, IGenericRepository<Core.DomainModel.Profile> profileRepo, IGenericRepository<Token> tokenRepo)
        {
            _uow = uow;
            _profileRepo = profileRepo;
            _tokenRepo = tokenRepo;
            _rateRepo = rateRepo;
        }

        // POST api/userdata
        public IHttpActionResult Post(TokenViewModel obj)
        {
            obj = Encryptor.EncryptToken(obj);

            //Confirm link with token
            var tokens = _tokenRepo.Get(t => t.TokenString == obj.TokenString);

            if (tokens.Any())
            {
                //There could be multiple, loop if that is the case
                foreach (var token in tokens)
                {
                    if (token.Status == 2)
                    {
                        token.Status = 1;
                        _uow.Save();


                        //Return user info (optional)
                        var profile = AutoMapper.Mapper.Map<ProfileViewModel>(token.Profile);
                        profile = Encryptor.DecryptProfile(profile);

                        UserInfoViewModel ui = new UserInfoViewModel();
                        ui.profile = profile;
                        ui.rates = AutoMapper.Mapper.Map<List<RateViewModel>>(_rateRepo.Get().ToList());

                        return Ok(ui);
                    }
                }

                return new CustomErrorActionResult(Request, "Token allready used", ErrorCodes.TokenAllreadyActivated, HttpStatusCode.BadRequest);
                
            }
            else
            {
                return new CustomErrorActionResult(Request,"Token not found", ErrorCodes.InvalidAuthorization, HttpStatusCode.Unauthorized);
            }
        }
    }
}
