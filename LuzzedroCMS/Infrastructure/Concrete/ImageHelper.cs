using LuzzedroCMS.Domain.Abstract;
using LuzzedroCMS.Domain.Infrastructure.Abstract;
using LuzzedroCMS.Infrastructure.Abstract;
using LuzzedroCMS.Models;
using LuzzedroCMS.WebUI.Infrastructure.Static;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Infrastructure.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private IConfigurationKeyRepository repoConfig;
        private IImageModifier imageModifier;
        private IFtp ftp;
        private ITextBuilder textBuilder;
        private string imageString;
        private const string NULL_IMG = "null.png";

        public HttpPostedFileBase File { get; set; }

        public Image Image { get; set; }

        public string ImageString
        {
            get
            {
                return imageString;
            }
            set
            {
                Image = ConvertToImageFromString(value);
            }
        }

        public ImageHelper(
            IConfigurationKeyRepository configRepo,
            IImageModifier imgModifier,
            IFtp ft,
            ITextBuilder txtBuilder)
        {
            repoConfig = configRepo;
            imageModifier = imgModifier;
            ftp = ft;
            textBuilder = txtBuilder;
        }

        public bool UploadArticleImagesToFtp(string imageName)
        {
            try
            {
                string path = repoConfig.Get(ConfigurationKeyStatic.FTP_PATH);
                Image img320 = imageModifier.ResizeImage(File, 320, 240);
                Image img120 = imageModifier.ResizeImage(File, 120, 90);
                Image img900 = imageModifier.ResizeImage(File, 900, 600);
                ftp.UploadImage(img320, String.Format("{0}ArticleImage/Images320/{1}", path, imageName));
                ftp.UploadImage(img120, String.Format("{0}ArticleImage/Images120/{1}", path, imageName));
                ftp.UploadImage(img900, String.Format("{0}ArticleImage/Images900/{1}", path, imageName));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UploadArticleImagesToLocal(string imageName, HttpServerUtilityBase server)
        {
            try
            {
                Image img320 = imageModifier.ResizeImage(File, 320, 240);
                Image img120 = imageModifier.ResizeImage(File, 120, 90);
                Image img800 = imageModifier.ResizeImage(File, 900, 600);
                var path320 = Path.Combine(server.MapPath(String.Format("{0}ArticleImage/Images320/", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL))), imageName);
                img320.Save(path320, ImageFormat.Jpeg);
                var path120 = Path.Combine(server.MapPath(String.Format("{0}ArticleImage/Images120/", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL))), imageName);
                img120.Save(path120, ImageFormat.Jpeg);
                var path800 = Path.Combine(server.MapPath(String.Format("{0}ArticleImage/Images900/", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL))), imageName);
                img800.Save(path800, ImageFormat.Jpeg);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetImageName(string imageDesc)
        {
            return textBuilder.RemovePolishChars(textBuilder.RemoveSpaces(imageDesc)) + Path.GetExtension(File.FileName);
        }

        public bool IsFileSet()
        {
            return File != null && File.ContentLength > 0;
        }

        public bool IsFileInGoodSize()
        {
            return File.ContentLength > 4000000; //4MB
        }

        public bool IsFileImage()
        {
            string permittedType = "image/jpeg,image/gif,image/png";
            return permittedType.Split(",".ToCharArray()).Contains(File.ContentType);
        }

        public bool IsFtp()
        {
            return Convert.ToBoolean(repoConfig.Get(ConfigurationKeyStatic.USE_FTP_FOR_EXTERNAL_CONTENT));
        }

        public Image ConvertToImageFromString(string imageString)
        {
            byte[] bytes = Convert.FromBase64String(imageString
                    .Replace("data:image/png;base64,", String.Empty)
                    .Replace("data:image/jpg;base64,", String.Empty)
                    .Replace("data:image/bmp;base64,", String.Empty)
                    .Replace("data:image/gif;base64,", String.Empty));

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        public bool UploadUserProfileImagesToFtp(string imageName)
        {
            try
            {
                string path = repoConfig.Get(ConfigurationKeyStatic.FTP_PATH);
                Image img320 = imageModifier.ResizeImage(Image, 320, 240);
                ftp.UploadImage(img320, String.Format("{0}UserProfileImage/{1}", path, imageName));
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        public bool RemoveOldPhotoUserProfileImagesFromFtp(string oldPhoto)
        {
            try
            {
                if (oldPhoto != NULL_IMG)
                {
                    string path = repoConfig.Get(ConfigurationKeyStatic.FTP_PATH);
                    string fullPath = String.Format("{0}UserProfileImage/{1}", path, oldPhoto);
                    bool isOldPhotoExist = ftp.GetFileSize(fullPath) != string.Empty;
                    if (isOldPhotoExist)
                    {
                        ftp.Delete(fullPath);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UploadUserProfileImagesToLocal(string imageName, HttpServerUtilityBase server)
        {
            try
            {
                var path = Path.Combine(server.MapPath(String.Format("{0}UserProfileImage", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL))), imageName);
                Image img = imageModifier.ResizeImage(Image, 320, 240);
                img.Save(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveOldPhotoUserProfileImagesFromLocal(string oldPhoto, HttpRequestBase request)
        {
            try
            {
                string fullPath = request.MapPath(String.Format("{0}UserProfileImage/" + oldPhoto, repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL)));
                if (oldPhoto != NULL_IMG)
                {
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IList<string> GetAllImagesForArticleFromFtp()
        {
            IList<string> rawList = ftp.DirectoryListSimple(repoConfig.Get(ConfigurationKeyStatic.FTP_PATH) + "ArticleImage/Images120").ToList<string>();
            rawList.Remove(string.Empty);
            rawList.Remove("Images120/.");
            rawList.Remove("Images120/..");
            rawList.Remove("Images120/index.php");
            return rawList;
        }

        public IList<string> GetAllImagesForArticleFromLocal(HttpServerUtilityBase server)
        {
            return Directory.GetFiles(server.MapPath(String.Format("{0}ArticleImage/Images120", repoConfig.Get(ConfigurationKeyStatic.CONTENT_EXTERNAL_URL)))).ToList<string>(); ;
        }
    }
}