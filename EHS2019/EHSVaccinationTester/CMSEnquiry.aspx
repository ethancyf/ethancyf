<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="CMSEnquiry.aspx.vb" Inherits="EHSVaccinationTester._CMSImmuEnquiry"  ValidateRequest="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td style="width: 379px;" valign="top">
                        <asp:TextBox ID="txtRequest" runat="server" Height="274px" Width="628px" Font-Names="Tahoma" TextMode="MultiLine"></asp:TextBox></td>
                    <td valign="top">
                        <table width="100%">
                                <tr>
                                    <td width="20%">
                                        <asp:Label ID="lblRequestFormat" runat="server" Text="Request Format:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbNewFormat" checked="true" AutoPostBack="True" GroupName="rbgRequestFormat" runat="server" Text="New Format"/>
                                        <asp:RadioButton ID="rbExistFormat" AutoPostBack="True" GroupName="rbgRequestFormat" runat="server" Text="Existing Format "/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%">
                                         <asp:Label ID="lblSample" runat="server" Text="Sample:"></asp:Label>
                                    </td>
                                    <td>
                                         <asp:DropDownList ID="ddlSample" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                                    </td>
                                </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                <td style="width: 379px">
                        <asp:TextBox ID="txtResult" runat="server" Height="274px" Width="627px" Font-Names="Tahoma" TextMode="MultiLine"></asp:TextBox><br />
                    Enquiry Time:
                    <asp:Label ID="lblEnquiryTime" runat="server" Text="--:--:---"></asp:Label></td>
                <td valign="top"><asp:Button ID="BtnQuery" runat="server" Text="Query" /></td></tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label>
                        <asp:Label ID="lblUsername" runat="server"></asp:Label><br />
                        <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
                        <asp:Label ID="lblPassword" runat="server"></asp:Label><br />
                        <asp:Label ID="Label3" runat="server" Text="URL:"></asp:Label><asp:Label ID="lblURL"
                            runat="server"></asp:Label><br />
                        <asp:Label ID="Label4" runat="server" Text="Exception:"></asp:Label><asp:Label ID="lblException"
                            runat="server"></asp:Label></td><td></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
