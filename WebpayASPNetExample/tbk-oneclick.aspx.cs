using System;
using System.Collections.Generic;
using System.Web;
using Transbank.Webpay;

namespace TestWebpay
{
    public partial class tbk_oneclick : System.Web.UI.Page
    {
        /** Mensaje de Ejecución */
        private string message;

        /** Crea Dictionary con datos de entrada */
        private Dictionary<string, string> request = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            var configuration = Configuration.ForTestingWebpayOneClickNormal();

            /** Creacion Objeto Webpay */
            var webpay = new Webpay(configuration);

            /** Información de Host para crear URL */
            var httpHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            var selfURL = HttpContext.Current.Request.ServerVariables["URL"].ToString();
            string action = !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["action"]) ? HttpContext.Current.Request.QueryString["action"] : "init";

            /** Crea URL de Aplicación */
            string sample_baseurl = "http://" + httpHost + selfURL;

            HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 200%;'>Ejemplos Webpay - Transacci&oacute;n OneClick</p>");
            var random = new Random();

            /** Nombre de usuario o cliente en el sistema del comercio */
            string username = "usuario";

            /** Dirección de correo electrónico registrada por el comercio */
            string email = "usuario@email.cl";
            string buyOrder;
            string tx_step = "";

            switch (action)
            {
                default:
                    tx_step = "Init";

                    try
                    {
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        string urlReturn = sample_baseurl + "?action=OneClickFinishInscription";

                        request.Add("username", username.ToString());
                        request.Add("email", email.ToString());
                        request.Add("urlReturn", urlReturn.ToString());

                        var result = webpay.OneClickTransaction.initInscription(username, email, urlReturn);

                        /** Verificamos respuesta de inicio en webpay */
                        if (result.token != null && result.token != "") 
                        {
                            message = "Sesion iniciada con exito en Webpay";
                        } else {
                            message = "webpay no disponible";
                        }

                        string token = result.token;
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");
                        HttpContext.Current.Response.Write("" + message + "</br></br>");
                        HttpContext.Current.Response.Write("<form action=" + result.urlWebpay + " method='post'><input type='hidden' name='TBK_TOKEN' value=" + token + "><input type='submit' value='Continuar &raquo;'></form>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;

                case "OneClickFinishInscription":
                    tx_step = "OneClickFinishInscription";
                    HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                    string next_page = sample_baseurl + "?action=OneClickAuthorize";

                    try
                    {
                        /** Obtiene Información POST */
                        string[] keysPost = Request.Form.AllKeys;

                        /** Token de la transacción */
                        string token = Request.Form["TBK_TOKEN"];
                        request.Add("token", token.ToString());
                        var result = webpay.OneClickTransaction.finishInscription(token);

                        if (result.responseCode != 0) {
                            message = "Transacci&oacute;n RECHAZADO por webpay";
                        } else {
                            message = "Transacci&oacute;n ACEPTADA por webpay";
                        }

                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");
                        HttpContext.Current.Response.Write("" + message + "</br></br>");
                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='tbkUser' value=" + result.tbkUser + "><input type='submit' value='Continuar &raquo;'></form>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;

                case "OneClickAuthorize":
                    tx_step = "OneClickAuthorize";

                    try
                    {
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        next_page = sample_baseurl + "?action=OneClickReverse";

                        /** Usuario Transbank */
                        string tbkUser = Request.Form["tbkUser"];
                        
                        /** Monto del pago en pesos */
                        string amount = random.Next(0, 1000).ToString();

                        var today = DateTime.Now;
                        
                        /** Identificador único de la compra generado por el comercio */
                        buyOrder = today.ToString("yyyyMMddHHmmss") + random.Next(0, 1000).ToString();

                        request.Add("buyOrder", buyOrder);
                        request.Add("tbkUser", tbkUser);
                        request.Add("username", username);
                        request.Add("amount", amount);

                        var result = webpay.OneClickTransaction.authorize(buyOrder, tbkUser, username, amount);

                        /** Verificamos respuesta de inicio en webpay */
                        if (result.responseCode == 0)
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
                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='tbkUser' value=" + tbkUser + "><input type='hidden' name='buyOrder' value=" + buyOrder + "><input type='submit' value='Continuar &raquo;'></form>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;

                case "OneClickReverse":
                    tx_step = "OneClickReverse";

                    try
                    {
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        next_page = sample_baseurl + "?action=OneClickFinal";

                        /** Usuario Transbank */
                        buyOrder = Request.Form["buyOrder"];
                        request.Add("buyOrder", buyOrder);
                        var result = webpay.OneClickTransaction.reverseTransaction(buyOrder);

                        /** Verificamos respuesta de inicio en webpay */
                        if (result.reversed == true)
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
                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='tbkUser' value=" + Request.Form["tbkUser"] + "><input type='submit' value='Continuar &raquo;'></form>");
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                    }
                    break;

                case "OneClickFinal":
                    tx_step = "Get removeUser";

                    try
                    {
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        /** Usuario Transbank */
                        string tbkUser = Request.Form["tbkUser"];

                        request.Add("tbkUser", tbkUser);
                        request.Add("username", username);

                        bool result = webpay.OneClickTransaction.oneClickremoveUserOutput(tbkUser, username);

                        /** Verificamos respuesta de inicio en webpay */
                        if (result == true)
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
