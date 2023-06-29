using AutoMapper;
using Business_Logic.Modules.BanHistoryModule.Response;
using Business_Logic.Modules.ItemModule.Response;
using Business_Logic.Modules.CategoryModule.Response;
using Business_Logic.Modules.SessionDetailModule.Response;
using Business_Logic.Modules.SessionModule.Response;
using Business_Logic.Modules.StaffModule.Response;
using Business_Logic.Modules.UserModule.Response;
using Data_Access.Entities;
using Data_Access.Enum;
using Business_Logic.Modules.DescriptionModule.Response;

namespace BIDs_API.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Staff, StaffResponseStaff>()
                .ForMember(x => x.Email, d => d.MapFrom(s => s.Email))
                .ForMember(x => x.Password, d => d.MapFrom(s => s.Password))
                .ForMember(x => x.StaffName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.DateOfBirth, d => d.MapFrom(s => s.DateOfBirth))
                .ForMember(x => x.Address, d => d.MapFrom(s => s.Address))
                .ForMember(x => x.Phone, d => d.MapFrom(s => s.Phone));
            CreateMap<Staff, StaffResponseAdmin>()
                .ForMember(x => x.StaffId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.Email, d => d.MapFrom(s => s.Email))
                .ForMember(x => x.StaffName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.DateOfBirth, d => d.MapFrom(s => s.DateOfBirth))
                .ForMember(x => x.Address, d => d.MapFrom(s => s.Address))
                .ForMember(x => x.Phone, d => d.MapFrom(s => s.Phone))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<User, UserResponseUser>()
                .ForMember(x => x.Email, d => d.MapFrom(s => s.Email))
                .ForMember(x => x.Role, d => d.ConvertUsing(new UserRoleConverter(), s => s.Role))
                .ForMember(x => x.Password, d => d.MapFrom(s => s.Password))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.DateOfBirth, d => d.MapFrom(s => s.DateOfBirth))
                .ForMember(x => x.Address, d => d.MapFrom(s => s.Address))
                .ForMember(x => x.Phone, d => d.MapFrom(s => s.Phone))
                .ForMember(x => x.CCCDNumber, d => d.MapFrom(s => s.Cccdnumber))
                .ForMember(x => x.CCCDBackImage, d => d.MapFrom(s => s.CccdbackImage))
                .ForMember(x => x.CCCDFrontImage, d => d.MapFrom(s => s.CccdfrontImage));
            
            CreateMap<User, UserResponseStaffAndAdmin>()
                .ForMember(x => x.UserId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.Role, d => d.ConvertUsing(new UserRoleConverter(), s => s.Role))
                .ForMember(x => x.Email, d => d.MapFrom(s => s.Email))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.DateOfBirth, d => d.MapFrom(s => s.DateOfBirth))
                .ForMember(x => x.Address, d => d.MapFrom(s => s.Address))
                .ForMember(x => x.Phone, d => d.MapFrom(s => s.Phone))
                .ForMember(x => x.Cccdnumber, d => d.MapFrom(s => s.Cccdnumber))
                .ForMember(x => x.CccdbackImage, d => d.MapFrom(s => s.CccdbackImage))
                .ForMember(x => x.CccdfrontImage, d => d.MapFrom(s => s.CccdfrontImage))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));
            
            CreateMap<Session, SessionResponseUser>()
                .ForMember(x => x.FeeName, d => d.MapFrom(s => s.Fee.Name))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Item.Name))
                .ForMember(x => x.SessionName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.BeginTime, d => d.MapFrom(s => s.BeginTime))
                .ForMember(x => x.AuctionTime, d => d.MapFrom(s => s.AuctionTime))
                .ForMember(x => x.EndTime, d => d.MapFrom(s => s.EndTime))
                .ForMember(x => x.FinailPrice, d => d.MapFrom(s => s.FinailPrice));
            
            CreateMap<Session, SessionResponseStaffAndAdmin>()
                .ForMember(x => x.SessionId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.FeeId, d => d.MapFrom(s => s.Fee.Id))
                .ForMember(x => x.FeeName, d => d.MapFrom(s => s.Fee.Name))
                .ForMember(x => x.SessionName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.Item.Id))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Item.Name))
                .ForMember(x => x.BeginTime, d => d.MapFrom(s => s.BeginTime))
                .ForMember(x => x.AuctionTime, d => d.MapFrom(s => s.AuctionTime))
                .ForMember(x => x.EndTime, d => d.MapFrom(s => s.EndTime))
                .ForMember(x => x.FinailPrice, d => d.MapFrom(s => s.FinailPrice))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));
            
            CreateMap<SessionDetail, SessionDetailResponseUser>()
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Session.Item.Name))
                .ForMember(x => x.SessionName, d => d.MapFrom(s => s.Session.Name))
                .ForMember(x => x.Price, d => d.MapFrom(s => s.Price))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate));
            
            CreateMap<SessionDetail, SessionDetailResponseStaffAndAdmin>()
                .ForMember(x => x.SessionDetailId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.UserId, d => d.MapFrom(s => s.User.Id))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.Session.Item.Id))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Session.Item.Name))
                .ForMember(x => x.SessionId, d => d.MapFrom(s => s.SessionId))
                .ForMember(x => x.SessionName, d => d.MapFrom(s => s.Session.Name))
                .ForMember(x => x.Price, d => d.MapFrom(s => s.Price))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<Description, DescriptionResponse>()
                .ForMember(x => x.Name, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.Detail, d => d.MapFrom(s => s.Detail));

            CreateMap<Category, CategoryResponseAdmin>()
                .ForMember(x => x.CategoryId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.Descriptions.ToList()))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<Category, CategoryResponseUserAndStaff>()
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.Description, d => d.MapFrom(s => s.Descriptions.ToList()));

            CreateMap<Item, ItemResponseUser>()
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Category.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.Category.Descriptions.ToList()))
                .ForMember(x => x.DescriptionDetail, d => d.MapFrom(s => s.DescriptionDetail))
                .ForMember(x => x.Quantity, d => d.MapFrom(s => s.Quantity))
                .ForMember(x => x.Image, d => d.MapFrom(s => s.Image))
                .ForMember(x => x.FristPrice, d => d.MapFrom(s => s.FristPrice))
                .ForMember(x => x.StepPrice, d => d.MapFrom(s => s.StepPrice))
                .ForMember(x => x.Deposit, d => d.MapFrom(s => s.Deposit));
            
            CreateMap<Item, ItemResponseStaffAndAdmin>()
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Category.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.Category.Descriptions.ToList()))
                .ForMember(x => x.DescriptionDetail, d => d.MapFrom(s => s.DescriptionDetail))
                .ForMember(x => x.Quantity, d => d.MapFrom(s => s.Quantity))
                .ForMember(x => x.Image, d => d.MapFrom(s => s.Image))
                .ForMember(x => x.FristPrice, d => d.MapFrom(s => s.FristPrice))
                .ForMember(x => x.StepPrice, d => d.MapFrom(s => s.StepPrice))
                .ForMember(x => x.Deposit, d => d.MapFrom(s => s.Deposit))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<BanHistory, BanHistoryResponseUser>()
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.Reason, d => d.MapFrom(s => s.Reason))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate));

            CreateMap<BanHistory, BanHistoryResponseAdminAndStaff>()
                .ForMember(x => x.BanId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.Reason, d => d.MapFrom(s => s.Reason))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));
        }

        public class UserRoleConverter : IValueConverter<int, string>
        {
            public string Convert(int sourceMember, ResolutionContext context)
            {
                return ((RoleEnum)sourceMember).ToString();
            }
        }
    }
}
