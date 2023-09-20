using Business_Logic.Modules.UserModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserModule
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        private readonly BIDsContext _db;

        public UserRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Users>> GetUsersBy(
            Expression<Func<Users, bool>> filter = null,
            Func<IQueryable<Users>, ICollection<Users>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Users> query = DbSet;

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

            query = query.Include(s => s.UserPaymentInformations);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
