<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyCommon.ascx.vb"
    Inherits="HCSP.ucReadOnlyCommon" %>
<asp:Panel ID="panReadonlyVertical" runat="server">
    <asp:Panel ID="panReadonlyTempAccountRefNo" runat="server">
        <table style="border-width:0px;border-collapse:collapse;padding:0px 0px 0px 0px">
            <tr>
                <td style="vertical-align:top" runat="server" id="cellReadonlyRefenceText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyRefenceText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"
                        Text=""></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    &nbsp;</td>
                <td style="vertical-align:top" class="tableCellStyle">
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
        <table style="border-width:0px;border-collapse:collapse;padding:0px 0px 0px 0px">
            <tr>
                <td style="vertical-align:top"  runat="server" id="cellReadonlyCreationDateTimeText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyCreationDateTimeText" runat="server" CssClass="tableTitle"
                         Width="150px"></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyCreationDateTime" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
    <table id="tblDoc" runat="server" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td style="vertical-align:top" runat="server" id="cellReadonlyDocumentTypeText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocumentTypeText" runat="server" CssClass="tableTitle"
                          Width="150px"></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align:top" runat="server" id="cellReadonlyRegNoText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocNoText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDocNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>            
            <tr>
                <td style="vertical-align:top" runat="server" id="cellReadonlyNameText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyNameText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyEName" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align:top" runat="server" id="cellReadonlyGenderText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyGenderText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align:top" runat="server" id="cellReadonlyDOBText" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDOBText" runat="server" CssClass="tableTitle" 
                        Width="150px"></asp:Label></td>
                <td style="vertical-align:top" class="tableCellStyle">
                    <asp:Label ID="lblReadonlyDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<asp:Panel ID="panReadonlyHorizontal" runat="server">
    <table id="TABLE1" style="border-width:0px;border-collapse:collapse;padding:0px 0px 0px 0px">
        <tr>
            <td style="vertical-align:top" runat="server" id="cellReadonlyHorizontalDocumentTypeText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocumentTypeText" runat="server" CssClass="tableTitle"
                      Width="160px"></asp:Label></td>
            <td colspan="3" style="vertical-align:top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td style="vertical-align:top" runat="server" id="cellReadonlyHorizontalNameText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalNameText" runat="server" CssClass="tableTitle"
                     Width="160px"></asp:Label></td>
            <td style="vertical-align:top;width: 301px; word-wrap: break-word; word-break:break-all" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalEName" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
            <td style="vertical-align:top"  runat="server" id="cellReadonlyHorizontalDOBGenderText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDOBGenderText" runat="server" CssClass="tableTitle"
                     Width="160px"></asp:Label></td>
            <td style="vertical-align:top" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDOB" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblReadonlyHorizontalDOBGender" runat="server" CssClass="tableText">/</asp:Label>
                <asp:Label ID="lblReadonlyHorizontalGender" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td style="vertical-align:top;padding-bottom: 10px"  runat="server" id="cellReadonlyHorizontalRegNoText" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocNoText" runat="server" CssClass="tableTitle"
                     Width="160px"></asp:Label></td>
            <td style="vertical-align:top;padding-bottom: 10px; width: 301px" class="tableCellStyleLite">
                <asp:Label ID="lblReadonlyHorizontalDocNo" runat="server" CssClass="tableText" Width="300px"></asp:Label></td>
            <td style="vertical-align:top;padding-bottom: 10px" class="tableCellStyleLite">
            </td>
            <td style="vertical-align:top;padding-bottom: 10px" class="tableCellStyleLite">
            </td>
        </tr>
    </table>
</asp:Panel>
