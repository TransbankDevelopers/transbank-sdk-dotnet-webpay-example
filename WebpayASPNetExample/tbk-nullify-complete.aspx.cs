using System;
using System.Collections.Generic;
using System.Web;
using Transbank.Webpay;

namespace TestWebpay
{
    public partial class tbk_nullify_complete : System.Web.UI.Page
    {
        /** Mensaje de Ejecución */
        private string message;

        /** Crea Dictionary con datos Integración Pruebas */
        private Dictionary<string, string> certificate = certificates.CertComplete.certificate();

        /** Crea Dictionary con datos de entrada */
        private Dictionary<string, string> request = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            var configuration = new Configuration()
            {
                Environment = certificate["environment"],
                CommerceCode = certificate["commerce_code"],
                WebpayCertPath = certificate["public_cert"],
                PrivateCertPfxPath = certificate["webpay_cert"],
                Password = certificate["password"]
            };

            /** Creacion Objeto Webpay */
            var webpay = new Webpay(configuration);

            /** Información de Host para crear URL */
            var httpHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            var selfURL = HttpContext.Current.Request.ServerVariables["URL"].ToString();

            string action = !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["action"]) ? HttpContext.Current.Request.QueryString["action"] : "init";

            /** Crea URL de Aplicación */
            string sample_baseurl = "http://" + httpHost + selfURL;
            HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 200%;'>Ejemplos Webpay - Transacci&oacute;n Completa</p>");
            string buyOrder;
            string tx_step = "";

            switch (action)
            {
                default:
                    tx_step = "Init";

                    try
                    {
                        string next_page = sample_baseurl + "?action=nullify";
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        HttpContext.Current.Response.Write("<form id='formulario' action=" + next_page + " method='post'>");
                        HttpContext.Current.Response.Write("<fieldset>");
                        HttpContext.Current.Response.Write("<legend>Formulario de Anulaci&oacute;n</legend><br/><br/>");
                        HttpContext.Current.Response.Write("<label>authorizationCode:</label>");
                        HttpContext.Current.Response.Write("<input id='authorizationCode' name='authorizationCode' type='text' />&nbsp;&nbsp;&nbsp;");
                        HttpContext.Current.Response.Write("<label>authorizedAmount:</label>");
                        HttpContext.Current.Response.Write("<input id='authorizedAmount' name='authorizedAmount' type='text' />&nbsp;&nbsp;&nbsp;");
                        HttpContext.Current.Response.Write("<label>buyOrder:</label>");
                        HttpContext.Current.Response.Write("<input id='buyOrder' name='buyOrder' type='text' /><br/><br/><br/>");
                        HttpContext.Current.Response.Write("<input id='campo3' name='enviar' type='submit' value='Enviar' />");
                        HttpContext.Current.Response.Write("</fieldset>");
                        HttpContext.Current.Response.Write("</form>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;

                case "nullify":
                    tx_step = "nullify";

                    try
                    {
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        /** Obtiene Información POST */
                        string[] keysNullify = Request.Form.AllKeys;

                        /** Codigo de Comercio */
                        string commercecode = certificate["commerce_code"];

                        /** Código de autorización de la transacción que se requiere anular */
                        string authorizationCode = Request.Form["authorizationCode"];

                        /** Monto autorizado de la transacción que se requiere anular */
                        decimal authorizedAmount = Int64.Parse(Request.Form["authorizedAmount"]);

                        /** Orden de compra de la transacción que se requiere anular */
                        buyOrder = Request.Form["buyOrder"];

                        /** Monto que se desea anular de la transacción */
                        decimal nullifyAmount = authorizedAmount;

                        request.Add("authorizationCode", authorizationCode.ToString());
                        request.Add("authorizedAmount", authorizedAmount.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        request.Add("nullifyAmount", nullifyAmount.ToString());
                        request.Add("commercecode", commercecode.ToString());

                        var result = webpay.NullifyTransaction.nullify(authorizationCode, authorizedAmount, buyOrder, nullifyAmount, commercecode);
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");
                        message = "Transacci&oacute;n Finalizada";
                        HttpContext.Current.Response.Write(message + "</br></br>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;
            }
            HttpContext.Current.Response.Write("</br><a href='default.aspx'>&laquo; volver a index</a>");
        }
    }
}
