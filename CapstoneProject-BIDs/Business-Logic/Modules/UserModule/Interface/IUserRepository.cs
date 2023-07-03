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
    public interface IUserRepository : IRepository<Users>
    {
        public Task<ICollection<Users>> GetUsersBy(
               Expression<Func<Users, bool>> filter = null,
               Func<IQueryable<Users>, ICollection<Users>> options = null,
               string includeProperties = null
           );
    }
}
