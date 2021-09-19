using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TechnicalSkill.BLL;
using TechnicalSkill.Controllers;
using TechnicalSkill.DAL;
using TechnicalSkill.DAL.Utils;

namespace TechnicalSkill.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        //Khai báo phương thức thao tác giao tiếp với lớp data
        private readonly IRepository<User> users;

        //Khởi tạo
        public UserController()
        {
            users = new Repository<User>();
        }
        public ActionResult Index()
        {
            return View();
        }

        //Lấy tất cả User
        public ActionResult GetData()
        {
            var data = users.Get();
            return Json(new
            {
                data = data.ToList(),
                message = "Success",
                statusCode = 200
            }, JsonRequestBehavior.AllowGet);
        }

        //Tìm user theo Id
        public ActionResult FindId(int id)
        {
            var data = users.Get(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Sửa user item
        [HttpPost]
        public ActionResult Edit(User user)
        {
            var user1 = users.Get(user.Id);
            var errors = new Dictionary<string, string>();
            var check = true;
            if (!ModelState.IsValid)
                return Json(new
                {
                    statusCode = 402,
                    message = "Error",
                    data = user
                }, JsonRequestBehavior.AllowGet);

            if (users.CheckDuplicate(x => x.Name == user.Name && x.Id != user1.Id))
            {
                check = false;
                errors.Add("Name", "Your Name has been used!");
            }

            if (users.CheckDuplicate(x => x.UserName == user.UserName && x.Id != user1.Id))
            {
                check = false;
                errors.Add("UserName", "Your UserName has been used!");
            }

            if (ModelState.IsValid && check)
            {
                var user3 = users.Get(user.Id);
                user3.Name = user.Name;
                user3.UserName = user.UserName;
               

                if (!users.Update(user3))
                {
                    return Json(new
                    {
                        statusCode = 400,
                        message = "Error",
                        data = "Error"
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    statusCode = 200,
                    message = "Success"
                }, JsonRequestBehavior.AllowGet);
            }

            foreach (var k in ModelState.Keys)
                foreach (var err in ModelState[k].Errors)
                {
                    var key = Regex.Replace(k, @"(\w+)\.(\w+)", @"$2");
                    if (!errors.ContainsKey(key))
                        errors.Add(key, err.ErrorMessage);
                }

            return Json(new
            {
                statusCode = 400,
                message = "Error",
                data = errors
            }, JsonRequestBehavior.AllowGet);
        }

        //Thêm user item
        [HttpPost]
        public ActionResult Create(User user)
        {
            var errors = new Dictionary<string, string>();
            var check = true;
            if (!ModelState.IsValid)
                return Json(new
                {
                    statusCode = 402,
                    message = "Error",
                    data = user
                }, JsonRequestBehavior.AllowGet);

            if (users.CheckDuplicate(x => x.Name == user.Name))
            {
                check = false;
                errors.Add("Name", "Your Name has been used!");
            }

            if (users.CheckDuplicate(x => x.UserName == user.UserName))
            {
                check = false;
                errors.Add("UserName", "Your UserName has been used!");
            }

            if (ModelState.IsValid && check)
            {
                var user1 = new User
                {
                    Name = user.Name,
                    UserName = user.UserName,
                    Password = Utils.HashPassword("123456"),
                    Rold = "User"
            };

                users.Add(user1);
                return Json(new
                {
                    statusCode = 200,
                    message = "Success"
                }, JsonRequestBehavior.AllowGet);
            }

            foreach (var k in ModelState.Keys)
                foreach (var err in ModelState[k].Errors)
                {
                    var key = Regex.Replace(k, @"(\w+)\.(\w+)", @"$2");
                    if (!errors.ContainsKey(key))
                        errors.Add(key, err.ErrorMessage);
                }

            return Json(new
            {
                statusCode = 400,
                message = "Error",
                data = errors
            }, JsonRequestBehavior.AllowGet);
        }

        //Xoá user item
        public ActionResult Delete(int id)
        {
            if (users.Delete(id))
            {
                return Json(new
                {
                    statusCode = 200,
                    message = "Success"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                statusCode = 400,
                message = "Error"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}