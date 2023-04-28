namespace GrupparbeteMVC.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public decimal? CartTotal { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
