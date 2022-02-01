using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Funda.Client.Services;
using System;

namespace Funda.Client
{
    public class program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            
            //configuring services
            ConfigureServices(services);

            //providing services of IServiceCollection
            var provider = services.BuildServiceProvider();

            try
            {
                //executing httpServiceClient method async
                await provider.GetRequiredService<IHttpClientService>()
                    .Execute();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex}");
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            //using dependency injection for registering httpclientservice with the scoped lifetime 
            services.AddScoped<IHttpClientService, HttpClientService>();
        }
    }
}
