using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Tests.Shared;
using System;

namespace Api.Tests.ControllerTests
{
    [TestClass]
    public class SampleControllerTests
    {
        private int _coeff;

        public static ApiWebApplicationFactory Factory { get; set; }
        public static HttpClient Client { get; set; }

        [TestInitialize]
        public void Init()
        {
            _coeff = 5;
        }

        [ClassInitialize]
        public static void InitOnce(TestContext context)
        {
            Factory = new ApiWebApplicationFactory();
            Client = Factory.CreateClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [ClassCleanup]
        public static void CleanUpOnce()
        {
            Client.Dispose();
            Factory.Dispose();
        }

        [TestMethod]
        public async Task GetRandom_ExpectZero_WhenCoeffIsNull()
        {
            var response = await Client.GetAsync($"api/sample");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(jsonString);
            var data = JsonConvert.DeserializeObject<Tuple<int, int>>(jsonString);
            Assert.IsNotNull(data);
            Assert.AreEqual(0, data.Item2);
        }

        [TestMethod]
        [DataRow(1, 10)]
        [DataRow(7, 70)]
        [DataRow(10, 100)]
        public async Task GetRandom_ExpectResultNotGreaterThanSpecifiedValue_DependingOnTestCases(int coeff, int maxValue)
        {
            var response = await Client.GetAsync($"api/sample?coeff={coeff}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(jsonString);
            var data = JsonConvert.DeserializeObject<Tuple<int, int>>(jsonString);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Item2 <= maxValue);
        }

        [TestMethod]
        public async Task GetRandom_ExpectSuccessResult_UnderValidCircumstances()
        {
            var response = await Client.GetAsync($"api/sample?coeff={_coeff}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(jsonString);
            var data = JsonConvert.DeserializeObject<Tuple<int, int>>(jsonString);
            Assert.IsNotNull(data);
        }
    }
}