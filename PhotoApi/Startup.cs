using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PhotoApi.Database;
using PhotoApi.services;
using PhotoApi.services.Interfaces;

namespace PhotoApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<PhotoDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            var cloudinarySettings = this.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            if (cloudinarySettings == null)
            {
                throw new Exception("Cloudinary settings are not configured properly.");
            }

            services.AddSingleton(new Cloudinary(new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret
            )));

            services.AddTransient<IPhotoServices, PhotoServices>();

            services.AddAutoMapper(typeof(Startup));

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            // Add Swagger services
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Photo API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Photo API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
