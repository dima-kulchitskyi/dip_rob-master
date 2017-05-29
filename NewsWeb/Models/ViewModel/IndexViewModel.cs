using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace NewsWebSite.Models.ViewModel
{
    public class IndexViewModel
    {
        [Display(Name = "Show only my articles")]
        public bool OnlyMy { get; set; }

        [Display(Name = "Tags")]
        [StringLength(50, ErrorMessage = "Tags Max Length is 50")]
        public string Tags { get;  set;}
    }
}