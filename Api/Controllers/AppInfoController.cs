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
using Newtonsoft.Json;
using Core.ApplicationServices.Logger;

namespace Api.Controllers
{   
    public class AppInfoController : ApiController
    {
        //api/appinfo
        public object Get()
        {
            ILogger _logger = new Logger();
            _logger.Log("api/appinfo initial", "api", 3);

            var allText = System.IO.File.ReadAllText(
               System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "appInfo.json");
            
            var jsonObject = JsonConvert.DeserializeObject(allText);
            _logger.Log("api/appinfo before. json: " + jsonObject, "api", 3);
            return jsonObject;
        }
    }
}