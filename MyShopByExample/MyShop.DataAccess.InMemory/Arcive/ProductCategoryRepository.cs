using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    //public 
    class ProductCatagory
    {
            ObjectCache cache = MemoryCache.Default;
            List<ProductCategory> productCategoryList;// = new List<ProductCategory>();

            public ProductCatagory()
            {
                productCategoryList = cache["productCategoryList"] as List<ProductCategory>;
                if (productCategoryList == null)
                {
                    productCategoryList = new List<ProductCategory>();
                }
            }
            public void Commit()
            {
                cache["productCategoryList"] = productCategoryList;
            }

            public void Insert(ProductCategory p)
            {
                productCategoryList.Add(p);
            }

            public void Update(ProductCategory product)
            {
                ProductCategory productCategoryToUpdate = productCategoryList.Find(p => p.Id == product.Id);

                if (productCategoryToUpdate != null)
                {
                    productCategoryToUpdate = product;
                }
                else
                {
                    throw new Exception("ProductCategory not found");
                }
            }

            public ProductCategory Find(string Id)
            {
                ProductCategory product = productCategoryList.Find(p => p.Id == Id);

                if (product != null)
                {
                    return product;
                }
                else
                {
                    throw new Exception("Product not found");
                }
            }

            public IQueryable<ProductCategory> Collection()
            {
                return productCategoryList.AsQueryable();
            }

            public void Delete(string Id)
            {
                ProductCategory productCategoryToDelete = productCategoryList.Find(p => p.Id == Id);

                if (productCategoryToDelete != null)
                {
                    productCategoryList.Remove(productCategoryToDelete);
                }
                else
                {
                    throw new Exception("ProductCategory not found");
                }
            }
       }
    
}
