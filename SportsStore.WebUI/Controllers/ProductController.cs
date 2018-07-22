using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 4;

        private IProductsRepository productsRespository;

        public ProductController(IProductsRepository productsRespository)
        {
            this.productsRespository = productsRespository;
        }
        public ViewResult List(string category,int page = 1)
        {

            ProductsListViewModel productsListViewModel = new ProductsListViewModel
            {
                Products = productsRespository.Products
                .Where(c=> c.Category == null ||c.Category==category)
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                paggingInfo = new PaggingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category==null ? productsRespository.Products.Count() : productsRespository.Products.Where(c=>c.Category==category).Count()
                },
                CurrentCategoty = category
        };
            
            return View(productsListViewModel);
            
         }
    }
}