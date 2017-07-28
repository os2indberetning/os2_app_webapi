using Core.DomainModel.Model;
using Core.DomainServices;
using log4net;
using System;

namespace Core.ApplicationServices.Logger
{
    public class Logger : ILogger
    {
        private ILog _log { get; set; }
        private IGenericRepository<Auditlog> _auditlogRepo;
        private IUnitOfWork _uow;

        public Logger(IGenericRepository<Auditlog> auditlogRepo, IUnitOfWork uow)
        {
            _log = LogManager.GetLogger("Logger");
            _auditlogRepo = auditlogRepo;
            _uow = uow;
        }

        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Error(string message, Exception e)
        {
            _log.Error(message, e);
        }

        public void AuditLog(string user, string userLocation, string controller, string action, string parameters)
        {
            Auditlog logEntry = new Auditlog
            {
                Date = DateTime.Now.ToString(),
                User = user ?? "not available",
                Location = userLocation ?? "not available",
                Controller = controller ?? "not available",
                Action = action ?? "not available",
                Parameters = parameters ?? "not available"
            };

            _auditlogRepo.Insert(logEntry);
            _uow.Save();
        }

    }
}
