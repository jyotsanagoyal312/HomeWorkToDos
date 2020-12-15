using System;
using System.Collections.Generic;

namespace HomeWorkToDos.Util.Models
{
    /// <summary>
    /// PagedList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.List{T}" />
    public class PagedList<T> : List<T>
	{
        /// <summary>
        /// Gets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage { get; private set; }
        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        public int TotalPages { get; private set; }
        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; private set; }
        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has previous.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has previous; otherwise, <c>false</c>.
        /// </value>
        public bool HasPrevious => CurrentPage > 1;
        /// <summary>
        /// Gets a value indicating whether this instance has next.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has next; otherwise, <c>false</c>.
        /// </value>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="count">The count.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			AddRange(items);
		}

	}
}
