using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
 
namespace Api.Controllers
{

    public enum ErrorCodes
    {
        UnknownError = 600,
        InvalidAuthorization = 610,
        SaveError = 640,
        BadPassword = 650,
        UserNotFound = 660,
        UserNotActive = 665,
        ReportAndUserDoNotMatch = 670,
        IncorrectUserNameOrPassword = 680,
        DuplicateReportFound = 690,
    }

    public class ErrorObject
    {
        public String ErrorMessage { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }

    public class CustomErrorActionResult : IHttpActionResult
    {
        public string Message { get; private set; }
        public HttpRequestMessage Request { get; private set; }
        public HttpStatusCode Status;
        public ErrorCodes ErrorCode;

        public CustomErrorActionResult(HttpRequestMessage request, string message, ErrorCodes errorCode, HttpStatusCode status)
        {
            this.Request = request;
            this.Message = message;
            this.ErrorCode = errorCode;
            this.Status = status;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ExecuteResult());
        }

        public HttpResponseMessage ExecuteResult()
        {
            ErrorObject obj = new ErrorObject() {ErrorCode = this.ErrorCode, ErrorMessage = this.Message};
            return Request.CreateResponse(this.Status, obj);
        }
    }
}