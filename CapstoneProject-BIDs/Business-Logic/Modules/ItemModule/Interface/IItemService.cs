using Business_Logic.Modules.ItemModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemModule.Interface
{
    public interface IItemService
    {
        public Task<Item> AddNewItem(CreateItemRequest ItemCreate);

        public Task<Item> UpdateItem(UpdateItemRequest ItemUpdate);

        public Task<Item> DeleteItem(Guid ItemDeleteID);

        public Task<ICollection<Item>> GetAll();

        public Task<ICollection<Item>> GetItemByID(Guid? id);

        public Task<ICollection<Item>> GetItemByName(string Name);

        public Task<ICollection<Item>> GetItemByCategoryName(string CategoryName);

        public Task<ICollection<Item>> GetItemByUserID(Guid? id);

    }
}
