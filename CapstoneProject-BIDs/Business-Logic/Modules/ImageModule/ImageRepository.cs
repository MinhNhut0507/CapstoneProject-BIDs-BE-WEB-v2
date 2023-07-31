using Business_Logic.Modules.ImageModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ImageModule
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly BIDsContext _db;

        public ImageRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Image>> GetImagesBy(
            Expression<Func<Image, bool>> filter = null,
            Func<IQueryable<Image>, ICollection<Image>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Image> query = DbSet;

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

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
