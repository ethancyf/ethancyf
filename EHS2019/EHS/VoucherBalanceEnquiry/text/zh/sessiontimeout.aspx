<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="sessiontimeout.aspx.vb" Inherits="VoucherBalanceEnquiry.Text.ZH.sessiontimeout" ContentType="text/html; charset=UTF-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>醫健通(資助)系統 - 系統逾時</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>  
</head>
<body>
 <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server" style="width: 240px">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="醫健通(資助)系統"
                            Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">系統逾時。


                        <br />
                            請按 <a href="../main.aspx">
                            此處</a> 重新登入。</span></td>
                </tr>
            </table>
        </div>
        <br />
    </form>
</body>
</html>
