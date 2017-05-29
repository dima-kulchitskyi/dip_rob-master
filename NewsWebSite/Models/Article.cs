﻿using NewsUa.Attributes;
using NewsUa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsUa.Models
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string FullDescription { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string Image { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime LastUpdateDate { get; set; }
        public virtual int UserId { get; set; }
        private ISet<Tag> tags = new HashSet<Tag>();

        public virtual ISet<Tag> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                tags = value;
            }
        }


        public Article(string title,string shortdescription, string fulldescription, string image, int userId)

        {
            Title = title;
            ShortDescription = shortdescription;
            FullDescription = fulldescription;
            Image = image;
            UserId = userId;
        }
	 
        public Article() { }   

    }
}