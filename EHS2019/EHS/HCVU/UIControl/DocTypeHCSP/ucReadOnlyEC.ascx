<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyEC.ascx.vb"
    Inherits="HCVU.UIControl.DocTypeHCSP.ucReadOnlyEC" %>
<asp:Panel ID="pnlStep2ECHolder" runat="server" Visible="False">
    <table>
        <tr>
            <td style="padding-bottom: 5px; padding-top: 5px;">
            </td>
            <td style="padding-bottom: 5px; padding-top: 5px">
                <asp:Image ID="imgReadonlyECHolder" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/others/ec.png" /></td>
            <td style="padding-bottom: 5px; padding-top: 5px">
                <asp:Label ID="lblReadonlyECHolder" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyVerticalEC" runat="server">
    <asp:Panel ID="pnlReadonlyTempAccountRefNo" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"
                        Text=""></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    &nbsp;</td>
                <td valign="top" class="tableCellStyle">
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
                <td valign="top" runat="server" id="cellReadonlyCreationDateTimeText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyCreationDateTimeText" runat="server" CssClass="tableTitle"
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyCreationDateTime" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
    <table id="tblEC" runat="server" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td valign="top" runat="server" id="cellReadonlyDocumentTypeText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trECHKIDModication" runat="server">
                <td valign="top" runat="server" id="cellReadonlyECHKIDModificationText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECHKIDModificationText" runat="server" CssClass="tableTitle"
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECHKIDModification" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" runat="server" id="cellReadonlyECSerialNoText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECSerialNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" runat="server" id="cellReadonlyECReferenceNoText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECReferenceNoText" runat="server" CssClass="tableTitle"
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECReferenceNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" runat="server" id="cellReadonlyECDateText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECDateText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" runat="server" id="cellReadonlyNameText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyNameText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyEName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblReadonlyCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" runat="server" id="cellReadonlyDOBText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDOBText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" runat="server" id="cellReadonlyGenderText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyGenderText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label>
                </td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="trECHKIDCreation" runat="server">
                <td valign="top" runat="server" id="cellReadonlyECHKIDText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECHKIDText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyECHKID" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyHorizontalEC" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" runat="server" id="cellReadonlyHorizontalDocumentTypeText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocumentTypeText" runat="server" CssClass="tableTitle"
                    Width="160px"></asp:Label></td>
            <td colspan="3" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" runat="server" id="cellReadonlyHorizontalNameText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalNameText" runat="server" CssClass="tableTitle"
                    Width="160px"></asp:Label></td>
            <td style="width: 300px" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalEName" runat="server" CssClass="tableText"></asp:Label><asp:Label
                    ID="lblReadonlyHorizontalCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
            <td valign="top" runat="server" id="cellReadonlyHorizontalECSerialNoText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECSerialNoText" runat="server" CssClass="tableTitle"
                    Width="160px"></asp:Label></td>
            <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECSerialNo" runat="server" CssClass="tableText"
                    Width="300px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" runat="server" id="cellReadonlyHorizontalECHKIDText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECHKIDText" runat="server" CssClass="tableTitle"
                    Width="160px"></asp:Label></td>
            <td style="width: 300px;" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECHKID" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
            <td valign="top" runat="server" id="cellReadonlyHorizontalECReferenceNoText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECReferenceNoText" runat="server" CssClass="tableTitle"
                    Width="160px"></asp:Label></td>
            <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECReferenceNo" runat="server" CssClass="tableText"
                    Width="320px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" style="padding-bottom: 10px" runat="server" id="cellReadonlyHorizontalDOBGenderText"
                class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDOBGenderText" runat="server" CssClass="tableTitle"
                    Width="160px"></asp:Label></td>
            <td style="padding-bottom: 10px; width: 300px;" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDOB" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblReadonlyHorizontalDOBGender" runat="server" CssClass="tableText">/</asp:Label>
                <asp:Label ID="lblReadonlyHorizontalGender" runat="server" CssClass="tableText"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px" runat="server" id="cellReadonlyHorizontalECDateText"
                class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECDateText" runat="server" CssClass="tableTitle"
                    Width="160px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalECDate" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
