using Business_Logic.Modules.PaymentStaffModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.PaymentStaffModule
{
    public class PaymentStaffRepository : Repository<PaymentStaff>, IPaymentStaffRepository
    {
        private readonly BIDsContext _db;

        public PaymentStaffRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<PaymentStaff>> GetPaymentStaffsBy(
            Expression<Func<PaymentStaff, bool>> filter = null,
            Func<IQueryable<PaymentStaff>, ICollection<PaymentStaff>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<PaymentStaff> query = DbSet;

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
                .Include(s => s.Session.SessionDetails)
                .Include(s => s.PaymentInformation)
                .Include(s => s.Staff);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
