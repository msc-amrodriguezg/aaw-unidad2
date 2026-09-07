using Api.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Api.GraphQL.Types
{
    public class ProductoType : ObjectType<Producto>
    {
        protected override void Configure(IObjectTypeDescriptor<Producto> descriptor)
        {
            descriptor.Description("Representa un producto en el sistema");

            descriptor.Field(p => p.Id)
                .Description("Identificador único del producto");

            descriptor.Field(p => p.Nombre)
                .Description("Nombre del producto");

            descriptor.Field(p => p.Descripcion)
                .Description("Descripción del producto");

            descriptor.Field(p => p.Precio)
                .Description("Precio del producto");
        }
    }
}