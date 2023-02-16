// See https://aka.ms/new-console-template for more information

using linode_console.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;


 Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();
     

var host = Host.CreateDefaultBuilder()
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders(); // clear all the previous logger providers
    })
    .ConfigureServices( services =>
    {
        services.AddSingleton<IS3Service, S3Service>();
        
    }) // use serilog
    .UseSerilog((hostingContext,loggerConfiguration) => 
         loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
        )
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        builder.AddEnvironmentVariables();
       
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            builder.AddUserSecrets<Program>();
        }
    })
    .Build();
IS3Service app = ActivatorUtilities.CreateInstance<S3Service>(host.Services);
await app.ListS3BucketsAsync();

