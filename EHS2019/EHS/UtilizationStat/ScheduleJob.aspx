<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ScheduleJob.aspx.vb" Inherits="UtilizationStat.ScheduleJob" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Schedule Job</title>
    <link rel="Stylesheet" href="CSS/InterfaceControl.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:MultiView ID="MultiViewCore" runat="server">
                    <asp:View ID="ViewScheduleJobSuspend" runat="server">
                        <strong>eHealth System (Subsidies) - Schedule Job Suspend</strong><br />
                        <br />
                        
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="font-weight: bold">
                                                            Suspend Item</td>
                                                        <td align="right">
                                                            <asp:Label ID="lblSJSOutstandingLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #9900cc">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            Schedule Job:
                                                        </td>
                                                        <td colspan="5">
                                                            <asp:DropDownList ID="ddlSJSID" runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            From:
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtSJSFrom" runat="server" Width="150"></asp:TextBox>
                                                            To &nbsp;<asp:TextBox ID="txtSJSTo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()"></asp:TextBox>
                                                            <asp:LinkButton ID="lbtnSJSNow" runat="server" OnClick="lbtnSJSNow_Click">[Now]</asp:LinkButton></td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Description:</td>
                                                        <td colspan="5">
                                                            <asp:TextBox ID="txtSJSDesc" runat="server" MaxLength="510" Width="478px"></asp:TextBox>
                                                            <span style="color:Gray">(Optional)</span></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td colspan="5">
                                                            <asp:Button ID="btnSJSSubmit" runat="server" Text="Submit" OnClick="btnSJSSubmit_Click" />
                                                            <asp:Label ID="lblErrorSJS" runat="server" ForeColor="Red" Visible="False"></asp:Label></td>
                                                    </tr>
                                                    <tr style="height: 10px">
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvSJSOutstanding" runat="server" AutoGenerateColumns="False" SkinID="IC"
                                                                Width="100%" AllowSorting="True">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Start_Dtm" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Start Time" SortExpression="Start_Dtm">
                                                                        <HeaderStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="End_Dtm" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="End Time" SortExpression="End_Dtm">
                                                                        <HeaderStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SJ_Name" HeaderText="ID" SortExpression="SJ_Name">
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                        <HeaderStyle Width="180px" />
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Create_Dtm" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Create Time" SortExpression="Create_Dtm">
                                                                        <HeaderStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btnSJSDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                Text="Delete" OnClientClick="if(confirm ('Confirm delete?')) { return true } else { return false};" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <span style="color:Red">Red:&nbsp</span>Suspending</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="font-weight: bold">
                                                            Dispatch File</td>
                                                        <td align="right">
                                                            <asp:Label ID="lblSJSControlLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #9900cc">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSJSControlRefresh" runat="server" Text="Refresh" OnClick="btnSJSControlRefresh_Click"  /></td>
                                                    </tr>
                                                    <tr style="height: 10px">
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvSJSControl" runat="server" AutoGenerateColumns="False" SkinID="IC" AllowSorting="True">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Server_Name" HeaderText="Server Name" SortExpression="Server_Name">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SJ_Name" HeaderText="ID" SortExpression="SJ_Name">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Update_Dtm" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Last Update Time" SortExpression="Update_Dtm">
                                                                        <HeaderStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="IsUpToDate" HeaderText="Is Up To Date" SortExpression="IsUpToDate">
                                                                        <HeaderStyle Width="130px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <span style="color: #ff0000"> </span></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="font-weight: bold">
                                                            Suspend
                                                            History</td>
                                                        <td align="right">
                                                            <asp:Label ID="lblSJSHistoryLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #9900cc">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSJSRefreshHistory" runat="server" Text="Refresh" OnClick="btnSJSRefreshHistory_Click" /></td>
                                                    </tr>
                                                    <tr style="height: 10px">
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvSJSHistory" runat="server" AutoGenerateColumns="False" SkinID="IC"
                                                                Width="100%" AllowSorting="True">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Start_Dtm" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Start Time" SortExpression="Start_Dtm">
                                                                        <HeaderStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="End_Dtm" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="End Time" SortExpression="Start_Dtm">
                                                                        <HeaderStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SJ_Name" HeaderText="ID" SortExpression="SJ_Name">
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                        <HeaderStyle Width="180px" />
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Create_Dtm" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Create Time" SortExpression="Create_Dtm">
                                                                        <HeaderStyle Width="130px" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btnSJSDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                Text="Delete" OnClientClick="if(confirm ('Confirm delete?')) { return true } else { return false};" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:View>
                </asp:MultiView>
                <br />
                <br />
                <asp:Button ID="btnBack" runat="server" Text="Back" Width="100" OnClick="btnBack_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
        </div>
    </form>
</body>
</html>
