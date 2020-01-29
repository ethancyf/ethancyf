<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyMessageDetails.ascx.vb" Inherits="HCVU.ucReadOnlyMessageDetails" %>

<asp:Panel ID="panReadOnlyMessageDetails" runat="server">
    <%-- Message Detail ----------------------------------------------------%>
    <table cellpadding="3" cellspacing="0" style="border:1px solid #999999; border-collapse:collapse">
        <tr id="trTemplateID" runat="server" valign="top" visible="false">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblTemplateIDText" runat="server" Text="<%$ Resources:Text, MessageTemplateID %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblTemplateID" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trMessageID" runat="server" valign="top">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblMessageIDText" runat="server" Text="<%$ Resources:Text, MessageID %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblMessageID" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trStatus" runat="server" valign="top">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblStatusText" runat="server" Text="<%$ Resources:Text, Status %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblStatus" runat="server" Text="" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblRejectedReason" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trSentDateTime" runat="server" valign="top" visible="false">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblSentDateTimeText" runat="server" Text="<%$ Resources:Text, SentDtm %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblSentDateTime" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trCreatedBy" runat="server" valign="top">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblCreatedByText" runat="server" Text="<%$ Resources:Text, CreateBy %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblCreatedBy" runat="server" Text="" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblCreatedDateTime" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trApprovedBy" runat="server" valign="top" visible="false">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblApprovedByText" runat="server" Text="<%$ Resources:Text, ApprovedBy %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblApprovedBy" runat="server" Text="" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblApprovedDateTime" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trRejectedBy" runat="server" valign="top" visible="false">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblRejectedByText" runat="server" Text="<%$ Resources:Text, RejectedBy %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblRejectedBy" runat="server" Text="" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblRejectedDateTime" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblCategoryText" runat="server" Text="<%$ Resources:Text, Category %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblCategory" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblRecipientText" runat="server" Text="<%$ Resources:Text, Recipient %>" CssClass="tableTitle"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblRecipient" runat="server" Text="" CssClass="tableText" Visible="false"></asp:Label>
                <asp:GridView ID="gvRecipient" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false" BackColor="White"
                              HeaderStyle-VerticalAlign="Top">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, HealthProf %>">
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblHealthProfessionText" runat="server" Text='<%# Eval("SOMR_Profession_DisplayText") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:Label ID="lblSchemeText" runat="server" Text='<%# Eval("SOMR_Scheme_DisplayText") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <br /><br />
    <%-- Message Content ----------------------------------------------------%>
    <div class="headingText">
        <asp:Label ID="lblMessageContentText" runat="server" Text="<%$ Resources:Text, MessageContent %>" Font-Bold="true"></asp:Label>
    </div>
    <br />
    <table cellpadding="3" cellspacing="0" style="border:1px solid #999999; border-collapse:collapse">
        <tr valign="top">
            <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                <asp:Label ID="lblSubjectText" runat="server" Text="<%$ Resources:Text, Subject %>" CssClass="tableTitle" Width="110px"></asp:Label>
            </td>
            <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                <asp:Label ID="lblSubject" runat="server" Text="" CssClass="tableText" Width="690px"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <tr valign="top">
            <td style="width:800px">
                <asp:Label ID="lblContent" runat="server" Text="" CssClass="tableText" Width="800px"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
