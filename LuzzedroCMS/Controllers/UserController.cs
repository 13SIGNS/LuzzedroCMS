using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Attributes;
using LuzzedroCMS.Models;
using LuzzedroCMS.WebUI.Infrastructure.Enums;
using LuzzedroCMS.WebUI.Infrastructure.Helpers;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using LuzzedroCMS.WebUI.Properties;
using LuzzedroCMS.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class UserController : Controller
    {
        private IUserRepository repoUser;
        private ICommentRepository repoComment;
        private IArticleRepository repoArticle;
        private ICategoryRepository repoCategory;
        private IConfigurationKeyRepository repoConfig;
        private IAccount account;
        private IFtp ftp;
        private ITextBuilder textBuilder;
        private IImageModifier imageModifier;
        private ISessionHelper repoSession;
        private IImageHelper imageHelper;
        private const string NULLIMAGE = "null.png";
        public int PageSize = 10;

        private User user
        {
            get
            {
                return repoUser.User(email: repoSession.UserEmail);
            }
        }

        public UserController(
            IUserRepository userRepo,
            ICommentRepository commentRepo,
            IArticleRepository articleRepo,
            ICategoryRepository categoryRepo,
            IConfigurationKeyRepository configRepo,
            IAccount accnt,
            IFtp ft,
            ITextBuilder txtBuilder,
            IImageModifier imgModifier,
            ISessionHelper sessionRepo,
            IImageHelper imgHelper)
        {
            repoUser = userRepo;
            repoComment = commentRepo;
            repoArticle = articleRepo;
            repoCategory = categoryRepo;
            repoConfig = configRepo;
            account = accnt;
            ftp = ft;
            textBuilder = txtBuilder;
            imageModifier = imgModifier;
            repoSession = sessionRepo;
            imageHelper = imgHelper;
            repoSession.Controller = this;

        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditAccount()
        {
            ViewBag.Title = Resources.EditData;
            return View(new UserViewModel
            {
                User = user,
                PhotoUrlPath = String.Format("{0}UserProfileImage/{1}", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL), repoSession.UserPhotoUrl)
            });
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(User user)
        {
            return Redirect(Url.Action("EditAccount", "User"));
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != changePasswordViewModel.OldPassword)
                {
                    this.SetMessage(InfoMessageType.Danger, Resources.PasswordIncorrect);
                    return Redirect(Url.Action("EditAccount", "User"));
                }
                else
                {
                    user.Password = changePasswordViewModel.NewPassword;
                    repoUser.Save(user);
                    this.SetMessage(InfoMessageType.Success, Resources.ProperlyChangedPassword);
                }

            }
            return Redirect(Url.Action("EditAccount", "User"));
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoto(string ImageCropped)
        {
            bool isImage = false;
            try
            {
                Convert.FromBase64String(ImageCropped
                    .Replace("data:image/png;base64,", String.Empty)
                    .Replace("data:image/jpg;base64,", String.Empty)
                    .Replace("data:image/bmp;base64,", String.Empty)
                    .Replace("data:image/gif;base64,", String.Empty));
                isImage = true;
            }
            catch (Exception)
            {
                isImage = false;
            }

            if (ImageCropped != null && isImage)
            {
                imageHelper.ImageString = ImageCropped;

                string imageName = repoUser.GetUniqueImageTitle(string.Empty);
                bool isUploaded = true;
                if (imageHelper.IsFtp())
                {
                    if (!imageHelper.UploadUserProfileImagesToFtp(imageName))
                    {
                        this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                        isUploaded = false;
                    }

                    if (!imageHelper.RemoveOldPhotoUserProfileImagesFromFtp(user.PhotoUrl))
                    {
                        this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                        isUploaded = false;
                    }
                }
                else
                {
                    if (!imageHelper.UploadUserProfileImagesToLocal(imageName, Server))
                    {
                        this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                        isUploaded = false;
                    }
                    if (!imageHelper.RemoveOldPhotoUserProfileImagesFromLocal(user.PhotoUrl, Request))
                    {
                        this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                        isUploaded = false;
                    }
                }

                if (isUploaded)
                {
                    user.PhotoUrl = imageName;
                    repoUser.Save(user);
                    account.SaveUserInSession(this);
                    this.SetMessage(InfoMessageType.Success, Resources.ProperlyAddedImage);
                }
            }
            return Redirect(Url.Action("EditAccount", "User"));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult Bookmarks(int page = 1)
        {
            ViewBag.Title = Resources.Bookmarks;
            ViewBag.HideTitle = false;
            IList<Article> articles = repoArticle.Articles(bookmarkUserID: user.UserID).OrderBy(x => x.DateAdd).Skip((page - 1) * PageSize).Take(PageSize).ToList();
            ArticlesViewModel articlesCategoriesPagingViewModel = new ArticlesViewModel
            {
                Articles = articles,
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repoArticle.Articles(bookmarkUserID: user.UserID).Count()
                }
            };
            return View(articlesCategoriesPagingViewModel);
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditBookmark(int articleID)
        {
            if (ModelState.IsValid)
            {
                repoArticle.SaveBookmark(articleID, user.UserID);
            }
            return Redirect(Url.Action("Favourites", "User"));
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveBookmark(int articleID)
        {
            if (ModelState.IsValid)
            {
                repoArticle.RemoveBookmark(articleID, user.UserID);
            }
            return Redirect(Url.Action("Favourites", "User"));
        }

        [HttpGet]
        public ViewResult Comments(int page = 1)
        {
            ViewBag.Title = Resources.ListMyComments;
            IList<Comment> Comments = repoComment.Comments(userID: user.UserID, page: page, take: PageSize);
            IList<CommentWithArticleViewModel> commentWithArticleViewModel = new List<CommentWithArticleViewModel>();
            foreach (var comment in Comments)
            {
                commentWithArticleViewModel.Add(new CommentWithArticleViewModel
                {
                    Comment = comment,
                    Article = repoArticle.Article(commentID: comment.CommentID)
                });
            }
            CommentsWithArticlesViewModel CommentsViewModel = new CommentsWithArticlesViewModel
            {
                CommentWithArticleViewModel = commentWithArticleViewModel,
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repoComment.Comments(userID: user.UserID).Count()
                }
            };
            return View(CommentsViewModel);
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditComment(int articleID, string returnUrl)
        {
            ViewBag.Title = Resources.EditComment;
            return View(new EditCommentViewModel
            {
                ArticleID = articleID,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Comment comment, int articleID, string returnUrl)
        {
            if (string.IsNullOrEmpty(repoSession.UserNick))
            {
                ModelState.AddModelError(string.Empty, Resources.YouMustCreateNick);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Comment newComment = new Comment
                    {
                        Content = comment.Content
                    };
                    int commentID = repoComment.Save(newComment);
                }
            }
            return Redirect(returnUrl ?? Url.Action("Comments", "User"));
        }

        [HttpGet]
        [ChildActionOnly]
        public int BookmarksCount()
        {
            IList<Article> articles = repoArticle.Articles(bookmarkUserID: user.UserID);
            bool any = articles != null;
            return any ? articles.Count() : 0;
        }

        [HttpGet]
        [ChildActionOnly]
        public int CommentsCount()
        {
            IList<Comment> comments = repoComment.Comments(userID: user.UserID);
            bool any = comments != null;
            return any ? comments.Count() : 0;
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult EditNick()
        {
            return View("EditNick", new NickViewModel
            {
                Nick = repoSession.UserNick
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditNick(NickViewModel nickViewModel)
        {
            bool isNickAdded = false;
            string error = string.Empty;
            if (ModelState.IsValid)
            {
                User existingUser = repoUser.User(nick: nickViewModel.Nick);
                if (existingUser == null)
                {
                    User user = repoUser.User(email: repoSession.UserEmail);
                    user.Nick = nickViewModel.Nick;
                    repoUser.Save(user);
                    repoSession.UserNick = nickViewModel.Nick;
                    isNickAdded = true;
                }
                else
                {
                    error = Resources.UserNickExists;
                    isNickAdded = false;
                }
            }
            return Json(new { IsNickAdded = isNickAdded, Error = error }); ;
        }
    }
}