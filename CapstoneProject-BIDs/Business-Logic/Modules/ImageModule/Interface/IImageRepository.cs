using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ImageModule.Interface
{
    public interface IImageRepository : IRepository<Image>
    {
        public Task<ICollection<Image>> GetImagesBy(
               Expression<Func<Image, bool>> filter = null,
               Func<IQueryable<Image>, ICollection<Image>> options = null,
               string includeProperties = null
           );
    }
}
