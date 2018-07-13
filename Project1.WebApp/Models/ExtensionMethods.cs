using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public static class ExtensionMethods
    {
        public static HtmlString ToHtmlString(this String str)
        {
            return new HtmlString(str);
        }
    }
}
