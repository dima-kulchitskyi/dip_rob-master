﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.ViewModel
{
    public class ArticleForView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Editable { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }

        public ISet<Tag> ArticleTags { get; set; }

        public ArticleForView(Article a)
        {
            Id = a.Id;
            Title = a.Title;
            FullDescription = a.FullDescription;
            Image = a.Image;
            ArticleTags = a.Tags;
            ShortDescription = a.ShortDescription;
            CreateDate = a.CreateDate;
            UpdateDate = (a.CreateDate != a.LastUpdateDate ? a.LastUpdateDate : a.CreateDate);
            Url = a.Url;
            Editable = false;
        }
        public ArticleForView() { }
    }
    
}