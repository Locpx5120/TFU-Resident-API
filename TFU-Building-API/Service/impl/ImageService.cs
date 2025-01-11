using BuildingModels;
using Constant;
using Core.Enums;
using Core.Model;
using fake_tool.Helpers;
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

        public async Task<ResponseData<ImageDto>> Add(IFormFile file)
        {
            ImgBase imgBase = new ImgBase();

            imgBase.Id = Guid.NewGuid();
            imgBase.Base64 = await Utill.ConvertFilePDFToBase64(file);
            imgBase.FileName = file.FileName;
            imgBase.Name = file.Name;
            imgBase.ContentType = file.ContentType;
            imgBase.ContentDisposition = file.ContentDisposition;
            imgBase.Length = file.Length;

            _unitOfWork.ImgBaseRepository.Add(imgBase);

            await _unitOfWork.SaveChangesAsync();

            // Tạo response
            var response = new ImageDto
            {
                Id = imgBase.Id,
                Base64 = imgBase.Base64,
                FileName = imgBase.FileName,
                Name = imgBase.Name,
                ContentType = imgBase.ContentType,
                ContentDisposition = imgBase.ContentDisposition,
                Length = imgBase.Length,
            };

            return new ResponseData<ImageDto>
            {
                Success = true,
                Message = "Tệp đã được tạo thành công.",
                Data = response,
                Code = (int)ErrorCodeAPI.OK
            };
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
                        Message = "Không tìm thấy ảnh",
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
                    Message = MessConstant.Successfully,
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
