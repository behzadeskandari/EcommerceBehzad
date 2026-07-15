namespace EcommerceBehzad.Domain.Interfaces
{
    public interface IComicFileRepository
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadFileAsync(string fileId);
        Task DeleteFileAsync(string fileId);
    }
}
