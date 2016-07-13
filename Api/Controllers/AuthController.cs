﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Api.Encryption;
using Api.Models;
using Core.DomainModel;
using Core.DomainModel.Model;
using Core.DomainServices;

namespace Api.Controllers
{
    public class AuthController : ApiController
    {
        private IGenericRepository<Rate> RateRepo { get; set; }
        private IGenericRepository<UserAuth> AuthRepo { get; set; }

        public AuthController(IGenericRepository<Rate> rateRepo, IGenericRepository<UserAuth> authRepo)
        {
            RateRepo = rateRepo;
            AuthRepo = authRepo;
        }

        private static string GetHash(string salt, string password)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(salt + password));

                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        // POST api/auth
        public IHttpActionResult Post(AuthRequestViewModel obj)
        {
            
            var auth = Encryptor.EncryptAuthRequest(obj);

            var user = AuthRepo.Get(x => x.UserName == auth.UserName).FirstOrDefault();

            if(user == null || user.Password != GetHash(user.Salt, obj.Password) || user.Profile.IsActive == false)
                return new CustomErrorActionResult(Request, "Username or password is incorrect", ErrorCodes.IncorrectUserNameOrPassword, HttpStatusCode.Unauthorized);

            var profile = AutoMapper.Mapper.Map<ProfileViewModel>(user.Profile);

            profile = Encryptor.DecryptProfile(profile);
            var currentTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            profile.Employments = profile.Employments.AsQueryable().Where(x => x.StartDateTimestamp < currentTimestamp && (x.EndDateTimestamp > currentTimestamp || x.EndDateTimestamp == 0)).ToList();

            var authModel = new AuthorizationViewModel
            {
                GuId = user.GuId
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
