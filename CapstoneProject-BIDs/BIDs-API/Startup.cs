using BIDs_API.Mapper;
using BIDs_API.SignalR;
using Business_Logic.Modules.AdminModule;
using Business_Logic.Modules.AdminModule.Interface;
using Business_Logic.Modules.BanHistoryModule;
using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.CategoryModule;
using Business_Logic.Modules.CategoryModule.Interface;
using Business_Logic.Modules.DescriptionModule;
using Business_Logic.Modules.DescriptionModule.Interface;
using Business_Logic.Modules.FeeModule;
using Business_Logic.Modules.FeeModule.Interface;
using Business_Logic.Modules.ItemModule;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.LoginModule;
using Business_Logic.Modules.LoginModule.InterFace;
using Business_Logic.Modules.SessionDetailModule;
using Business_Logic.Modules.SessionDetailModule.Interface;
using Business_Logic.Modules.SessionModule;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.StaffModule;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule;
using Business_Logic.Modules.UserModule.Interface;
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
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
            }));
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
                    Type = SecuritySchemeType.Http,
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
            

            //services.AddIdentity<Staff, Role>()
            //    .AddEntityFrameworkStores<BIDsContext>()
            //    .AddDefaultTokenProviders(); ;
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
            //Login Module
            services.AddScoped<ILoginService, LoginService>();

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

