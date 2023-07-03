using Business_Logic.Modules.ItemDescriptionModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemDescriptionModule
{
    public class ItemDescriptionRepository : Repository<ItemDescription>, IItemDescriptionRepository
    {
        private readonly BIDsContext _db;

        public ItemDescriptionRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<ItemDescription>> GetAll()
        {
            var itemDescription = await _db.ItemDescriptions
                .Include(i => i.Item)
                .Include(i => i.Description)
                .ThenInclude(i => i.Category)
                .ToListAsync();

            return itemDescription;
        }
        public async Task<ICollection<ItemDescription>> GetItemDescriptionsBy(
            Expression<Func<ItemDescription, bool>> filter = null,
            Func<IQueryable<ItemDescription>, ICollection<ItemDescription>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<ItemDescription> query = DbSet;

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

            query = query.Include(s => s.Item)
                .Include(s => s.Description)
                    .ThenInclude(s => s.Category);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
