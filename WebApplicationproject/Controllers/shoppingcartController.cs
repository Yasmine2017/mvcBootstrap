using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationproject.Models;

namespace WebApplicationproject.Controllers
{
    public class shoppingcartController : Controller
    {
        private shoppingcartEntities de = new shoppingcartEntities();

        // GET: shoppingcart
        public ActionResult Index(string search)
        {
            ViewBag.listproducts = de.product.ToList();

            return View(de.product.Where(s => s.name.StartsWith(search) || search == null).ToList());
        }

        private int isExisting(int id)
        {
            List<items> cart = (List<items>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)

                if (cart[i].Product.productID == id)
                    return i;
            return -1;

        }

        public ActionResult Delete(int id)
        {
            int index = isExisting(id);
            List<items> cart = (List<items>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return View("cart");

        }
        public ActionResult Details(int id)
        {
            product p = (from c in de.product
                         where c.productID == id
                         select c).FirstOrDefault<product>();
            return View(p);
        }
     //   [Authorize(Roles ="user")]
        public ActionResult ordernow(int id)
        {
            if (Session["cart"] == null)
            {
                List<items> cart = new List<items>();
                cart.Add(new items(de.product.Find(id), 1));
                Session["cart"] = cart;
            }
            else
            {
                List<items> cart = (List<items>)Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                    cart.Add(new items(de.product.Find(id), 1));
                else
                    cart[index].Quantity++;
                Session["cart"] = cart;
            }
            return View("cart");
        }

        
     //   [Authorize(Roles ="user")]
        public ActionResult checkout()
        {
            return View("checkout");
        }
  
        //   [Authorize(Roles ="user")]
        public ActionResult saveorder(FormCollection fc )
        {

            List<items> cart = (List<items>)Session["cart"];
            //save invoice
            invoice Invoice = new invoice();
            Invoice.data = DateTime.Now;
            Invoice.customername = fc["customer name"];
            Invoice.customername = fc["customer address"];
            Invoice.name = "New Invoice";
            de.invoice.Add(Invoice);
            de.SaveChanges();
            //save details
            foreach (items item in cart)
            {
                invoicedetails Invoicedetails = new invoicedetails();
                Invoicedetails.invoiceid = Invoice.id;
                Invoicedetails.Quentity = item.Quantity;
                Invoicedetails.price = (double)item.Product.price;
                Invoicedetails.productid = item.Product.productID;
                de.invoicedetails.Add(Invoicedetails);
                de.SaveChanges();
            }
            //clear all item
            Session.Remove("cart");
            return View("thanks");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserAcount s1 )
        {
             
            if (ModelState.IsValid)
            {
                UserAcount ss = new UserAcount() { fristname=s1.fristname,lastname=s1.lastname,
                email=s1.email,username=s1.username};
                if (ss.password == "123")
                    ss.isadmin = true;
                else ss.isadmin = false;
                de.UserAcount.Add(ss);
                 de.SaveChanges();
                
                ModelState.Clear();
                ViewBag.Message = s1.fristname + " " + s1.lastname + "Successfuly Registered.";
            }
            return View();
        }

              public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserAcount user)
        {

            //  var usr = de.UserAcount.Single(U => U.username == user.username && U.password == user.password);
            UserAcount usr = (from u in de.UserAcount
                              where u.password == user.password
                              select u).FirstOrDefault();
            if (usr != null)
                {
                    Session["UserID"] = usr.id.ToString();
                    Session["username"] = usr.username.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password Is Wrong.");
                }
            
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}