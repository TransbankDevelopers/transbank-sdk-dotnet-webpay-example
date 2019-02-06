using System;
using System.Collections.Generic;
using System.Web;
using Transbank.Webpay;

namespace TestWebpay
{
    public partial class tbk_complete : System.Web.UI.Page
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
                //TbkPublicCertPath = certificate["public_cert"],
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
            string token;
            string next_page;

            string tx_step = "";

           switch (action)
            {

                default:

                    tx_step = "Init";

                    try
                    {

                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        next_page = sample_baseurl + "?action=queryshare";

                        var random = new Random();

                        /** Monto de la transacción */
                        decimal amount = 9990;

                        /** Orden de compra de la tienda */
                        buyOrder = random.Next(0, 1000).ToString();

                        /** (Opcional) Identificador de sesión, uso interno de comercio */
                        string sessionId = random.Next(0, 1000).ToString();

                        /** Fecha de expiración de tarjeta, formato YY/MM */
                        string cardExpirationDate = "18/04";

                        /** Código de verificación de la tarjeta */
                        int cvv = 123;

                        /** Número de la tarjeta */
                        string cardNumber = "4051885600446623";

                        request.Add("amount", amount.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        request.Add("sessionId", sessionId.ToString());
                        request.Add("cardExpirationDate", cardExpirationDate.ToString());
                        request.Add("cvv", cvv.ToString());
                        request.Add("cardNumber", cardNumber.ToString());

                        var result = webpay.CompleteTransaction.initCompleteTransaction(amount, buyOrder, sessionId, cardExpirationDate, cvv, cardNumber);

                        /** Verificamos respuesta de inicio en webpay */
                        if (result.token != null && result.token != "")
                        {
                            message = "Sesion iniciada con exito en Webpay";

                        }
                        else
                        {
                            message = "webpay no disponible";
                        }

                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        HttpContext.Current.Response.Write("" + message + "</br></br>");

                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'>");
                        HttpContext.Current.Response.Write("<input type='hidden' name='token_ws' value=" + result.token + ">");
                        HttpContext.Current.Response.Write("<input type='hidden' name='buyOrder' value=" + buyOrder + ">");
                        HttpContext.Current.Response.Write("<input type='submit' value='Continuar &raquo;'>");
                        HttpContext.Current.Response.Write("</form>");

                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }

                    break;

                case "queryshare":

                    tx_step = "Queryshare";

                    try
                    {

                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        next_page = sample_baseurl + "?action=authorize";

                        /** Obtiene Información POST */
                        string[] keysPost = Request.Form.AllKeys;

                        /** Token de la transacción */
                        token = Request.Form["token_ws"];

                        /** Orden de compra de la transacción  */
                        buyOrder = Request.Form["buyOrder"];

                        /** Número de cuotas */
                        string shareNumber = "2";

                        request.Add("token", token.ToString());
                        request.Add("shareNumber", shareNumber.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        
                        var result = webpay.CompleteTransaction.queryShare(token, buyOrder, shareNumber);

                        /** Verificamos respuesta de inicio en webpay */
                        if (result.token != null && result.token != "")
                        {
                            message = "Transacci&oacute;n realizada con exito en Webpay";
                        }
                        else
                        {
                            message = "webpay no disponible";
                        }

                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        HttpContext.Current.Response.Write("" + message + "</br></br>");

                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'>");

                         HttpContext.Current.Response.Write("<input type='hidden' name='buyOrder' value=" + buyOrder + ">");
                         HttpContext.Current.Response.Write("<input type='hidden' name='token_ws' value=" + token + ">");
                         HttpContext.Current.Response.Write("<input type='hidden' name='queryId' value=" + result.queryId + ">");
                         HttpContext.Current.Response.Write("<input type='submit' value='Continuar &raquo;'>");
                         HttpContext.Current.Response.Write("</form>");

                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }

                    break;

                case "authorize":

                    tx_step = "Authorize";

                    try
                    {

                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        next_page = sample_baseurl + "?action=end";

                        /** Obtiene Información POST */
                        string[] keysPost = Request.Form.AllKeys;

                        /** Token de la transacción */
                        token = Request.Form["token_ws"];

                        /** Orden de compra de la transacción */
                        buyOrder =  Request.Form["buyOrder"];

                        /** (Opcional) Flag que indica si aplica o no periodo de gracia */
                        bool gracePeriod = false;

                        /** Identificador de la consulta de cuota */
                        long queryShare = Convert.ToInt64(Request.Form["queryId"]);

                        /** (Opcional) Lista de contiene los meses en los cuales se puede diferir el pago, y el monto asociado a cada periodo */
                        int deferredPeriodIndex = 1;

                        request.Add("token", token.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        request.Add("gracePeriod", gracePeriod.ToString());
                        request.Add("queryShare", queryShare.ToString());
                        request.Add("deferredPeriodIndex", deferredPeriodIndex.ToString());

                        var result = webpay.CompleteTransaction.authorize(token, buyOrder, gracePeriod, queryShare, deferredPeriodIndex);

                        /** Verificamos respuesta de inicio en webpay */
                        if (result.detailsOutput[0].responseCode == 0)
                        {

                            message = "Transacci&oacute;n realizada con exito en Webpay";

                        } else {

                            message = "webpay no disponible";

                        }

                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        HttpContext.Current.Response.Write("" + message + "</br></br>");
                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='buyOrder' value=" + buyOrder + "><input type='hidden' name='token_ws' value=" + token + "><input type='hidden' name='authorizationCode' value=" + result.detailsOutput[0].authorizationCode + "><input type='submit' value='Continuar &raquo;'></form>");

                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }

                    break;

                case "end":

                    tx_step = "End";

                    try
                    {

                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        next_page = sample_baseurl + "?action=nullify";

                        /** Obtiene Información POST */
                        string[] keysPost = Request.Form.AllKeys;

                        /** Token de la transacción */
                        token = Request.Form["token_ws"];

                        /** Orden de compra de la transacción */
                        buyOrder = Request.Form["buyOrder"];

                        /** Identificador de la consulta de cuota */
                        string idQueryShare = Request.Form["idQueryShare"];
                        
                        /** Codigo de Autorización */
                        int authorizationCode = Int32.Parse(Request.Form["authorizationCode"]);

                        request.Add("", "");
                        message = "Transacci&oacute;n Finalizada";

                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(token) + "</p>");

                        HttpContext.Current.Response.Write("" + message + "</br></br>");
                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='buyOrder' value=" + buyOrder + "><input type='hidden' name='token_ws' value=" + token + "><input type='hidden' name='idQueryShare' value=" + idQueryShare + "><input type='hidden' name='authorizationCode' value=" + authorizationCode + "><input type='submit' value='Anular Transacci&oacute;n &raquo;'></form>");

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
                        decimal authorizedAmount = 9990;

                        /** Orden de compra de la transacción que se requiere anular */
                        buyOrder = Request.Form["buyOrder"];

                        /** Monto que se desea anular de la transacción */
                        decimal nullifyAmount = 9990;

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
