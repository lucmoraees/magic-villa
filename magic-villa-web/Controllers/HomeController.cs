using Microsoft.AspNetCore.Mvc;
using magic_villa_web.Models;
using AutoMapper;
using magic_villa_web.Services.IServices;
using Newtonsoft.Json;
using magic_villa_web.Models.Dto;
using magic_villa_utillity;

namespace magic_villa_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public HomeController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new();

            string token = HttpContext.Session.GetString(SD.SessionToken);

            var response = await _villaService.GetAllAsync<APIResponse>(token);
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

    }
}