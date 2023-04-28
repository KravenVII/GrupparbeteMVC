using Azure;

namespace GrupparbeteMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? SEKPrice { get; set; }

        public decimal? DollarPrice { get; set; }

        public List<Category>? Categories { get; set; }

        public List<Tag>? Tags { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
