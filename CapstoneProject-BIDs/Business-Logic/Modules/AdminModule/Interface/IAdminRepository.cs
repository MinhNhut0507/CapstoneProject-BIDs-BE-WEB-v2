using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.AdminModule.Interface
{
    public interface IAdminRepository : IRepository<Admin>
    {
        public Task<ICollection<Admin>> GetAdminsBy(
               Expression<Func<Admin, bool>> filter = null,
               Func<IQueryable<Admin>, ICollection<Admin>> options = null,
               string includeProperties = null
           );
    }
}
