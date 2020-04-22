<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="udcCheckBox.ascx.vb"
    Inherits="HCVU.udcCheckBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panChkItem" runat="server">
        <tr class="CheckBox_tr">
            <td align="left" style="width: 220px; vertical-align: top" class="CheckBox_LabelWidth">
                <asp:Label ID="lblChkItemText" runat="server" Text=""></asp:Label>
            </td>
            <td align="left" style="width: 600px; vertical-align: top" class="CheckBox_ValueWidth">
                <asp:CheckBox ID="chkItem" runat="server" />
            </td>
        </tr>
    </asp:Panel>
</table>

