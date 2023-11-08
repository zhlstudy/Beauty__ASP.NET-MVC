using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beautydemo.Models;
using PagedList;

namespace Beautydemo.Controllers
{
    public class ProductController : Controller
    {
        private BeautyEntities2 db = new BeautyEntities2();

        // GET: Product
        public ActionResult Index(int? page)
        {
            var productList = from s in db.tb_product select s;
            productList = productList.OrderByDescending(a => a.salenums);
            int pageNumber = page ?? 1;
            //每页显示多少条
            int pageSize = 4;
           // int pageSize = productList.Count();
            IPagedList<tb_product> productsPageList = productList.ToPagedList(pageNumber, pageSize);
            return View(productsPageList);
        }

        



        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_product tb_product = db.tb_product.Find(id);
            if (tb_product == null)
            {
                return HttpNotFound();
            }
            return View(tb_product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pid,pname,photo,price,pnums,salenums,state,mess")] tb_product tb_product)
        {

            HttpPostedFile hpf = System.Web.HttpContext.Current.Request.Files["file"];
            var fileName = hpf.FileName;
            var filePath = Server.MapPath(string.Format("~/{0}", "image"));
            string name = hpf.FileName;
            string originalFileName = "";
            if (name.Contains("."))
            {
                originalFileName = Path.GetFileNameWithoutExtension(hpf.FileName.ToString());
            }
            string allowExtension = ".jpg|.jpeg|.png|.doc|.docx|.pdf|.xls|.xlsx|.mp4|.bmp|";
            string fileExtension = Path.GetExtension(name);
            if(!allowExtension.Contains(fileExtension.ToLower()))
            {
                return Json(new { code = false, message = "文件格式不合法" });
            }

            
            string path = AppDomain.CurrentDomain.BaseDirectory + "image/";
            string fullpath = path + originalFileName + fileExtension;
            if (!System.IO.Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

           
            //if (System.IO.File.Exists(fullpath))
            //{
            //    System.IO.File.Delete(fullpath);
            //}
            hpf.SaveAs(fullpath);
            tb_product.photo = "/image/" + originalFileName + fileExtension;
            if (ModelState.IsValid)
            {
                db.tb_product.Add(tb_product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            hpf.SaveAs(Path.Combine(filePath, fileName));
            ModelState.Clear();
            return View(tb_product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_product tb_product = db.tb_product.Find(id);
            if (tb_product == null)
            {
                return HttpNotFound();
            }
            return View(tb_product);
        }

        // POST: Product/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pid,pname,photo,price,pnums,salenums,state,mess")] tb_product tb_product)
        {
            HttpPostedFile hpf = null;
            try
            {
                hpf = System.Web.HttpContext.Current.Request.Files[0];
            }
            catch (Exception)
            {
            }
            if (hpf.FileName != "")
            {
                string name = hpf.FileName;
                string originalFileName = "";//原始文件名
                if (name.Contains("."))
                {
                    originalFileName = Path.GetFileNameWithoutExtension(hpf.FileName.ToString());
                }
                string allowExtension = ".jpg|.jpeg|.png|.doc|.docx|.pdf|.xls|.xlsx|.mp4";
                string fileExtension = Path.GetExtension(name);//文件扩展名
                if (!allowExtension.Contains(fileExtension.ToLower()))
                {
                    return Json(new { code = false, message = "文件格式不合法" });
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "image/";
                if (!System.IO.Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fullpath = path + originalFileName + fileExtension;
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.File.Delete(fullpath);
                }
                hpf.SaveAs(fullpath);
                tb_product.photo = "/image/" + originalFileName + fileExtension;
            }
            else
            {
                var productObj = db.tb_product.AsNoTracking().FirstOrDefault(a => a.pid == tb_product.pid);
                tb_product.photo = productObj.photo;
            }
            if (ModelState.IsValid)
            {
                db.Entry(tb_product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_product tb_product = db.tb_product.Find(id);
            if (tb_product == null)
            {
                return HttpNotFound();
            }
            return View(tb_product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_product tb_product = db.tb_product.Find(id);
            db.tb_product.Remove(tb_product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
