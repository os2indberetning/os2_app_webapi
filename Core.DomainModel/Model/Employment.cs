using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class Employment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string EmploymentPosition { get; set; }
        public string ManNr { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
        public long StartDateTimestamp { get; set; }
        public long EndDateTimestamp { get; set; }
    }
}
