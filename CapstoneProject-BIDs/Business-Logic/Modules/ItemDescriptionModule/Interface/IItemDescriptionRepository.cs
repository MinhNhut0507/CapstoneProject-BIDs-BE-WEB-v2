using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemDescriptionModule.Interface
{
    public interface IItemDescriptionRepository : IRepository<ItemDescription>
    {
        public Task<ICollection<ItemDescription>> GetItemDescriptionsBy(
               Expression<Func<ItemDescription, bool>> filter = null,
               Func<IQueryable<ItemDescription>, ICollection<ItemDescription>> options = null,
               string includeProperties = null
           );
        public Task<ICollection<ItemDescription>> GetAll();
    }
}
