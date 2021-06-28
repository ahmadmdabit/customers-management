using Common.Attributes;
using Common.Extensions;
using DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DAL.Bases
{
    public abstract class BaseEntity : IEntity
    {
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