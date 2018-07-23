using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        public const string CartSession = "Cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null;
            var currentSessionCart = controllerContext.HttpContext.Session[CartSession];
            if (currentSessionCart != null)
            {
                cart = (Cart)currentSessionCart;
            }
            else
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[CartSession] = cart;
            }
            return cart;
        }
    }
}