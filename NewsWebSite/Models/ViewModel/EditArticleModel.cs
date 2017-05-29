using Microsoft.Security.Application;
using NewsUa.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsUa.Models.ViewModel
{
    public class EditArticleModel : IEquatable<EditArticleModel>
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        //string _Title;
        [Required]
        [Display(Name = "Заголовок")]
        [StringLength(150, ErrorMessage = "Description Max Length is 150")]
        public string Title { get; set; }
        // public string Title { get { return _Title; } set { _Title = Sanitizer.GetSafeHtmlFragment(value).Replace("&#1084;", "м"); } }

        //string _ShortDescription;
        [Required]
        [Display(Name = "Краткое описание статьи")]
        [StringLength(250, ErrorMessage = "Максимальная длина описания статьи 250 символов")]
        public string ShortDescription { get; set; }
        //public string ShortDescription { get { return _ShortDescription; } set { _ShortDescription = Sanitizer.GetSafeHtmlFragment(value).Replace("&#1084;", "м"); } }

        string _FullDescription;
        [Required]
        [Display(Name = "Текст статьи")]
        [DataType(DataType.MultilineText)]
        [StringLength(20000, ErrorMessage = "Description Max Length is 20000")]
        public string FullDescription { get { return _FullDescription; } set { _FullDescription = Sanitizer.GetSafeHtmlFragment(value); } }


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

        public override bool Equals(object obj)
        {
            return Equals(obj as EditArticleModel);
        }
        public override int GetHashCode()
        {
            return this.Id;
        }

        public bool Equals(EditArticleModel other)
        {
            if (other.Title != this.Title) return false;
            if (other.ShortDescription != this.ShortDescription) return false;
            if (other.FullDescription != this.FullDescription) return false;
            if (!this.ArticleTags.SequenceEqual(other.ArticleTags)) return false;
            return true;
        }
    }
}