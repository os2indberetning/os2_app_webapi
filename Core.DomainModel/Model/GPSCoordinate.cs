using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class GPSCoordinate
    {
        public int Id { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsViaPoint { get; set; }

        public int RouteId { get; set; }
        public virtual Route Route { get; set; }
    }
}
