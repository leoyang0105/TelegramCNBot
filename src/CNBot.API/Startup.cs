using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CNBot.API.Application.EventHandlers;
using CNBot.API.Application.Events;
using CNBot.Core.Clients;
using CNBot.Core.Configurations;
using CNBot.Core.EventBus.Abstractions;
using CNBot.Core.Services;
using CNBot.Infrastructure;
using CNBot.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace CNBot.API
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

            services.RegisterMySqlDbContext<ApplicationDbContext>(Configuration["DefaultConnection"]);
            services.RegisterRedisCache(Configuration["RedisConnection"]);
            services.RegisterRabbitMQ(Configuration);

            RegisterTelegramHttpClients(services);

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<TelegramChatRefreshEventHandling>();
            services.AddTransient<TelegramMessageEventHandling>();
            services.AddTransient<TelegramUpdateEventHandling>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ConfigureEventBus(app);
        }
        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TelegramChatRefreshEvent, TelegramChatRefreshEventHandling>();
            eventBus.Subscribe<TelegramMessageEvent, TelegramMessageEventHandling>();
            eventBus.Subscribe<TelegramUpdateEvent, TelegramUpdateEventHandling>();

        }
        private void RegisterTelegramHttpClients(IServiceCollection services)
        {
            services.Configure<TelegramUrlsConfig>(Configuration.GetSection("Telegram"));
            services.AddHttpClient<ITelegramHttpClient, TelegramHttpClient>().AddPolicyHandler(GetRetryPolicy());
        }
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        }
    }
}
