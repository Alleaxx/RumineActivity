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
namespace RumineActivity.View
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            bool onlineMode = false;
            if (onlineMode)
            {
                builder.Services.AddScoped<IActivityApi, ActivityWebStorageLoggedApi>();
            }
            else
            {
                //builder.Services.AddScoped<IActivityApi, LocalLoggedApi>();
                builder.Services.AddScoped<IActivityApi, LocalApi>();
            }

            builder.Services.AddScoped<IStatApp, StatApp>();
            builder.Services.AddScoped<IPostEditor, PostEditor>();
            builder.Services.AddScoped<IReportsFactory, ReportsFactory>();
            builder.Services.AddScoped<IComparison, Comparison>();

            await builder.Build().RunAsync();
        }
    }
}
