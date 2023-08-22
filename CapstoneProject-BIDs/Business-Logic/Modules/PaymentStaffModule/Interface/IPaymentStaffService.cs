using Business_Logic.Modules.PaymentStaffModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.PaymentStaffModule.Interface
{
    public interface IPaymentStaffService
    {
        public Task<PaymentStaff> AddNewPaymentStaff(CreatePaymentStaffRequest PaymentStaffCreate);
        public Task<PaymentStaff> AddNewReturnDeposit(CreateReturnDepositRequest PaymentStaffCreate);
        public Task<PaymentStaff> UpdatePaymentStaff(UpdatePaymentStaffStatusRequest PaymentStaffUpdate);
        public Task<ICollection<PaymentStaff>> GetAll();
        public Task<ICollection<PaymentStaff>> GetPaymentStaffByID(Guid id);
        public Task<ICollection<PaymentStaff>> GetPaymentStaffBySession(Guid id);

    }
}
