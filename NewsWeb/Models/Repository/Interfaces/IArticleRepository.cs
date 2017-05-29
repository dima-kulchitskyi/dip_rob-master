﻿using NewsWebSite.Controllers;
using NewsWebSite.Models.ViewModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using static NewsWebSite.Models.Repository.ArticleRepository;

namespace NewsWebSite.Models.Repository
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
        public static PagedList<T> Create<T>(IEnumerable<T> list, int totalCount)
        {
            return new PagedList<T>(list)
            {
                LinesCount = totalCount
            };
        }
    }
}