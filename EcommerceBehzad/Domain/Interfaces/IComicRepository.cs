using EcommerceBehzad.Domain.Entities;

namespace EcommerceBehzad.Domain.Interfaces
{
    public interface IComicRepository
    {
        Task<ComicBook?> GetByIdAsync(Guid id);
        Task<IEnumerable<ComicBook>> GetAllAsync();
        Task AddAsync(ComicBook comic);
        void Update(ComicBook comic);
        void Delete(ComicBook comic);
    }
}
