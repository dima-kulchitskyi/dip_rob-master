using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.SignalrModels
{
    public class CommentsUser
    {
        public string ConnectionId { get; set; }
        public int AppUserId { get; set; }
        public string UserName { get; set; }
        public int ArticleId { get; set; }

    }
}