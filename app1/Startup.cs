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
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationEnvironment env, IHostingEnvironment host)
        {
            Console.WriteLine("webroot: " + host.WebRootPath);
            var ksi = app.ServerFeatures.Get<IKestrelServerInformation>();
            ksi.NoDelay = true;

            loggerFactory.AddConsole(LogLevel.Error);

            app.UseKestrelConnectionLogging();
            app.UseStaticFiles();
            app.UseMvc();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(@"D:\aaa_development\Spikes\dotnetcli\cli\TestAssets\TestProjects\PortableTests\StandaloneApp\StaticFiles"),
            //    RequestPath = new PathString("/StaticFiles")
            //});

            app.UseDeveloperExceptionPage();

            //app.Run(async context =>
            //{
            //    Console.WriteLine("{0} {1}{2}{3}",
            //        context.Request.Method,
            //        context.Request.PathBase,
            //        context.Request.Path,
            //        context.Request.QueryString);
            //    Console.WriteLine($"Method: {context.Request.Method}");
            //    Console.WriteLine($"PathBase: {context.Request.PathBase}");
            //    Console.WriteLine($"Path: {context.Request.Path}");
            //    Console.WriteLine($"QueryString: {context.Request.QueryString}");

            //    var connectionFeature = context.Connection;
            //    Console.WriteLine($"Peer: {connectionFeature.RemoteIpAddress?.ToString()} {connectionFeature.RemotePort}");
            //    Console.WriteLine($"Sock: {connectionFeature.LocalIpAddress?.ToString()} {connectionFeature.LocalPort}");
            //    string s = null;
            //    var foo = s.ToString();
            //    var content = $"Hello world!{Environment.NewLine}Received '{Args}' from command line.";
            //    context.Response.ContentLength = content.Length;
            //    context.Response.ContentType = "text/plain";
            //    await context.Response.WriteAsync(content);
            //});
        }

        public static int Main(string[] args)
        {
            // if (args.Length == 0)
            // {
                // Console.WriteLine("KestrelHelloWorld <url to host>");
                // return 1;
            // }

            // var url = new Uri(args[0]);
			var url = "http://localhost:5004";
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
