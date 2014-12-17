namespace FileExchange.Models
{
    public class jQueryDataTableParamModel
    {
        public int StatusTypeID { get; set; }

        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }


        public int SortingColumnNumber
        {
            get
            {
                return iSortCol_0;
            }
        }

        public int iSortCol_0 { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        public string sSortDir_0 { get; set; }

        public string sSearch_0 { get; set; }
        public string sSearch_1 { get; set; }
        public string sSearch_2 { get; set; }
        public string sSearch_3 { get; set; }
        public string sSearch_4 { get; set; }
        public string sSearch_5 { get; set; }
        public string sSearch_6 { get; set; } 
    }
}