namespace Models
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
