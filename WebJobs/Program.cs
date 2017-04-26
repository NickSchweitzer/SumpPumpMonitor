using System;
using System.Configuration;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace WebJobs
{
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                string eventHubName = ConfigurationManager.AppSettings["EventHubName"];
                string eventHubEndpoint = ConfigurationManager.AppSettings["EventHubEndpoint"];

                config.UseDevelopmentSettings();
                EventHubConfiguration hubConfig = new EventHubConfiguration();
                hubConfig.AddReceiver(eventHubName, eventHubEndpoint);
                config.UseEventHub(hubConfig);
            }

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}