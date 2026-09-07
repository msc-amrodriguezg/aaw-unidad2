using Api.Data;
using Api.Models;
using Api.GraphQL.Inputs;
using Api.GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Mutations
{
    public class ProductoMutation
    {
        public async Task<Producto> CreateProducto(
            CreateProductoInput input,
            [Service] AppDbContext context,
            CancellationToken cancellationToken)
        {
            var producto = new Producto
            {
                Nombre = input.Nombre,
                Descripcion = input.Descripcion,
                Precio = input.Precio
            };

            context.Productos.Add(producto);
            await context.SaveChangesAsync(cancellationToken);

            return producto;
        }

        public async Task<Producto?> UpdateProducto(
            UpdateProductoInput input,
            [Service] AppDbContext context,
            CancellationToken cancellationToken)
        {
            var producto = await context.Productos.FindAsync(new object[] { input.Id }, cancellationToken);
            if (producto is null)
            {
                return null;
            }

            producto.Nombre = input.Nombre;
            producto.Descripcion = input.Descripcion;
            producto.Precio = input.Precio;

            await context.SaveChangesAsync(cancellationToken);

            return producto;
        }

        public async Task<bool> DeleteProducto(
            DeleteProductoInput input,
            [Service] AppDbContext context,
            CancellationToken cancellationToken)
        {
            var producto = await context.Productos.FindAsync(new object[] { input.Id }, cancellationToken);
            if (producto is null)
            {
                return false;
            }

            context.Productos.Remove(producto);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

    public class ProductoMutationType : ObjectType<ProductoMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductoMutation> descriptor)
        {
            descriptor.Description("Mutaciones relacionadas con productos");

            descriptor.Field(p => p.CreateProducto(default!, default!, default!))
                .Description("Crea un nuevo producto")
                .Argument("input", a => a.Description("Datos del producto a crear"))
                .Type<NonNullType<ProductoType>>();

            descriptor.Field(p => p.UpdateProducto(default!, default!, default!))
                .Description("Actualiza un producto existente")
                .Argument("input", a => a.Description("Datos del producto a actualizar"))
                .Type<ProductoType>();

            descriptor.Field(p => p.DeleteProducto(default!, default!, default!))
                .Description("Elimina un producto")
                .Argument("input", a => a.Description("Identificador del producto a eliminar"))
                .Type<NonNullType<BooleanType>>();
        }
    }
}