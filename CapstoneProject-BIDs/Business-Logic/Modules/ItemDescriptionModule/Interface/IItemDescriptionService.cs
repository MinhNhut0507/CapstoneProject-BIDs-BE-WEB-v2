using Business_Logic.Modules.ItemDescriptionModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemDescriptionModule.Interface
{
    public interface IItemDescriptionService
    {
        public Task<ItemDescription> AddNewItemDescription(CreateItemDescriptionRequest ItemDescriptionCreate);

        public Task<ItemDescription> UpdateStatusItemDescription(UpdateItemDescriptionRequest ItemDescriptionUpdate);


        public Task<ICollection<ItemDescription>> GetAll();
        public Task<ICollection<ItemDescription>> GetItemDescriptionByID(Guid id);

        public Task<ICollection<ItemDescription>> GetItemDescriptionByItem(Guid id);

        public Task<ICollection<ItemDescription>> GetItemDescriptionByDescription(Guid id);

    }
}
