using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnicalSkill.BLL;
using TechnicalSkill.DAL;

namespace TechnicalSkill.Controllers
{
    public class CategoryController : Controller
    {
        //Khai báo phương thức thao tác giao tiếp với lớp data
        private readonly IRepository<Category> categorries;
        private readonly IRepository<Post> posts;
        public CategoryController()
        {
            categorries = new Repository<Category>();
            posts = new Repository<Post>();
        }
        
        //Trang chủ của các menu item
        public ActionResult Index(int Id, int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 6;
            var data = posts.Get(x => x.CategoryId == Id).OrderByDescending(x => (DateTime.Parse(x.Created_At))).ToPagedList(pageNumber, pageSize);
            var data1 = posts.Get(x => x.CategoryId == Id).FirstOrDefault();
            ViewBag.Title = data1.Categories.Name;
            return View(data);
        }
    
        //Lấy ra category
        public ActionResult GetCategory()
        {
            var data = categorries.Get();

            return Json(new
            {
                data = data,
                statusCode = 200,
                message = "Succces",
            }, JsonRequestBehavior.AllowGet);
        }
    }
}