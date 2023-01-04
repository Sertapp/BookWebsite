using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI_0.VMClasses
{
    public class BookVM
    {
        public Book Book { get; set; }
        public List<Book> Books { get; set; }
        public List<Category> Categories { get; set; }
    }
}