
using GrupparbeteMVC.Models;
using GrupparbeteMVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrupparbeteMVC.Models.ViewModels;

namespace Grupparbete.Controllers
{
    public class CartsController : Controller
    {
        public readonly ApplicationContext Context;
        public CartsController(ApplicationContext context)
        {
            Context = context;
        }

        public IActionResult AddToCart(int id)
        {
            Product CurrentProduct = Context.Products.FirstOrDefault(x => x.Id == id);
            CartItem ExistingCartItem = Context.CartItems.FirstOrDefault(x => x.CartId == 1 && x.ProductId == CurrentProduct.Id);
            if (ExistingCartItem != null)
            {
                ExistingCartItem.Quantity = ExistingCartItem.Quantity + 1;
                ExistingCartItem.CartItemTotal = ExistingCartItem.CartItemTotal + CurrentProduct.SEKPrice;
                Context.CartItems.Update(ExistingCartItem);

                Cart Cart = Context.Carts.FirstOrDefault(x => x.Id == 1);
                Cart.CartTotal = Cart.CartTotal + CurrentProduct.SEKPrice;
                Context.SaveChanges();
            }
            else
            {
                CartItem CartItem = new CartItem();
                CartItem.ProductId = CurrentProduct.Id;
                CartItem.Quantity = 1;
                CartItem.CartItemTotal = CurrentProduct.SEKPrice;
                Cart Cart = Context.Carts.FirstOrDefault(x => x.Id == 1);
                //  Cart.Date = DateTime.Now;
                Cart.CartTotal = Cart.CartTotal + CartItem.CartItemTotal;
                Cart.CartItems = new List<CartItem>();
                Cart.CartItems.Add(CartItem);
                Context.Carts.Add(Cart);
                Context.Carts.Update(Cart);
                Context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Index()
        {
            Cart CurrentCart = Context.Carts.Include(cart => cart.CartItems).ThenInclude(cartItem => cartItem.Product).FirstOrDefault(cart => cart.Id == 1);
            return View(CurrentCart);
            /*
            Cart Cart = Context.Carts.Include(x => x.CartItems).FirstOrDefault(x => x.Id == 1);

            return View(Cart);
            */
        }

        public IActionResult Delete(int Id)
        {

            
          
            CartItem CartItem = Context.CartItems.FirstOrDefault(x => x.Id == Id);
            Product CurrentProduct = Context.Products.FirstOrDefault(x => x.Id == CartItem.ProductId);

            // Skapar en viewmodel för att kunna skriva ut namnet på produkten i View filen.
            var viewModel = new CartItemProduct();
            viewModel.cartItem = CartItem;
            viewModel.product = CurrentProduct;

      

            return View(viewModel);
         

        }
        public IActionResult DeleteConfirm(int Id)
        {
            CartItem CartItem = Context.CartItems.FirstOrDefault(x => x.Id == Id);
            Context.CartItems.Remove(CartItem);
            Context.SaveChanges();

            //Updaterar det totala priset i varukorgen
            Cart CurrentCart = Context.Carts.FirstOrDefault(x => x.Id == 1);
            CurrentCart.CartTotal = CurrentCart.CartTotal - CartItem.CartItemTotal;
            Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteAll()
        {
            var cartItems = Context.CartItems.ToList();
            foreach (var cartItem in cartItems)
            {
                Context.CartItems.Remove(cartItem);
            }
            Cart CurrentCart = Context.Carts.FirstOrDefault(x => x.Id == 1);
            CurrentCart.CartTotal = 0;

            Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteAllConfirm()
        {
            List<CartItem> MyItems = Context.CartItems.ToList();
            return View(MyItems);
        }

        public IActionResult AddOne(int Id)
        {
            CartItem CartItem = Context.CartItems.FirstOrDefault(x => x.Id == Id);
            Cart CurrentCart = Context.Carts.FirstOrDefault(x => x.Id == 1);
            Product CurrentProduct = Context.Products.FirstOrDefault(x => x.Id == CartItem.ProductId);

            //Uppdaterar kvantiteten för den enskillda varan 
            CartItem.Quantity = CartItem.Quantity + 1;
            //Updaterar det totala priset i varukorgen
            CurrentCart.CartTotal = CurrentCart.CartTotal + CurrentProduct.SEKPrice;
            //Uppdaterar priset för den enskillda varan
            CartItem.CartItemTotal = CartItem.CartItemTotal + CurrentProduct.SEKPrice;
            Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveOne(int Id)
        {
            CartItem CartItem = Context.CartItems.FirstOrDefault(x => x.Id == Id);
            Cart CurrentCart = Context.Carts.FirstOrDefault(x => x.Id == 1);
            Product CurrentProduct = Context.Products.FirstOrDefault(x => x.Id == CartItem.ProductId);

            // Om det bara finns 1st en vara så tas den bort om användaren trycker på minustecknet
            if (CartItem.Quantity == 1)
            {
                Context.CartItems.Remove(CartItem);
                Context.SaveChanges();

                //Updaterar det totala priset i varukorgen
                CurrentCart.CartTotal = CurrentCart.CartTotal - CurrentProduct.SEKPrice;
                //Uppdaterar priset för den enskillda varan
                CartItem.CartItemTotal = CartItem.CartItemTotal - CurrentProduct.SEKPrice;
                Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                CartItem.Quantity = CartItem.Quantity - 1;
                Context.SaveChanges();

                CurrentCart.CartTotal = CurrentCart.CartTotal - CurrentProduct.SEKPrice;
                CartItem.CartItemTotal = CartItem.CartItemTotal - CurrentProduct.SEKPrice;
                Context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }

        }





    }
}