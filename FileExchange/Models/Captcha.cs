using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FileExchange.Models
{
    public class Captcha
    {
        [Display(Name = "Captcha",Description = "Captcha", Order = 20)] 
      
        public virtual string CaptchaValue { get; set; }
        public Captcha()
        {

        }
    }
}