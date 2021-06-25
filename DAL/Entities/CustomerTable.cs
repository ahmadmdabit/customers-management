using DAL.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Entities
{
    public partial class CustomerTable : BaseEntity
    {
        [Key]
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
    }
}
