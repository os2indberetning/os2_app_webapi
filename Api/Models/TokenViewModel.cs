using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.DomainModel;

namespace Api.Models
{
    public class TokenViewModel
    {
        public String GuId { get; set; }
        public String TokenString { get; set; }
        public int Status { get; set; }
    }
}