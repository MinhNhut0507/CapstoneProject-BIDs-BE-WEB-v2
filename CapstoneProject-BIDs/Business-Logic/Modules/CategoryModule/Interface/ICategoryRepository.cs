using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CategoryModule.Interface
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<ICollection<Category>> GetCategorysBy(
               Expression<Func<Category, bool>> filter = null,
               Func<IQueryable<Category>, ICollection<Category>> options = null,
               string includeProperties = null
           );
    }
}
