using Api.Data;
using Api.Models;
using MediatR;

namespace Api.Features.Productos.Commands
{
    public record CreateProductoCommand(string Nombre, string Descripcion, decimal Precio) : IRequest<Producto>;

    public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, Producto>
    {
        private readonly AppDbContext _context;

        public CreateProductoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Producto> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = new Producto
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Precio = request.Precio
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync(cancellationToken);

            return producto;
        }
    }
}
