using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace NewsWebSite.Attributes
{
    public class ValidImage : ValidationAttribute
    {

        //public ValidImage(int maxSizeMB)
        //{
        //    this.maxSizeMB = maxSizeMB;
        //}

        public int maxSizeMB { get; set; } = 4; //Max: 15
        public override bool IsValid(object value)
        {
            if (value == null) return true;


           
            var file = value as HttpPostedFileBase;

            if (file == null) return false;
            int maxContent = maxSizeMB * 1024 * 1024;
            if (file.ContentLength > maxContent)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + maxSizeMB + "MB";
                return false;
            }

            return true;
        }
    }
}