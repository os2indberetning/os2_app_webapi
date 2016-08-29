using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Encryption;
using Api.Models;
using Core.DomainModel;
using Core.DomainModel.Model;
using Core.DomainServices;
using Core.ApplicationServices.Logger;

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
            ILogger _logger = new Logger();
            _logger.Log("Post api/userinfo. Object AuthorizationViewModel GUID initial: " + obj.GuId, "api", 3);
            var encryptedGuid = Encryptor.EncryptAuthorization(obj).GuId;

            var auth = AuthRepo.Get(t => t.GuId == encryptedGuid).FirstOrDefault();

            if (auth == null) {
                _logger.Log("Post api/userinfo. Error: Invalid authorization ", "api", 3);
                return new CustomErrorActionResult(Request, "Invalid authorization", ErrorCodes.InvalidAuthorization,
                    HttpStatusCode.Unauthorized);
            }
            var profile = AutoMapper.Mapper.Map<ProfileViewModel>(auth.Profile);
            profile = Encryptor.DecryptProfile(profile);
            var currentTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            profile.Employments = profile.Employments.AsQueryable().Where(x => x.StartDateTimestamp < currentTimestamp && (x.EndDateTimestamp > currentTimestamp || x.EndDateTimestamp == 0)).ToList();

            var authModel = new AuthorizationViewModel
            {
                GuId = auth.GuId
            };

            profile.Authorization = Encryptor.DecryptAuthorization(authModel);

            var currentYear = DateTime.Now.Year;

            var ui = new UserInfoViewModel
            {
                profile = profile,
                rates = AutoMapper.Mapper.Map<List<RateViewModel>>(RateRepo.Get().Where(x => x.Year == currentYear.ToString() && x.isActive).ToList())
            };

            _logger.Log("Post api/userinfo. Before OK: ", "api", 3);
            return Ok(ui);
            
        }

    }
}
