using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;
using System.Runtime.Caching;

/// <summary>
///  MyShop.DataAccess.InMemory 
///  Class designed to initiate a local memory cache. 
///  The object will be able to perform CRUD operations.
/// </summary>
namespace MyShop.DataAccess.InMemory
{
    //public 
    class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> productsList;// = new List<Product>();

        public ProductRepository()
        {
            productsList = cache["productsList"] as List<Product>;
            if (productsList == null)
            {
                productsList = new List<Product>();
            }
        }
        public void Commit()
        {
            cache["productsList"] = productsList;
        }

        public void Insert(Product p)
        {
            productsList.Add(p);
        }

        public void Update(Product product)
        {
            Product productToUpdate = productsList.Find(p => p.Id == product.Id);

            if( productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public Product Find(string Id)
        {
            Product product = productsList.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public IQueryable<Product> Collection()
        {
            return productsList.AsQueryable();
        }

        public void Delete(string Id)
        {
            Product productToDelete = productsList.Find(p => p.Id == Id);

            if (productToDelete != null)
            {
                productsList.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
    }
}
