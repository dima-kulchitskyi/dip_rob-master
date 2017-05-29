using NewsWebSite.Models;
using NewsWebSite.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebSite
{
    public class TagsHelper
    {

        public static IEnumerable<Tag> CreateTagList(string[] tags , ITagRepository tagRepo)
        {
            List<string> allTag = tagRepo.GetAllTags().Select(x => x.TagText).ToList();
            List<string> needInSave = tags.ToList().Except(allTag).ToList();
            List<string> areInDB = tags.ToList().Intersect(allTag).ToList();
            IEnumerable<Tag> loadTags = tagRepo.GatTagsByName(areInDB.ToArray()).Concat(tagRepo.Save(needInSave.ToArray()));
            return loadTags;
        }
        public static void SetTagForModel(Article article , IEnumerable<Tag> tagList)
        {
            article.Tags = new HashSet<Tag>(tagList);
        }

        public static void SetTagForModel(AppUser user , IEnumerable<Tag> tagList)
        {
            user.Tags.Clear();
            foreach(Tag tag in tagList)
            {
                user.Tags.Add(tag);
            }
        }

        public string[] GetArray(string tags)
        {

            if (string.IsNullOrEmpty(tags)) return new string[] { };
            tags = System.Text.RegularExpressions.Regex.Replace(tags, @"\s+", "");
            if (tags == "") return new string[] { };
            tags = tags.ToLower();
            tags = tags.Trim(',');
            var arr = tags.Split(',').ToList<string>();
            for (int i = 0; i < arr.Count; i++)
            {
                arr[i] = arr[i].Trim(',');
                if (arr[i] == string.Empty)
                {
                    arr.RemoveAt(i);
                    i--;
                }
            }
            return arr.ToArray();
        }
        public string GetLine(string tags)
        {
            var array = GetArray(tags);
            if (array.Length == 0) return string.Empty;
            return "," + string.Join(",", GetArray(tags)).Trim(',') + ",";
        }
        public string GetLineToShow(string tags)
        {
            var array = GetArray(tags);
            if (array.Length == 0) return string.Empty;
            return string.Join(", ", GetArray(tags));
        }
    }
}