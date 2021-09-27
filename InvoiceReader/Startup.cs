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
using Communication;
using Communication.Consumer;
using Communication.Extensions;
using Communication.Messages;
using Communication.Sender;
using Integration;
using Integration.Likvido;
using InvoiceReader.Application.Infrastructure;
using InvoiceReader.Application.Queries.GetInvoices;
using InvoiceReader.Messages;
using MediatR;
using Serilog;

namespace InvoiceReader
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InvoiceReader", Version = "v1" });
            });

            var likvidoClientConfiguration = new LikvidoClientConfiguration();
            Configuration.GetSection("Likvido").Bind(likvidoClientConfiguration);
            services.AddSingleton(likvidoClientConfiguration);

            services.AddHttpClient<ILikvidoClient, LikvidoClient>(c =>
            {
                c.BaseAddress = new Uri(likvidoClientConfiguration.BaseAddress);
                c.DefaultRequestHeaders.Add("X-ApiKey", likvidoClientConfiguration.ApiKey);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.Decorate<ILikvidoClient, CachedLikvidoClient>();
            
            services.AddMediatR(typeof(GetInvoices).GetTypeInfo().Assembly);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisCache");
                options.InstanceName = Configuration["Redis:InstanceName"];
            });

            var servicebusConfiguration = new ServiceBusConfiguration();
            Configuration.GetSection("Servicebus").Bind(servicebusConfiguration);
            services.AddSingleton(servicebusConfiguration);

            services.AddSingleton<IDistributedConsumer, DistributedConsumer>();
            services.AddTransient<IDistributedSender, DistributedSender>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ServiceBusConfiguration busConfig)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InvoiceReader v1"));
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            var distributedCommands = typeof(NewInvoiceAdded).Assembly.GetTypes()
                .Where(p => p.GetInterfaces().Contains(typeof(IDistributedCommand))).ToList();

            app.RegisterServiceBus(serviceProvider, distributedCommands, typeof(NewInvoiceAdded).Assembly, busConfig.PrimaryConnectionString);

            app.UseRouting();

            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
