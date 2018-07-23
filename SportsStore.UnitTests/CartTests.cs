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
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Linest()
        {
            Product product1 = new Product { ProductID = 1, Name = "P1" };
            Product product2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 2);
            CartLine[] cartLines = cart.Lines.ToArray();
            Assert.AreEqual(cartLines.Length, 2);
            Assert.AreEqual(cartLines[0].Product, product1);
            Assert.AreEqual(cartLines[1].Product, product2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product product1 = new Product { ProductID = 1, Name = "P1" };
            Product product2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 2);
            cart.AddItem(product1, 11);
            CartLine[] cartLines = cart.Lines.ToArray();
            Assert.AreEqual(cartLines[0].Quantity, 12);
            Assert.AreEqual(cartLines[1].Quantity, 2);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Product product1 = new Product { ProductID = 1, Name = "P1", Price = 500 };
            Product product2 = new Product { ProductID = 2, Name = "P2", Price = 400 };
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 2);
            cart.AddItem(product1, 2);
            var totalValue = cart.ComputeTotalValue();
            Assert.AreEqual(totalValue, 2300m);

        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
            });
            Cart cart = new Cart();
            CartController cartController = new CartController(mock.Object,null);
            cartController.AddToCart(cart, 1, null);
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }
        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
            });
            Cart cart = new Cart();
            CartController cartController = new CartController(mock.Object,null);
            RedirectToRouteResult routeResult= cartController.AddToCart(cart, 1, "myurl");
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["returnUrl"], "myurl");

        }

    }
}
