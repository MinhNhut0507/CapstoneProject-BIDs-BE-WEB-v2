using Business_Logic.Modules.StaffModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.StaffModule
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        private readonly BIDsContext _db;

        public StaffRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Staff>> GetStaffsBy(
            Expression<Func<Staff, bool>> filter = null,
            Func<IQueryable<Staff>, ICollection<Staff>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Staff> query = DbSet;

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


            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
