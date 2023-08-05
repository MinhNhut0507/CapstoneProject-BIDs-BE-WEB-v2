using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.PaymentUserModule.Interface
{
    public interface IPaymentUserRepository : IRepository<PaymentUser>
    {
        public Task<ICollection<PaymentUser>> GetPaymentUsersBy(
               Expression<Func<PaymentUser, bool>> filter = null,
               Func<IQueryable<PaymentUser>, ICollection<PaymentUser>> options = null,
               string includeProperties = null
           );
    }
}
