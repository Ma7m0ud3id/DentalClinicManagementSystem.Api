using DentalClinicManagementSystem.BLL.CurrentUser;
using DentalClinicManagementSystem.BLL.DTOs.Dashboard;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinicManagementSystem.Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICurrentUserService _currentUserService;

        public DashboardController(IDashboardService dashboardService, ICurrentUserService currentUserService)
        {
            _dashboardService = dashboardService;
            _currentUserService = currentUserService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var currentUserRole = _currentUserService.Role;

                var stats = await _dashboardService.GetStatsAsync(currentUserId, currentUserRole);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}