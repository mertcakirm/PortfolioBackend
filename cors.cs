namespace Cors
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS yapılandırmasını ekleyin
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://localhost") 
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Diğer hizmetleri ekleyin
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Geliştirme ortamında CORS kullanımı
            if (env.IsDevelopment())
            {
                app.UseCors("AllowSpecificOrigin");  // CORS politikasını uygulamak
            }

            // HTTPS yönlendirmesi ve diğer middleware'ler
            app.UseHttpsRedirection();

            // Controller'lar için route'ları haritalamak
            app.UseRouting();

            // CORS politikasını uygulamak için middleware ekleyin
            app.UseCors("AllowSpecificOrigin");

            // Son olarak, Controller'lar ile ilgili işlem yapalım
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
