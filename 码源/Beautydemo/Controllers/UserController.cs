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
    public class UserController : Controller
    {
        private BeautyEntities2 db = new BeautyEntities2();

        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "uid,password")] tb_user tb_user)
        {
            var user = db.tb_user.Where(a => a.uid == tb_user.uid).FirstOrDefault();
            if (user == null)
            {
                return Content("<script>alert('用户名不存在');history.go(-1);</script>");
            }
            if (user.password != tb_user.password)
            {
                return Content("<script>alert('用户名或密码输入错误');history.go(-1);</script>");
            }
            Session["Role"] = "user";
            Session["IdInfo"] = user;
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Index()
        {
            if (Session["Role"] == null)
            {
                return Content("<script>alert('用户登录已过期或未登录，请重新登录！');window.location.href='/User/Login';</script>");
            }
            if (Session["Role"].ToString() == "admin")
            {
                return View(db.tb_user.ToList());
            }
            else
            {
                int uid = ((tb_user)Session["IdInfo"]).uid;
                return View(db.tb_user.Where(a => a.uid == uid).ToList());
            }
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            return View(tb_user);
        }

        // GET: User/Create
        public ActionResult Register()
        {
            return View();
        }
        // POST: User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "uid,name,password,confirmpassword,address,tel,emil")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                db.tb_user.Add(tb_user);
                db.SaveChanges();
                return Login(tb_user);
            }

            return View();
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            return View(tb_user);
        }

        // POST: User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "uid,name,password,confirmpassword,address,tel,emil")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            return View(tb_user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_user tb_user = db.tb_user.Find(id);
            db.tb_user.Remove(tb_user);
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

        public ActionResult LogOut()
        {
            Session["Role"] = null;
            Session["IdInfo"] = null;
            return RedirectToAction("Index", "Home");
        }

      

    }
}
