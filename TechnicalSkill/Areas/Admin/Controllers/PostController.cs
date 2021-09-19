using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TechnicalSkill.BLL;
using TechnicalSkill.Controllers;
using TechnicalSkill.DAL;

namespace TechnicalSkill.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        //Khai báo phương thức thao tác giao tiếp với lớp data
        private readonly IRepository<Post> posts;

        //Khởi tạo
        public PostController()
        {
            posts = new Repository<Post>();
        }
        // GET: Admin/Post
        public ActionResult Index()
        {
            return View();
        }

        // Lấy tất cả bài viết
        public ActionResult GetData()
        {
            var data = posts.Get().Select(x => new CategoryPostViewModels(x));
            return Json(new
            {
                data = data.ToList(),
                message = "Success",
                statusCode = 200
            }, JsonRequestBehavior.AllowGet);
        }

        // Xoá bài viết theo Id
        public ActionResult Delete(int id)
        {
            if (posts.Delete(id))
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