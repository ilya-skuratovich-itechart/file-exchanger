﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace FileExchange.Models
{
    public class CreateNewsModel
    {
        public int? NewsId { get; set; }
       
        [Required(ErrorMessage = "Необходимо ввести заголовок")]
        [StringLength(255,ErrorMessage = "Максимальная длина 255")]
        [Display(Name = "Заголовок")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Необходимо ввести текст")]
        [AllowHtml] 
        public string Text { get; set; }

        [Required(ErrorMessage = "Необходимо добавить файл")]
        public HttpPostedFileBase File { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreateDate { get; set; }
    }
}