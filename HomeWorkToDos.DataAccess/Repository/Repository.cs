using HomeWorkToDos.DataAccess.Contract;
using HomeWorkToDos.DataAccess.Data;
using HomeWorkToDos.Util.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeWorkToDos.DataAccess.Repository
{
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="HomeWorkToDos.DataAccess.Contract.IRepository{T}" />
    public class Repository<T> : IRepository<T>
       where T : class
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly DbContext _context;
        /// <summary>
        /// The dbset
        /// </summary>
        private readonly DbSet<T> _dbset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(HomeworktodosContext context)
        {
            if (context != null)
            {
                _context = context;
                _dbset = context.Set<T>();
            }
        }

        /// <summary>
        /// Gets the current database context.
        /// </summary>
        /// <value>
        /// The current database context.
        /// </value>
        public DbContext CurrentDbContext
        {
            get { return _context; }
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual T Add(T entity)
        {
            return _dbset.Add(entity).Entity;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void AddRange(IList<T> entities)
        {
            _dbset.AddRange(entities);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(object id)
        {
            T existing = _dbset.Find(id);
            _dbset.Remove(existing);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Updates the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void UpdateRange(IList<T> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                _context.Entry(entity).State = EntityState.Modified;
            });
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual T FindById(object id)
        {
            return _dbset.Find(id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return _dbset;
        }

        /// <summary>
        /// Gets all by database set.
        /// </summary>
        /// <returns></returns>
        public virtual DbSet<T> GetAllByDbSet()
        {
            return _dbset;
        }

        /// <summary>
        /// Filters the list.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        public PagedList<T> FilterList(PaginationParameters paginationParams = null, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<T> query = _dbset.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (paginationParams != null)
                return PagedList<T>.ToPagedList(query, paginationParams.PageNumber, paginationParams.PageSize);
            else
                return PagedList<T>.ToPagedList(query, 1, Int32.MaxValue);
        }

        /// <summary>
        /// Filters the list.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        public IQueryable<T> FilterList(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<T> query = _dbset.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties?.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }
        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}