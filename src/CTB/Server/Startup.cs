using CTB.Server.Data;
using CTB.Server.Hubs;
using CTB.Server.Logic;
using CTB.Server.Models;
using CTB.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CTB.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSignalR()
            .AddMessagePackProtocol();
        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddOptions<CTBOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("CTB").Bind(settings);
            });
        services.AddSingleton<IRepository, Repository>();
        services.AddSingleton<IGameEngineServer, GameEngineServer>();
        services.AddSingleton<IHostedService, GameEngineBackgroundService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        logger.LogInformation("Configuring");

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
            endpoints.MapHub<GameHub>("/GameHub");
        });

        logger.LogInformation("Configure completed.");
    }
}
