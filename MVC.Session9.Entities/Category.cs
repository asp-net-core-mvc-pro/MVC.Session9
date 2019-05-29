using System;
using System.Collections.Generic;
using System.Text;

namespace MVC.Session9.Entities
{

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
