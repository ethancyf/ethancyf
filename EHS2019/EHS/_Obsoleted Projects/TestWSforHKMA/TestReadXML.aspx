<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestReadXML.aspx.vb"  validateRequest="false" Inherits="TestWSforHKMA.TestReadXML" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td valign="top" >
                        <asp:TextBox ID="txtRequest" runat="server" Height="274px" Width="700px" Font-Names="Tahoma" TextMode="MultiLine"></asp:TextBox></td>
                    <td valign="top" style="width: 118px" >
                        <asp:DropDownList ID="ddlSample" runat="server" AutoPostBack="True">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                <td>
                        <asp:TextBox ID="txtResult" runat="server" Height="274px" Width="700px" TextMode="MultiLine"></asp:TextBox></td>
                <td valign="top" style="width: 118px"><asp:Button ID="BtnQuery" runat="server" Text="Internal Test" Enabled="false"  />
                <asp:Button ID="BtnQuery2" runat="server" Text="WS Test" />
                <asp:Button ID="BtnBack" runat="server" Text="Back" /></td></tr>
            </table>
        </div>
    </form>
</body>
</html>
