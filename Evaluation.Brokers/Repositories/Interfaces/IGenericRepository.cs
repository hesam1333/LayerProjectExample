using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Brokers.Context;
using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluation.Brokers.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        //Get
        ValueTask<TEntity>  GetAsync(int id);
        IQueryable<TEntity> GetAll();
        ValueTask<List<TEntity>> GetAllAsync();
        ValueTask<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        //Add
        ValueTask<TEntity> AddAsync(TEntity entity);
        TEntity Add(TEntity entity);
        ValueTask<int> AddRangeAsync(IEnumerable<TEntity> entities);


        //Edit
        ValueTask<TEntity> EditAsync(TEntity entity);
        TEntity Edit(TEntity entity);


        //any
        ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);


        //Remove
        ValueTask RemoveAsync(TEntity entity);
        ValueTask RemoveRangeAsync(IEnumerable<TEntity> entities);

        ValueTask<int> SaveChangeAsync();

    }
}
