using Business_Logic.Modules.StaffNotificationDetailModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.StaffNotificationDetailModule.Interface
{
    public interface IStaffNotificationDetailService
    {
        public Task<StaffNotificationDetail> AddNewStaffNotificationDetail(CreateStaffNotificationDetailRequest StaffNotificationDetailCreate);

        //public Task<StaffNotificationDetail> UpdateStaffNotificationDetail(UpdateStaffNotificationDetailRequest StaffNotificationDetailUpdate);

        public Task<ICollection<StaffNotificationDetail>> GetAll();

        public Task<ICollection<StaffNotificationDetail>> GetStaffNotificationDetailByStaff(Guid id);

    }
}
