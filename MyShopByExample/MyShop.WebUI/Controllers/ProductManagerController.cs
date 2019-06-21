using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers{
    
    public class ProductManagerController : Controller
    {
        InMemoryRepository<Product> context;
        InMemoryRepository<ProductCategory> productCatagory;

        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            productCatagory = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> productsList = context.Collection().ToList();
            return View(productsList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Will need to return a new product with a list of catagories. 
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            viewModel.Product = new Product();
            viewModel.ProductCategories = productCatagory.Collection();
            // Product product = new Product();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCatagory.Collection();
                return View(viewModel);
            }
                
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);
            if(product == null)
            {
                return HttpNotFound();
            }

            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                /* Using the model from context (ProductRepository)
                 * Adding any changes 
                 */
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();// Refrerence to DataAccess Model
                                 // Used as programable Save within code

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                
                context.Commit(); // Refrerence to DataAccess Model
                                  // Used as programable Save within code
                return RedirectToAction("Index");
            }
        }
    }
}