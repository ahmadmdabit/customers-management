using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Bases;

namespace DAL.Interfaces
{
    /// <summary>
    /// Specifies the contract for Repository
    /// </summary>
    /// <typeparam name="TEntity">A type that inherits from the BaseEntity type and the IEntity interface.</typeparam>
    public interface IRepository<TEntity> where TEntity : BaseEntity, IEntity
    {
        /// <summary>
        /// Adds the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task of TEntity.</returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task of TEntity.</returns>
        Task<TEntity> Delete(string id);

        /// <summary>
        /// Deletes range of TEntityEntities
        /// </summary>
        /// <param name="TEntities"></param>
        /// <returns></returns>
        Task<List<TEntity>> DeleteRange(List<TEntity> TEntities);

        /// <summary>
        /// Froms the sql raw.
        /// </summary>
        /// <param name="sql">The sql.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A Task of List<TEntity>.</returns>
        Task<List<TEntity>> FromSqlRaw(string sql, params object[] parameters);

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task of TEntity.</returns>
        Task<TEntity> Get(string id);

        /// <summary>
        /// Gets the all.
        /// </summary>
        /// <returns>A Task of List<TEntity>.</returns>
        Task<List<TEntity>> GetAll();

        /// <summary>
        /// Gets the by.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A Task of List<TEntity>.</returns>
        Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task of TEntity.</returns>
        Task<TEntity> Update(TEntity entity);

        /// <summary>
        /// Wrap the context operations in one transaction
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        /// Example:
        /// return await this._repository.WrapInTransaction(async () =>
        ///     {
        ///         // operations
        ///     }
        /// ).ConfigureAwait(false) as BusinessEntity<Tables>;
        Task<object> WrapInTransaction(Func<Task<object>> func);
    }
}