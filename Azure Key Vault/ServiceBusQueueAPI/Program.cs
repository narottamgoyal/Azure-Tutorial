using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace ServiceBusQueueAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig  = config.Build();
                    var azureVaultUrl = builtConfig .GetSection("Configuration:AzureVaultUrl").Value;
                    var keyVaultClient = new KeyVaultClient(async (aut, re, sc) =>
                    {
                        var credential = new DefaultAzureCredential(false);
                        var token = await credential.GetTokenAsync(
                            new TokenRequestContext(new[] { "https://vault.azure.net/.default" }));
                        return token.Token;
                    });
                    config.AddAzureKeyVault(azureVaultUrl, keyVaultClient, new DefaultKeyVaultSecretManager());
                });
    }
}
