using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserPaymentInformationModule.Interface
{
    public interface IUserPaymentInformationRepository : IRepository<UserPaymentInformation>
    {
        public Task<ICollection<UserPaymentInformation>> GetUserPaymentInformationsBy(
               Expression<Func<UserPaymentInformation, bool>> filter = null,
               Func<IQueryable<UserPaymentInformation>, ICollection<UserPaymentInformation>> options = null,
               string includeProperties = null
           );
    }
}
