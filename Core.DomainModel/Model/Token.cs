using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class Token
    {
        public int Id { get; set; }

        public String GuId { get; set; }
        public String TokenString { get; set; }
        public int Status { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}