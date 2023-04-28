namespace GrupparbeteMVC.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? DeliveryAdress { get; set; }

        public DateTime? Date { get; set; }

        public decimal? OrderTotal { get; set; }

        public List<Product>? Products { get; set; }

        public int? Quantity { get; set; }

        public List<CartItem>? CartItems { get; set; }
    }
}
