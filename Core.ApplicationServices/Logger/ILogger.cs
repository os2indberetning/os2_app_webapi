using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApplicationServices.Logger
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(string message, Exception e);
        void AuditLog(string user, string userLocation, string controller, string action, string parameters);
    }
}
