using System.Web.Optimization;

namespace bundles
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = true;

            var bundleJquery = new ScriptBundle("~/bundles/jquery").Include("~/Content/Scripts/jquery-2.2.4.js");
            bundleJquery.CdnFallbackExpression = "window.jQuery";
            bundleJquery.CdnPath = "https://ajax.googleapis.com/ajax/libs/jquery/2.2.4/jquery.min.js";
            bundles.Add(bundleJquery);

            var bundleBootstrap = new ScriptBundle("~/bundles/bootstrap").Include("~/Content/Scripts/bootstrap.js");
            bundleBootstrap.CdnFallbackExpression = "$.fn.modal";
            bundleBootstrap.CdnPath = "https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/bootstrap.min.js";
            bundles.Add(bundleBootstrap);

            var bundleJqueryValidate = new ScriptBundle("~/bundles/jqueryvalidate").Include("~/Content/Scripts/jquery.validate.js");
            bundleJqueryValidate.CdnFallbackExpression = "$.fn.validate";
            bundleJqueryValidate.CdnPath = "https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.16.0/jquery.validate.min.js";
            bundles.Add(bundleJqueryValidate);

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidateunobtrusive").Include("~/Content/Scripts/jquery.validate.unobtrusive.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajax").Include("~/Content/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/cookie").Include("~/Content/Scripts/Internal/cookieconsent.js"));
            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include("~/Content/Scripts/tinymce/tinymce.js"));
            bundles.Add(new ScriptBundle("~/bundles/chosen").Include("~/Content/Scripts/chosen.jquery.js"));
            bundles.Add(new ScriptBundle("~/bundles/forms").Include("~/Content/Scripts/Internal/forms.js"));
            bundles.Add(new ScriptBundle("~/bundles/highlight").Include("~/Content/Scripts/highlight.js"));
            bundles.Add(new ScriptBundle("~/bundles/croppie").Include("~/Content/Scripts/croppie.js"));
            bundles.Add(new ScriptBundle("~/bundles/login").Include("~/Content/Scripts/Internal/login.js"));
            bundles.Add(new ScriptBundle("~/bundles/editarticle").Include("~/Content/Scripts/Internal/editarticle.js"));
            bundles.Add(new ScriptBundle("~/bundles/editaccount").Include("~/Content/Scripts/Internal/editaccount.js"));
            bundles.Add(new ScriptBundle("~/bundles/editnick").Include("~/Content/Scripts/Internal/editnick.js"));
            bundles.Add(new ScriptBundle("~/bundles/article").Include("~/Content/Scripts/Internal/article.js"));
            bundles.Add(new StyleBundle("~/bundles/style/bootstrap").Include("~/Content/Styles/bootstrap.css"));
            bundles.Add(new StyleBundle("~/bundles/style/index").Include("~/Content/Styles/index.css"));
            bundles.Add(new StyleBundle("~/bundles/style/chosen").Include("~/Content/Styles/Chosen.css"));
            bundles.Add(new StyleBundle("~/bundles/style/highlight").Include("~/Content/Styles/highlight-atelier-cave-dark.css"));
            bundles.Add(new StyleBundle("~/bundles/style/croppie").Include("~/Content/Styles/croppie.css"));
            bundles.Add(new StyleBundle("~/bundles/style/fontawesome").Include("~/Content/Styles/font-awesome.css"));
        }
    }
}