using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrinkAndGo.Controllers
{
    public class OrderController:Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
