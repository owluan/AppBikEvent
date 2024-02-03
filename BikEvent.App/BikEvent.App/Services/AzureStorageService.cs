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
        private const string ContainerName = "imagestorage"; // Substitua pelo nome do seu contêiner

        public async Task<string> UploadFile(Stream originalImageStream)
        {
            try
            {
                // Crie uma cópia do stream original
                MemoryStream imageStreamCopy = new MemoryStream();
                await originalImageStream.CopyToAsync(imageStreamCopy);

                // Conecte-se ao armazenamento do Azure
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Obtenha referência para o contêiner
                CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);

                // Garanta que o contêiner exista
                await container.CreateIfNotExistsAsync();

                // Defina um nome exclusivo para o blob (pode ser um GUID ou algo exclusivo)
                string blobName = $"{Guid.NewGuid()}.jpg";

                // Obtenha referência para o blob no contêiner
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

                // Garanta que o stream esteja na posição correta
                imageStreamCopy.Position = 0;

                // Faça upload do MemoryStream para o blob
                await blockBlob.UploadFromStreamAsync(imageStreamCopy);

                // Obtenha a URL do blob após o upload
                string imageUrl = blockBlob.Uri.ToString();

                Console.WriteLine($"URL: {imageUrl}");

                // Retorne a URL da imagem
                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
                return null;
            }
        }
    }
}
