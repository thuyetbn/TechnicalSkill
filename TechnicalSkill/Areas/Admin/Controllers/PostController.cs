using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TechnicalSkill.BLL;
using TechnicalSkill.DAL;

namespace TechnicalSkill.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        private readonly IRepository<Post> posts;
        public PostController()
        {
            posts = new Repository<Post>();
        }
        // GET: Admin/Post
        public ActionResult Index()
        {
            return View();
        }
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