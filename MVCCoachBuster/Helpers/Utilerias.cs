namespace MVCCoachBuster.Helpers;
public static class Utilerias
{

    public static async Task<string> LeerImagen(IFormFile archivo, IConfiguration configuration)
    {
        var esImagenRutaRelativa = false;
        var rutaDirectorioArchivos = string.Empty;

        bool.TryParse(configuration["rutaRelativaArchivo"], out esImagenRutaRelativa);

        // Si ImagenRutaRelativa significa que esta en desarrollo
        if (esImagenRutaRelativa)
        {
            // rutaDirectorioArchivos = Path.Combine(Directory.GetCurrentDirectory() + configuration["rutaArchivos"]);
            rutaDirectorioArchivos = configuration["rutaArchivos"];

        }
        else
        {
            rutaDirectorioArchivos = configuration["rutaArchivos"];
        }

        bool existeRutaDirectorioArchivos = System.IO.Directory.Exists(rutaDirectorioArchivos);
        if (!existeRutaDirectorioArchivos) Directory.CreateDirectory(rutaDirectorioArchivos);

        var extension = Path.GetExtension(archivo.FileName);
        var nombreArchivo = Path.GetFileNameWithoutExtension(archivo.FileName.Trim());
        var guid = Guid.NewGuid().ToString();
        nombreArchivo = $"{nombreArchivo}_{guid}{extension}";
        var rutaArchivo = Path.Combine(rutaDirectorioArchivos, nombreArchivo);
        if (!System.IO.File.Exists(rutaArchivo))
        {
            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }
            return nombreArchivo;
        }
        return null;
    }

    public static async Task<byte[]> ConvertirImagenABytes(string imagen, IConfiguration configuration)
    {

        var esImagenRutaRelativa = false;
        var rutaDirectorioArchivos = string.Empty;

        bool.TryParse(configuration["rutaRelativaArchivo"], out esImagenRutaRelativa);

        if (esImagenRutaRelativa)
        {
            //rutaDirectorioArchivos = Path.Combine(Directory.GetCurrentDirectory() + configuration["rutaArchivos"]);
            rutaDirectorioArchivos = configuration["rutaArchivos"];

        }
        else
        {
            rutaDirectorioArchivos = configuration["rutaArchivos"];
        }

        bool existeRutaDirectorioArchivos = System.IO.Directory.Exists(rutaDirectorioArchivos);
        if (!existeRutaDirectorioArchivos) Directory.CreateDirectory(rutaDirectorioArchivos);
        var rutaArchivo = Path.Combine(rutaDirectorioArchivos, imagen);
        if (System.IO.File.Exists(rutaArchivo))
        {
            return await System.IO.File.ReadAllBytesAsync(rutaArchivo);
        }
        return null;
    }
}