﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Bases;
using DAL.Interfaces;

namespace BLL.Interfaces
{
    public interface IBusiness<TEntity>
        where TEntity : BaseEntity, IEntity
    {
        Task<TEntity> Add(TEntity entity, string ip = "");

        Task<TEntity> Delete(long id, string ip = "");

        Task<List<TEntity>> DeleteRange(List<TEntity> entities, string ip = "");

        Task<TEntity> Get(long id, string ip = "");

        Task<List<TEntity>> GetAll(string ip = "");

        Task<List<TEntity>> GetBy(string propertyName, string propertyValue, string ip = "");

        Task<List<TEntity>> GetBy(string property1Name, string property1Value, string property2Name, string property2Value, string ip = "");

        Task<TEntity> Update(long id, TEntity entity, string ip = "");
    }
}