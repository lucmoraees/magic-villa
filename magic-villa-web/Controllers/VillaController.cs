using AutoMapper;
using magic_villa_web.Models;
using magic_villa_web.Models.Dto;
using magic_villa_web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using magic_villa_utillity;

namespace magic_villa_web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVilla()
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString(SD.SessionToken);
                var response = await _villaService.CreateAsync<APIResponse>(model, token);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            TempData["error"] = "Error envountered";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            string token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaService.GetAsync<APIResponse>(villaId, token);
            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Session.GetString(SD.SessionToken);
                var response = await _villaService.UpdateAsync<APIResponse>(model, token);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa updated successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            TempData["error"] = "Error envountered";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            string token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaService.GetAsync<APIResponse>(villaId, token);
            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            string token = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id, token);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexVilla));
            }

            TempData["error"] = "Error envountered";
            return View(model);
        }

    }
}
