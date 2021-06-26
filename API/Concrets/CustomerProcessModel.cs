using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Concrets
{
    public class CustomerProcessModel
    {
        public string UserCode { get; set; }
        public CustomerTable Customer { get; set; }
        public int Type { get; set; }
        public string Filter { get; set; }
    }
}
