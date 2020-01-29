<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CallEHS.aspx.vb" Inherits="eHRIntegrationTester.CallEHS" %>

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
                        <asp:Label ID="lblCallEHS" runat="server" Text="Call EHS"></asp:Label>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnCallEHR" runat="server" Text="Call EHR" PostBackUrl="~/CallEHR.aspx"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <br />
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100px">Endpoint URL:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndpointURL" runat="server" Width="500px"></asp:TextBox>
                    </td>
                    <td style="width: 50px"></td>
                    <td style="width: 90px">Load Sample:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLoadSample" runat="server" Width="300" OnSelectedIndexChanged="ddlLoadSample_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblLoadSampleInstruction" runat="server" Font-Size="8pt"></asp:Label></td>
                </tr>
                <tr style="height: 5px"></tr>
            </table>
            <asp:TextBox ID="txtRequest" runat="server" EnableViewState="false" Height="200px" TextMode="MultiLine" Width="100%"></asp:TextBox>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100px">
                        <asp:Button ID="btnSubmitRequest" runat="server" Text="SubmitRequest" OnClick="btnSubmitRequest_Click" />
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblSubmitOption" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Raw" Value="R"></asp:ListItem>
                            <asp:ListItem Text="Beautified" Value="B" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:TextBox ID="txtResult" runat="server" EnableViewState="false" Height="300px" TextMode="MultiLine" Width="100%"></asp:TextBox>
        </div>
    </form>
</body>
</html>
