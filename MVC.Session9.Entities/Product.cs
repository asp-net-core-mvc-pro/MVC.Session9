using System;
using System.Collections.Generic;

namespace MVC.Session9.Entities
{
    public class Product
    {

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Version { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public Category CategoryName { get; set; }
    }
}

