using NewsWebSite.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite.Models.ViewModel
{
    public class EditArticleModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Заголовок статьи")]
        [StringLength(50, ErrorMessage = "Description Max Length is 50")]

        public string Title { get; set; }
        [Required]
        [Display(Name = "Краткое описание статьи")]
        [StringLength(200, ErrorMessage = "Максимальная длина описания статьи 200 символов")]
        public string ShortDescription { get; set; }

        [Display(Name = "Текст статьи")]
        [DataType(DataType.MultilineText)]
        [StringLength(2000, ErrorMessage = "Description Max Length is 2000")]
        public string FullDescription { get; set; }


        [Display(Name = "Изображение")]
        [ValidImage(maxSizeMB = 5)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name ="Теги статьи")]
        public ISet<Tag> ArticleTags { get; set; }
        public IEnumerable<Tag> AllTags { get; set; }

        public string ImagePath { get; set; }

        public EditArticleModel(Article a)
        {
            Id = a.Id;
            Title = a.Title;
            ShortDescription = a.ShortDescription;
            FullDescription = a.FullDescription;
            ArticleTags = a.Tags;
            ImagePath = a.Image;
        }
        public EditArticleModel()
        {
        }
    }
}