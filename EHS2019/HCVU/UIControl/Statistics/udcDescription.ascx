<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcDescription.ascx.vb"
    Inherits="HCVU.udcDescription" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panDescription" runat="server">
        <tr class="CheckBox_tr">
            <td id="tdDescriptionText" runat="server"  align="left" style="width: 220px; vertical-align: top" class ="Description_LabelWidth">
                <asp:Label ID="lblDescriptionText" runat="server" Text=""></asp:Label>
            </td>
            <td id="tdDescription" runat="server" align="left" style="width: 600px;" class ="Description_ValueWidth">
                
            </td>
        </tr>
    </asp:Panel>
</table>

