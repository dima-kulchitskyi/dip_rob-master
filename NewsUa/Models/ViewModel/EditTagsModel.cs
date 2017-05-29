using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsUa.Models.ViewModel
{
    public class EditTagsModel
    {
        public ISet<Tag> UserTags { get; set; }
        public IEnumerable<Tag> AllTags { get; set; }
    }
}