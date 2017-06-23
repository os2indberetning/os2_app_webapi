using Core.ApplicationServices;
using Core.ApplicationServices.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Filters
{
    public class AuditlogFilter : IActionFilter
    {
        private ILogger _logger;

        public AuditlogFilter()
        {
        }

        public bool AllowMultiple
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(HttpActionContext actionContext)
        {
            var parameter = actionContext.ActionArguments;
            try
            {
                string user = "testuser", location = "testlocation", controller = "testcontroller", action = "testaction", jsonParameters = "testjsonparameters";
                //var user = HttpContext.Current.User.Identity.Name;
                //var location = HttpContext.Current.Request.UserHostAddress;

                //object controller, action = null;
                //actionContext.RequestContext.RouteData.Values.TryGetValue("controller", out controller);
                //actionContext.RequestContext.RouteData.Values.TryGetValue("action", out action);

                //string jsonParameters = JsonConvert.SerializeObject(actionContext.ActionArguments);
                _logger.AuditLog(user, location, controller?.ToString(), action?.ToString(), jsonParameters);
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "Auditlogging failed");
            }
        }
    }
}