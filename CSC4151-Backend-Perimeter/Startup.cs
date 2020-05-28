using System;
using CSC4151_Backend_Perimeter.Middleware;
using CSC4151_Backend_Perimeter.Queues;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CSC4151_Backend_Perimeter
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
            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => Configuration.Bind("Auth0", options));

            string endpointName;
#if DEBUG
            endpointName = $"Tak.ProfileService.{Environment.MachineName}";
#else
            endpointName = "Tak.ProfileService";
#endif

            string taskEnpoint;
#if DEBUG
            taskEnpoint = $"Tak.TaskService.{Environment.MachineName}";
#else
            taskEnpoint = "Tak.TaskService";
#endif

            string choreEndpoint;
#if DEBUG
            choreEndpoint = $"Tak.ChoreService.{Environment.MachineName}";
#else
            choreEndpoint = "Tak.ChoreService";
#endif

            services.AddSingleton <ITaskQueueClient>(new APIiQueueClient(Configuration["ServiceBus"], taskEnpoint));
            services.AddSingleton<IChoreQueueClient>(new APIiQueueClient(Configuration["ServiceBus"], choreEndpoint));
            services.AddSingleton<IProfileQueueClient>(new APIiQueueClient(Configuration["ServiceBus"], endpointName));

            services.AddHttpClient();

            services.AddControllers();

            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication().UseMiddleware<IdentityMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}