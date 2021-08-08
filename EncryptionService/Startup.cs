using EncryptionService.Encryption;
using EncryptionService.Encryption.AES;
using EncryptionService.Encryption.Keys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EncryptionService
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
            services.AddResponseCompression();
            services.AddControllers();
            services.AddHealthChecks();

            services
                .AddSingleton<IEncryptionService, AESEncryptionService>()
                .AddSingleton<IEncryptionKeyManager<AESKey>, InMemoryEncryptionKeyManager<AESKey>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEncryptionService encryptionService)
        {
            encryptionService.RotateKey(); // Initialise key
            
            app.UseResponseCompression();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
