using EcommerceBehzad.Domain.Interfaces;
using EcommerceBehzad.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBehzad.Application.Comics.Queries
{
    public record GetComicStreamQuery(Guid ComicId, string UserEmail) : IRequest<ComicStreamResult>;
    public record ComicStreamResult(Stream Stream, string FileName);

    public class GetComicStreamQueryHandler : IRequestHandler<GetComicStreamQuery, ComicStreamResult>
    {
        private readonly AppDbContext _context;
        private readonly IComicFileRepository _fileRepository;

        public GetComicStreamQueryHandler(AppDbContext context, IComicFileRepository fileRepository)
        {
            _context = context;
            _fileRepository = fileRepository;
        }

        public async Task<ComicStreamResult> Handle(GetComicStreamQuery request, CancellationToken cancellationToken)
        {
            // 1. Verify that this user bought this digital comic
            var hasPurchased = await _context.Orders
                .AsNoTracking()
                .AnyAsync(o => o.CustomerEmail == request.UserEmail &&
                               o.OrderItems.Any(oi => oi.ProductId == request.ComicId),
                          cancellationToken);

            if (!hasPurchased)
            {
                throw new UnauthorizedAccessException("You have not purchased access to this comic book.");
            }

            // 2. Resolve internal MongoDB pointer location
            var comic = await _context.Comics
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.ComicId, cancellationToken);

            if (comic == null)
                throw new KeyNotFoundException("Comic metadata not resolved.");

            // 3. Open on-demand network chunk stream from MongoDB GridFS
            var stream = await _fileRepository.DownloadFileAsync(comic.MongoFileId);

            return new ComicStreamResult(stream, $"{comic.Name.Replace(" ", "_")}.pdf");
        }
    }
}
