using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserPaymentInformationModule
{
    public class UserPaymentInformationRepository : Repository<UserPaymentInformation>, IUserPaymentInformationRepository
    {
        private readonly BIDsContext _db;

        public UserPaymentInformationRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<UserPaymentInformation>> GetUserPaymentInformationsBy(
            Expression<Func<UserPaymentInformation, bool>> filter = null,
            Func<IQueryable<UserPaymentInformation>, ICollection<UserPaymentInformation>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<UserPaymentInformation> query = DbSet;

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

            //query = query.Include(s => s.PaymentStaffs)
            //    .Include(s => s.User);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
