using Api.Data;
using MediatR;

namespace Api.Features.Productos.Commands
{
    public record UpdateProductoCommand(int Id, string Nombre, string Descripcion, decimal Precio) : IRequest<bool>;

    public class UpdateProductoCommandHandler : IRequestHandler<UpdateProductoCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateProductoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = await _context.Productos.FindAsync([request.Id], cancellationToken);
            if (producto is null)
            {
                return false;
            }

            producto.Nombre = request.Nombre;
            producto.Descripcion = request.Descripcion;
            producto.Precio = request.Precio;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
