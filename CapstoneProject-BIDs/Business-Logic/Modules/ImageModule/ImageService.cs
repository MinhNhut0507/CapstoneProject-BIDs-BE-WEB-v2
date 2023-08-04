using Business_Logic.Modules.ImageModule.Interface;
using Business_Logic.Modules.ImageModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Business_Logic.Modules.ImageModule
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _ImageRepository;
        private readonly IStaffRepository _StaffRepository;
        public ImageService(IImageRepository ImageRepository
            ,IStaffRepository staffRepository)
        {
            _ImageRepository = ImageRepository;
            _StaffRepository = staffRepository;
        }

        public async Task<ICollection<Image>> GetAll()
        {
            var result = await _ImageRepository.GetAll(includeProperties: "Item");
            return result;
        }

        public Task<ICollection<Image>> GetImagesIsValid()
        {
            return _ImageRepository.GetAll(options: o => o.Where(x => x.Status == true).ToList());
        }

        public async Task<ICollection<Image>> GetImageByID(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Image = await _ImageRepository.GetAll(includeProperties: "Item",
                options: o => o.Where(x => x.Id == id).ToList());
            if (Image == null)
            {
                throw new Exception(ErrorMessage.ImageError.IMAGE_NOT_FOUND);
            }
            return Image;
        }

        public async Task<ICollection<Image>> GetImageByItem(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Image = await _ImageRepository.GetAll(includeProperties: "Item",
                options: o => o.Where(x => x.ItemId == id).ToList());
            if (Image == null)
            {
                throw new Exception(ErrorMessage.ImageError.IMAGE_NOT_FOUND);
            }
            return Image;
        }

        public async Task<Image> AddNewImage(CreateImageRequest ImageRequest)
        {

            ValidationResult result = new CreateImageRequestValidator().Validate(ImageRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            if (!ImageRequest.DetailImage.Contains(".jpg")
                && !ImageRequest.DetailImage.Contains(".png")
                && !ImageRequest.DetailImage.Contains(".heic"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
            }

            var newImage = new Image();

            newImage.Id = Guid.NewGuid();
            newImage.ItemId = ImageRequest.ItemId;
            newImage.DetailImage = ImageRequest.DetailImage;
            newImage.Status = true;

            await _ImageRepository.AddAsync(newImage);
            return newImage;
        }

        public async Task<Image> UpdateImage(UpdateImageRequest ImageRequest)
        {
            try
            {
                var ImageUpdate = _ImageRepository.GetFirstOrDefaultAsync(x => x.Id == ImageRequest.Id).Result;

                if (ImageUpdate == null)
                {
                    throw new Exception(ErrorMessage.ImageError.IMAGE_NOT_FOUND);
                }

                ValidationResult result = new UpdateImageRequestValidator().Validate(ImageRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                if (!ImageRequest.DetailImage.Contains(".jpg")
                && !ImageRequest.DetailImage.Contains(".png")
                && !ImageRequest.DetailImage.Contains(".heic"))
                {
                    throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
                }

                ImageUpdate.DetailImage = ImageRequest.DetailImage;

                await _ImageRepository.UpdateAsync(ImageUpdate);
                return ImageUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }


    }
}
