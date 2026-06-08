using DentalClinicManagementSystem.BLL.CurrentUser;
using DentalClinicManagementSystem.BLL.DTOs;
using DentalClinicManagementSystem.BLL.DTOs.Appointments;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinicManagementSystem.Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICurrentUserService _currentUserService;

        public AppointmentsController(IAppointmentService appointmentService, ICurrentUserService currentUserService)
        {
            _appointmentService = appointmentService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll(
            [FromQuery] DateTime? date,
            [FromQuery] int? doctorId,
            [FromQuery] int? status)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var appointments = await _appointmentService.GetAllAsync(date, doctorId, status, currentUserId, currentUserRole);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetToday()
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var appointments = await _appointmentService.GetTodayAsync(currentUserId, currentUserRole);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound(new { message = $"Appointment with ID {id} not found" });
                }
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto dto)
        {
            try
            {
                var result = await _appointmentService.CreateAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ActionResult<AppointmentDto>> Update(int id, UpdateAppointmentDto dto)
        {
            try
            {
                var result = await _appointmentService.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ActionResult<AppointmentDto>> Cancel(int id)
        {
            try
            {
                var result = await _appointmentService.CancelAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ActionResult<AppointmentDto>> Complete(int id)
        {
            try
            {
                var result = await _appointmentService.CompleteAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}