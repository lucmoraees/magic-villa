using System;
using magic_villa_api.Models.Dto;
namespace magic_villa_api.Models.Dto
{
	public class LoginResponseDTO
	{
		public UserDTO User { get; set; }

		public string Token { get; set; }
	}
}

