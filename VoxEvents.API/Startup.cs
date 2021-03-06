﻿using System;
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
using VoxEvents.API.Models;
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
                cfg.CreateMap<Member, MemberNoAvailabilitiesDto>();
                cfg.CreateMap<Member, MemberDto>();
                cfg.CreateMap<MemberCreateDto, Member>();
                cfg.CreateMap<Member, MemberUpdateDto>().ReverseMap();
                cfg.CreateMap<VoxEventCreateDto, VoxEvent>();
                cfg.CreateMap<VenueCreateDto, Venue>();
                cfg.CreateMap<Availability, MemberAvailabilityDto>();
                cfg.CreateMap<Availability, MemberAvailabilityUpdateDto>().ReverseMap();
                cfg.CreateMap<Availability, VoxEventAvailabilityDto>();
                cfg.CreateMap<VoxEvent, VoxEventNoAvailabilitiesDto>();
                cfg.CreateMap<VoxEvent, VoxEventDto>();
                cfg.CreateMap<VoxEvent, VoxEventUpdateDto>().ReverseMap();
                cfg.CreateMap<Venue, VenueNoEventsDto>();
                cfg.CreateMap<Venue, VenueUpdateDto>().ReverseMap();
                cfg.CreateMap<Venue, VenueDto>();
                cfg.CreateMap<MemberAvailabilityCreateDto, Availability>();
            });
        }
    }
}
