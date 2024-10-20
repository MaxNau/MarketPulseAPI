using MarketPriceAPI.Configurations.MapProfiles;
using MarketPriceAPI.Configurations.Settings;
using MarketPriceAPI.Data.Contexts;
using MarketPriceAPI.Data.Repositories;
using MarketPulseAPI.Automation.Jobs;
using MarketPulseAPI.BackgroundServices;
using MarketPulseAPI.Fintacharts.Models;
using MarketPulseAPI.Interfaces.Clients.Fintacharts;
using MarketPulseAPI.Interfaces.Data.Repositories;
using MarketPulseAPI.Interfaces.Services.Mid;
using MarketPulseAPI.Services.Clients.Fintacharts;
using MarketPulseAPI.Services.Mid;
using MarketPulseAPI.Services.TokenStores;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace MarketPulseAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var fintachartsHttpSettings = builder.Configuration.GetSection("FintachartsHttp").Get<FintachartsHttpSettings>();
            if (fintachartsHttpSettings != null)
            {
                builder.Services.AddSingleton(fintachartsHttpSettings);
            }

            var fintachartsWebSocketsSettings = builder.Configuration.GetSection("FintachartsWebSockets").Get<FintachartsWebSocketsSettings>();
            if (fintachartsWebSocketsSettings != null)
            {
                builder.Services.AddSingleton(fintachartsWebSocketsSettings);
            }

            var fintachartsAuthSettings = builder.Configuration.GetSection("FintachartsAuth").Get<FintachartsAuthSettings>();
            if (fintachartsAuthSettings != null)
            {
                builder.Services.AddSingleton(fintachartsAuthSettings);
            }

            builder.Services.AddAutoMapper(typeof(PricesProfile));
            builder.Services.AddDbContext<MarketPulseAPIDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__MarketPulseDB")));

            builder.Services.AddQuartz(q =>
            {
                q.UsePersistentStore(options =>
                {
                    var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings_QuartzDB");
                    if (connectionString != null)
                    {
                        options.UseSqlServer(connectionString);
                        options.UseProperties = true;
                    }
                    options.UseNewtonsoftJsonSerializer();
                });

                var jobKey = new JobKey(nameof(AssetFetchJob));
                q.AddJob<AssetFetchJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("assetFetchTrigger")
                    .WithCronSchedule("0 0 * * * ?"));
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            builder.Services.AddTransient<IAssetsRepository, AssetsRepository>();
            builder.Services.AddHttpClient<IFintachartsHttpClient, FintachartsHttpClient>()
                .ConfigureHttpClient((sp, httpClient) =>
                {
                    var settings = sp.GetRequiredService<FintachartsHttpSettings>();
                    httpClient.BaseAddress = settings.BaseUri;
                });
            builder.Services.AddHttpClient<FintachartsTokenClient>()
                .ConfigureHttpClient((sp, httpClient) =>
                {
                    var settings = sp.GetRequiredService<FintachartsHttpSettings>();
                    httpClient.BaseAddress = settings.BaseUri;
                });

            builder.Services.AddSingleton<ITokenStore<FintachartsToken>, FintachartsTokenStore>();

            builder.Services.AddHostedService<FintachartsTokenRefreshBackgroundService>();

            builder.Services.AddTransient<IAssetsPricesService, AssetsPricesService>();
            builder.Services.AddTransient<IFintachartsWebSocketClient, FintachartsWebSocketClient>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Ensure database is created or migrated
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<MarketPulseAPIDbContext>();
                await context.Database.EnsureCreatedAsync(); // Use this for migrations
                                                       // Alternatively, you could use context.Database.EnsureCreated(); if not using migrations
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseWebSockets();

            var scheduler = await app.Services.GetRequiredService<ISchedulerFactory>().GetScheduler();
            await scheduler.Start();

            await scheduler.TriggerJob(new JobKey(nameof(AssetFetchJob)));

            app.Run();
        }
    }
}
