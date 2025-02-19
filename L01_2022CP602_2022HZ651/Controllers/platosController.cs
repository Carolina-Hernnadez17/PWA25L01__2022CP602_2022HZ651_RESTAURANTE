using L01_2022CP602_2022HZ651.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022CP602_2022HZ651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly RestauranteContext _RestauranteContexto;

        public platosController(RestauranteContext LibroContexto)
        {
            _RestauranteContexto = LibroContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los platos existentes
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<platos> listadoLibro = (from e in _RestauranteContexto.platos
                                         select e).ToList();
            if (listadoLibro.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoLibro);
        }

        /// <summary>
        /// EndPoint guardar platos
        /// </summary>
        /// <returns></returns>



        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarPlatos([FromBody] platos platos)
        {
            try
            {
                _RestauranteContexto.platos.Add(platos);
                _RestauranteContexto.SaveChanges();
                return Ok(platos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// EndPoint actualizar platos
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPlatos(int id, [FromBody] platos platosModificar)
        {
            platos? platosActual = (from e in _RestauranteContexto.platos
                                    where e.platoId == id
                                    select e).FirstOrDefault();

            if (platosActual == null)
            {
                return NotFound();
            }

            platosActual.nombrePlato = platosModificar.nombrePlato;
            platosActual.precio = platosModificar.precio;

            _RestauranteContexto.Entry(platosActual).State = EntityState.Modified;
            _RestauranteContexto.SaveChanges();

            return Ok(platosModificar);
        }
        /// <summary>
        /// EndPoint eliminar platos
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult Eliminarplatos(int id)
        {
            platos? platos = (from e in _RestauranteContexto.platos
                              where e.platoId == id
                              select e).FirstOrDefault();

            if (platos == null)
            {
                return NotFound();
            }

            _RestauranteContexto.platos.Attach(platos);
            _RestauranteContexto.platos.Remove(platos);
            _RestauranteContexto.SaveChanges();

            return Ok(platos);
        }
        /// <summary>
        /// EndPoint para retornar el listado de los platos filtrados cuando el precio sea menor de un valor dado.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetById/{precio}")]
        public IActionResult Get(decimal precio)
        {
            List<platos> platos = (from e in _RestauranteContexto.platos
                                   where e.precio < precio
                                   select e).ToList();

            if (!platos.Any())
            {
                return NotFound();
            }

            return Ok(platos);
        }
    }
}
