using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public class Route
    {
        public Route()
        {
            this.GPSCoordinates = new HashSet<GPSCoordinate>();
        }

        public int Id { get; set; }
        public double TotalDistance { get; set; }

        public virtual DriveReport DriveReport { get; set; }
        public virtual ICollection<GPSCoordinate> GPSCoordinates { get; set; }
    }
}
