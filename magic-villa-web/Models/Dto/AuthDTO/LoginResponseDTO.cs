using System;
using magic_villa_web.Models.Dto;

namespace magic_villa_web.Models.Dto
{
	public class LoginResponseDTO
	{
		public UserDTO User { get; set; }

		public string Token { get; set; }
	}
}

