using Business_Logic.Modules.ItemModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemModule
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly BIDsContext _db;

        public ItemRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        //public async Task<ICollection<Item>> GetAll()
        //{
        //    var items = await _db.Items
        //        .Include(i => i.User)
        //        .Include(i => i.Category)
        //        .Include(i => i.ItemDescriptions)
        //        .Include(i => i.BookingItems)
        //        .ToListAsync();

        //    return items;
        //}

        public async Task<ICollection<Item>> GetItemsBy(
            Expression<Func<Item, bool>> filter = null,
            Func<IQueryable<Item>, ICollection<Item>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Item> query = DbSet;

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

            query = query.Include(s => s.User)
                .Include(s => s.Category)
                .Include(s => s.BookingItems)
                .Include(s => s.ItemDescriptions)
                .ThenInclude(s => s.Description);

            return options != null ? options(query).ToImmutableList() : await query.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
