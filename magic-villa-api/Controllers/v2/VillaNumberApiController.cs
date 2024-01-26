using System;
using System.Data;
using System.Net;
using AutoMapper;
using magic_villa_api.Models;
using magic_villa_api.Models.Dto;
using magic_villa_api.Respository;
using magic_villa_api.Respository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace magic_villa_api.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberApi")]
    [ApiController]
    [ApiVersion("2.0")]

    public class VillaNumberApiController: ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRespository _villaNumberRespository;
        private readonly IVillaRespository _villaRespository;
        private readonly IMapper _mapper;

        public VillaNumberApiController(
            IVillaNumberRespository villaNumberRespository,
            IVillaRespository villaRespository,
            IMapper mapper)
        {
            _villaNumberRespository = villaNumberRespository;
            _villaRespository = villaRespository;
            _mapper = mapper;
            this._response = new();
        }

        // Find list of villa numbers
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {

                IEnumerable<VillaNumber> villaNumberList = await _villaNumberRespository.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
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

        // Find Villa number by villaNo
        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumber = await _villaNumberRespository.GetAsync(u => u.VillaNo == villaNo);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response); ;
                }

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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

        // Create Villa Number
        [HttpPost]
        [Authorize(Roles = "admin")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {

                if (await _villaNumberRespository.GetAsync(u => u.VillaNo == villaNumberCreateDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number already Exists!");
                    return BadRequest(ModelState);
                }

                if (await _villaRespository.GetAsync(u => u.Id == villaNumberCreateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }

                if (villaNumberCreateDTO == null)
                {
                    return BadRequest(villaNumberCreateDTO);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);


                await _villaNumberRespository.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // Delete Villa number
        [HttpDelete("{villaNo:int}", Name = "DeleteVillaNumber")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumber = await _villaNumberRespository.GetAsync(u => u.VillaNo == villaNo);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response); ;
                }

                await _villaNumberRespository.RemoveAsync(villaNumber);

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

        // Update Villa Number
        [HttpPut("{villaNo:int}", Name = "UpdateVillaNumber")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int villaNo, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            try
            {
                if (villaNumberUpdateDTO == null || villaNo != villaNumberUpdateDTO.VillaNo)
                {
                    return BadRequest();
                }
                if (await _villaRespository.GetAsync(u => u.Id == villaNumberUpdateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }
                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);

                await _villaNumberRespository.UpdateAsync(model);
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
    }
}

