using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DAL.Bases;
using DAL.Interfaces;
using API.Concrets;

namespace API.Interfaces
{
    public interface IController<TEntity>
        where TEntity : BaseEntity, IEntity
    {
        Task<ActionResult<ApiResult<TEntity>>> Delete(long id);

        Task<ActionResult<ApiResult<TEntity>>> Get();

        Task<ActionResult<ApiResult<TEntity>>> Get(long id);

        Task<ActionResult<ApiResult<TEntity>>> GetBy(string propertyName, string propertyValue);

        Task<ActionResult<ApiResult<TEntity>>> Post(TEntity entity);

        Task<ActionResult<ApiResult<TEntity>>> Put(long id, TEntity entity);
    }
}