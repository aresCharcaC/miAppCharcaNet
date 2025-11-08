using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MiAppCharca.Application.UseCases.Tickets.Commands.CreateTicket;
using MiAppCharca.Application.UseCases.Tickets.Commands.UpdateTicket;
using MiAppCharca.Application.UseCases.Tickets.Commands.CloseTicket;
using MiAppCharca.Application.UseCases.Tickets.Commands.DeleteTicket;
using MiAppCharca.Application.UseCases.Tickets.Queries.GetAllTickets;
using MiAppCharca.Application.UseCases.Tickets.Queries.GetTicketById;
using MiAppCharca.Application.UseCases.Tickets.Queries.GetMyTickets;
using MiAppCharca.Application.UseCases.Tickets.Queries.GetTicketsByStatus;

namespace MiAppCharca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtener todos los tickets
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            try
            {
                var query = new GetAllTicketsQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener ticket por ID con sus respuestas
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(Guid id)
        {
            try
            {
                var query = new GetTicketByIdQuery { TicketId = id };
                var result = await _mediator.Send(query);
                
                if (result == null)
                    return NotFound(new { message = "Ticket no encontrado" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener tickets del usuario actual
        /// </summary>
        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "Usuario no autenticado" });

                var userId = Guid.Parse(userIdClaim);
                var query = new GetMyTicketsQuery { UserId = userId };
                var result = await _mediator.Send(query);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener tickets por estado
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTicketsByStatus(string status)
        {
            try
            {
                var validStatuses = new[] { "abierto", "en_proceso", "cerrado" };
                if (!validStatuses.Contains(status.ToLower()))
                    return BadRequest(new { message = "Estado inválido. Use: abierto, en_proceso, cerrado" });

                var query = new GetTicketsByStatusQuery { Status = status.ToLower() };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Crear un nuevo ticket
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand command)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "Usuario no autenticado" });

                command.UserId = Guid.Parse(userIdClaim);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(GetTicketById), new { id = result.TicketId }, result);
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
        /// Actualizar un ticket
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] UpdateTicketCommand command)
        {
            try
            {
                command.TicketId = id;
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
        /// Cerrar un ticket
        /// </summary>
        [HttpPatch("{id}/close")]
        public async Task<IActionResult> CloseTicket(Guid id)
        {
            try
            {
                var command = new CloseTicketCommand { TicketId = id };
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { message = "Ticket no encontrado" });

                return Ok(new { message = "Ticket cerrado exitosamente" });
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
        /// Eliminar un ticket (Solo Admin)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            try
            {
                var command = new DeleteTicketCommand { TicketId = id };
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(new { message = "Ticket no encontrado" });

                return Ok(new { message = "Ticket eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}