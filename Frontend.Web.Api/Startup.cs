using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceUsers;
using Infrastructure.Grid;
using Infrastructure.MetaData;
//using ADO;
//changed
using ADO.NET;
using Infrastructure.Repository;
using Infrastructure.Pivot;
using StructureMap;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Frontend.Web.Api.Helper;

namespace Frontend.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            //services.AddControllers();

            // Conifigure AutoMapper
            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});
            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);


            //Configure CROS To Allow access to Web API
            services.AddCors(options =>
            {
                options.AddPolicy("angular", policy => policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
                //options.AddPolicy("Localhost", policy =>   policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().AllowAnyOrigin());
            });


            services.AddMvc();

            //Add Authrntication Using JwtBearer Middle layer
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecurityKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(10)
                };
            });




            //Add MailKit
            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    //get options from sercets.json
                    Server = Configuration["EmailConfiguration:SmtpServer"],
                    Port = Convert.ToInt32(Configuration["EmailConfiguration:Port"]),
                    SenderName = Configuration["EmailConfiguration:SenderName"],
                    SenderEmail = Configuration["EmailConfiguration:From"],

                    // can be optional with no authentication 
                    Account = Configuration["EmailConfiguration:Username"],
                    Password = Configuration["EmailConfiguration:Password"],

                    // enable ssl or tls
                    //Security = true
                });
            });
            //    var emailConfig = Configuration
            //.GetSection("EmailConfiguration")
            //.Get<EmailConfiguration>();
            //    services.AddSingleton(emailConfig);

            services.AddOptions();

            var container = new Container();
            container.Configure(config =>
            {
                //Add in our custom registry
                config.AddRegistry(new ServicesRegistry(Configuration));
                //Push the .net Core Services Collection into StructureMap
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Must be first 
            app.UseCors("angular");
            //Must be caled before MVC initialization 
            //app.UseMvc();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
