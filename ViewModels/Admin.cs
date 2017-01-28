using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using M3LMS.Models;
using System.Web.Mvc;

namespace M3LMS.ViewModels
{
    public class Admin
    {
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new Models.DB_A14BC3_M3LMSEntities();
        public List<HtmlString> Links { get; set; }
        //public HtmlString Header = new HtmlString(string.Format("<div class='header-bg' style='{0}'><div id='header-logo' class='logo-bg'><a href='#' class='logo' style='color:green'>{1}<br /> <span>{2}</span></a></div></div>", "background-color:red;float: left;", "pankaj","govind"));
        public static string  BackColor { get; set; }
        public static  string Name1 { get; set; }
        public static string Name2 { get; set; }
        public static string Image { get; set; }
        public string   GetImage(string Email)
        {
            return db.ref_Users.Where(a => a.Email == Email).Select(a => a.ImagePath).FirstOrDefault();
        }


        public   string  LoginUserName(string Email)
        {
           
            return db.ref_Users.Where(a => a.Email == Email).Select(a => a.FirstName + " " + a.LastName).FirstOrDefault();

        }
       // public HtmlString Header = new HtmlString(string.Format("<div class='header' style='{0}'><div class='pull-left logo'><h2>{1}</h2></div><div class='pull-right'><div class='user-account'><a href='#' title='My Account' class='user-profile'><img src='../images/gravatar.jpg' alt='Profile image'><span>{2}</span><i class='glyph-icon icon-angle-down'></i></a></div></div></div>", "background-color:#0093d9;float: left;", "pankaj", "govind"));
        public HtmlString Header
        {
         get   
        {
            //return new HtmlString(string.Format("<div class='header' style='background-color:{0};'><div class='pull-left logo'><h2>{1}</h2></div><div class='pull-right'><div class='user-account'><a href='#' title='My Account' class='user-profile'><img src='../Content/Upload/{2}' alt='Profile image'><span>{3}</span><i class='glyphicon glyphicon-chevron-down'></i></a></div></div></div>", Admin.BackColor, Admin.Name1,Admin.Image, Admin.Name2));
            return new HtmlString(string.Format("<div class='header' style='background-color:{0};'><div class='pull-left logo'><h2>{1}</h2><span style='color: #fff;margin-left: 110px;font-size: 25px;'>{3}</span></div><div class='pull-right'><div class='user-account'><a href='#' title='My Account' class='user-profile'><img src='../Content/Upload/{2}' alt='Profile image'><i class='glyphicon glyphicon-chevron-down'></i></a></div></div></div>", Admin.BackColor, Admin.Name1, Admin.Image, Admin.Name2));
        }
            
        }
        //(string.Format("<div class='header' style='{0}'><div class='pull-left logo'><h2>{1}</h2></div><div class='pull-right'><div class='user-account'><a href='#' title='My Account' class='user-profile'><img src='../images/gravatar.jpg' alt='Profile image'><span>{2}</span><i class='glyph-icon icon-angle-down'></i></a></div></div></div>",BackColor,Name1,Name2));
    }
}