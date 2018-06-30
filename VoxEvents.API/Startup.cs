using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using VoxEvents.API.Entities;
using VoxEvents.API.Services;

namespace VoxEvents.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));

            var connectionString = Startup.Configuration["connectionStrings:voxEventsConnectionString"];
            services.AddDbContext<VoxEventsContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IVoxEventsRepository, VoxEventsRepository>();

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            VoxEventsContext voxEventsContext)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            voxEventsContext.EnsureSeedDataForContext();

            app.UseMvc();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Member, Models.MemberNoAvailabilitiesDto>();
                cfg.CreateMap<Entities.Member, Models.MemberDto>();
                cfg.CreateMap<Entities.Availability, Models.MemberAvailabilityDto>();
                cfg.CreateMap<Entities.Availability, Models.VoxEventAvailabilityDto>();
            });
        }
    }
}
