namespace FileExchange.Models.DataTable
{
    public class DefaultDataTablesRequest
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets/Sets the draw counter from DataTables.
        /// </summary>
        public virtual int Draw { get; set; }
        /// <summary>
        /// Gets/Sets the start record number (jump) for paging.
        /// </summary>
        public virtual int Start { get; set; }
        /// <summary>
        /// Gets/Sets the length of the page (paging).
        /// </summary>
        public virtual int Length { get; set; }
        /// <summary>
        /// Gets/Sets the global search term.
        /// </summary>
        public virtual Search Search { get; set; }
        /// <summary>
        /// Gets/Sets the column collection.
        /// </summary>
        public virtual ColumnCollection Columns { get; set; } 
    }
}