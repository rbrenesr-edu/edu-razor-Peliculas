namespace Peliculas.Server.Helpers
{
    public class AlmacenadorArchivosLocalStorage : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AlmacenadorArchivosLocalStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task EliminarArchivo(string ruta, string nombreContenedor)
        {

            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(env.WebRootPath, nombreContenedor, nombreArchivo);

            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);               
            }     
            

            return Task.CompletedTask;
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor)
        {
            var archivoNombre = $"{Guid.NewGuid()}{extension}";
            var folder = Path.Combine(env.WebRootPath, nombreContenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }


            var rutaGuardado = Path.Combine(folder, archivoNombre);
            await File.WriteAllBytesAsync(rutaGuardado, contenido);

            var urlActual = $"{httpContextAccessor!.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var rutaParaBD = Path.Combine(urlActual, nombreContenedor, archivoNombre).Replace("\\","/");

            return rutaParaBD;
        }
    }
}
