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
                <tr style="height:35px">
                    <td style="width: 200px">Token Endpoint URL:</td>
                    <td style="width: 500px">
                        <asp:TextBox ID="txtEndpointURL" runat="server" Width="450px"></asp:TextBox>
                    </td>
                    <td style="width: 50px"></td>
                    <td style="width: 100px">Load Sample:</td>
                    <td style="width: 300px">
                        <asp:DropDownList ID="ddlLoadSample" runat="server" Width="290" OnSelectedIndexChanged="ddlLoadSample_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <br /><asp:Label ID="lblLoadSampleInstruction" runat="server" Font-Size="8pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Patient Portal Endpoint URL:</td>
                    <td>
                        <asp:TextBox ID="txtPatientPortalEndpointURL" runat="server" Width="450px"></asp:TextBox>
                    </td>
                    <td style="width: 50px"></td>
                    <td>Load Sample:</td>
                    <td><asp:DropDownList ID="ddlPPLoadSample" runat="server" Width="290" OnSelectedIndexChanged="ddlPPLoadSample_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <br /><asp:Label ID="lblPPLoadSampleInstruction" runat="server" Font-Size="8pt" /></td>
                    <td style="vertical-align:top">
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">DHC Endpoint URL:</td>
                    <td style="width: 500px">
                        <asp:TextBox ID="txtDHCEndpointURL" runat="server" Width="450px"></asp:TextBox>
                    </td>
                    <td style="width: 50px"></td>
                    <td style="width: 100px">Load Sample:</td>
                    <td style="width: 300px"> <asp:DropDownList ID="ddlDHCLoadSample" runat="server" Width="290" OnSelectedIndexChanged="ddlDHCLoadSample_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <br /><asp:Label ID="lblDHCLoadSampleInstruction" runat="server" Font-Size="8pt"></asp:Label>

                    </td>
                </tr>
                </table>
            <asp:TextBox ID="txtRequest" runat="server" EnableViewState="false" Height="250px" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
            <br />
            <br />
            <%--<asp:TextBox ID="txtByte" runat="server" EnableViewState="false" Height="300px" TextMode="MultiLine" Width="100%"></asp:TextBox>--%>
            <asp:Button ID="btnDownload" runat="server" Text="DownloadZip" OnClick="btnDownload_Click" Visible="false" /><br/>
            <asp:Label ID="lblMessage" runat="server"/>
            <asp:Button ID="btnReadFileToByte" runat="server" Text="Read File to Byte" OnClick="btnReadFileToByte_Click" Visible="false" /><br/>
        </div>
    </form>
</body>
</html>
