namespace EcommerceBehzad.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; private set; } = null!;
        public Guid? ParentCategoryId { get; private set; }
        public virtual Category? ParentCategory { get; private set; }
        public virtual ICollection<Category> SubCategories { get; private set; } = new List<Category>();
        public virtual ICollection<BaseProduct> Products { get; private set; } = new List<BaseProduct>();

        private Category() { } // EF Core Required

        public Category(string name, Guid? parentCategoryId = null)
        {
            Name = name;
            ParentCategoryId = parentCategoryId;
        }
    }
}
