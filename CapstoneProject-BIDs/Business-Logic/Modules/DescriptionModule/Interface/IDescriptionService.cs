using Business_Logic.Modules.DescriptionModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.DescriptionModule.Interface
{
    public interface IDescriptionService
    {
        public Task<Description> AddNewDescription(CreateDescriptionRequest DescriptionCreate);

        public Task<Description> UpdateDescription(UpdateDescriptionRequest DescriptionUpdate);

        public Task<Description> DeleteDescription(Guid? DescriptionDeleteID);

        public Task<ICollection<Description>> GetAll();

        public Task<Description> GetDescriptionByID(Guid? id);

        public Task<Description> GetDescriptionByCategoryName(string Name);

    }
}
