<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHSIVSS.ascx.vb"
    Inherits="HCSP.ucInputHSIVSS" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table runat="server" id="tblHSIVSSDetail" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td valign="top">
            <asp:Panel ID="panHSIVSSDetail" runat="server">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" style="width: 160px">
                            <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle" Height="25px"
                                Width="160px"></asp:Label></td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <asp:RadioButtonList ID="rbCategorySelection" runat="server" AutoPostBack="True"
                                            RepeatDirection="Vertical" CssClass="tableText">
                                        </asp:RadioButtonList></td>
                                    <td valign="top">
                                        <asp:Image ID="imgCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                </tr>
                            </table>
                            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<asp:Panel ID="panPreConditions" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 160px" valign="top">
                <asp:Label ID="lblPreConditionsText" runat="server" CssClass="tableTitle" Width="160px"
                    Height="25px"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlPreConditionSelection" runat="server">
                </asp:DropDownList><asp:Image ID="imgPreConditionsError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
        </tr>
    </table>
</asp:Panel>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="padding-bottom: 3px">
        </td>
    </tr>
</table>
<cc1:ClaimVaccineInput ID="udcClaimVaccineInputHSIVSS" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
