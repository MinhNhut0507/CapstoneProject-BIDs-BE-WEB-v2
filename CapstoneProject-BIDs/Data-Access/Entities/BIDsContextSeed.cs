using Data_Access.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class BIDsContextSeed
    {
        public async Task SeedAsync(BIDsContext context, ILogger<BIDsContextSeed> logger)
        {
            // dữ liệu mẫu cho nhân viên
            if (!context.Staff.Any())
            {
                var Staff = new Staff()
                {
                    Id = Guid.NewGuid(),
                    Address = "115/4/2 đường số 11 thành phố Thủ Đức",
                    CreateDate = DateTime.Now,
                    DateOfBirth = new DateTime(2001, 07, 05),
                    UpdateDate = DateTime.Now,
                    Email = "seedstaff@gmail.com",
                    Password = "05072001",
                    Phone = "0933403842",
                    Name = "Seed Staff",
                    Status = true
                };
                context.Staff.Add(Staff);
            }
            // dữ liệu mẫu cho quản trị viên
            if (!context.Admins.Any())
            {
                var Admin = new Admin()
                {
                    Id = Guid.NewGuid(),
                    Address = "115/4/2 đường số 11 thành phố Thủ Đức",
                    Email = "seedadmin@gmail.com",
                    Password = "05072001",
                    Phone = "0933403842",
                    Name = "Seed Admin",
                    Status = true
                };
                context.Admins.Add(Admin);
            }
            // dữ liệu mẫu cho người dùng
            if (!context.Users.Any())
            {
                var user1 = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "User seed 1",
                    Role = (int)RoleEnum.Bidder,
                    Address = "115/4/2 đường số 11 thành phố Thủ Đức",
                    CreateDate = DateTime.Now,
                    DateOfBirth = new DateTime(2003, 07, 07),
                    UpdateDate = DateTime.Now,
                    Avatar = "Avater mẫu",
                    Email = "minhnhut05072003@gmail.com",
                    Cccdnumber = "077201000702",
                    CccdbackImage = "CCCD mặt sau mẫu",
                    CccdfrontImage = "CCCD mặt trước mẫu",
                    Password = "07072001",
                    Phone = "0933403844",
                    Status = (int)UserStatusEnum.Waitting,

                };
                var user2 = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "User seed 2",
                    Role = (int)RoleEnum.Bidder,
                    Address = "115/4/2 đường số 11 thành phố Thủ Đức",
                    CreateDate = DateTime.Now,
                    DateOfBirth = new DateTime(2003, 07, 07),
                    UpdateDate = DateTime.Now,
                    Avatar = "Avater mẫu",
                    Email = "minhnhut05072001@gmail.com",
                    Cccdnumber = "077201000702",
                    CccdbackImage = "CCCD mặt sau mẫu",
                    CccdfrontImage = "CCCD mặt trước mẫu",
                    Password = "07072001",
                    Phone = "0933403842",
                    Status = (int)UserStatusEnum.Acctive,


                };
                var user3 = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "User seed 3",
                    Role = (int)RoleEnum.Bidder,
                    Address = "115/4/2 đường số 11 thành phố Thủ Đức",
                    CreateDate = DateTime.Now,
                    DateOfBirth = new DateTime(2003, 07, 07),
                    UpdateDate = DateTime.Now,
                    Avatar = "Avater mẫu",
                    Email = "minhnhut05072004@gmail.com",
                    Cccdnumber = "077201000702",
                    CccdbackImage = "CCCD mặt sau mẫu",
                    CccdfrontImage = "CCCD mặt trước mẫu",
                    Password = "07072001",
                    Phone = "0933403844",
                    Status = (int)UserStatusEnum.Deny

                };
                var user4 = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "User seed 4",
                    Role = (int)RoleEnum.Auctioneer,
                    Address = "115/4/2 đường số 11 thành phố Thủ Đức",
                    CreateDate = DateTime.Now,
                    DateOfBirth = new DateTime(2003, 07, 07),
                    UpdateDate = DateTime.Now,
                    Avatar = "Avater mẫu",
                    Email = "nhutdmse151298@fpt.edu.vn",
                    Cccdnumber = "077201000702",
                    CccdbackImage = "CCCD mặt sau mẫu",
                    CccdfrontImage = "CCCD mặt trước mẫu",
                    Password = "07072001",
                    Phone = "0933403844",
                    Status = (int)UserStatusEnum.Acctive

                };
                var user5 = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "User seed 5",
                    Role = (int)RoleEnum.Auctioneer,
                    Address = "115/4/2 đường số 11 thành phố Thủ Đức",
                    CreateDate = DateTime.Now,
                    DateOfBirth = new DateTime(2003, 07, 07),
                    UpdateDate = DateTime.Now,
                    Avatar = "Avater mẫu",
                    Email = "nhutdmse1512910@fpt.edu.vn",
                    Cccdnumber = "077201000702",
                    CccdbackImage = "CCCD mặt sau mẫu",
                    CccdfrontImage = "CCCD mặt trước mẫu",
                    Password = "07072001",
                    Phone = "0933403844",
                    Status = (int)UserStatusEnum.Ban

                };
                context.Users.Add(user1);
                context.Users.Add(user2);
                context.Users.Add(user3);
                context.Users.Add(user4);
                context.Users.Add(user5);
                await context.SaveChangesAsync();
                var ban = new BanHistory()
                {
                    Id = Guid.NewGuid(),
                    Reason = "Dữ liệu khóa tài khoản mẫu",
                    UserId = user5.Id,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = true
                };
                context.BanHistories.Add(ban);
            }
            await context.SaveChangesAsync();
            // dữ liệu mẫu cho loại sản phẩm
            if (!context.Categories.Any())
            {
                var category = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Đồ công nghệ",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = true
                };
                var category2 = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Xe máy",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = true
                };
                var category3 = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Xe đạp",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = true
                };
                var category4 = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Đồ cổ",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = false
                };
                context.Categories.Add(category);
                context.Categories.Add(category2);
                context.Categories.Add(category3);
                context.Categories.Add(category4);
                await context.SaveChangesAsync();
                
                // dữ liệu mẫu của mô tả cho xe máy
                var description = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category2.Id,
                    Name = "Màu sắc",
                    Status = true
                };
                var description2 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category2.Id,
                    Name = "Biển số",
                    Status = true
                };
                var description3 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category2.Id,
                    Name = "Hãng xe",
                    Status = true
                };
                var description4 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category2.Id,
                    Name = "Mua vào năm",
                    Status = true
                };
                context.Descriptions.Add(description);
                context.Descriptions.Add(description2);
                context.Descriptions.Add(description3);
                context.Descriptions.Add(description4);
                // dữ liệu mẫu của mô tả cho xe dạp
                var description5 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category3.Id,
                    Name = "Màu sắc",
                    Status = true
                };
                var description6 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category3.Id,
                    Name = "Nhãn hiệu",
                    Status = true
                };
                var description7 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category3.Id,
                    Name = "Mua vào năm",
                    Status = true
                };
                context.Descriptions.Add(description5);
                context.Descriptions.Add(description6);
                context.Descriptions.Add(description7);
                // dữ liệu mẫu của mô tả cho đồ điện tử
                var description8 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category.Id,
                    Name = "Loại sản phẩm cụ thể",
                    Status = true
                };
                var description9 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category.Id,
                    Name = "Nhãn hiệu",
                    Status = true
                };
                var description10 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category.Id,
                    Name = "Mua vào năm",
                    Status = true
                };
                context.Descriptions.Add(description8);
                context.Descriptions.Add(description9);
                context.Descriptions.Add(description10);
                // dữ liệu mẫu của mô tả cho đồ cổ
                var description11 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category4.Id,
                    Name = "Loại sản phẩm cụ thể",
                    Status = true
                };
                var description12 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category4.Id,
                    Name = "Nhãn hiệu(Nếu có)",
                    Status = true
                };
                var description13 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category4.Id,
                    Name = "Niên đại(đã tồn tại bao lâu)",
                    Status = true
                };
                var description14 = new Description()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = category4.Id,
                    Name = "Độ hư hại",
                    Status = true
                };
                context.Descriptions.Add(description11);
                context.Descriptions.Add(description12);
                context.Descriptions.Add(description13);
                context.Descriptions.Add(description14);
            }
            await context.SaveChangesAsync();
            // dữ liệu mẫu cho phân khúc
            if (!context.Fees.Any())
            {
                var Fee = new Fee()
                {
                    Name = "Phân khúc vừa và nhỏ",
                    Min = 1000000,
                    Max = 10000000,
                    DepositFee = 0,
                    ParticipationFee = 0.5/100,
                    Surcharge = 10/100,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = true
                };
                var Fee2 = new Fee()
                {
                    Name = "Phân khúc trung bình và cao",
                    Min = 10000000,
                    Max = 30000000,
                    DepositFee = 15/100,
                    ParticipationFee = 0.4/100,
                    Surcharge = 10/100,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = true
                };
                var Fee3 = new Fee()
                {
                    Name = "Phân khúc cao cấp",
                    Min = 30000000,
                    Max = 1000000000,
                    DepositFee = 25/100,
                    ParticipationFee = 0.5/100,
                    Surcharge = 10/100,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Status = true
                };
                context.Fees.Add(Fee);
                context.Fees.Add(Fee2);
                context.Fees.Add(Fee3);
            }
            await context.SaveChangesAsync();
        }
    }
}