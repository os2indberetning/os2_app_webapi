using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class DriveReportViewModel
    {
        public string Date { get; set; }
        public string ManualEntryRemark { get; set; }
        public string Purpose { get; set; }
        public int EmploymentId { get; set; }

        public int RateId { get; set; }
        public int ProfileId { get; set; }

        public bool EndsAtHome { get; set; }
        public bool StartsAtHome { get; set; }

        public RouteViewModel route { get; set; }
    }
}
