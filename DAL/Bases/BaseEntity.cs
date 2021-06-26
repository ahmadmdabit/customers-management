using Common.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Attributes;
using DAL.Interfaces;

namespace DAL.Bases
{
    public abstract class BaseEntity : IEntity
    {
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }

        public object GetKeyProperty()
        {
            return this.PropertyFindValueByAttribute(typeof(KeyAttribute));
        }

        public object GetParentKeyProperty()
        {
            return this.PropertyFindValueByAttribute(typeof(ParentKeyAttribute));
        }
    }
}