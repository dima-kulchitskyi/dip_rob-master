using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class Notification
    {
        public virtual int Id { get; set; }
        public virtual string Message { get; set; }
        public virtual int CommentId { get; set; }
        public virtual int UserId { get; set; }
        public virtual bool Viewed { get; set; }
        public virtual int ArticleId { get; set; }
        public virtual string FromWho { get; set; }

        public Notification()
        {
            Viewed = false;
        }
    }
}