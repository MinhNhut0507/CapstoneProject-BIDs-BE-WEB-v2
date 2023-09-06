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
            return await _UserNotificationDetailRepository.GetAll(includeProperties: "User,Type");
        }

        public async Task<ICollection<UserNotificationDetail>> GetUserNotificationDetailByUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var UserNotificationDetail = await _UserNotificationDetailRepository.GetAll(includeProperties: "User,Type",
                options: o => o.OrderBy( x => x.UserId == id).ToList());
            if (UserNotificationDetail == null)
            {
                throw new Exception(ErrorMessage.UserNotificationDetailError.USER_NOTIFICATION_DETAIL_NOT_FOUND);
            }
            return UserNotificationDetail;
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

        //public async Task<UserNotificationDetail> UpdateUserNotificationDetail(UpdateUserNotificationDetailRequest UserNotificationDetailRequest)
        //{
        //    try
        //    {
        //        var UserNotificationDetailUpdate = await GetUserNotificationDetailByID(UserNotificationDetailRequest.Id);

        //        if (UserNotificationDetailUpdate == null)
        //        {
        //            throw new Exception(ErrorMessage.UserNotificationDetailError.NOTIFICATION_TYPE_NOT_FOUND);
        //        }

        //        ValidationResult result = new UpdateUserNotificationDetailRequestValidator().Validate(UserNotificationDetailRequest);
        //        if (!result.IsValid)
        //        {
        //            throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
        //        }

        //        UserNotificationDetailUpdate.Name = UserNotificationDetailRequest.Name;

        //        await _UserNotificationDetailRepository.UpdateAsync(UserNotificationDetailUpdate);
        //        return UserNotificationDetailUpdate;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error at update type: " + ex.Message);
        //        throw new Exception(ex.Message);
        //    }

        //}
    }
}
