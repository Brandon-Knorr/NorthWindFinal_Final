using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CheckoutController : Controller
{
    private readonly DataContext _context;

    public CheckoutController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        int customerId = 1; // Replace with actual user session/authentication

        // Fetch cart items for this customer
        var cartItems = _context.CartItems
                                .Where(c => c.CustomerId == customerId)
                                .Include(c => c.Product)
                                .ToList();

        if (!cartItems.Any()) return RedirectToAction("EmptyCart");

        decimal totalAmount = cartItems.Sum(item => item.Product.UnitPrice * item.Quantity);

        var order = new Order
        {
            Items = cartItems,
            TotalAmount = totalAmount,
            DiscountApplied = false
        };

        return View(order);
    }

    [HttpPost]
    public ActionResult CompleteCheckout()
    {
        int customerId = 1; // Replace with actual user ID
        string discountCode = Request.Form["DiscountCode"];
        decimal discountRate = discountCode == "SAVE10" ? 0.1m : 0m; // Apply 10% discount

        var cartItems = _context.CartItems.Where(c => c.CustomerId == customerId).Include(c => c.Product).ToList();
        if (!cartItems.Any()) return RedirectToAction("EmptyCart");

        // Create a new Order and save it first
        var newOrder = new Order { CustomerId = customerId };
        _context.Orders.Add(newOrder);
        _context.SaveChanges(); // Ensure OrderId is assigned

        // Save order details
        foreach (var item in cartItems)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = newOrder.OrderId, // Use generated OrderId
                ProductId = item.ProductId,
                UnitPrice = item.Product.UnitPrice,
                Quantity = item.Quantity,
                Discount = discountRate
            };

            _context.OrderDetails.Add(orderDetail);
        }

        // Remove items from CartItem after checkout
        _context.CartItems.RemoveRange(cartItems);
        _context.SaveChanges();

        return RedirectToAction("Confirmation", new { orderId = newOrder.OrderId });
    }

    public ActionResult Confirmation(int orderId)
    {
        var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        return View(orderDetails);
    }
}