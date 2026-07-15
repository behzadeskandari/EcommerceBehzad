namespace EcommerceBehzad.Domain.Entities
{
    public class Order : Entity
    {
        public string CustomerEmail { get; private set; } = null!;
        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; private set; }
        public virtual ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

        private Order() { }

        public Order(string customerEmail, List<OrderItem> items)
        {
            CustomerEmail = customerEmail;
            OrderItems = items;
            TotalAmount = items.Sum(item => item.Price * item.Quantity);
        }
    }
}
