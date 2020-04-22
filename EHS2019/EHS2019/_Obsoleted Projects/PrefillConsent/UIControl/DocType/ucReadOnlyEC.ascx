<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyEC.ascx.vb"
    Inherits="PrefillConsent.ucReadOnlyEC" %>
<asp:Panel ID="pnlStep2ECHolder" runat="server" Visible="False">
    <table>
        <tr>
            <td style="width: 10px">
            </td>
            <td>
                <asp:Image ID="imgReadonlyECHolder" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/others/ec.png" /></td>
            <td>
                <asp:Label ID="lblReadonlyECHolder" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyVerticalEC" runat="server">
    <asp:Panel ID="pnlReadonlyTempAccountRefNo" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle" Height="28px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"
                        Text=""></asp:Label></td>
                <td valign="top">
                    &nbsp;</td>
                <td valign="top">
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
    <table id="tblEC" runat="server" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"
                        Height="25px" Style="height: 28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trECHKIDModication" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyECHKIDModificationText" runat="server" CssClass="tableTitle"
                        Height="25px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECHKIDModification" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECSerialNoText" runat="server" CssClass="tableTitle" Height="25px"
                        Style="height: 28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECReferenceNoText" runat="server" CssClass="tableTitle"
                        Height="25px" Style="height: 28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECReferenceNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECDateText" runat="server" CssClass="tableTitle" Height="25px"
                        Style="height: 28px" Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyNameText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyEName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblReadonlyCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOBText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label ID="lblReadonlyGenderText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label>
                </td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trECHKIDCreation" runat="server">
                <td valign="top">
                    <asp:Label ID="lblReadonlyECHKIDText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="150px"></asp:Label></td>
                <td valign="top">
                    <asp:Label ID="lblReadonlyECHKID" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyHorizontalEC" runat="server">
    <table cellpadding="0" cellspacing="0">
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
                <asp:Label ID="lblReadonlyHorizontalEName" runat="server" CssClass="tableText"></asp:Label><asp:Label
                    ID="lblReadonlyHorizontalCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalECSerialNoText" runat="server" CssClass="tableTitle"
                    Height="25px" Style="height: 28px" Width="175px"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalECSerialNo" runat="server" CssClass="tableText"
                    Width="300px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalECHKIDText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td style="width: 350px;" valign="top">
                <asp:Label ID="lblReadonlyHorizontalECHKID" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalECReferenceNoText" runat="server" CssClass="tableTitle"
                    Height="25px" Style="height: 28px" Width="175px"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblReadonlyHorizontalECReferenceNo" runat="server" CssClass="tableText"
                    Width="300px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalDOBGenderText" runat="server" CssClass="tableTitle"
                    Height="25px" Width="175px"></asp:Label></td>
            <td style="padding-bottom: 10px;" valign="top">
                <asp:Label ID="lblReadonlyHorizontalDOB" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblReadonlyHorizontalDOBGender" runat="server" CssClass="tableText">/</asp:Label>
                <asp:Label ID="lblReadonlyHorizontalGender" runat="server" CssClass="tableText"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalECDateText" runat="server" CssClass="tableTitle"
                    Height="25px" Style="height: 28px" Width="175px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px">
                <asp:Label ID="lblReadonlyHorizontalECDate" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
