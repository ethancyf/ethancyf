<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyID235B.ascx.vb"
    Inherits="HCVU.ucReadOnlyID235B" %>
<asp:MultiView ID="MultiViewID235B" runat="server">
    <asp:View ID="ViewHorizontal" runat="server">
        <table id="tblHID235B" runat="server">
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
                    <asp:Label ID="lblHEName" runat="server" CssClass="tableText" />
                    <asp:Label ID="lblHCName" runat="server" CssClass="tableText TextChineseName" />
                </td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOBGenderText" runat="server" Text="<%$ Resources:Text, DOBLongGender %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOB" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHSlash" runat="server" Text="/" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHBENoText" runat="server" Text="<%$ Resources:Text, BirthEntryNo %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHBENo" runat="server" CssClass="tableText"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHPmtRemainText" runat="server" Text="<%$ Resources:Text, PermitToRemain %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHPmtRemain" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="ViewVertical" runat="server">
        <table id="tblVID235B" runat="server">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVBENoText" runat="server" Text="<%$ Resources:Text, BirthEntryNo %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVBENo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVEName" runat="server" CssClass="tableText" />
                    <asp:Label ID="lblVCName" runat="server" CssClass="tableText TextChineseName"/>
                </td>
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
                    <asp:Label ID="lblVPmtRemainText" runat="server" Text="<%$ Resources:Text, PermitToRemain %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVPmtRemain" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
