namespace MoviesAPI.Services
{
    public class FileStorageLocal : IFileStorage // "Ctrl + ." e implementar la interfaz
    {
        private readonly IWebHostEnvironment env; // para obtener ruta de wwwroot
        private readonly IHttpContextAccessor httpContextAccessor; // para determinar el dominio donde esta el webapi

        public FileStorageLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task deleteFile(string route, string storage)
        {
            if(route != null)
            {
                var fileName = Path.GetFileName(route);
                string fileDirectory = Path.Combine(env.WebRootPath, storage, fileName);
                if (File.Exists(fileDirectory))
                {
                    File.Delete(fileDirectory);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> editFile(byte[] content, string extension, string storage, string route, string contentType)
        {
            await deleteFile(route, storage);
            return await saveFile(content, extension, storage, contentType);
        }

        public async Task<string> saveFile(byte[] content, string extension, string storage, string contentType)
        {
            // nombre de archivo
            var fileName = $"{Guid.NewGuid()}{extension}";

            // ruta de archivo
            string folder = Path.Combine(env.WebRootPath, storage);

            // validamos si ya existe directorio
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // guardando imagen
            string route = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(route, content);

            // url, algo como: https://dominiio
            var urlCurrent = $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";
            var urlDB = Path.Combine(urlCurrent, storage, fileName).Replace("\\", "/");
            return urlDB;
        }
    }
}
