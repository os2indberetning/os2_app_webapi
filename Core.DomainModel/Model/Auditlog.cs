using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel.Model
{
    public class Auditlog
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string User { get; set; }
        public string Location { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Parameters { get; set; }
    }
}
