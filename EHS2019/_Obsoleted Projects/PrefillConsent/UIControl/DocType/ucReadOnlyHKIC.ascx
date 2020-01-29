<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHKIC.ascx.vb"
    Inherits="PrefillConsent.ucReadOnlyHKIC" %>
<asp:Panel ID="panReadonlyVerticalHKIC" runat="server">
    <asp:Panel ID="panReadonlyTempAccountRefNo" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle" Height="28px"
                         Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label></td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
                    <asp:Panel ID="panReadonlyTempAccountNotice" runat="server">
                        <asp:Label ID="lblReadonlyOpen" runat="server" CssClass="tableText" Text="("></asp:Label>
                        <asp:Label
                        ID="lblReadonlyConfirmTempAcctText" runat="server" CssClass="tableText" ></asp:Label>
                        <asp:Label
                            ID="lblReadonlyClose" runat="server" CssClass="tableText" Text=")"></asp:Label></asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panReadonlyCreationDatetime" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyCreationDateTimeText" runat="server" CssClass="tableTitle"
                        Height="28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyCreationDateTime" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
    <table id="tblHKIC" runat="server" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"
                        Height="28px" Style="height: 28px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trHKIDModification" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDModificationText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDModification" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyNameText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyEName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblReadonlyCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOBText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyGenderText" runat="server" CssClass="tableTitle" Height="28px"  Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDIssueDateText" runat="server" CssClass="tableTitle"
                        Height="28px" Style="height: 28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDIssueDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trHKIDCreation" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKID" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyHorizontalHKIC" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalDocumentTypeText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td colspan="3" valign="top">
                <asp:Label ID="lblReadonlyHorizontalDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalNameText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalEName" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblReadonlyHorizontalCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalDOBGenderText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalDOB" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblReadonlyHorizontalDOBGender" runat="server" CssClass="tableText">/</asp:Label>
                <asp:Label ID="lblReadonlyHorizontalGender" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalHKIDText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalHKID" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalHKIDIssueDateText" runat="server" CssClass="tableTitle"
                    Height="25px" Style="height: 28px" Width="175px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalHKIDIssueDate" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
