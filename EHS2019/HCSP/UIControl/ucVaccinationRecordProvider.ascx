<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucVaccinationRecordProvider.ascx.vb"
    Inherits="HCSP.ucVaccinationRecordProvider" %>
<br />
<asp:GridView ID="gvDocument" runat="server" AutoGenerateColumns="False" Width="98%"
    OnRowDataBound="gvDocument_RowDataBound">
    <Columns>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" BorderColor="Black" CssClass="tableTopCellVaccinationRecordProvider" />
            <ItemStyle VerticalAlign="Top" CssClass="tableCellVaccinationRecordProvider" />
            <ItemTemplate>
                <asp:Label ID="lblDocumentName" runat="server" Text='<%# Bind("Document_Name") %>'></asp:Label>
                <asp:HiddenField ID="hfDocumentCode" runat="server" Value='<%# Bind("Document_Code") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" BorderColor="Black" CssClass="tableTopCellVaccinationRecordProvider" />
            <ItemStyle Width="120px" VerticalAlign="Top" HorizontalAlign="Center" CssClass="tableCellVaccinationRecordProvider" />
            <ItemTemplate>
                <asp:Image ID="imgProvider1" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" BorderColor="Black" CssClass="tableTopCellVaccinationRecordProvider" />
            <ItemStyle Width="120px" VerticalAlign="Top" HorizontalAlign="Center" CssClass="tableCellVaccinationRecordProvider" />
            <ItemTemplate>
                <asp:Image ID="imgProvider2" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" BorderColor="Black" CssClass="tableTopLastCellVaccinationRecordProvider" />
            <ItemStyle Width="120px" VerticalAlign="Top" HorizontalAlign="Center" CssClass="tableLastCellVaccinationRecordProvider" />
            <ItemTemplate>
                <asp:Image ID="imgProvider3" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<table style="width: 95%">
    <tr>
        <td style="vertical-align: top">
            <asp:Label ID="lblNoteText" runat="server" Text="*" Font-Size="9pt"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblNote" runat="server" Text="<%$ Resources: Text, VaccinationRecordProviderNote %>"
                Font-Size="9pt"></asp:Label>
        </td>
    </tr>
</table>
