using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.BookingItemModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Business_Logic.Modules.BookingItemModule
{
    public class BookingItemService : IBookingItemService
    {
        private readonly IBookingItemRepository _BookingItemRepository;
        private readonly IStaffRepository _StaffRepository;
        public BookingItemService(IBookingItemRepository BookingItemRepository
            ,IStaffRepository staffRepository)
        {
            _BookingItemRepository = BookingItemRepository;
            _StaffRepository = staffRepository;
        }

        public async Task<ICollection<BookingItem>> GetAll()
        {
            return await _BookingItemRepository.GetAll(includeProperties: "Staff,Item"
                , options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public Task<ICollection<BookingItem>> GetBookingItemsIsWatting()
        {
            return _BookingItemRepository.GetAll(includeProperties: "Staff,Item"
                , options: o => o.Where(x => x.Status == (int)BookingItemEnum.Watting).ToList());
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByID(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item"
                , options: o => o.Where(x => x.Id == id).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByStaff(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item"
                , options: o => o.Where(x => x.StaffId == id).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByItem(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item"
                , options: o => o.Where(x => x.ItemId == id).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task AddNewBookingItem(CreateBookingItemRequest BookingItemRequest)
        {

            ValidationResult result = new CreateBookingItemRequestValidator().Validate(BookingItemRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            ICollection<Staff> listStaffActive = await _StaffRepository.GetStaffsBy(x => x.Status == true);
            Random random = new Random();

            var newBookingItem = new BookingItem();

            newBookingItem.Id = Guid.NewGuid();
            newBookingItem.ItemId = BookingItemRequest.ItemId;
            newBookingItem.StaffId = listStaffActive.ElementAt(random.Next(0,listStaffActive.Count-1)).Id;
            newBookingItem.UpdateDate = DateTime.Now;
            newBookingItem.CreateDate = DateTime.Now;
            newBookingItem.Status = (int)BookingItemEnum.Watting;

            await _BookingItemRepository.AddAsync(newBookingItem);
        }

        public async Task<BookingItem> UpdateStatusBookingItem(UpdateBookingItemRequest BookingItemRequest)
        {
            try
            {
                var BookingItemUpdate = _BookingItemRepository.GetFirstOrDefaultAsync(x => x.Id == BookingItemRequest.Id).Result;

                if (BookingItemUpdate == null)
                {
                    throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
                }

                ValidationResult result = new UpdateBookingItemRequestValidator().Validate(BookingItemRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }


                BookingItemUpdate.UpdateDate = DateTime.Now;
                BookingItemUpdate.Status = BookingItemRequest.Status;

                await _BookingItemRepository.UpdateAsync(BookingItemUpdate);
                return BookingItemUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<BookingItem> AcceptStatusBookingItem(Guid id)
        {
            try
            {
                var BookingItemUpdate = _BookingItemRepository.GetFirstOrDefaultAsync(x => x.Id == id).Result;

                if (BookingItemUpdate == null)
                {
                    throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
                }


                BookingItemUpdate.UpdateDate = DateTime.Now;
                BookingItemUpdate.Status = (int)BookingItemEnum.Accepted;

                await _BookingItemRepository.UpdateAsync(BookingItemUpdate);
                return BookingItemUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<BookingItem> DenyStatusBookingItem(Guid id)
        {
            try
            {
                var BookingItemUpdate = _BookingItemRepository.GetFirstOrDefaultAsync(x => x.Id == id).Result;

                if (BookingItemUpdate == null)
                {
                    throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
                }


                BookingItemUpdate.UpdateDate = DateTime.Now;
                BookingItemUpdate.Status = (int)BookingItemEnum.Denied;

                await _BookingItemRepository.UpdateAsync(BookingItemUpdate);
                return BookingItemUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }


    }
}
