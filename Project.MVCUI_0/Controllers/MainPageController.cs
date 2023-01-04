using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI_0.Controllers
{
    public class MainPageController : Controller
    {
        public MainPageController()
        {

        }
        // GET: MainPage
        public ActionResult HomePage()
        {
            return View();
        }
    }
}