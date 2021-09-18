using System;
using System.Collections.Generic;
using System.Linq;
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
            return View(posts.Get().OrderByDescending(x => x.Created_At));
        }
    }
}