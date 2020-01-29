<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Error.aspx.vb" Inherits="HCVU._Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="PageTitle" runat="server">eHealth System (Subsidies) - Error Page</title>
    <base id="basetag" href="http://localhost/hcvu/" runat="server" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="JS/Common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(Images/master/banner_header.jpg); width: 990px; background-repeat: no-repeat; height: 100px"
                runat="server">
                <tr>
                    <td class="AppEnvironment" style="vertical-align: top">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <span style="font-size: 16pt;">&nbsp;System error. Please click <a href="login.aspx">here</a> to login again.</span>
    </form>
</body>
</html>
