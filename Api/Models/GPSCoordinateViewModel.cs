using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class GPSCoordinateModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsViaPoint { get; set; }
    }
}
