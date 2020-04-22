<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyPPP.ascx.vb" Inherits="HCVU.ucReadOnlyPPP" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<table cellpadding="0" cellspacing="0">
    <tr style="height: 22px">
        <td id="tdCategory" runat="server" style="vertical-align: top">
            <asp:Label ID="lblCategoryText" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trSchoolCode" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblSchoolCodeText" runat="server" Text="<%$ Resources:Text, SchoolCode %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblSchoolCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trSchoolName" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblSchoolNameText" runat="server" Text="<%$ Resources:Text, SchoolName %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblSchoolName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>

</table>

<cc1:ClaimVaccineReadOnly ID="udcReadOnlyVaccine" runat="server" />
