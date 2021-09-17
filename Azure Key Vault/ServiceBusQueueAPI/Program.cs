using Microsoft.AspNetCore.Hosting;
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

                //Using ClienId and Client Secret

                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    var builtConfig = config.Build();
                //    var azureVaultUrl = builtConfig.GetSection("Configuration:AzureVaultUrl").Value;
                //    var clientId = builtConfig.GetSection("Configuration:ClientId").Value;
                //    var clientSecret = builtConfig.GetSection("Configuration:ClientSecret").Value;
                //    config.AddAzureKeyVault(azureVaultUrl, clientId, clientSecret,
                //        new PrefixKeyVaultSecretManager("ServiceBusQueueAPI"));
                //})

                //+-+-+-+-+-+-+-+-+-+- Uncomment one of the option -+-+-+-+-+-+-+-+-+

                //Using ClienId and Certificate Thumb print

                //.ConfigureAppConfiguration((context, config) =>
                //{

                //    var builtConfig = config.Build();
                //    var azureVaultUrl = builtConfig.GetSection("Configuration:AzureVaultUrl").Value;
                //    var clientId = builtConfig.GetSection("Configuration:ClientId").Value;
                //    var thumbprint = builtConfig.GetSection("Configuration:Thumbprint").Value;
                //    config.AddAzureKeyVault(
                //        azureVaultUrl,
                //        clientId,
                //        AzureKeyVaultCertificationExtension.FindCertificateByThumbprint(thumbprint),
                //        new PrefixKeyVaultSecretManager("ServiceBusQueueAPI"));
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
