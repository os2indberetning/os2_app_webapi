using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class DriveReport
    {
        public int Id { get; set; }
        
        public string Date { get; set; }
        public string ManualEntryRemark { get; set; }
        public string Purpose { get; set; }
        public bool FourKmRule { get; set; }
        public bool StartsAtHome { get; set; }
        public bool EndsAtHome { get; set; }
        public long? SyncedAt { get; set; }
        public int EmploymentId { get; set; }

        public int ProfileId { get; set; }
        public int RateId { get; set; }
        //public int RouteId { get; set; }

        public string Uuid { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Route Route { get; set; }
        public virtual Rate Rate { get; set; }
    }
}
