using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.DescriptionModule.Interface
{
    public interface IDescriptionRepository : IRepository<Description>
    {
        public Task<ICollection<Description>> GetDescriptionsBy(
               Expression<Func<Description, bool>> filter = null,
               Func<IQueryable<Description>, ICollection<Description>> options = null,
               string includeProperties = null
           );
    }
}
