using System;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DasKlub.Lib.Configs;

namespace DasKlub.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString CSSClassValidationMessageFor<TModel, TProperty>
            (this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            if (helper == null) throw new ArgumentNullException("helper");
            if (expression == null) throw new ArgumentNullException("expression");
            return helper.ValidationMessageFor(expression, null, new {@class = "error"});
        }


        public static MvcHtmlString QueryAsHiddenFields(this HtmlHelper htmlHelper)
        {
            var result = new StringBuilder();
            var query = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            foreach (var key in query.Keys.Cast<string>().Where(key => key != null))
            {
                result.Append(htmlHelper.Hidden(key, query[key]).ToHtmlString());
            }
            return MvcHtmlString.Create(result.ToString());
        }


        public static MvcHtmlString S3ContentPath(this HtmlHelper helper, string filePath)
        {
            string bucket = AmazonCloudConfigs.AmazonBucketName;
            string url = string.Format(AmazonCloudConfigs.AmazonCloudDomain, bucket, filePath);
            return new MvcHtmlString(url);
        }
    }
}