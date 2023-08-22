using Business_Logic.Modules.PaymentUserModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.PaymentUserModule
{
    public class PaymentUserRepository : Repository<PaymentUser>, IPaymentUserRepository
    {
        private readonly BIDsContext _db;

        public PaymentUserRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<PaymentUser>> GetPaymentUsersBy(
            Expression<Func<PaymentUser, bool>> filter = null,
            Func<IQueryable<PaymentUser>, ICollection<PaymentUser>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<PaymentUser> query = DbSet;

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

            query = query.Include(s => s.Session)
                .Include(s => s.User);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
