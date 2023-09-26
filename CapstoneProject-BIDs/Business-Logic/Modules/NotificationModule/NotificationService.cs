using Business_Logic.Modules.NotificationModule.Interface;
using Business_Logic.Modules.NotificationModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.NotificationModule
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _NotificationRepository;
        public NotificationService(INotificationRepository NotificationRepository)
        {
            _NotificationRepository = NotificationRepository;
        }

        public async Task<ICollection<Notification>> GetAll()
        {
            return await _NotificationRepository.GetAll();
        }

        public Task<ICollection<Notification>> GetNotificationsIsValid()
        {
            return _NotificationRepository.GetNotificationsBy(x => x.Status == true);
        }

        public async Task<Notification> GetNotificationByID(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Notification = await _NotificationRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Notification == null)
            {
                throw new Exception(ErrorMessage.NotificationError.NOTIFICATION_NOT_FOUND);
            }
            return Notification;
        }

        public async Task<Notification> AddNewNotification(CreateNotificationRequest NotificationRequest)
        {

            ValidationResult result = new CreateNotificationRequestValidator().Validate(NotificationRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newNotification = new Notification();

            newNotification.Id = Guid.NewGuid();
            var date = DateTime.UtcNow;
            newNotification.CreateDate = date.AddHours(7);
            newNotification.ExpireDate = newNotification.CreateDate.AddDays(NotificationRequest.ExpireDate);
            newNotification.Status = true;

            await _NotificationRepository.AddAsync(newNotification);
            return newNotification;
        }

        public async Task<Notification> UpdateNotification(UpdateNotificationRequest NotificationRequest)
        {
            try
            {
                var NotificationUpdate = await GetNotificationByID(NotificationRequest.Id);

                if (NotificationUpdate == null)
                {
                    throw new Exception(ErrorMessage.NotificationError.NOTIFICATION_NOT_FOUND);
                }

                ValidationResult result = new UpdateNotificationRequestValidator().Validate(NotificationRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                NotificationUpdate.ExpireDate = NotificationRequest.ExpireDate;
                NotificationUpdate.Status = NotificationRequest.Status;

                await _NotificationRepository.UpdateAsync(NotificationUpdate);
                return NotificationUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Notification> DeleteNotification(Guid NotificationDeleteID)
        {
            try
            {
                if (NotificationDeleteID != null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                var NotificationDelete = await _NotificationRepository.GetFirstOrDefaultAsync(x => x.Id == NotificationDeleteID);

                if (NotificationDelete == null)
                {
                    throw new Exception(ErrorMessage.NotificationError.NOTIFICATION_NOT_FOUND);
                }

                await _NotificationRepository.RemoveAsync(NotificationDelete);
                return NotificationDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
