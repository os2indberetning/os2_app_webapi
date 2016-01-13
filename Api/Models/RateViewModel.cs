using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.DomainModel;

namespace Api.Models
{
    public class RateViewModel
    {
        public int Id { get; set; }

        public String Description { get; set; }
        public String Year { get; set; }
    }
}