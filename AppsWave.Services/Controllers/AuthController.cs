using Microsoft.AspNetCore.Mvc;

namespace AppsWave.Services.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppsWave.Services.Services.Auth.IAuthService _authService;
        protected AppsWave.DTO.ResponseDTO _responseDTO;

        public AuthController(AppsWave.Services.Services.Auth.IAuthService authService)
        {
            _authService = authService;
            _responseDTO = new();
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] AppsWave.DTO.RegisterationRequestDTO requestDTO)
        {
            var errorMessage = await _authService.Register(requestDTO);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = errorMessage;
                return BadRequest(_responseDTO);
            }
            return Ok(_responseDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppsWave.DTO.LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _authService.Login(loginRequestDTO);

            if (loginResponse.User is null)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Email or password is incorrect.";
                return BadRequest(_responseDTO);
            }
            _responseDTO.Result = loginResponse;
            return Ok(_responseDTO);
        }
    }
}
