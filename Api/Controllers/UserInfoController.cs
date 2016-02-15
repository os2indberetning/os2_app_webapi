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
            var auth = AuthRepo.Get(t => t.GuId == Encryptor.EncryptAuthorization(obj).GuId).FirstOrDefault();

            if (auth == null)
                return new CustomErrorActionResult(Request, "Invalid authorization", ErrorCodes.TokenNotFound,
                    HttpStatusCode.Unauthorized);

            var profile = AutoMapper.Mapper.Map<ProfileViewModel>(auth.Profile);
            profile = Encryptor.DecryptProfile(profile);

            var ui = new UserInfoViewModel
            {
                profile = profile,
                rates = AutoMapper.Mapper.Map<List<RateViewModel>>(RateRepo.Get().ToList())
            };

            return Ok(ui);
            
        }

    }
}
