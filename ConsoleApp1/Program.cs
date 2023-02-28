using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (X509Certificate2 pubOnly = new X509Certificate2("myCert.crt"))
            using (X509Certificate2 pubPrivEphemeral = pubOnly.CopyWithPrivateKey(privateKey))
            {
                // Export as PFX and re-import if you want "normal PFX private key lifetime"
                // (this step is currently required for SslStream, but not for most other things
                // using certificates)
                return new X509Certificate2(pubPrivEphemeral.Export(X509ContentType.Pfx));
            }



            // Cargar certificado digital y clave privada desde archivos
            var certificado = new X509Certificate2("ruta/al/certificado.crt");
            var clavePrivada = new X509Certificate2("ruta/a/claveprivada.key", "contraseña");

            // Crear objeto PfxExportParameters
            var pfxExport = new PfxExportParameters(clavePrivada.PrivateKey);

            // Establecer contraseña para el archivo PFX
            pfxExport.Password = "contraseña del archivo PFX";

            // Agregar certificado digital y clave privada a un objeto ContentInfo
            var contentInfo = new ContentInfo(certificado.RawData);

            // Crear objeto EnvelopedCms y cifrar el contenido con la clave privada
            var envolventeCms = new EnvelopedCms(contentInfo);
            var destinatario = new CmsRecipient(clavePrivada);
            envolventeCms.Encrypt(destinatario);

            // Generar archivo PFX
            var pfxBytes = envolventeCms.Encode(pfxExport);

            // Guardar archivo PFX en disco
            System.IO.File.WriteAllBytes("ruta/al/archivo.pfx", pfxBytes);
        }
    }
}
