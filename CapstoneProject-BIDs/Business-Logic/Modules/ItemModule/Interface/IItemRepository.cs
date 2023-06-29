using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemModule.Interface
{
    public interface IItemRepository : IRepository<Item>
    {
        public Task<ICollection<Item>> GetItemsBy(
               Expression<Func<Item, bool>> filter = null,
               Func<IQueryable<Item>, ICollection<Item>> options = null,
               string includeProperties = null
           );
    }
}
