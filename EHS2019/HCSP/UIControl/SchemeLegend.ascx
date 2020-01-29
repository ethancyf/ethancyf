<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SchemeLegend.ascx.vb" Inherits="HCSP.SchemeLegend" %>
<br />
<asp:CheckBox ID="chkDisplayInactive" runat="server" AutoPostBack="true" Text="<%$ Resources:Text, ShowInactiveLegend %>" Checked="false"/>
<asp:GridView ID="gvSchemeNameHelpScheme" runat="server" Width="550px">
</asp:GridView>
<br />

