using Business_Logic.Modules.BookingItemModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BookingItemModule
{
    public class BookingItemRepository : Repository<BookingItem>, IBookingItemRepository
    {
        private readonly BIDsContext _db;

        public BookingItemRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemsBy(
            Expression<Func<BookingItem, bool>> filter = null,
            Func<IQueryable<BookingItem>, ICollection<BookingItem>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<BookingItem> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            query = query.Include(s => s.Item);
            query = query.Include(s => s.Staff);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
