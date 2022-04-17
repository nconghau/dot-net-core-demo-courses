using CourseDemo.Config;
using CourseDemo.Context.Postgres;
using CourseDemo.CQRS.Logger;
using CourseDemo.CQRS.Validator;
using CourseDemo.Models;
using CourseDemo.Mongo.Context;
using CourseDemo.Mongo.UnitOfWork;
using CourseDemo.Repositories.Mongo;
using CourseDemo.Repositories.Postgres;
using CourseDemo.Servives;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;

namespace CourseDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // MongoDB
            services.Configure<CoursesStoreDatabaseSettings>(
                Configuration.GetSection(nameof(CoursesStoreDatabaseSettings)));

            services.AddSingleton<ICoursesStoreDatabaseSettings>(provider =>
                provider.GetRequiredService<IOptions<CoursesStoreDatabaseSettings>>().Value);

            // Postgres
            services.AddEntityFrameworkNpgsql().AddDbContext<PGContext>(
                options => options.UseNpgsql(new CoursesStoreDatabasePGSettings().ConnectionString));

            // Repository
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();

            services.AddScoped<ICoursesPGRepository, CoursesPGRepository>();
            services.AddScoped<ICourseService, CourseService>();

            // MediaR
            services.AddMediatR(typeof(Startup).Assembly);

            // Validator
            AssemblyScanner.FindValidatorsInAssembly(typeof(Startup).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingHandler<,>));

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Course Demo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });

            // Controller
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //app.UseHttpsRedirection();

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course Demo API V1");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
