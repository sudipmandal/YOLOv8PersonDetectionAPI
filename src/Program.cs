using YoloPersonDetectionAPI.Services;

namespace YoloPersonDetectionAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHostedService<EnvDefaultService>();
            builder.Services.AddHostedService<LocalCacheService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.MapControllers();


            app.Run();
        }
    }
}