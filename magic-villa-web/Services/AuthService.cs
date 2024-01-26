using System;
using magic_villa_utillity;
using magic_villa_web.Models;
using magic_villa_web.Models.Dto;
using magic_villa_web.Services.IServices;

namespace magic_villa_web.Services
{
	public class AuthService: BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;

        private string villaUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");

        }

        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDTO,
                Url = villaUrl + "/api/v1/UsersAuth/login",
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTO,
                Url = villaUrl + "/api/v1/UsersAuth/register",
            });
        }
    }
}
