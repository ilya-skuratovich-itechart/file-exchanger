using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace FileExchange.Models
{
    public class CreateExchangeFileModel
    {
        [Required(ErrorMessage = "File category required")]
        public int SelectedFileCategoryId { get; set; }
        [Display(Name = "File category:")]
        public IEnumerable<SelectListItem> FileCategories;

        [Display(Name = "File:")]
        [Required(ErrorMessage = "Need to add a file")]
        public HttpPostedFileBase File { get; set; }

        [Display(Name = "Description:")]
        [Required(ErrorMessage = "Need to write a description")]
        public string Description { get; set; }

        [Display(Name = "Tags:")]
        public string Tags { get; set; }

        [Display(Name = "Allow anonymous user to view")]
        public bool AllowViewAnonymousUsers { get; set; }

        [Display(Name = "Deny all")]
        public bool DenyAll { get; set; }

        public CreateExchangeFileModel()
        {

        }

        public CreateExchangeFileModel(IEnumerable<SelectListItem> fileCategories)
        {
            FileCategories = fileCategories;
        }


    }
}