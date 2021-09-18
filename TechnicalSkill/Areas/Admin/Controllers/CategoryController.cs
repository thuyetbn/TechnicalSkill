using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TechnicalSkill.Areas.Admin.Data;
using TechnicalSkill.BLL;
using TechnicalSkill.DAL;
using TechnicalSkill.DAL.Utils;

namespace TechnicalSkill.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> categories;
        private readonly IRepository<Post> posts;
        public CategoryController()
        {
            categories = new Repository<Category>();
            posts = new Repository<Post>();
        }
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExportReport(int Id)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "Report.rpt"));
            rd.SetDataSource(posts.Get(x => x.CategoryId == Id).OrderByDescending(x => x.Created_At).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Report_PostViews.pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetMorePost(int Id)
        {
            var dataCat = categories.Get(Id);
            var job = new Job();
            var listposts = job.ConvertRss(dataCat);
            posts.AddRange(listposts);
            var count = listposts.Count;
            return Json(new
            {
                data = listposts.Count(),
                message = "Success",
                statusCode = 200
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int Id)
        {
            var x = categories.Get(Id);
            if (x == null)
            {
                return View();
            }

            return View(x);
        }
        public ActionResult GetData()
        {
            var data = categories.Get();
            return Json(new
            {
                data = data.ToList(),
                message = "Success",
                statusCode = 200
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPost(int Id)
        {
            var data = posts.Get(x=>x.CategoryId == Id).Select(x => new CategoryPostViewModels(x));
            return Json(new
            {
                data = data.ToList(),
                message = "Success",
                statusCode = 200
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FindId(int id)
        {
            var data = categories.Get(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            var cat = categories.Get(category.Id);
            var errors = new Dictionary<string, string>();
            var check = true;
            if (!ModelState.IsValid)
                return Json(new
                {
                    statusCode = 402,
                    message = "Error",
                    data = category
                }, JsonRequestBehavior.AllowGet);

            if (categories.CheckDuplicate(x => x.Name == category.Name && x.Id != cat.Id))
            {
                check = false;
                errors.Add("Name", "Your Name has been used!");
            }

            if (categories.CheckDuplicate(x => x.Description == category.Description && x.Id != cat.Id))
            {
                check = false;
                errors.Add("Description", "Your Description has been used!");
            }

            if (categories.CheckDuplicate(x => x.LinkRSS == category.LinkRSS && x.Id != cat.Id))
            {
                check = false;
                errors.Add("LinkRSS", "Your LinkRSS has been used!");
            }

            if (ModelState.IsValid && check)
            {
                var cat2 = categories.Get(category.Id);
                cat2.Name = category.Name;
                cat2.Description = category.Description;
                cat2.LinkRSS = category.LinkRSS;

                if (!categories.Update(cat2))
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
        [HttpPost]
        public ActionResult Create(Category category)
        {
            var errors = new Dictionary<string, string>();
            var check = true;
            if (!ModelState.IsValid)
                return Json(new
                {
                    statusCode = 402,
                    message = "Error",
                    data = category
                }, JsonRequestBehavior.AllowGet);

            if (categories.CheckDuplicate(x => x.Name == category.Name))
            {
                check = false;
                errors.Add("Name", "Your Name has been used!");
            }

            if (categories.CheckDuplicate(x => x.Description == category.Description))
            {
                check = false;
                errors.Add("Description", "Your Description has been used!");
            }
            if (categories.CheckDuplicate(x => x.LinkRSS == category.LinkRSS))
            {
                check = false;
                errors.Add("LinkRSS", "Your LinkRSS has been used!");
            }

            if (ModelState.IsValid && check)
            {
                var cat = new Category
                {
                    Name = category.Name,
                    Description = category.Description,
                    LinkRSS = category.LinkRSS
                };

                categories.Add(cat);
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
        public ActionResult Delete(int id)
        {
            if (categories.Delete(id))
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