using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Encryption;
using Core.DomainModel;
using Core.DomainServices;
using Api.Models;
using Core.ApplicationServices.Logger;

namespace Api.Controllers
{
    public class SyncWithTokenController : BaseController
    {
        private IUnitOfWork _uow { get; set; }
        private IGenericRepository<Core.DomainModel.Profile> _profileRepo { get; set; }
        private IGenericRepository<Token> _tokenRepo { get; set; }
        private IGenericRepository<Rate> _rateRepo { get; set; }
        

        public SyncWithTokenController(IUnitOfWork uow, IGenericRepository<Rate> rateRepo, IGenericRepository<Core.DomainModel.Profile> profileRepo, IGenericRepository<Token> tokenRepo, ILogger logger) : base(logger)
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

            Auditlog(null, System.Reflection.MethodBase.GetCurrentMethod().Name, obj);

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
                _logger.Debug($"{GetType().Name}, Post(), Token already used, guid: {obj.GuId}");
                return new CustomErrorActionResult(Request, "Token allready used", ErrorCodes.TokenAllreadyActivated, HttpStatusCode.BadRequest);
                
            }
            else
            {
                _logger.Debug($"{GetType().Name}, Post(), Token not found, guid: {obj.GuId}");
                return new CustomErrorActionResult(Request,"Token not found", ErrorCodes.InvalidAuthorization, HttpStatusCode.Unauthorized);
            }
        }
    }
}
