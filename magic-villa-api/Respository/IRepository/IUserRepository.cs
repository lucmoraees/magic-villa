using System;
using magic_villa_api.Models;
using magic_villa_api.Models.Dto;

namespace magic_villa_api.Respository.IRepository
{
	public interface IUserRepository
	{
        bool IsUniqueUser(string username);

		Task<LoginResponseDTO> Login(LoginRequestDTO loginResponseDTO);

        Task<UserDTO> Register(RegistrationRequestDTO registerationRequestDTO);
    }
}

