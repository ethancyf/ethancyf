<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyMessageHistory.ascx.vb"
    Inherits="HCVU.ucReadOnlyMessageHistory" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/SentOutMessage/ucReadOnlyMessageDetails.ascx" TagName="ucMessageDetails" TagPrefix="uc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:MultiView ID="MultiViewMessageHistory_t1" runat="server" ActiveViewIndex="0">
            <%-- Message History --%>
            <asp:View ID="ViewMessageHistory_t1" runat="server">
                <asp:Panel ID="panRadioStatus" runat="server" Visible="False">
                    <div>
                        <asp:Label ID="lblMessageStatusType_t1" runat="server" Text="<%$ Resources:Text, DraftStatus %>"
                            Visible="False" />
                        <asp:RadioButton ID="rbtnStatusPending_t1" runat="server" Text="<%$ Resources:Text, PendingApproval %>"
                            AutoPostBack="True" GroupName="groupMessageStatus" Checked="False" Visible="False" />
                        <asp:RadioButton ID="rbtnStatusConfirmed_t1" runat="server" Text="<%$ Resources:Text, Approved %>"
                            AutoPostBack="True" GroupName="groupMessageStatus" Checked="False" Visible="False" />
                        <asp:RadioButton ID="rbtnStatusRejected_t1" runat="server" Text="<%$ Resources:Text, Rejected %>"
                            AutoPostBack="True" GroupName="groupMessageStatus" Checked="False" Visible="False" />
                        <asp:RadioButton ID="rbtnStatusSent_t1" runat="server" Text="<%$ Resources:Text, Sent %>"
                            AutoPostBack="True" GroupName="groupMessageStatus" Checked="False" Visible="False" />
                        <p />
                    </div>
                </asp:Panel>
                <asp:GridView ID="gvMessageHistory_t1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" OnPageIndexChanging="gvMessageHistory_t1_PageIndexChanging"
                    OnPreRender="gvMessageHistory_t1_PreRender" OnRowCommand="gvMessageHistory_t1_RowCommand"
                    OnRowCreated="gvMessageHistory_t1_RowCreated" OnRowDataBound="gvMessageHistory_t1_RowDataBound"
                    OnSorting="gvMessageHistory_t1_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, MessageID %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_SentOutMsg_ID">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <asp:LinkButton ID="lbtnMessageID_t1" runat="server"></asp:LinkButton>
                                        </td>
                                        <td style="padding-left: 5px">
                                            <asp:Image ID="imgWarning_t1" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:TextBox ID="txtMessageID_t1" runat="server" Style="display: none;" Value='<%# Eval("SOMS_SentOutMsg_ID") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="92px" />
                            <ItemStyle Width="92px" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, Subject %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_SentOutMsgSubject">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject_t1" runat="server" Text='<%# Eval("SOMS_SentOutMsgSubject") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="360px" />
                            <ItemStyle Width="360px" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, Category %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_SentOutMsgCategory">
                            <ItemTemplate>
                                <asp:Label ID="lblCategory_t1" runat="server" Text='<%# Eval("SOMS_SentOutMsgCategory") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, CreateBy %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_Create_By">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedBy_t1" runat="server" Text='<%# Eval("SOMS_Create_By") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="80px"/>
                            <ItemStyle Width="80px" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, CreationTime %>" HeaderStyle-VerticalAlign="Top" SortExpression="SOMS_Create_Dtm">
                            <ItemTemplate>
                                <asp:Label ID="lblCreationTime_t1" runat="server" Text='<%# Eval("SOMS_Create_Dtm") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>" HeaderStyle-VerticalAlign="Top" Visible="False" SortExpression="SOMS_Record_Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus_t1" runat="server" Text='<%# Eval("SOMS_Record_Status") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, SentDtm %>" HeaderStyle-VerticalAlign="Top" Visible="False" SortExpression="SOMS_Sent_Dtm">
                            <ItemTemplate>
                                <asp:Label ID="lblSentDtm_t1" runat="server" Text='<%# Eval("SOMS_Sent_Dtm") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, RejectedDtm %>" HeaderStyle-VerticalAlign="Top" Visible="False" SortExpression="SOMS_Reject_Dtm">
                            <ItemTemplate>
                                <asp:Label ID="lblRejectDtm_t1" runat="server" Text='<%# Eval("SOMS_Reject_Dtm") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="90px" />
                            <ItemStyle Width="90px" VerticalAlign="Top" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                </asp:GridView>
                <br />
                <table class="TableStyle2">
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lbl_KeepFilePeriodNote" runat="server" CssClass="tableText" Visible="False" Text="<%$ Resources:Text, InboxMsgKeepPeriodText %>"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <%-- Message History Detail --%>
            <asp:View ID="ViewMessageHistoryDetail_t1" runat="server">
                <uc1:ucMessageDetails ID="ucMessageDetails" runat="server" />
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
