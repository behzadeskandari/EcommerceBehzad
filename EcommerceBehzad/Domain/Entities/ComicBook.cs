namespace EcommerceBehzad.Domain.Entities
{
    public class ComicBook : BaseProduct
    {
        public string Author { get; private set; } = null!;
        public string Illustrator { get; private set; } = null!;
        public string Publisher { get; private set; } = null!;
        public int PageCount { get; private set; }
        public string MongoFileId { get; private set; } = null!; // References GridFS Object _id

        private ComicBook() { }

        public ComicBook(string name, string description, decimal price, string coverImageUrl, Guid categoryId, string author, string illustrator, string publisher, int pageCount, string mongoFileId)
        {
            Name = name;
            Description = description;
            Price = price;
            CoverImageUrl = coverImageUrl;
            CategoryId = categoryId;
            Author = author;
            Illustrator = illustrator;
            Publisher = publisher;
            PageCount = pageCount;
            MongoFileId = mongoFileId;
        }
    }
}
