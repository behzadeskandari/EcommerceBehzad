namespace EcommerceBehzad.Domain.Entities
{
    public class NintendoGame : BaseProduct
    {
        public string Platform { get; private set; } = null!; // e.g., "Switch", "3DS"
        public int StockQuantity { get; private set; }
        public bool IsDigital { get; private set; }
        public string? DigitalKey { get; private set; }

        private NintendoGame() { }

        public NintendoGame(string name, string description, decimal price, string coverImageUrl, Guid categoryId, string platform, int stockQuantity, bool isDigital, string? digitalKey = null)
        {
            Name = name;
            Description = description;
            Price = price;
            CoverImageUrl = coverImageUrl;
            CategoryId = categoryId;
            Platform = platform;
            StockQuantity = stockQuantity;
            IsDigital = isDigital;
            DigitalKey = digitalKey;
        }

        public void UpdateStock(int quantity)
        {
            if (StockQuantity + quantity < 0)
                throw new InvalidOperationException("Stock cannot be negative.");
            StockQuantity += quantity;
        }
    }
}
