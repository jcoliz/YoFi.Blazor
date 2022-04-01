using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoFi.AspNet.Data;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.Experiments.WebApi.Tests
{
    [TestClass]
    public abstract class BaseApiTests: IntegrationTests
    {
        #region Fields

        protected static SampleDataStore data;
        protected abstract string urlroot { get; set; }

        #endregion

        #region Helpers
        protected async Task<JsonDocument> WhenGetAsync(string url, HttpStatusCode expectedresult = HttpStatusCode.OK)
        {
            // When: Getting {url}
            var response = await client.GetAsync(url);

            // Then: Result as expected
            Assert.AreEqual(expectedresult, response.StatusCode, url);

            // And: Response is valid JSON, if valid
            JsonDocument document = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

            return document;
        }
        protected async Task<HttpResponseMessage> WhenSendAsync<T>(string url, T item, HttpMethod method = null)
        {
            var request = new HttpRequestMessage(method ?? HttpMethod.Post, url);
            request.Content = new StringContent(JsonSerializer.Serialize<T>(item), Encoding.UTF8, "application/json");
            var outresponse = await client.SendAsync(request);

            return outresponse;
        }

        #endregion
    }
}
