<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="udcDateType.ascx.vb"
    Inherits="HCVU.udcDateType" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panDateType" runat="server">
        <tr class="DateType_tr">
            <td style="width:150px;text-align:left;vertical-align:top" class="DateType_td_01">
                <asp:Label ID="lblDateType" runat="server" CssClass="DateType_td_01_label" Text="Date Type" />
            </td>
            <td style="text-align:left" class="DateType_td_02">
                <table style="border-collapse:collapse;padding:0px;border-spacing:0px" class="DateType_td_02_table">
                    <tr>
                        <td style="width: 235px; vertical-align: top">
                            <asp:RadioButtonList ID="rbtnDateType" runat="server" RepeatDirection="Horizontal"
                                CellSpacing="0" CellPadding="0" CssClass="DateType_RadioButtonList">
                                <asp:ListItem Text="Service Date" Value="S"></asp:ListItem>
                                <asp:ListItem Text="Transaction Date" Value="T"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 30px; vertical-align: top">
                            <asp:Image ID="imgErrorDateType" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" CssClass="DateType_Alert" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
</table>
