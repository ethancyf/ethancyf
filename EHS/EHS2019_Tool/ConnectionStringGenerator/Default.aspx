<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ConnectionStringGenerator._Default"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ED</title>
    <style type="text/css">
        *
        {
            font-family: Arial;
            font-size: 10pt;
        }
        
        a:visited
        {
            color: #0000FF;
        }
        
        table td
        {
            vertical-align: top;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 900px; border: black 1px solid; background-color: #F5F5DC">
            <tr>
                <td style="padding: 15px">
                    <asp:Panel ID="panD" runat="server" DefaultButton="btnDDecrypt">
                        Connection String:
                        <br />
                        <asp:TextBox ID="txtDEncryptStr" runat="server" Width="850px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        Key:
                        <br />
                        <asp:TextBox ID="txtDKey" runat="server" Width="100px"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Button ID="btnDDecrypt" runat="server" Width="100px" Text="Decrypt" OnClick="btnDDecrypt_Click" />
                        &nbsp; &nbsp;                              
                        <asp:CheckBox ID="chkDFillEncryptionPart" runat="server" Text="Fill Encryption Part"
                            Checked="true" />
                        &nbsp; &nbsp;
                        Decryption Method:
                        <asp:RadioButtonList ID="rbDMethod" runat="server" RepeatDirection="Horizontal" style="display:inline" RepeatLayout="Flow">
                            <asp:ListItem Value="O">Old    </asp:ListItem>
                            <asp:ListItem Value="N" Selected="True">New</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <br />
                        <asp:TextBox ID="txtDResult" runat="server" Width="850px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 900px; border: black 1px solid; background-color: #F5F5DC">
            <tr>
                <td style="padding: 15px">
                    User ID:
                    <br />
                    <asp:TextBox ID="txtEUID" runat="server" Width="500px"></asp:TextBox>
                    <br />
                    Password:
                    <br />
                    <asp:TextBox ID="txtEPassword" runat="server" Width="500px"></asp:TextBox>
                    <br />
                    Data Source:
                    <br />
                    <asp:TextBox ID="txtESvrName" runat="server" Width="500px"></asp:TextBox>
                    <br />
                    Initial catalog:
                    <br />
                    <asp:TextBox ID="txtEDBName" runat="server" Width="500px"></asp:TextBox>
                    <br />
                    Max pool size:
                    <br />
                    <asp:TextBox ID="txtEConnSize" runat="server" Width="100px"></asp:TextBox>
                    <br />
                    Key:
                    <br />
                    <asp:TextBox ID="txtEKey" runat="server" Width="100px"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="btnEEncrypt" runat="server" Width="100px" Text="Encrypt" OnClick="btnEEncrypt_Click" />
                    &nbsp; &nbsp;
                    Encryption Method:
                    <asp:RadioButtonList ID="rbEMethod" runat="server" RepeatDirection="Horizontal" style="display:inline" RepeatLayout="Flow">
                        <asp:ListItem Value="O">Old</asp:ListItem>
                        <asp:ListItem Value="N" Selected="True">New</asp:ListItem>
                    </asp:RadioButtonList>
                    <br />
                    <br />
                    <asp:TextBox ID="txtEResult" runat="server" Width="850px" Height="70px" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <br />
                    Key:
                    <br />
                    <asp:TextBox ID="txtEEncryptKey" runat="server" Width="100px" ReadOnly="true" Style="background-color: #DDDDDD"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <a href=".">Reset</a>
    </div>
    </form>
</body>
</html>
