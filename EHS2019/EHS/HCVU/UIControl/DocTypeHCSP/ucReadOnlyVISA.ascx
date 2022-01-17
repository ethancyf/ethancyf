<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyVISA.ascx.vb"
    Inherits="HCVU.UIControl.DocTypeHCSP.ucReadOnlyVISA" %>
<asp:Panel ID="panReadonlyVerticalVISA" runat="server">
    <asp:Panel ID="panReadonlyTempAccountRefNo" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td  valign="top"  runat="server" id="cellReadonlyRefenceText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td  valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"
                        Text=""></asp:Label></td>
                <td  valign="top" class="tableCellStyle">
                    &nbsp;</td>
                <td  valign="top" class="tableCellStyle">
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
                <td valign="top"  runat="server" id="cellReadonlyCreationDateTimeText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyCreationDateTimeText" runat="server" CssClass="tableTitle"
                         Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyCreationDateTime" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
    <table id="tblVISA" runat="server" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td valign="top"  runat="server" id="cellReadonlyDocumentTypeText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"
                          Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top"  runat="server" id="cellReadonlyVISANoText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyVISANoText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyVISANo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top"  runat="server" id="cellReadonlyPassportNoText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyPassportNoText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadOnlyPassportNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top"  runat="server" id="cellReadonlyNameText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyNameText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyEName" runat="server" CssClass="tableText" />
                    <asp:Label ID="lblReadonlyCName" runat="server" CssClass="tableText TextChineseName" />
                </td>
            </tr>
            <tr>
                <td valign="top"  runat="server" id="cellReadonlyGenderText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyGenderText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top"  runat="server" id="cellReadonlyDOBText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDOBText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyHorizontalVISA" runat="server">
    <table id="TABLE1" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top"  runat="server" id="cellReadonlyHorizontalDocumentTypeText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocumentTypeText" runat="server" CssClass="tableTitle"
                      Width="160px"></asp:Label></td>
            <td colspan="3" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top"  runat="server" id="cellReadonlyHorizontalNameText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalNameText" runat="server" CssClass="tableTitle"
                     Width="160px"></asp:Label></td>
            <td style="width: 300px" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalEName" runat="server" CssClass="tableText" />
                <asp:Label ID="lblReadonlyHorizontalCName" runat="server" CssClass="tableText TextChineseName" />
            </td>
            <td valign="top"  runat="server" id="cellReadonlyHorizontalDOBGenderText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDOBGenderText" runat="server" CssClass="tableTitle"
                     Width="160px"></asp:Label></td>
            <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDOB" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblReadonlyHorizontalDOBGender" runat="server" CssClass="tableText">/</asp:Label>
                <asp:Label ID="lblReadonlyHorizontalGender" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" style="padding-bottom: 10px"  runat="server" id="cellReadonlyHorizontalVISANoText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalVISANoText" runat="server" CssClass="tableTitle"
                     Width="160px"></asp:Label></td>
            <td style="padding-bottom: 10px; width: 300px;" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalVISANo" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px"  runat="server" id="cellReadonlyHorizontalPassportNoText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalPassportNoText" runat="server" CssClass="tableTitle"
                     Width="160px"></asp:Label>
            </td>
            <td style="padding-bottom: 10px" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalPassportNo" runat="server" CssClass="tableText"
                    Width="300px"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
