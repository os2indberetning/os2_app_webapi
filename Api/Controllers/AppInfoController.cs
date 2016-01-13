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

namespace Api.Controllers
{   
    public class AppInfoController : ApiController
    {
        public object Get()
        {
            var allText = System.IO.File.ReadAllText(
               System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "appInfo.json");

            var jsonObject = JsonConvert.DeserializeObject(allText);
            return jsonObject;
        }
    }
}