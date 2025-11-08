using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MiAppCharca.Application.UseCases.Responses.Commands.CreateResponse;
using MiAppCharca.Application.UseCases.Responses.Commands.UpdateResponse;
using MiAppCharca.Application.UseCases.Responses.Commands.DeleteResponse;
using MiAppCharca.Application.UseCases.Responses.Queries.GetAllResponses;
using MiAppCharca.Application.UseCases.Responses.Queries.GetResponseById;
using MiAppCharca.Application.UseCases.Responses.Queries.GetResponsesByTicketId;

namespace MiAppCharca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResponsesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResponsesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener todas las respuestas (Solo Admin/Support)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> GetAllResponses()
        {
            try
            {
                var query = new GetAllResponsesQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener respuesta por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResponseById(Guid id)
        {
            try
            {
                var query = new GetResponseByIdQuery { ResponseId = id };
                var result = await _mediator.Send(query);
                
                if (result == null)
                    return NotFound(new { message = "Respuesta no encontrada" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener respuestas de un ticket específico
        /// </summary>
        [HttpGet("ticket/{ticketId}")]
        public async Task<IActionResult> GetResponsesByTicketId(Guid ticketId)
        {
            try
            {
                var query = new GetResponsesByTicketIdQuery { TicketId = ticketId };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Crear una nueva respuesta a un ticket
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateResponse([FromBody] CreateResponseCommand command)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "Usuario no autenticado" });

                command.ResponderId = Guid.Parse(userIdClaim);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(GetResponseById), new { id = result.ResponseId }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar una respuesta
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResponse(Guid id, [FromBody] UpdateResponseCommand command)
        {
            try
            {
                command.ResponseId = id;
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar una respuesta (Solo Admin o autor)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse(Guid id)
        {
            try
            {
                var command = new DeleteResponseCommand { ResponseId = id };
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { message = "Respuesta no encontrada" });

                return Ok(new { message = "Respuesta eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}