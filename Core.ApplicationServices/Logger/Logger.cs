using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApplicationServices.Logger
{
    public class Logger : ILogger
    {
        private ILog log;
        private CustomHttpClient _client;

        public Logger()
        {
            log = LogManager.GetLogger("Logger");
            _client = new CustomHttpClient();
        }

        public void Log(string msg, string fileName, int level)
        {
            var message = "[Niveau " + level + "] - " + msg;
            switch (level)
            {
                case 1: log.Error(message); break;
                case 2: log.Warn(message); break;
                default:
                    log.Info(message); break;
            }
        }

        public void AuditLog(Dictionary<string, string> data)
        {
            _client.PostAuditLog(data);
        }
    }
}
