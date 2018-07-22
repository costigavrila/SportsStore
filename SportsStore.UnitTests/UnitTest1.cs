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
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;
            Product[] arrayProduct = result.Products.ToArray();
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
            ProductsListViewModel productViewModel =(ProductsListViewModel) productController.List(null,2).Model;
            PaggingInfo paggingInfoResult = productViewModel.paggingInfo;
            Assert.AreEqual(paggingInfoResult.CurrentPage, 2);
            Assert.AreEqual(paggingInfoResult.TotalItems, 4);
            Assert.AreEqual(paggingInfoResult.ItemsPerPage, 2);
            Assert.AreEqual(paggingInfoResult.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Fileter_Products()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
              new Product{ProductID=1,Name="P1",Category="C1"},
              new Product{ProductID=1,Name="P2",Category="C2"},
              new Product{ProductID=1,Name="P3",Category="C3"},
              new Product{ProductID=1,Name="P4",Category="C4"},
            });
            ProductController productController = new ProductController(mock.Object);
            productController.PageSize = 3;
            Product[] results = ((ProductsListViewModel)productController.List("C1", 1).Model).Products.ToArray();
            Assert.AreEqual(results.Length,1);
            Assert.IsTrue(results[0].Name == "P1" && results[0].Category == "C1");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
              new Product{ProductID=1,Name="P1",Category="C1"},
              new Product{ProductID=1,Name="P2",Category="C1"},
              new Product{ProductID=1,Name="P3",Category="C2"},
              new Product{ProductID=1,Name="P4",Category="C3"},
            });
            NavController navController = new NavController(mock.Object);
            string[] results = ((IEnumerable<string>)navController.Menu().Model).ToArray();
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "C1");
            Assert.AreEqual(results[1], "C2");
            Assert.AreEqual(results[2], "C3");
        }
    }
}
