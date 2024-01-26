using Microsoft.AspNetCore.Mvc;
using magic_villa_api.Models;
using magic_villa_api.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using magic_villa_api.Respository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace magic_villa_api.Controllers.v1
{
	[Route("api/v{version:apiVersion}/VillaApi")]
	[ApiController]
    [ApiVersion("1.0")]

    public class VillaApiController: ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRespository _villaRespository;
        private readonly IMapper _mapper;

        public VillaApiController(IVillaRespository villaRespository, IMapper mapper)
        {
            _villaRespository = villaRespository;
            _mapper = mapper;
            this._response = new();
        }

        // Find list of villas
        [HttpGet]
        [Produces("application/json")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        async public Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "filterOccupancy")] int? occupancy,
            [FromQuery()] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Villa> villaList;

                if (occupancy > 0)
                {
                    villaList = await _villaRespository.GetAllAsync(u => u.Occupancy == occupancy, pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    villaList = await _villaRespository.GetAllAsync(pageSize: pageSize,
                        pageNumber: pageNumber);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    villaList = villaList.Where(u => u.Name.ToLower().Contains(search));
                }

                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                var villas = _mapper.Map<List<VillaDTO>>(villaList);
                _response.Result = villas;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        // Find Villa by Id
        [HttpGet("{id:int}", Name = "GetVilla")]
        [Produces("application/json")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                     _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRespository.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);;
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

		// Create Villa
		[HttpPost]
        [Authorize(Roles = "admin")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
		{
            try
            {
                var villaByName = await _villaRespository.GetAsync(u => u.Name.ToLower() == villaCreateDTO.Name);

                if (villaByName != null)
                {
                    ModelState.TryAddModelError("message", "Villa already exists!");
                    return BadRequest(ModelState);
                }

                if (villaCreateDTO == null)
                {
                    return BadRequest(villaCreateDTO);
                }

                Villa villa = _mapper.Map<Villa>(villaCreateDTO);

                await _villaRespository.CreateAsync(villa);

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        // Delete Villa
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await _villaRespository.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }
                await _villaRespository.RemoveAsync(villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        // Update Villa
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                Villa model = _mapper.Map<Villa>(updateDTO);

                await _villaRespository.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

		// Update specific part of Villa
		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                {
                     _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRespository.GetAsync(u => u.Id == id, false);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);

                patchDTO.ApplyTo(villaUpdateDTO, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Villa model = _mapper.Map<Villa>(villaUpdateDTO);

                await _villaRespository.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
