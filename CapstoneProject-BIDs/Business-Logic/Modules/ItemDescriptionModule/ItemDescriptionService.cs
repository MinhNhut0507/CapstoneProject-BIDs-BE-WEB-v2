using Business_Logic.Modules.ItemDescriptionModule.Interface;
using Business_Logic.Modules.ItemDescriptionModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Business_Logic.Modules.ItemDescriptionModule
{
    public class ItemDescriptionService : IItemDescriptionService
    {
        private readonly IItemDescriptionRepository _ItemDescriptionRepository;
        private readonly IStaffRepository _StaffRepository;
        public ItemDescriptionService(IItemDescriptionRepository ItemDescriptionRepository
            ,IStaffRepository staffRepository)
        {
            _ItemDescriptionRepository = ItemDescriptionRepository;
            _StaffRepository = staffRepository;
        }

        public async Task<ICollection<ItemDescription>> GetAll()
        {
            var result = await _ItemDescriptionRepository.GetAll();
            return result;
        }

        public Task<ICollection<ItemDescription>> GetItemDescriptionsIsValid()
        {
            return _ItemDescriptionRepository.GetAll(options: o => o.Where(x => x.Status == true).ToList());
        }

        public async Task<ICollection<ItemDescription>> GetItemDescriptionByID(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var ItemDescription = await _ItemDescriptionRepository.GetAll(options: o => o.Where(x => x.Id == id).ToList());
            if (ItemDescription == null)
            {
                throw new Exception(ErrorMessage.ItemDescriptionError.ITEM_DESCRIPTION_NOT_FOUND);
            }
            return ItemDescription;
        }

        public async Task<ICollection<ItemDescription>> GetItemDescriptionByItem(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var ItemDescription = await _ItemDescriptionRepository.GetAll(options: o => o.Where(x => x.ItemId == id).ToList());
            if (ItemDescription == null)
            {
                throw new Exception(ErrorMessage.ItemDescriptionError.ITEM_DESCRIPTION_NOT_FOUND);
            }
            return ItemDescription;
        }

        public async Task<ICollection<ItemDescription>> GetItemDescriptionByDescription(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var ItemDescription = await _ItemDescriptionRepository.GetAll(options: o => o.Where(x => x.DescriptionId == id).ToList());
            if (ItemDescription == null)
            {
                throw new Exception(ErrorMessage.ItemDescriptionError.ITEM_DESCRIPTION_NOT_FOUND);
            }
            return ItemDescription;
        }

        public async Task<ItemDescription> AddNewItemDescription(CreateItemDescriptionRequest ItemDescriptionRequest)
        {

            ValidationResult result = new CreateItemDescriptionRequestValidator().Validate(ItemDescriptionRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }


            var newItemDescription = new ItemDescription();

            newItemDescription.Id = Guid.NewGuid();
            newItemDescription.ItemId = ItemDescriptionRequest.ItemId;
            newItemDescription.DescriptionId = ItemDescriptionRequest.DescriptionId;
            newItemDescription.Detail = ItemDescriptionRequest.Detail;
            newItemDescription.Status = true;

            await _ItemDescriptionRepository.AddAsync(newItemDescription);
            return newItemDescription;
        }

        public async Task<ItemDescription> UpdateStatusItemDescription(UpdateItemDescriptionRequest ItemDescriptionRequest)
        {
            try
            {
                var ItemDescriptionUpdate = _ItemDescriptionRepository.GetFirstOrDefaultAsync(x => x.Id == ItemDescriptionRequest.Id).Result;

                if (ItemDescriptionUpdate == null)
                {
                    throw new Exception(ErrorMessage.ItemDescriptionError.ITEM_DESCRIPTION_NOT_FOUND);
                }

                ValidationResult result = new UpdateItemDescriptionRequestValidator().Validate(ItemDescriptionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }


                ItemDescriptionUpdate.Detail = ItemDescriptionRequest.Detail;

                await _ItemDescriptionRepository.UpdateAsync(ItemDescriptionUpdate);
                return ItemDescriptionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }


    }
}
