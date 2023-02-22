namespace AkvelonTest
{
    public interface IStorageService
    {
        public Task<Stream> GetFileAsync(string path);
        public Task<bool> SaveFileAsync(string path, Stream stream);

        public Uri GetContainerUri();
    }
}
