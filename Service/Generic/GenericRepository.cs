using Core.Models;
using Microsoft.EntityFrameworkCore;
using Service.UnitOfWork;
using System.Linq.Expressions;

namespace Service.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public readonly MotorbikeDBContext _context;
        public readonly IUnitOfWork _unitOfWork;
        private DbSet<TEntity> _entities;
        public GenericRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
            _unitOfWork = unitOfWork;
        }
        public virtual async Task Add(TEntity entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Delete(TEntity entity)
        {
            //if(entity is IsDelete)
            //{
            //    ((IsDelete)entity).IsDeleted = true;
            //} else
            //{
            //    _entities.Remove(entity);
            //}
            _entities.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties)
        {
            IQueryable<TEntity>? query = _entities;
            query = expression == null ? query : query.Where(expression);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
