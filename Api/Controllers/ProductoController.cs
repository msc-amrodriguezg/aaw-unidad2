using Api.Features.Productos.Commands;
using Api.Features.Productos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Producto>>> GetAll(CancellationToken cancellationToken)
        {
            var productos = await _mediator.Send(new GetProductosQuery(), cancellationToken);

            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Models.Producto>> GetById(int id, CancellationToken cancellationToken)
        {
            var producto = await _mediator.Send(new GetProductoByIdQuery(id), cancellationToken);
            if (producto is null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Models.Producto>> Create(Models.Producto producto, CancellationToken cancellationToken)
        {
            var creado = await _mediator.Send(
                new CreateProductoCommand(producto.Nombre, producto.Descripcion, producto.Precio),
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Models.Producto producto, CancellationToken cancellationToken)
        {
            var actualizado = await _mediator.Send(
                new UpdateProductoCommand(id, producto.Nombre, producto.Descripcion, producto.Precio),
                cancellationToken);

            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var eliminado = await _mediator.Send(new DeleteProductoCommand(id), cancellationToken);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
