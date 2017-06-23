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
using Api.Filters;

namespace Api.Controllers
{   
    public class AppInfoController : BaseController
    {
        public AppInfoController(ILogger logger) : base(logger)
        {
        }

        //api/appinfo
        public object Get()
        {
            var allText = System.IO.File.ReadAllText(
               System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "appInfo.json");
            
            var jsonObject = JsonConvert.DeserializeObject(allText);
            return jsonObject;
        }
    }
}