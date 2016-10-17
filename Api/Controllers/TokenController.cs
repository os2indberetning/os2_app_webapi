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

    public class TokenController : ApiController
    {
        public const string PasswordString = "";
        private IUnitOfWork _uow { get; set; }
        private IGenericRepository<Core.DomainModel.Profile> _profileRepo { get; set; }
        private IGenericRepository<Token> _tokenRepo { get; set; }

        public TokenController(IUnitOfWork uow, IGenericRepository<Core.DomainModel.Profile> profileRepo, IGenericRepository<Token> tokenRepo)
        {
            _uow = uow;
            _tokenRepo = tokenRepo;
            _profileRepo = profileRepo;
        }

        public class tmpCreateToken
        {
            public string Token { get; set; }
            public string Password { get; set; }
            public string GuId { get; set; }
            public int ProfileId { get; set; }
        }

        public class tmpDeleteToken
        {
            public string Password { get; set; }
            public string GuId { get; set; }
        }

        // POST new token from main server
        public IHttpActionResult Post(tmpCreateToken obj)
        {
            ILogger _logger = new Logger();
            _logger.Log("Post TokenController. Object tmpCreateToken initial: " + obj.Token, "api", 3);
            if (obj.Password == PasswordString)
            {
                Core.DomainModel.Profile profile = _profileRepo.Get(x => x.Id == obj.ProfileId).FirstOrDefault();

                if (profile != null)
                {
                    //Create viewmodel and encrypt it
                    TokenViewModel token = new TokenViewModel();
                    token.Status = 2;
                    token.GuId = obj.GuId;
                    token.TokenString = obj.Token;
                    token = Encryptor.EncryptToken(token);

                    bool anyToken = _tokenRepo.Get(x => x.GuId == token.GuId).Any();

                    if (!anyToken)
                    {
                        //Map to datamodel, and add profileid
                        Token mToken = AutoMapper.Mapper.Map<Token>(token);
                        mToken.ProfileId = obj.ProfileId;

                        try
                        {
                            _tokenRepo.Insert(mToken);
                            _uow.Save();
                            _logger.Log("Post TokenController. Before OK: ", "api", 3);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            _logger.Log("Post TokenController. Save error. Exception: " + ex.Message, "api", 3);
                            return new CustomErrorActionResult(Request, "Save Error", ErrorCodes.SaveError,HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        _logger.Log("Post TokenController. Error: Token already exists ", "api", 3);
                        return new CustomErrorActionResult(Request, "Token allready exists", ErrorCodes.TokenAllreadyExists, HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    _logger.Log("Post TokenController. Error: User not found", "api", 3);
                    return new CustomErrorActionResult(Request, "User not found", ErrorCodes.UserNotFound, HttpStatusCode.BadRequest);
                }
            }
            else
            {
                _logger.Log("Post TokenController. Error: Wrong password. Password: " + obj.Password, "api", 3);
                return new CustomErrorActionResult(Request, "Wrong Password", ErrorCodes.BadPassword, HttpStatusCode.Unauthorized);
            }
        }

        // Delete token from main server
        public IHttpActionResult Delete(tmpDeleteToken obj)
        {
            ILogger _logger = new Logger();
            _logger.Log("Delete TokenController. Object tmpDeleteToken initial: " + obj, "api", 3);
            if (obj.Password == PasswordString)
            {
                TokenViewModel tvm = new TokenViewModel();
                tvm.GuId = obj.GuId;
                tvm = Encryptor.EncryptToken(tvm);

                Token token = _tokenRepo.Get(x => x.GuId == tvm.GuId && x.Status > 0).FirstOrDefault();

                if (token != null)
                {
                    token.Status = 0;

                    try
                    {
                        _uow.Save();
                        _logger.Log("Delete TokenController. Before OK: ", "api", 3);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        _logger.Log("Delete TokenController. Save error. Exception: " + ex.Message, "api", 3);
                        return new CustomErrorActionResult(Request, "Save Error", ErrorCodes.SaveError, HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    _logger.Log("Delete TokenController. Error: Token not found ", "api", 3);
                    return new CustomErrorActionResult(Request, "Token not found", ErrorCodes.InvalidAuthorization, HttpStatusCode.BadRequest);
                }
            }
            else
            {
                _logger.Log("Delete TokenController. Error: Wrong password. Password: " + obj.Password, "api", 3);
                return new CustomErrorActionResult(Request, "Wrong Password", ErrorCodes.BadPassword, HttpStatusCode.Unauthorized);
            }
        }
    }
}
