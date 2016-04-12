using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Helper
{
    public static class UriExtension
    {
        public static Uri Append(this Uri baseUri, params string[] paths)
        {
            return new Uri(paths.Aggregate(baseUri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }
    }
}