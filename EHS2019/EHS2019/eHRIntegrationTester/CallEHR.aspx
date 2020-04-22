<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CallEHR.aspx.vb" Inherits="eHRIntegrationTester.CallEHR" %>

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
                    <td style="width: 300px">
                        <asp:Label ID="lblCallEHR" runat="server" Text="Call EHR"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblVerifySystem" runat="server" Text="VerifySystem"></asp:Label>
                        <asp:LinkButton ID="lbtnVerifySystem" runat="server" Text="VerifySystem" OnClick="lbtnVerifySystem_Click"></asp:LinkButton>
                        &nbsp;
                        <asp:Label ID="lblGetEhrWebS" runat="server" Text="GetEhrWebS"></asp:Label>
                        <asp:LinkButton ID="lbtnGetEhrWebS" runat="server" Text="GetEhrWebS" OnClick="lbtnGetEhrWebS_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <br />
            <asp:MultiView ID="mvFunction" runat="server">
                <asp:View ID="vVerifySystem" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 100px">Mode:</td>
                            <td>
                                <asp:RadioButtonList ID="rblVMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                                    <asp:ListItem Text="WS" Value="WS" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="EMULATE" Value="EMULATE"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px">Endpoint URL:
                            </td>
                            <td>
                                <asp:TextBox ID="txtVEndpointURL" runat="server" Width="500px"></asp:TextBox>
                            </td>
                            <td style="width: 50px"></td>
                            <td style="width: 90px">Load Sample:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVLoadSample" runat="server" Width="300" OnSelectedIndexChanged="ddlVLoadSample_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblVLoadSampleInstruction" runat="server" Font-Size="8pt"></asp:Label></td>
                        </tr>
                        <tr style="height: 5px"></tr>
                    </table>
                    <asp:TextBox ID="txtVRequest" runat="server" EnableViewState="false" Height="250px" TextMode="MultiLine" Width="100%" Text="<Request here>"></asp:TextBox>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 100px">
                                <asp:Button ID="btnVSubmit" runat="server" Text="VerifySystem" OnClick="btnVSubmit_Click" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cboVRemoveWhiteSpace" runat="server" Text="Remove white space before send" Checked="true" />
                            </td>
                            <td style="padding-left: 25px">
                                <asp:RadioButtonList ID="rblVSubmitOption" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Raw" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Beautified" Value="B" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:TextBox ID="txtVResult" runat="server" EnableViewState="false" Height="300px" TextMode="MultiLine" Width="100%" Text="<Result here>"></asp:TextBox>
                </asp:View>
                <asp:View ID="vGetEhrWebS" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 100px">Mode:</td>
                            <td>
                                <asp:RadioButtonList ID="rblGMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                                    <asp:ListItem Text="WS" Value="WS" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="EMULATE" Value="EMULATE"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px">Endpoint URL:
                            </td>
                            <td>
                                <asp:TextBox ID="txtGEndPointURL" runat="server" Width="500px"></asp:TextBox>
                            </td>
                            <td style="width: 50px"></td>
                            <td style="width: 90px">Load Sample:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlGLoadSample" runat="server" Width="300" OnSelectedIndexChanged="ddlGLoadSample_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblGLoadSampleInstruction" runat="server" Font-Size="8pt"></asp:Label></td>
                        </tr>
                        <tr style="height: 5px"></tr>
                    </table>
                    <asp:TextBox ID="txtGRequest" runat="server" EnableViewState="false" Height="250px" TextMode="MultiLine" Width="100%" Text="<Request here>"></asp:TextBox>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnGSubmit" runat="server" Text="GetEhrWebS" OnClick="btnGSubmit_Click" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cboGRemoveWhiteSpace" runat="server" Text="Remove white space before send" Checked="true" />
                            </td>
                            <td style="padding-left: 25px">
                                <asp:RadioButtonList ID="rblGSubmitOption" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Raw" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Beautified" Value="B" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:TextBox ID="txtGResult" runat="server" EnableViewState="false" Height="300px" TextMode="MultiLine" Width="100%" Text="<Result here>"></asp:TextBox>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
