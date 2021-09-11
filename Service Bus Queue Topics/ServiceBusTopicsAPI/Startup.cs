using AzureEventServiceBus;
using BasicEventBus;
using BasicEventBus.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using ServiceBusTopicsAPI.Events;
using ServiceBusTopicsAPI.Events.EventHandlers;
using System.Collections.Generic;

namespace ServiceBusTopicsAPI
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

            // https://stackoverflow.com/a/55541764/1175623
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddControllers();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiceBusTopicsAPI", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceBusTopicsAPI v1"));
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
            eventBus.Subscribe<SampleTodoItemEvent, SampleDemoEventHandler>();
        }

        public void AddEventHandlers(IServiceCollection services)
        {
            services.AddTransient<SampleDemoEventHandler>();
        }

        private List<string> GetEventOrQueueNames()
        {
            return new List<string> { typeof(SampleTodoItemEvent).FullName };
        }

        #endregion Event Consumer
    }
}
