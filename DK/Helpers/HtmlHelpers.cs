//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

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