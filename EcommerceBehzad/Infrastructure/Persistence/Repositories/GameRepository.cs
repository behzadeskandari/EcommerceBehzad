using EcommerceBehzad.Domain.Entities;
using EcommerceBehzad.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBehzad.Infrastructure.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context) => _context = context;

        public async Task<NintendoGame?> GetByIdAsync(Guid id) =>
            await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

        public async Task<IEnumerable<NintendoGame>> GetAllAsync() =>
            await _context.Games.ToListAsync();

        public async Task AddAsync(NintendoGame game) =>
            await _context.Games.AddAsync(game);

        public void Update(NintendoGame game) =>
            _context.Games.Update(game);

        public void Delete(NintendoGame game) =>
            _context.Games.Remove(game);
    }
}
