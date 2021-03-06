using Calendar.WebService;
using Kalantyr.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        RunWrapper.LogIfException(() =>
        {
            CreateHostBuilder(args).Build().Run();
        });
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseIISIntegration()
                    .UseStartup<Startup>();
            });
}
