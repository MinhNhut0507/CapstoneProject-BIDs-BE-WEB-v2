using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.StaffModule.Interface
{
    public interface IStaffRepository : IRepository<Staff>
    {
        public Task<ICollection<Staff>> GetStaffsBy(
               Expression<Func<Staff, bool>> filter = null,
               Func<IQueryable<Staff>, ICollection<Staff>> options = null,
               string includeProperties = null
           );
    }
}
