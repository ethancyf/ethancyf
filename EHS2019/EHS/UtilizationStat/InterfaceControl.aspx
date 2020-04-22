<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InterfaceControl.aspx.vb"
    Inherits="UtilizationStat.InterfaceControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/ucClearCacheRequestView.ascx" TagName="ucClearCacheRequestView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eHealth System (Subsidies) - Interface Control Webpage</title>
    <link rel="Stylesheet" href="CSS/InterfaceControl.css" />
    <style type="text/css">
        table.Call th {
            background-color: #C0C0C0;
        }

        table.Call td {
            background-color: #FFF;
            padding: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnDRDownloadFile" />
            </Triggers>
            <ContentTemplate>
                <asp:MultiView ID="MultiViewCore" runat="server" OnActiveViewChanged="MultiViewCore_ActiveViewChanged">
                    <asp:View ID="ViewLogin" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <strong>eHealth System (Subsidies) - Interface Control Web Page</strong>
                                </td>
                                <td>
                                    <a href="UtitlizationSummary.aspx">Utilization Summary</a>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Label runat="server" ID="lblErrorL" ForeColor="Red" Visible="False"></asp:Label>
                        <br />
                        <table border="0" cellpadding="5" cellspacing="1">
                            <tr>
                                <td style="width: 120px">
                                    <asp:Label runat="server" ID="lblStaffIDLText" Text="Staff ID"></asp:Label>
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox runat="server" ID="txtStaffIDL" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblPasswordLText" Text="Password"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPasswordL" MaxLength="20" TextMode="Password"
                                        Width="150px" autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnLogin" runat="server" Text="Login" Width="100" OnClick="btnLogin_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewLoginChangePassword" runat="server">
                        <strong>eHealth System (Subsidies) - Interface Control Web Page</strong>
                        <br />
                        <br />
                        <table border="0" cellpadding="5" cellspacing="1">
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblMessageCP" Text="For the first time login, you must set a new password."></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblErrorCP" ForeColor="Red" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td style="width: 180px">
                                    <asp:Label runat="server" ID="lblStaffIDCPText" Text="Staff ID"></asp:Label>
                                </td>
                                <td style="width: 200px">
                                    <asp:Label runat="server" ID="lblStaffIDCP"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td>
                                    <asp:Label runat="server" ID="lblPasswordACPText" Text="New Password"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPasswordACP" MaxLength="20" TextMode="Password"
                                        Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td>
                                    <asp:Label runat="server" ID="lblPasswordBCPText" Text="Confirm New Password"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtPasswordBCP" MaxLength="20" TextMode="Password"
                                        Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSetPassword" runat="server" Text="Set Password" OnClick="btnSetPassword_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewMenu" runat="server">
                        <strong>eHealth System (Subsidies) - Interface Control Web Page</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Menu
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #009933"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td style="width:180px">
                                                                        <asp:Label runat="server" ID="lblEVaccCheck" Text="eVaccination Check"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton ID="lbtnCMSEVaccCheck" runat="server" Text="[CMS]" OnClick="lbtnCMSEVaccCheck_Click"></asp:LinkButton>
                                                                        &nbsp; &nbsp;
                                                                        <asp:LinkButton ID="lbtnDHCIMSEvaccCheck" runat="server" Text="[DH CIMS]" OnClick="lbtnDHCIMSEVaccCheck_Click"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 40px">
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblEVaccControl" Text="eVaccination Control"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:LinkButton ID="lbtnCMSEVaccControl" runat="server" Text="[CMS]" OnClick="lbtnCMSEVaccControl_Click"></asp:LinkButton>
                                                                        &nbsp; &nbsp;
                                                                        <asp:LinkButton ID="lbtnDHCIMSEVaccControl" runat="server" Text="[DH CIMS]" OnClick="lbtnDHCIMSEVaccControl_Click"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnPPIePRControl" runat="server" Text="TSW Patient List Control" OnClick="lbtnPPIePRControl_Click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnTokenServerControl" runat="server" Text="Token Server Control"
                                                                OnClick="lbtnTokenServerControl_Click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnEHRControlSite" runat="server" Text="eHR - Control Sites"
                                                                OnClick="lbtnEHRControlSite_Click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnEHREnquireEHR" runat="server" Text="eHR - Enquire eHR"
                                                                OnClick="lbtnEHREnquireEHR_Click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnEHRTraceOutSyncRecord" runat="server" Text="eHR - Trace Outsync Records"
                                                                OnClick="lbtnEHRTraceOutSyncRecord_Click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnOCSSSControlSite" runat="server" Text="OCSSS - Control Sites"
                                                                OnClick="lbtnOCSSSControlSite_Click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnOCSSSEnquireOCSSS" runat="server" Text="OCSSS - Enquire OCSSS"
                                                                OnClick="lbtnOCSSSEnquireOCSSS_Click"></asp:LinkButton>
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
                        <strong>eHealth System (Subsidies) - Schedule Job</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Menu
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #009933"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnScheduleJobSuspend" runat="server" Text="Schedule Job Suspend"
                                                                OnClick="lbtnScheduleJobSuspend_Click"></asp:LinkButton>
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
                        <strong>eHealth System (Subsidies) - Download Report and Data File</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Menu
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #009933"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnDownloadReport" runat="server" Text="Download Report and Data File"
                                                                OnClick="lbtnDownloadReport_Click"></asp:LinkButton>
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
                        <strong>eHealth System (Subsidies) - Connection String Tester</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Menu
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #009933"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 40px">
                                                        <td>
                                                            <asp:LinkButton ID="lbtnConnectionStringTester" runat="server" Text="Connection String Tester"
                                                                OnClick="lbtnConnectionStringTester_Click"></asp:LinkButton>
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
                        <br />
                        <asp:Button ID="btnLogout" runat="server" Text="Logout" Width="100" OnClick="btnLogout_Click"></asp:Button>
                    </asp:View>
                    <asp:View ID="ViewEVaccCheck" runat="server">
                        <strong>HA CMS eVaccination Check</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">Summary
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblECSLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <asp:Panel ID="pnlCMSSites" runat="server">
                                                </asp:Panel>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECSFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECSTo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnECSRefreshAll" runat="server" Text="Refresh All" OnClick="btnECSRefreshAll_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorECS" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblECSContent1" runat="server" Text="(1) eHS(S) to CMS Health Check History"
                                                                ToolTip="Check if any records in [InterfaceHealthCheckLog] have connection failed cases"></asp:Label>
                                                        </td>
                                                        <td style="width: 130px; padding-left: 5px">
                                                            <asp:Label ID="lblECSResult1" runat="server" Width="180"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblECSContent2" runat="server" Text="(2) eVaccination Record Interface Status (Transactions)"
                                                                ToolTip="Check if any records in [VoucherTransaction] have [Ext_Ref_Status] = _CN or _UN"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 5px">
                                                            <asp:Label ID="lblECSResult2" runat="server" Width="180"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblECSContent3" runat="server" Text="(3) eVaccination Record Interface Status (Audit Logs)"
                                                                ToolTip="Check if any records in [AuditLogHCSPxx] have vaccination fail cases. e.g. 01109 (Invalid Parameters)"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 5px">
                                                            <asp:Label ID="lblECSResult3" runat="server" Width="180"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblECSContent4" runat="server" Text="(4) CMS to eHS(S) Enquiry" ToolTip="Check if any records in [AuditLogInterfacexx] with [Function_Code] 060101 and [Log_ID] 00003 requested by CMS which requires &gt;= 6000 ms to process"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 5px">
                                                            <asp:Label ID="lblECSResult4" runat="server" Width="180"></asp:Label>
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
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(1) eHS(S) to CMS Health Check History
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblECCLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <%-- Top Row:--%>
                                                        </td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="txtECCTopRow" runat="server" Width="80" Visible="False">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnECCRefresh" runat="server" Text="Refresh" OnClick="btnECCRefresh_Click"
                                                                Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnECCClear" runat="server" Text="Clear" OnClick="btnECCClear_Click"
                                                                Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorECC" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECC2From" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECC2To" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnECC2Refresh" runat="server" Text="Refresh" OnClick="btnECCRefresh2_Click" />
                                                            <asp:Button ID="btnECC2Clear" runat="server" Text="Clear" OnClick="btnECCClear_Click" />
                                                            <asp:LinkButton ID="lbtnECC2GetSummary" runat="server" Text="[Get From Summary]"
                                                                OnClick="lbtnECC2GetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorECC2" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 50%">
                                                            <asp:Label ID="lblECCSite1" runat="server" Text="Site 1:"></asp:Label>
                                                            <asp:GridView ID="gvECC1" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvECC1_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="System Dtm">
                                                                        <ItemStyle Width="180px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSystemDtm" runat="server" Text='<%# Bind("System_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Log ID">
                                                                        <ItemStyle Width="80px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLogID" runat="server" Text='<%# Bind("Log_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemStyle Width="120px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblECCSite2" runat="server" Text="Site 2:"></asp:Label>
                                                            <asp:GridView ID="gvECC2" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvECC2_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="System Dtm">
                                                                        <ItemStyle Width="180px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSystemDtm" runat="server" Text='<%# Bind("System_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Log ID">
                                                                        <ItemStyle Width="80px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLogID" runat="server" Text='<%# Bind("Log_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemStyle Width="120px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
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
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(2) eVaccination Record Interface Status (Transactions)
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblECTLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECTFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECTTo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnECTRefresh" runat="server" Text="Refresh" OnClick="btnECTRefresh_Click" />
                                                            <asp:Button ID="btnECTClear" runat="server" Text="Clear" OnClick="btnECTClear_Click" />
                                                            <asp:LinkButton ID="lbtnECTGetSummary" runat="server" Text="[Get From Summary]" OnClick="lbtnECTGetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorECT" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvECT" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvECT_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="HA Vaccine Ref Status">
                                                                        <ItemStyle Width="160px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblExtRefStatus" runat="server" Text='<%# Bind("Ext_Ref_Status")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Transaction Time">
                                                                        <ItemStyle Width="180px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransactionTime" runat="server" Text='<%# Bind("Transaction_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Transaction No.">
                                                                        <ItemStyle Width="160px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransactionNo" runat="server" Text='<%# Bind("Transaction_ID") %>'></asp:Label>
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
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(3) eVaccination Record Interface Status (Audit Logs)
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblECALastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECAFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECATo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnECARefresh" runat="server" Text="Refresh" OnClick="btnECARefresh_Click" />
                                                            <asp:Button ID="btnECAClear" runat="server" Text="Clear" OnClick="btnECAClear_Click" />
                                                            <asp:LinkButton ID="lbtnECAGetSummary" runat="server" Text="[Get From Summary]" OnClick="lbtnECAGetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorECA" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvECA" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvECA_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="System Dtm">
                                                                        <ItemStyle Width="160px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSystemDtm" runat="server" Text='<%# Bind("System_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Function">
                                                                        <ItemStyle Width="70px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFunctionCode" runat="server" Text='<%# Bind("Function_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Log ID">
                                                                        <ItemStyle Width="60px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLogID" runat="server" Text='<%# Bind("Log_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemStyle Width="350px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Session ID">
                                                                        <ItemStyle Width="210px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSessionID" runat="server" Text='<%# Bind("Session_ID") %>'></asp:Label>
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
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(4) CMS to eHS(S) Enquiry
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblECELastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECEFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtECETo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnECERefresh" runat="server" Text="Refresh" OnClick="btnECERefresh_Click" />
                                                            <asp:Button ID="btnECEClear" runat="server" Text="Clear" OnClick="btnECEClear_Click" />
                                                            <asp:LinkButton ID="lbtnECEGetSummary" runat="server" Text="[Get From Summary]" OnClick="lbtnECEGetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorECE" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvECE" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvECE_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="System Dtm">
                                                                        <ItemStyle Width="170px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSystemDtm" runat="server" Text='<%# Bind("System_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Function">
                                                                        <ItemStyle Width="80px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransactionTime" runat="server" Text='<%# Bind("Function_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Log ID">
                                                                        <ItemStyle Width="60px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLogID" runat="server" Text='<%# Bind("Log_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Time Used (ms)">
                                                                        <ItemStyle Width="130px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTimeDiff" runat="server" Text='<%# Bind("Time_Diff") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Action Key">
                                                                        <ItemStyle Width="300px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblActionKey" runat="server" Text='<%# Bind("Action_Key") %>'></asp:Label>
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
                        <br />
                        <asp:Button ID="btnECBack" runat="server" Text="Back" Width="100" OnClick="btnECBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewEVaccControl" runat="server">
                        <strong>HA CMS eVaccination Control</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Switch HA CMS (eVaccination) Connection Mode
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Using:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblEOMCurrentUsing" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777">Current Standby:
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblEOMCurrentStandby" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #777777"></td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewEOM" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewEOMButton" runat="server">
                                                        <asp:Button ID="btnEOMRefresh" runat="server" Text="Refresh" OnClick="btnEOMRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnEOMSwitch" runat="server" Text="Switch Mode" BackColor="Red" ForeColor="White"
                                                            OnClick="btnEOMSwitch_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewEOMConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblEOMSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnEOMYes" runat="server" Text="Yes" Width="100px" OnClick="btnEOMYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnEOMNo" runat="server" Text="No" Width="100px" OnClick="btnEOMNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Switch HA CMS (eVaccination) Interface
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Using:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblEOLCurrentUsing" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblEOLCurrentUsingResult" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblEOLCurrentUsingResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777">Current Standby:
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:MultiView ID="MultiViewEOLCurrentStandby" runat="server" ActiveViewIndex="0">
                                                                <asp:View ID="ViewEOLCurrentStandbyNotInUse" runat="server">
                                                                    <asp:Label runat="server">(Not In Use)</asp:Label>
                                                                </asp:View>
                                                                <asp:View ID="ViewEOLCurrentStandbyONE" runat="server">
                                                                    <asp:Label ID="lblEOLCurrentStandby" runat="server"></asp:Label>
                                                                </asp:View>
                                                                <asp:View ID="ViewEOLCurrentStandbyMULTIPLE" runat="server">
                                                                    <asp:RadioButtonList ID="rdoEOLCurrentStandbyMULTIPLE" runat="server">
                                                                    </asp:RadioButtonList>
                                                                </asp:View>
                                                            </asp:MultiView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #777777"></td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblEOLCurrentStandbyResult" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblEOLCurrentStandbyResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewEOL" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewEOLButton" runat="server">
                                                        <asp:Button ID="btnEOLRefresh" runat="server" Text="Refresh" OnClick="btnEOLRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnEOLPoll" runat="server" Text="Poll Links" OnClick="btnEOLPoll_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnEOLSwitch" runat="server" Text="Switch Links" BackColor="Red"
                                                            ForeColor="White" OnClick="btnEOLSwitch_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewEOLConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblEOLSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnEOLYes" runat="server" Text="Yes" Width="100px" OnClick="btnEOLYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnEOLNo" runat="server" Text="No" Width="100px" OnClick="btnEOLNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Turn On / Suspend eVaccination Record Sharing
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Status:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblEOVCurrentStatus" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewEOV" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewEOVButton" runat="server">
                                                        <asp:Button ID="btnEOVRefresh" runat="server" Text="Refresh" OnClick="btnEOVRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnEOVTurnOn" runat="server" Text="Turn On (Y)" BackColor="Red" ForeColor="White"
                                                            OnClick="btnEOVTurnOn_Click" />
                                                        <asp:Button ID="btnEOVSuspend" runat="server" Text="Suspend (S)" BackColor="Red"
                                                            ForeColor="White" OnClick="btnEOVSuspend_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewEOVConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblEOVSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnEOVYes" runat="server" Text="Yes" Width="100px" OnClick="btnEOVYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnEOVNo" runat="server" Text="No" Width="100px" OnClick="btnEOVNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <uc1:ucClearCacheRequestView ID="ucClearCacheRequestView_EVaccControl" runat="server" />
                        <br />
                        <br />
                        <asp:Button ID="btnEOBack" runat="server" Text="Back" Width="100" OnClick="btnEOBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewDHCIMSEVaccCheck" runat="server">
                        <strong>DH CIMS eVaccination Check</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">Summary
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblDCSLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <asp:Panel ID="pnlCIMSSites" runat="server">
                                                </asp:Panel>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCSFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCSTo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDCSRefreshAll" runat="server" Text="Refresh All" OnClick="btnDCSRefreshAll_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorDCS" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblDCSContent1" runat="server" Text="(1) eHS(S) to CIMS Health Check History"
                                                                ToolTip="Check if any records in [InterfaceHealthCheckLog] have connection failed cases"></asp:Label>
                                                        </td>
                                                        <td style="width: 130px; padding-left: 5px">
                                                            <asp:Label ID="lblDCSResult1" runat="server" Width="220"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblDCSContent2" runat="server" Text="(2) eVaccination Record Interface Status (Transactions)"
                                                                ToolTip="Check if any records in [VoucherTransaction] have [DH_Vaccine_Ref_Status] = _CN or _UN"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 5px">
                                                            <asp:Label ID="lblDCSResult2" runat="server" Width="220"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblDCSContent3" runat="server" Text="(3) eVaccination Record Interface Status (Audit Logs)"
                                                                ToolTip="Check if any records in [AuditLogHCSPxx] have vaccination fail cases. e.g. 01109 (Invalid Parameters)"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 5px">
                                                            <asp:Label ID="lblDCSResult3" runat="server" Width="220"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>
                                                            <asp:Label ID="lblDCSContent4" runat="server" Text="(4) CIMS to eHS(S) Enquiry" ToolTip="Check if any records in [AuditLogInterfacexx] with [Function_Code] 060101 and [Log_ID] 00003 requested by CIMS which requires &gt;= 6000 ms for single mode or &gt;= 15000 ms for batch mode to process"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 5px">
                                                            <asp:Label ID="lblDCSResult4" runat="server" Width="220"></asp:Label>
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
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(1) eHS(S) to CIMS Health Check History
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblDCCLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <%-- Top Row:--%>
                                                        </td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="txtDCCTopRow" runat="server" Width="80" Visible="False">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDCCRefresh" runat="server" Text="Refresh" OnClick="btnDCCRefresh_Click"
                                                                Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDCCClear" runat="server" Text="Clear" OnClick="btnDCCClear_Click"
                                                                Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorDCC" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCC2From" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCC2To" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDCC2Refresh" runat="server" Text="Refresh" OnClick="btnDCCRefresh2_Click" />
                                                            <asp:Button ID="btnDCC2Clear" runat="server" Text="Clear" OnClick="btnDCCClear_Click" />
                                                            <asp:LinkButton ID="lbtnDCC2GetSummary" runat="server" Text="[Get From Summary]"
                                                                OnClick="lbtnDCC2GetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorDCC2" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 50%">
                                                            <asp:GridView ID="gvDCC1" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvDCC1_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="System Dtm">
                                                                        <ItemStyle Width="180px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSystemDtm" runat="server" Text='<%# Bind("System_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Function Code">
                                                                        <ItemStyle Width="100px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFunctionCode" runat="server" Text='<%# Bind("Function_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Log ID">
                                                                        <ItemStyle Width="80px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLogID" runat="server" Text='<%# Bind("Log_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemStyle Width="120px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
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
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(2) eVaccination Record Interface Status (Transactions)
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblDCTLastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCTFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCTTo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDCTRefresh" runat="server" Text="Refresh" OnClick="btnDCTRefresh_Click" />
                                                            <asp:Button ID="btnDCTClear" runat="server" Text="Clear" OnClick="btnDCTClear_Click" />
                                                            <asp:LinkButton ID="lbtnDCTGetSummary" runat="server" Text="[Get From Summary]" OnClick="lbtnDCTGetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorDCT" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvDCT" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvDCT_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="DH Vaccine Ref Status">
                                                                        <ItemStyle Width="160px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDHVaccRefStatus" runat="server" Text='<%# Bind("DH_Vaccine_Ref_Status")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Transaction Time">
                                                                        <ItemStyle Width="180px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransactionTime" runat="server" Text='<%# Bind("Transaction_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Transaction No.">
                                                                        <ItemStyle Width="160px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransactionNo" runat="server" Text='<%# Bind("Transaction_ID") %>'></asp:Label>
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
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(3) eVaccination Record Interface Status (Audit Logs)
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblDCALastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCAFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCATo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDCARefresh" runat="server" Text="Refresh" OnClick="btnDCARefresh_Click" />
                                                            <asp:Button ID="btnDCAClear" runat="server" Text="Clear" OnClick="btnDCAClear_Click" />
                                                            <asp:LinkButton ID="lbtnDCAGetSummary" runat="server" Text="[Get From Summary]" OnClick="lbtnDCAGetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorDCA" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvDCA" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvDCA_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="System Dtm">
                                                                        <ItemStyle Width="160px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSystemDtm" runat="server" Text='<%# Bind("System_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Function">
                                                                        <ItemStyle Width="70px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFunctionCode" runat="server" Text='<%# Bind("Function_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Log ID">
                                                                        <ItemStyle Width="60px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLogID" runat="server" Text='<%# Bind("Log_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemStyle Width="350px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Session ID">
                                                                        <ItemStyle Width="210px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSessionID" runat="server" Text='<%# Bind("Session_ID") %>'></asp:Label>
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
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">(4) CIMS to eHS(S) Enquiry
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblDCELastUpdate" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #0099FF"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td>From:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCEFrom" runat="server" Width="150">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>To:
                                                        </td>
                                                        <td style="width: 170px">
                                                            <asp:TextBox ID="txtDCETo" runat="server" Width="150" ToolTip="Leave this field empty for GETDATE()">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDCERefresh" runat="server" Text="Refresh" OnClick="btnDCERefresh_Click" />
                                                            <asp:Button ID="btnDCEClear" runat="server" Text="Clear" OnClick="btnDCEClear_Click" />
                                                            <asp:LinkButton ID="lbtnDCEGetSummary" runat="server" Text="[Get From Summary]" OnClick="lbtnDCEGetSummary_Click"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblErrorDCE" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvDCE" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                                OnRowDataBound="gvDCE_RowDataBound" SkinID="IC">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="System Dtm">
                                                                        <ItemStyle Width="170px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSystemDtm" runat="server" Text='<%# Bind("System_Dtm") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Function">
                                                                        <ItemStyle Width="80px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTransactionTime" runat="server" Text='<%# Bind("Function_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Log ID">
                                                                        <ItemStyle Width="60px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLogID" runat="server" Text='<%# Bind("Log_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Time Used (ms)">
                                                                        <ItemStyle Width="130px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTimeDiff" runat="server" Text='<%# Bind("Time_Diff") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Batch Enquiry">
                                                                        <ItemStyle Width="100px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBatchEnquiry" runat="server" Text='<%# Bind("Batch_Enquiry") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Action Key">
                                                                        <ItemStyle Width="300px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblActionKey" runat="server" Text='<%# Bind("Action_Key") %>'></asp:Label>
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
                        <br />
                        <asp:Button ID="btnDCBack" runat="server" Text="Back" Width="100" OnClick="btnDCBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewDHCIMSEvaccControl" runat="server">
                        <strong>DH CIMS eVaccination Control</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Switch DH CIMS (eVaccination) Connection Mode
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Using:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDOMCurrentUsing" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777">Current Standby:
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblDOMCurrentStandby" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #777777"></td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewDOM" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewDOMButton" runat="server">
                                                        <asp:Button ID="btnDOMRefresh" runat="server" Text="Refresh" OnClick="btnDOMRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnDOMSwitch" runat="server" Text="Switch Mode" BackColor="Red" ForeColor="White"
                                                            OnClick="btnDOMSwitch_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewDOMConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblDOMSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnDOMYes" runat="server" Text="Yes" Width="100px" OnClick="btnDOMYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnDOMNo" runat="server" Text="No" Width="100px" OnClick="btnDOMNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Switch DH CIMS (eVaccination) Interface
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Using:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDOLCurrentUsing" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblDOLCurrentUsingResult" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblDOLCurrentUsingResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777">Current Standby:
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:MultiView ID="MultiViewDOLCurrentStandby" runat="server" ActiveViewIndex="0">
                                                                <asp:View ID="ViewDOLCurrentStandbyNotInUse" runat="server">
                                                                    <asp:Label runat="server">(Not In Use)</asp:Label>
                                                                </asp:View>
                                                                <asp:View ID="ViewDOLCurrentStandbyONE" runat="server">
                                                                    <asp:Label ID="lblDOLCurrentStandby" runat="server"></asp:Label>
                                                                </asp:View>
                                                                <asp:View ID="ViewDOLCurrentStandbyMULTIPLE" runat="server">
                                                                    <asp:RadioButtonList ID="rdoDOLCurrentStandbyMULTIPLE" runat="server">
                                                                    </asp:RadioButtonList>
                                                                </asp:View>
                                                            </asp:MultiView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="color: #777777"></td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblDOLCurrentStandbyResult" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblDOLCurrentStandbyResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewDOL" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewDOLButton" runat="server">
                                                        <asp:Button ID="btnDOLRefresh" runat="server" Text="Refresh" OnClick="btnDOLRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnDOLPoll" runat="server" Text="Poll Links" OnClick="btnDOLPoll_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnDOLSwitch" runat="server" Text="Switch Links" BackColor="Red"
                                                            ForeColor="White" OnClick="btnDOLSwitch_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewDOLConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblDOLSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnDOLYes" runat="server" Text="Yes" Width="100px" OnClick="btnDOLYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnDOLNo" runat="server" Text="No" Width="100px" OnClick="btnDOLNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Turn On / Suspend CIMS eVaccination Record Sharing
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Status:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDOVCurrentStatus" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewDOV" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewDOVButton" runat="server">
                                                        <asp:Button ID="btnDOVRefresh" runat="server" Text="Refresh" OnClick="btnDOVRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnDOVTurnOn" runat="server" Text="Turn On (Y)" BackColor="Red" ForeColor="White"
                                                            OnClick="btnDOVTurnOn_Click" />
                                                        <asp:Button ID="btnDOVSuspend" runat="server" Text="Suspend (S)" BackColor="Red"
                                                            ForeColor="White" OnClick="btnDOVSuspend_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewDOVConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblDOVSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnDOVYes" runat="server" Text="Yes" Width="100px" OnClick="btnDOVYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnDOVNo" runat="server" Text="No" Width="100px" OnClick="btnDOVNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <uc1:ucClearCacheRequestView ID="ucClearCacheRequestView_CIMSEVaccControl" runat="server" />
                        <br />
                        <br />
                        <asp:Button ID="btnDOBack" runat="server" Text="Back" Width="100" OnClick="btnDOBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewPPIePRControl" runat="server">
                        <strong>eHealth System (Subsidies) - Interface Control Web Page</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Switch TSW Patient List Site
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="vertical-align: top; width: 130px">
                                                            <asp:Label ID="lblCurrentUsingText" runat="server" Text="Current Using:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblUsing_PPIePRText" runat="server" Text="PPIePRWSLink" Font-Bold="true"
                                                                            Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblUsing_PPIePR" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                                    <td>
                                                                        <asp:Label ID="lblUsing_SSO_PPI_App_IdP_WS_UrlText" runat="server" Text="SSO_PPI_App_IdP_WS_Url"
                                                                            Font-Bold="true" Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblUsing_SSO_PPI_App_IdP_WS_Url" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblUsing_SSO_PPI_App_SP_WS_UrlText" runat="server" Text="SSO_PPI_App_SP_WS_Url"
                                                                            Font-Bold="true" Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblUsing_SSO_PPI_App_SP_WS_Url" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblUsing_TokenReplacementWS_EHSToPPI_UrlText" runat="server" Text="TokenReplacementWS_EHSToPPI_Url"
                                                                            Font-Bold="true" Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblUsing_TokenReplacementWS_EHSToPPI_Url" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>--%>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777; vertical-align: top">
                                                            <asp:Label ID="lblCurrentStandbyText" runat="server" Text="Current Standby:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="color: #777777">
                                                            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblStandby_PPIePRText" runat="server" Text="PPIePRWSLink" Font-Bold="true"
                                                                            Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblStandby_PPIePR" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                                    <td>
                                                                        <asp:Label ID="lblStandby_SSO_PPI_App_IdP_WS_UrlText" runat="server" Text="SSO_PPI_App_IdP_WS_Url"
                                                                            Font-Bold="true" Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblStandby_SSO_PPI_App_IdP_WS_Url" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblStandby_SSO_PPI_App_SP_WS_UrlText" runat="server" Text="SSO_PPI_App_SP_WS_Url"
                                                                            Font-Bold="true" Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblStandby_SSO_PPI_App_SP_WS_Url" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblStandby_TokenReplacementWS_EHSToPPI_UrlText" runat="server" Text="TokenReplacementWS_EHSToPPI_Url"
                                                                            Font-Bold="true" Font-Underline="true"></asp:Label>
                                                                        <br />
                                                                        <asp:Label ID="lblStandby_TokenReplacementWS_EHSToPPI_Url" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>--%>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewPPIePRSite" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewPPIePRSite_Button" runat="server">
                                                        <asp:Button ID="btnPPIePRSite_Refresh" runat="server" Text="Refresh" OnClick="btnPPIePRSite_Refresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnPPIePRSite_Switch" runat="server" Text="Switch Site" BackColor="Red"
                                                            ForeColor="White" OnClick="btnPPIePRSite_Switch_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewPPIePRSite_Confirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblPPIePRSite_Switch_Sure" runat="server" Text="Are you sure?" ForeColor="Red"></asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnPPIePRSite_Switch_Yes" runat="server" Text="Yes" Width="100px"
                                                                        OnClick="btnPPIePRSite_Switch_Yes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnPPIePRSite_Switch_No" runat="server" Text="No" Width="100px" OnClick="btnPPIePRSite_Switch_No_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <uc1:ucClearCacheRequestView ID="ucClearCacheRequestView_PPIePRControl" runat="server" />
                        <br />
                        <br />
                        <asp:Button ID="btnBack_PPIePRControl" runat="server" Text="Back" Width="100" OnClick="btnBack_PPIePRControl_Click" />
                    </asp:View>
                    <asp:View ID="ViewTokenServerControl" runat="server">
                        <strong>eHealth System (Subsidies) - Interface Control Web Page</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Main Token Server (<asp:Label runat="server" ID="lblTMRSAVersion"></asp:Label>)
                                                <asp:HiddenField ID="hfTMRSAVersion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 220px">Link:
                                                        </td>
                                                        <td style="width: 630px">
                                                            <asp:Label ID="lblTMLink" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>WebLogic Username:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTMWebLogicUsername" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>API Username:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTMAMUsername" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>Connection Status:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTMStatus" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>API Password change at:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTMAMPasswordLastChange" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>API Password must be changed:
                                                        </td>
                                                        <td>Every 1440 days
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <hr />
                                                            <table>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 220px">WS Link:
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblTMWSLink" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td>WS Connection Status:
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblTMWSStatus" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="mvTMAction" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="vTMButton" runat="server">
                                                        <asp:Button ID="btnTMPoll" runat="server" Text="Poll RSA Server" OnClick="btnTMPoll_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnTMChangePassword" runat="server" Text="Change API Password on both eHS(S) and RSA"
                                                            BackColor="Red" ForeColor="White" OnClick="btnTMChangePassword_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnTMWSHealthCheck" runat="server" Text="WS Health Check" OnClick="btnTMWSHealthCheck_Click" />
                                                    </asp:View>
                                                    <asp:View ID="vTMChangePassword" runat="server">
                                                        <table cellpadding="0" cellspacing="2">
                                                            <tr>
                                                                <td style="width: 140px">New API Password:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTMPPassword" runat="server" Width="150" TextMode="password"></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 40px; font-size: 8pt; font-weight: bold; vertical-align: top">Password Criteria:
                                                                </td>
                                                                <td rowspan="3" style="padding-left: 10px; font-size: 8pt; vertical-align: top">- Length in 8 to 32 characters<br />
                                                                    - At least 6 alphabetic characters<br />
                                                                    - At least 1 special characters<br />
                                                                    - Cannot contain the following 2 special characters: @ ~<br />
                                                                    - Cannot reuse last 5 passwords
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Retype Password:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTMPPasswordRetype" runat="server" Width="150" TextMode="password"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td style="padding-top: 10px">
                                                                    <asp:Button ID="btnTMPConfirm" runat="server" Text="Confirm" Width="100px" OnClick="btnTMPConfirm_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnTMPCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnTMPCancel_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table id="tblRSASub" runat="server" style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Sub Token Server (<asp:Label runat="server" ID="lblTSRSAVersion"></asp:Label>)
                                                <asp:HiddenField ID="hfTSRSAVersion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 220px">Link:
                                                        </td>
                                                        <td style="width: 630px">
                                                            <asp:Label ID="lblTSLink" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>WebLogic Username:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTSWebLogicUsername" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>API Username:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTSAMUsername" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>Connection Status:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTSStatus" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>API Password change at:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTSAMPasswordLastChange" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td>API Password must be changed:
                                                        </td>
                                                        <td>Every 1440 days
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <hr />
                                                            <table>
                                                                <tr style="height: 25px">
                                                                    <td style="width: 220px">WS Link:
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblTSWSLink" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td>WS Connection Status:
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblTSWSStatus" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="mvTSAction" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="vTSButton" runat="server">
                                                        <asp:Button ID="btnTSPoll" runat="server" Text="Poll RSA Server" OnClick="btnTSPoll_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnTSChangePassword" runat="server" Text="Change API Password on both eHS(S) and RSA"
                                                            BackColor="Red" ForeColor="White" OnClick="btnTSChangePassword_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnTSWSHealthCheck" runat="server" Text="WS Health Check" OnClick="btnTSWSHealthCheck_Click" />
                                                    </asp:View>
                                                    <asp:View ID="vTSChangePassword" runat="server">
                                                        <table cellpadding="0" cellspacing="2">
                                                            <tr>
                                                                <td style="width: 140px">New API Password:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTSPPassword" runat="server" Width="150" TextMode="password"></asp:TextBox>
                                                                </td>
                                                                <td style="padding-left: 40px; font-size: 8pt; font-weight: bold; vertical-align: top">Password Criteria:
                                                                </td>
                                                                <td rowspan="3" style="padding-left: 10px; font-size: 8pt; vertical-align: top">- Length in 8 to 32 characters<br />
                                                                    - At least 6 alphabetic characters<br />
                                                                    - At least 1 special characters<br />
                                                                    - Cannot contain the following 2 special characters: @ ~<br />
                                                                    - Cannot reuse last 5 passwords
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Retype Password:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTSPPasswordRetype" runat="server" Width="150" TextMode="password"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td style="padding-top: 10px">
                                                                    <asp:Button ID="btnTSPConfirm" runat="server" Text="Confirm" Width="100px" OnClick="btnTSPConfirm_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnTSPCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnTSPCancel_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <uc1:ucClearCacheRequestView ID="ucClearCacheRequestView_TokenServerControl" runat="server" />
                        <br />
                        <br />
                        <asp:Button ID="btnBack_TokenServerControl" runat="server" Text="Back" Width="100"
                            OnClick="btnBack_TokenServerControl_Click" />
                    </asp:View>
                    <asp:View ID="ViewEHRControlSite" runat="server">
                        <strong>eHR - Control Sites</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Switch Sites Control
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC1Text" runat="server" Text="Endpoint:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="font-weight: bold">
                                                            <asp:Label ID="lblRCDC1EndPoint" runat="server">DC1 </asp:Label>
                                                            <asp:Label ID="lblRCDC1Primary" runat="server" Visible="False">(Primary)</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC1StatusText" runat="server" Text="Server Status:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC1Status" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label runat="server" Text="Functional Link" Width="130px" Font-Bold="true" Font-Underline="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC1_WS_Using_Text" runat="server" Text="Current Using:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC1_WS_Using_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC1_WS_Using_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC1_WS_Using_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777;">
                                                            <asp:Label ID="lblRCDC1_WS_Standby_Text" runat="server" Text="Current Standby:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblRCDC1_WS_Standby_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC1_WS_Standby_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC1_WS_Standby_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label runat="server" Text="Get VP Link" Width="130px" Font-Bold="true" Font-Underline="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC1_VS_Using_Text" runat="server" Text="Current Using:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC1_VS_Using_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC1_VS_Using_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC1_VS_Using_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777;">
                                                            <asp:Label ID="lblRCDC1_VS_Standby_Text" runat="server" Text="Current Standby:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblRCDC1_VS_Standby_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC1_VS_Standby_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC1_VS_Standby_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewRCDC1" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewRCDC1_Button" runat="server">
                                                        <asp:Button ID="btnRCDC1_Resume" runat="server" Text="Resume (Y)" BackColor="Red" ForeColor="White"
                                                            OnClick="btnRC_Resume_Click" />
                                                        <asp:Button ID="btnRCDC1_Suspend" runat="server" Text="Suspend (N)" BackColor="Red"
                                                            ForeColor="White" OnClick="btnRC_Suspend_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnRCDC1_Switch_WS" runat="server" Text="Switch Functional Link" BackColor="Red"
                                                            ForeColor="White" OnClick="btnRC_Switch_WS_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnRCDC1_Switch_VS" runat="server" Text="Switch Get VP Link" BackColor="Red"
                                                            ForeColor="White" OnClick="btnRC_Switch_VS_Click" />
                                                        &nbsp; &nbsp;
                                                    </asp:View>
                                                    <asp:View ID="ViewRCDC1_Confirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblRCDC1_Sure" runat="server" Text="Are you sure?" ForeColor="Red"></asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnRCDC1_Yes" runat="server" Text="Yes" Width="100px"
                                                                        OnClick="btnRC_Yes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnRCDC1_No" runat="server" Text="No" Width="100px"
                                                                        OnClick="btnRC_No_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <hr />
                                                <table style="width: 100%">
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC2Text" runat="server" Text="Endpoint:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="font-weight: bold">
                                                            <asp:Label ID="lblRCDC2EndPoint" runat="server">DC2 </asp:Label>
                                                            <asp:Label ID="lblRCDC2Primary" runat="server" Visible="False">(Primary)</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC2StatusText" runat="server" Text="Server Status:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC2Status" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label runat="server" Text="Functional Link" Width="130px" Font-Bold="true" Font-Underline="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC2_WS_Using_Text" runat="server" Text="Current Using:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC2_WS_Using_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC2_WS_Using_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC2_WS_Using_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777;">
                                                            <asp:Label ID="lblRCDC2_WS_Standby_Text" runat="server" Text="Current Standby:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblRCDC2_WS_Standby_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC2_WS_Standby_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC2_WS_Standby_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">
                                                            <asp:Label runat="server" Text="Get VP Link" Width="130px" Font-Bold="true" Font-Underline="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label ID="lblRCDC2_VS_Using_Text" runat="server" Text="Current Using:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC2_VS_Using_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC2_VS_Using_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC2_VS_Using_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777;">
                                                            <asp:Label ID="lblRCDC2_VS_Standby_Text" runat="server" Text="Current Standby:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblRCDC2_VS_Standby_Link" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblRCDC2_VS_Standby_Link_Result" runat="server" Style="font-size: 8pt"></asp:Label>
                                                            <asp:Label ID="lblRCDC2_VS_Standby_Link_ResultTime" runat="server" Style="font-size: 8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewRCDC2" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewRCDC2_Button" runat="server">
                                                        <asp:Button ID="btnRCDC2_Resume" runat="server" Text="Resume (Y)" BackColor="Red" ForeColor="White"
                                                            OnClick="btnRC_Resume_Click" />
                                                        <asp:Button ID="btnRCDC2_Suspend" runat="server" Text="Suspend (N)" BackColor="Red"
                                                            ForeColor="White" OnClick="btnRC_Suspend_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnRCDC2_Switch_WS" runat="server" Text="Switch Functional Link" BackColor="Red"
                                                            ForeColor="White" OnClick="btnRC_Switch_WS_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnRCDC2_Switch_VS" runat="server" Text="Switch Get VP Link" BackColor="Red"
                                                            ForeColor="White" OnClick="btnRC_Switch_VS_Click" />
                                                        &nbsp; &nbsp;
                                                    </asp:View>
                                                    <asp:View ID="ViewRCDC2_Confirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblRCDC2_Sure" runat="server" Text="Are you sure?" ForeColor="Red"></asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnRCDC2_Yes" runat="server" Text="Yes" Width="100px"
                                                                        OnClick="btnRC_Yes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnRCDC2_No" runat="server" Text="No" Width="100px"
                                                                        OnClick="btnRC_No_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <asp:Button ID="btnRC_Refresh" runat="server" Text="Refresh" OnClick="btnRC_Refresh_Click" />
                                                &nbsp; &nbsp;
                                                <asp:Button ID="btnRC_Poll" runat="server" Text="Poll Links" Width="100px" OnClick="btnRC_Poll_Click" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Turn On / Suspend eHRSS Function in HCSP Platform
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Status:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRCTCurrentStatus" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewRCT" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewRCTButton" runat="server">
                                                        <asp:Button ID="btnRCTRefresh" runat="server" Text="Refresh" OnClick="btnRCTRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnRCTTurnOn" runat="server" Text="Turn On (Y)" BackColor="Red" ForeColor="White"
                                                            OnClick="btnRCTTurnOn_Click" />
                                                        <asp:Button ID="btnRCTSuspend" runat="server" Text="Suspend (N)" BackColor="Red"
                                                            ForeColor="White" OnClick="btnRCTSuspend_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewRCTConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblRCTSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnRCTYes" runat="server" Text="Yes" Width="100px" OnClick="btnRCTYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnRCTNo" runat="server" Text="No" Width="100px" OnClick="btnRCTNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <uc1:ucClearCacheRequestView ID="ucClearCacheRequestView_eHRControl" runat="server" />
                        <br />
                        <br />
                        <asp:Button ID="btnRCBack" runat="server" Text="Back" Width="100" OnClick="btnRCBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewEHREnquireEHR" runat="server">
                        <strong>eHR - Enquire eHR</strong>
                        <br />
                        <br />
                        <table style="width: 900px" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 150px">Mode:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtREMode" runat="server" Width="100" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td>Primary Site:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtREPrimarySite" runat="server" Width="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>VP Link:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtREVPLink" runat="server" Width="600"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>GetWebS Link:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtREGetWebSLink" runat="server" Width="600"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>System ID:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRESystemID" runat="server" Width="300"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Certification Mark:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRECertificationMark" runat="server" Width="300"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Service Code:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtREServiceCode" runat="server" Width="300"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">Health Check
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblREHealthCheckLastUpdate" runat="server" Font-Size="8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="background-color: #9E9E9E" cellspacing="2" class="Call">
                                                    <tr>
                                                        <th style="width: 250px">Input Parameters</th>
                                                        <th style="width: 120px"></th>
                                                        <th style="width: 450px">Output</th>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center; font-style: italic">-- No input --</td>
                                                        <td style="text-align: center">
                                                            <asp:Button ID="btnREHealthCheckCall" runat="server" Text=">> Call >>" BackColor="Red"
                                                                ForeColor="White" OnClick="btnREHealthCheckCall_Click" /></td>
                                                        <td>
                                                            <asp:Label ID="lblREHealthCheckResult" runat="server"></asp:Label>
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
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">Get eHR Token
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblREGetEHRTokenLastUpdate" runat="server" Font-Size="8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="background-color: #9E9E9E" cellspacing="2" class="Call">
                                                    <tr>
                                                        <th style="width: 250px">Input Parameters</th>
                                                        <th style="width: 120px"></th>
                                                        <th style="width: 450px">Output</th>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <table class="NoPadding">
                                                                <tr>
                                                                    <td>SPID:
                                                            <asp:TextBox ID="txtRETSPID" runat="server" Width="90"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>-- OR --
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>HKID:
                                                            <asp:TextBox ID="txtRETHKID" runat="server" Width="90"></asp:TextBox></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Button ID="btnREGetEHRToken" runat="server" Text=">> Call >>" BackColor="Red"
                                                                ForeColor="White" OnClick="btnREGetEHRToken_Click" />
                                                            <br />
                                                            <asp:CheckBox ID="chkRETMaskHKID" runat="server" Checked="true" Text="Mask HKID" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblREGetEHRToken" runat="server"></asp:Label>
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
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">Get eHR Login Alias
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblREGetEHRLoginAliasLastUpdate" runat="server" Font-Size="8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="background-color: #9E9E9E" cellspacing="2" class="Call">
                                                    <tr>
                                                        <th style="width: 250px">Input Parameters</th>
                                                        <th style="width: 120px"></th>
                                                        <th style="width: 450px">Output</th>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <table class="NoPadding">
                                                                <tr>
                                                                    <td>SPID:                             
                                                            <asp:TextBox ID="txtRELSPID" runat="server" Width="90"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>-- OR --
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>HKID:                
                                                            <asp:TextBox ID="txtRELHKID" runat="server" Width="90"></asp:TextBox></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Button ID="btnREGetEHRLoginAlias" runat="server" Text=">> Call >>" BackColor="Red"
                                                                ForeColor="White" OnClick="btnREGetEHRLoginAlias_Click" />
                                                            <br />
                                                            <asp:CheckBox ID="chkRELMaskHKID" runat="server" Checked="true" Text="Mask HKID" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblREGetEHRLoginAliasResult" runat="server"></asp:Label>
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
                        <br />
                        <asp:Button ID="btnREBack" runat="server" Text="Back" Width="100" OnClick="btnREBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewEHRTraceOutsyncRecord" runat="server">
                        <strong>eHR - Trace Outsync Records</strong>
                        <br />
                        <br />
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="padding-right: 10px">From
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRTFrom" runat="server" Width="140"></asp:TextBox>
                                </td>
                                <td style="padding-left: 10px; padding-right: 10px">To
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRTTo" runat="server" Width="140"></asp:TextBox>
                                </td>
                                <td style="padding-left: 10px">
                                    <asp:Button ID="btnRTSearch" runat="server" Text="Search" OnClick="btnRTSearch_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:LinkButton ID="lbtnRTLastOneDay" runat="server" Text="[Last 1 day]" OnClick="lbtnRTLastOneDay_Click"></asp:LinkButton>
                                    &nbsp; &nbsp; 
                                    <asp:LinkButton ID="lbtnRTLast24Hour" runat="server" Text="[Last 24 hours]" OnClick="lbtnRTLast24Hour_Click"></asp:LinkButton>
                                    &nbsp; &nbsp; 
                                    <asp:LinkButton ID="lbtnRTLast1Hour" runat="server" Text="[Last 1 hour]" OnClick="lbtnRTLast1Hour_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Label ID="lblRTError" runat="server" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="gvRT" runat="server" AutoGenerateColumns="False" AllowPaging="False" SkinID="IC">
                            <Columns>
                                <asp:TemplateField HeaderText="Case #">
                                    <ItemStyle Width="50" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGCaseID" runat="server" Text='<%# Bind("Set_Share_Case_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="SPID">
                                    <ItemStyle Width="80px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGSPID" runat="server" Text='<%# Bind("SP_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="eHR Set Share Action Time">
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGSetShareTime" runat="server" Text='<%# Bind("Set_Share_Action_Dtm_String") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="eHR Notification Time">
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGNotificationTime" runat="server" Text='<%# Bind("Set_Share_Notification_Dtm_String") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Suspicious eHS Action">
                                    <ItemStyle Width="140px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGEHSAction" runat="server" Text='<%# Bind("Suspicious_EHS_Action") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Suspicious eHS Action Time">
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGEHSActionTime" runat="server" Text='<%# Bind("Suspicious_EHS_Action_Dtm_String")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Token Serial No.">
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGTokenSerialNo" runat="server" Text='<%# Bind("Token_Serial_No")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Token Serial No. Replacement">
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGTokenSerialNoReplacement" runat="server" Text='<%# Bind("Token_Serial_No_Replacement")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Replace Reason / Deactivate Reason">
                                    <ItemStyle Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGActionRemark" runat="server" Text='<%# Bind("Action_Remark")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Button ID="btnRTBack" runat="server" Text="Back" Width="100" OnClick="btnRTBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewOCSSSControlSite" runat="server">
                        <strong>OCSSS - Control Sites</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Switch Sites Control
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 130px">
                                                            <asp:Label runat="server" Text="Current Using:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblOCLCurrentUsing" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 25px">
                                                        <td style="color: #777777;">
                                                            <asp:Label runat="server" Text="Current Standby:" Width="130px"></asp:Label>
                                                        </td>
                                                        <td style="color: #777777">
                                                            <asp:Label ID="lblOCLCurrentStandby" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewOCL" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewOCL_Button" runat="server">
                                                        <asp:Button ID="btnOCLRefresh" runat="server" Text="Refresh" OnClick="btnOCLRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnOCLSwitch" runat="server" Text="Switch Link" BackColor="Red" ForeColor="White" OnClick="btnOCLSwitch_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewOC_Confirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblOCLSure" runat="server" Text="Are you sure?" ForeColor="Red"></asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnOCLYes" runat="server" Text="Yes" Width="100px" OnClick="btnOCLYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnOCLNo" runat="server" Text="No" Width="100px" OnClick="btnOCLNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Turn On / Suspend OCSSS Function in HCSP Platform
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table>
                                                    <tr style="height: 25px">
                                                        <td style="width: 130px">Current Status:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblOCTCurrentStatus" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:MultiView ID="MultiViewOCT" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="ViewOCTButton" runat="server">
                                                        <asp:Button ID="btnOCTRefresh" runat="server" Text="Refresh" OnClick="btnOCTRefresh_Click" />
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnOCTTurnOn" runat="server" Text="Turn On (Y)" BackColor="Red" ForeColor="White"
                                                            OnClick="btnOCTTurnOn_Click" />
                                                        <asp:Button ID="btnOCTSuspend" runat="server" Text="Suspend (N)" BackColor="Red"
                                                            ForeColor="White" OnClick="btnOCTSuspend_Click" />
                                                    </asp:View>
                                                    <asp:View ID="ViewOCTConfirm" runat="server">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="text-align: left">
                                                                    <asp:Label ID="lblOCTSure" runat="server" Text="Are you sure?" ForeColor="Red">
                                                                    </asp:Label>
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnOCTYes" runat="server" Text="Yes" Width="100px" OnClick="btnOCTYes_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Button ID="btnOCTNo" runat="server" Text="No" Width="100px" OnClick="btnOCTNo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <uc1:ucClearCacheRequestView ID="ucClearCacheRequestView_OCSSSControl" runat="server" />
                        <br />
                        <br />
                        <asp:Button ID="btnOCBack" runat="server" Text="Back" Width="100" OnClick="btnOCBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewOCSSSEnquireOCSSS" runat="server">
                        <strong>OCSSS - Enquire OCSSS</strong>
                        <br />
                        <br />
                        <table style="width: 900px" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>Link:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOEWSLink" runat="server" Width="600"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Pass Phrase:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOEPassPhrase" runat="server" Width="600"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">Health Check
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblOEHealthCheckLastUpdate" runat="server" Font-Size="8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="background-color: #9E9E9E" cellspacing="2" class="Call">
                                                    <tr>
                                                        <th style="width: 250px">Input Parameters</th>
                                                        <th style="width: 120px"></th>
                                                        <th style="width: 450px">Output</th>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center; font-style: italic">-- No input --</td>
                                                        <td style="text-align: center">
                                                            <asp:Button ID="btnOEHealthCheckCall" runat="server" Text=">> Call >>" BackColor="Red"
                                                                ForeColor="White" OnClick="btnOEHealthCheckCall_Click" /></td>
                                                        <td>
                                                            <asp:Label ID="lblOEHealthCheckResult" runat="server"></asp:Label>
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
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="font-weight: bold">Check Residential Status
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblOECheckResidentialStatusLastUpdate" runat="server" Font-Size="8pt"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="background-color: #9E9E9E" cellspacing="2" class="Call">
                                                    <tr>
                                                        <th style="width: 250px">Input Parameters</th>
                                                        <th style="width: 120px"></th>
                                                        <th style="width: 450px">Output</th>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <table class="NoPadding">
                                                                <tr>
                                                                    <td>HKID:                
                                                            <asp:TextBox ID="txtOEHKID" runat="server" Width="90"></asp:TextBox></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Button ID="btnOECheckResidentialStatus" runat="server" Text=">> Call >>" BackColor="Red"
                                                                ForeColor="White" OnClick="btnOECheckResidentialStatus_Click" />
                                                            <br />
                                                            <asp:CheckBox ID="chkOEMaskHKID" runat="server" Checked="true" Text="Mask HKID" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblOECheckResidentialStatusResult" runat="server"></asp:Label>
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
                        <br />
                        <asp:Button ID="btnOEBack" runat="server" Text="Back" Width="100" OnClick="btnOEBack_Click" />
                    </asp:View>
                    <asp:View ID="vDownloadReport" runat="server">
                        <strong>eHealth System (Subsidies) - Interface Control Web Page</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Download Report and Data File</td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 140px">Generation ID</td>
                                                        <td>
                                                            <asp:TextBox ID="txtDRGenerationID" runat="server" Width="150"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Password for Excel</td>
                                                        <td>
                                                            <asp:TextBox ID="txtDRExcelPassword" runat="server" Width="150" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td style="padding-top: 10px">
                                                            <asp:Button ID="btnDRDownloadFile" runat="server" Text="Download File" OnClick="btnDRDownloadFile_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblDRError" runat="server" ForeColor=" Red"></asp:Label>
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
                        <br />
                        <asp:Button ID="btnDRBack" runat="server" Text="Back" Width="100" OnClick="btnDRBack_Click" />
                    </asp:View>
                    <asp:View ID="vConnectStringTester" runat="server">
                        <strong>eHealth System (Subsidies) - Interface Control Web Page</strong>
                        <br />
                        <br />
                        <table style="width: 900px; background-color: #999999" cellpadding="3" cellspacing="1">
                            <tr>
                                <td style="background-color: white">
                                    <table style="width: 100%" cellpadding="2">
                                        <tr>
                                            <td style="font-weight: bold">Connection String Tester</td>
                                        </tr>
                                        <tr>
                                            <td style="height: 2px; background-color: #DD3333" colspan="3"></td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px">
                                                <asp:Button ID="btnCallAllConnstr" runat="server" Text="Call All"/>
                                                <asp:Button ID="btnClearAllConnstr" runat="server" Text="Clear All"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <asp:GridView ID="gvConnectionString" runat="server" style="margin-bottom: 0px" AutoGenerateColumns="False" AllowPaging="False" width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Connection String" HeaderStyle-Width="300" >
                                                        <ItemStyle/>
                                                        <ItemTemplate>
                                                            <asp:Label ID="ConnStr" runat="server" style="padding: 10px" Text='<%# Bind("ConnStr")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="90px">
                                                        <ItemStyle/>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnCall" Text='Call' runat="server" style="padding: 1px"
                                                                            CommandArgument='<%# Bind("Call")%>'></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkbtnCancel" Text='Clear' runat="server" style="padding: 1px"
                                                                            CommandArgument='<%# Bind("Clear")%>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Response" HeaderStyle-Width="370px">
                                                        <ItemStyle/>
                                                        <ItemTemplate>
                                                            <asp:Label ID="ConnStrResponse" runat="server" style="padding: 10px" Width="300px" Text='<%# Bind("ResponseTime")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Label ID="ConnStrR" runat="server" ></asp:Label>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Button ID="btnConnStrTestBack" runat="server" Text="Back" Width="100"/>
                    </asp:View>


                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

