using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using Moq;
using System.Linq;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using SportsStore.WebUI.HtmlHelpers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product(){ProductID=1,Name="P1"},
                new Product(){ProductID=1,Name="P2"},
                new Product(){ProductID=1,Name="P3"},
                new Product(){ProductID=1,Name="P4"},
                new Product(){ProductID=1,Name="P5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            IEnumerable<Product> result = (IEnumerable<Product>)controller.List(2).Model;
            Product[] arrayProduct = result.ToArray();
            Assert.IsTrue(arrayProduct.Length == 2);
            Assert.AreEqual(arrayProduct[0].Name, "P4");
            Assert.AreEqual(arrayProduct[1].Name, "P5");


        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper htmlHelper = null;
            PaggingInfo paggingInfo = new PaggingInfo()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrl = i => "Page" + i;
            MvcHtmlString result = htmlHelper.PageLinks(paggingInfo, pageUrl);
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                                    + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());

        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
              new Product{ProductID=1,Name="P1"},
              new Product{ProductID=1,Name="P2"},
              new Product{ProductID=1,Name="P3"},
              new Product{ProductID=1,Name="P4"},
            });
            ProductController productController = new ProductController(mock.Object);
            productController.PageSize = 2;
            ProductsListViewModel productViewModel =(ProductsListViewModel) productController.List(2).Model;
            PaggingInfo paggingInfoResult = productViewModel.paggingInfo;
            Assert.AreEqual(paggingInfoResult.CurrentPage, 2);
            Assert.AreEqual(paggingInfoResult.TotalItems, 4);
            Assert.AreEqual(paggingInfoResult.ItemsPerPage, 2);
            Assert.AreEqual(paggingInfoResult.TotalPages, 2);




        }
    }
}
