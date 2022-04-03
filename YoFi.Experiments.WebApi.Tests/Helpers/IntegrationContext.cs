using Common.DotNet;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YoFi.Core;
using YoFi.Experiments.WebApi.Data;

namespace YoFi.Tests.Integration.Helpers
{
    public class IntegrationContext: IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> factory;
        private readonly TestServer server;
        private readonly IServiceScope scope;

        public HttpClient client { get; private set; }
        public ApplicationDbContext context { get; private set; }
        public TestClock clock { get; private set; }

        public IntegrationContext(string name)
        {
            factory = new CustomWebApplicationFactory<Program>() { DatabaseName = name };
            server = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IClock>(clock = new TestClock());
                });
            }).Server;
            client = server.CreateClient();
            scope = server.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            context = scopedServices.GetRequiredService<ApplicationDbContext>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
