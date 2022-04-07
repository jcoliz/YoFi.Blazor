using Common.DotNet;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using YoFi.WireApi.Host.Data;

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
