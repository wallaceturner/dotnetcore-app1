// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.AspNetCore.Server.Kestrel.Filter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApp
{
    public class Startup
    {
        private static string Args { get; set; }
        private static CancellationTokenSource ServerCancellationTokenSource { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
           services.AddMvcCore()
                   .AddJsonFormatters();
            services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationEnvironment env, IHostingEnvironment host)
        {
            var baseDirectory = PlatformServices.Default.Application.ApplicationBasePath;
            Console.WriteLine("ApplicationBasePath: " + PlatformServices.Default.Application.ApplicationBasePath);
            var ksi = app.ServerFeatures.Get<IKestrelServerInformation>();
            ksi.NoDelay = true;

            loggerFactory.AddConsole(LogLevel.Error);

            app.UseKestrelConnectionLogging();
            app.UseStaticFiles();
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(baseDirectory, @"..\..\..\..\StaticFiles")),
            //    RequestPath = new PathString("/StaticFiles")
            //});
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(baseDirectory, @"..\..\..\..\StaticFiles")),
                RequestPath = new PathString("/StaticFiles"),
                EnableDirectoryBrowsing = true
            });
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseMvcWithDefaultRoute();
			// app.UseWelcomePage();
        }

        public static int Main(string[] args)
        {
            // if (args.Length == 0)
            // {
            // Console.WriteLine("KestrelHelloWorld <url to host>");
            // return 1;
            // }
            //var foo = new SampleApp.Model.FooClass();

            // var url = new Uri(args[0]);
			var url = "http://*:5004";
            Args = string.Join(" ", args);

            var host = new WebHostBuilder()
                .UseServer("Microsoft.AspNetCore.Server.Kestrel")
                .UseUrls(url.ToString())
                .UseStartup<Startup>()
                .Build();

            ServerCancellationTokenSource = new CancellationTokenSource();

            // // shutdown server after 20s.
            // var shutdownTask = Task.Run(async () =>
            // {
                // await Task.Delay(20000);
                // ServerCancellationTokenSource.Cancel();
            // });

            host.Run(ServerCancellationTokenSource.Token);
            // shutdownTask.Wait();
			Console.WriteLine("running...");
			Console.ReadLine();

            return 0;
        }
    }
}
