using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BookingItemModule.Interface
{
    public interface IBookingItemRepository : IRepository<BookingItem>
    {
        public Task<ICollection<BookingItem>> GetBookingItemsBy(
               Expression<Func<BookingItem, bool>> filter = null,
               Func<IQueryable<BookingItem>, ICollection<BookingItem>> options = null,
               string includeProperties = null
           );
    }
}
