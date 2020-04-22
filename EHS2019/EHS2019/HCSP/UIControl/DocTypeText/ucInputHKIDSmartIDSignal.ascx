<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKIDSmartIDSignal.ascx.vb"
    Inherits="HCSP.UIControl.DocTypeText.ucInputHKIDSmartIDSignal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
</script>
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="tableCellStyle" valign="top" style="width: 100%">
                <asp:Label ID="lblDocumentTypeText" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" valign="top" style="width: 100%">
                <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" class="tableCellStyle" style="width: 100%">
                <asp:Label ID="lblHKICNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" valign="top" style="width: 100%">
                <asp:Label ID="lblHKICNo" runat="server" CssClass="tableText" MaxLength="9"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 100%;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblENameText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" style="height: 19px; width: 100%;" valign="top">
                <asp:Label ID="lblEName" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblCName" runat="server" Font-Names="HA_MingLiu"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" style="width: 100%;" valign="top">
                <asp:Label ID="lblCCCodeText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" style="width: 100%;" valign="top">
                <asp:Panel ID="panCCCode" runat="server">
                <asp:Label ID="lblCCCode1" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                    <asp:Label ID="lblCCCode2" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                    <asp:Label ID="lblCCCode3" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                    <asp:Label ID="lblCCCode4" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                    <asp:Label ID="lblCCCode5" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                    <asp:Label ID="lblCCCode6" runat="server" CssClass="tableText" Width="35px"></asp:Label></asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tableCellStyle" valign="top" style="width: 100%">
                <asp:Label ID="lblDOBText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" valign="top" style="width: 100%">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" class="tableCellStyle" style="width: 100%">
                <asp:Label ID="lblGenderText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" valign="top" style="width: 100%">
                <asp:Label ID="lblGender" runat="server" CssClass="tableText" Visible="false"></asp:Label>
                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" BackColor="#FFFF99" Visible="false">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Label ID="lblGenderSmartIDError" runat="server" CssClass="validateFailText"
                    Text="*"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" class="tableCellStyle" style="width: 100%">
                <asp:Label ID="lblDOIText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" valign="top" style="width: 100%">
                <asp:Label ID="lblDOI" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
