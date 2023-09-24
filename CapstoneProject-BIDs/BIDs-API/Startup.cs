using BIDs_API.Mapper;
using BIDs_API.PaymentPayPal;
using BIDs_API.PaymentPayPal.Interface;
using BIDs_API.SignalR;
using Business_Logic.Modules.AdminModule;
using Business_Logic.Modules.AdminModule.Interface;
using Business_Logic.Modules.BanHistoryModule;
using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.BookingItemModule;
using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.CategoryModule;
using Business_Logic.Modules.CategoryModule.Interface;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.DescriptionModule;
using Business_Logic.Modules.DescriptionModule.Interface;
using Business_Logic.Modules.FeeModule;
using Business_Logic.Modules.FeeModule.Interface;
using Business_Logic.Modules.ImageModule;
using Business_Logic.Modules.ImageModule.Interface;
using Business_Logic.Modules.ItemDescriptionModule;
using Business_Logic.Modules.ItemDescriptionModule.Interface;
using Business_Logic.Modules.ItemModule;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.LoginModule;
using Business_Logic.Modules.LoginModule.InterFace;
using Business_Logic.Modules.NotificationModule;
using Business_Logic.Modules.NotificationModule.Interface;
using Business_Logic.Modules.NotificationTypeModule;
using Business_Logic.Modules.NotificationTypeModule.Interface;
using Business_Logic.Modules.PaymentStaffModule;
using Business_Logic.Modules.PaymentStaffModule.Interface;
using Business_Logic.Modules.PaymentUserModule;
using Business_Logic.Modules.PaymentUserModule.Interface;
using Business_Logic.Modules.SessionDetailModule;
using Business_Logic.Modules.SessionDetailModule.Interface;
using Business_Logic.Modules.SessionModule;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionRuleModule;
using Business_Logic.Modules.SessionRuleModule.Interface;
using Business_Logic.Modules.StaffModule;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.StaffNotificationDetailModule;
using Business_Logic.Modules.StaffNotificationDetailModule.Interface;
using Business_Logic.Modules.UserModule;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserNotificationDetailModule;
using Business_Logic.Modules.UserNotificationDetailModule.Interface;
using Business_Logic.Modules.UserPaymentInformationModule;
using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using Data_Access.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

namespace BIDs_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                //options.AddPolicy("AllowReact", builder =>
                //{
                //    builder.AllowAnyOrigin()
                //        .AllowAnyMethod()
                //        .AllowAnyHeader()
                //        .SetIsOriginAllowed((host) => true)
                //        .WithExposedHeaders("Authorization");
                //});
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed((host) => true)
                        .WithOrigins("http://localhost:3000",
"https://localhost:3000",
"https://capstone-bid-h70yuvqdd-doannguyenquochuy13-gmailcom.vercel.app",
"https://capstone-bid-fe.vercel.app");
                });
            });
            services.AddSignalR();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BIDs", Version = "v1" });

                // hiển thị khung authorize điền token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \\n\\n
                      Enter your token in the text input below.
                      \\n\\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                { 
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.CustomSchemaIds(type => type.FullName);
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            services.AddDbContext<BIDsContext>(
                opt => opt.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(otp =>
                {
                    otp.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization();

            //Admin Module
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminService, AdminService>();
            //Staff Module
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IStaffService, StaffService>();
            //User Module
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            //Fee Module
            services.AddScoped<IFeeRepository, FeeRepository>();
            services.AddScoped<IFeeService, FeeService>();
            //Ban History Module
            services.AddScoped<IBanHistoryRepository, BanHistoryRepository>();
            services.AddScoped<IBanHistoryService, BanHistoryService>();
            //Category Module
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            //Session Module
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ISessionService, SessionService>();
            //Item Module
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemService, ItemService>();
            //Description Module
            services.AddScoped<IDescriptionRepository, DescriptionRepository>();
            services.AddScoped<IDescriptionService, DescriptionService>();
            //Session Detail Module
            services.AddScoped<ISessionDetailRepository, SessionDetailRepository>();
            services.AddScoped<ISessionDetailService, SessionDetailService>();
            //Booking Item Module
            services.AddScoped<IBookingItemRepository, BookingItemRepository>();
            services.AddScoped<IBookingItemService, BookingItemService>();
            //Item Description Module
            services.AddScoped<IItemDescriptionRepository, ItemDescriptionRepository>();
            services.AddScoped<IItemDescriptionService, ItemDescriptionService>();
            //Session Rule Module
            services.AddScoped<ISessionRuleRepository, SessionRuleRepository>();
            services.AddScoped<ISessionRuleService, SessionRuleService>();
            //Notification Module
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            //Notification Type Module
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            services.AddScoped<INotificationTypeService, NotificationTypeService>();
            //Staff Notification Detail Module
            services.AddScoped<IStaffNotificationDetailRepository, StaffNotificationDetailRepository>();
            services.AddScoped<IStaffNotificationDetailService, StaffNotificationDetailService>();
            //User Notification Detail Module
            services.AddScoped<IUserNotificationDetailRepository, UserNotificationDetailRepository>();
            services.AddScoped<IUserNotificationDetailService, UserNotificationDetailService>();
            //Payment User Module
            services.AddScoped<IPaymentUserRepository, PaymentUserRepository>();
            services.AddScoped<IPaymentUserService, PaymentUserService>();
            //Payment Staff Module
            services.AddScoped<IPaymentStaffRepository, PaymentStaffRepository>();
            services.AddScoped<IPaymentStaffService, PaymentStaffService>();
            //Payment User Information Module
            services.AddScoped<IUserPaymentInformationRepository, UserPaymentInformationRepository>();
            services.AddScoped<IUserPaymentInformationService, UserPaymentInformationService>();
            //Image Module
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IImageService, ImageService>();
            //Login Module
            services.AddScoped<ILoginService, LoginService>();
            //Send Email Module
            services.AddScoped<ICommon, Business_Logic.Modules.CommonModule.Common>();
            //Paypal Module
            services.AddScoped<IPayPalPayment, PayPalPayment>();

            services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();
            services.AddAutoMapper(typeof(Mapping));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BIDs v1"));
            }

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            //app.UseCors("AllowReact");

            app.UseCors("CorsPolicy");
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<UserHub>("/userhub");
                endpoints.MapHub<AdminHub>("/adminhub");
                endpoints.MapHub<StaffHub>("/staffhub");
                endpoints.MapHub<SessionHub>("/sessionhub");
                endpoints.MapHub<FeeHub>("/feehub");
                endpoints.MapHub<SessionDetailHub>("/sessiondetailhub");
                endpoints.MapHub<DescriptionHub>("/descriptionhub");
                endpoints.MapHub<ItemHub>("/itemhub");
                endpoints.MapHub<CategoryHub>("/categoryhub");
                endpoints.MapHub<BanHistoryHub>("/banhistoryhub");
                endpoints.MapHub<BookingItemHub>("/bookingitemhub");
                endpoints.MapHub<ItemDescriptionHub>("/itemdescriptionhub");
                endpoints.MapHub<SessionRuleHub>("/sessionrulehub");
                endpoints.MapHub<NotificationHub>("/notificationhub");
                endpoints.MapHub<StaffNotificationDetailHub>("/staffnotificationdetailhub");
                endpoints.MapHub<NotificationTypeHub>("/notificationtypehub");
                endpoints.MapHub<UserNotificationDetailHub>("/usernotificationdetailhub");
                endpoints.MapHub<PaymentStaffHub>("/paymentstaffhub");
                endpoints.MapHub<PaymentUserHub>("/paymentuserhub");
                endpoints.MapHub<UserPaymentInformationHub>("/userpaymentinformationhub");
            });
        }

        public class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var authAttributes = context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>();
                if (authAttributes.Any())
                {
                    var authHeaderParameter = operation.Parameters.SingleOrDefault(p => p.In == ParameterLocation.Header && p.Name == "Authorization");
                    if (authHeaderParameter != null)
                    {
                        authHeaderParameter.Description = authHeaderParameter.Description.Replace("Bearer ", "");
                    }
                }
            }
        }
    }
}

