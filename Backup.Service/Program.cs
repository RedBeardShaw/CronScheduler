﻿using Scheduling.Service.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace Scheduling.Service
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var isDebugging = !(Debugger.IsAttached || args.Contains("--console"));
            var hostBuilder = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<SchedulingService>();
                });
            if (isDebugging)
            {
                await hostBuilder.RunTheServiceAsync();
            }
            else
            {
                await hostBuilder.RunConsoleAsync();
            }
        }
    }
}
