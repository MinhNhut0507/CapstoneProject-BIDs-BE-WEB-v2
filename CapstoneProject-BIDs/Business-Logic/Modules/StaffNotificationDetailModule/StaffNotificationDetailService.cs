using Business_Logic.Modules.StaffNotificationDetailModule.Interface;
using Business_Logic.Modules.StaffNotificationDetailModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.StaffNotificationDetailModule
{
    public class StaffNotificationDetailService : IStaffNotificationDetailService
    {
        private readonly IStaffNotificationDetailRepository _StaffNotificationDetailRepository;
        public StaffNotificationDetailService(IStaffNotificationDetailRepository StaffNotificationDetailRepository)
        {
            _StaffNotificationDetailRepository = StaffNotificationDetailRepository;
        }

        public async Task<ICollection<StaffNotificationDetail>> GetAll()
        {
            return await _StaffNotificationDetailRepository.GetAll(includeProperties: "Staff,Type,Notification");
        }

        public async Task<ICollection<StaffNotificationDetail>> GetStaffNotificationDetailByStaff(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var StaffNotificationDetail = await _StaffNotificationDetailRepository.GetAll(includeProperties: "Staff,Type,Notification",
                options: o => o.Where(x => x.StaffId == id).ToList());
            if (StaffNotificationDetail == null)
            {
                throw new Exception(ErrorMessage.StaffNotificationDetailError.STAFF_NOTIFICATION_DETAIL_NOT_FOUND);
            }
            var response = StaffNotificationDetail.OrderByDescending(x => x.Notification.CreateDate).ToList();
            return response;
        }

        public async Task<StaffNotificationDetail> AddNewStaffNotificationDetail(CreateStaffNotificationDetailRequest StaffNotificationDetailRequest)
        {

            ValidationResult result = new CreateStaffNotificationDetailRequestValidator().Validate(StaffNotificationDetailRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newStaffNotificationDetail = new StaffNotificationDetail();

            newStaffNotificationDetail.NotificationId = StaffNotificationDetailRequest.NotificationId;
            newStaffNotificationDetail.StaffId = StaffNotificationDetailRequest.StaffId;
            newStaffNotificationDetail.TypeId = StaffNotificationDetailRequest.TypeId;
            newStaffNotificationDetail.Messages = StaffNotificationDetailRequest.Messages;

            await _StaffNotificationDetailRepository.AddAsync(newStaffNotificationDetail);
            return newStaffNotificationDetail;
        }

        public async Task Delete(Guid notificationId, Guid staffId)
        {
            try
            {
                var StaffNotificationDetailUpdate = await _StaffNotificationDetailRepository.GetFirstOrDefaultAsync(x => x.NotificationId == notificationId & x.StaffId == staffId);

                if (StaffNotificationDetailUpdate == null)
                {
                    throw new Exception(ErrorMessage.StaffNotificationDetailError.STAFF_NOTIFICATION_DETAIL_NOT_FOUND);
                }

                await _StaffNotificationDetailRepository.RemoveAsync(StaffNotificationDetailUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
