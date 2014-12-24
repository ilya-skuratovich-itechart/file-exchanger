using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileExchange.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "User name:")]
        public string UserName { get; set; }

        [Display(Name = "User email:")]
        public string UserEmail { get; set; }


        [Display(Name = "Max download speed kbps (0 unlimited):")]
        public int MaxDonwloadSpeedKbps { get; set; }

        [Display(Name = "Max upload file size kbps (0 unlimited):")]
        public int FileMaxSizeKbps { get; set; }

        /// <summary>
        /// use for a post data
        /// </summary>
        public int[] RolesIds { get; set; }

        public IEnumerable<UserRolesModel> SelectedUserRoles { get; set; }

        public IEnumerable<UserRolesModel> UserRoles { get; set; }

    }
}