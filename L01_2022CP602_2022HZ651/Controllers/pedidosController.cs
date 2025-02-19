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

        // EndPoint que retornar el listado de pedidos filtrado por cliente

        [HttpGet]
        [Route("getBycliente/{nombreCliente}")]
        public IActionResult GetByCliente(string nombreCliente)
        {
            var resultado = (from p in _Pedidoscontext.pedidos
                             join c in _Pedidoscontext.clientes on p.clienteId equals c.clienteId
                             join m in _Pedidoscontext.motoristas on p.motoristaId equals m.motoristaId
                             join pl in _Pedidoscontext.platos on p.platoId equals pl.platoId
                             where c.nombreCliente == nombreCliente
                             select new
                             {
                                 pedidoId = p.pedidoId,
                                 p.cantidad,
                                 p.precio,
                                 NombreMotorista = m.nombreMotorista,
                                 NombrePlato = pl.nombrePlato,
                                 NombreCliente = c.nombreCliente
                             }).ToList();

            if (!resultado.Any())
            {
                return NotFound();
            }

            return Ok(resultado);
        }
        // EndPoint que retornar el listado de pedidos filtrado por motorista

        [HttpGet]
        [Route("getBymotorista/{nombreMotorista}")]
        public IActionResult GetByMotorista(string nombreMotorista)
        {
            var resultado = (from p in _Pedidoscontext.pedidos
                             join c in _Pedidoscontext.clientes on p.clienteId equals c.clienteId
                             join m in _Pedidoscontext.motoristas on p.motoristaId equals m.motoristaId
                             join pl in _Pedidoscontext.platos on p.platoId equals pl.platoId
                             where m.nombreMotorista == nombreMotorista
                             select new
                             {
                                 pedidoId = p.pedidoId,
                                 p.cantidad,
                                 p.precio,
                                 NombreMotorista = m.nombreMotorista,
                                 NombrePlato = pl.nombrePlato,
                                 NombreCliente = c.nombreCliente
                             }).ToList();

            if (!resultado.Any())
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        //EndPoint de agregar un nuevo pedido
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

        // EndPoint de actualizar un pedido
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

        // EndPoint de eliminar un pedido
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeletePedido(int id)
        {

            pedidos? pedido = (from e in _Pedidoscontext.pedidos
                               where e.pedidoId == id
                               select e).FirstOrDefault();

            if (pedido == null)
            {
                return NotFound($"No se encontró el pedido con ID {id}.");
            }

            _Pedidoscontext.pedidos.Remove(pedido);
            _Pedidoscontext.SaveChanges();

            return Ok($"Pedido con ID {id} eliminado correctamente.");
        }

        //EndPoint del TOP N de los platos que mas pedidos tienen
        [HttpGet]
        [Route("GetTopPlatos/{topN}")]
        public IActionResult GetTopPlatos(int topN)
        {
            var topPlatos = _Pedidoscontext.pedidos
                .GroupBy(p => p.platoId)
                .Select(g => new
                {
                    PlatoId = g.Key,
                    CantidadTotal = g.Sum(p => p.cantidad),
                    NombrePlato = _Pedidoscontext.platos
                        .Where(pl => pl.platoId == g.Key)
                        .Select(pl => pl.nombrePlato)
                        .FirstOrDefault()
                })
                .OrderByDescending(p => p.CantidadTotal)
                .Take(topN)
                .ToList();

            if (!topPlatos.Any())
            {
                return NotFound("No hay pedidos registrados.");
            }

            return Ok(topPlatos);
        }

    }
}
