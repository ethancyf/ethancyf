<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyVISA.ascx.vb"
    Inherits="PrefillConsent.ucReadOnlyVISA" %>
<asp:Panel ID="panReadonlyVerticalVISA" runat="server">
    <asp:Panel ID="panReadonlyTempAccountRefNo" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="height: 19px" valign="top">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle" Height="28px"
                        Width="150px"></asp:Label></td>
                <td style="height: 19px" valign="top">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"
                        Text=""></asp:Label></td>
                <td style="height: 19px" valign="top">
                    &nbsp;</td>
                <td style="height: 19px" valign="top">
                    <asp:Panel ID="panReadonlyTempAccountNotice" runat="server">
                        <asp:Label ID="lblReadonlyOpen" runat="server" CssClass="tableText" Text="("></asp:Label>
                        <asp:Label ID="lblReadonlyConfirmTempAcctText" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblReadonlyClose" runat="server" CssClass="tableText" Text=")"></asp:Label>
                    </asp:Panel>
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
    <table id="tblVISA" runat="server" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"
                        Height="25px" Style="height: 28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyVISANoText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyVISANo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyPassportNoText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadOnlyPassportNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyNameText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyEName" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyGenderText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOBText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyHorizontalVISA" runat="server">
    <table id="TABLE1" cellpadding="0" cellspacing="0" language="javascript" onclick="return TABLE1_onclick()">
        <tr>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalDocumentTypeText" runat="server" CssClass="tableTitle"
                    Height="25px" Style="height: 28px" Width="175px"></asp:Label></td>
            <td colspan="3" valign="top">
                <asp:Label ID="lblReadonlyHorizontalDocumentType" runat="server" CssClass="tableText"
                    Width="300px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalNameText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td style="width: 350px" valign="top">
                <asp:Label ID="lblReadonlyHorizontalEName" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
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
                <asp:Label ID="lblReadonlyHorizontalVISANoText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td style="padding-bottom: 10px;" valign="top">
                <asp:Label ID="lblReadonlyHorizontalVISANo" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalPassportNoText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label>
            </td>
            <td style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalPassportNo" runat="server" CssClass="tableText"
                    Width="300px"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
