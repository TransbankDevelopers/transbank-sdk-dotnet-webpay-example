using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Transbank.Webpay;

namespace TestWebpay
{
    public partial class tbk_capture : Page
    {
        /** Mensaje de Ejecución */
        private string message;

        /** Crea Dictionary con datos de entrada */
        private Dictionary<string, string> request = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            var configuration = Configuration.ForTestingWebpayPlusCapture();

            /** Creacion Objeto Webpay */
            var webpay = new Webpay(configuration);

            /** Información de Host para crear URL */
            var httpHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            var selfURL = HttpContext.Current.Request.ServerVariables["URL"].ToString();
            string action = !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["action"]) ? HttpContext.Current.Request.QueryString["action"] : "init";

            /** Crea URL de Aplicación */
            string sample_baseurl = "http://" + httpHost + selfURL;
            HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 200%;'>Ejemplos Webpay - Transacci&oacute;n Normal Captura Diferida</p>");
            string buyOrder;
            string tx_step = "";

            switch (action)
            {
                default:
                    tx_step = "Init";

                    try
                    {
                        string next_page = sample_baseurl + "?action=capture";
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        HttpContext.Current.Response.Write("<form id='formulario' action="+next_page+" method='post'>");
                        HttpContext.Current.Response.Write("<fieldset>");
                        HttpContext.Current.Response.Write("<legend>Formulario de Captura</legend><br/><br/>");
                        HttpContext.Current.Response.Write("<label>authorizationCode:</label>");
                        HttpContext.Current.Response.Write("<input id='authorizationCode' name='authorizationCode' type='text' />&nbsp;&nbsp;&nbsp;");    
                        HttpContext.Current.Response.Write("<label>captureAmount:</label>");    
                        HttpContext.Current.Response.Write("<input id='captureAmount' name='captureAmount' type='text' />&nbsp;&nbsp;&nbsp;");    
                        HttpContext.Current.Response.Write("<label>buyOrder:</label>");    
                        HttpContext.Current.Response.Write("<input id='buyOrder' name='buyOrder' type='text' />&nbsp;&nbsp;&nbsp;<br/><br/><br/>"); 
                        HttpContext.Current.Response.Write("<input name='enviar' type='submit' value='Enviar' />"); 
                        HttpContext.Current.Response.Write("</fieldset>"); 
                        HttpContext.Current.Response.Write("</form>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;

                case "capture":
                    tx_step = "capture";

                    try
                    {
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        /** Obtiene Información POST */
                        string[] keysNullify = Request.Form.AllKeys;

                        /** Código de autorización de la transacción que se requiere capturar */
                        string authorizationCode = Request.Form["authorizationCode"];

                        /** Monto autorizado de la transacción que se requiere capturar */
                        decimal authorizedAmount = Int64.Parse(Request.Form["captureAmount"]);

                        /** Orden de compra de la transacción que se requiere capturar */
                        buyOrder = Request.Form["buyOrder"];

                        request.Add("authorizationCode", authorizationCode.ToString());
                        request.Add("captureAmount", authorizedAmount.ToString());
                        request.Add("buyOrder", buyOrder.ToString());

                        var result = webpay.CaptureTransaction.capture(authorizationCode, authorizedAmount, buyOrder);
                        request.Add("", "");

                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");
                        message = "Transacci&oacute;n Finalizada";
                        HttpContext.Current.Response.Write(message + "</br></br>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;
            }
            HttpContext.Current.Response.Write("</br><a href='default.aspx'>&laquo; volver a index</a>");
        }
    }
}
