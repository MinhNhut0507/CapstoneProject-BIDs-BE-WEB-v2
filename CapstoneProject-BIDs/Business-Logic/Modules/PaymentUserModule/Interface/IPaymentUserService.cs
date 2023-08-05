using Business_Logic.Modules.PaymentUserModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.PaymentUserModule.Interface
{
    public interface IPaymentUserService
    {
        public Task<PaymentUser> AddNewPaymentUser(CreatePaymentUserRequest PaymentUserCreate);
        //public Task<PaymentUser> UpdatePaymentUser(UpdatePaymentUserRequest PaymentUserUpdate);
        public Task<ICollection<PaymentUser>> GetAll();
        public Task<ICollection<PaymentUser>> GetPaymentUserByID(Guid id);
        public Task<ICollection<PaymentUser>> GetPaymentUserBySession(Guid id);

    }
}
