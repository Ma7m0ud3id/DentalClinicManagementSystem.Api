using DentalClinicManagementSystem.BLL.CurrentUser;
using DentalClinicManagementSystem.BLL.DTOs.Visits;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinicManagementSystem.Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly ICurrentUserService _currentUserService;

        public VisitsController(IVisitService visitService, ICurrentUserService currentUserService)
        {
            _visitService = visitService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetAll()
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var visits = await _visitService.GetAllAsync(currentUserId, currentUserRole);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitDto>> GetById(int id)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var visit = await _visitService.GetByIdAsync(id, currentUserId, currentUserRole);
                if (visit == null)
                {
                    return NotFound(new { message = $"Visit with ID {id} not found" });
                }
                return Ok(visit);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetByPatient(int patientId)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var visits = await _visitService.GetByPatientIdAsync(patientId, currentUserId, currentUserRole);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<VisitDto>> Create(CreateVisitDto dto)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var result = await _visitService.CreateAsync(dto, currentUserId, currentUserRole);
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
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
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
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<VisitDto>> Update(int id, UpdateVisitDto dto)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var result = await _visitService.UpdateAsync(id, dto, currentUserId, currentUserRole);
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
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
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
    }
}