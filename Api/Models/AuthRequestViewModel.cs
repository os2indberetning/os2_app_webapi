using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class AuthRequestViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}