using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.StreammimgApi.Tests
{
    public class ApiTestsServer : WebApplicationFactory<Program>
    {
        private readonly string _environment;
        private readonly string API_BASE_URL = "http://localhost:8080";
        private const string DEFAULT_ENVIRONMENT = "Development";

        public ApiTestsServer(string environment = DEFAULT_ENVIRONMENT)
        {
            _environment = environment;
            WebApplicationFactoryClientOptions webApplicationFactoryClientOptions = new WebApplicationFactoryClientOptions();
            webApplicationFactoryClientOptions.BaseAddress = new Uri(API_BASE_URL);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
           builder.UseEnvironment(_environment);
            return base.CreateHost(builder);
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
        }

        public HttpClient CreateJsonClient()
        {
            var client = CreateClient();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
