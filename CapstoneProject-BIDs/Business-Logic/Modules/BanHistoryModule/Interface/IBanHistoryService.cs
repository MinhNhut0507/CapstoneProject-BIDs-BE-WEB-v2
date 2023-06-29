using Business_Logic.Modules.BanHistoryModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BanHistoryModule.Interface
{
    public interface IBanHistoryService
    {
        public Task<BanHistory> AddNewBanHistory(CreateBanHistoryRequest BanHistoryCreate);

        public Task<BanHistory> UpdateBanHistory(UpdateBanHistoryRequest BanHistoryUpdate);

        //public Task DeleteBanHistory(Guid? BanHistoryDeleteID);

        public Task<ICollection<BanHistory>> GetAll();

        public Task<ICollection<BanHistory>> GetBanHistoryByUserID(Guid? id);

        public Task<ICollection<BanHistory>> GetBanHistoryByUserName(string Name);

    }
}
