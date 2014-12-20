using System;

namespace FileExchange.Models
{
    public class FileCommentsViewModel
    {
        public int CommentId { get; set; }

        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }

        public string Comment { get; set; }
    }
}