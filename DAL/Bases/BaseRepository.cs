using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Attributes;
using DAL.Bases;
using DAL.Interfaces;

namespace DAL.Bases
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : BaseEntity, IEntity
        where TContext : DbContext
    {
        protected readonly TContext _context;

        protected BaseRepository(TContext context)
        {
            this._context = context;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            //entity.CreatedDate = DateTime.Now;
            this._context.Set<TEntity>().Add(entity);
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<TEntity> Delete(string id)
        {
            var entity = await this._context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                return entity;
            }

            this._context.Set<TEntity>().Remove(entity);
            await this._context.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        public async Task<List<TEntity>> DeleteRange(List<TEntity> entities)
        {
            var entitiesFound = await this._context.Set<TEntity>().AsNoTracking().AsAsyncEnumerable()
                .Where(e => Array.IndexOf(entities.Select(t => t.GetKeyProperty()).ToArray(), e.GetKeyProperty().ToString()) > -1)
                .ToListAsync().ConfigureAwait(false);
            if (entitiesFound == null || entitiesFound?.Count() == 0)
            {
                return entitiesFound;
            }

            this._context.Set<TEntity>().RemoveRange(entitiesFound);
            await this._context.SaveChangesAsync().ConfigureAwait(false);

            return entitiesFound;
        }

        public async Task<List<TEntity>> FromSqlRaw(string sql, params object[] parameters)
        {
            return await this._context.Set<TEntity>().FromSqlRaw(sql, parameters).ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> Get(string id)
        {
            IQueryable<TEntity> dbSet = IncludeBuild();
            return await dbSet.AsNoTracking().AsAsyncEnumerable()
                .FirstOrDefaultAsync(x => x.GetKeyProperty().ToString() == id).ConfigureAwait(false);

            //var entityFound = await this._context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            //this._context.Entry(entityFound).State = EntityState.Detached;
            //return entityFound;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await IncludeBuild().AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetBy(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await IncludeBuild().Where(expression).AsNoTracking().ToListAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return new List<TEntity>();
            }
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var entityFound = await this._context.Set<TEntity>().FindAsync(entity.GetKeyProperty().ToString()).ConfigureAwait(false);
            if (entityFound == null)
            {
                return null;
            }
            this._context.Entry(entityFound).State = EntityState.Detached;

            //this._context.Entry(entity).State = EntityState.Modified; // Can't be used with .Update(entity) method
            this._context.Set<TEntity>().Update(entity); // Using this method for update the entity with related entities
            await this._context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<object> WrapInTransaction(Func<Task<object>> func)
        {
            using var transaction = this._context.Database.BeginTransaction();
            try
            {
                var result = await func().ConfigureAwait(false);

                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        protected IQueryable<TEntity> IncludeBuild()
        {
            IQueryable<TEntity> queryable = this._context.Set<TEntity>();
            if (typeof(TEntity).IsDefined(typeof(IncludeAttribute), false))
            {
                if (Attribute.GetCustomAttribute(typeof(TEntity), typeof(IncludeAttribute)) is IncludeAttribute attribute)
                {
                    for (int ii = 0; ii < attribute.NavigationPropertyPath.Length; ii++)
                    {
                        queryable = queryable.Include(attribute.NavigationPropertyPath[ii]);
                    }
                }
            }
            return queryable;
        }
    }
}