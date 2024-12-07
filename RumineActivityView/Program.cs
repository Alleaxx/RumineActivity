using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using RumineActivity.Core;
using RumineActivity.Core.API;
using RumineActivity.Core.Comparisons;

namespace RumineActivity.View
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<Core.Logging.ILogger, Core.Logging.ConsoleLogger>();
            builder.Services.AddScoped<IJsonService, JsonServiceDefault>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

            bool onlineMode = false;

            if (onlineMode)
            {
                builder.Services.AddScoped<IActivityApi, ActivityWebApi>();
            }
            else
            {
                var filePathV4 = JsonFileInfo.FromObjectJson("data/ForumPostsV4.json", true);

                builder.Services.AddScoped<IActivityApi, ActivityFileApi>(sc =>
                {
                    return new ActivityFileApi(sc.GetService<HttpClient>(), sc.GetService<Core.Logging.ILogger>(), filePathV4);
                });
            }

            var ragesFile = JsonFileInfo.FromObjectJson("data/RageStatisticsV1.json", true);

            builder.Services.AddScoped<IRagesApi, RagesFileApi>(sc =>
            {
                return new RagesFileApi(sc.GetService<HttpClient>(), sc.GetService<Core.Logging.ILogger>(), ragesFile);
            });


            builder.Services.AddScoped<IStatApp, StatApp>();
            builder.Services.AddScoped<IReportsFactory, ReportsFactory>();

            await builder.Build().RunAsync();
        }
    }
}
