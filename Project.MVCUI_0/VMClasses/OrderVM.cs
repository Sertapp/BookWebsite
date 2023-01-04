using Project.DTO.Models;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI_0.VMClasses
{
    public class OrderVM
    {
        public PaymentDTO PaymentDTO { get; set; }
        public Order Order { get; set; }
        public List<Order> Orders { get; set; }

    }
}