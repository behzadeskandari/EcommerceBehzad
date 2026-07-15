using EcommerceBehzad.Domain.Entities;
using EcommerceBehzad.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBehzad.Infrastructure.Persistence.Repositories
{
    public class ComicRepository : IComicRepository
    {
        private readonly AppDbContext _context;
        public ComicRepository(AppDbContext context) => _context = context;

        public async Task<ComicBook?> GetByIdAsync(Guid id) =>
            await _context.Comics.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<ComicBook>> GetAllAsync() =>
            await _context.Comics.ToListAsync();

        public async Task AddAsync(ComicBook comic) =>
            await _context.Comics.AddAsync(comic);

        public void Update(ComicBook comic) =>
            _context.Comics.Update(comic);

        public void Delete(ComicBook comic) =>
            _context.Comics.Remove(comic);
    }
}
