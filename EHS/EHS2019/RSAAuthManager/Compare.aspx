<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Compare.aspx.vb" Inherits="RSAAuthManager.Compare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="tHeader"></title>    
    <style type="text/css">
    * {
        font-family: Tahoma;
        font-size: 10pt;
    }
    
    .UGridView {
        border: 1px solid #000000;
    }
    
    .UGridView th, .UGridView td {
        border: 1px solid #000000;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                Compare by User ID</h2>
            <table>
                <tr>
                    <td>
                        User ID<br />
                        <asp:TextBox ID="txtUUserID" runat="server" Height="200" TextMode='MultiLine'></asp:TextBox>
                        <br />
                        <asp:CheckBox ID="cboUHide" runat="server" Checked="true" Text="Hide identical records" />
                    </td>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <asp:Button ID="btnUCompare" runat="server" Text="Compare" Height="40" OnClick="btnUCompare_Click" />
                    </td>
                    <td style="vertical-align: top">
                        Unmatched Records
                        <br />
                        <asp:GridView ID="gvU" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvU_RowDataBound"
                            class="UGridView" HeaderStyle-Height="25px">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        User ID</HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFFFF" />
                                    <ItemStyle BackColor="#FFFFFF" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGUserID" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>                                        
                                        <asp:Label ID="lblUGRSAAExistHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSAAExist" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>                                        
                                        <asp:Label ID="lblUGRSAATokenHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSAAToken" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="180" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUGRSAATokenEnableHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSAATokenEnable" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUGRSAANTMHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSAANTM" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUGRSABExistHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSABExist" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUGRSABTokenHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSABToken" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="180" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUGRSABTokenEnableHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSABTokenEnable" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblUGRSABNTMHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUGRSABNTM" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Label ID="lblUError" runat="server"></asp:Label>
                        <br />
                        <br />
                        <asp:LinkButton ID="lbtnUDownload" runat="server" OnClick="lbtnUDownload_Click" Visible="false">Download CSV</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <hr />
            <h2>
                Compare by Token Serial No.</h2>
            <table>
                <tr>
                    <td>
                        Token Serial No.<br />
                        <asp:TextBox ID="txtTToken" runat="server" Height="200" TextMode='MultiLine'></asp:TextBox>
                        <br />
                        <asp:CheckBox ID="cboTHide" runat="server" Checked="true" Text="Hide identical records" />
                    </td>
                    <td style="padding-left: 10px; padding-right: 10px">
                        <asp:Button ID="btnTCompare" runat="server" Text="Compare" Height="40" OnClick="btnTCompare_Click" />
                    </td>
                    <td style="vertical-align: top">
                        Unmatched Records
                        <br />
                        <asp:GridView ID="gvT" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvT_RowDataBound"
                            class="UGridView" HeaderStyle-Height="25px">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Serial Number</HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFFFF" />
                                    <ItemStyle BackColor="#FFFFFF" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGSerialNumber" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="120" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSAAExistHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSAAExist" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSAAEnableHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSAAEnable" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSAANTMHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSAANTM" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSAAUserIDHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#E7EFF7" />
                                    <ItemStyle BackColor="#E7EFF7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSAAUserID" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSABExistHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSABExist" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="60" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSABEnableHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSABEnable" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSABNTMHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSABNTM" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTGRSABUserIDHeader" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor="#FFFF57" />
                                    <ItemStyle BackColor="#FFFF57" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTGRSABUserID" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Label ID="lblTError" runat="server"></asp:Label>
                        <br />
                        <br />
                        <asp:LinkButton ID="lbtnTDownload" runat="server" OnClick="lbtnTDownload_Click" Visible="false">Download CSV</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
