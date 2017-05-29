using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsUa.Models
{
    public class DemoArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}