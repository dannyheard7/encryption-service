using System;
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
                .AddSingleton<IEncryptionKeyManager<AESKey>, InMemoryEncryptionKeyManager<AESKey>>()
                .AddScoped<IEncryptionService, AESEncryptionService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEncryptionService encryptionService)
        {
            encryptionService.RotateKey(); // Initialise key
            
            app.UseResponseCompression();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
