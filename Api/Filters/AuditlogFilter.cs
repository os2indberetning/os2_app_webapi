using Core.ApplicationServices;
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
        private CustomHttpClient _client;

        public AuditlogFilter()
        {
            _client = new CustomHttpClient();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Dictionary<string, string> loggingData = new Dictionary<string, string>();

            loggingData.Add("user", HttpContext.Current.User.Identity.Name);
            loggingData.Add("location", HttpContext.Current.Request.UserHostAddress);
            loggingData.Add("controller", actionContext.RequestContext.RouteData.Values["controller"].ToString());
            //loggingData.Add("action", actionContext.RequestContext.RouteData.Values["action"].ToString());
            loggingData.Add("parameters", JsonConvert.SerializeObject(actionContext.ActionArguments));

            _client.PostAuditLog(loggingData);
            //_logger.AuditLog($"{DateTime.Now.Date} - {user} - {location} - {controller} - {action} - {parameters}");
            base.OnActionExecuting(actionContext);
        }
    }
}