using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace LuzzedroCMS.Infrastructure.Abstract
{
    public interface IImageHelper
    {
        HttpPostedFileBase File { get; set; }
        Image Image { get; set; }
        string ImageString { get; set; }
        bool UploadArticleImagesToFtp(string imageName);
        bool UploadArticleImagesToLocal(string imageName, HttpServerUtilityBase server);
        string GetImageName(string imageDesc);
        bool IsFileSet();
        bool IsFileInGoodSize();
        bool IsFileImage();
        bool IsFtp();
        Image ConvertToImageFromString(string imageString);
        bool UploadUserProfileImagesToFtp(string imageName);
        bool RemoveOldPhotoUserProfileImagesFromFtp(string oldPhoto);
        bool UploadUserProfileImagesToLocal(string imageName, HttpServerUtilityBase server);
        bool RemoveOldPhotoUserProfileImagesFromLocal(string oldPhoto, HttpRequestBase request);
        IList<string> GetAllImagesForArticleFromFtp();
        IList<string> GetAllImagesForArticleFromLocal(HttpServerUtilityBase server);
    }
}