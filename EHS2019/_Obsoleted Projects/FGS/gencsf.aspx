<%  Response.CacheControl = "no-cache"%>
<%  Response.AddHeader("Pragma", "no-cache")%>
<%  Response.Expires = -1%>

<%@ Page Language="vb" AutoEventWireup="false" Codebehind="gencsf.aspx.vb" Inherits="ConsentFormEHS.gencsf" %>

<%@ Register Assembly="ActiveReports.Web, Version=5.2.1013.2, Culture=neutral, PublicKeyToken=cc4967777c49a3ff"
    Namespace="DataDynamics.ActiveReports.Web" TagPrefix="ActiveReportsWeb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    * {
        font-family: Arial;
        font-size: 12pt;
    }
    .Background {
        background-image: url('Image/background.png');
        background-position: right top;
        background-repeat: no-repeat;
        background-color: #D2D2D2;
    }
    </style>

    <script type="text/javascript">
    function resizeMyselfConsentForm()
    {
        var width = 1020;
        var height = 500;
        
        var left = parseInt((screen.availWidth / 2) - (width / 2));
        var top = parseInt((screen.availHeight / 2) - (height / 2));

        window.resizeTo(width, height);
        window.moveTo(left, top);
    }
    
    function resizeMyselfPassword()
    {
        var width = 700;
        var height = 250;
        
        var left = parseInt((screen.availWidth / 2) - (width / 2));
        var top = parseInt((screen.availHeight / 2) - (height / 2));

        window.resizeTo(width, height);
        window.moveTo(left, top);
        
        document.getElementById('txtPassword').focus();
    }
    
    function resizeMyselfError()
    {
        var width = 700;
        var height = 250;
        
        var left = parseInt((screen.availWidth / 2) - (width / 2));
        var top = parseInt((screen.availHeight / 2) - (height / 2));

        window.resizeTo(width, height);
        window.moveTo(left, top);
    }
    </script>

</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
        <div>
            <asp:MultiView ID="mvGenerate" runat="server">
                <asp:View ID="ViewInputPassword" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" class="Background">
                        <tr style="height: 140px">
                            <td style="vertical-align: top">
                                <asp:Image ID="imgBannerPassword" runat="server" />
                            </td>
                        </tr>
                        <tr style="height: 300px">
                            <td style="text-align: center; vertical-align: top">
                                <asp:Panel ID="panInputPassword" runat="server" DefaultButton="ibtnGenerate">
                                    <center>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPassword" runat="server" Text="[Please enter the password for the consent form:]"></asp:Label>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPassword" runat="server" Width="150px" TextMode="Password">
                                                    </asp:TextBox>
                                                    <asp:Button ID="ibtnGenerate" runat="server" Text="[Submit]" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="lblPasswordError" runat="server" Text="* Password cannot be empty"
                                                        ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewErrorPage" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0" class="Background">
                        <tr style="height: 140px">
                            <td style="vertical-align: top">
                                <asp:Image ID="imgBannerError" runat="server" />
                            </td>
                        </tr>
                        <tr style="height: 300px">
                            <td style="vertical-align: top; padding-left: 10px">
                                <table style="width: 670px; border: solid 1px #FF0000; background-color: #FFE0C0"
                                    cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-bottom: 5px">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 28px">
                                                        <img src="Image/icon_caution.gif" alt="" />
                                                    </td>
                                                    <td style="color: #636563; font-weight: bold">
                                                        <asp:Label ID="lblValidationFail" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="color: #FF0000">
                                                        <asp:Label ID="lblInvalidInputInformation" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
