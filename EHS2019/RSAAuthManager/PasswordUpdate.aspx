<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PasswordUpdate.aspx.vb"
    Inherits="RSAAuthManager.PasswordUpdate" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RSA 7.1 Password Update</title>    
    <style type="text/css">
    * {
        font-family: Tahoma;
        font-size: 8pt;
    }
    
    .SPGridView {
        border: 1px solid #000000;
    }
    
    .SPGridView th, .SPGridView td {
        border: 1px solid #000000;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <cc1:ToolKitScriptManager ID="ScriptManager1" runat="server">
            </cc1:ToolKitScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <h2>
                        Enquiry</h2>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnEEnquire" runat="server" Text="Enquire" OnClick="btnEEnquire_Click" />
                                <asp:Button ID="btnEClear" runat="server" Text="Clear" OnClick="btnEClear_Click" />
                                &nbsp; &nbsp;
                                <asp:Label ID="lblEResult" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Where
                            </td>
                            <td>
                                <asp:CheckBox ID="cboEPasswordDate" runat="server" Text="Password Change Date" />
                                &nbsp; &nbsp;
                                <asp:RadioButtonList ID="rblEPasswordDate" runat="server" RepeatLayout="flow" RepeatDirection="horizontal">
                                    <asp:ListItem Text="before" Value="<" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="after" Value=">"></asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp; &nbsp; &nbsp;
                                <asp:TextBox ID="txtEPasswordDate" runat="server" Width="130"></asp:TextBox>
                                &nbsp;
                                <asp:Label ID="lblEPasswordDateHint" runat="server" Text="(yyyy-mm-dd hh:mm:ss)"
                                    ForeColor="gray"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <strong>AND</strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: top">
                                            <asp:CheckBox ID="cboEUserID" runat="server" Text="User ID" /></td>
                                        <td style="vertical-align: top; padding: 5px 15px 0px 15px">
                                            in</td>
                                        <td style="vertical-align: top">
                                            <asp:TextBox ID="txtEUserID" runat="server" Height="200" TextMode="MultiLine"></asp:TextBox></td>
                                        <td style="vertical-align: top; padding-left: 10px">
                                            <asp:Label ID="lblEUserIDError" runat="server" ForeColor="red"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Order by
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblESort" runat="server" RepeatLayout="flow" RepeatDirection="horizontal">
                                    <asp:ListItem Text="Password Change Date" Value="P" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="User ID" Value="U"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblESortDirection" runat="server" RepeatLayout="flow" RepeatDirection="horizontal">
                                    <asp:ListItem Text="ASC" Value="ASC" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="DESC" Value="DESC"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gvE" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvE_RowDataBound"
                                    class="SPGridView" HeaderStyle-Height="25px" RowStyle-BackColor="#FFFFFF" AlternatingRowStyle-BackColor="#FFE5FA">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                User ID</HeaderTemplate>
                                            <HeaderStyle BackColor="#E7EFF7" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPGUserID" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="160" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                Enable</HeaderTemplate>
                                            <HeaderStyle BackColor="#E7EFF7" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPGEnable" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                Fail Count</HeaderTemplate>
                                            <HeaderStyle BackColor="#E7EFF7" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPGFailCount" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                Lockout</HeaderTemplate>
                                            <HeaderStyle BackColor="#E7EFF7" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPGLockout" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Password Change Date</HeaderTemplate>
                                            <HeaderStyle BackColor="#E7EFF7" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPGPasswordDate" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="150" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
