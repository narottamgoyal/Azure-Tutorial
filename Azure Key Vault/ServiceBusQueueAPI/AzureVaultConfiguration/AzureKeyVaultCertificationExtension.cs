using System;
using System.Security.Cryptography.X509Certificates;

namespace ServiceBusQueueAPI.AzureVaultConfiguration
{
    public class AzureKeyVaultCertificationExtension
    {
        public static X509Certificate2 FindCertificateByThumbprint(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (certificates?.Count == 0)
                {
                    throw new Exception("Error: Certificate not found!");
                }
                return certificates[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                store.Close();
            }
        }
    }
}
