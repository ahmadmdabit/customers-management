using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Bases;

namespace BLL.Bases
{
    public abstract class BaseBusiness<TEntity> : IBusiness<TEntity>
        where TEntity : BaseEntity, IEntity
    {
        protected readonly IRepository<TEntity> _repository;
        //protected readonly ILogger _logger;

        protected BaseBusiness(IRepository<TEntity> repository
            //, ILogger<BaseBusiness<TEntity>> logger
            )
        {
            this._repository = repository;
            //this._logger = logger;
        }

        public virtual async Task<TEntity> Add(TEntity entity, string ip = "")
        {
            return await this._repository.Add(entity).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> Delete(long id, string ip = "")
        {
            return await this._repository.Delete(id).ConfigureAwait(false);
        }

        public async Task<List<TEntity>> DeleteRange(List<TEntity> entities, string ip = "")
        {
            return await this._repository.DeleteRange(entities).ConfigureAwait(false);
        }

        public async Task<TEntity> Get(long id, string ip = "")
        {
            return await this._repository.Get(id).ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetAll(string ip = "")
        {
            return await this._repository.GetAll().ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetBy(string propertyName, string propertyValue, string ip = "")
        {
            var propertyType = typeof(TEntity).GetProperty(propertyName).PropertyType;
            var param = Expression.Parameter(typeof(TEntity));
            var condition =
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(
                        Expression.Property(param, propertyName),
                        Expression.Constant(Convert.ChangeType(propertyValue, propertyType), propertyType)
                    ),
                    param
                );
            //.Compile(); // for LINQ to SQl/Entities skip Compile() call
            return await this._repository.GetBy(condition).ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetBy(string property1Name, string property1Value, string property2Name, string property2Value, string ip = "")
        {
            var property1Type = typeof(TEntity).GetProperty(property1Name).PropertyType;
            var property2Type = typeof(TEntity).GetProperty(property2Name).PropertyType;
            var param = Expression.Parameter(typeof(TEntity));
            var condition =
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.And(
                        Expression.Equal(
                            Expression.Property(param, property1Name),
                            Expression.Constant(Convert.ChangeType(property1Value, property1Type), property1Type)
                        ),
                        Expression.Equal(
                            Expression.Property(param, property2Name),
                            Expression.Constant(Convert.ChangeType(property2Value, property2Type), property2Type)
                        )
                    ),
                    param
                );
            //.Compile(); // for LINQ to SQl/Entities skip Compile() call
            return await this._repository.GetBy(condition).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> Update(long id, TEntity entity, string ip = "")
        {
            return await this._repository.Update(entity).ConfigureAwait(false);
        }
    }
}
