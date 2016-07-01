using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel.Model
{
    public class OrgUnit
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public bool FourKmRuleAllowed { get; set; }
    }
}
