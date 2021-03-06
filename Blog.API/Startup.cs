using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackgroundTasksSample.Services;
using Blog.API.Notifications;
using Blog.API.Services;
using Blog.API.Services.Abstraction;
using Blog.API.ViewModels.Fillers;
using Blog.API.ViewModels.Mapping;
using Blog.Data;
using Blog.Data.Abstract;
using Blog.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Blog.API
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
            services.AddCors();

            services
                .AddDbContext<BlogContext>(options =>
                    options.UseMySql(
                        Configuration.GetConnectionString("BlogContext"),
                        o => o.MigrationsAssembly("Blog.API")
                    )
                );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWTSecretKey"))
                        )
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notifications")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IShareRepository, ShareRepository>();

            services.AddSingleton<IAuthService>(
                new AuthService(
                    Configuration.GetValue<string>("JWTSecretKey"),
                    Configuration.GetValue<int>("JWTLifespan")
                )
            );

            services.AddControllers();
            services.AddSignalR().AddJsonProtocol();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();


            services.AddSingleton<TimedService>();
            #region snippet1

            if (Configuration.GetValue<bool>("EnableTimedService"))
            {
                //有这个标志才会开启系统的后台服务
                //可以在mysql 服务器上驻存这个服务，专门来取数据，减小主服务器的网络带宽压力。
                //然后在其他服务器上关闭这个服务，减小资源占用。
                //需要副作用wrapper 来开启timed service中的定时器
                services.AddHostedService<WrappeHostedService>();
            }
            #endregion



            #region snippet3
            //提供后台队列服务
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            #endregion

            int threadNum = Configuration.GetValue<int>("MaxThreadNum");
            BaseDoWorkViewModel.MaxThreadNum = threadNum;


            services.AddSingleton<BackServiceUtil>();
            services.AddScoped<PullAllStockNamesViewModel>();
            services.AddScoped<F10FHPGFillerViewModel>();
            services.AddScoped<DayDataFillerViewModel>();
            services.AddScoped<RealTimeDataFillerViewModel>();

        }


        private void printSystemInfo()
        {
            var temp2 = Assembly.GetEntryAssembly();
            string ret = temp2.GetName().Version.ToString();
            System.Console.WriteLine($"Service version is {ret}");
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            IConfigurationSection myArraySection = Configuration.GetSection("corsOrigins");
            var corsOrigins = (from i in myArraySection.AsEnumerable()
                               where i.Value != null
                               select i.Value).ToArray();

            foreach (var item in corsOrigins)
                System.Console.WriteLine("allow cors have " + item);
            printSystemInfo();

            app.UseCors(builder => builder
                .WithOrigins(corsOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
            // app.UseHttpsRedirection();

            Utils.DbInitializer.MigrateLatest(app);
            Utils.DbInitializer.Seed(app);
            //不要所有的程序都对这flag进行操作
            //Utils.DbInitializer.CheckIfClearFlags(app);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationsHub>("/notifications");
                endpoints.MapControllers();
            });
        }
    }
}
