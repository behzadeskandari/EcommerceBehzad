namespace EcommerceBehzad.Domain.Entities
{
    public abstract class BaseProduct : Entity
    {
        public string Name { get; protected set; } = null!;
        public string Description { get; protected set; } = null!;
        public decimal Price { get; protected set; }
        public string CoverImageUrl { get; protected set; } = null!;
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public Guid CategoryId { get; protected set; }
        public virtual Category Category { get; protected set; } = null!;

        protected BaseProduct() { }
    }
}
