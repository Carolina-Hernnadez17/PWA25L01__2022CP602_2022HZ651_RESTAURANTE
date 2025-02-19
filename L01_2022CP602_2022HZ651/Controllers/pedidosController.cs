using L01_2022CP602_2022HZ651.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022CP602_2022HZ651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly RestauranteContext _Pedidoscontext;

        public pedidosController(RestauranteContext context)
        {
            _Pedidoscontext = context;
        }

        //// Obtener todos los pedidos con JOIN para incluir detalles del cliente y motorista
        //[HttpGet]
        //[Route("GetAll")]
        //public IActionResult GetAll()
        //{
        //    var pedidos = _Pedidoscontext.pedidos
        //        .Include(p => p.cliente) // Relación con clientes
        //        .Include(p => p.Motorista) // Relación con motoristas
        //        .Include(p => p.Plato) // Relación con platos
        //        .ToList();

        //    if (!pedidos.Any())
        //    {
        //        return NotFound("No hay pedidos registrados.");
        //    }

        //    return Ok(pedidos);
        //}

        //// ✅ Obtener pedidos filtrados por cliente
        //[HttpGet]
        //[Route("GetByCliente/{clienteId}")]
        //public IActionResult GetByCliente(int clienteId)
        //{
        //    var pedidos = _Pedidoscontext.pedidos
        //        .Where(p => p.clienteId == clienteId)
        //        .Include(p => p.Cliente)
        //        .Include(p => p.Plato)
        //        .ToList();

        //    if (!pedidos.Any())
        //    {
        //        return NotFound($"No hay pedidos para el cliente con ID {clienteId}.");
        //    }

        //    return Ok(pedidos);
        //}

        //// ✅ Obtener pedidos filtrados por motorista
        //[HttpGet]
        //[Route("GetByMotorista/{motoristaId}")]
        //public IActionResult GetByMotorista(int motoristaId)
        //{
        //    var pedidos = _Pedidoscontext.pedidos
        //        .Where(p => p.motoristaId == motoristaId)
        //        .Include(p => p.Motorista)
        //        .Include(p => p.Plato)
        //        .ToList();

        //    if (!pedidos.Any())
        //    {
        //        return NotFound($"No hay pedidos asignados al motorista con ID {motoristaId}.");
        //    }

        //    return Ok(pedidos);
        //}

        // ✅ Agregar un nuevo pedido
        [HttpPost]
        [Route("Add")]
        public IActionResult AddPedido([FromBody] pedidos pedido)
        {
            try
            {
                _Pedidoscontext.pedidos.Add(pedido);
                _Pedidoscontext.SaveChanges();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al guardar el pedido: {ex.Message}");
            }
        }

        // ✅ Actualizar un pedido
        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult UpdatePedido(int id, [FromBody] pedidos pedidoActualizar)
        {
            var pedidoActual = _Pedidoscontext.pedidos.Find(id);

            if (pedidoActual == null)
            {
                return NotFound($"No se encontró el pedido con ID {id}.");
            }

            pedidoActual.motoristaId = pedidoActualizar.motoristaId;
            pedidoActual.clienteId = pedidoActualizar.clienteId;
            pedidoActual.platoId = pedidoActualizar.platoId;
            pedidoActual.cantidad = pedidoActualizar.cantidad;
            pedidoActual.precio = pedidoActualizar.precio;

            _Pedidoscontext.Entry(pedidoActual).State = EntityState.Modified;
            _Pedidoscontext.SaveChanges();

            return Ok(pedidoActual);
        }

        // ✅ Eliminar un pedido
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeletePedido(int id)
        {
            var pedido = _Pedidoscontext.pedidos.Find(id);

            if (pedido == null)
            {
                return NotFound($"No se encontró el pedido con ID {id}.");
            }

            _Pedidoscontext.pedidos.Remove(pedido);
            _Pedidoscontext.SaveChanges();

            return Ok($"Pedido con ID {id} eliminado correctamente.");
        }
    }
}
