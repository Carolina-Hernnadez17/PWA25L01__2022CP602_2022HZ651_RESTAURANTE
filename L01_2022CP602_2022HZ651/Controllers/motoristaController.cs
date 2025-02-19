using L01_2022CP602_2022HZ651.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022CP602_2022HZ651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class motoristaController : ControllerBase
    {
        private readonly RestauranteContext _RestauranteContext;

        public motoristaController(RestauranteContext restauranteContext)
        {
            _RestauranteContext = restauranteContext;
        }

        // Obtener todos los motoristas
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var motoristas = _RestauranteContext.motoristas.ToList();

            if (!motoristas.Any())
            {
                return NotFound("No hay motoristas registrados.");
            }

            return Ok(motoristas);
        }

        // Obtener motoristas filtrados por nombre
        [HttpGet]
        [Route("GetByNombre/{nombre}")]
        public IActionResult GetByNombre(string nombre)
        {
            var motoristas = _RestauranteContext.motoristas
                .Where(m => m.nombreMotorista.Contains(nombre))
                .ToList();

            if (!motoristas.Any())
            {
                return NotFound($"No hay motoristas con el nombre '{nombre}'.");
            }

            return Ok(motoristas);
        }

        // Agregar un nuevo motorista
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] motoristas motorista)
        {
            try
            {
                _RestauranteContext.motoristas.Add(motorista);
                _RestauranteContext.SaveChanges();
                return Ok(motorista);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar motorista: {ex.Message}");
            }
        }

        // Actualizar un motorista
        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update(int id, [FromBody] motoristas motoristaModificar)
        {
            var motoristaActual = _RestauranteContext.motoristas.Find(id);

            if (motoristaActual == null)
            {
                return NotFound($"No se encontró un motorista con ID {id}.");
            }

            motoristaActual.nombreMotorista = motoristaModificar.nombreMotorista;

            _RestauranteContext.Entry(motoristaActual).State = EntityState.Modified;
            _RestauranteContext.SaveChanges();

            return Ok(motoristaActual);
        }

        // Eliminar un motorista
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var motorista = _RestauranteContext.motoristas.Find(id);

            if (motorista == null)
            {
                return NotFound($"No se encontró un motorista con ID {id}.");
            }

            _RestauranteContext.motoristas.Remove(motorista);
            _RestauranteContext.SaveChanges();

            return Ok($"Motorista con ID {id} eliminado correctamente.");
        }
    }
}
