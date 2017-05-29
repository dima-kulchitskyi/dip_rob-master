using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsUa.Models.SignalrModels
{
    public class CommentInfo
    {
        public int Depth { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public bool Deleted { get; set; }
    }
}