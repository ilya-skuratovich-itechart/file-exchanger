using System;

namespace FileExchange.Models
{
    public class ViewNewsViewModel
    {
        public int NewsId { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }

        public string ImagePath { get; set; }

        public string OrigImageName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; } 
    }
}