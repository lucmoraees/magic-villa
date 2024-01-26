using System;
using Microsoft.AspNetCore.Identity;
namespace magic_villa_api.Models
{
	public class ApplicationUser: IdentityUser
	{
		public string Name { get; set; }
	}
}

