using Microsoft.Extensions.Configuration;

namespace Api.Tests.Shared
{
    public class TestServerStartup : Startup
    {
        public TestServerStartup(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}