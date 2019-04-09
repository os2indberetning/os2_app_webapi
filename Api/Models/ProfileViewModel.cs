using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.DomainModel;

namespace Api.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string HomeLatitude { get; set; }
        public string HomeLongitude { get; set; }

        public AuthorizationViewModel Authorization { get; set; }

        public virtual ICollection<EmploymentViewModel> Employments { get; set; }
    }
}