using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using M3LMS.DTO;

namespace M3LMS.Helper
{
    public class Config
    {
        //const HtmlString Section_Html_Not_Found_Text = "Page Content Not Found";
        public static HtmlString  GetSectionHtml(string controller,string action,string section)
        {
            var url = GetUri(controller, action);
            var item = GetSectionItem(section, url);
            //if (item == null)
            //    return string.Format(Section_Html_Not_Found_Text, url, section);

            return new HtmlString(item.Text);
        }
        public static string GetUri(string controller,string action)
        {
            return string.Format("{0}/{1}", controller, action);
        }
        public static Section GetSectionItem(string section,string uri)
        {
            return MvcApplication.PageSections.First();

        }
    }
}