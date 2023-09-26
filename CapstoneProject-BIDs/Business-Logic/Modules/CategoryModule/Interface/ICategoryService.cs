using Business_Logic.Modules.CategoryModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CategoryModule.Interface
{
    public interface ICategoryService
    {
        public Task<Category> AddNewCategory(CreateCategoryRequest CategoryCreate);

        public Task<ICollection<Category>> GetCategorysIsValid();
        public Task<Category> UpdateCategory(UpdateCategoryRequest CategoryUpdate);

        public Task<Category> DeleteCategory(Guid? CategoryDeleteID);

        public Task<ICollection<Category>> GetAll();

        public Task<ICollection<Category>> GetCategoryByID(Guid? id);

        public Task<ICollection<Category>> GetCategoryByName(string Name);

    }
}
