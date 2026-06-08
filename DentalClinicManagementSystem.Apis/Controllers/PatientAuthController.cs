using DentalClinicManagementSystem.BLL.DTOs.Auth;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinicManagementSystem.Apis.Controllers
{
    [ApiController]
    [Route("api/patient/auth")]
    public class PatientAuthController : ControllerBase
    {
        private readonly IPatientAuthService _patientAuthService;

        public PatientAuthController(IPatientAuthService patientAuthService)
        {
            _patientAuthService = patientAuthService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<PatientLoginResponseDto>> Register(PatientRegisterDto dto)
        {
            var result = await _patientAuthService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<PatientLoginResponseDto>> Login(PatientLoginDto dto)
        {
            var result = await _patientAuthService.LoginAsync(dto);
            return Ok(result);
        }
    }
}