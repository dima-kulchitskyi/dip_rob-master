using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models
{
    public class AppUser : IUser<int>
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
     //   public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Image { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTimeOffset lockoutEnd { get; set; }

        private ISet<Tag> tags = new HashSet<Tag>();
        public virtual ISet<Tag> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                tags = value;
            }
        }

    }
}
