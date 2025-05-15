using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CheckoutController : Controller
{
    private readonly DataContext _context;

    public CheckoutController(DataContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "northwind-customer")]
    public IActionResult Index()
    {
        return View(_context.CartItems.Include("Product")
                .Where(c => c.CustomerId == _context.Customers
                .FirstOrDefault(c => c.Email == User.Identity.Name).CustomerId));
    }

    [HttpPost]
    [Authorize(Roles = "northwind-customer")]
    public IActionResult RemoveFromCart(int cartItemId)
    {
        var cartItem = _context.CartItems.FirstOrDefault(c => c.CartItemId == cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = "northwind-customer")]
    public IActionResult CompleteCheckout()
    {
        var cartItems = _context.CartItems.Where(c => c.CustomerId == _context.Customers
                .FirstOrDefault(c => c.Email == User.Identity.Name).CustomerId);
        if (!cartItems.Any()) return RedirectToAction("EmptyCart");

        // TODO: Create a new Order and save it first


        // TODO: Save order details


        // Remove items from CartItem after checkout
        _context.CartItems.RemoveRange(cartItems);
        _context.SaveChanges();

        return RedirectToAction("Confirmation");
    }

    public IActionResult Confirmation()
    {
        return View();
    }
}