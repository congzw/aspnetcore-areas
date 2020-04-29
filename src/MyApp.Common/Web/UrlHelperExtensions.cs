using Microsoft.AspNetCore.Mvc;

namespace MyApp.Common.Web
{
    public static class UrlHelperExtensions
    {
        public static string AreaContent(this IUrlHelper urlHelper, string contentPath, string area)
        {
            //~/Content/css/foo.css" => ~/Areas/Demo/Content/css/foo.css"
            var areaContentPath = "~/Areas/" + area + "/" + contentPath.TrimStart('~').TrimStart('/');
            return urlHelper.Content(areaContentPath);
        }
    }
}
