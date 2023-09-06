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
            return await _DescriptionRepository.GetAll(includeProperties: "Category,ItemDescriptions", options: o => o.Where(x => x.Status == true).ToList());
        }

        public Task<ICollection<Description>> GetDescriptionsIsValid()
        {
            return _DescriptionRepository.GetAll(includeProperties: "Category,ItemDescriptions"
                , options: o => o.Where(x => x.Status == true).ToList());
        }

        public async Task<ICollection<Description>> GetDescriptionByID(Guid? id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Description = await _DescriptionRepository.GetAll(includeProperties: "Category,ItemDescriptions"
                , options: o => o.Where(x => x.Id == id).ToList());
            if (Description == null)
            {
                throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_NOT_FOUND);
            }
            return Description;
        }

        public async Task<ICollection<Description>> GetDescriptionByCategoryName(string CategoryName)
        {
            if (CategoryName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Category = await _CategoryRepository.GetFirstOrDefaultAsync(x => x.Name == CategoryName);
            var Description = await _DescriptionRepository.GetAll(includeProperties: "Category,ItemDescriptions"
                , options: o => o.Where(x => x.CategoryId == Category.Id).ToList());
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
            Description DescriptionCheck = await _DescriptionRepository.GetFirstOrDefaultAsync(x => x.Name == DescriptionRequest.Detail);

            if (DescriptionCheck != null)
            {
                throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_EXISTED);
            }

            var newDescription = new Description();

            newDescription.Id = Guid.NewGuid();
            newDescription.CategoryId = DescriptionRequest.CategoryId;
            newDescription.Name = DescriptionRequest.Detail;
            newDescription.Status = true;

            await _DescriptionRepository.AddAsync(newDescription);
            return newDescription;
        }

        public async Task<Description> UpdateDescription(UpdateDescriptionRequest DescriptionRequest)
        {
            try
            {
                var DescriptionUpdate = await _DescriptionRepository.GetFirstOrDefaultAsync(x => x.Id == DescriptionRequest.DescriptionId);

                if (DescriptionUpdate == null)
                {
                    throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_NOT_FOUND);
                }

                ValidationResult result = new UpdateDescriptionRequestValidator().Validate(DescriptionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                Description DescriptionCheck = await _DescriptionRepository.GetFirstOrDefaultAsync(x => x.Name == DescriptionRequest.Detail);

                if (DescriptionCheck != null)
                {
                    if(DescriptionCheck.Id == DescriptionRequest.DescriptionId)
                    {
                        throw new Exception(ErrorMessage.DescriptionError.DESCRIPTION_EXISTED);
                    }
                }

                DescriptionUpdate.Name = DescriptionRequest.Detail;
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
                if (DescriptionDeleteID == Guid.Empty)
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
