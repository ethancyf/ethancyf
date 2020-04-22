<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="parametermaintenance.aspx.vb" Inherits="HCVU.parametermaintenance"
    Title="<%$ Resources:Title, ParameterMaintenance %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Image ID="imgParameterManagementBanner" runat="server" ImageUrl="<%$ Resources:ImageUrl, ParameterMaintenanceBanner %>"
                            AlternateText="<%$ Resources:AlternateText, parameterMaintenanceBanner %>"></asp:Image>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc2:MessageBox ID="udcMessageBox" runat="server"></cc2:MessageBox>
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server"></cc2:InfoMessageBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvParameterMaintenance" runat="server" AutoGenerateColumns="False"
                            Width="1135px" AllowSorting="True" OnRowDataBound="gvParameterMaintenance_RowDataBound"
                            OnPreRender="gvParameterMaintenance_PreRender" OnSorting="gvParameterMaintenance_Sorting"
                            OnRowEditing="gvParameterMaintenance_RowEditing" OnRowCancelingEdit="gvParameterMaintenance_RowCancelingEdit"
                            OnRowUpdating="gvParameterMaintenance_RowUpdating">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRow" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category" SortExpression="Category">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategory" runat="server" Text='<%# eval("Category") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="65px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parameter ID"  SortExpression="Parameter_Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblParameterID" runat="server" Text='<%# Eval("Parameter_Name")%>'></asp:Label>
                                        <asp:HiddenField ID="hfGKey" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="370px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parameter Description">
                                    <ItemTemplate>
                                        <table id="tabParameterDescription" cellpadding="0" cellspacing="0" runat="server"
                                            width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblParameterDescription" runat="server" Text='<%# eval("Description") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parameter Value" >
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtParameterValue" runat="server" Text='<%# eval("Parm_Value1") %>'
                                            Width="234px" Height="43px" TextMode="MultiLine" ReadOnly="true" BackColor="Transparent" BorderStyle="None"></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtGParameterValue" runat="server" Text='<%# eval("Parm_Value1") %>'
                                            Width="230px" Height="43px" TextMode="MultiLine" BackColor="White" MaxLength="255"></asp:TextBox>
                                        <asp:Image ID="imgParameterValueError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Width="20px"></asp:Image>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="125px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parameter Boundary">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGParameterBoundary" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnGEdit" runat="server" CommandName="Edit" ImageUrl="<%$ Resources: ImageUrl, EditSBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, EditSBtn %>" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ibtnGSave" runat="server" CommandName="Update" ImageUrl="<%$ Resources: ImageUrl, SaveSBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, SaveSBtn %>" />
                                        <div style="height: 10px"></div>
                                        <asp:ImageButton ID="ibtnGCancel" runat="server" CommandName="Cancel" ImageUrl="<%$ Resources: ImageUrl, CancelSBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelSBtn %>" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
