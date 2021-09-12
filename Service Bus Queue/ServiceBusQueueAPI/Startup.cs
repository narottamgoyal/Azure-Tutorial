using AzureEventServiceBus;
using BasicEventBus;
using BasicEventBus.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServiceBusQueueAPI.Events;
using ServiceBusQueueAPI.Events.EventHandlers;
using System.Collections.Generic;

namespace ServiceBusQueueAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiceBusQueueAPI", Version = "v1" });
            });
            RegisterServices(services);
            AddEventHandlers(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceBusQueueAPI v1"));
            }

            ConfigureEventBus(app);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #region Event Consumer

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();
            services.AddSingleton<IEventBusService, EventBusService>();
            services.AddSingleton<IEventConsumerService, EventConsumerService>();
            services.AddSingleton<IHostedService>(sp =>
            {
                var baseEventServiceBus = sp.GetRequiredService<IEventConsumerService>();
                return new HostedBackgroundService(baseEventServiceBus, GetEventOrQueueNames());
            });
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBusService>();
            //eventBus.Subscribe<SampleDemo1Event, SampleDemo1EventHandler>();
            eventBus.Subscribe<SampleDemo2Event, SampleDemo2EventHandler>();
        }

        public void AddEventHandlers(IServiceCollection services)
        {
            services.AddTransient<SampleDemo1EventHandler>();
            services.AddTransient<SampleDemo2EventHandler>();
        }

        private List<string> GetEventOrQueueNames()
        {
            return new List<string>
            {
                typeof(SampleDemo1Event).FullName,
                typeof(SampleDemo2Event).FullName
            };
        }

        #endregion Event Consumer
    }
}
