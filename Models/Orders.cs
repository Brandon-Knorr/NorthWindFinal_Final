public class Orders
{
    public int OrderId { get; set; }
    public List<CartItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public bool DiscountApplied { get; set; }
    public int CustomerId { get; internal set; }
}