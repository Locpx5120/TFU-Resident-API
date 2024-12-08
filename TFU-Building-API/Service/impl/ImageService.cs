using Core.Enums;
using Core.Model;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ImageService : BaseHandler, IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdentity _userIdentity;

        public ImageService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            IUserIdentity userIdentity)
            : base(unitOfWork, httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userIdentity = userIdentity;
        }

        public async Task<ResponseData<ImageDto>> Get(Guid id)
        {
            try
            {
                ImageDto imageDto = new ImageDto();
                var data = _unitOfWork.ImgBaseRepository.GetById(id);

                if (data == null)
                {
                    return new ResponseData<ImageDto>
                    {
                        Success = false,
                        Message = "Image not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                imageDto.Id = data.Id;
                imageDto.Base64 = data.Base64;
                imageDto.FileName = data.FileName;
                imageDto.Name = data.Name;
                imageDto.ContentType = data.ContentType;
                imageDto.ContentDisposition = data.ContentDisposition;
                imageDto.Length = data.Length;


                return new ResponseData<ImageDto>
                {
                    Success = true,
                    Message = "imageDto information successfully.",
                    Data = imageDto,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<ImageDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
