namespace TH4.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public decimal TotalAmount { get; set; }
		public string Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
	}
}
