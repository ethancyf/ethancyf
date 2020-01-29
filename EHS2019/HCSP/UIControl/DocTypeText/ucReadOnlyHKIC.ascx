<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHKIC.ascx.vb" Inherits="HCSP.UIControl.DocTypeText.ucReadOnlyHKIC" %>
<asp:Panel ID="panReadonlyVerticalHKIC" runat="server">
    <asp:Panel ID="panReadonlyTempAccountRefNo" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Panel ID="panReadonlyTempAccountNotice" runat="server">
                        <asp:Label ID="lblReadonlyOpen" runat="server" CssClass="tableText" Text="("></asp:Label><asp:Label ID="lblReadonlyConfirmTempAcctText" runat="server" CssClass="tableText"></asp:Label><asp:Label ID="lblReadonlyClose" runat="server" CssClass="tableText" Text=")"></asp:Label></asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panReadonlyCreationDatetime" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyCreationDateTimeText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyCreationDateTime" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
    <table id="tblHKIC" runat="server" cellpadding="0" cellspacing="0" class="textVersionTable">
        <tbody>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trHKIDModificationText" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDModificationText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr id="trHKIDModification" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDModification" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyNameText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyEName" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOBText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyGenderText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" style="height: 19px">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDIssueDateText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDIssueDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trHKIDCreationText" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKIDText" runat="server" CssClass="tableTitle" />
                </td>
            </tr>
            <tr id="trHKIDCreation" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyHKID" runat="server" CssClass="tableText" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Panel>