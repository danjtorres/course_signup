using Courses.Domain.Course;
using Courses.Domain.Lecturer;
using Courses.Infra;
using Courses.Infra.Repository;
using Courses.MessageBus;
using CoursesSignUp.API.Commands.Handlers;
using CoursesSignUp.API.Commands.Requests;
using CoursesSignUp.API.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CoursesSignUp.API
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
            //Multi Context
            services.AddDbContext<CourseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<LecturerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.AddMediatR(typeof(Startup));

            // Commands
            services.AddScoped<IRequestHandler<CreateCourseRequest, CreateCourseResponse>, CreateCourseHandler>();
            services.AddScoped<IRequestHandler<CreateLecturerRequest, CreateLecturerResponse>, CreateLecturerHandler>();
            services.AddScoped<IRequestHandler<SignUpCourseRequest, SignUpCourseResponse>, SignUpCourseHandler>();

            //bus
            services.AddMessageBus(Configuration?.GetSection("MessageQueueConnection")?["MessageBus"]);

            // Data
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ILecturerRepository, LecturerRepository>();
            services.AddScoped<CourseContext>();
            services.AddScoped<LecturerContext>();


            services.AddCors(options =>
            {
                options.AddPolicy("Everithing",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Everithing");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
