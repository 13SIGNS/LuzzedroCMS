using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Entities;
using LuzzedroCMS.Domain.Infrastructure;
using LuzzedroCMS.Infrastructure.Concrete;
using LuzzedroCMS.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using LuzzedroCMS.WebUI.Properties;

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
        private TextBuilder textBuilder;
        private User user;

        public AdminController(IArticleRepository articleRepo, ICategoryRepository categoryRepo, ICommentRepository commentRepo, IUserRepository userRepo, ITagRepository tagRepo)
        {
            repoArticle = articleRepo;
            repoCategory = categoryRepo;
            repoComment = commentRepo;
            repoUser = userRepo;
            repoTag = tagRepo;
            var userx = User;
            user = repoUser.UserByEmail(System.Web.HttpContext.Current.User.Identity.Name);
            textBuilder = new TextBuilder();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(user);
        }

        [HttpPost]
        public ActionResult RemoveArticle(int articleID)
        {
            if (ModelState.IsValid)
            {
                repoArticle.RemovePermanently(articleID);
                TempData["Info.success"] = Resources.ProperlyRemovedArticle;
            }
            return Redirect(Url.Action("Articles", "Admin"));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditArticle(string url = "")
        {
            IQueryable<Tag> tags = repoTag.TagsEnabled;
            IQueryable<Category> categories = repoCategory.CategoriesEnabled;
            Article article;
            IQueryable<int> selectedTagIDs;
            int selectedCategoryID;

            if (url == "")
            {
                article = new Article
                {
                    DatePub = DateTime.Now,
                    DateExp = DateTime.Now.AddYears(50),
                    Status = 1
                };
                ViewBag.Title = Resources.AddingArticle;
                selectedTagIDs = null;
                selectedCategoryID = 0;
            }
            else
            {
                article = repoArticle.ArticleByUrl(url);
                ViewBag.Title = Resources.EditArticle;
                selectedTagIDs = repoTag.TagIDsEnabledByArticleID(article.ArticleID);
                selectedCategoryID = article.CategoryID;
            }

            return View(new EditArticleViewModel
            {
                Article = article,
                Categories = categories,
                Tags = tags,
                SelectedCategoryID = selectedCategoryID,
                SelectedTagIDs = selectedTagIDs
            });
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult EditArticle(Article article, int[] SelectedTagIDs, string returnUrl, string ExistingImageName)
        {
            if (ModelState.IsValid)
            {
                Article updatedArticle = new Article
                {
                    ArticleID = article.ArticleID,
                    CategoryID = article.CategoryID,
                    UserID = user.UserID,
                    ImageName = ExistingImageName,
                    ImageDesc = article.ImageDesc,
                    DatePub = article.DatePub,
                    DateExp = article.DateExp,
                    Title = article.Title,
                    Lead = article.Lead,
                    Content = article.Content,
                    Source = article.Source,
                    Status = article.Status
                };
                int articleID = repoArticle.Save(updatedArticle);
                
                if (SelectedTagIDs != null)
                {
                    foreach (int selectedTagID in SelectedTagIDs)
                    {
                        repoArticle.AddTagToArticle(articleID, selectedTagID);
                    }
                }
                if (article.ArticleID == 0)
                {
                    TempData["Info.success"] = Resources.ProperlyAddedArticle;
                }
                else
                {
                    TempData["Info.success"] = Resources.CorrectlyChanged;
                }
                return Redirect(Url.Action("Articles", "Admin"));
            }
            else
            {
                return Redirect(Url.Action("EditArticle", "Admin", new { article.Url }));
            }

        }

        [HttpGet]
        public ViewResult Articles()
        {
            ViewBag.Title = Resources.ListOfArticles;
            IQueryable<Article> articles = repoArticle.Articles;
            return View(articles);
        }

        [HttpGet]
        public ViewResult Comments()
        {
            ViewBag.Title = Resources.ListOfComments;
            IQueryable<Comment> comments = repoComment.CommentsEnabled;
            return View(comments);
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditComment(int commentID)
        {
            ViewBag.Title = Resources.EditComment;
            Comment comment = repoComment.CommentByID(commentID);
            return View(comment);
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult EditComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                repoComment.Save(comment);
                TempData["Info.success"] = Resources.CorrectlyEditComment;
                return Redirect(Url.Action("Comments", "Admin"));
            }
            return Redirect(Url.Action("EditComment", "Admin", new { comment.CommentID }));
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditPhoto()
        {
            ViewBag.Title = Resources.AddingAnImage;
            return View(new ImageViewModel { });
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult EditPhoto(ImageViewModel imageViewModel)
        {
            if (ModelState.IsValid)
            {
                string imageInfo = "";
                string imageName = "";
                if (Request.Files.Count > 0)
                {
                    imageName = textBuilder.RemovePolishChars(textBuilder.RemoveSpaces(imageViewModel.ImageDesc));
                    int permittedSizeInBytes = 4000000;//4mb
                    string permittedType = "image/jpeg,image/gif,image/png";
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > permittedSizeInBytes)
                        {
                            imageInfo = Resources.FileCannotBeMore;
                        }
                        else
                        {
                            if (!permittedType.Split(",".ToCharArray()).Contains(file.ContentType))
                            {
                                imageInfo = Resources.InvalidFileType;
                            }
                            else
                            {
                                ImageModifier imageModifier = new ImageModifier();
                                String ext = Path.GetExtension(file.FileName);
                                imageName += ext;
                                imageName = repoArticle.GetUniqueImageTitle(imageName);
                                var path320 = Path.Combine(Server.MapPath("~/Content/ArticleImage/Images320/"), imageName);
                                Image img320 = imageModifier.ResizeImage(file, 320, 240);
                                img320.Save(path320, ImageFormat.Jpeg);

                                var path120 = Path.Combine(Server.MapPath("~/Content/ArticleImage/Images120/"), imageName);
                                Image img120 = imageModifier.ResizeImage(file, 120, 90);
                                img120.Save(path120, ImageFormat.Jpeg);

                                var path800 = Path.Combine(Server.MapPath("~/Content/ArticleImage/Images900/"), imageName);
                                Image img800 = imageModifier.ResizeImage(file, 900, 600);
                                img800.Save(path800, ImageFormat.Jpeg);
                            }
                        }
                    }
                    else
                    {
                        imageInfo = Resources.FileNotHaveContent;
                    }
                }
                if(imageInfo != "")
                {
                    imageInfo = Resources.NoAddedImage;
                    TempData["Info.danger"] = imageInfo;
                    return Redirect(Url.Action("EditPhoto", "Admin"));
                }
                TempData["Info.success"] = Resources.ProperlyAddedImage;
                return Redirect(Url.Action("EditPhoto", "Admin"));
            }
            return Redirect(Url.Action("EditPhoto", "Admin"));
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult RemoveComment(int commentID)
        {
            if (ModelState.IsValid)
            {
                repoComment.Remove(commentID);
                TempData["Info.success"] = Resources.ProperlyRemovedComment;
                return Redirect(Url.Action("Comments", "Admin"));
            }
            return Redirect(Url.Action("EditComment", "Admin"));
        }

        [HttpGet]
        public ViewResult Tags()
        {
            ViewBag.Title = Resources.Tags;
            IQueryable<Tag> tags = repoTag.TagsEnabled;
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
                tag = repoTag.TagByID(tagID);
            }
            return View(tag);
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult EditTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                repoTag.Save(tag);
                TempData["Info.success"] = Resources.ProperlyAddedTag;
                return Redirect(Url.Action("Tags", "Admin"));
            }
            return Redirect(Url.Action("EditTag", "Admin"));
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult RemoveTag(int tagID)
        {
            if (ModelState.IsValid)
            {
                repoTag.Remove(tagID);
                TempData["Info.success"] = Resources.ProperlyRemovedTag;
                return Redirect(Url.Action("Tags", "Admin"));
            }
            return Redirect(Url.Action("EditTag", "Admin"));
        }

        [HttpGet]
        public ViewResult Users()
        {
            ViewBag.Title = Resources.UsersList;
            IQueryable<User> users = repoUser.UsersTotal;
            return View(users);
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ViewResult EditUser(int userID)
        {
            ViewBag.Title = Resources.EditUser;
            User user = repoUser.UserByID(userID);
            return View(user);
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                repoUser.Save(user);
                TempData["Info.success"] = Resources.UserProperlyEdited;
                return Redirect(Url.Action("Users", "Admin"));
            }
            return Redirect(Url.Action("EditUser", "Admin", new { user.UserID }));
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult RemoveUser(int userID)
        {
            if (ModelState.IsValid)
            {
                repoUser.Remove(userID);
                TempData["Info.success"] = Resources.UserProperlyRemoved;
                return Redirect(Url.Action("Users", "Admin"));
            }
            return Redirect(Url.Action("EditUser", "Admin"));
        }

        [HttpGet]
        public ViewResult Categories()
        {
            ViewBag.Title = Resources.CategoryList;
            IQueryable<Category> category = repoCategory.CategoriesTotal;
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
                category = repoCategory.CategoryByID(categoryID);
                ViewBag.Title = Resources.EditArticle;
            }
            return View(category);
        }

        [HttpPost]
        [SetTempDataModelState]
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
                    TempData["Info.success"] = Resources.ProperlyAddedCategory;
                }
                else
                {
                    TempData["Info.success"] = Resources.ProperlyEditedCategory;
                }
                return Redirect(Url.Action("Categories", "Admin"));
            }
            return Redirect(Url.Action("EditCategory", "Admin", new { category.CategoryID }));
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult RemoveCategory(int categoryID)
        {
            if (ModelState.IsValid)
            {
                repoCategory.RemovePermanently(categoryID);
                TempData["Info.success"] = Resources.ProperlyRemovedCategory;
            }
            return Redirect(Url.Action("Categories", "Admin"));
        }
    }
}