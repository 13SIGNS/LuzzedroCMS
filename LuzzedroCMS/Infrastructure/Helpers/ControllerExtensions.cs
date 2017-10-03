using LuzzedroCMS.WebUI.Infrastructure.Enums;
using System.Web.Mvc;

namespace LuzzedroCMS.WebUI.Infrastructure.Helpers
{
    public static class ControllerExtensions
    {
        public static void SetMessage(this Controller instance, InfoMessageType infoMessageType, string value)
        {
            string infoMessage = string.Empty;
            switch (infoMessageType)
            {
                case InfoMessageType.Success:
                    infoMessage = "Info.success";
                    break;
                case InfoMessageType.Danger:
                    infoMessage = "Info.danger";
                    break;
                case InfoMessageType.Warning:
                    infoMessage = "Info.warning";
                    break;
                case InfoMessageType.Info:
                    infoMessage = "Info.info";
                    break;
            }
            instance.TempData[infoMessage] = value;
        }
    }
}