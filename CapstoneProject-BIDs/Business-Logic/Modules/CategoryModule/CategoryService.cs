using Business_Logic.Modules.CategoryModule.Interface;
using Business_Logic.Modules.CategoryModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.CategoryModule
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository;
        public CategoryService(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        public async Task<ICollection<Category>> GetAll()
        {
            return await _CategoryRepository.GetAll(includeProperties: "Descriptions");
        }

        public Task<ICollection<Category>> GetCategorysIsValid()
        {
            return _CategoryRepository.GetAll(includeProperties: "Descriptions"
                , options: o => o.Where(x => x.Status == true).ToList());
        }

        public async Task<ICollection<Category>> GetCategoryByID(Guid? id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Category = await _CategoryRepository.GetAll(includeProperties: "Descriptions"
                , options: o => o.Where(x => x.Id == id).ToList());
            if (Category == null)
            {
                throw new Exception(ErrorMessage.CategoryError.CATEGORY_NOT_FOUND);
            }
            return Category;
        }

        public async Task<ICollection<Category>> GetCategoryByName(string CategoryName)
        {
            if (CategoryName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Category = await _CategoryRepository.GetAll(includeProperties: "Descriptions"
                , options: o => o.Where(x => x.Name == CategoryName).ToList());
            if (Category == null)
            {
                throw new Exception(ErrorMessage.CategoryError.CATEGORY_NOT_FOUND);
            }
            return Category;
        }

        public async Task<Category> AddNewCategory(CreateCategoryRequest CategoryRequest)
        {

            ValidationResult result = new CreateCategoryRequestValidator().Validate(CategoryRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            Category CategoryCheck = _CategoryRepository.GetFirstOrDefaultAsync(x => x.Name == CategoryRequest.CategoryName).Result;

            if (CategoryCheck != null)
            {
                throw new Exception(ErrorMessage.CategoryError.CATEGORY_EXISTED);
            }

            var newCategory = new Category();

            newCategory.Id = Guid.NewGuid();
            newCategory.Name = CategoryRequest.CategoryName;
            DateTime dateTime = DateTime.UtcNow;
            newCategory.CreateDate = dateTime.AddHours(7);
            newCategory.UpdateDate = dateTime.AddHours(7);
            newCategory.Status = true;

            await _CategoryRepository.AddAsync(newCategory);
            return newCategory;
        }

        public async Task<Category> UpdateCategory(UpdateCategoryRequest CategoryRequest)
        {
            try
            {
                var CategoryUpdate = await _CategoryRepository.GetFirstOrDefaultAsync(x => x.Id == CategoryRequest.CategoryId);

                if (CategoryUpdate == null)
                {
                    throw new Exception(ErrorMessage.CategoryError.CATEGORY_NOT_FOUND);
                }

                ValidationResult result = new UpdateCategoryRequestValidator().Validate(CategoryRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                var CategoryCheck = await _CategoryRepository.GetFirstOrDefaultAsync(x => x.Name == CategoryRequest.CategoryName);

                if (CategoryCheck != null)
                {
                    if(CategoryCheck.Id != CategoryUpdate.Id)
                    {
                        throw new Exception(ErrorMessage.CategoryError.CATEGORY_EXISTED);
                    }
                }

                CategoryUpdate.Name = CategoryRequest.CategoryName;
                DateTime dateTime = DateTime.UtcNow;
                CategoryUpdate.UpdateDate = dateTime.AddHours(7);
                CategoryUpdate.Status = CategoryRequest.Status;

                await _CategoryRepository.UpdateAsync(CategoryUpdate);
                return CategoryUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Category> DeleteCategory(Guid? CategoryDeleteID)
        {
            try
            {
                if (CategoryDeleteID == Guid.Empty)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Category CategoryDelete = await _CategoryRepository.GetFirstOrDefaultAsync(x => x.Id == CategoryDeleteID && x.Status == true);

                if (CategoryDelete == null)
                {
                    throw new Exception(ErrorMessage.CategoryError.CATEGORY_NOT_FOUND);
                }

                CategoryDelete.Status = false;
                await _CategoryRepository.UpdateAsync(CategoryDelete);
                return CategoryDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
