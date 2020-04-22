<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyEC.ascx.vb" Inherits="HCSP.UIControl.DocTypeText.ucReadOnlyEC" %>
<asp:Panel ID="panReadonlyVerticalEC" runat="server">
    <asp:Panel ID="pnlReadonlyTempAccountRefNo" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Panel ID="panReadonlyTempAccountNotice" runat="server">
                        <asp:Label ID="lblReadonlyOpen" runat="server" CssClass="tableText" Text="("></asp:Label><asp:Label ID="lblReadonlyConfirmTempAcctText" runat="server" CssClass="tableText"></asp:Label><asp:Label ID="lblReadonlyClose" runat="server" CssClass="tableText" Text=")"></asp:Label>
                    </asp:Panel>
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
    <table id="tblEC" runat="server" cellpadding="0" cellspacing="0" class="textVersionTable">
        <tbody>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECSerialNoText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr >
                <td valign="top">
                    <asp:Label ID="lblReadonlyECReferenceNoText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECReferenceNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECDateText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECDate" runat="server" CssClass="tableText"></asp:Label></td>
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
                <td valign="top">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECHKIDText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr >
                <td valign="top">
                    <asp:Label ID="lblReadonlyECHKID" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>