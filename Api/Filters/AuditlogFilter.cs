using Core.ApplicationServices;
using Core.ApplicationServices.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Api.Filters
{
    public class AuditlogFilter : ActionFilterAttribute
    {
        private ILogger _logger;

        public AuditlogFilter()
        {
            _logger = new Logger();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Dictionary<string, string> loggingData = new Dictionary<string, string>();

            loggingData.Add("user", HttpContext.Current.User.Identity.Name);
            loggingData.Add("location", HttpContext.Current.Request.UserHostAddress);
            loggingData.Add("controller", actionContext.RequestContext.RouteData.Values["controller"].ToString());
            //loggingData.Add("action", actionContext.RequestContext.RouteData.Values["action"].ToString());
            loggingData.Add("parameters", JsonConvert.SerializeObject(actionContext.ActionArguments));

            _logger.AuditLog(loggingData);
            base.OnActionExecuting(actionContext);
        }
    }
}