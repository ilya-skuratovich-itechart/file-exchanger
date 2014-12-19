namespace FileExchange.Models
{
    public class CategoryFilesViewModel
    {
        public int FileId { get; set; }

        public int FileCategoryId { get; set; }

        public string UniqFileName { get; set; }

        public string OrigFileName { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; } 
    }
}