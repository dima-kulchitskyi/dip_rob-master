using NewsUa.Controllers;
using NewsUa.Models.ViewModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using static NewsUa.Models.Repository.ArticleRepository;

namespace NewsUa.Models.Repository
{
    public interface IArticleRepository
    {
        int GetUserId(int id);
        Article GetItem(int id);
        int GetCountOfLines();
        PagedList<DemoArticle> GetDemoList(ArticleCriteria cr);
        PagedList<DemoArticle> GetArticleByTags(IEnumerable<Tag> tags, ArticleCriteria cr);
        int Save(Article article);
        bool IsExist(int id);
        void Delete(int articleId);
    }

    public class PagedList<T> : List<T>
    {
        public int LinesCount { get; set; }
        public int PageCount { get; set; }

        public PagedList()
        {

        }

        public PagedList(IEnumerable<T> list) : base(list)
        {

        }
    }

    public class PagedList
    {
        public static PagedList<T> Create<T>(IEnumerable<T> list, int pageCount)
        {
            return new PagedList<T>(list)
            {
                PageCount = pageCount
            };
        }
    }
}