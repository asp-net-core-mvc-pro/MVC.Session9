using Microsoft.EntityFrameworkCore;
using MVC.Session9.Entities;
using System;
using System.Collections.Generic;

namespace MVC.Session9.DAL
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=local; initial catalog = BackwardDB;integrated security = true");
        }
    } 
}
