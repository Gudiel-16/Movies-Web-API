namespace MoviesAPI.Services
{
    public interface IFileStorage
    {
        // storage: contenedor, carpeta de datos
        Task<string> saveFile(byte[] content, string extension, string storage, string contentType);
        Task<string> editFile(byte[] content, string extension, string storage, string route, string contentType);
        Task deleteFile(string route, string storage);
    }
}
