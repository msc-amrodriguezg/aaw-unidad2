using HotChocolate;
using HotChocolate.Types;

namespace Api.GraphQL.Inputs
{
    public class CreateProductoInput
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
    }

    public class CreateProductoInputType : InputObjectType<CreateProductoInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateProductoInput> descriptor)
        {
            descriptor.Description("Entrada para crear un nuevo producto");

            descriptor.Field(p => p.Nombre)
                .Description("Nombre del producto")
                .Type<NonNullType<StringType>>();

            descriptor.Field(p => p.Descripcion)
                .Description("Descripción del producto")
                .Type<NonNullType<StringType>>();

            descriptor.Field(p => p.Precio)
                .Description("Precio del producto")
                .Type<NonNullType<DecimalType>>();
        }
    }

    public class UpdateProductoInput
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
    }

    public class UpdateProductoInputType : InputObjectType<UpdateProductoInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProductoInput> descriptor)
        {
            descriptor.Description("Entrada para actualizar un producto existente");

            descriptor.Field(p => p.Id)
                .Description("Identificador del producto")
                .Type<NonNullType<IntType>>();

            descriptor.Field(p => p.Nombre)
                .Description("Nombre del producto")
                .Type<NonNullType<StringType>>();

            descriptor.Field(p => p.Descripcion)
                .Description("Descripción del producto")
                .Type<NonNullType<StringType>>();

            descriptor.Field(p => p.Precio)
                .Description("Precio del producto")
                .Type<NonNullType<DecimalType>>();
        }
    }

    public class DeleteProductoInput
    {
        public int Id { get; set; }
    }

    public class DeleteProductoInputType : InputObjectType<DeleteProductoInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteProductoInput> descriptor)
        {
            descriptor.Description("Entrada para eliminar un producto");

            descriptor.Field(p => p.Id)
                .Description("Identificador del producto")
                .Type<NonNullType<IntType>>();
        }
    }
}