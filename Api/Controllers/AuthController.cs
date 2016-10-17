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
using Core.ApplicationServices.Logger;

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
            ILogger _logger = new Logger();
            try
            {
                _logger.Log("Post api/auth. Object AuthRequestViewModel initial: pw" + obj.Password + "user" + obj.UserName, "api", 3);
                var auth = Encryptor.EncryptAuthRequest(obj);

                var user = AuthRepo.Get(x => x.UserName == auth.UserName).FirstOrDefault();

                if (user == null || user.Password != GetHash(user.Salt, obj.Password) || user.Profile.IsActive == false)
                {
                    _logger.Log("Post api/auth. Username or password is incorrect: User: " + user, "api", 3);
                    return new CustomErrorActionResult(Request, "Username or password is incorrect", ErrorCodes.IncorrectUserNameOrPassword, HttpStatusCode.Unauthorized);
                }
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
                    rates = AutoMapper.Mapper.Map<List<RateViewModel>>(RateRepo.Get().Where(x => x.Year == currentYear.ToString() && x.isActive).ToList())
                };
                _logger.Log("Post api/auth. Before Ok. profile: " + ui.profile + " rates: " + ui.rates, "api", 3);
                return Ok(ui);
            }
            catch (Exception e)
            {
                _logger.Log("Post api/auth. Exception message: " + e.Message, "api", 3);
                _logger.Log("Post api/auth. Exception stack trace: " + e.StackTrace, "api", 3);
                _logger.Log("Post api/auth. InnerException stack trace: " + e.InnerException.Message, "api", 3);
                _logger.Log("Post api/auth. InnerException stack trace: " + e.InnerException.StackTrace, "api", 3);
                throw;
            }
        }

    }
}
