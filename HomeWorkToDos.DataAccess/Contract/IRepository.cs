using HomeWorkToDos.Util.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeWorkToDos.DataAccess.Contract
{
    /// <summary>
    /// IRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets the current database context.
        /// </summary>
        /// <value>
        /// The current database context.
        /// </value>
        DbContext CurrentDbContext { get; }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T Add(T entity);
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void AddRange(IList<T> entities);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(object id);
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);
        /// <summary>
        /// Updates the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void UpdateRange(IList<T> entities);
        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        T FindById(object Id);
        /// <summary>
        /// Filters the list.
        /// </summary>
        /// <param name="paginationParams">The pagination parameters.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        PagedList<T> FilterList(PaginationParameters paginationParams, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");
        /// <summary>
        /// Filters the list.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        IQueryable<T> FilterList(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");
        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        int Save();
    }
}
