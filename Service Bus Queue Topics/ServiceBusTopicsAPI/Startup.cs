using AzureEventServiceBus;
using AzureEventServiceBus.Events;
using BasicEventBus;
using BasicEventBus.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using ServiceBusTopicsAPI.Events.EventHandlers;

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

            BindAzureTopicSubscription(app);

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
            services.AddSingleton<IHostedService, HostedBackgroundService>();
        }

        private void BindAzureTopicSubscription(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBusService>();

            eventBus.Subscribe<TodoTaskCreatedEvent, WeekdaySubscriptionHandler>(SubscriptionNames.weekday.ToString());

            eventBus.Subscribe<TodoTaskCreatedEvent, WeekendSubscriptionHandler>(SubscriptionNames.weekend.ToString());
        }

        public void AddEventHandlers(IServiceCollection services)
        {
            services.AddTransient<WeekdaySubscriptionHandler>();
            services.AddTransient<WeekendSubscriptionHandler>();
        }

        #endregion Event Consumer
    }
}
