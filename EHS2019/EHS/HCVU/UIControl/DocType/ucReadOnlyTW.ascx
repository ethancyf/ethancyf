<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyTW.ascx.vb"
    Inherits="HCVU.ucReadOnlyTW" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:MultiView ID="MultiViewDocType" runat="server">
    <asp:View ID="ViewHorizontal" runat="server">
        <table id="tblHorizontal" runat="server">
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
                    <asp:Label ID="lblHEName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOBGenderText" runat="server" Text="<%$ Resources:Text, DOBLongGender %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOB" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHSlash" runat="server" Text="/" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDocNoText" runat="server" Text="<%$ Resources:Text, OTHERDocNo %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDocNo" runat="server" CssClass="tableText"></asp:Label></td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="ViewVertical" runat="server">
        <table id="tblVertical" runat="server">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDocNoText" runat="server" Text="<%$ Resources:Text, OTHERDocNo %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDocNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDOBText" runat="server" Text="<%$ Resources:Text, DOB %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVEName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblVCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVGenderText" runat="server" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="lblVGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
