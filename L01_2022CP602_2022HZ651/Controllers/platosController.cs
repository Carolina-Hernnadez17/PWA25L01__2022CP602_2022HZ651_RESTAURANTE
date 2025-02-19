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
        /// EndPoint que retorna el listado de todos los libros existentes
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
        /// EndPoint que retorna el registro de un libro por su Id, incluyendo el nombre del autor
        /// </summary>
        /// <returns></returns>

        //[HttpGet]
        //[Route("GetById/{id}")]
        //public IActionResult Get(int id)
        //{
        //    var resultado = (from e in _RestauranteContexto.Libro
        //                     join a in _RestauranteContexto.Autor
        //                     on e.AutorId equals a.Id
        //                     where e.Id == id
        //                     select new
        //                     {
        //                         e.Id,
        //                         e.Titulo,
        //                         e.AnioPublicacion,
        //                         e.AutorId,
        //                         e.CategoriaId,
        //                         e.Resumen,
        //                         Nombre_Autor = a.Nombre
        //                     }).FirstOrDefault();

        //    if (resultado == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(resultado);
        //}



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

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPlatos(int id, [FromBody] platos platosModificar)
        {
            // Para actualizar un registro, se obtiene el registro original de la base de datos
            // al cual alteraremos alguna propiedad
            platos? platosActual = (from e in _RestauranteContexto.platos
                                    where e.platoId == id
                                    select e).FirstOrDefault();

            // Verificamos que exista el registro segun su ID
            if (platosActual == null)
            {
                return NotFound();
            }

            // Si se encuentra el registro, se alteran los campos modificables
            platosActual.nombrePlato = platosModificar.nombrePlato;
            platosActual.precio = platosModificar.precio;

            // Se marca el registro como modificado en el contexto
            // y se envia la modificacion a la base de datos
            _RestauranteContexto.Entry(platosActual).State = EntityState.Modified;
            _RestauranteContexto.SaveChanges();

            return Ok(platosModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult Eliminarplatos(int id)
        {
            // Para actualizar un registro, se obtiene el registro original de la base de datos
            // al cual eliminaremos
            platos? platos = (from e in _RestauranteContexto.platos
                              where e.platoId == id
                              select e).FirstOrDefault();

            // Verificamos que exista el registro según su ID
            if (platos == null)
            {
                return NotFound();
            }

            // Ejecutamos la acción de eliminar el registro
            _RestauranteContexto.platos.Attach(platos);
            _RestauranteContexto.platos.Remove(platos);
            _RestauranteContexto.SaveChanges();

            return Ok(platos);
        }
        /// <summary>
        /// EndPoint que retorna el registro de un libro por su Id, incluyendo el nombre del autor
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            List<platos> platos = (from e in _RestauranteContexto.platos
                                   where e.precio < id
                                   select e).ToList();

            if (!platos.Any())
            {
                return NotFound();
            }

            return Ok(platos);
        }
    }
}
