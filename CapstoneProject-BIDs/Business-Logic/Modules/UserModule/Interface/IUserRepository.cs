using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserModule.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<ICollection<User>> GetUsersBy(
               Expression<Func<User, bool>> filter = null,
               Func<IQueryable<User>, ICollection<User>> options = null,
               string includeProperties = null
           );
    }
}
