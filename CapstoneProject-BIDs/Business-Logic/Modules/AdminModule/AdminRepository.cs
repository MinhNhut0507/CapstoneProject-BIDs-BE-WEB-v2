using Business_Logic.Modules.AdminModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.AdminModule
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly BIDsContext _db;

        public AdminRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Admin>> GetAdminsBy(
            Expression<Func<Admin, bool>> filter = null,
            Func<IQueryable<Admin>, ICollection<Admin>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Admin> query = DbSet;

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
