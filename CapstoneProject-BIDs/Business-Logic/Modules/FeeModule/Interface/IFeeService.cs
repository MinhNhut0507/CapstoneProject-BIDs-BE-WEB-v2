using Business_Logic.Modules.FeeModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.FeeModule.Interface
{
    public interface IFeeService
    {
        public Task<Fee> AddNewFee(CreateFeeRequest FeeCreate);

        public Task<Fee> UpdateFee(UpdateFeeRequest FeeUpdate);

        public Task<Fee> DeleteFee(int FeeDeleteID);

        public Task<ICollection<Fee>> GetAll();

        public Task<Fee> GetFeeByID(int id);

        public Task<Fee> GetFeeByName(string Name);

    }
}
