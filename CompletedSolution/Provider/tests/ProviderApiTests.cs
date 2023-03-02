using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using System;
using System.Collections.Generic;
using tests.XUnitHelpers;
using Xunit;
using Xunit.Abstractions;

namespace tests
{
    public class ProviderApiTests : IDisposable
    {
        private string ProviderUri { get; }
        private string PactServiceUri { get; }
        private IWebHost WebHost { get; }
        private ITestOutputHelper OutputHelper { get; }

        public ProviderApiTests(ITestOutputHelper output)
        {
            OutputHelper = output;
            ProviderUri = "http://localhost:9000";
            PactServiceUri = "http://localhost:9001";

            WebHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseUrls(PactServiceUri)
                .UseStartup<TestStartup>()
                .Build();

            WebHost.Start();
        }

        [Fact]
        public void EnsureProviderApiHonorsPactWithConsumer()
        {
            // Arrange
            var config = new PactVerifierConfig
            {

                // NOTE: We default to using a ConsoleOutput,
                // however xUnit 2 does not capture the console output,
                // so a custom out-putter is required.
                Outputters = new List<IOutput>
                                {
                                    new XUnitOutput(OutputHelper)
                                },

                // Output verbose verification logs to the test output
                Verbose = true,
                PublishVerificationResults = true,
                ProviderVersion = "2.4.1-f3842db9e603d7",

            };

            //Act + Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{PactServiceUri}/provider-states")
                .ServiceProvider("provider", ProviderUri)
                .HonoursPactWith("consumer")
                .PactBroker("http://localhost:9292"
                    // uriOptions: new PactUriOptions(System.Environment.GetEnvironmentVariable("PACT_BROKER_TOKEN")),
                    // consumerVersionTags: new List<string> { "master" }
                    )
                .Verify();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    WebHost.StopAsync().GetAwaiter().GetResult();
                    WebHost.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
