using System;
using magic_villa_web.Models;

namespace magic_villa_web.Services.IServices
{
	public interface IBaseService
	{
		APIResponse resonseModel { get; set; }

		Task<T> SendAsync<T>(APIRequest apiRequest);
	}
}

