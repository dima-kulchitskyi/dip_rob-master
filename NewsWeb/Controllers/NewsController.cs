using NewsWebSite.Models.ViewModel;
using NewsWebSite.Models;
using Newtonsoft.Json;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Microsoft.AspNet.Identity;
using NewsWebSite.Models.Repository;

namespace NewsWebSite.Controllers
{
    public class NewsController : Controller
    {
        readonly int NumberOfItemsOnPage = int.Parse(ConfigurationManager.AppSettings["NumberOfItemsOnPage"]);

        readonly IArticleRepository repo;

        readonly ICommentsRepository commentsRepository;
        readonly INotifiactionsRepository notificationRepo;
        readonly ITagRepository tagRepo;
        readonly IUserRepository userRepo;
        readonly NotifiCountCache notifiCountCache;
        public NewsController(IArticleRepository repo, IUserRepository userRepo, ITagRepository tagRepo, ICommentsRepository commentsRepository, INotifiactionsRepository notifiRepo)

        {
            notificationRepo = notifiRepo;
            notifiCountCache = new NotifiCountCache();
            this.userRepo = userRepo;
            this.tagRepo = tagRepo;
            this.repo = repo;
            this.commentsRepository = commentsRepository;
        }




        #region ForDebug




        [HttpGet]
        public ActionResult CreateLines(int n = 0)
        {

            for (int i = 1; i <= n; i++)
            {
                var a = new Article();
                a.Title = i.ToString();
                a.FullDescription = a.Title;
                a.UserId = 11;
                repo.Save(a);

            }
            return Content("ok");
        }

        #endregion


        public ActionResult Index(bool isUserNews = false, bool isInterestingNews = false, bool setCache = false)
        {

            if (User.Identity.IsAuthenticated && setCache)
            {
                var identityId = User.Identity.GetUserId<int>();
                var notificationsCount = notificationRepo.GetLinesCount(identityId);
                notifiCountCache.Set(identityId, notificationsCount);
            }

            var list = new PagedList<DemoArticle>();
            int userId = 0;
            AppUser currentUser = userRepo.GetById(User.Identity.GetUserId<int>());
            if (isUserNews)  userId = currentUser.Id;
            if (!isInterestingNews)
            {
                list = repo.GetDemoList(new ArticleCriteria() { StartFrom = 0, UserId = userId, Count = NumberOfItemsOnPage, LastId = 0 });
            }
            else
            {
                list = repo.GetArticleByTags(currentUser.Tags, new ArticleCriteria() { StartFrom = 0, UserId = 0, Count = NumberOfItemsOnPage, LastId = 0 });
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult Article(int id = 0, int notifiId = 0)
        {
            if (id < 1) return HttpNotFound();

            if (notifiId > 0)
            {
                if (notificationRepo.View(User.Identity.GetUserId<int>(), notifiId))
                    notifiCountCache.Update(User.Identity.GetUserId<int>(), -1);
            }
            var article = repo.GetItem(id);
            Models.UrlHelper.validateURL(article);
            if (article == null) return HttpNotFound();
            var viewArticle = new ArticleForView(article);
            
            if (User.Identity.IsAuthenticated)
            {
                if (article.UserId == User.Identity.GetUserId<int>())
                    viewArticle.Editable = true;
                viewArticle.UserId = User.Identity.GetUserId<int>();
            }
            else viewArticle.UserId = 0;
            ViewBag.MaxCommentLength = int.Parse(ConfigurationManager.AppSettings["MaxCommentLength"]);
            return View(viewArticle);

        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateArticle()
        {
            CreateArticleModel createArticle = new CreateArticleModel { AllTags = tagRepo.GetAllTags() };
            return View(createArticle);
        }


        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult CreateArticle(CreateArticleModel a, string[] tags)
        {
            if (!ModelState.IsValid) return View(a);
            tags = tags ?? new string[0];
            Article newArticle = new Article
            {
                Title = a.Title,
                ShortDescription = a.ShortDescription,
                FullDescription = a.FullDescription,
                UserId = User.Identity.GetUserId<int>()
            };
            if (a.Image != null)
            {
                newArticle.Image = a.Image.FileName;
            }
            else newArticle.Image = "Empty";
            newArticle.Tags.Clear();
            IEnumerable<Tag> articleTags = TagsHelper.CreateTagList(tags, tagRepo);
            TagsHelper.SetTagForModel(newArticle, articleTags);
            var id = repo.Save(newArticle);
            if (newArticle.Image != "Empty")
            {
                FileHelper fileHelper = new FileHelper();
                fileHelper.SaveOrUpdateArticleImage(Server.MapPath(ConfigurationManager.AppSettings["ArticlImagesFolder"]), a.Image, id);
            }
            return RedirectToAction("Article", new { Id = id });
        }


        [HttpGet]
        [Authorize]
        public ActionResult EditArticle(int id = 0)
        {
            if (id < 1) return HttpNotFound();
            var article = repo.GetItem(id);
            if (article == null) return HttpNotFound();
            EditArticleModel editArticle = new EditArticleModel(article);
            editArticle.AllTags = tagRepo.GetAllTags();
            if (article == null || article.UserId != User.Identity.GetUserId<int>()) return HttpNotFound();
            return View(editArticle);
        }


        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult EditArticle(EditArticleModel edited, string[] tags, string imageCondition)
        {

            if (!ModelState.IsValid) return View(edited);
            var baseArticle = repo.GetItem(edited.Id);

            if (baseArticle == null || baseArticle.UserId != User.Identity.GetUserId<int>()) return HttpNotFound();

            var changesExist = false;
            if (imageCondition == "Empty")
            {
                baseArticle.Image = "Empty";
                changesExist = true;
            }


            if (edited.Image != null)
            {
                var fileHelper = new FileHelper();
                var isChanged = fileHelper.SaveOrUpdateArticleImage(Server.MapPath(ConfigurationManager.AppSettings["ArticlImagesFolder"]), edited.Image, baseArticle.Id);
                if (isChanged)
                {
                    baseArticle.Image = edited.Image.FileName;
                    changesExist = true;
                }
            }
            if (baseArticle.Title != edited.Title)
            {
                baseArticle.Title = edited.Title;
                changesExist = true;
            }
            if (baseArticle.ShortDescription != edited.ShortDescription)
            {
                baseArticle.ShortDescription = edited.ShortDescription;
                changesExist = true;
            }
            if (baseArticle.FullDescription != edited.FullDescription)
            {
                baseArticle.FullDescription = edited.FullDescription;
                changesExist = true;
            }
            baseArticle.Tags.Clear();
            if (tags != null)
            {
                IEnumerable<Tag> newTags = TagsHelper.CreateTagList(tags, tagRepo);
                TagsHelper.SetTagForModel(baseArticle, newTags);
                changesExist = true;
            }
            if (changesExist) repo.Save(baseArticle);
            return RedirectToAction("Article", new { Id = edited.Id });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var authorId = repo.GetUserId(id);
            if (authorId == User.Identity.GetUserId<int>())
                repo.Delete(id);
            return RedirectToAction("Index", "News");
        }
        #region ForAjaxRequests

        [HttpPost]
        public string GetArticles(int page = 1, int n = 1, int lastId = 0, int userId = 0)
        {
            if (page < 1) return "";
            var cr = new ArticleCriteria() { StartFrom = page * NumberOfItemsOnPage, UserId = userId, Count = n * NumberOfItemsOnPage, LastId = lastId };
            var lst = repo.GetDemoList(cr);// as IList<DemoArticle>;
            return JsonConvert.SerializeObject(lst);
        }

        [HttpPost]
        public string GetComments(int articleId)
        {
            var list = commentsRepository.GetList(articleId);
            return JsonConvert.SerializeObject(list);
        }

        #endregion
    }
}
