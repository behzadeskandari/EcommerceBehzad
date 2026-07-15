using EcommerceBehzad.Domain.Entities;
using EcommerceBehzad.Infrastructure.Persistence;
using MediatR;

namespace EcommerceBehzad.Application.Orders.Commands
{
    public record OrderItemDto(Guid ProductId, int Quantity);
    public record CreateOrderCommand(string CustomerEmail, List<OrderItemDto> Items) : IRequest<Guid>;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly AppDbContext _context;

        public CreateOrderCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                var product = await _context.Products.FindAsync(new object[] { item.ProductId }, cancellationToken);
                if (product == null)
                    throw new KeyNotFoundException($"Product {item.ProductId} not found.");

                if (product is NintendoGame game)
                {
                    // Decrement stock in real-time
                    game.UpdateStock(-item.Quantity);
                }

                orderItems.Add(new OrderItem(product.Id, product.Price, item.Quantity));
            }

            var order = new Order(request.CustomerEmail, orderItems);
            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
