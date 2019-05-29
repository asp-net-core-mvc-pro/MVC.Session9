using MVC.Session9.DAL;
using MVC.Session9.Entities;
using System;

namespace MVC.Session9.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductContext productContext = new ProductContext();

            var product = new Product
            {
                Name = "H456",
                Series = "H",
              //  CategoryName="Cinema",
                Version="00"
            };
            productContext.Products.Add(product);
            productContext.SaveChanges();
            Console.WriteLine("Hello World!");
        }
    }
}
