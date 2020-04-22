<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Inbox.aspx.vb" Inherits="HCSP.Inbox" 
    title="<%$ Resources:Title, InboxPage %>" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td colspan="2" rowspan="1">
                        <asp:Image ID="imgLoginInfoBanner" runat="server" ImageUrl="<%$ Resources:ImageUrl, InboxBanner %>"
                            AlternateText="<%$ Resources:AlternateText, InboxBanner %>" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PanMessageBox" runat="server" Width="930px">
                <cc1:InfoMessageBox ID="udcInfoMessageBox" runat="server" />
                <cc1:MessageBox ID="udcErrorMessage" runat="server" />
            </asp:Panel>
            <%-- Main table container --%>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 930px">
                <tr>
                    <td>
                        <!-- Tab Header -------------------------------------------------------------->
                        <table border="0" cellpadding="0" cellspacing="0" style="height: 25px">
                            <tr>
                                <td id="tdTabHeaderInboxL" runat="server">
                                    <asp:Image ID="imgTabHeaderInboxL" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                </td>
                                <td id="tdTabHeaderInboxM" runat="server" valign="middle" style="background-repeat: repeat-x;
                                    text-align: center">
                                    <asp:LinkButton ID="lbtnTabHeaderInbox" runat="server" Text="<%$ Resources:Text, Inbox %>"
                                                    Style="display: block; width: 89px;"
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
                                    <asp:ImageButton ID="ibtnTabHeaderContentClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, TabHeaderClose %>"
                                        OnClick="ibtnTabHeaderContentClose_Click" />
                                </td>
                                <td id="tdTabHeaderContentR" runat="server">
                                    <asp:Image ID="imgTabHeaderContentR" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:1px">
                        <!-- Tab Content -------------------------------------------------------------->
                        <table cellpadding="0" cellspacing="0" style="width:100%; border:1px solid #999999">
                            <tr style="height:12px">
                                <td>
                                </td>
                            </tr>
                            <tr style="height:355px" valign="top">
                                <!-- Side Bar Menu -------------------------------------------------------------->
                                <td style="width: 100px; border-top: 1px solid #BBBBBB; border-right: 1px solid #BBBBBB">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <!-- Side Bar Menu Item - Inbox -------------------------------------------------------------->
                                        <tr>
                                            <td id="tdSidebarInbox" runat="server">
                                                <asp:LinkButton ID="lbtnSidebarInbox" runat="server" Text="<%$ Resources:Text, Inbox %>"
                                                                Style="display:block; width: 100px; padding:10px 0px 10px 5px;"
                                                                OnClick="lbtnSidebarInbox_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <!-- Side Bar Menu Item - Trash -------------------------------------------------------------->
                                        <tr>
                                            <td id="tdSidebarTrash" runat="server">
                                                <asp:LinkButton ID="lbtnSidebarTrash" runat="server" Text="<%$ Resources:Text, Trash %>"
                                                                Style="display:block; width: 100px; padding:10px 0px 10px 5px;"
                                                                OnClick="lbtnSidebarTrash_Click">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <!-- Content -------------------------------------------------------------->
                                <td valign="top">
                                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0" EnableTheming="True">
                                        <asp:View ID="v_inbox" runat="server">
                                            <%--&nbsp;<asp:Label ID="lbl_inboxSelect" runat="server" Text="<%$ Resources:Text, SelectMailbox %>"></asp:Label>&nbsp;
                                            <asp:RadioButton ID="rbInbox" runat="server" Text="<%$ Resources:Text, Inbox %>"
                                                AutoPostBack="True" GroupName="msgType" />
                                            <asp:RadioButton ID="rbTrash" runat="server" Text="<%$ Resources:Text, Trash %>"
                                                AutoPostBack="True" GroupName="msgType" /><br />
                                            <br />--%>
                                            <table>
                                                <tr>
                                                    <td style="padding: 0px 10px 0px 10px">
                                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="#DEDFDE" OnRowDataBound="GridView1_RowDataBound"
                                                            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowPaging="True" AllowSorting="True">
                                                            <Columns>
                                                                <asp:TemplateField>                                       
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chk_selected" runat="server" />
                                                                        <asp:Label ID="lblMessageID" runat="server" Text='<%# Eval("MessageID") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="10px" />
                                                                    <ItemStyle Width="10px" VerticalAlign="Top" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField SortExpression="status" HeaderText="<%$ Resources:Text, Status %>">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgLetterOpen" ImageUrl="<%$ Resources:ImageUrl, LetterOpen %>" runat="server" />
                                                                        <asp:Image ID="imgLetterClose" ImageUrl="<%$ Resources:ImageUrl, LetterClose %>"
                                                                            runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="45px" />
                                                                    <ItemStyle Width="45px" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Sender" HeaderText="Sender" Visible="False" SortExpression="Sender" />
                                                                <asp:TemplateField SortExpression="subject" HeaderText="<%$ Resources:Text, Subject %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGridSubject" runat="server" Text='<%# Eval("subject") %> '></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="550px" />
                                                                    <ItemStyle Width="550px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField SortExpression="rDate" HeaderText="<%$ Resources:Text, ReceiveDate %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReceiveDtm" runat="server" Text='<%# Eval("rDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="150px" />
                                                                    <ItemStyle Width="150px" VerticalAlign="Top" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField SortExpression="rDate_Chi" HeaderText="<%$ Resources:Text, ReceiveDate %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReceiveDtm_Chi" runat="server" Text='<%# Eval("rDate_Chi") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="150px" />
                                                                    <ItemStyle Width="150px" VerticalAlign="Top" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#CCCC99" />
                                                            <PagerStyle BackColor="#F7F7DE" ForeColor="White" HorizontalAlign="Left" />
                                                            <HeaderStyle BackColor="#6B696B" ForeColor="White" />
                                                        </asp:GridView>
                                                        <%--<asp:ImageButton ID="ibtn_delete" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>" />
                                                            <asp:ImageButton ID="ibtn_undelete" runat="server" ImageUrl="<%$ Resources:ImageUrl, UndeleteSBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, UndeleteSBtn %>" />
                                                            <asp:Label ID="lbl_KeepFilePeriodNote" runat="server" CssClass="tableText" Text="<%$ Resources:Text, InboxMsgKeepPeriodText %>"></asp:Label>
                                                            <asp:Label ID="lbl_TrashNote" runat="server" Text="<%$ Resources:Text, TrashNote %>"
                                                                CssClass="tableText"></asp:Label></asp:Panel>--%>
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
                                            <%-- ready for delete --%>
                                            <asp:Panel ID="panel_content" runat="server" Visible="False">
                                                <table border="1">
                                                    <tr>
                                                        <td style="width: 100px" valign="top">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Text, Subject %>" Width="106px"></asp:Label>
                                                        </td>
                                                        <td style="width: 100px" valign="top">
                                                            <asp:Label ID="Lbl_subject" runat="server" Width="677px" CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px;" valign="top">
                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Text, Content %>" Width="106px"></asp:Label></td>
                                                        <td style="width: 100px;" valign="top">
                                                            <asp:Label ID="lbl_messageContent" runat="server" CssClass="tableText" Width="677px"></asp:Label></td>
                                                    </tr>
                                                </table>
                                                <asp:Label ID="Label6" runat="server" Text="Sender" Width="106px" Visible="False"></asp:Label>
                                                <asp:Label ID="Lbl_sender" runat="server" Width="677px" Visible="False"></asp:Label>
                                            </asp:Panel>
                                            <%-- ready for delete end --%>
                                        </asp:View>
                                        <%-- View 1 - Inbox content --%>
                                        <asp:View ID="viewInboxContent" runat="server">
                                            <table>
                                                <tr>
                                                    <td style="padding: 0px 10px 0px 10px">
                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                            <%-- For message subject, receive time and status --%>
                                                            <tr>
                                                                <table style="border-collapse: collapse; border: 1px solid #cccccc;">
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
                                                                    <asp:ImageButton ID="ibtnContentDelete" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>" />
                                                                    <asp:ImageButton ID="ibtnContentUndelete" runat="server" ImageUrl="<%$ Resources:ImageUrl, UndeleteSBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, UndeleteSBtn %>" />
                                                                    <asp:ImageButton ID="ibtnContentMarkAsUnread" runat="server" ImageUrl="<%$ Resources:ImageUrl, MarkAsUnreadBtn %>" 
                                                                        AlternateText="<%$ Resources:AlternateText, MarkAsUnreadBtn %>"/>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                                <asp:Label ID="lblConfirmTitle" runat="server" Text="<%$ Resources:Text, ConfirmDeleteEmailBoxTitle %>"></asp:Label>
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
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnDialogCancel_Click" />
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="btn_confirm_Click" />                                            
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
        </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>
