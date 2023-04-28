namespace GrupparbeteMVC.Models.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? DeliveryAdress { get; set; }

        public DateTime? Date { get; set; }

        public decimal? OrderTotal { get; set; }

        public List<Product>? Products { get; set; }

        public int? TotalQuantity { get; set; }

        public List<CartItem>? CartItems { get; set; }

        public List<int> Quantities { get; set; }



    }
}


