using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhotoApi.Database;
using PhotoApi.Dtos;
using PhotoApi.Dtos.Photo;
using PhotoApi.entities;
using PhotoApi.services.Interfaces;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace PhotoApi.services
{
    public class PhotoServices : IPhotoServices
    {
        private readonly PhotoDbContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;

        public PhotoServices(PhotoDbContext context, Cloudinary cloudinary, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ResponseDto<PhotoDto>> SavePhoto(PhotoCreateDto photoCreateDto)
        {
            if (photoCreateDto.File == null || photoCreateDto.File.Length == 0)
            {
                return new ResponseDto<PhotoDto>
                {
                    Status = false,
                    Message = "No file provided"
                };
            }

            Console.WriteLine($"Received file: {photoCreateDto.File.FileName}");
            Console.WriteLine($"Description: {photoCreateDto.Description}");

            var uploadResult = new ImageUploadResult();

            using (var stream = photoCreateDto.File.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(photoCreateDto.File.FileName, stream)
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            Console.WriteLine($"Upload result: {uploadResult}");
            Console.WriteLine($"Upload result URL: {uploadResult.Url}");

            if (uploadResult == null || uploadResult.Url == null)
            {
                return new ResponseDto<PhotoDto>
                {
                    Status = false,
                    Message = "Failed to upload image to Cloudinary"
                };
            }

            var photo = new PhotoEntity
            {
                Id = Guid.NewGuid(),
                Description = photoCreateDto.Description,
                Url = uploadResult.Url.ToString(),
                date = DateTime.UtcNow
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            var photoDto = new PhotoDto
            {
                Id = photo.Id,
                Description = photo.Description,
                Url = photo.Url,
                Date = photo.date
            };

            return new ResponseDto<PhotoDto>
            {
                Status = true,
                Message = "Photo saved successfully",
                Data = photoDto
            };
        }




        public async Task<ResponseDto<PhotoDto>> ViewPhoto(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null)
            {
                return new ResponseDto<PhotoDto>
                {
                    Status = false,
                    Message = "Photo not found"
                };
            }

            var photoDto = _mapper.Map<PhotoDto>(photo);

            return new ResponseDto<PhotoDto>
            {
                Status = true,
                Message = "Photo retrieved successfully",
                Data = photoDto
            };
        }
        //obtener todas las fotos
        public async Task<ResponseDto<List<PhotoDto>>> GetAllPhotos()
        {
            var photos = await _context.Photos.ToListAsync();

            var photoDtos = photos.Select(photo => new PhotoDto
            {
                Id = photo.Id,
                Description = photo.Description,
                Url = photo.Url,
                Date = photo.date 
            }).ToList();

            return new ResponseDto<List<PhotoDto>>
            {
                Status = true,
                Message = "Photos retrieved successfully",
                Data = photoDtos
            };
        }

    }
}
