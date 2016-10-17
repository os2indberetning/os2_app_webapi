using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class Rate
    {
        public Rate()
        {
            this.DriveReports = new HashSet<DriveReport>();
        }

        public int Id { get; set; }
        public String Description { get; set; }
        public String Year { get; set; }
        public bool isActive { get; set; }

        public virtual ICollection<DriveReport> DriveReports { get; set; }
    }
}
