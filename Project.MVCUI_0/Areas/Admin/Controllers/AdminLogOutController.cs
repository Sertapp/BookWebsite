using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI_0.Areas.Admin.Controllers
{
    public class AdminLogOutController : Controller
    {
        // GET: Admin/AdminLogOut
        public ActionResult AdminLogOut()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "MainPage", new { Area = "" });
        }
    }
}