using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MoviesAPI.Services
{
    public class FileStorageAzure : IFileStorage // "Ctrl + ." e implementar la interfaz
    {
        private readonly string connectionString;

        public FileStorageAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage"); // appsettings.Dev
        }

        public async Task deleteFile(string route, string storage)
        {
            if(string.IsNullOrEmpty(route)) 
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, storage);
            await client.CreateIfNotExistsAsync(); // crear contenedor si no existe
            var file = Path.GetFileName(route);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> editFile(byte[] content, string extension, string storage, string route, string contentType)
        {
            await deleteFile(route, storage);
            return await saveFile(content, extension, storage, contentType);
        }

        public async Task<string> saveFile(byte[] content, string extension, string storage, string contentType)
        {
            var client = new BlobContainerClient(connectionString, storage);
            await client.CreateIfNotExistsAsync(); // crear contenedor si no existe
            client.SetAccessPolicy(PublicAccessType.Blob);

            // nombre de archivo
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);

            // para el contentType
            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeader;

            // insertando en Azure
            await blob.UploadAsync(new BinaryData(content), blobUploadOptions);
            return blob.Uri.ToString(); // ruta de imagen

        }
    }
}
