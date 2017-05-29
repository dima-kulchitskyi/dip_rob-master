using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NewsWebSite
{
    public class CommentsHelper
    {
        public bool ValidateText(string text)
        {
            if (text.Length == 0 || text.Length > int.Parse(ConfigurationManager.AppSettings["MaxCommentLength"]))
                return false;
            return true;
           

        }
    }
}