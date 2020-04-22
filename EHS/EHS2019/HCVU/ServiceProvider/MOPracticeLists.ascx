<%@ Control Language="vb" AutoEventWireup="false" Codebehind="MOPracticeLists.ascx.vb"
    Inherits="HCVU.MOPracticeLists" %>
<div class="headingText">
    <asp:Label ID="lblTitle" runat="server"></asp:Label>
</div>
<asp:GridView ID="gvMO" runat="server" AutoGenerateColumns="false" ShowHeader="true"
    Width="100%" Visible="False">
    <Columns>
    <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationName %>">
            <ItemTemplate>
                <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEngName") %>'></asp:Label></td>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Text, MONo %>">
            <ItemTemplate>
                <asp:Label ID="lblMOIndex" runat="server" Text='<%# Bind("DisplaySeq") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        
    </Columns>
</asp:GridView>
<asp:GridView ID="gvPractice" runat="server" AutoGenerateColumns="False" ShowHeader="true"
    Width="100%" Visible="False">
    <Columns>
    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeName %>">
            <ItemTemplate>
                <asp:Label ID="lblPracitceEName" runat="server" Text='<%# Bind("PracticeName") %>'></asp:Label></td>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
            <ItemTemplate>
                <asp:Label ID="lblPracticeIndex" runat="server" Text='<%# Bind("DisplaySeq") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        
    </Columns>
</asp:GridView>
