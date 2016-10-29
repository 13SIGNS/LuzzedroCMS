using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure;
using LuzzedroCMS.Infrastructure.Concrete;
using LuzzedroCMS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using LuzzedroCMS.WebUI.Properties;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private IUserRepository repoUser;
        private ICommentRepository repoComment;
        private IArticleRepository repoArticle;
        private ICategoryRepository repoCategory;
        private TextBuilder textBuilder;
        private User user;

        public UserController(IUserRepository userRepo, ICommentRepository commentRepo, IArticleRepository articleRepo, ICategoryRepository categoryRepo)
        {
            repoUser = userRepo;
            repoComment = commentRepo;
            repoArticle = articleRepo;
            repoCategory = categoryRepo;
            user = repoUser.UserByEmail(System.Web.HttpContext.Current.User.Identity.Name);
            textBuilder = new TextBuilder();
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditAccount()
        {
            ViewBag.Title = Resources.EditData;
            return View(user);
        }

        [HttpPost]
        public ActionResult EditAccount(User user)
        {
            if (ModelState.IsValid)
            {

            }
            return Redirect(Url.Action("EditAccount", "User"));
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult EditPassword(string OldPassword, ResetViewModel resetViewModel)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != OldPassword)
                {
                    TempData["Info.danger"] = Resources.PasswordIncorrect;
                    return Redirect(Url.Action("EditAccount", "User"));
                }else
                {
                    user.Password = resetViewModel.Password;
                    repoUser.Save(user);
                    TempData["Info.success"] = Resources.ProperlyChangedPassword;
                }
                
            }
            return Redirect(Url.Action("EditAccount", "User"));
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult EditPhoto(string ImageCropped)
        {
            if (ImageCropped != null)
            {
                byte[] bytes = Convert.FromBase64String(ImageCropped
                    .Replace("data:image/png;base64,", String.Empty)
                    .Replace("data:image/jpg;base64,", String.Empty)
                    .Replace("data:image/bmp;base64,", String.Empty)
                    .Replace("data:image/gif;base64,", String.Empty));

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }
                string imageName = repoUser.GetUniqueImageTitle("");

                ImageModifier imageModifier = new ImageModifier();
                var path = Path.Combine(Server.MapPath("~/Content/UserProfileImage/"), imageName);
                Image img = imageModifier.ResizeImage(image, 320, 240);
                img.Save(path);
                string oldPhoto = user.PhotoUrl;
                if(oldPhoto != "null.png")
                {
                    string fullPath = Request.MapPath("~/Content/UserProfileImage/" + oldPhoto);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                user.PhotoUrl = imageName;
                repoUser.Save(user);
                SessionSaver sessionSaver = new SessionSaver(repoUser);
                sessionSaver.SetSessionUserData();
                TempData["Info.success"] = Resources.ProperlyAddedImage;
            }
            return Redirect(Url.Action("EditAccount", "User"));
        }

        [HttpGet]
        public ViewResult Bookmarks()
        {
            ViewBag.Title = Resources.ArticleByTag;
            ViewBag.HideTitle = true;
            IQueryable<Article> articles = repoArticle.ArticlesEnabledActualByBookmark(user.UserID);
            IList<string> categories = new List<string>();
            foreach (var article in articles)
            {
                Category category = repoCategory.CategoryByID(article.CategoryID);
                categories.Add(category.Name);
            }
            ArticlesCategoriesViewModel articlesCategoriesViewModel = new ArticlesCategoriesViewModel
            {
                Articles = articles,
                Categories = categories
            };
            return View(articlesCategoriesViewModel);
        }

        [HttpPost]
        public ActionResult EditBookmark(int articleID)
        {
            if (ModelState.IsValid)
            {
                repoArticle.SaveBookmark(articleID, user.UserID);
            }
            return Redirect(Url.Action("Favourites", "User"));
        }

        [HttpPost]
        public ActionResult RemoveBookmark(int articleID)
        {
            if (ModelState.IsValid)
            {
                repoArticle.RemoveBookmark(articleID, user.UserID);
            }
            return Redirect(Url.Action("Favourites", "User"));
        }

        [HttpGet]
        public ViewResult Comments()
        {
            ViewBag.Title = Resources.ListMyComments;
            IQueryable<Comment> comments = repoComment.CommentsEnabledByUserID(user.UserID);
            IList<Category> categories = new List<Category>();
            IList<Article> articles = new List<Article>();
            foreach (var comment in comments)
            {
                Article article = repoArticle.ArticleEnabledActualByComment(comment.CommentID);
                Category category = repoCategory.CategoryByID(article.CategoryID);
                categories.Add(category);
                articles.Add(article);
            }
            CommentsArticlesViewModel commentsArticleViewModel = new CommentsArticlesViewModel
            {
                Comments = comments,
                Categories = categories,
                Articles = articles
            };
            return View(commentsArticleViewModel);
        }

        [HttpGet]
        public ViewResult EditComment(int articleID, string returnUrl)
        {
            return View(new EditCommentViewModel {
            ArticleID = articleID,
            ReturnUrl= returnUrl
            });
        }

        [HttpPost]
        public ActionResult EditComment(Comment comment, int articleID, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment
                {
                    ArticleID = articleID,
                    Content = comment.Content,
                    UserID = user.UserID
                };
                int commentID = repoComment.Save(newComment);
            }
            return Redirect(returnUrl ?? Url.Action("Comments", "User"));
        }

        [HttpGet]
        [ChildActionOnly]
        public int BookmarksCount()
        {
            IQueryable<Article> articles = repoArticle.ArticlesEnabledActualByBookmark(user.UserID);
            bool any = articles != null;
            return any ? articles.Count() : 0;
        }

        [HttpGet]
        [ChildActionOnly]
        public int CommentsCount()
        {
            IQueryable<Comment> comments = repoComment.CommentsEnabledByUserID(user.UserID);
            bool any = comments != null;
            return any ? comments.Count() : 0;
        }

    }
}