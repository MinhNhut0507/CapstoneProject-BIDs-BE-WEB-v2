using Business_Logic.Modules.CategoryModule;
using Business_Logic.Modules.CategoryModule.Interface;
using Business_Logic.Modules.DescriptionModule.Interface;
using Business_Logic.Modules.DescriptionModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.DescriptionModule
{
    public class DescriptionService : IDescriptionService
    {
        private readonly IDescriptionRepository _DescriptionRepository;
        private readonly ICategoryRepository _CategoryRepository;
        public DescriptionService(IDescriptionRepository DescriptionRepository
            , ICategoryRepository CategoryRepository)
        {
            _DescriptionRepository = DescriptionRepository;
            _CategoryRepository = CategoryRepository;
        }

        public async Task<ICollection<Description>> GetAll()
        {
            return await _DescriptionRepository.GetAll(includeProperties: "Category");
        }

        public Task<ICollection<Description>> GetDescriptionsIsValid()
        {
            return _DescriptionRepository.GetDescriptionsBy(x => x.Status == true);
        }

        public async Task<Description> GetDescriptionByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Description = await _DescriptionRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Description == null)
            {
                throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_NOT_FOUND);
            }
            return Description;
        }

        public async Task<Description> GetDescriptionByCategoryName(string CategoryName)
        {
            if (CategoryName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Category = await _CategoryRepository.GetFirstOrDefaultAsync(x => x.Name == CategoryName);
            var Description = await _DescriptionRepository.GetFirstOrDefaultAsync(x => x.CategoryId == Category.Id);
            if (Description == null)
            {
                throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_NOT_FOUND);
            }
            return Description;
        }

        public async Task<Description> AddNewDescription(CreateDescriptionRequest DescriptionRequest)
        {

            ValidationResult result = new CreateDescriptionRequestValidator().Validate(DescriptionRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var Category = await _CategoryRepository.GetFirstOrDefaultAsync(x => x.Id == DescriptionRequest.CategoryId);
            Description DescriptionCheck = _DescriptionRepository.GetFirstOrDefaultAsync(x => x.CategoryId == DescriptionRequest.CategoryId).Result;

            if (DescriptionCheck != null)
            {
                throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_EXISTED);
            }

            var newDescription = new Description();

            newDescription.Id = Guid.NewGuid();
            newDescription.CategoryId = DescriptionRequest.CategoryId;
            newDescription.Detail = DescriptionRequest.Detail;
            newDescription.Status = true;

            await _DescriptionRepository.AddAsync(newDescription);
            return newDescription;
        }

        public async Task<Description> UpdateDescription(UpdateDescriptionRequest DescriptionRequest)
        {
            try
            {
                var DescriptionUpdate = GetDescriptionByID(DescriptionRequest.DescriptionId).Result;

                if (DescriptionUpdate == null)
                {
                    throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_NOT_FOUND);
                }

                ValidationResult result = new UpdateDescriptionRequestValidator().Validate(DescriptionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                DescriptionUpdate.Detail = DescriptionRequest.Detail;
                DescriptionUpdate.Status = DescriptionRequest.Status;

                await _DescriptionRepository.UpdateAsync(DescriptionUpdate);
                return DescriptionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Description> DeleteDescription(Guid? DescriptionDeleteID)
        {
            try
            {
                if (DescriptionDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Description DescriptionDelete = _DescriptionRepository.GetFirstOrDefaultAsync(x => x.Id == DescriptionDeleteID && x.Status == true).Result;

                if (DescriptionDelete == null)
                {
                    throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_NOT_FOUND);
                }

                DescriptionDelete.Status = false;
                await _DescriptionRepository.UpdateAsync(DescriptionDelete);
                return DescriptionDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
