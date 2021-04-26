using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MISA.CukCuk.Api.MiddleWare;
using MISA.CukCuk.Core.Exceptions;
using MISA.CukCuk.Core.Interfaces.Repositories;
using MISA.CukCuk.Core.Interfaces.Services;
using MISA.CukCuk.Core.Services;
using MISA.CukCuk.Infrastructure.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MISA.CukCuk.Api", Version = "v1" });
            });

            //Tiêm vào start-up để xác định xem cái nào làm việc với cái nào
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerServices, CustomerServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MISA.CukCuk.Api v1"));
            }


            //Xử lý global exception 
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //Xử lý exception
            app.UseExceptionHandler(c => c.Run(async context =>
            {

                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                if (exception is CustomerException)
                {

                    var response = new
                    {
                        devMsg = exception.Message,
                        userMsg = "Có lỗi xảy ra vui lòng liên hệ MISA",
                        MISACode = "002",
                        Data = "CustomerCode",
                    };
                    var result = JsonConvert.SerializeObject(response);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //var jsonObject = JsonConvert.SerializeObject(My Custom Model);
                    await context.Response.WriteAsJsonAsync(response);
                }
                else
                {
                    var response = new
                    {
                        devMsg = exception.Message,
                        userMsg = "Có lỗi xảy ra vui lòng liên hệ MISA",
                        MISACode = "002",
                        Data = exception
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }));
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
