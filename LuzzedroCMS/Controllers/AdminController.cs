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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LuzzedroCMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IArticleRepository repoArticle;
        private ICategoryRepository repoCategory;
        private ICommentRepository repoComment;
        private IUserRepository repoUser;
        private ITagRepository repoTag;
        private IConfigurationKeyRepository repoConfig;
        private IImageModifier imageModifier;
        private IFtp ftp;
        private ITextBuilder textBuilder;
        private ISessionHelper repoSession;
        private IImageHelper imageHelper;
        public int PageSize = 10;

        private User user
        {
            get
            {
                return repoUser.User(email: repoSession.UserEmail);
            }
        }

        public AdminController(
            IArticleRepository articleRepo,
            ICategoryRepository categoryRepo,
            ICommentRepository commentRepo,
            IUserRepository userRepo,
            ITagRepository tagRepo,
            IConfigurationKeyRepository configRepo,
            IImageModifier imgModifier,
            IFtp ft,
            ITextBuilder txtBuilder,
            ISessionHelper sessionRepo,
            IImageHelper imgHelper)
        {
            repoArticle = articleRepo;
            repoCategory = categoryRepo;
            repoComment = commentRepo;
            repoUser = userRepo;
            repoTag = tagRepo;
            repoConfig = configRepo;
            imageModifier = imgModifier;
            ftp = ft;
            textBuilder = txtBuilder;
            repoSession = sessionRepo;
            imageHelper = imgHelper;
            repoSession.Controller = this;
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult Index()
        {
            return View(user);
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveArticle(int articleID)
        {
            if (ModelState.IsValid)
            {
                repoArticle.RemovePermanently(articleID);
                this.SetMessage(InfoMessageType.Success, Resources.ProperlyRemovedArticle);
            }
            return Redirect(Url.Action("Articles", "Admin"));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditArticle(string url = "")
        {
            IList<Tag> tags = repoTag.Tags();
            IList<int> selectedTagsID = new List<int>();
            IList<Category> categories = repoCategory.Categories();
            Article article;
            ArticleExtended articleExtended;
            IList<string> imageNames = new List<string>();

            foreach (var imageName in imageHelper.IsFtp() ? imageHelper.GetAllImagesForArticleFromFtp() : imageHelper.GetAllImagesForArticleFromLocal(Server))
            {
                imageNames.Add(imageName.Split('/').Last());
            }

            if (url == string.Empty)
            {
                article = new Article
                {
                    DatePub = DateTime.Now,
                    DateExp = DateTime.Now.AddYears(50),
                    Status = 1
                };
                ViewBag.Title = Resources.AddingArticle;
                articleExtended = new ArticleExtended
                {
                    Article = article
                };
            }
            else
            {
                articleExtended = repoArticle.ArticleExtended(url: url, actual: false);
                ViewBag.Title = Resources.EditArticle;
            }

            if (articleExtended.Tags != null)
            {
                foreach (var tag in articleExtended.Tags)
                {
                    selectedTagsID.Add(tag.TagID);
                }
            }

            return View(new EditArticleViewModel
            {
                ArticleExtended = articleExtended,
                SelectedTagsId = selectedTagsID,
                Categories = categories,
                Tags = tags,
                ImageNames = imageNames,
                ContentExternalUrl = repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL)
            });
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditArticle(ArticleExtended articleExtended, int[] SelectedTagsID, string returnUrl, string ExistingImageName)
        {
            if (ModelState.IsValid)
            {
                int articleID = repoArticle.Save(new Article
                {
                    ArticleID = articleExtended.Article.ArticleID,
                    CategoryID = articleExtended.Article.CategoryID,
                    UserID = user.UserID,
                    ImageName = ExistingImageName,
                    ImageDesc = articleExtended.Article.ImageDesc,
                    DatePub = articleExtended.Article.DatePub,
                    DateExp = articleExtended.Article.DateExp,
                    Title = articleExtended.Article.Title,
                    Lead = articleExtended.Article.Lead,
                    Content = articleExtended.Article.Content,
                    Source = articleExtended.Article.Source,
                    Status = articleExtended.Article.Status
                });

                if (SelectedTagsID != null)
                {
                    foreach (int selectedTagID in SelectedTagsID)
                    {
                        repoArticle.AddTagToArticle(articleID, selectedTagID);
                    }
                }
                this.SetMessage(InfoMessageType.Success, (articleExtended.Article.ArticleID == 0) ? Resources.ProperlyAddedArticle : Resources.CorrectlyChanged);
                return Redirect(Url.Action("Articles", "Admin"));
            }
            else
            {
                return Redirect(Url.Action("EditArticle", "Admin", new { articleExtended.Article.Url }));
            }

        }

        [HttpGet]
        public ViewResult Articles()
        {
            ViewBag.Title = Resources.ListOfArticles;
            IList<Article> articles = repoArticle.Articles(actual: false, enabled: false);
            return View(articles);
        }

        [HttpGet]
        public ViewResult Comments(int page = 1)
        {
            ViewBag.Title = Resources.ListOfComments;
            IList<CommentExtended> commentsExtended = repoComment.CommentsExtended(page: page, take: PageSize, enabled: false);
            CommentsViewModel commentsViewModel = new CommentsViewModel
            {
                CommentsExtended = commentsExtended,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repoComment.Comments(enabled: false).Count()
                }
            };
            return View(commentsViewModel);
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditComment(int commentID)
        {
            ViewBag.Title = Resources.EditComment;
            Comment comment = repoComment.Comment(commentID: commentID, enabled: false);
            return View(comment);
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                Comment oldComment = repoComment.Comment(commentID: comment.CommentID, enabled: false);
                oldComment.Content = comment.Content;
                oldComment.Status = comment.Status;
                repoComment.Save(oldComment);
                this.SetMessage(InfoMessageType.Success, Resources.CorrectlyEditComment);
                return Redirect(Url.Action("Comments", "Admin"));
            }
            return Redirect(Url.Action("EditComment", "Admin", new { comment.CommentID }));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditPhoto()
        {
            ViewBag.Title = Resources.AddingAnImage;
            return View(new ImageViewModel());
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoto(ImageViewModel imageViewModel)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    string imageInfo = string.Empty;
                    var file = Request.Files[0];
                    imageHelper.File = file;
                    bool isUploaded = true;
                    if (!imageHelper.IsFileSet())
                    {
                        imageInfo = Resources.FileNotHaveContent;
                    }
                    else if (imageHelper.IsFileInGoodSize())
                    {
                        imageInfo = Resources.FileCannotBeMore;
                    }
                    else if (!imageHelper.IsFileImage())
                    {
                        imageInfo = Resources.InvalidFileType;
                    }
                    else if (imageHelper.IsFtp())
                    {
                        if (!imageHelper.UploadArticleImagesToFtp(repoArticle.GetUniqueImageTitle(imageHelper.GetImageName(imageViewModel.ImageDesc))))
                        {
                            this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                            isUploaded = false;
                        }
                    }
                    else
                    {
                        if (!imageHelper.UploadArticleImagesToLocal(repoArticle.GetUniqueImageTitle(imageHelper.GetImageName(imageViewModel.ImageDesc)), Server))
                        {
                            this.SetMessage(InfoMessageType.Danger, Resources.ErrorOcurred);
                            isUploaded = false;
                        }
                    }

                    if (isUploaded)
                    {
                        if (imageInfo != string.Empty)
                        {
                            imageInfo = Resources.NoAddedImage;
                            this.SetMessage(InfoMessageType.Info, imageInfo);
                        }
                        else
                        {
                            this.SetMessage(InfoMessageType.Success, Resources.ProperlyAddedImage);
                        }
                    }
                }
            }
            return Redirect(Url.Action("EditPhoto", "Admin"));
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveComment(int commentID)
        {
            if (ModelState.IsValid)
            {
                repoComment.Remove(commentID);
                this.SetMessage(InfoMessageType.Success, Resources.ProperlyRemovedComment);
                return Redirect(Url.Action("Comments", "Admin"));
            }
            return Redirect(Url.Action("EditComment", "Admin"));
        }

        [HttpGet]
        public ViewResult Tags()
        {
            ViewBag.Title = Resources.Tags;
            IList<Tag> tags = repoTag.Tags(enabled: false);
            return View(tags);
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditTag(int tagID = 0)
        {
            ViewBag.Title = Resources.EditTag;
            Tag tag;
            if (tagID == 0)
            {
                tag = new Tag
                {
                    Status = 1
                };
            }
            else
            {
                tag = repoTag.Tag(tagID: tagID, enabled: false);
            }
            return View(tag);
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                repoTag.Save(tag);
                this.SetMessage(InfoMessageType.Success, Resources.ProperlyAddedTag);
                return Redirect(Url.Action("Tags", "Admin"));
            }
            return Redirect(Url.Action("EditTag", "Admin"));
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveTag(int tagID)
        {
            if (ModelState.IsValid)
            {
                repoTag.Remove(tagID);
                this.SetMessage(InfoMessageType.Success, Resources.ProperlyRemovedTag);
                return Redirect(Url.Action("Tags", "Admin"));
            }
            return Redirect(Url.Action("EditTag", "Admin"));
        }

        [HttpGet]
        public ViewResult Users(int page = 1)
        {
            ViewBag.Title = Resources.UsersList;
            IList<UserViewModel> users = new List<UserViewModel>();
            foreach (var user in repoUser.Users(enabled: false, page: page, take: PageSize).ToList())
            {
                users.Add(new UserViewModel
                {
                    User = user,
                    PhotoUrlPath = String.Format("{0}UserProfileImage/{1}", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL), string.IsNullOrEmpty(user.PhotoUrl) ? "null.png" : user.PhotoUrl)
                });
            }
            UsersViewModel usersViewModel = new UsersViewModel
            {
                Users = users,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repoUser.Users().Count()
                }
            };
            return View(usersViewModel);
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditUser(int userID)
        {
            ViewBag.Title = Resources.EditUser;
            User editedUser = repoUser.User(userID: userID, enabled: false);
            return View(new UserViewModel
            {
                User = editedUser,
                PhotoUrlPath = String.Format("{0}UserProfileImage/{1}", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL), editedUser.PhotoUrl == null ? "null.png" : editedUser.PhotoUrl)
            });
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                repoUser.Save(user);
                this.SetMessage(InfoMessageType.Success, Resources.UserProperlyEdited);
                return Redirect(Url.Action("Users", "Admin"));
            }
            return Redirect(Url.Action("EditUser", "Admin", new { user.UserID }));
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUser(int userID)
        {
            if (ModelState.IsValid)
            {
                repoUser.Remove(userID);
                this.SetMessage(InfoMessageType.Success, Resources.UserProperlyRemoved);
                return Redirect(Url.Action("Users", "Admin"));
            }
            return Redirect(Url.Action("EditUser", "Admin"));
        }

        [HttpGet]
        public ViewResult Categories()
        {
            ViewBag.Title = Resources.CategoryList;
            IList<Category> category = repoCategory.Categories(enabled: false);
            return View(category);
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditCategory(int categoryID = 0)
        {
            ViewBag.Title = Resources.EditCategory;
            Category category;
            if (categoryID == 0)
            {
                category = new Category
                {
                    CategoryID = 0,
                    Status = 1
                };
                ViewBag.Title = Resources.AddCategory;
            }
            else
            {
                category = repoCategory.Category(categoryID: categoryID);
                ViewBag.Title = Resources.EditArticle;
            }
            return View(category);
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                Category newCategory = new Category
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Order = category.Order,
                    Status = category.Status
                };
                repoCategory.Save(newCategory);
                if (category.CategoryID == 0)
                {
                    this.SetMessage(InfoMessageType.Success, Resources.ProperlyAddedCategory);
                }
                else
                {
                    this.SetMessage(InfoMessageType.Success, Resources.ProperlyEditedCategory);
                }
                return Redirect(Url.Action("Categories", "Admin"));
            }
            return Redirect(Url.Action("EditCategory", "Admin", new { category.CategoryID }));
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCategory(int categoryID)
        {
            if (ModelState.IsValid)
            {
                repoCategory.RemovePermanently(categoryID);
                this.SetMessage(InfoMessageType.Success, Resources.ProperlyRemovedCategory);
            }
            return Redirect(Url.Action("Categories", "Admin"));
        }

        [HttpGet]
        [SetTempDataModelState]
        public ActionResult EditConfiguration()
        {
            ViewBag.Title = Resources.Configuration;
            ConfigurationViewModel configurationViewModel = new ConfigurationViewModel
            {
                ConfigurationKeys = repoConfig.ConfigurationKeys
            };
            return View(configurationViewModel);
        }

        [HttpPost]
        [SetTempDataModelState]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfiguration(ConfigurationKey configurationKey)
        {
            if (ModelState.IsValid)
            {
                ConfigurationKey newConfiguration = new ConfigurationKey
                {
                    Key = configurationKey.Key,
                    Value = configurationKey.Value
                };
                repoConfig.Save(newConfiguration);
                this.SetMessage(InfoMessageType.Success, Resources.ProperlyEditedConfig);
            }
            return Redirect(Url.Action("EditConfiguration", "Admin"));
        }
    }
}