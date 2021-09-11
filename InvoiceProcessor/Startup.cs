using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using InvoiceProcessor.Application.Commands.CreateInvoiceRequest;
using InvoiceProcessor.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Communication;
using Communication.Consumer;
using Communication.Extensions;
using InvoiceProcessor.Infrastructure.Outbox.Services;
using InvoiceProcessor.Infrastructure.Outbox.Storage;
using Outbox = InvoiceProcessor.Application.Commands.Outbox.Outbox;
using Communication.Messages;
using Communication.Sender;
using Integration;
using Integration.Likvido;
using InvoiceProcessor.Application.Commands.Outbox.Events;
using InvoiceProcessor.Application.Factories;
using InvoiceProcessor.Application.Scheduler.Jobs;
using InvoiceProcessor.Domain.Interfaces.GetProcessStatus;
using InvoiceProcessor.Domain.Interfaces.Outbox;
using InvoiceProcessor.Infrastructure.Repositories.BaseRepositories;
using InvoiceProcessor.Infrastructure.Repositories.FeatureRepositories;
using InvoiceProcessor.Infrastructure.Scheduler;
using InvoiceProcessor.Messages;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace InvoiceProcessor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InvoiceProcessor", Version = "v1" });
            });
            services.AddDbContext<InvoiceDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("InvoiceContext")));

            services.AddLogging();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IEntityRepository<,>), typeof(EntityRepository<,>));

            services.AddTransient<IOutboxStorage, OutBoxStorage>();
            services.AddTransient<IOutBoxService, OutBoxService>();
            services.AddTransient<IOutBoxFactory, OutBoxFactory>();

            services.AddTransient<IProcessStatusRepository, ProcessStatusRepository>();
            
            services.AddTransient<IRequestHandler<Outbox.Command<IOutBoxEvent>>, Outbox.CommandHandler>();

            services.AddMediatR(typeof(InvoiceRequestCreated).GetTypeInfo().Assembly);

            var servicebusConfiguration = new ServiceBusConfiguration();
            Configuration.GetSection("Servicebus").Bind(servicebusConfiguration);
            services.AddSingleton(servicebusConfiguration);

            services.AddSingleton<IDistributedConsumer, DistributedConsumer>();
            services.AddTransient<IDistributedSender, DistributedSender>();

            var likvidoClientConfiguration = new LikvidoClientConfiguration();
            Configuration.GetSection("Likvido").Bind(likvidoClientConfiguration);
            services.AddSingleton(likvidoClientConfiguration);

            services.AddHttpClient<ILikvidoClient, LikvidoClient>(c =>
            {
                c.BaseAddress = new Uri(likvidoClientConfiguration.BaseAddress);
                c.DefaultRequestHeaders.Add("X-ApiKey",likvidoClientConfiguration.ApiKey );
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // Add Quartz services
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<ProcessPendingInvoiceJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(ProcessPendingInvoiceJob),
                cronExpression: Configuration.GetSection("ProcessPendingInvoiceCorn").Value));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ServiceBusConfiguration busConfig, InvoiceDbContext dbContext)
        {
            dbContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InvoiceProcessor v1"));
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            var distributedCommands = typeof(SendInvoiceCommand).Assembly.GetTypes()
                .Where(p => p.GetInterfaces().Contains(typeof(IDistributedCommand))).ToList();

            app.RegisterServiceBus(serviceProvider, distributedCommands, typeof(SendInvoiceCommand).Assembly, busConfig.PrimaryConnectionString);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
