<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="SendMessageConfirmation.aspx.vb" Inherits="HCVU.SendMessageConfirmation"
    Title="<%$ Resources:Title, MessageDraftApproval %>" EnableEventValidation="False" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/SentOutMessage/ucReadOnlyMessageDetails.ascx" TagName="ucMessageDetails" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, MessageDraftApprovalBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, MessageDraftApprovalBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanMessageBox" runat="server" Width="930px">
                <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
                <cc2:MessageBox ID="udcMessageBox" runat="server" Width="95%"></cc2:MessageBox>
            </asp:Panel>
            <%--<uc1:ucReadOnlyMessageConfirm ID="ucReadOnlyMessageConfirm" runat="server" />--%>
            <asp:MultiView ID="MultiViewSendMessageConfirmation" runat="server" ActiveViewIndex="0">
                <%-- View Pending Message --%>
                <asp:View ID="ViewPendingMessage" runat="server">
                    <asp:Panel ID="panHeadingText" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblPendingMessage" runat="server" Text="<%$ Resources:Text, DraftPendingApproval %>" />
                        </div>
                    </asp:Panel>                    
                    <asp:GridView ID="gvPendingMessage" runat="server" AllowPaging="True" AllowSorting="True" Width="950px"
                        AutoGenerateColumns="False" OnPageIndexChanging="gvPendingMessage_PageIndexChanging"
                        OnPreRender="gvPendingMessage_PreRender" OnRowCommand="gvPendingMessage_RowCommand"
                        OnRowCreated="gvPendingMessage_RowCreated" OnRowDataBound="gvPendingMessage_RowDataBound"
                        OnSorting="gvPendingMessage_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, MessageID %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_SentOutMsg_ID">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <asp:LinkButton ID="lbtnMessageID" runat="server" />
                                            </td>
                                            <td style="padding-left: 5px">
                                                <asp:Image ID="imgWarning" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                    Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:TextBox ID="txtMessageID" runat="server" Style="display: none;" Value='<%# Eval("SOMS_SentOutMsg_ID") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, Category %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_SentOutMsgCategory">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("SOMS_SentOutMsgCategory") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="70px" />
                                <ItemStyle Width="70px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, Subject %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_SentOutMsgSubject">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("SOMS_SentOutMsgSubject") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="570px" />
                                <ItemStyle Width="570px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, CreateBy %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_Create_By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("SOMS_Create_By") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                                <ItemStyle Width="80px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, CreationTime %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_Create_Dtm">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreationTime" runat="server" Text='<%# Eval("SOMS_Create_Dtm") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="130px" />
                                <ItemStyle Width="130px" VerticalAlign="Top" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />
                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                    </asp:GridView>
                </asp:View>
                <%-- View Pending Message Detail --%>
                <asp:View ID="ViewPendingMessageDetail" runat="server">
                    <uc1:ucMessageDetails ID="ucMessageDetails" runat="server" />
                    <asp:Panel ID="panActionBtn" runat="server">
                        <table width="100%">
                            <tr>
                                <td style="width: 100px">
                                    <asp:ImageButton ID="ibtnApproveReviewBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBack_Click" />
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnApproveToSend" runat="server" ImageUrl="<%$ Resources:ImageUrl, ApproveBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ApproveBtn %>" OnClick="ibtnApproveToSend_Click" />
                                    <asp:ImageButton ID="ibtnReject" runat="server" ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, RejectBtn %>" OnClick="ibtnReject_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <%-- View Complete Pending Message --%>
                <asp:View ID="ViewCompletePendingMessage" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCompletePendingMessageReturn" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnCompletePendingMessageReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            
            <%-- Popup for Approve to send (Start) --%>
            <asp:Button ID="btnHiddenApproveToSend" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupApproveToSend" runat="server" TargetControlID="btnHiddenApproveToSend"
                PopupControlID="panApprove" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="None" PopupDragHandleControlID="panApproveHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panApprove" runat="server" Style="display: none;">
                <%-- Panel header --%>
                <asp:Panel ID="panApproveHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblConfirmTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- Panel body --%>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <%-- Panel body content --%>
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgApprove" runat="server" ImageUrl="<%$ Resources:ImageUrl, QuestionMarkIcon %>" />
                                    </td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblApproveToSend" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ApproveToSend %>" />
                                    </td>
                                    <td style="width: 40px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnConfirmPopupConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnPopupConfirmToSend_Click" />
                                        <asp:ImageButton ID="ibtnConfirmPopupCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnPopupConfirmToSendCancel_Click" />
                                    </td>
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
            <%-- Popup for Approve to send (End) --%>
            
            <%-- Popup for Reject (Start) --%>
            <asp:Button ID="btnHiddenReject" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupReject" runat="server" TargetControlID="btnHiddenReject"
                PopupControlID="panReject" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="None" PopupDragHandleControlID="panRejectHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panReject" runat="server" Style="display: none;">
                <%-- Panel header --%>
                <asp:Panel ID="panRejectHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblRejectTitle" runat="server" Text="Input reject reason"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- Panel body --%>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <%-- Wake up warning message, it is a table --%>
                            <cc2:MessageBox ID="udcRejectReasonMsgBox" runat="server" Visible="false" Width="584px"></cc2:MessageBox>
                            <%-- Panel body content --%>
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 60px; height: 42px" valign="middle">
                                        <%--<asp:Image ID="imgReject" runat="server" ImageUrl="~/Images/others/questionMark.png" />--%>
                                        <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:Text, RejectBoxReason %>"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 520px; height: 42px">
                                        <%--<asp:Label ID="lblReject" runat="server" Font-Bold="True" Text="Please input reject reason"></asp:Label>--%>
                                        <asp:TextBox ID="txtRejectReason" runat="server" Width="475px" MaxLength="1000"></asp:TextBox>
                                        <asp:Image ID="imgErrorReason" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                   ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                   Visible="False" runat="server" ImageAlign="Right" />
                                    </td>
                                    <td style="width: 20px">             
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnRejectPopupReject" runat="server" ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, RejectBtn %>" OnClick="ibtnConfirmReject_Click" />
                                        <asp:ImageButton ID="ibtnRejectPopupCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnConfirmRejectCancel_Click" />
                                    </td>
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
            <%-- Popup for Reject (End) --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
