<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InfoCategory.aspx.vb" Inherits="HCSP.InfoCategory" Title="<%$ Resources:Text, Category %>" %>

<link href="./CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
<script language="javascript" src="./JS/jquery-3.4.1.min.js" type="text/javascript"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Category</title>
</head>
<body>
    <form runat="server">

        <table cellpadding="0" cellspacing="0" border="0" style="padding: 5px;" >
            <tr>
                <td>
                    <asp:Table ID="tbCategory" runat="server" BorderStyle="Solid" BorderColor="Black" BorderWidth="1" GridLines="Both">
                        <asp:TableRow runat="server" BackColor="Gray">
                            <asp:TableCell ID="tcCategoryTitle" Style="text-align: center; color: white"></asp:TableCell>
                            <asp:TableCell ID="tcSubCategoryTitle" Style="text-align: center; color: white"></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; padding-top: 5px;">
                    <asp:Label ID="lblEffectiveDateText" runat="server"></asp:Label>
                    <asp:Label ID="lblEffectiveDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    <asp:Label ID="lblRemart" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; padding-top: 5px;">
                    <asp:ImageButton ID="ibtnClose" runat="server" autopostback="False" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

<script type="text/javascript" language="javascript">
    $(function () {
        $(document).on('click', "[id$='ibtnClose']", function () {
            window.close()

        });
    });
</script>
