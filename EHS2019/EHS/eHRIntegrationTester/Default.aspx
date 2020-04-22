<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="eHRIntegrationTester._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td style="width: 100px">
                        <asp:LinkButton ID="lbtnCallEHS" runat="server" Text="Call EHS" PostBackUrl="~/CallEHS.aspx"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnCallEHR" runat="server" Text="Call EHR" PostBackUrl="~/CallEHR.aspx"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
