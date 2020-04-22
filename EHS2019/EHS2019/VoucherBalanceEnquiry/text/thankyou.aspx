<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="thankyou.aspx.vb" Inherits="VoucherBalanceEnquiry.thankyou1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title id="PageTitle" runat="server"></title>    
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>  
</head>
<body>
    <form id="form1" runat="server"  autocomplete="off">
    <div>
            <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <asp:Label Font-Bold="False" ID="lblBannerText" runat="server" Height="16px" Text="<%$ Resources:Text, EVoucherSystem %>"></asp:Label></td>
                </tr>              
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkbtnTradChinese" runat="server">繁體</asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnEnglish" runat="server" Enabled="False">English</asp:LinkButton></td>
                </tr>
                <tr>
                    <td bgcolor="silver">
                        <asp:Label ID="lblHeaderText" runat="server" Font-Underline="False" Font-Bold="False"
                            Text="<%$ Resources:Text, VoucherEnquiry %>" BackColor="Transparent"></asp:Label></td>
                </tr>
               <tr>
                    <td>
                        <br /><asp:Label ID="lblThankEng" Text="Thank you for using eHealth System (Subsidies)" runat="server"  CssClass="labelText" ></asp:Label><br /><br />
                        <asp:Label ID="lblThankChi" Text="多謝使用醫健通(資助)系統" runat="server"  CssClass="labelText" ></asp:Label>
                        </td>
                </tr> 
            </table>            
        </div>
    </form>
</body>
</html>
