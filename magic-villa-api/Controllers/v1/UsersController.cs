using magic_villa_api.Respository.IRepository;
using magic_villa_api.Models;
using magic_villa_api.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using magic_villa_api.Respository;

namespace magic_villa_api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiController]
    [ApiVersion("1.0")]

    public class UsersController: Controller
    {
        private readonly IUserRepository _userRepository;

        protected APIResponse _response;

        public UsersController(IUserRepository userRepository)
		{
            _userRepository = userRepository;
            this._response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);

            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool isUserNameUnique = _userRepository.IsUniqueUser(model.UserName);

            if (!isUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            var user = await _userRepository.Register(model);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            return Ok(_response);
        }

    }
}