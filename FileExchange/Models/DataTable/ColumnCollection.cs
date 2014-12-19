using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExchange.Models.DataTable
{
    public class ColumnCollection : IEnumerable<Column>
    {
        /// <summary>
        /// For internal use only.
        /// Stores data.
        /// </summary>
        private IReadOnlyList<Column> Data;
        /// <summary>
        /// Created a new ReadOnlyColumnCollection with predefined data.
        /// </summary>
        /// <param name="columns">The column collection from DataTables.</param>
        public ColumnCollection(IEnumerable<Column> columns)
        {
            if (columns == null) throw new ArgumentNullException("The provided column collection cannot be null", "columns");
            Data = columns.ToList().AsReadOnly();
        }
        /// <summary>
        /// Get sorted columns on client-side already on the same order as the client requested.
        /// The method checks if the column is bound and if it's ordered on client-side.
        /// </summary>
        /// <returns>The ordered enumeration of sorted columns.</returns>
        public IOrderedEnumerable<Column> GetSortedColumns()
        {
            return Data
                .Where(_column => !String.IsNullOrWhiteSpace(_column.Data) && _column.IsOrdered)
                .OrderBy(_c => _c.OrderNumber);
        }
        /// <summary>
        /// Get filtered columns on client-side.
        /// The method checks if the column is bound and if the search has a value.
        /// </summary>
        /// <returns>The enumeration of filtered columns.</returns>
        public IEnumerable<Column> GetFilteredColumns()
        {
            return Data
                .Where(_column => !String.IsNullOrWhiteSpace(_column.Data) && _column.Searchable && !String.IsNullOrWhiteSpace(_column.Search.Value));
        }
        /// <summary>
        /// Returns the enumerable element as defined on IEnumerable.
        /// </summary>
        /// <returns>The enumerable elemento to iterate through data.</returns>
        public IEnumerator<Column> GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        /// <summary>
        /// Returns the enumerable element as defined on IEnumerable.
        /// </summary>
        /// <returns>The enumerable element to iterate through data.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Data).GetEnumerator();
        }
    }
}