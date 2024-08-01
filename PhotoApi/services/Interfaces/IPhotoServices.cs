using PhotoApi.Dtos;
using PhotoApi.Dtos.Photo;

namespace PhotoApi.services.Interfaces
{
    public interface IPhotoServices
    {
        Task<ResponseDto<List<PhotoDto>>> GetAllPhotos();
        Task<ResponseDto<PhotoDto>> SavePhoto(PhotoCreateDto photoCreateDto);
        Task<ResponseDto<PhotoDto>> ViewPhoto(Guid id);
    }
}

