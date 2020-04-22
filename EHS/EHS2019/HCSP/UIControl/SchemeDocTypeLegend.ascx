<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SchemeDocTypeLegend.ascx.vb"
    Inherits="HCSP.SchemeDocTypeLegend" %>
<asp:GridView ID="gvDocument" runat="server" AutoGenerateColumns="False" Width="98%"
    OnRowDataBound="gvDocument_RowDataBound"
    RowStyle-BorderColor="Black" RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="1px"
    HeaderStyle-BorderColor="Black" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px">
    <Columns>
        <asp:TemplateField HeaderText="Document Type (Identity Document No.) (Controlled code-behind)">
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="252px" VerticalAlign="Top" />
            <ItemTemplate>
                <asp:Label ID="lblDocumentName" runat="server" Text='<%# Bind("Document_Name") %>'></asp:Label>
                <asp:HiddenField ID="hfDocumentCode" runat="server" Value='<%# Bind("Document_Code") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme1" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme2" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme3" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme4" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme5" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme6" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme7" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme8" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle VerticalAlign="Top" />
            <ItemStyle Width="140px" VerticalAlign="Top" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:Image ID="imgScheme9" runat="server" ImageUrl="<%$ Resources: ImageUrl, Tick %>"
                    Visible="False" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<table style="width: 95%">
    <tr>
        <td style="vertical-align: top; padding-top: 5px">
            <asp:Label ID="lblHSIVSSRemark" runat="server" Font-Size="9pt" Visible="False"></asp:Label>
        </td>
    </tr>
</table>
