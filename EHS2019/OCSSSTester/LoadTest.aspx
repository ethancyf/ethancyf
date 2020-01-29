<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoadTest.aspx.vb" Inherits="OCSSSTester.LoadTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OCSSS Load Test page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtHKID" runat="server" Width="90"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnCallOCSSS" runat="server" Text="Call OCSSS" />
                </td>        
            </tr>
        </table>
        <br />
        <asp:Label ID="lblResultCallOCSSS" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
