using Business_Logic.Modules.NotificationTypeModule.Interface;
using Business_Logic.Modules.NotificationTypeModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.NotificationTypeModule
{
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly INotificationTypeRepository _NotificationTypeRepository;
        public NotificationTypeService(INotificationTypeRepository NotificationTypeRepository)
        {
            _NotificationTypeRepository = NotificationTypeRepository;
        }

        public async Task<ICollection<NotificationType>> GetAll()
        {
            return await _NotificationTypeRepository.GetAll();
        }

        public async Task<NotificationType> GetNotificationTypeByID(int id)
        {
            if (id <= 0)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var NotificationType = await _NotificationTypeRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (NotificationType == null)
            {
                throw new Exception(ErrorMessage.NotificationTypeError.NOTIFICATION_TYPE_NOT_FOUND);
            }
            return NotificationType;
        }

        public async Task<NotificationType> AddNewNotificationType(CreateNotificationTypeRequest NotificationTypeRequest)
        {

            ValidationResult result = new CreateNotificationTypeRequestValidator().Validate(NotificationTypeRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var notiType = await _NotificationTypeRepository.GetFirstOrDefaultAsync(x => x.Name == NotificationTypeRequest.Name);

            if(notiType != null)
            {
                throw new Exception(ErrorMessage.NotificationTypeError.NOTIFICATION_TYPE_EXISTED);
            }

            var newNotificationType = new NotificationType();

            newNotificationType.Name = NotificationTypeRequest.Name;

            await _NotificationTypeRepository.AddAsync(newNotificationType);
            return newNotificationType;
        }

        public async Task<NotificationType> UpdateNotificationType(UpdateNotificationTypeRequest NotificationTypeRequest)
        {
            try
            {
                var NotificationTypeUpdate = await GetNotificationTypeByID(NotificationTypeRequest.Id);

                if (NotificationTypeUpdate == null)
                {
                    throw new Exception(ErrorMessage.NotificationTypeError.NOTIFICATION_TYPE_NOT_FOUND);
                }

                ValidationResult result = new UpdateNotificationTypeRequestValidator().Validate(NotificationTypeRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                NotificationTypeUpdate.Name = NotificationTypeRequest.Name;

                await _NotificationTypeRepository.UpdateAsync(NotificationTypeUpdate);
                return NotificationTypeUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }
    }
}
