using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationproject.Models;

namespace WebApplicationproject.Controllers
{
    public class productController : Controller
    {
        private shoppingcartEntities de = new shoppingcartEntities();

        // GET: product
       // [Authorize(Roles ="admin")]
        public ActionResult Index(string search)
        {
            return View(de.product.Where(s => s.name.StartsWith(search) || search == null).ToList());
        }

        // GET: product/Details/5
       // [Authorize(Roles ="user")]
        public ActionResult Details(int id)
        {
            product p = (from c in de.product
                      where c.productID == id
                      select c).FirstOrDefault<product>();
            return View(p);
        }

        // GET: product/Create
       // [Authorize(Roles ="admin")]
        public ActionResult Create()
        {
            SelectList m = new SelectList(de.product.ToList<product>(), "categoryid", "categoryid");
            ViewBag.categoryid = m;
            return View();
        }

        // POST: product/Create
        [HttpPost]
     //   [Authorize(Roles ="admin")]
        public ActionResult Create(product p1, HttpPostedFileBase image)
        {
            SelectList m = new SelectList(de.product.ToList<product>(), "categoryid", "categoryid");
            ViewBag.id = m;
            try
            {
                if (ModelState.IsValid)
                {
                    if (image != null)
                    {
                        string img = System.IO.Path.GetFileName(image.FileName);
                        
                        string path = System.IO.Path.Combine(Server.MapPath("~/Content/image"), img);
                        image.SaveAs(path);
                        p1.image = "Content/image/" + img;
                    }
                  
                    de.product.Add(p1);
                    de.SaveChanges();
                    return RedirectToAction("Index"); //View("Index", contect.product);

                }
                else {

                    return View();
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: product/Edit/5
      //  [Authorize(Roles ="admin")]
        public ActionResult Edit(int id)
        {
            product p1 = (from p in de.product
                          where p.productID == id
                          select p).FirstOrDefault();
            SelectList m = new SelectList(de.category.ToList<category>(), "id", "categoryname");
            ViewBag.categoryname = m;
            return View(p1);
        }

        // POST: product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, product p1)
        {
            try
            {
                // TODO: Add update logic here
                product oldp1 = de.product.ToList<product>().Find(b => b.productID == id);
                // TODO: Add update logic here
                oldp1.name = p1.name;
                oldp1.descripe = p1.descripe;
                oldp1.price = p1.price;
                oldp1.image = p1.image;
                oldp1.category = p1.category;
                de.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: product/Delete/5
        public ActionResult Delete(int id)
        {
           // product p = de.product.Find(id);

            return View(de.product.ToList<product>().Find(e => e.productID == id));
        }

        // POST: product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, product prod)
        {
            prod = de.product.ToList<product>().Find(e => e.productID == id);//de.product.Find(id);

            try
            {
                // TODO: Add delete logic here
                prod = de.product.ToList<product>().Find(e => e.productID == id);//de.product.Find(id);
                if (ModelState.IsValid)
                {
                    de.product.Remove(prod);
                    de.SaveChanges();
                    return View("Index", de.product);
                }
                return View(prod);
            }
            catch
            {
                return View(prod);
            }
        }
    }
}
