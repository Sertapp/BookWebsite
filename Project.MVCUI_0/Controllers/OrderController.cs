using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.MVCUI_0.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI_0.Controllers
{
    public class OrderController : Controller
    {
        OrderRepository _oRep;
        public OrderController()
        {
            _oRep = new OrderRepository();
        }

        // GET: Order
        public ActionResult Orders(int id)
        {

            OrderVM ovm = new OrderVM()
            {
                Orders= _oRep.Where(x => x.AppUserID == id),

            };
            return View(ovm);
        }
    }
}