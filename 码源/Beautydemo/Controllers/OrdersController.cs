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
    public class OrdersController : Controller
    {
        private BeautyEntities2 db = new BeautyEntities2();

        // GET: Orders
        public ActionResult Index()
        {
            if (Session["Role"] == null)
            {
                return Content("<script>alert('用户登录已过期或未登录，请重新登录！');window.location.href='/User/Login';</script>");
            }
            if (Session["Role"].ToString() == "user")
            {
                var user = Session["IdInfo"] as tb_user;
                return View(db.tb_orders.Where(a => a.name == user.uid.ToString()).ToList());
            }
            return View(db.tb_orders.ToList());
        }

        public ActionResult InsertOrder()
        {
            if (Session["IdInfo"] == null)
            {
                return Content("<script>alert('用户登录已过期或未登录，请重新登录！');window.location.href='/User/Login';</script>");
            }
            var user = Session["IdInfo"] as tb_user;
            tb_orders order = new tb_orders();
            order.name = user.uid.ToString();
            order.orderTime = DateTime.Now.ToString("G");
            //计算订单金额
            var cart = db.tb_cart.Where(a => a.name == user.uid.ToString()).ToList();
            decimal? priceTotal = 0;
            //计算订单商品数量
            int productCounts = 0;
            foreach (var c in cart)
            {
                priceTotal += c.price * c.nums;
                productCounts++;
            }
            order.allPrice = priceTotal;
            var currentUser = db.tb_user.Find(user.uid);
            order.address = currentUser.address;
            order.tel = currentUser.tel;
            order.pcounts = productCounts;
            db.tb_orders.Add(order);
            foreach (var myC in cart)
            {
                var orderDetail = new tb_orderdetails();
                orderDetail.oid = order.oid;
                orderDetail.name = myC.name;
                orderDetail.pid = myC.pid;
                orderDetail.pname = myC.pname;
                orderDetail.price = myC.price;
                orderDetail.nums = myC.nums;
                orderDetail.photo = myC.photo;
                orderDetail.state = "未付款";
                db.tb_orderdetails.Add(orderDetail);
                if (myC.nums > 0)
                {
                    foreach (var c in cart)
                    {
                        db.Entry(c).State = EntityState.Deleted;
                        db.tb_cart.Remove(c);
                    }
                    db.SaveChanges();
                    return Content("<script>alert('提交订单成功!');window.location.href='/Orders/Index';</script>");
                }
                else
                {
                    foreach (var c in cart)
                    {
                        db.Entry(c).State = EntityState.Deleted;
                        db.tb_cart.Remove(c);
                    }
                    db.SaveChanges();
                    return Content("<script>alert('提交订单失败!');window.location.href='/Cart/Index';</script>");
                }
            }
            return Content("<script>alert('提交订单失败!');window.location.href='/Cart/Index';</script>");

        }




        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orders tb_orders = db.tb_orders.Find(id);
            if (tb_orders == null)
            {
                return HttpNotFound();
            }
            return View(tb_orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "oid,name,orderTime,allPrice,tel,pcounts")] tb_orders tb_orders)
        {
            if (ModelState.IsValid)
            {
                db.tb_orders.Add(tb_orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orders tb_orders = db.tb_orders.Find(id);
            if (tb_orders == null)
            {
                return HttpNotFound();
            }
            return View(tb_orders);
        }

        // POST: Orders/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "oid,name,orderTime,allPrice,tel,pcounts")] tb_orders tb_orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orders tb_orders = db.tb_orders.Find(id);
            if (tb_orders == null)
            {
                return HttpNotFound();
            }
            return View(tb_orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_orders tb_orders = db.tb_orders.Find(id);
            db.tb_orders.Remove(tb_orders);
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
