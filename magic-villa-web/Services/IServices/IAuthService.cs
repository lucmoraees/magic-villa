using System;
using magic_villa_web.Models.Dto;
namespace magic_villa_web.Services.IServices
{
	public interface IAuthService
	{
		Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO);

        Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO);
    }
}

