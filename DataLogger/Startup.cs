using AutoMapper;
using Business.Consumers;
using Business.Consumers.Contracts;
using Business.Hubs;
using Business.Hubs.Contracts;
using Business.Services;
using Business.Services.Contracts;
using Connection.Extensions;
using DataLogger.Mappings;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using System.Linq;

namespace DataLogger
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
            services.AddDbContext<DataLoggerDbContext>(o => o.UseSqlite(@"Data Source=DataLogger.db"), ServiceLifetime.Transient);
            services.AddScoped<IConsoleMeasurementHandler, ConsoleMeasurementHandler>();
            services.AddScoped<IDatabaseMeasurementHandler, DatabaseMeasurementHandler>();
            services.AddScoped<IOutputMeasurementHandler, OutputMeasurementHandler>();

            services.AddScoped<IMeasurementService, MeasurementService>();
            services.AddScoped<IManagementService, ManagementService>();
            services.AddScoped<IMeasurementHub, MeasurementHub>();
            services.AddScoped<IRestClient, RestClient>();

            services.AddTransient<ISensorService, SensorService>();


            services.AddSensorHub(Configuration);
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            }));
            services.AddControllers();

            services.AddSignalR();





            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection(); Automatische Weiterleitung auf HTTPS verhindern, da kein Zertifikate.
            app.UseRouting();

            app.UseStaticFiles();
            app.UseBlazorFrameworkFiles();


            app.UseResponseCompression();

            app.UseSensorHub();
            (app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService(typeof(IDatabaseMeasurementHandler)) as DatabaseMeasurementHandler).StartHandler();
            (app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService(typeof(IOutputMeasurementHandler)) as OutputMeasurementHandler).StartHandler();
            (app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService(typeof(IConsoleMeasurementHandler)) as ConsoleMeasurementHandler).StartHandler();
            (app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService(typeof(IManagementService)) as ManagementService).InitDeviceProperties();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
                
            

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapHub<MeasurementHub>("/measurementhub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}