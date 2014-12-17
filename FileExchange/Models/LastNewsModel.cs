using System.Collections.Generic;

namespace FileExchange.Models
{
    public class LastNewsModel
    {
        public bool AllowEdit { get; set; }
        public List<EditNewsModel> News { get; set; } 
        public LastNewsModel()
        {
            
        }

        public void BindNews(List<EditNewsModel> news)
        {
            News = news ?? new List<EditNewsModel>(0);
        }
    }
}