using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AkvelonTest
{
    public class StorageService : IStorageService
    {
        private readonly BlobContainerClient _client;

        public StorageService(BlobContainerClient client)
        {
            _client = client;
        }

        public Uri GetContainerUri()
        {
            return _client.Uri;
        }

        public async Task<Stream> GetFileAsync(string path)
        {
            try
            {
                var file = _client.GetBlobClient(path);
                if (await file.ExistsAsync())
                {
                    return await file.OpenReadAsync();
                }
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                //_logger.LogError($"File {path} was not found.");
            }

            return null;
        }

        public async Task<bool> SaveFileAsync(string path, Stream stream)
        { 
            try
            {
                BlobClient client = _client.GetBlobClient(path);
                await client.UploadAsync(stream);

                return true;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                //_logger.LogError($"File with name {path} already exists in container.");
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                //_logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
            }

            return false;
        }
    }
}
