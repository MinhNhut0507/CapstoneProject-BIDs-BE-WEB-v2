using Business_Logic.Modules.StaffNotificationDetailModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.StaffNotificationDetailModule
{
    public class StaffNotificationDetailRepository : Repository<StaffNotificationDetail>, IStaffNotificationDetailRepository
    {
        private readonly BIDsContext _db;

        public StaffNotificationDetailRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<StaffNotificationDetail>> GetStaffNotificationDetailsBy(
            Expression<Func<StaffNotificationDetail, bool>> filter = null,
            Func<IQueryable<StaffNotificationDetail>, ICollection<StaffNotificationDetail>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<StaffNotificationDetail> query = DbSet;

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

            query = query.Include(x => x.Staff)
                .Include(x => x.Type);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
