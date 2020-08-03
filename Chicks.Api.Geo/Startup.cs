using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Chicks.Database.Sql;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Chicks.Api.Geo
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
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddDbContext<ChicksDbContext>(options => {
                options.UseLazyLoadingProxies()
                        .UseSqlServer(this.Configuration.GetConnectionString("ChicksSqlServer"));
                //.UseSqlServer("Data Source=sqldata;Initial Catalog=Chicks;User Id=sa;Password=Pass@word");

            }, ServiceLifetime.Scoped);

            services.AddScoped<ChicksRepositoryProvider>();


            // Maybe can be a good idea use a abstraction of more level to use rabbitmq as
            // https://github.com/dotnet-architecture/eShopOnContainers/tree/dev/src/BuildingBlocks/EventBus/EventBusRabbitMQ
            services.AddScoped<RabbitMQ.Client.IConnection>(x => {
                var rabbitMQConfig = new Config.RabbitMQ();
                this.Configuration.Bind("RabbitMQ", rabbitMQConfig);

                var connectionFactory = new RabbitMQ.Client.ConnectionFactory() { 
                    UserName = rabbitMQConfig.UserName,
                    Password = rabbitMQConfig.Password,
                    HostName = rabbitMQConfig.HostName
                };

                return connectionFactory.CreateConnection();
            });            

            services.AddControllers();

            services.AddSwaggerGen(config =>
            {
                config.EnableAnnotations();

                config.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Chicks",
                        Version = "v1.0.0"
                    });

                config.CustomSchemaIds((type) => type.FullName);


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ChicksDbContext>();
                context.Database.Migrate();
            }


            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger(x => x.SerializeAsV2 = env.IsDevelopment() ? false : true);

            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Kamino");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
