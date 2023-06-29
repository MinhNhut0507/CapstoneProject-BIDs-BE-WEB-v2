using Business_Logic.Modules.AdminModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.AdminModule.Interface
{
    public interface IAdminService
    {
        public Task<Admin> AddNewAdmin(CreateAdminRequest AdminCreate);

        public Task<Admin> UpdateAdmin(UpdateAdminRequest AdminUpdate);

        public Task<ICollection<Admin>> GetAll();

        public Task<Admin> GetAdminByID(Guid? id);

        public Task<Admin> GetAdminByName(string Name);

        public Task<Admin> GetAdminByEmail(string Email);

    }
}
