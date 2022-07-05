using System;
using System.Net.Http;
using Calendar.DataModels;
using Calendar.WebService.Config;
using Calendar.WebService.Services;
using Kalantyr.Auth.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Calendar.WebService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthConfig>(_configuration.GetSection("AuthService"));

            services.AddSingleton<IAppAuthClient>(sp => new AuthClient(
                sp.GetService<IHttpClientFactory>(),
                sp.GetService<IOptions<AuthConfig>>().Value.AppKey));
            services.AddScoped<EventService>();
            services.AddScoped<IEventStorage, SqlStorage.SqlStorage>();
            services.AddScoped<IEventValidator, EventValidator>();
            services.AddScoped<IHealthCheck, EventService>();

            services.AddHttpClient<AuthClient>((sp, client) =>
            {
                client.BaseAddress = new Uri(sp.GetService<IOptions<AuthConfig>>().Value.ServiceUrl);
            });

            services.AddSwaggerDocument();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();
            app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
        }
    }
}
