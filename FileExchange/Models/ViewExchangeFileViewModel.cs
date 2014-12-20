using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Models
{
    public class ViewExchangeFileViewModel
    {
        public int FileId { get; set; }

        public string UniqFileName { get; set; }

        public string OrigFileName { get; set; }

        public string Description { get; set; }

        public string Owner { get; set; }

        public string Tags { get; set; }

        public bool HasSubscription { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}