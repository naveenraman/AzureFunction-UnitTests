using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Functions.Tests
{
    /// <summary>
    /// Functions Tests
    /// </summary>
    public class FunctionsTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void Http_trigger_should_return_known_string()
        {
            var request = TestFactory.CreateHttpRequest("name", "Bill");
            var response = (OkObjectResult)await HttpFunction.Run(request, logger);
            Assert.Equal("Hello, Bill", response.Value);
        }

        [Theory]
        [MemberData(nameof(TestFactory.Data), MemberType = typeof(TestFactory))]
        public async void Http_trigger_should_return_known_string_from_member_data(string queryStringKey, string queryStringValue)
        {
            var request = TestFactory.CreateHttpRequest(queryStringKey, queryStringValue);
            var response = (OkObjectResult)await HttpFunction.Run(request, logger);
            Assert.Equal($"Hello, {queryStringValue}", response.Value);
        }

        [Fact]
        public void Timer_should_log_message()
        {
            var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            TimerFunction.Run(null, logger);
            var msg = logger.Logs[0];
            Assert.Contains("C# Timer trigger function executed at", msg);
        }

        [Fact]
        public async Task Blob_trigger_should_return_valid_stream_and_name()
        {
            var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            Stream s = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(s))
            {
                await sw.WriteLineAsync("This is a test");
                BlobFunction.Run(s, "TestBlob", logger);
            }
            var msg = logger.Logs[0];
            Assert.Contains("C# Blob trigger function Processed blob Name:TestBlob", msg);
        }
    }
}
