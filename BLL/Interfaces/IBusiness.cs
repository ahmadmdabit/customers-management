using DAL.Bases;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBusiness<TEntity>
        where TEntity : BaseEntity, IEntity
    {
        Task<TEntity> Add(TEntity entity, string ip = "");

        Task<TEntity> Delete(string id, string ip = "");

        Task<List<TEntity>> DeleteRange(List<TEntity> entities, string ip = "");

        Task<TEntity> Get(string id, string ip = "");

        Task<List<TEntity>> GetAll(string ip = "");

        Task<List<TEntity>> GetBy(string propertyName, string propertyValue, string ip = "");

        Task<List<TEntity>> GetBy(string property1Name, string property1Value, string property2Name, string property2Value, string ip = "");

        Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> condition, string ip = "");

        Task<TEntity> Update(TEntity entity, string ip = "");
    }
}