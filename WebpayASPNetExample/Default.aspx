<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="TestWebpay.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <h1>Ejemplos Webpay</h1>

    <table border="0" style="width:70%">
  <tr>
    <td><h3>Transacci&oacute;n Normal</h3></td>
    <td><h3><a href="tbk-normal.aspx">webpay Normal</a></h3></td> 
    <td><h3><a href="tbk-nullify-normal.aspx">webpay Normal Anulaci&oacute;n </a></h3></td>
  </tr>
  <tr>
    <td><h3>Transacci&oacute;n Mall Normal</h3></td>
    <td><h3><a href="tbk-mall-normal.aspx">webpay Mall Normal</a></h3></td> 
    <td><h3><a href="tbk-nullify-mall-normal.aspx">webpay Mall Normal Anulaci&oacute;n </a></h3></td>
  </tr>
  <tr>
    <td><h3>Transacci&oacute;n Completa</h3></td>
    <td><h3><a href="tbk-complete.aspx">webpay Completa</a></h3></td> 
    <td><h3><a href="tbk-nullify-complete.aspx">Webpay Completa Anulaci&oacute;n </a></h3></td>
  </tr>
  <tr>
    <td><h3>Transacci&oacute;n Captura Diferida</h3></td>
    <td><h3><a href="tbk-normal-capture.aspx">Webpay Captura Diferida</a></h3></td> 
    <td><h3><a href="tbk-capture.aspx">webpay Captura Diferida</a></h3></td>
  </tr>
  <tr>
    <td><h3>Transacci&oacute;n OneClick</h3></td>
    <td><h3><a href="tbk-oneclick.aspx">webpay OneClick</a></h3></td> 
    <td><h3> - </h3></td>
  </tr>
</table>

</body>
</html>


