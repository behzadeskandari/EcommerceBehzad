using System.Text.Json;
using EcommerceBehzad.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
namespace EcommerceBehzad.Infrastructure.Persistence.Repositories
{
    public class ComicFileRepository : IComicFileRepository
    {
        private readonly IGridFSBucket _bucket;

        public ComicFileRepository(IMongoDatabase database)
        {
            // Custom bucket name "comic_vault"
            _bucket = new GridFSBucket(database, new GridFSBucketOptions
            {
                BucketName = "comic_vault",
                ChunkSizeBytes = 1048576 // Optimized to 1MB chunks instead of the 255KB default for larger files
            });
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument("UploadedAt", DateTime.UtcNow)
            };

            var id = await _bucket.UploadFromStreamAsync(fileName, fileStream, options);
            return id.ToString();
        }

        public async Task<Stream> DownloadFileAsync(string fileId)
        {
            if (!ObjectId.TryParse(fileId, out var objectId))
            {
                throw new ArgumentException("Invalid Mongo File ID format.", nameof(fileId));
            }

            // OpenDownloadStreamAsync lets us stream chunks on-demand without memory overload
            return await _bucket.OpenDownloadStreamAsync(objectId);
        }

        public async Task DeleteFileAsync(string fileId)
        {
            if (!ObjectId.TryParse(fileId, out var objectId))
            {
                throw new ArgumentException("Invalid Mongo File ID format.", nameof(fileId));
            }

            await _bucket.DeleteAsync(objectId);
        }
    }
}
