using System.Drawing;
using System.Web;

namespace LuzzedroCMS.Infrastructure.Abstract
{
    public interface IImageModifier
    {
        Image ResizeImage(HttpPostedFileBase httpPostedFileBase, int maxWidth, int maxHeight);
        Image ResizeImage(Image img, int maxWidth, int maxHeight);
    }
}