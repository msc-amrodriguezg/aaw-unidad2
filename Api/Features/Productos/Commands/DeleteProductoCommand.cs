using Api.Data;
using MediatR;

namespace Api.Features.Productos.Commands
{
    public record DeleteProductoCommand(int Id) : IRequest<bool>;

    public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteProductoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = await _context.Productos.FindAsync([request.Id], cancellationToken);
            if (producto is null)
            {
                return false;
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
