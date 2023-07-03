using Data_Access.Enum;
using Microsoft.Extensions.Logging;

namespace Data_Access.Entities
{
    public class BIDsContextSeed
    {
        //public Guid adminIDseed = Guid.NewGuid();
        //public Guid staffIDseed = Guid.NewGuid();
        //public Guid userWaittingIDseed = Guid.NewGuid();
        //public Guid userActiveIDseed = Guid.NewGuid();
        //public Guid userActive2IDseed = Guid.NewGuid();
        //public Guid userDenyIDseed = Guid.NewGuid();
        //public Guid userBanIDseed = Guid.NewGuid();
        //public Guid banIDseed = Guid.NewGuid();
        //public Guid categoryTechIDseed = Guid.NewGuid();
        //public Guid categoryBikeIDseed = Guid.NewGuid();
        //public Guid categoryMotoIDseed = Guid.NewGuid();
        //public Guid categoryOldIDseed = Guid.NewGuid();
        //public Guid descriptionTech1IDseed = Guid.NewGuid();
        //public Guid descriptionTech2IDseed = Guid.NewGuid();
        //public Guid descriptionTech3IDseed = Guid.NewGuid();
        //public Guid descriptionTech4IDseed = Guid.NewGuid();
        //public Guid descriptionBike1IDseed = Guid.NewGuid();
        //public Guid descriptionBike2IDseed = Guid.NewGuid();
        //public Guid descriptionBike3IDseed = Guid.NewGuid();
        //public Guid descriptionMoto1IDseed = Guid.NewGuid();
        //public Guid descriptionMoto2IDseed = Guid.NewGuid();
        //public Guid descriptionMoto3IDseed = Guid.NewGuid();
        //public Guid descriptionMoto4IDseed = Guid.NewGuid();
        //public Guid descriptionOld1IDseed = Guid.NewGuid();
        //public Guid descriptionOld2IDseed = Guid.NewGuid();
        //public Guid descriptionOld3IDseed = Guid.NewGuid();
        //public Guid descriptionOld4IDseed = Guid.NewGuid();
        //public Guid item1IDseed = Guid.NewGuid();
        //public Guid item2IDseed = Guid.NewGuid();
        //public Guid itemdescriptionTech1IDseed = Guid.NewGuid();
        //public Guid itemdescriptionTech2IDseed = Guid.NewGuid();
        //public Guid itemdescriptionTech3IDseed = Guid.NewGuid();
        //public Guid itemdescriptionTech4IDseed = Guid.NewGuid();
        //public Guid itemdescriptionMoto1IDseed = Guid.NewGuid();
        //public Guid itemdescriptionMoto2IDseed = Guid.NewGuid();
        //public Guid itemdescriptionMoto3IDseed = Guid.NewGuid();
        //public Guid itemdescriptionMoto4IDseed = Guid.NewGuid();

        public async Task SeedAsync(BIDsContext context, ILogger<BIDsContextSeed> logger)
        {
            //    // dữ liệu mẫu cho nhân viên
            //    if (!context.Staff.Any())
            //    {
            //        var Staff = new Staff()
            //        {
            //            Id = staffIDseed,
            //            Address = "115/4/2 đường số 11 thành phố Thủ Đức",
            //            CreateDate = DateTime.Now,
            //            DateOfBirth = new DateTime(2001, 07, 05),
            //            UpdateDate = DateTime.Now,
            //            Email = "seedstaff@gmail.com",
            //            Password = "05072001",
            //            Phone = "0933403842",
            //            Name = "Seed Staff",
            //            Status = true
            //        };
            //        context.Staff.Add(Staff);
            //    }
            //    await context.SaveChangesAsync();
            //    // dữ liệu mẫu cho quản trị viên
            //    if (!context.Admins.Any())
            //    {
            //        var Admin = new Admin()
            //        {
            //            Id = adminIDseed,
            //            Address = "115/4/2 đường số 11 thành phố Thủ Đức",
            //            Email = "seedadmin@gmail.com",
            //            Password = "05072001",
            //            Phone = "0933403842",
            //            Name = "Seed Admin",
            //            Status = true
            //        };
            //        context.Admins.Add(Admin);
            //    }
            //    await context.SaveChangesAsync();
            //    // dữ liệu mẫu cho loại sản phẩm
            //    if (!context.Categories.Any())
            //    {
            //        var category = new Category()
            //        {
            //            Id = categoryTechIDseed,
            //            Name = "Đồ công nghệ",
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = true
            //        };
            //        var category2 = new Category()
            //        {
            //            Id = categoryMotoIDseed,
            //            Name = "Xe máy",
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = true
            //        };
            //        var category3 = new Category()
            //        {
            //            Id = categoryBikeIDseed,
            //            Name = "Xe đạp",
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = true
            //        };
            //        var category4 = new Category()
            //        {
            //            Id = categoryOldIDseed,
            //            Name = "Đồ cổ",
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = false
            //        };
            //        context.Categories.Add(category);
            //        context.Categories.Add(category2);
            //        context.Categories.Add(category3);
            //        context.Categories.Add(category4);

            //        // dữ liệu mẫu của mô tả cho xe máy
            //        if (!context.Descriptions.Any())
            //        {
            //            var description = new Description()
            //            {
            //                Id = descriptionMoto1IDseed,
            //                CategoryId = categoryMotoIDseed,
            //                Name = "Màu sắc",
            //                Status = true
            //            };
            //            var description2 = new Description()
            //            {
            //                Id = descriptionMoto2IDseed,
            //                CategoryId = categoryMotoIDseed,
            //                Name = "Biển số",
            //                Status = true
            //            };
            //            var description3 = new Description()
            //            {
            //                Id = descriptionMoto3IDseed,
            //                CategoryId = categoryMotoIDseed,
            //                Name = "Hãng xe",
            //                Status = true
            //            };
            //            var description4 = new Description()
            //            {
            //                Id = descriptionMoto4IDseed,
            //                CategoryId = categoryMotoIDseed,
            //                Name = "Mua vào năm",
            //                Status = true
            //            };

            //            // dữ liệu mẫu của mô tả cho xe dạp
            //            var description5 = new Description()
            //            {
            //                Id = descriptionBike1IDseed,
            //                CategoryId = categoryBikeIDseed,
            //                Name = "Màu sắc",
            //                Status = true
            //            };
            //            var description6 = new Description()
            //            {
            //                Id = descriptionBike2IDseed,
            //                CategoryId = categoryBikeIDseed,
            //                Name = "Nhãn hiệu",
            //                Status = true
            //            };
            //            var description7 = new Description()
            //            {
            //                Id = descriptionBike3IDseed,
            //                CategoryId = categoryBikeIDseed,
            //                Name = "Mua vào năm",
            //                Status = true
            //            };
            //            // dữ liệu mẫu của mô tả cho đồ điện tử
            //            var description8 = new Description()
            //            {
            //                Id = descriptionTech1IDseed,
            //                CategoryId = categoryTechIDseed,
            //                Name = "Loại sản phẩm cụ thể",
            //                Status = true
            //            };
            //            var description9 = new Description()
            //            {
            //                Id = descriptionTech2IDseed,
            //                CategoryId = categoryTechIDseed,
            //                Name = "Nhãn hiệu",
            //                Status = true
            //            };
            //            var description10 = new Description()
            //            {
            //                Id = descriptionTech3IDseed,
            //                CategoryId = categoryTechIDseed,
            //                Name = "Màu sắc",
            //                Status = true
            //            };
            //            var description11 = new Description()
            //            {
            //                Id = descriptionTech4IDseed,
            //                CategoryId = categoryTechIDseed,
            //                Name = "Mua vào năm",
            //                Status = true
            //            };

            //            // dữ liệu mẫu của mô tả cho đồ cổ
            //            var description12 = new Description()
            //            {
            //                Id = descriptionOld1IDseed,
            //                CategoryId = categoryOldIDseed,
            //                Name = "Loại sản phẩm cụ thể",
            //                Status = true
            //            };
            //            var description13 = new Description()
            //            {
            //                Id = descriptionOld2IDseed,
            //                CategoryId = categoryOldIDseed,
            //                Name = "Nhãn hiệu(Nếu có)",
            //                Status = true
            //            };
            //            var description14 = new Description()
            //            {
            //                Id = descriptionOld3IDseed,
            //                CategoryId = categoryOldIDseed,
            //                Name = "Niên đại(đã tồn tại bao lâu)",
            //                Status = true
            //            };
            //            var description15 = new Description()
            //            {
            //                Id = descriptionOld4IDseed,
            //                CategoryId = categoryOldIDseed,
            //                Name = "Độ hư hại",
            //                Status = true
            //            };
            //            context.Descriptions.Add(description);
            //            context.Descriptions.Add(description2);
            //            context.Descriptions.Add(description3);
            //            context.Descriptions.Add(description4);
            //            context.Descriptions.Add(description5);
            //            context.Descriptions.Add(description6);
            //            context.Descriptions.Add(description7);
            //            context.Descriptions.Add(description8);
            //            context.Descriptions.Add(description9);
            //            context.Descriptions.Add(description10);
            //            context.Descriptions.Add(description11);
            //            context.Descriptions.Add(description12);
            //            context.Descriptions.Add(description13);
            //            context.Descriptions.Add(description14);
            //            context.Descriptions.Add(description15);
            //            await context.SaveChangesAsync();
            //        }

            //    }

            //    // dữ liệu mẫu cho người dùng
            //    if (!context.Users.Any())
            //    {
            //        var user1 = new Users()
            //        {
            //            Id = userWaittingIDseed,
            //            Name = "User seed 1",
            //            Role = (int)RoleEnum.Bidder,
            //            Address = "115/4/2 đường số 11 thành phố Thủ Đức",
            //            CreateDate = DateTime.Now,
            //            DateOfBirth = new DateTime(2003, 07, 07),
            //            UpdateDate = DateTime.Now,
            //            Avatar = "Avater mẫu",
            //            Email = "minhnhut05072003@gmail.com",
            //            Cccdnumber = "077201000702",
            //            CccdbackImage = "CCCD mặt sau mẫu",
            //            CccdfrontImage = "CCCD mặt trước mẫu",
            //            Password = "07072001",
            //            Phone = "0933403844",
            //            Status = (int)UserStatusEnum.Waitting,

            //        };
            //        var user2 = new Users()
            //        {
            //            Id = userActiveIDseed,
            //            Name = "User seed 2",
            //            Role = (int)RoleEnum.Auctioneer,
            //            Address = "115/4/2 đường số 11 thành phố Thủ Đức",
            //            CreateDate = DateTime.Now,
            //            DateOfBirth = new DateTime(2003, 07, 07),
            //            UpdateDate = DateTime.Now,
            //            Avatar = "Avater mẫu",
            //            Email = "minhnhut05072001@gmail.com",
            //            Cccdnumber = "077201000702",
            //            CccdbackImage = "CCCD mặt sau mẫu",
            //            CccdfrontImage = "CCCD mặt trước mẫu",
            //            Password = "07072001",
            //            Phone = "0933403842",
            //            Status = (int)UserStatusEnum.Acctive,


            //        };
            //        var user3 = new Users()
            //        {
            //            Id = userDenyIDseed,
            //            Name = "User seed 3",
            //            Role = (int)RoleEnum.Bidder,
            //            Address = "115/4/2 đường số 11 thành phố Thủ Đức",
            //            CreateDate = DateTime.Now,
            //            DateOfBirth = new DateTime(2003, 07, 07),
            //            UpdateDate = DateTime.Now,
            //            Avatar = "Avater mẫu",
            //            Email = "minhnhut05072004@gmail.com",
            //            Cccdnumber = "077201000702",
            //            CccdbackImage = "CCCD mặt sau mẫu",
            //            CccdfrontImage = "CCCD mặt trước mẫu",
            //            Password = "07072001",
            //            Phone = "0933403844",
            //            Status = (int)UserStatusEnum.Deny

            //        };
            //        var user4 = new Users()
            //        {
            //            Id = userActive2IDseed,
            //            Name = "User seed 4",
            //            Role = (int)RoleEnum.Auctioneer,
            //            Address = "115/4/2 đường số 11 thành phố Thủ Đức",
            //            CreateDate = DateTime.Now,
            //            DateOfBirth = new DateTime(2003, 07, 07),
            //            UpdateDate = DateTime.Now,
            //            Avatar = "Avater mẫu",
            //            Email = "nhutdmse151298@fpt.edu.vn",
            //            Cccdnumber = "077201000702",
            //            CccdbackImage = "CCCD mặt sau mẫu",
            //            CccdfrontImage = "CCCD mặt trước mẫu",
            //            Password = "07072001",
            //            Phone = "0933403844",
            //            Status = (int)UserStatusEnum.Acctive

            //        };
            //        var user5 = new Users()
            //        {
            //            Id = userBanIDseed,
            //            Name = "User seed 5",
            //            Role = (int)RoleEnum.Auctioneer,
            //            Address = "115/4/2 đường số 11 thành phố Thủ Đức",
            //            CreateDate = DateTime.Now,
            //            DateOfBirth = new DateTime(2003, 07, 07),
            //            UpdateDate = DateTime.Now,
            //            Avatar = "Avater mẫu",
            //            Email = "nhutdmse1512910@fpt.edu.vn",
            //            Cccdnumber = "077201000702",
            //            CccdbackImage = "CCCD mặt sau mẫu",
            //            CccdfrontImage = "CCCD mặt trước mẫu",
            //            Password = "07072001",
            //            Phone = "0933403844",
            //            Status = (int)UserStatusEnum.Ban

            //        };
            //        context.Users.Add(user1);
            //        context.Users.Add(user2);
            //        context.Users.Add(user3);
            //        context.Users.Add(user4);
            //        context.Users.Add(user5);
            //        await context.SaveChangesAsync();
            //    }
            //    // dữ liệu mẫu cho khóa tài khoản
            //    if (!context.BanHistories.Any())
            //    {
            //        var ban = new BanHistory()
            //        {
            //            Id = banIDseed,
            //            Reason = "Dữ liệu khóa tài khoản mẫu",
            //            UserId = userBanIDseed,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = true
            //        };
            //        context.BanHistories.Add(ban);
            //        await context.SaveChangesAsync();
            //    }
            //    // dữ liệu mẫu cho sản phẩm
            //    if (!context.Items.Any())
            //    {
            //        var Item1 = new Item()
            //        {
            //            Id = item1IDseed,
            //            Name = "Laptop ASUS",
            //            DescriptionDetail = "NVIDIA 930, intel core i7 8th Gen",
            //            Image = "Image mẫu",
            //            Quantity = 1,
            //            FristPrice = 25000000,
            //            StepPrice = 1000000,
            //            Deposit = false,
            //            CategoryId = categoryTechIDseed,
            //            UserId = userActiveIDseed,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //        };
            //        var Item2 = new Item()
            //        {
            //            Id = item2IDseed,
            //            Name = "XSR 155R",
            //            DescriptionDetail = "Xe máy xsr 155r của yamaha",
            //            Image = "Image mẫu",
            //            Quantity = 1,
            //            FristPrice = 50000000,
            //            StepPrice = 2000000,
            //            Deposit = false,
            //            CategoryId = categoryMotoIDseed,
            //            UserId = userActive2IDseed,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //        };
            //        context.Items.Add(Item1);
            //        context.Items.Add(Item2);
            //        await context.SaveChangesAsync();
            //    }
            //    // dữ liệu mẫu cho sản phẩm mô tả
            //    if (!context.ItemDescriptions.Any())
            //    {
            //        var ItemDescription1 = new ItemDescription()
            //        {
            //            Id = itemdescriptionTech1IDseed,
            //            DescriptionId = descriptionTech1IDseed,
            //            ItemId = item1IDseed,
            //            Detail = "Laptop",
            //            Status = true
            //        };
            //        var ItemDescription2 = new ItemDescription()
            //        {
            //            Id = itemdescriptionTech2IDseed,
            //            DescriptionId = descriptionTech2IDseed,
            //            ItemId = item1IDseed,
            //            Detail = "Asus",
            //            Status = true
            //        };
            //        var ItemDescription3 = new ItemDescription()
            //        {
            //            Id = itemdescriptionTech3IDseed,
            //            DescriptionId = descriptionTech3IDseed,
            //            ItemId = item1IDseed,
            //            Detail = "Đen nhám",
            //            Status = true
            //        };
            //        var ItemDescription4 = new ItemDescription()
            //        {
            //            Id = itemdescriptionTech4IDseed,
            //            DescriptionId = descriptionTech4IDseed,
            //            ItemId = item1IDseed,
            //            Detail = "2018",
            //            Status = true
            //        };

            //        var ItemDescription5 = new ItemDescription()
            //        {
            //            Id = itemdescriptionMoto1IDseed,
            //            DescriptionId = descriptionMoto1IDseed,
            //            ItemId = item2IDseed,
            //            Detail = "Bạc và nâu",
            //            Status = true
            //        };
            //        var ItemDescription6 = new ItemDescription()
            //        {
            //            Id = itemdescriptionMoto2IDseed,
            //            DescriptionId = descriptionMoto2IDseed,
            //            ItemId = item2IDseed,
            //            Detail = "72K109144",
            //            Status = true
            //        };
            //        var ItemDescription7 = new ItemDescription()
            //        {
            //            Id = itemdescriptionMoto3IDseed,
            //            DescriptionId = descriptionMoto3IDseed,
            //            ItemId = item2IDseed,
            //            Detail = "Yamaha",
            //            Status = true
            //        };
            //        var ItemDescription8 = new ItemDescription()
            //        {
            //            Id = itemdescriptionMoto4IDseed,
            //            DescriptionId = descriptionMoto4IDseed,
            //            ItemId = item2IDseed,
            //            Detail = "2023",
            //            Status = true
            //        };
            //        context.ItemDescriptions.Add(ItemDescription1);
            //        context.ItemDescriptions.Add(ItemDescription2);
            //        context.ItemDescriptions.Add(ItemDescription3);
            //        context.ItemDescriptions.Add(ItemDescription4);
            //        context.ItemDescriptions.Add(ItemDescription5);
            //        context.ItemDescriptions.Add(ItemDescription6);
            //        context.ItemDescriptions.Add(ItemDescription7);
            //        context.ItemDescriptions.Add(ItemDescription8);
            //        await context.SaveChangesAsync();
            //    }
            //    if (!context.BookingItems.Any())
            //    {
            //        var bookingItem = new BookingItem()
            //        {
            //            Id = Guid.NewGuid(),
            //            ItemId = item1IDseed,
            //            StaffId = staffIDseed,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = (int)BookingItemEnum.Watting
            //        };
            //        var bookingItem2 = new BookingItem()
            //        {
            //            Id = Guid.NewGuid(),
            //            ItemId = item2IDseed,
            //            StaffId = staffIDseed,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = (int)BookingItemEnum.Watting
            //        };
            //        context.BookingItems.Add(bookingItem);
            //        context.BookingItems.Add(bookingItem2);
            //        await context.SaveChangesAsync();
            //    }


            //    // dữ liệu mẫu cho phân khúc
            //    if (!context.Fees.Any())
            //    {
            //        var Fee = new Fee()
            //        {
            //            Name = "Phân khúc vừa và nhỏ",
            //            Min = 1000000,
            //            Max = 10000000,
            //            DepositFee = 0,
            //            ParticipationFee = 0.005,
            //            Surcharge = 0.1,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = true
            //        };
            //        var Fee2 = new Fee()
            //        {
            //            Name = "Phân khúc trung bình và cao",
            //            Min = 10000000,
            //            Max = 30000000,
            //            DepositFee = 0.15,
            //            ParticipationFee = 0.004,
            //            Surcharge = 0.1,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = true
            //        };
            //        var Fee3 = new Fee()
            //        {
            //            Name = "Phân khúc cao cấp",
            //            Min = 30000000,
            //            Max = 1000000000,
            //            DepositFee = 0.25,
            //            ParticipationFee = 0.003,
            //            Surcharge = 0.1,
            //            CreateDate = DateTime.Now,
            //            UpdateDate = DateTime.Now,
            //            Status = true
            //        };
            //        context.Fees.Add(Fee);
            //        context.Fees.Add(Fee2);
            //        context.Fees.Add(Fee3);
            //        await context.SaveChangesAsync();
            //    }
        }
    }
}