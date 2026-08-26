using Api.Data;
using Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Productos.Queries
{
    public record GetProductosQuery : IRequest<List<Producto>>;

    public class GetProductosQueryHandler : IRequestHandler<GetProductosQuery, List<Producto>>
    {
        private readonly AppDbContext _context;

        public GetProductosQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Producto>> Handle(GetProductosQuery request, CancellationToken cancellationToken)
        {
            return _context.Productos.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
