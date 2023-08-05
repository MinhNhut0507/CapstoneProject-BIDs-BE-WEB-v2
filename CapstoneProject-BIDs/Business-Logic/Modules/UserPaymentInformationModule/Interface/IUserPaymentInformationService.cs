using Business_Logic.Modules.UserPaymentInformationModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserPaymentInformationModule.Interface
{
    public interface IUserPaymentInformationService
    {
        public Task<UserPaymentInformation> AddNewUserPaymentInformation(CreateUserPaymentInformationRequest UserPaymentInformationCreate);
        public Task<UserPaymentInformation> UpdateUserPaymentInformation(UpdateUserPaymentInformationRequest UserPaymentInformationUpdate);
        public Task<ICollection<UserPaymentInformation>> GetAll();
        public Task<ICollection<UserPaymentInformation>> GetUserPaymentInformationByID(Guid id);
        public Task<ICollection<UserPaymentInformation>> GetUserPaymentInformationByUser(Guid id);

    }
}
