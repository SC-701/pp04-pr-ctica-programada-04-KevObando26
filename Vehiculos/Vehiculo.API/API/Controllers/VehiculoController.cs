using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiculoController : ControllerBase, IVehiculoController
    {
        private IVehiculoFujo _vehiculoFlujo;
        private ILogger<VehiculoController> _logger;

        public VehiculoController(IVehiculoFujo vehiculoFlujo, ILogger<VehiculoController> logger)
        {
            _vehiculoFlujo = vehiculoFlujo;
            _logger = logger;
        }

        #region OPERACIONESCRUD
        [HttpPost]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Agregar([FromBody] VehiculoRequest vehiculo)
        {
            var resultado = await _vehiculoFlujo.Agregar(vehiculo);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado }, resultado);
        }
        [Authorize(Roles = "2")]
        [HttpPut ("{Id}") ]
        public async Task<IActionResult> Editar([FromRoute] Guid Id, [FromBody] VehiculoRequest vehiculo)
        {
            if (!await VerificarVehiculoExiste(Id))
                return NotFound("El vehiculo no existe.");
            var resultado = await _vehiculoFlujo.Editar(Id, vehiculo);
            return Ok(resultado);
        }


        [Authorize(Roles = "2")]
        [HttpDelete ("{Id}")]
        public async Task<IActionResult> Eliminar([FromRoute] Guid Id)
        {
            if (!await VerificarVehiculoExiste(Id))
                return NotFound("El vehiculo no existe.");

            var resultado = await _vehiculoFlujo.Eliminar(Id);
            return NoContent();
        }
        [Authorize(Roles = "1")]
        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _vehiculoFlujo.Obtener();
            if (!resultado.Any())
            
            return NoContent();
            return Ok(resultado);
        }
        [Authorize(Roles = "1")]
        [HttpGet("ObtenerPorId/{Id}")]
        public async Task<IActionResult> Obtener([FromRoute] Guid Id)
        {
            var resultado = await _vehiculoFlujo.Obtener(Id);
            return Ok(resultado);
        }
       
        
        #endregion OPERACIONESCRUD

        #region HELPERS
        private async Task<bool> VerificarVehiculoExiste(Guid Id)
        {
            var resultadoValidacion = false;
            var resultadoVehiculoExiste = await _vehiculoFlujo.Obtener(Id);
            if (resultadoVehiculoExiste != null)
                resultadoValidacion = true;
            return resultadoValidacion;
        }
        #endregion HELPERS
    }
}
