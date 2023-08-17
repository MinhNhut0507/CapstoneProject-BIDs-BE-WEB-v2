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
using Business_Logic.Modules.BookingItemModule.Response;
using Business_Logic.Modules.ItemDescriptionModule.Response;
using Business_Logic.Modules.FeeModule.Response;
using Business_Logic.Modules.SessionRuleModule.Response;
using Common.Helper;
using Business_Logic.Modules.NotificationModule.Response;
using Business_Logic.Modules.NotificationTypeModule.Response;
using Business_Logic.Modules.StaffNotificationDetailModule.Response;
using Business_Logic.Modules.UserNotificationDetailModule.Response;
using Business_Logic.Modules.ImageModule.Response;
using Business_Logic.Modules.SessionRuleModule.Request;
using Business_Logic.Modules.UserPaymentInformationModule.Response;

namespace BIDs_API.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DateTime, DTODateTime>()
                .ForMember(x => x.Year, d => d.MapFrom(s => s.Year))
                .ForMember(x => x.Month, d => d.MapFrom(s => s.Month))
                .ForMember(x => x.Day, d => d.MapFrom(s => s.Day))
                .ForMember(x => x.Hours, d => d.MapFrom(s => s.Hour))
                .ForMember(x => x.Minute, d => d.MapFrom(s => s.Minute));

            CreateMap<TimeSpan, TimeDTO>()
                .ForMember(x => x.hour, d => d.MapFrom(s => s.Hours))
                .ForMember(x => x.minute, d => d.MapFrom(s => s.Minutes))
                .ForMember(x => x.second, d => d.MapFrom(s => s.Seconds));

            CreateMap<DateTime, DTODateOfBirth>()
                .ForMember(x => x.Year, d => d.MapFrom(s => s.Year))
                .ForMember(x => x.Month, d => d.MapFrom(s => s.Month))
                .ForMember(x => x.Day, d => d.MapFrom(s => s.Day));

            CreateMap<Staff, StaffResponse>()
                .ForMember(x => x.StaffId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.Email, d => d.MapFrom(s => s.Email))
                .ForMember(x => x.StaffName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.DateOfBirth, d => d.MapFrom(s => s.DateOfBirth))
                .ForMember(x => x.Address, d => d.MapFrom(s => s.Address))
                .ForMember(x => x.Phone, d => d.MapFrom(s => s.Phone))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<Users, UserResponse>()
                .ForMember(x => x.UserId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.Role, d => d.ConvertUsing(new RoleEnumConverter(), s => s.Role))
                .ForMember(x => x.Email, d => d.MapFrom(s => s.Email))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.Password, d => d.MapFrom(s => s.Password))
                .ForMember(x => x.Avatar, d => d.MapFrom(s => s.Avatar))
                .ForMember(x => x.DateOfBirth, d => d.MapFrom(s => s.DateOfBirth))
                .ForMember(x => x.Address, d => d.MapFrom(s => s.Address))
                .ForMember(x => x.Phone, d => d.MapFrom(s => s.Phone))
                .ForMember(x => x.Cccdnumber, d => d.MapFrom(s => s.Cccdnumber))
                .ForMember(x => x.CccdbackImage, d => d.MapFrom(s => s.CccdbackImage))
                .ForMember(x => x.CccdfrontImage, d => d.MapFrom(s => s.CccdfrontImage))
                .ForMember(x => x.Status, d => d.ConvertUsing(new UserStatusEnumConverter(), s => s.Status));

            CreateMap<Image, ImageResponseOrther>()
                .ForMember(x => x.Detail, d => d.MapFrom(s => s.DetailImage));

            CreateMap<SessionDetail, SessionDetailResponse>()
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
                .ForMember(x => x.Id, d => d.MapFrom(s => s.Id));

            CreateMap<Description, DescriptionDetailResponse>()
                .ForMember(x => x.DescriptionId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.DescriptionName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.CategoryID, d => d.MapFrom(s => s.CategoryId))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Category.Name))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));


            CreateMap<Category, CategoryResponse>()
                .ForMember(x => x.CategoryId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.Descriptions.ToList()))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            //CreateMap<Description, ItemDescriptionResponse>()
            //    .ForMember(x => x.Description, d => d.MapFrom(s => s.Name));

            CreateMap<ItemDescription, ItemDescriptionResponse>()
                .ForMember(x => x.Description, d => d.MapFrom(s => s.Description.Name))
                .ForMember(x => x.Detail, d => d.MapFrom(s => s.Detail));

            CreateMap<Item, ItemResponse>()
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Category.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.ItemDescriptions.ToList()))
                .ForMember(x => x.DescriptionDetail, d => d.MapFrom(s => s.DescriptionDetail))
                .ForMember(x => x.Quantity, d => d.MapFrom(s => s.Quantity))
                .ForMember(x => x.AuctionTime, d => d.MapFrom(s => s.AuctionTime))
                .ForMember(x => x.Images, d => d.MapFrom(s => s.Images.ToList()))
                .ForMember(x => x.FirstPrice, d => d.MapFrom(s => s.FirstPrice))
                .ForMember(x => x.StepPrice, d => d.MapFrom(s => s.StepPrice))
                .ForMember(x => x.Deposit, d => d.MapFrom(s => s.Deposit))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate));

            CreateMap<Session, SessionResponseComplete>()
                .ForMember(x => x.SessionId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.FeeName, d => d.MapFrom(s => s.Fee.Name))
                .ForMember(x => x.SessionName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Item.Name))
                .ForMember(x => x.AuctionTime, d => d.MapFrom(s => s.Item.AuctionTime))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Item.Category.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.Item.ItemDescriptions.ToList()))
                .ForMember(x => x.Images, d => d.MapFrom(s => s.Item.Images.ToList()))
                .ForMember(x => x.Description, d => d.MapFrom(s => s.Item.DescriptionDetail))
                .ForMember(x => x.BeginTime, d => d.MapFrom(s => s.BeginTime))
                .ForMember(x => x.EndTime, d => d.MapFrom(s => s.EndTime))
                .ForMember(x => x.ParticipationFee, d => d.MapFrom(s => s.Fee.ParticipationFee))
                .ForMember(x => x.DepositFee, d => d.MapFrom(s => s.Fee.DepositFee))
                .ForMember(x => x.Deposit, d => d.MapFrom(s => s.Item.Deposit))
                .ForMember(x => x.FinalPrice, d => d.MapFrom(s => s.FinalPrice))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<Session, SessionResponse>()
                .ForMember(x => x.SessionId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.FeeId, d => d.MapFrom(s => s.FeeId))
                .ForMember(x => x.FeeName, d => d.MapFrom(s => s.Fee.Name))
                .ForMember(x => x.SessionName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.Item.Id))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Item.Name))
                .ForMember(x => x.AuctionTime, d => d.MapFrom(s => s.Item.AuctionTime))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Item.Category.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.Item.ItemDescriptions.ToList()))
                .ForMember(x => x.Images, d => d.MapFrom(s => s.Item.Images.ToList()))
                .ForMember(x => x.Description, d => d.MapFrom(s => s.Item.DescriptionDetail))
                .ForMember(x => x.FreeTime, d => d.MapFrom(s => s.SessionRule.FreeTime))
                .ForMember(x => x.DelayTime, d => d.MapFrom(s => s.SessionRule.DelayTime))
                .ForMember(x => x.DelayFreeTime, d => d.MapFrom(s => s.SessionRule.DelayFreeTime))
                .ForMember(x => x.BeginTime, d => d.MapFrom(s => s.BeginTime))
                .ForMember(x => x.EndTime, d => d.MapFrom(s => s.EndTime))
                .ForMember(x => x.FirstPrice, d => d.MapFrom(s => s.Item.FirstPrice))
                .ForMember(x => x.ParticipationFee, d => d.MapFrom(s => s.Fee.ParticipationFee))
                .ForMember(x => x.DepositFee, d => d.MapFrom(s => s.Fee.DepositFee))
                .ForMember(x => x.Deposit, d => d.MapFrom(s => s.Item.Deposit))
                .ForMember(x => x.FinalPrice, d => d.MapFrom(s => s.FinalPrice))
                .ForMember(x => x.StepPrice, d => d.MapFrom(s => s.Item.StepPrice))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<BanHistory, BanHistoryResponse>()
                .ForMember(x => x.BanId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.User.Name))
                .ForMember(x => x.Reason, d => d.MapFrom(s => s.Reason))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<BookingItem, BookingItemResponse>()
                .ForMember(x => x.BookingItemId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.Item.Id))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Item.Name))
                .ForMember(x => x.UserName, d => d.MapFrom(s => s.Item.User.Name))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Item.Category.Name))
                .ForMember(x => x.Descriptions, d => d.MapFrom(s => s.Item.ItemDescriptions.ToList()))
                .ForMember(x => x.Images, d => d.MapFrom(s => s.Item.Images.ToList()))
                .ForMember(x => x.DescriptionDetail, d => d.MapFrom(s => s.Item.DescriptionDetail))
                .ForMember(x => x.Deposit, d => d.MapFrom(s => s.Item.Deposit))
                .ForMember(x => x.Quantity, d => d.MapFrom(s => s.Item.Quantity))
                .ForMember(x => x.FirstPrice, d => d.MapFrom(s => s.Item.FirstPrice))
                .ForMember(x => x.StepPrice, d => d.MapFrom(s => s.Item.StepPrice))
                .ForMember(x => x.StaffName, d => d.MapFrom(s => s.Staff.Name))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.ConvertUsing(new BookingItemEnumConverter(), s => s.Status));

            CreateMap<ItemDescription, ItemDescriptionDetailResponse>()
                .ForMember(x => x.ItemDescriptionId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.CategoryName, d => d.MapFrom(s => s.Description.Category.Name))
                .ForMember(x => x.DescriptionName, d => d.MapFrom(s => s.Description.Name))
                .ForMember(x => x.Detail, d => d.MapFrom(s => s.Detail))
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.ItemId))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Item.Name))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<Fee, FeeResponse>()
                .ForMember(x => x.FeeId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.FeeName, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.Min, d => d.MapFrom(s => s.Min))
                .ForMember(x => x.Max, d => d.MapFrom(s => s.Max))
                .ForMember(x => x.ParticipationFee, d => d.MapFrom(s => s.ParticipationFee))
                .ForMember(x => x.DepositFee, d => d.MapFrom(s => s.DepositFee))
                .ForMember(x => x.Surcharge, d => d.MapFrom(s => s.Surcharge))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<SessionRule, SessionRuleResponse>()
                .ForMember(x => x.SessionRuleId, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.Name, d => d.MapFrom(s => s.Name))
                .ForMember(x => x.FreeTime, d => d.MapFrom(s => s.FreeTime))
                .ForMember(x => x.DelayTime, d => d.MapFrom(s => s.DelayTime))
                .ForMember(x => x.DelayFreeTime, d => d.MapFrom(s => s.DelayFreeTime))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.UpdateDate, d => d.MapFrom(s => s.UpdateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<StaffNotificationDetail, StaffNotificationDetailResponse>()
                .ForMember(x => x.NotificationId, d => d.MapFrom(s => s.NotificationId))
                .ForMember(x => x.Message, d => d.MapFrom(s => s.Messages))
                .ForMember(x => x.StaffId, d => d.MapFrom(s => s.StaffId))
                .ForMember(x => x.TypeName, d => d.MapFrom(s => s.Type.Name));

            CreateMap<Notification, NotificationResponse>()
                .ForMember(x => x.Id, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.ExpireDate, d => d.MapFrom(s => s.ExpireDate))
                .ForMember(x => x.CreateDate, d => d.MapFrom(s => s.CreateDate))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status));

            CreateMap<NotificationType, NotificationTypeResponse>()
                .ForMember(x => x.Id, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.Name, d => d.MapFrom(s => s.Name));

            CreateMap<UserNotificationDetail, UserNotificationDetailResponse>()
                .ForMember(x => x.NotificationId, d => d.MapFrom(s => s.NotificationId))
                .ForMember(x => x.Message, d => d.MapFrom(s => s.Messages))
                .ForMember(x => x.UserId, d => d.MapFrom(s => s.UserId))
                .ForMember(x => x.TypeName, d => d.MapFrom(s => s.Type.Name));

            CreateMap<Image, ImageResponse>()
                .ForMember(x => x.Id, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.ItemId, d => d.MapFrom(s => s.Item.Id))
                .ForMember(x => x.ItemName, d => d.MapFrom(s => s.Item.Name))
                .ForMember(x => x.Detail, d => d.MapFrom(s => s.DetailImage));

            CreateMap<UserPaymentInformation, UserPaymentInformationResponse>()
                .ForMember(x => x.Id, d => d.MapFrom(s => s.Id))
                .ForMember(x => x.Status, d => d.MapFrom(s => s.Status))
                .ForMember(x => x.UserId, d => d.MapFrom(s => s.UserId))
                .ForMember(x => x.PayPalAccount, d => d.MapFrom(s => s.PayPalAccount));
        }

        public class RoleEnumConverter : IValueConverter<int, string>
        {
            public string Convert(int sourceMember, ResolutionContext context)
            {
                return ((RoleEnum)sourceMember).ToString();
            }
        }

        public class BookingItemEnumConverter : IValueConverter<int, string>
        {
            public string Convert(int sourceMember, ResolutionContext context)
            {
                return ((BookingItemEnum)sourceMember).ToString();
            }
        }

        public class UserStatusEnumConverter : IValueConverter<int, string>
        {
            public string Convert(int sourceMember, ResolutionContext context)
            {
                return ((UserStatusEnum)sourceMember).ToString();
            }
        }
    }
}
