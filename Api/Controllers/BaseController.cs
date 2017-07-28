using Core.ApplicationServices.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    public class BaseController : ApiController
    {
        public ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }

        public void Auditlog(string user, string method, object arguments)
        {
            var location = HttpContext.Current.Request.UserHostAddress;
            var controller = GetType().ToString();

            _logger.AuditLog(user, location, controller, method, JsonConvert.SerializeObject(arguments));
        }
    }
}
