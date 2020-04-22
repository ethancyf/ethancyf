<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="thankyou.aspx.vb" Inherits="VoucherBalanceEnquiry.thankyou" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title id="PageTitle" runat="server"></title>     
     <base id="basetag" runat="server" /> 
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="JS/Common.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        
    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(Images/master/banner_header.jpg);
            width: 994px; background-repeat: no-repeat; height: 100px" runat="server">
            <tr>
                <td align="right" valign="top">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkbtnTradChinese" runat="server" CssClass="languageText">繁體</asp:LinkButton><asp:LinkButton
                                    ID="lnkbtnSimpChinese" runat="server" CssClass="languageText" Visible="false">简体</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkbtnEnglish" runat="server" CssClass="languageText">English</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 990px;">
            <tr style="height: 220px; width: 95%;">
                <td style="background-color: #fcfcfc">
                    <table cellpadding="2" cellspacing="2" style="height: 100%; width: 95%;" align= "center">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="border-left-color: gray; border-right-color: gray; border-top-color: gray; border-bottom-color: gray; border-top-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-style: solid" align="center">
                                <asp:Label ID="lblThankEng" Text="Thank you for using eHealth System (Subsidies)" runat="server" Font-Bold="true" Font-Size="20px"></asp:Label><br /><br /><br />
                                <asp:Label ID="lblThankChi" Text="多謝使用醫健通(資助)系統" runat="server" Font-Bold="true" Font-Size="20px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="background-repeat: no-repeat; height: 8px; background-color: #d6d6d6;
                    font-size: 14px" valign="bottom">
                    <table style="width: 100%">
                        <tr>
                            <td align="left" style="height: 18px">
                                &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnPrivacyPolicy" runat="server" CssClass="footerText" Text="<%$ Resources:Text, PrivacyPolicy %>"></asp:LinkButton>
                                <asp:Label ID="lblseparator1" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                    ID="lnkBtnDisclaimer" runat="server" CssClass="footerText" Text="<%$ Resources:Text, ImportantNotices %>"></asp:LinkButton>
                                <asp:Label ID="lblseparator2" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                    ID="lnkBtnSysMaint" runat="server" CssClass="footerText" Text="<%$ Resources:Text, SysMaint %>"></asp:LinkButton>
                            </td>
                            <td align="right" style="height: 18px">
                                <asp:Label ID="lblFooterCopyright" runat="server" CssClass="footerText" Text="© Copyright Hospital Authority . All rights reserved."
                                    Visible="False"></asp:Label>
                                &nbsp; &nbsp; &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
