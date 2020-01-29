<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="error.aspx.vb" Inherits="VoucherBalanceEnquiry.Text.ZH._error" ContentType="text/html; charset=UTF-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>醫健通(資助)系統 - 系統錯誤</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>  
</head>
<body>
 <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server">
                <tr>
                    <td style="width: 240px">
                        <asp:Label ID="Label1" runat="server" Text="醫健通(資助)系統" Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 240px">
                        <span style="font-size: 12pt;">系統發生錯誤。

                            <br />
                            請按 <a href="../main.aspx">此處</a> 重新登入。</span></td>
                </tr>
            </table>
        </div>
        <br />
    </form>
</body>
</html>
