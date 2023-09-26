using Business_Logic.Modules.UserNotificationDetailModule.Interface;
using Business_Logic.Modules.UserNotificationDetailModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.UserNotificationDetailModule
{
    public class UserNotificationDetailService : IUserNotificationDetailService
    {
        private readonly IUserNotificationDetailRepository _UserNotificationDetailRepository;
        public UserNotificationDetailService(IUserNotificationDetailRepository UserNotificationDetailRepository)
        {
            _UserNotificationDetailRepository = UserNotificationDetailRepository;
        }

        public async Task<ICollection<UserNotificationDetail>> GetAll()
        {
            return await _UserNotificationDetailRepository.GetAll(includeProperties: "User,Type,Notification");
        }

        public async Task<ICollection<UserNotificationDetail>> GetUserNotificationDetailByUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var UserNotificationDetail = await _UserNotificationDetailRepository.GetAll(includeProperties: "User,Type,Notification",
                options: o => o.Where( x => x.UserId == id).ToList());
            if (UserNotificationDetail == null)
            {
                throw new Exception(ErrorMessage.UserNotificationDetailError.USER_NOTIFICATION_DETAIL_NOT_FOUND);
            }
            var response = UserNotificationDetail.OrderByDescending(x => x.Notification.CreateDate).ToList();
            return response;
        }

        public async Task<UserNotificationDetail> AddNewUserNotificationDetail(CreateUserNotificationDetailRequest UserNotificationDetailRequest)
        {

            ValidationResult result = new CreateUserNotificationDetailRequestValidator().Validate(UserNotificationDetailRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newUserNotificationDetail = new UserNotificationDetail();

            newUserNotificationDetail.NotificationId = UserNotificationDetailRequest.NotificationId;
            newUserNotificationDetail.UserId = UserNotificationDetailRequest.UserId;
            newUserNotificationDetail.TypeId = UserNotificationDetailRequest.TypeId;
            newUserNotificationDetail.Messages = UserNotificationDetailRequest.Messages;

            await _UserNotificationDetailRepository.AddAsync(newUserNotificationDetail);
            return newUserNotificationDetail;
        }

        public async Task Delete(Guid notificationId, Guid userId)
        {
            try
            {
                var UserNotificationDetailUpdate = await _UserNotificationDetailRepository.GetFirstOrDefaultAsync(x => x.NotificationId == notificationId & x.UserId == userId);

                if (UserNotificationDetailUpdate == null)
                {
                    throw new Exception(ErrorMessage.UserNotificationDetailError.USER_NOTIFICATION_DETAIL_NOT_FOUND);
                }

                await _UserNotificationDetailRepository.RemoveAsync(UserNotificationDetailUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
