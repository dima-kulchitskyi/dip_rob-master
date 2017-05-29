using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace NewsWebSite.Models.Repository
{
    public class NotifiCountCache
    {
        public int GetValue(int id)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            var value = memoryCache.Get(id.ToString());
            if (value == null) return 0;
            return (int)value;
        }

        public void Set(int userId, int value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (!memoryCache.Contains(userId.ToString()))
            {
                memoryCache.Add(userId.ToString(), value, DateTime.Now.AddMinutes(20));
            }
            else
            {
                memoryCache.Set(userId.ToString(), value, DateTime.Now.AddMinutes(20));
            }
        }

        public void Update(int userId, int value)
        {
            var newVal = GetValue(userId) + value;
            if (newVal < 1) newVal = 0;
            Set(userId, newVal);
        }

        public void Delete(int id)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(id.ToString()))
            {
                memoryCache.Remove(id.ToString());
            }
        }
    }
}