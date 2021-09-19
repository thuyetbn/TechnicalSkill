using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnicalSkill.DAL;
using TechnicalSkill.BLL;
using TechnicalSkill.DAL.Utils;
using RestSharp;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using TechnicalSkill.Models;
using static TechnicalSkill.Models.RootModel;
using System.Text.RegularExpressions;
using System.IO;
using PagedList;

namespace TechnicalSkill.Controllers
{
    public class HomeController : Controller
    {
        //Khai báo phương thức thao tác giao tiếp với lớp data
        private readonly IRepository<User> users;
        private readonly IRepository<Post> posts;
        private readonly IRepository<Category> categories;
        public HomeController()
        {
            users = new Repository<User>();
            posts = new Repository<Post>();
            categories = new Repository<Category>();
        }

        //Trang chủ
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 6;

            var data = posts.Get().OrderByDescending(x => x.Created_At).ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        //Xem chi tiết bài viết
        public ActionResult DetailPost(int Id)
        {
            return View(posts.Get(Id));
        }
        
        //Thêm lượt viet cho bài viết
        public ActionResult Views(int Id)
        {
            var postView = posts.Get(Id);
            postView.Views += 1;
            posts.Update(postView);
            return Json(new
            {
                statusCode = 200,
                message = "Success",
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Login()
        {
            return View();
        }
        
        //Xử lý Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var errors = new Dictionary<string, string>();
            var obj = users.Get(x => x.UserName == username).FirstOrDefault();

            if (Utils.IsNullOrEmpty(obj))
            {
                errors.Add("UserName", "UserName is not exists!");

                return Json(new
                {
                    statusCode = 400,
                    message = "Error",
                    data = errors
                }, JsonRequestBehavior.AllowGet);
            }
            if (!Utils.ValidatePassword(password, obj.Password))
            {
                errors.Add("Password", "Your password is wrong!");
                return Json(new
                {
                    statusCode = 400,
                    message = "Error",
                    data = errors
                }, JsonRequestBehavior.AllowGet);
            }
            Session["user"] = obj;
            return Json(new
            {
                statusCode = 200,
                message = "Success",
                url = "Home/Index"
            }, JsonRequestBehavior.AllowGet);
        }

    }
}