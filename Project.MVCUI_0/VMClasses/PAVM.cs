using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Project.ENTITIES.Models;

namespace Project.MVCUI_0.VMClasses
{
    public class PAVM
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public IPagedList<Product> PagedProducts { get; set; }
        
    }
}