<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyAccountInfo.ascx.vb" Inherits="HCVU.ucReadOnlyAccountInfo" %>
<table>
    <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblRefNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RefNo %>"
                ></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblRefNo" runat="server" CssClass="tableText"></asp:Label></td>
        <td valign="top" style="width: 457px;">
        </td>
    </tr>
    <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblAcctTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountType %>"
                ></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblAcctType" runat="server" CssClass="tableText"></asp:Label></td>
        <td valign="top" style="width: 457px;">
        </td>
    </tr>
    <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblAcctStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountStatus %>"
                ></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblAcctStatus" runat="server" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblAcctStatusRemark" runat="server" CssClass="tableText"></asp:Label></td>
        <td valign="top" style="width: 457px">
        </td>
    </tr>
    <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblDeceasedText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Deceased %>"
                ></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblDeceased" runat="server" CssClass="tableText"></asp:Label>
        </td>
        <td valign="top" style="width: 457px">
        </td>
    </tr>

    <asp:Panel ID = "panEnquiry" runat ="server">
        <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblEnquiryStatusText" runat="server" CssClass="tableTitle"
                Text="<%$ Resources:Text, EnquiryStatus %>" ></asp:Label></td>
        <td  valign="top">
            <asp:Label ID="lblEnquiryStatus" runat="server" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblEnquiryStatusRemark" runat="server" CssClass="tableText"></asp:Label></td>
        <td valign="top" style="width: 457px">
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblCreatedByText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CreateBy %>"
                ></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblCreatedBy" runat="server" CssClass="tableText"></asp:Label></td>
        <td valign="top" style="width: 457px">
        </td>
    </tr>
    <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblCreatedDtmText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CreationTime %>"
                ></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblCreatedDtm" runat="server" CssClass="tableText"></asp:Label></td>
        <td valign="top" style="width: 457px">
        </td>
    </tr>
    <!--<tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblDeletedByText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DeletedBy %>"
                Visible="False"></asp:Label></td>
        <td colspan="2"  valign="top">
            <asp:Label ID="lblDeletedBy" runat="server" CssClass="tableText"  Visible="False"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 220px" valign="top">
            <asp:Label ID="lblDeletedDtmText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DeletedDtm %>"
                Visible="False"></asp:Label></td>
        <td colspan="2"  valign="top">
            <asp:Label ID="lblDeletedDtm" runat="server" CssClass="tableText"  Visible="False"></asp:Label></td>
    </tr>-->
</table>