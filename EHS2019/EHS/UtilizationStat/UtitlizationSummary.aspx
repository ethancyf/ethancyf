<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UtitlizationSummary.aspx.vb"
    Inherits="UtilizationStat.UtitlizationSummary" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eHealth System (Subsidies)</title>
</head>
<body style="font-family: Arial">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:MultiView ID="mvReport" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vLogin" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <strong>eHS(S) Statistic Report Login</strong></td>
                                <td>
                                    <a href="InterfaceControl.aspx">Interface Control</a></td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label runat="server" ID="lblErr" ForeColor="red" Visible="false"></asp:Label>
                        <br />
                        <table border="0" cellpadding="5" cellspacing="1" width="400">
                            <tr>
                                <td style="width: 150px">
                                    <asp:Label runat="server" ID="lblLoginID" Text="Login ID"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtLoginID" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px">
                                    <asp:Label runat="server" ID="lblPassword" Text="Password"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtLoginPassword" MaxLength="20" TextMode="Password"
                                        Width="150px" autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px">
                                </td>
                                <td>
                                    <asp:Button runat="server" Text="Login" ID="btnLogin" />
                                    <asp:Button runat="server" Text="Exit" ID="btnExit" OnClientClick="javascript:window.opener='X';window.open('','_parent','');window.close(); return false;" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="vReportSummary" runat="server">
                        <div>          <%-- CRE13-016 Upgrade to excel 2007 [Start] [Karl]
                            <asp:Button Text="eHS Enrolment Summary" runat="server" ID="btnGenerateXLS" Visible="false" />
                            <asp:Button Text="eHS Enrolment Medical Organization Summary" runat="server" ID="btnMO"
                                Visible="false" />
                            <asp:Button Text="eHS Enrolled Practice Summary" runat="server" ID="btnEnrolledPractice"
                                Visible="false" />
                
                           <strong>Generate Ad-hoc Report</strong> &nbsp;<asp:LinkButton ID="lbtnAROpen" runat="server"
                                Text="[+]" OnClick="lbtnAROpen_Click"></asp:LinkButton>
                            <asp:LinkButton ID="lbtnARClose" runat="server" Text="[-]" Visible="False" OnClick="lbtnARClose_Click"></asp:LinkButton>
                            <br />
                            <asp:Panel ID="panAR" runat="server" Visible="False">
                                <table>
                                    <tr>
                                        <td style="padding-left: 20px">
                                            <table>
                                                <tr>
                                                    <td style="width: 220px">
                                                        Database Connection String</td>
                                                    <td style="width: 400px">
                                                        <asp:DropDownList ID="ddlARDBFlag" runat="server" Width="400px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Stored Procedure Name</td>
                                                    <td>
                                                        <asp:TextBox ID="txtARStoredProcedureName" runat="server" Width="300px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Excel Template Name</td>
                                                    <td>
                                                        <asp:TextBox ID="txtARExcelTemplateName" runat="server" Width="300px"></asp:TextBox>
                                                        <asp:Label ID="lblARExcelTemplateNameRemark" runat="server" Text="(Can include or exclude .xls)"
                                                            Style="color: Gray"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Command Timeout</td>
                                                    <td>
                                                        <asp:TextBox ID="txtARCommandTimeout" runat="server" Width="70px"></asp:TextBox>
                                                        <asp:Label ID="lblARCommandTimeoutSec" runat="server" Text="seconds"></asp:Label>
                                                        <asp:Label ID="lblARCommandTimeoutRemark" runat="server" Text="(Leave empty for default)"
                                                            Style="color: Gray"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnARGenerate" runat="server" Text="Generate" Width="100px" OnClick="btnARGenerate_Click" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblARError" runat="server" Style="color: Red" Visible="False"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <hr />
                       CRE13-016 Upgrade to excel 2007 [End] [Karl]--%>
                            <strong>Generate Daily Statistic Report</strong>
                            <table>
                                <tr>
                                    <td style="padding-left: 20px">
                                        <table>
                                            <tr>
                                                <td style="width: 80px">
                                                    Report</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlReport">
                                                    </asp:DropDownList></td>
                                                <td>
                                                    <asp:CheckBox ID="cboPassword" runat="server" Text="Password Protected" AutoPostBack="true"
                                                        Visible="False" />
                                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="20" TextMode="Password" Visible="False"></asp:TextBox></td>
                                                <td>
                                                    <asp:Button ID="btnGen" runat="server" Text="Generate" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:Label runat="server" ID="lbl1"></asp:Label>
                        <br />
                        <hr />
                        <strong>** Daily Report
                            <asp:LinkButton ID="lnkbtnDaily" runat="server"></asp:LinkButton></strong>
                        <br />
                        <br />
                        ** Getting other statistic report from https://192.168.116.92/regenform/
                        <br />
                        <br />
                                                 <%-- CRE13-016 Upgrade to excel 2007 [Start] [Karl] 
                        <table width="1000px" cellpadding="10" cellspacing="1" style="background-color: Gray">
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <strong>eHS Voucher Claim Summary
                                        <br />
                                        (eHSD0001-HCVS Claim)</strong></td>
                                <td style="background-color: White" valign="top">
                                    <strong>eHS EVSS Vaccination Claim
                                        <br />
                                        (eHSD0002-EVSS Claim)</strong></td>
                                <td style="background-color: White" valign="top">
                                    <strong>eHS CIVSS Vaccination Claim
                                        <br />
                                        (eHSD0003-CIVSS Claim)</strong></td>
                                <td style="background-color: White" valign="top">
                                    <strong>eHS RVP Vaccination Claim
                                        <br />
                                        (eHSD0004-RVP Claim)</strong></td>
                                <td style="background-color: White" valign="top">
                                    <strong>eHS EHAPP Claim
                                        <br />
                                        (eHSD0023-EHAPP Claim)</strong></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn1_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn1_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn1_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn1_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn1_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn2_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn2_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn2_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn2_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn2_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn3_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn3_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn3_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn3_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn3_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn4_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn4_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn4_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn4_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn4_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn5_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn5_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn5_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn5_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn5_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn6_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn6_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn6_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn6_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn6_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn7_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn7_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn7_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn7_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn7_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn8_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn8_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn8_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn8_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn8_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn9_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn9_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn9_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn9_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn9_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn10_HCVS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn10_EVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn10_CIVSS" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn10_RVP" runat="server"></asp:LinkButton></td>
                                <td style="background-color: White" valign="top">
                                    <asp:LinkButton ID="lnkbtn10_EHAPP" runat="server"></asp:LinkButton></td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <br />                        
                           CRE13-016 Upgrade to excel 2007 [Start] [Karl] --%>
                        <asp:Button runat="server" Text="Exit" ID="btnReportExit" OnClientClick="javascript:window.opener='X';window.open('','_parent','');window.close(); return false;" />
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;
    </form>
</body>
</html>
