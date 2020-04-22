<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="ConversionRateRequestApproval.aspx.vb" Inherits="HCVU.ConversionRateRequestApproval"
    Title="<%$ Resources: Title, ConversionRateRequestApproval %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="tsmConversionRateRequestApproval" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConversionRateRequestApprovalBanner %>"
        AlternateText="<%$ Resources: AlternateText, ConversionRateRequestApprovalBanner %>">
    </asp:Image>

    <asp:UpdatePanel ID="upanConversionRateRequestApproval" runat="server">
        <ContentTemplate>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="910px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="910px" />

            <%-- Master View of Conversion Rate Request Approval --%>
            <asp:MultiView ID="mvConversionRateRequestApproval" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vNotice" runat="server">
                    <asp:Table ID="tblNotice" BorderWidth="0px" Width="940px" CellSpacing="0" runat="server">
                        <asp:TableRow>
                            <asp:TableCell RowSpan="1" ColumnSpan="2" VerticalAlign="Top">

                                <asp:MultiView ID="mvPendingConversionRateRequest" runat="server" ActiveViewIndex="-1">
                                    <%-- 0. No Pending Conversion Rate Request --%>
                                    <asp:View ID="vNoPendingConversionRateRequest" runat="server">
                                        <%--<asp:Table ID="tblNoPendingConversionRateRequest" BorderWidth="0px" Width="430px" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="4px" />
                                                <asp:TableCell VerticalAlign="top" Width="430px" style="border-top-style:solid;border-top-width:1px">
                                                    <asp:Label ID="lblPendingConversionRateRequestRecord" runat="server"
                                                         Text="<%$ Resources: Text, NoPendingApprovalConversionRateRequestRecord %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>--%>
                                    </asp:View>

                                    <%-- 1. Pending Approval Conversion Rate Request --%>
                                    <asp:View ID="vPendingApprovalConversionRateRequest" runat="server">
                                        <div class = "headingText">
                                            <asp:Label ID="lblPendingConversionRateRequest" runat="server"
                                                Text="<%$ Resources: Text, PendingApprovalConversionRateRequest %>"></asp:Label>
                                        </div>
                                        <asp:Table ID="tblPendingApprovalConversionRateRequest" BorderWidth="0px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="top">
                                                    <asp:Table ID="tblPendingApprovalConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalRequestTypeText" runat="server" Text="<%$ Resources: Text, RequestType %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalRequestType" runat="server" Text="<%$ Resources: Text, NewRecord %>" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingApprovalCreatedByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
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
                                                <asp:TableCell VerticalAlign="bottom" Width="480px" Height="38px" style="text-align:center">
                                                    <asp:ImageButton ID="ibtnApprovePendingRequest" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, ApproveBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, ApproveBtn %>"/>
                                                    <span> </span>
                                                    <asp:ImageButton ID="ibtnRejectPendingRequest" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, RejectBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, RejectBtn %>"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View> 

                                    <%-- 2. Pending Delete Approved Conversion Rate Record --%>
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
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
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
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
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
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
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
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedRequestTypeText" runat="server" Text="<%$ Resources: Text, RequestType %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblPendingDeleteApprovedRequestType" runat="server" Text="<%$ Resources: Text, DeleteRecord %>" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
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
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
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
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="30px" style="white-space:nowrap">
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

                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" />
                                                <asp:TableCell VerticalAlign="bottom" Width="480px" Height="38px" style="text-align:center">
                                                    <asp:ImageButton ID="ibtnApproveToDeleteApprovedRecord" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, ApproveBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, ApproveBtn %>"/>
                                                    <span> </span>
                                                    <asp:ImageButton ID="ibtnRejectToDeleteApprovedRecord" runat="server"
                                                        ImageUrl="<%$ Resources: ImageUrl, RejectBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, RejectBtn %>"/>
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
                                    <td align="left" style="width: 108px; height: 42px" />
                                    <td align="left" style="height: 42px">
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" /></td>
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
            <asp:Button runat="server" ID="btnHiddenApprovePendingRequest" Style="display: none" /> 
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmApprovePendingRequest" runat="server" TargetControlID="btnHiddenApprovePendingRequest"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnHiddenRejectPendingRequest" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmRejectPendingRequest" runat="server" TargetControlID="btnHiddenRejectPendingRequest"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnHiddenApproveToDeleteApprovedRecord" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmApproveToDeleteApprovedRecord" runat="server" TargetControlID="btnHiddenApproveToDeleteApprovedRecord"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnRejectToDeleteApprovedRecord" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmRejectToDeleteApprovedRecord" runat="server" TargetControlID="btnRejectToDeleteApprovedRecord"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
