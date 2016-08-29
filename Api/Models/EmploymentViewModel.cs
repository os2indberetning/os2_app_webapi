using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.DomainModel;

namespace Api.Models
{
    public class EmploymentViewModel
    {
        public int Id { get; set; }
        public string EmploymentPosition { get; set; }
        public string ManNr { get; set; }
        public long StartDateTimestamp { get; set; }
        public long EndDateTimestamp { get; set; }

        public OrgUnitViewModel OrgUnit { get; set; }
    }
}