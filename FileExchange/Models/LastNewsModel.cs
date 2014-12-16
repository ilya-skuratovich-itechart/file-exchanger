using System.Collections.Generic;

namespace FileExchange.Models
{
    public class LastNewsModel
    {
        public bool AllowEdit { get; set; }
        public List<NewsModel> News { get; set; } 
        public LastNewsModel()
        {
            
        }

        public void BindNews(List<NewsModel> news)
        {
            News = news ?? new List<NewsModel>(0);
        }
    }
}