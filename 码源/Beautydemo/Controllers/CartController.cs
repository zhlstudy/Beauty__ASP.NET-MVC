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
    public class CartController : Controller
    {
        private BeautyEntities2 db = new BeautyEntities2();

        // GET: Cart
        public ActionResult Index()
        {
            if (Session["IdInfo"] == null)
            {
                return Content("<script>alert('用户登录已过期或未登录，请重新登录！');window.location.href='/User/Login';</script>");
            }
            var user = Session["IdInfo"] as tb_user;
            var tb_cart = db.tb_cart.Where(a => a.name == user.uid.ToString());
           
            return View(tb_cart.ToList());
        }

        public ActionResult JoinCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = Session["IdInfo"] as tb_user;
            if (user == null)
            {
                return Content("<script>alert('用户登录已过期或未登录，请重新登录！');window.location.href='/User/Login';</script>");
            }

            var cart = db.tb_cart.Where(a => a.name == user.uid.ToString()).FirstOrDefault();
            if (cart == null)
            {
                cart = new tb_cart();
                cart.name = user.uid.ToString();
                tb_product tb_product = db.tb_product.Find(id);
                if (tb_product == null)
                {
                    return HttpNotFound();
                }
                cart.pid = (int)id;
                cart.pname = tb_product.pname;
                cart.price = tb_product.price;
                cart.nums = 1;
                cart.photo = tb_product.photo;
                db.tb_cart.Add(cart);
                db.SaveChanges();
                return Content("<script>alert('添加购物车成功');window.location.href='/Cart/Index';</script>");
            }
            else
            {
                tb_product tb_product = db.tb_product.Find(id);
                if (tb_product == null)
                {
                    return HttpNotFound();
                }
                //判断添加的商品id是否在购物车已存在
                var myCart = db.tb_cart.Where(a => a.name == user.uid.ToString()).Where(p => p.pid == id).FirstOrDefault();
                if (myCart == null)
                {
                    myCart = new tb_cart();
                    myCart.name = user.uid.ToString();
                    myCart.pid = (int)id;
                    myCart.pname = tb_product.pname;
                    myCart.price = tb_product.price;
                    myCart.nums = 1;
                    myCart.photo = tb_product.photo;
                    db.tb_cart.Add(myCart);
                    db.SaveChanges();
                }
                else
                {
                    myCart.nums += 1;
                    db.Entry(myCart).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return Content("<script>alert('添加购物车成功');window.location.href='/Cart/Index';</script>");
            }
        }

        // GET: Cart/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_cart tb_cart = db.tb_cart.Find(id);
            if (tb_cart == null)
            {
                return HttpNotFound();
            }
            return View(tb_cart);
        }



        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname");
            return View();
        }

        // POST: Cart/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cid,name,pid,pname,price,nums,photo")] tb_cart tb_cart)
        {
            if (ModelState.IsValid)
            {
                db.tb_cart.Add(tb_cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname", tb_cart.pid);
            return View(tb_cart);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_cart tb_cart = db.tb_cart.Find(id);
            if (tb_cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname", tb_cart.pid);
            return View(tb_cart);
        }

        // POST: Cart/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cid,name,pid,pname,price,nums,photo")] tb_cart tb_cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pid = new SelectList(db.tb_product, "pid", "pname", tb_cart.pid);
            return View(tb_cart);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_cart tb_cart = db.tb_cart.Find(id);
            if (tb_cart == null)
            {
                return HttpNotFound();
            }
            return View(tb_cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_cart tb_cart = db.tb_cart.Find(id);
            db.tb_cart.Remove(tb_cart);
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
