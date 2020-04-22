<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHKIC.ascx.vb"
    Inherits="HCVU.ucReadOnlyHKIC" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:MultiView ID="MultiViewHKIC" runat="server">
    <asp:View ID="ViewHorizontal" runat="server">
        <table id="tblHHKIC" runat="server">
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
                    <asp:Label ID="lblHHKICText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHHKIC" runat="server" CssClass="tableText"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHHKIDIssueDateText" runat="server" Text="<%$ Resources:Text, DateOfIssue %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHHKIDIssueDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDODText" runat="server" Text="<%$ Resources: Text, DateOfDeath %>"></asp:Label></td>
                <td style="vertical-align: top" colspan="3">
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td>
                                <asp:Label ID="lblHDOD" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td style="padding-left: 5px">
                                <cc1:CustomImageButton ID="ibtnHDOD" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>"
                                    ImageUrlDisable="<%$ Resources:ImageUrl, DeathRecordDisableBtn %>" AlternateText="<%$ Resources:AlternateText, DeathRecord %>" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="ViewVertical" runat="server">
        <table id="tblVHKIC" runat="server">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVHKICText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVHKIC" runat="server" CssClass="tableText"></asp:Label></td>
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
                    <asp:Label ID="lblVDOBText" runat="server" Text="<%$ Resources:Text, DOBLong %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVGenderText" runat="server" Text="<%$ Resources:Text, Gender %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblVHKIDIssueDateText" runat="server" Text="<%$ Resources:Text, DateOfIssue %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVHKIDIssueDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trVCreationMethod" runat="server">
                <td valign="top">
                    <asp:Label ID="lblVCreationMethodText" runat="server" Text="<%$ Resources:Text, CreationMethod %>"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblVCreationMethod" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="rowDOD">
                <td valign="top">
                    <asp:Label ID="lblVDODText" runat="server" Text="<%$ Resources: Text, DateOfDeath %>"></asp:Label></td>
                <td valign="top">
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td>
                                <asp:Label ID="lblVDOD" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td style="padding-left: 5px">
                                <cc1:CustomImageButton ID="ibtnVDOD" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>"
                                    ImageUrlDisable="<%$ Resources:ImageUrl, DeathRecordDisableBtn %>" AlternateText="<%$ Resources:AlternateText, DeathRecord %>" />
                                <asp:Image ID="imgVDOD" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>"
                                     AlternateText="<%$ Resources:AlternateText, DeathRecord %>" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
