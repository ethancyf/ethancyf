<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="invalidlink.aspx.vb" Inherits="PrefillConsent.zh.invalidlink" ContentType="text/html; charset=UTF-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>�尷�q(��U)�t�� - ���������</title>
    <base id="basetag" runat="server" /> 
    <link href="../CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/Common.js"></script>
</head>
<body onload=setTimeout("location.href='../zh/index.aspx'",15000)>
    <form id="form1" runat="server">
    <div>
        <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header_tw.jpg);
            width: 990px; background-repeat: no-repeat; height: 100px" runat="server">
            <tr>
                <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironmentZH">
                    <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <span style="font-size: 16pt;">����������C <br /> �t�αN��15���۰ʪ�^<a href="../zh/index.aspx">����</a>�C</span>
    </form>
</body>
</html>