﻿using System;
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
    public class ReportController : BaseController
    {

        private IUnitOfWork Uow { get; set; }
        private IGenericRepository<DriveReport> DriveReportRepo { get; set; }
        private IGenericRepository<UserAuth> AuthRepo { get; set; }
       

        public ReportController(IUnitOfWork uow, IGenericRepository<DriveReport> driveReportRepo, IGenericRepository<UserAuth> authRepo, ILogger logger) : base(logger)
        {
            Uow = uow;
            DriveReportRepo = driveReportRepo;
            AuthRepo = authRepo;
        }

        public class DriveObject
        {
            public DriveReportViewModel DriveReport { get; set; }
            public AuthorizationViewModel Authorization { get; set; }
        }

        // POST /report
        public IHttpActionResult Post(DriveObject driveObject)
        {
            var encryptedGuId = Encryptor.EncryptAuthorization(driveObject.Authorization).GuId;
            var auth = AuthRepo.Get(t => t.GuId == encryptedGuId).FirstOrDefault();
            var DuplicateReportCheck = DriveReportRepo.Get(t => t.Uuid == driveObject.DriveReport.Uuid).Any();

            if (auth == null)
            {
                _logger.Debug($"{GetType().Name}, Post(), Invalid authorization for guid: {encryptedGuId}");
                return new CustomErrorActionResult(Request, "Invalid authorization", ErrorCodes.InvalidAuthorization,
                    HttpStatusCode.Unauthorized);
            }
            if(auth.ProfileId != driveObject.DriveReport.ProfileId)
            {
                _logger.Debug($"{GetType().Name}, Post(), User and drive report user do not match for profileId: {auth.ProfileId}");
                return new CustomErrorActionResult(Request, "User and drive report user do not match", ErrorCodes.ReportAndUserDoNotMatch,
                     HttpStatusCode.Unauthorized);
            }
            if (DuplicateReportCheck)
            {
                _logger.Debug($"{GetType().Name}, Post(), Report rejected, duplicate found. Drivereport uuid: {driveObject.DriveReport.Uuid}, profileId: {auth.ProfileId}");
                return new CustomErrorActionResult(Request, "Report rejected, duplicate found", ErrorCodes.DuplicateReportFound, HttpStatusCode.OK);
            }

            try
            {
                driveObject.DriveReport = Encryptor.EncryptDriveReport(driveObject.DriveReport);

                var model = AutoMapper.Mapper.Map<DriveReport>(driveObject.DriveReport);

                try
                {
                    Auditlog(auth.UserName, System.Reflection.MethodBase.GetCurrentMethod().Name, driveObject.DriveReport);
                }
                catch (Exception e)
                {
                    _logger.Error($"{GetType().Name}, Post(), Auditlog failed", e);
                    return InternalServerError();
                }

                DriveReportRepo.Insert(model);
                Uow.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error($"{GetType().Name}, Post(), Could not save drivereport, uuid: {driveObject.DriveReport.Uuid}, profileId: {auth.ProfileId}", ex);
                return new CustomErrorActionResult(Request, "Could not save drivereport", ErrorCodes.SaveError, HttpStatusCode.BadRequest);
            }
        }


    }
}
