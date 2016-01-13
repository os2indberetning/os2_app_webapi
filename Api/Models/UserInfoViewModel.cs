using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.DomainModel;

namespace Api.Models
{
    public class UserInfoViewModel
    {
        public ProfileViewModel profile { get; set; }
        public List<RateViewModel> rates { get; set; }
    }
}