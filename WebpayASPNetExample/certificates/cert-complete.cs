using System;
using System.Collections.Generic;

namespace TestWebpay.certificates
{
    public class CertComplete
    {
        internal static Dictionary<string, string> certificate()
        {
            /** Crea un Dictionary para almacenar los datos de integración pruebas */
            var certificate = new Dictionary<string, string>();

            /** Agregar datos de integración a Dictionary */

            var certFolder = System.Web.HttpContext.Current.Server.MapPath(".");

            /** Modo de Utilización */
            certificate.Add("environment", "INTEGRACION");

            /** Certificado Publico (Dirección fisica de certificado o contenido) */
            certificate.Add("public_cert", certFolder + "\\certificates\\597020000545\\tbk.pem");

            /** Ejemplo de Ruta de Certificado de Salida */
            certificate.Add("webpay_cert", certFolder + "\\certificates\\597020000545\\597020000545.pfx");

            /** Ejemplo de Password de Certificado de Salida */
            certificate.Add("password", "transbank123");

            /** Codigo Comercio */
            certificate.Add("commerce_code", "597020000545");

            return certificate;
        }
    }
}
