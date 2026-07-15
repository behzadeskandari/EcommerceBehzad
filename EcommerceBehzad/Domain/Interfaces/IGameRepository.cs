using EcommerceBehzad.Domain.Entities;

namespace EcommerceBehzad.Domain.Interfaces
{
    public interface IGameRepository
    {
        Task<NintendoGame?> GetByIdAsync(Guid id);
        Task<IEnumerable<NintendoGame>> GetAllAsync();
        Task AddAsync(NintendoGame game);
        void Update(NintendoGame game);
        void Delete(NintendoGame game);
    }
}
