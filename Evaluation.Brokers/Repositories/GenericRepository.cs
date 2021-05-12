using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Brokers.Context;
using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Brokers.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        private readonly EvaluationContext context;

        public GenericRepository(EvaluationContext context)
        {
            this.context = context ;
        }

        public async ValueTask<TEntity> GetAsync(int id)
        {
            return await this.context.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.context.Set<TEntity>();
        }

        public async ValueTask<List<TEntity>> GetAllAsync()
        {
            return await this.context.Set<TEntity>().ToListAsync();
        }

        public async ValueTask<List<TEntity>> FindAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return await this.context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async ValueTask<TEntity> AddAsync(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public TEntity Add(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
            return entity;
        }


        public async ValueTask<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            this.context.Set<TEntity>().AddRange(entities);
            return await this.context.SaveChangesAsync();
        }
        public async ValueTask<TEntity> EditAsync(TEntity entity)
        {
            this.context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.context.SaveChangesAsync();
            return entity;
        }

        public TEntity Edit(TEntity entity)
        {
            this.context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return entity;
        }

        public async ValueTask RemoveAsync(TEntity entity)
        {
            this.context.Set<TEntity>().Remove(entity);
            await this.context.SaveChangesAsync();
        }

        public async ValueTask RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            this.context.Set<TEntity>().RemoveRange(entities);
            await this.context.SaveChangesAsync();
        }

        public async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.context.Set<TEntity>().AnyAsync(predicate);
        }


        public async ValueTask<int> SaveChangeAsync()
        {
            return await this.context.SaveChangesAsync();
        }
    }
}
