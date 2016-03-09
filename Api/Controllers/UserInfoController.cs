﻿using System;
using System.Collections.Generic;
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
    public class UserInfoController : ApiController
    {
        private IGenericRepository<Rate> RateRepo { get; set; }
        private IGenericRepository<UserAuth> AuthRepo { get; set; }

        public UserInfoController(IGenericRepository<Rate> rateRepo, IGenericRepository<UserAuth> authRepo)
        {
            RateRepo = rateRepo;
            AuthRepo = authRepo;
        }

        // Post api/userinfo
        public IHttpActionResult Post(AuthorizationViewModel obj)
        {
            var encryptedGuid = Encryptor.EncryptAuthorization(obj).GuId;

            var auth = AuthRepo.Get(t => t.GuId == encryptedGuid).FirstOrDefault();

            if (auth == null)
                return new CustomErrorActionResult(Request, "Invalid authorization", ErrorCodes.InvalidAuthorization,
                    HttpStatusCode.Unauthorized);

            var profile = AutoMapper.Mapper.Map<ProfileViewModel>(auth.Profile);
            profile = Encryptor.DecryptProfile(profile);

            var authModel = new AuthorizationViewModel
            {
                GuId = auth.GuId
            };

            profile.Authorization = Encryptor.DecryptAuthorization(authModel);

            var currentYear = DateTime.Now.Year;

            var ui = new UserInfoViewModel
            {
                profile = profile,
                rates = AutoMapper.Mapper.Map<List<RateViewModel>>(RateRepo.Get().Where(x => x.Year == currentYear.ToString()).ToList())
            };

            return Ok(ui);
            
        }

    }
}
