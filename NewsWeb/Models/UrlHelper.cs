using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace NewsWebSite.Models
{
    public class UrlHelper
    {
        public static void validateURL(Article a)
        {
            string input = a.Title;
            a.Url = Regex.Replace(input, @"[\W \s]+", "-");
        }
    }
}