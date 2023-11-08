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
    public class OrderdetailsController : Controller
    {
        private BeautyEntities2 db = new BeautyEntities2();

        // GET: Orderdetails
        public ActionResult Index(int ?id)
        {
            if (Session["IdInfo"] == null)
            {
                return Content("<script>alert('用户登录已过期或未登录，请重新登录！');window.location.href='/User/Login';</script>");
            }
            if (Session["Role"].ToString() == "user")
            {
                var user = Session["IdInfo"] as tb_user;
                return View(db.tb_orderdetails.Where(a => a.name == user.uid.ToString()).Where(b => b.oid == id).ToList());
            }
            var tb_orderdetails = db.tb_orderdetails.Include(t => t.tb_orders).Include(t => t.tb_product);
            return View(tb_orderdetails.Where(b => b.oid == id).ToList());
        }

        // GET: Orderdetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orderdetails tb_orderdetails = db.tb_orderdetails.Find(id);
            if (tb_orderdetails == null)
            {
                return HttpNotFound();
            }
            return View(tb_orderdetails);
        }

        // GET: Orderdetails/Create
        public ActionResult Create()
        {
            ViewBag.oid = new SelectList(db.tb_orders, "oid", "name");
            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname");
            return View();
        }

        // POST: Orderdetails/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,oid,pid,pname,price,nums,photo,state")] tb_orderdetails tb_orderdetails)
        {
            if (ModelState.IsValid)
            {
                db.tb_orderdetails.Add(tb_orderdetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.oid = new SelectList(db.tb_orders, "oid", "name", tb_orderdetails.oid);
            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname", tb_orderdetails.pid);
            return View(tb_orderdetails);
        }

        // GET: Orderdetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orderdetails tb_orderdetails = db.tb_orderdetails.Find(id);
            if (tb_orderdetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.oid = new SelectList(db.tb_orders, "oid", "name", tb_orderdetails.oid);
            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname", tb_orderdetails.pid);
            return View(tb_orderdetails);
        }

        // POST: Orderdetails/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,oid,pid,pname,price,nums,photo,state")] tb_orderdetails tb_orderdetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_orderdetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.oid = new SelectList(db.tb_orders, "oid", "name", tb_orderdetails.oid);
            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname", tb_orderdetails.pid);
            return View(tb_orderdetails);
        }

        // GET: Orderdetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orderdetails tb_orderdetails = db.tb_orderdetails.Find(id);
            if (tb_orderdetails == null)
            {
                return HttpNotFound();
            }
            return View(tb_orderdetails);
        }

        // POST: Orderdetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_orderdetails tb_orderdetails = db.tb_orderdetails.Find(id);
            db.tb_orderdetails.Remove(tb_orderdetails);
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
