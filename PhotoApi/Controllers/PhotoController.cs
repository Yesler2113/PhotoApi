using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoApi.Dtos;
using PhotoApi.Dtos.Photo;
using PhotoApi.services;
using PhotoApi.services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PhotoApi.Controllers
{
    [Route("api/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoServices _photoServices;

        public PhotoController(IPhotoServices photoServices)
        {
            _photoServices = photoServices;
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> GuardarFoto([FromForm] PhotoCreateDto photoCreateDto)
        {
            if (photoCreateDto.File == null)
            {
                return BadRequest(new ResponseDto<PhotoDto>
                {
                    Status = false,
                    Message = "No file"
                });
            }

            var response = await _photoServices.SavePhoto(photoCreateDto);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        
        [HttpGet("ver/{id}")]
        public async Task<IActionResult> VerFoto(Guid id)
        {
            var response = await _photoServices.ViewPhoto(id);
            if (response.Status)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet]
        [Route("photos")]
        public async Task<IActionResult> GetAllPhotos()
        {
            var result = await _photoServices.GetAllPhotos();
            if (!result.Status)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

    }
}