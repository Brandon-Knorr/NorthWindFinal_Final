using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderDetail
{
    [Key]
    public int OrderDetailsId { get; set; } // Primary key
    public int OrderId { get; set; } // Foreign key referencing Order
    public int ProductId { get; set; } // Product being purchased
    public decimal UnitPrice { get; set; } // Product price
    public int Quantity { get; set; } // Purchase quantity
    public decimal Discount { get; set; } // Applied discount percentage
}