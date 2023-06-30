using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Business_Logic.Modules.CategoryModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;
using Business_Logic.Modules.BookingItemModule.Request;
using Business_Logic.Modules.BookingItemModule.Interface;
using Data_Access.Enum;

namespace Business_Logic.Modules.ItemModule
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _ItemRepository;
        private readonly ICategoryService _CategoryService;
        private readonly IBookingItemService _BookingItemService;

        public ItemService(IItemRepository ItemRepository
            , ICategoryService CategoryService
            , IBookingItemService BookingItemService)
        {
            _ItemRepository = ItemRepository;
            _CategoryService = CategoryService;
            _BookingItemService = BookingItemService;
        }

        public async Task<ICollection<Item>> GetAll()
        {
            return await _ItemRepository.GetAll(includeProperties: "User,Category"
                ,options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public Task<ICollection<Item>> GetItemsIsValid()
        {
            return _ItemRepository.GetAll(includeProperties: "User,Category"
                , options: o => o.Where(x => x.Status == true).ToList());
        }

        public async Task<ICollection<Item>> GetItemByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Item = await _ItemRepository.GetAll(includeProperties: "User,Category"
                , options: o => o.Where(x => x.Id == id).ToList());
            if (Item == null)
            {
                throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
            }
            return Item;
        }

        public async Task<ICollection<Item>> GetItemByName(string ItemName)
        {
            if (ItemName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Item = await _ItemRepository.GetAll(includeProperties: "User,Category"
                , options: o => o.Where(x => x.Name == ItemName).ToList());
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
            var Item = await _ItemRepository.GetAll(includeProperties: "User,Category"
                , options: o => o.Where(x => x.UserId == id).ToList());
            if (Item == null)
            {
                throw new Exception(ErrorMessage.ItemError.ITEM_NOT_FOUND);
            }
            return Item;
        }

        public async Task<ICollection<Item>> GetItemByCategoryName(string CategoryName)
        {
            if (CategoryName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            Category CategoryCheck = await _CategoryService.GetCategoryByName(CategoryName);
            var Item = await _ItemRepository.GetAll(includeProperties: "User,Category"
                , options: o => o.Where(x => x.CategoryId == CategoryCheck.Id).ToList());
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

            CreateBookingItemRequest bookingItemRequest = new CreateBookingItemRequest()
            {
                ItemId = newItem.Id,
                UserId = newItem.UserId,
            };
            await _BookingItemService.AddNewBookingItem(bookingItemRequest);
            return newItem;
        }

        public async Task<Item> UpdateItem(UpdateItemRequest ItemRequest)
        {
            try
            {
                var ItemUpdate = await _ItemRepository.GetFirstOrDefaultAsync(x => x.Id == ItemRequest.ItemId);

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

        public async Task<Item> DeleteItem(Guid ItemDeleteID)
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
                ICollection<BookingItem> bookingItem = await _BookingItemService.GetBookingItemByItem(ItemDeleteID);
                UpdateBookingItemRequest bookingItemRequest = new UpdateBookingItemRequest()
                {
                    Id = bookingItem.First().Id,
                    Status = (int)BookingItemEnum.Unactive
                };
                await _BookingItemService.UpdateStatusBookingItem(bookingItemRequest);
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
