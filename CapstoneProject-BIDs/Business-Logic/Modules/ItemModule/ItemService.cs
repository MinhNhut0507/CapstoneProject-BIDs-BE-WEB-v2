using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Business_Logic.Modules.CategoryModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.ItemModule
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _ItemRepository;
        private readonly ICategoryService _CategoryService;
        public ItemService(IItemRepository ItemRepository, ICategoryService CategoryService)
        {
            _ItemRepository = ItemRepository;
            _CategoryService = CategoryService;
        }

        public async Task<ICollection<Item>> GetAll()
        {
            return await _ItemRepository.GetAll(includeProperties: "User,Category",options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public Task<ICollection<Item>> GetItemsIsValid()
        {
            return _ItemRepository.GetItemsBy(x => x.Status == true, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<Item> GetItemByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Item = await _ItemRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Item == null)
            {
                throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
            }
            return Item;
        }

        public async Task<Item> GetItemByName(string ItemName)
        {
            if (ItemName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Item = await _ItemRepository.GetFirstOrDefaultAsync(x => x.Name == ItemName);
            if (Item == null)
            {
                throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
            }
            return Item;
        }

        public async Task<ICollection<Item>> GetItemByUserID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Item = await _ItemRepository.GetItemsBy(x => x.UserId == id);
            if (Item == null)
            {
                throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
            }
            return Item;
        }

        public async Task<ICollection<Item>> GetItemByTypeName(string TypeName)
        {
            if (TypeName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            Category TypeCheck = await _CategoryService.GetCategoryByName(TypeName);
            var Item = await _ItemRepository.GetItemsBy(x => x.CategoryId == TypeCheck.Id);
            if (Item == null)
            {
                throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
            }
            return Item;
        }

        public async Task<Item> AddNewItem(CreateItemRequest ItemRequest)
        {

            ValidationResult result = new CreateItemRequestValidator().Validate(ItemRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }


            var newItem = new Item();

            newItem.Id = new Guid();
            newItem.Name = ItemRequest.ItemName;
            newItem.DescriptionDetail = ItemRequest.Description;
            newItem.UserId = ItemRequest.UserId;
            newItem.Quantity = ItemRequest.Quantity;
            newItem.FristPrice = ItemRequest.FristPrice;
            newItem.StepPrice = ItemRequest.StepPrice;
            newItem.Image = ItemRequest.Image;
            newItem.CategoryId = ItemRequest.CategoryId;
            newItem.Deposit = ItemRequest.Deposit;
            newItem.UpdateDate = DateTime.Now;
            newItem.CreateDate = DateTime.Now;
            newItem.Status = false;

            await _ItemRepository.AddAsync(newItem);
            return newItem;
        }

        public async Task<Item> UpdateItem(UpdateItemRequest ItemRequest)
        {
            try
            {
                var ItemUpdate = GetItemByID(ItemRequest.ItemId).Result;

                if (ItemUpdate == null)
                {
                    throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
                }

                ValidationResult result = new UpdateItemRequestValidator().Validate(ItemRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                Item ItemCheck = _ItemRepository.GetFirstOrDefaultAsync(x => x.Name == ItemRequest.ItemName).Result;

                if (ItemCheck != null)
                {
                    throw new Exception(ErrorMessage.ItemError.ITEM_EXISTED);
                }
                ItemUpdate.Id = ItemRequest.ItemId;
                ItemUpdate.Name = ItemRequest.ItemName;
                ItemUpdate.DescriptionDetail = ItemRequest.Description;
                ItemUpdate.Quantity = ItemRequest.Quantity;
                ItemUpdate.FristPrice = ItemRequest.FristPrice;
                ItemUpdate.StepPrice = ItemRequest.StepPrice;
                ItemUpdate.Image = ItemRequest.Image;
                ItemUpdate.Deposit = ItemRequest.Deposit;
                ItemUpdate.UpdateDate = DateTime.Now;

                await _ItemRepository.UpdateAsync(ItemUpdate);
                return ItemUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Item> DeleteItem(Guid? ItemDeleteID)
        {
            try
            {
                if (ItemDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Item ItemDelete = _ItemRepository.GetFirstOrDefaultAsync(x => x.Id == ItemDeleteID && x.Status == true).Result;

                if (ItemDelete == null)
                {
                    throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
                }

                ItemDelete.Status = false;
                await _ItemRepository.UpdateAsync(ItemDelete);
                return ItemDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
