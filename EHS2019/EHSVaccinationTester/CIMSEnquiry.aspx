<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CIMSEnquiry.aspx.vb" Inherits="EHSVaccinationTester.CIMSEnquiry" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <!-- CIMS connection -->
            <table>
                <tr>
                    <td>URL</td>
                    <td>
                        <asp:TextBox ID="txtURL" runat="server" Width="500px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Server Cert Thumbprint</td>
                    <td>
                        <asp:TextBox ID="txtServerCertThumbprint" runat="server" Width="500px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Client Cert Thumbprint</td>
                    <td>
                        <asp:TextBox ID="txtClientCertThumbprint" runat="server" Width="500px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Connection Timeout (ms)</td>
                    <td>
                        <asp:TextBox ID="txtTimeout" runat="server" Width="50px"></asp:TextBox></td>
                </tr>
            </table>
            <!-- Request | Patient | Patient List -->
            <table>
                <tr>

                    <td style="vertical-align: top;">
                        <!-- Request (Left)-->
                        <table>
                            <tr style="background-color: #6699FF">
                                <td colspan="2" style="color: #FFFFFF">Request Information</td>
                            </tr>
                            <tr>
                                <td>Request Mode</td>
                                <td>
                                    <asp:TextBox ID="txtReqMode" runat="server" Width="50px" ToolTip="1 = Health Check
2 = Interim - For eHS(S) or HA CMS interim use only. Return vaccination records in DH scope
3 = Final – Returns all vaccination records">2</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Request System</td>
                                <td>
                                    <asp:TextBox ID="txtReqSys" runat="server" Width="50px">EHSS</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Vaccine Type</td>
                                <td>
                                    <asp:TextBox ID="txtVaccineType" runat="server" Width="50px" ToolTip="1 = Influenza and Pneumococcal
2 = Influenza
3 = Pneumococcal
4 = Influenza and Pneumococcal and Measles">4</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Influenza Count</td>
                                <td>
                                    <asp:TextBox ID="txtInfluenzaCount" runat="server" Width="50px" ToolTip="1 = Request 1 Influenza
2 = Request 2 Influenza
3 = Request 3 Influenza
999 = Request All Influenza">999</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style1">Client Count</td>
                                <td class="auto-style1">
                                    <asp:TextBox ID="txtClientCount" runat="server" Width="50px">1</asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <!-- Patient (Middle)-->
                        <table>
                            <tr style="background-color: #6600FF; color: #FFFFFF;">
                                <td colspan="2">Patient Information</td>
                            </tr>
                            <tr>
                                <td>Patient ID</td>
                                <td>
                                    <asp:TextBox ID="txtPatientID" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Name</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Doc Type</td>
                                <td>
                                    <asp:TextBox ID="txtDocType" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Doc No.</td>
                                <td>
                                    <asp:TextBox ID="txtDocNo" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>DOB</td>
                                <td>
                                    <asp:TextBox ID="txtDOB" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>DOB Flag</td>
                                <td>
                                    <asp:TextBox ID="txtDOBFlag" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Sex</td>
                                <td>
                                    <asp:TextBox ID="txtSex" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                        </table>

                    </td>
                    <!-- Patient List (Right)-->
                    <td style="vertical-align: top">
                        <table>
                            <tr style="background-color: #006600; color: #FFFFFF;">
                                <td>Demo Patient List 
                                    <asp:DropDownList ID="ddlDemoPatientList" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="173px" ScrollBars="Vertical" Width="100%">
                                        <asp:GridView ID="gvPatient" runat="server" AutoGenerateSelectButton="True" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Overline="False">
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <RowStyle ForeColor="#000066" Font-Names="Tahoma" Font-Size="Small" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnQuerySingle" runat="server" Text="Query (Single)" Width="100%" /></td>
                    <td style="text-align:right;"><asp:Button ID="btnQueryBatch" runat="server" Text="Query (Batch)" Width="150px" /></td>
                </tr>
            </table>

        </div>
        <asp:TextBox ID="txtResult" runat="server" Rows="6" TextMode="MultiLine" Width="100%" Height="125px"></asp:TextBox>
        <asp:GridView ID="gvVaccine" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Names="Tahoma">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" Font-Names="Tahoma" Font-Size="Small" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
        <asp:TextBox ID="txtRawData" runat="server" EnableViewState="False" Font-Names="Tahoma" Rows="50" TextMode="MultiLine" Width="100%"></asp:TextBox>
        <br />
    </form>
</body>
</html>
