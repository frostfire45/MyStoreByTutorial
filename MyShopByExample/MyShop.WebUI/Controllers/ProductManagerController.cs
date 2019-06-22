using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System.IO;

namespace MyShop.WebUI.Controllers{
    
    public class ProductManagerController : Controller
    {
        // INTERFACE VS. CLASS REFERENCE
        // Commented out and implementing an interface
        // -------------------------------------------
        // InMemoryRepository<Product> context;
        // InMemoryRepository<ProductCategory> productCatagory;
        // public ProductManagerController()
        //{
        //    context = new InMemoryRepository<Product>();
        //    productCatagory = new InMemoryRepository<ProductCategory>();
        //}
        // -------------------------------------------

        IRepository<Product> context;
        IRepository<ProductCategory> productCatagory;

        // INJECTION OF OUR INTERFACE
        // We will inject the contructor with the interfaces contents
        // =================================
        // public ProductManagerController()
        // -----------Injected-------------
        // public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCatagoryContext)
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCatagoryContext)
        {
            context = productContext;
            productCatagory = productCatagoryContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> productsList = context.Collection().ToList();
            return View(productsList);
        }

     
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
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if(file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
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
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
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
                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }
                
                /* Using the model from context (ProductRepository)
                 * Adding any changes 
                 */
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
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