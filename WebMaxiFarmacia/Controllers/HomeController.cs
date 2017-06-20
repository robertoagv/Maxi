using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMaxiFarmacia.Models;

namespace WebMaxiFarmacia.Controllers
{
    public class HomeController : Controller
    {
        maxifarmaciabdContext db = new maxifarmaciabdContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
               
                    var user = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();

                    return View(user);
                
            }

            return RedirectToAction("Login", "Account");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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