using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace anBlogg.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //using (var scope = host.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetService<BlogContext>();
            //    context.Database.EnsureDeleted();
            //    context.Database.Migrate();
            //}

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}