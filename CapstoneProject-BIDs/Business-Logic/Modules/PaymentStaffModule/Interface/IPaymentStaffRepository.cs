using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.PaymentStaffModule.Interface
{
    public interface IPaymentStaffRepository : IRepository<PaymentStaff>
    {
        public Task<ICollection<PaymentStaff>> GetPaymentStaffsBy(
               Expression<Func<PaymentStaff, bool>> filter = null,
               Func<IQueryable<PaymentStaff>, ICollection<PaymentStaff>> options = null,
               string includeProperties = null
           );
    }
}
