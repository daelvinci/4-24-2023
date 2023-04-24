namespace WebApplication1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSeller { get; set; }
        public bool StockStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Genre Genre { get; set; }
        public Author Author{ get; set; }
        public ICollection<Tag> BookTags{ get; set; }
        public ICollection<BookImage> BookImages { get; set; }
    }
}
