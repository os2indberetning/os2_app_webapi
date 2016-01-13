using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class Profile
    { 
        public Profile()
        {
            this.DriveReports = new HashSet<DriveReport>();
            this.Employments = new HashSet<Employment>();
            this.Tokens = new HashSet<Token>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }

        public string HomeLatitude { get; set; }
        public string HomeLongitude { get; set; }

        public virtual ICollection<Employment> Employments { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<DriveReport> DriveReports { get; set; }

    }
}
