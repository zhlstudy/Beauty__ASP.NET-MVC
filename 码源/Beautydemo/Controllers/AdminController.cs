using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beautydemo.Models;

namespace Beautydemo.Controllers
{
    public class AdminController : Controller
    {
        private BeautyEntities2 db = new BeautyEntities2();

        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "aid,password")] tb_admin ad)
        {
            var admin = db.tb_admin.Where(a => a.aid == ad.aid).FirstOrDefault();
            if (admin == null)
            {
                return Content("<script>alert('管理员不存在');history.go(-1);</script>");
            }
            if (admin.password != ad.password)
            {
                return Content("<script>alert('管理员密码输入错误');history.go(-1);</script>");
            }
            Session["Role"] = "admin";
            Session["IdInfo"] = admin;
            return RedirectToAction("Index", "Product");
        }
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["IdInfo"] == null)
            {
                return Content("<script>alert('管理员登录已过期或未登录，请重新登录！');window.location.href='/Admin/Login';</script>");
            }
            return View(db.tb_admin.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_admin tb_admin = db.tb_admin.Find(id);
            if (tb_admin == null)
            {
                return HttpNotFound();
            }
            return View(tb_admin);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "aid,aname,password,confirmpassword,tel")] tb_admin tb_admin)
        {
            if (ModelState.IsValid)
            {
                db.tb_admin.Add(tb_admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_admin);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_admin tb_admin = db.tb_admin.Find(id);
            if (tb_admin == null)
            {
                return HttpNotFound();
            }
            return View(tb_admin);
        }

        // POST: Admin/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "aid,aname,password,confirmpassword,tel")] tb_admin tb_admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_admin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_admin tb_admin = db.tb_admin.Find(id);
            if (tb_admin == null)
            {
                return HttpNotFound();
            }
            return View(tb_admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_admin tb_admin = db.tb_admin.Find(id);
            db.tb_admin.Remove(tb_admin);
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
