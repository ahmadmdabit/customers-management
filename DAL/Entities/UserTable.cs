using DAL.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Entities
{
    public partial class UserTable : BaseEntity
    {
        [Key]
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string UserFilter { get; set; }
    }
}
