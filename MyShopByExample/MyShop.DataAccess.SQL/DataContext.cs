using MyShop.Core.Models;
using System.Data.Entity;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection"){ } // Access by reference. 


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCatategory { get; set; }
    }
}
