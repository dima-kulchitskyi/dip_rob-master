using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace NewsWebSite.Attributes
{
    public class AllowedExtensions : ValidationAttribute
    {
        public AllowedExtensions(string[] allExt)
        {
            AllowedExt = allExt;
        }

        public string[] AllowedExt { get; set; } = { ".jpg", ".png" };
        public override bool IsValid(object value)
        {
            if (value as HttpPostedFileBase == null) return true;
            if (!AllowedExt.Contains(Path.GetExtension((value as HttpPostedFileBase).FileName)))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", AllowedExt);
                return false;
            }

            return true;
        }
    }
}