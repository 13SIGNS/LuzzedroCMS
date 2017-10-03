using System.Drawing;

namespace LuzzedroCMS.Infrastructure.Abstract
{
    public interface IFtp
    {
        void Download(string remoteFile, string localFile);
        void Upload(string remoteFile, string localFile);
        void UploadImage(Image img, string fileName);
        void Delete(string deleteFile);
        void Rename(string currentFileNameAndPath, string newFileName);
        void CreateDirectory(string newDirectory);
        string GetFileCreatedDateTime(string fileName);
        string GetFileSize(string fileName);
        string[] DirectoryListSimple(string directory);
        string[] DirectoryListDetailed(string directory);
    }
}