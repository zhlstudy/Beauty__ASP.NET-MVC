using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beautydemo.Models;
using PagedList;

namespace Beautydemo.Controllers
{
    public class MessageController : Controller
    {
        private BeautyEntities2 db = new BeautyEntities2();

        // GET: Message
        public ActionResult Index(int ?page)
        {
            var messageList = from s in db.tb_message select s;
            // messageList = messageList.OrderBy(a => a.messDate);
            messageList = messageList.OrderByDescending(a => a.messdate);
            int pageNumber = page ?? 1;
            //每页显示多少条  
            int pageSize = 10;
            IPagedList<tb_message> messagePagedList = messageList.ToPagedList(pageNumber, pageSize);
            return View(messagePagedList);
        }

        // GET: Message/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_message tb_message = db.tb_message.Find(id);
            if (tb_message == null)
            {
                return HttpNotFound();
            }
            return View(tb_message);
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mid,title,mess,name,messdate")] tb_message tb_message)
        {
            if (ModelState.IsValid)
            {
                tb_message.messdate = DateTime.Now.ToString("G");
                tb_message.name = (Session["IdInfo"] as tb_user).name;
                db.tb_message.Add(tb_message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_message);
        }

        //// GET: Message/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tb_message tb_message = db.tb_message.Find(id);
        //    if (tb_message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tb_message);
        //}

        //// POST: Message/Edit/5
        //// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        //// 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "mid,title,mess,name,messdate")] tb_message tb_message)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tb_message).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(tb_message);
        //}

        // GET: Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_message tb_message = db.tb_message.Find(id);
            if (tb_message == null)
            {
                return HttpNotFound();
            }
            return View(tb_message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_message tb_message = db.tb_message.Find(id);
            db.tb_message.Remove(tb_message);
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
