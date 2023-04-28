
using GrupparbeteMVC.Models;
using GrupparbeteMVC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grupparbete.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationContext Context;

        public ProductsController(ApplicationContext _context)
        {
            Context = _context;
        }
        public IActionResult Index()
        {
            List<Product> MyProducts = Context.Products.ToList();
            return View(MyProducts);
        }

        public IActionResult Details(int Id)
        {
            Product CurrentProduct = Context.Products.Include(p => p.Categories).Include(p => p.Tags).Where(p => p.Id == Id).FirstOrDefault();
            return View(CurrentProduct);
        }

        public IActionResult IndexDollarPrice()
        {
            List<Product> MyProducts = Context.Products.ToList();
            return View(MyProducts);
        }

        public IActionResult DetailsDollarPrice(int Id)
        {
            Product CurrentProduct = Context.Products.Include(p => p.Categories).Include(p => p.Tags).Where(p => p.Id == Id).FirstOrDefault();
            return View(CurrentProduct);
        }

    }
}
