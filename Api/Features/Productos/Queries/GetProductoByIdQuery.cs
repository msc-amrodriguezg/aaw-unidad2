using Api.Data;
using Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Productos.Queries
{
    public record GetProductoByIdQuery(int Id) : IRequest<Producto?>;

    public class GetProductoByIdQueryHandler : IRequestHandler<GetProductoByIdQuery, Producto?>
    {
        private readonly AppDbContext _context;

        public GetProductoByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<Producto?> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
        {
            return _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}
