<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyREPMT.ascx.vb"
    Inherits="HCVU.ucReadOnlyREPMT" %>
<asp:MultiView ID="MultiViewREPMT" runat="server">
    <asp:View ID="ViewHorizontal" runat="server">
        <table id="tblHREMPT" runat="server">
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                <td style="vertical-align: top" colspan="3">
                    <asp:Label ID="lblHDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHEName" runat="server" CssClass="tableText"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOBGenderText" runat="server" Text="<%$ Resources:Text, DOBLongGender %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOB" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHSlash" runat="server" Text="/" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHReentryPermitNoText" runat="server" Text="<%$ Resources:Text, ReentryPermitNo %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHReentryPermitNo" runat="server" CssClass="tableText"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDateOfIssueText" runat="server" Text="<%$ Resources:Text, DateOfIssue %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDateOfIssue" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="ViewVertical" runat="server">
        <table id="tblVREMPT" runat="server">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVTravelDocNoText" runat="server" Text="<%$ Resources:Text, ReentryPermitNo %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVTravelDocNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVEName" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVGenderText" runat="server" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="lblVGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDOBText" runat="server" Text="<%$ Resources:Text, DOB %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDateOfIssueText" runat="server" Text="<%$ Resources:Text, DateOfIssue %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDateOfIssue" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
