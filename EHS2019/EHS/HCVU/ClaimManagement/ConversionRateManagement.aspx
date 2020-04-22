<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="ConversionRateManagement.aspx.vb" Inherits="HCVU.ConversionRateManagement"
    Title="<%$ Resources: Title, ConversionRateManagement %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="tsmConversionRateManagement" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConversionRateManagementBanner %>"
        AlternateText="<%$ Resources: AlternateText, ConversionRateManagementBanner %>">
    </asp:Image>
    <asp:UpdatePanel ID="upanConversionRateManagement" runat="server">
        <ContentTemplate>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="910px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="910px" />

            <%-- Master View of Conversion Rate Management --%>
            <asp:MultiView ID="mvConversionRateManagement" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vNotice" runat="server">
                    <asp:Table ID="tblNotice" BorderWidth="0px" CellSpacing="0" runat="server">
                        <asp:TableRow Height="210px">
                            <asp:TableCell VerticalAlign="Top">
                                <div class = "headingText">
                                    <asp:Label ID="lblCurrentConversionRateInfo" runat="server" 
                                        Text="<%$ Resources: Text, CurrentConversionRateInfo %>"></asp:Label>
                                </div>
                                <asp:MultiView ID="mvCurrentConversionRateInfo" runat="server" ActiveViewIndex="-1">

                                    <%-- 0. No Conversion Rate Record --%>
                                    <asp:View ID="vNoCurrentConversionRate" runat="server">
                                        <asp:Table ID="tblNoCurrentConversionRate" BorderWidth="0px" Width="430px" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    &nbsp;
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top" Width="430px" style="border-top-style:solid;border-top-width:1px">
                                                    <asp:Label ID="lblCurrentConversionRateRecord" runat="server" Text="<%$ Resources: Text, NoConversionRateRecord %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>

                                    <%-- 1. Effected Conversion Rate Record  --%>
                                    <asp:View ID="vCurrentConversionRate" runat="server">
                                        <asp:Table ID="tblCurrentConversionRate" BorderWidth="0px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    <span style="width:6px">&nbsp;</span>
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top">
                                                    <asp:Table ID="tblCurrentConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateIDText" runat="server" Text="<%$ Resources: Text, ConversionRateID %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateID" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateCreateByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateCreateBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateCreateDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateApprovedByText" runat="server" Text="<%$ Resources: Text, ApprovedBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateApprovedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateApprovedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View> 

                                </asp:MultiView>

                            </asp:TableCell>

                            <asp:TableCell VerticalAlign="top">
                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                            </asp:TableCell>

                            <asp:TableCell VerticalAlign="Top">
                                <div class = "headingText">
                                    <asp:Label ID="lblNextConversionRateInfo" runat="server" 
                                        Text="<%$ Resources: Text, NextConversionRateInfo %>"></asp:Label>
                                </div>
                                <asp:MultiView ID="mvNextConversionRateInfo" runat="server" ActiveViewIndex="-1">
                                    <%-- 0. No Next Conversion Rate Record --%>
                                    <asp:View ID="vNoNextConversionRate" runat="server">
                                        <asp:Table ID="tblNoNextConversionRate" BorderWidth="0px" Width="430px" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    &nbsp;
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top" Width="412px" style="border-top-style:solid;border-top-width:1px">
                                                    <asp:Label ID="lblNextConversionRateRecord" runat="server" Text="<%$ Resources: Text, NoConversionRateRecord %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>

                                    <%-- 1. Approved Conversion Rate Record  --%>
                                    <asp:View ID="vNextConversionRate" runat="server">
                                        <asp:Table ID="tblNextConversionRate" BorderWidth="0px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    <span style="width:6px">&nbsp;</span>
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top">
                                                    <asp:Table ID="tblNextConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateIDText" runat="server" Text="<%$ Resources: Text, ConversionRateID %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateID" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <asp:Label ID="lblDeleteApprovedConversionRateAsterisk" runat="server"
                                                                    style="font-size: 16px;color: #0000FF;	font-family: Arial;	font-weight: bold;" Visible="false">*</asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateCreateByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateCreateBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateCreateDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateApprovedByText" runat="server" Text="<%$ Resources: Text, ApprovedBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateApprovedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateApprovedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="trNextConversionRateRemark" runat="server" Height="30" Visible="false">
                                                            <asp:TableCell VerticalAlign="top" ColumnSpan="3" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateRemarkAsterisk" runat="server" style="font-size: 16px;color: #0000FF;font-family: Arial;font-weight: bold;">*</asp:Label>
                                          
                                                                <asp:Label ID="lblNextConversionRateRemark" runat="server" style="font-size: 16px;color: #0000FF;
                                                                    	font-family: Arial;	font-weight: bold;" Text="<%$ Resources: Text, DeleteApprovedConversionRateRemark %>"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>            
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow ID="trDeleteApprovedRecord" runat="server">
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="bottom" Width="430px" Height="35px" style="text-align:center">
                                                    <asp:ImageButton ID="ibtnDeleteApprovedRecord" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, DeleteBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, DeleteBtn %>"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View> 

                                </asp:MultiView>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableCell RowSpan="1" ColumnSpan="3" VerticalAlign="Top">
                                <div class = "headingText">
                                    <asp:Label ID="lblPendingConversionRateRequest" runat="server"
                                        Text="<%$ Resources: Text, PendingApprovalConversionRateRequest %>"></asp:Label>
                                </div>
                                <asp:MultiView ID="mvPendingConversionRateRequest" runat="server" ActiveViewIndex="-1">
                                    <%-- 0. No Pending Conversion Rate Request --%>
                                    <asp:View ID="vNoPendingConversionRateRequest" runat="server">
                                        <asp:Table ID="tblNoPendingConversionRateRequest" BorderWidth="0px" Width="899px" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="4px" />
                                                <asp:TableCell VerticalAlign="top" Width="890px" style="border-top-style:solid;border-top-width:1px">
                                                    <asp:Label ID="lblPendingConversionRateRequestRecord" runat="server"
                                                         Text="<%$ Resources: Text, NoPendingApprovalConversionRateRequestRecord %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="4px" />
                                                <asp:TableCell VerticalAlign="top" Width="870px" Height="30px" style="vertical-align:bottom">
                                                    <asp:ImageButton ID="ibtnCreateConversionRate" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, CreateConversionRateBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, CreateConversionRateBtn %>"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>

                                    <%-- 1. Create Conversion Rate Request --%>
                                    <asp:View ID="vCreateConversionRateRequest" runat="server">
                                        <asp:Table ID="tblCreateConversionRateRequest" BorderWidth="0px" Width="430px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="top" Width="430px">
                                                    <asp:Table ID="tblCreateConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="2"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="20px">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCreateEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:TextBox ID="txtEffectiveDate" runat="server" Width="70" MaxLength="10" Style="text-align:left"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="filterEffectiveDate" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtEffectiveDate" ValidChars="-" Enabled="True" />
                                                                <span> </span>
                                                                <asp:ImageButton ID="ibtnEffectiveDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" style="vertical-align:text-top"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                                                <span> </span>
                                                                <asp:Image ID="imgEffectiveDateAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" style="position:absolute"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>"/>
                                                                <cc1:CalendarExtender ID="calEffectiveDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnEffectiveDate"
                                                                    TargetControlID="txtEffectiveDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                                </cc1:CalendarExtender>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="20px">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCreateConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCreateConversionRateFormulaText" runat="server" Text="<%$ Resources: Text, ConversionRateFormula %>" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span> </span>
                                                                <asp:TextBox ID="txtConversionRate" runat="server" Width="44" MaxLength="6"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="filterConversionRate" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtConversionRate" ValidChars="." Enabled="True" />
                                                                <span> </span>
                                                                <asp:Image ID="imgConversionRateAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" style="position:absolute"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>"/>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCreateRequestTypeText" runat="server" Text="<%$ Resources: Text, RequestType %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCreateRequestType" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="2"/>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="bottom" Width="430px" Height="40px" style="text-align:center">
                                                    <asp:ImageButton ID="ibtnSave" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, SaveBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, SaveBtn %>"/>
                                                    <span> </span>
                                                    <asp:ImageButton ID="ibtnCancel" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, CancelBtn %>"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View> 

                                    <%-- 2. Confirm Conversion Rate Request --%>
                                    <asp:View ID="vConfirmConversionRateRequest" runat="server">
                                        <asp:Table ID="tblConfirmConversionRateRequest" BorderWidth="0px" Width="430px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="top" Width="430px">
                                                    <asp:Table ID="tblConfirmConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="2"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblConfirmEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblConfirmEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblConfirmConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblConfirmConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblConfirmRequestTypeText" runat="server" Text="<%$ Resources: Text, RequestType %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblConfirmRequestType" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="2"/>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow ID="trConfirmCreation">
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="bottom" Width="430px" Height="40px" style="text-align:center">
                                                    <asp:ImageButton ID="ibtnConfirm" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>"/>
                                                    <span> </span>
                                                    <asp:ImageButton ID="ibtnBack" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, BackBtn %>"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            
                                            <asp:TableRow ID="trCancelBackToHome" style="display:none">
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="bottom" Width="430px" Height="40px" style="text-align:center">
                                                    <asp:ImageButton ID="ibtnCancelBackToHome" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, CancelBtn %>" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View> 

                                    <%-- 3. Pending Approval Conversion Rate Request --%>
                                    <asp:View ID="vPendingApprovalConversionRateRequest" runat="server">
                                        <asp:Table ID="tblPendingApprovalConversionRateRequest" BorderWidth="0px" Width="430px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    <span style="width:6px">&nbsp;</span>
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top">
                                                    <asp:Table ID="tblPendingApprovalConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalRequestTypeText" runat="server" Text="<%$ Resources: Text, RequestType %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalRequestType" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalCreatedByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalCreatedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalCreatedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="bottom" Width="430px" Height="35px" style="text-align:center">
                                                    <asp:ImageButton ID="ibtnDeletePendingRequest" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, DeleteBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, DeleteBtn %>"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View> 

                                    <%-- 4. Pending Delete Approved Conversion Rate Record --%>
                                    <asp:View ID="vPendingDeleteApprovedConversionRateRecord" runat="server">
                                        <asp:Table ID="tblPendingDeleteApprovedConversionRateRecord" BorderWidth="0px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="top">
                                                    <asp:Table ID="vPendingDeleteApprovedConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedConversionRateIDText" runat="server" Text="<%$ Resources: Text, ConversionRateID %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedConversionRateID" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedRequestTypeText" runat="server" Text="<%$ Resources: Text, RequestType %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedRequestType" runat="server"
                                                                    style="font-size: 16px;color: #0000FF;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedCreatedByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedCreatedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedCreatedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedByText" runat="server" Text="<%$ Resources: Text, ApprovedBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedRequestedByText" runat="server" Text="<%$ Resources: Text, DeletionRequestedBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedRequestedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedRequestedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View> 

                                </asp:MultiView>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:View>
            </asp:MultiView>

            <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" Enabled ="false" Visible ="false"/>

            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="<%$ Resources:ImageUrl, InformationIcon %>" /></td>
                                    
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" /></td>
                                    <td align="left" style="width: 35px; height: 42px" />
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                            height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button runat="server" ID="btnHiddenDeletePendingRequest" Style="display: none" /> 
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmDeletePendingRequest" runat="server" TargetControlID="btnHiddenDeletePendingRequest"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnHiddenDeleteApprovedRecord" Style="display: none" /> 
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmDeleteApprovedRecord" runat="server" TargetControlID="btnHiddenDeleteApprovedRecord"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
