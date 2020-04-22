<%@ Page Language="vb" AutoEventWireup="false" Codebehind="error.aspx.vb" Inherits="HCSP.Text.EN._error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eHealth System (Subsidies) - Error Page</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server" style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="eHealth System (Subsidies)"
                            Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">System error.
                        <br />
                        Please click <a href="../login.aspx">here</a>
                            to back to login again.</span></td>
                </tr>
            </table>
        </div>
        <br />
    </form>
</body>
</html>
