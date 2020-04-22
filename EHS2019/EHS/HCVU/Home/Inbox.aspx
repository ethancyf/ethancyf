<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Inbox.aspx.vb" 
         Inherits="HCVU.Inbox"
         Title="<%$ Resources:Title, InboxPage %>" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/SentOutMessage/CreateMessage.ascx" TagName="ucCreateMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/SentOutMessage/ucReadOnlyMessageHistory.ascx" TagName="ucMessageHistory" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <script language="javascript" src="../JS/Common.js" type="text/javascript"></script>
    <script type="text/javascript">

    function SelectAll(id) {
        var frm = document.forms[0];
        for (i=0;i<frm.elements.length;i++) {
            if (frm.elements[i].type == "checkbox") {
                frm.elements[i].checked = document.getElementById(id).checked;
            }
        }
    }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Image ID="imgLoginInfoBanner" runat="server" ImageUrl="<%$ Resources:ImageUrl, InboxBanner %>" 
                       AlternateText="<%$ Resources:AlternateText, InboxBanner %>" />
            <div style="height: 5px">
            </div>
            <asp:LinkButton ID="lbtnMessageBoxFocus" runat="server"></asp:LinkButton>
            <asp:Panel ID="PanMessageBox" runat="server" Width="950px">
                <cc1:InfoMessageBox ID="udcInfoMessageBox" runat="server" />
                <cc1:MessageBox ID="udcErrorMessage" runat="server" />
            </asp:Panel>            
            <table style="width: 955px" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <%-- Tab header --%>
                        <table style="height:25px" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td id="tdTabHeaderInboxL" runat="server">
                                    <asp:Image ID="imgTabHeaderInboxL" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                </td>
                                <td id="tdTabHeaderInboxM" runat="server" valign="middle" style="width: 94px; background-repeat:repeat-x; text-align:center">
                                    <asp:LinkButton ID="lbtnTabHeaderInbox" runat="server" Text="<%$ Resources:Text, Inbox %>"
                                                    Style="display: block; width: 89px; font-size: 13px"
                                                    OnClick="lbtnTabHeaderInbox_Click">
                                    </asp:LinkButton>
                                </td>
                                <td id="tdTabHeaderInboxR" runat="server">
                                    <asp:Image ID="imgTabHeaderInboxR" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                </td>
                                <td id="tdTabHeaderContentL" runat="server">
                                    <asp:Image ID="imgTabHeaderContentL" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                </td>
                                <td id="tdTabHeaderContentM" runat="server">
                                    <asp:LinkButton ID="lbtnTabHeaderContent" runat="server" Text="" OnClick="lbtnTabHeaderContent_Click"></asp:LinkButton>
                                    <asp:ImageButton ID="ibtnTabHeaderContentClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, TabHeaderClose %>" OnClick="ibtnTabHeaderContentClose_Click" />
                                </td>
                                <td id="tdTabHeaderContentR" runat="server">
                                    <asp:Image ID="imgTabHeaderContentR" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 1px">
                        <%-- Tab content --%>
                        <table cellpadding="0" cellspacing="0" style="width: 100%; border:1px solid #999999">
                            <tr style="height:12px">
                                <td>
                                </td>
                            </tr>
                            <tr style="height: 355px">
                                <%-- Side bar menu --%>
                                <td style="width: 105px; border-top: 1px solid #BBBBBB; border-right: 1px solid #BBBBBB" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                        <%-- Side bar menu item - Inbox --%>
                                        <tr>
                                            <td id="tdSidebarInbox" runat="server">
                                                <asp:LinkButton ID="lbtnSidebarInbox" runat="server" Text="<%$ Resources:Text, Inbox %>" 
                                                                Style="display:block; width: 105px; padding:10px 0px 10px 5px;" 
                                                                OnClick="lbtnSidebarInbox_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <%-- Side bar menu item - Trash --%>
                                        <tr>
                                            <td id="tdSidebarTrash" runat="server">
                                                <asp:LinkButton ID="lbtnSidebarTrash" runat="server" Text="<%$ Resources:Text, Trash %>"
                                                                Style="display:block; width: 105px; padding:10px 0px 10px 5px;"
                                                                OnClick="lbtnSidebarTrash_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <%-- Side bar menu item - Create Draft --%>
                                        <tr>
                                            <td id="tdSidebarNewMessage" runat="server">
                                                <asp:LinkButton ID="lbtnSidebarNewMessage" runat="server" Text="<%$ Resources:Text, CreateDraft %>"
                                                                Style="display:block; width: 105px; padding:10px 0px 10px 5px;"
                                                                OnClick="lbtnSidebarNewMessage_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <%-- Side bar menu item - Draft --%>
                                        <tr>
                                            <td id="tdSidebarOutBox" runat="server">
                                                <asp:LinkButton ID="lbtnSideBarOutBox" runat="server" Text="<%$ Resources:Text, Draft %>"
                                                                Style="display:block; width: 105px; padding:10px 0px 10px 5px;"
                                                                OnClick="lbtnSidebarOutBox_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <%-- Side bar menu item - Sent --%>                       
                                        <tr>
                                            <td id="tdSidebarSent" runat="server">
                                                <asp:LinkButton ID="lbtnSideBarSent" runat="server" Text="<%$ Resources:Text, Sent %>"
                                                                Style="display:block; width: 105px; padding:10px 0px 10px 5px;"
                                                                OnClick="lbtnSidebarSent_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <%-- Side bar menu item - Draft Rejected --%>                       
                                        <tr>
                                            <td id="tdSidebarRejected" runat="server">
                                                <asp:LinkButton ID="lbtnSideBarRejected" runat="server" Text="<%$ Resources:Text, DraftRejected %>"
                                                                Style="display:block; width: 105px; padding:10px 0px 10px 5px;"
                                                                OnClick="lbtnSidebarRejected_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%-- Multiview main content --%>
                                <td style="width: 840px" valign="top">
                                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0" EnableTheming="True">
                                        <%-- View 0 - Inbox gridview --%>
                                        <asp:View ID="vInbox" runat="server">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding: 0px 10px 0px 10px">                          
                                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                          BorderColor="#DEDFDE" ForeColor="Black" OnRowDataBound="GridView1_RowDataBound"
                                                                          OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowPaging="True" AllowSorting="True">
                                                                <Columns>
                                                                    <asp:TemplateField>                                                                        
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chk_selected" runat="server" />
                                                                            <asp:Label ID="lblMessageID" runat ="server" Text='<%# Eval("MessageID") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="10px" />
                                                                        <ItemStyle Width="10px" VerticalAlign="Top"/>
                                                                    </asp:TemplateField>                                    
                                                                    <asp:TemplateField SortExpression="status" HeaderText="<%$ Resources:Text, Status %>">
                                                                        <ItemTemplate>                                            
                                                                            <asp:Image ID="imgLetterOpen"  ImageUrl="<%$ Resources:ImageUrl, LetterOpen %>" runat="server" />
                                                                            <asp:Image ID="imgLetterClose"  ImageUrl="<%$ Resources:ImageUrl, LetterClose %>" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="30px" />
                                                                        <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Sender" HeaderText="Sender" Visible="False" SortExpression="Sender"/>                                                                   
                                                                    <asp:TemplateField SortExpression="subject" HeaderText="<%$ Resources:Text, Subject %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGridSubject" runat="server" Text='<%# Eval("subject") %> '></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="570px" />
                                                                        <ItemStyle Width="570px"/>
                                                                    </asp:TemplateField>                                    
                                                                    <asp:TemplateField SortExpression="rDate" HeaderText="<%$ Resources:Text, ReceiveDate %>">
                                                                        <ItemTemplate>
                                                                            <asp:label ID="lblrDate" runat="server" Text='<%# Eval("rDate") %> '></asp:label>
                                                                        </ItemTemplate>                
                                                                        <HeaderStyle Width="150px" />
                                                                        <ItemStyle Width="150px" VerticalAlign="Top" />                                          
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle BackColor="#CCCC99" />
                                                                <PagerStyle BackColor="#F7F7DE" ForeColor="White" HorizontalAlign="Left" />
                                                                <HeaderStyle BackColor="#6B696B" ForeColor="White" />
                                                            </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <table class="TableStyle2">
                                                <tr>                
                                                    <td style="padding-left: 10px" valign="top">
                                                        <asp:Label ID="lbl_KeepFilePeriodNote" runat="server" CssClass="tableText" Text="<%$ Resources:Text, InboxMsgKeepPeriodText %>"></asp:Label>
                                                        <asp:Label ID="lbl_TrashNote" runat="server" Text="<%$ Resources:Text, TrashNote %>" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 10px" valign="top">
                                                        <asp:ImageButton ID="ibtn_delete" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>" AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>"/>
                                                        <asp:ImageButton ID="ibtn_undelete" runat="server" ImageUrl="<%$ Resources:ImageUrl, UndeleteSBtn %>" AlternateText="<%$ Resources:AlternateText, UndeleteSBtn %>"/>
                                                        <asp:ImageButton ID="ibtn_MarkAsUnread" runat="server" ImageUrl="<%$ Resources:ImageUrl, MarkAsUnreadBtn %>" AlternateText="<%$ Resources:AlternateText, MarkAsUnreadBtn %>"/>
                                                    </td>
                                                </tr>
                                            </table>                                                                 
                                        </asp:View>
                                        <%-- View 1 - Inbox content --%>
                                        <asp:View ID="viewInboxContent" runat="server">
                                            <table>
                                                <tr>
                                                    <td style="padding: 0px 10px 0px 10px">
                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                            <%-- For message subject, receive time and status --%>
                                                            <tr>
                                                                <table style="border-collapse:collapse; border: 1px solid #cccccc;">
                                                                    <tr>
                                                                        <td style="width: 110px; border: 1px solid #cccccc; padding: 2px;" valign="top">
                                                                            <asp:Label ID="lblSubjectText" runat="server" Text="<%$ Resources: Text, Subject %>"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 680px; border: 1px solid #cccccc; padding: 2px;" valign="top">
                                                                            <asp:Label ID="lblSubject" runat="server" Text="Label" CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 110px; border: 1px solid #cccccc; padding: 2px;" valign="top">
                                                                            <asp:Label ID="lblReceiveDateText" runat="server" Text="<%$ Resources:Text, ReceiveDate %>"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 680px; border: 1px solid #cccccc; padding: 2px;" valign="top">
                                                                            <asp:Label ID="lblReceiveDate" runat="server" Text="Label" CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 110px; border: 1px solid #cccccc; padding: 2px;" valign="top">
                                                                            <asp:Label ID="lblStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 680px; border: 1px solid #cccccc; padding: 2px;" valign="top">
                                                                            <asp:Label ID="lblStatus" runat="server" Text="Label" CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>                                 
                                                            </tr>
                                                            <%-- For space only --%>
                                                            <tr style="height: 20px">                                                               
                                                            </tr>
                                                            <%-- For message content --%>
                                                            <tr>
                                                                <td colspan="2" style="padding: 0px 10px 0px 10px">
                                                                    <asp:Label ID="lblContent" runat="server" CssClass="tableText"></asp:Label>
                                                                </td>                                                                
                                                            </tr>
                                                            <%-- For space only --%>
                                                            <tr style="height: 20px">                                                                
                                                            </tr>
                                                            <%-- For delete button --%>
                                                            <tr>
                                                                <td colspan="2" style="padding: 0px 10px 0px 10px">
                                                                    <asp:ImageButton ID="ibtnContentDelete" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>" AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>"/>
                                                                    <asp:ImageButton ID="ibtnContentUndelete" runat="server" ImageUrl="<%$ Resources:ImageUrl, UndeleteSBtn %>" AlternateText="<%$ Resources:AlternateText, UndeleteSBtn %>"/>
                                                                    <asp:ImageButton ID="ibtnContentMarkAsUnread" runat="server" ImageUrl="<%$ Resources:ImageUrl, MarkAsUnreadBtn %>" AlternateText="<%$ Resources:AlternateText, MarkAsUnreadBtn %>"/>
                                                                </td>                                                               
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <%-- View 2 - Create Message --%>
                                        <asp:View ID="viewCreateMessage" runat="server">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding: 0px 10px 0px 10px">
                                                        <uc1:ucCreateMessage ID="ucCreateMessage" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>                                       
                                        </asp:View>
                                        <%-- View 3 - Message History --%>
                                        <asp:View ID="viewMessageHistory" runat="server">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding: 0px 10px 0px 10px">
                                                        <uc2:ucMessageHistory ID="ucMessageHistory" runat="server" />
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
            
            <%-- Popup Confirm message (for inbox) --%>
            <asp:Button ID="btnHiddenShowDialog" runat="server" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderConfirmDelete" runat="server" TargetControlID="btnHiddenShowDialog"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="None" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc2:ModalPopupExtender>
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                <%-- Panel header --%>
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
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
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmDeleteEmail %>" />
                                    </td>
                                    <td style="width: 40px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="btn_confirm_Click" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnDialogCancel_Click" />
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
            <%-- End of popup confirm message --%>
            
            <%-- Popup create message close --%>
            <asp:Button ID="btnHiddenShowCreateMsgClose" runat="server" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderConfirmClose" runat="server" TargetControlID="btnHiddenShowCreateMsgClose"
                PopupControlID="panConfirmClose" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="None" PopupDragHandleControlID="panConfirmCloseHeading">
            </cc2:ModalPopupExtender>
            <asp:Panel ID="panConfirmClose" runat="server" Style="display: none;">
                <%-- Panel header --%>
                <asp:Panel ID="panConfirmCloseHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 640px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- Panel body --%>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 640px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <%-- Panel body content --%>
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgQuesMark" runat="server" ImageUrl="<%$ Resources:ImageUrl, QuestionMarkIcon %>" />
                                    </td>
                                    <td align="left" style="height: 42px">
                                        <asp:Label ID="lblClearText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, DiscardTabData %>" />
                                    </td>
                                    <td style="width: 40px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDialogConfirmClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnDialogConfirmClose_Click" />
                                        <asp:ImageButton ID="ibtnDialogCancelClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnDialogCancelClose_Click" />
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
            <%-- End of popup create message close --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
