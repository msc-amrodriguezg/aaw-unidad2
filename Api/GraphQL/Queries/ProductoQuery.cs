using Api.Data;
using Api.Models;
using Api.GraphQL.Types;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Queries
{
    public class ProductoQuery
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Producto> GetProductos([Service] AppDbContext context)
        {
            return context.Productos.AsNoTracking();
        }

        public async Task<Producto?> GetProductoById(
            int id,
            [Service] AppDbContext context,
            CancellationToken cancellationToken)
        {
            return await context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }

    public class ProductoQueryType : ObjectType<ProductoQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductoQuery> descriptor)
        {
            descriptor.Description("Consultas relacionadas con productos");

            descriptor.Field(p => p.GetProductos(default!))
                .Description("Obtiene la lista de todos los productos")
                .Type<ListType<NonNullType<ProductoType>>>();

            descriptor.Field(p => p.GetProductoById(default!, default!, default!))
                .Description("Obtiene un producto por su ID")
                .Argument("id", a => a.Description("Identificador del producto"))
                .Type<ProductoType>();
        }
    }
}