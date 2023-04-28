using GrupparbeteMVC.Models;
using GrupparbeteMVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using GrupparbeteMVC.Models.ViewModels;

namespace Grupparbete.Controllers
{
    public class OrdersController : Controller
    {
        public readonly ApplicationContext Context;
        public OrdersController(ApplicationContext _context)
        {
            Context = _context;
        }

        public IActionResult Index()
        {
            // varför inkluderar jag products här?
            List<Order> MyOrders = Context.Orders.Include(o => o.Products).ToList();
            return View(MyOrders);

        }

        public IActionResult Details(int Id)
        {



            Order CurrentOrder = Context.Orders.Include(x => x.Products).FirstOrDefault(o => o.OrderId == Id);
            List<OrderItem> currentOrderItems = Context.OrderItems.Where(x => x.OrderId == Id).ToList();


            OrderViewModel OrderDetails = new OrderViewModel();
            OrderDetails.Products = new List<Product>();
            OrderDetails.Quantities = new List<int>();


            foreach (var item in CurrentOrder.Products)
            {
                OrderDetails.Products.Add(item);
            }

            foreach (var item in currentOrderItems)
            {
                OrderDetails.Quantities.Add(item.Quantity);
            }

            OrderDetails.DeliveryAdress = CurrentOrder.DeliveryAdress;
            OrderDetails.TotalQuantity = CurrentOrder.Quantity;
            OrderDetails.Date = CurrentOrder.Date;
            OrderDetails.Name = CurrentOrder.Name;
            OrderDetails.LastName = CurrentOrder.LastName;
            OrderDetails.OrderTotal = CurrentOrder.OrderTotal;
            OrderDetails.OrderId = CurrentOrder.OrderId;

            return View(OrderDetails);


        }

        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult CheckoutError()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder(int Id, string Name, string LastName, string DeliveryAdress, string CardNumber, DateTime CardExpirationDate, string CardCVC)
        {

            Cart CurrentCart = Context.Carts.Include(cart => cart.CartItems).ThenInclude(cartItem => cartItem.Product).FirstOrDefault(cart => cart.Id == 1);
          

            Order NewOrder = new Order();
           
            NewOrder.OrderId = Id;
            NewOrder.Name = Name;
            NewOrder.LastName = LastName;
            NewOrder.DeliveryAdress = DeliveryAdress;
            NewOrder.OrderTotal = CurrentCart.CartTotal;
            NewOrder.Quantity = 0;
            NewOrder.Products = new List<Product>();

            foreach (var cartItem in CurrentCart.CartItems)
            {

                NewOrder.Products.Add(cartItem.Product);
                NewOrder.Quantity = NewOrder.Quantity + cartItem.Quantity;

            }

            Context.Orders.Add(NewOrder);
            Context.SaveChanges();

            foreach (var cartItem in CurrentCart.CartItems)
            {

                OrderItem NewOrderItem = new OrderItem();
                NewOrderItem.OrderId = NewOrder.OrderId;
                NewOrderItem.ProductId = (int)cartItem.ProductId;
                NewOrderItem.Quantity = (int)cartItem.Quantity;
                Context.OrderItems.Add(NewOrderItem);

            }

            Context.SaveChanges();



            if (string.IsNullOrEmpty(NewOrder.Name))
            {
                return RedirectToAction(nameof(CheckoutError));
            }
            if (string.IsNullOrEmpty(NewOrder.LastName))
            {
                return RedirectToAction(nameof(CheckoutError));
            }
            if (string.IsNullOrEmpty(NewOrder.DeliveryAdress))
            {
                return RedirectToAction(nameof(CheckoutError));
            }
            // Använder regex för att kolla att CardNumber enbart innehåller siffror.
            if ((string.IsNullOrEmpty(CardNumber)) || CardNumber.Length < 16 || CardNumber.Length > 16 || !Regex.IsMatch(CardNumber, @"^\d+$"))
            {
                return RedirectToAction(nameof(CheckoutError));
            }
            if (!CardCVC.ToString().All(char.IsDigit) || CardCVC.Length < 3 || CardCVC.Length > 3)
            {
                return RedirectToAction(nameof(CheckoutError));
            }
            if (CardExpirationDate == DateTime.MinValue)
            {
                return RedirectToAction(nameof(CheckoutError));
            }





            return RedirectToAction(nameof(Details), new { Id = NewOrder.OrderId });




        }

    }
}
