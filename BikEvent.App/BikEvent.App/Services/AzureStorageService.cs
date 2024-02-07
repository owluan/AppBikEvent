using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BikEvent.App.Services
{
    public class AzureStorageService
    {
        private const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=bikeventstorage;AccountKey=Flkz3Cve5jG+VViPUbP5z/K9/PRJdoc0vWpSGlkSlYFficspsqh9/X44jpw8rf6HeYNNmaipF+EU+ASt5zwTng==;EndpointSuffix=core.windows.net";
        private const string ContainerName = "imagestorage"; 

        public async Task<string> UploadFile(Stream originalImageStream)
        {
            try
            {
                MemoryStream imageStreamCopy = new MemoryStream();
                await originalImageStream.CopyToAsync(imageStreamCopy);
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
                await container.CreateIfNotExistsAsync();
                string blobName = $"{Guid.NewGuid()}.jpg";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                imageStreamCopy.Position = 0;
                await blockBlob.UploadFromStreamAsync(imageStreamCopy);
                string imageUrl = blockBlob.Uri.ToString();
                Console.WriteLine($"URL: {imageUrl}");
                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteFile(string imageUrl)
        {
            try
            {
                Uri uri = new Uri(imageUrl);
                string blobName = Path.GetFileName(uri.LocalPath);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

                if (await blockBlob.DeleteIfExistsAsync())
                {
                    Console.WriteLine($"Arquivo {blobName} excluído com sucesso.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Falha ao excluir o arquivo {blobName}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
                return false;
            }
        }
    }
}
