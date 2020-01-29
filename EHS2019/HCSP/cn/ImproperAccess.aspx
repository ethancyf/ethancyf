<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImproperAccess.aspx.vb"
    Inherits="HCSP.CN.ImproperAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="zh">
<head id="Head2" runat="server">
    <title>医健通(资助)系統 - 发现多于一个浏览器同时操作或不正当的操作</title>
    <link href="../CSS/CommonStyle_cn.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/Common.js"></script>
</head>
<body>
    <form id="form2" runat="server">

        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header_cn.jpg); width: 990px; background-repeat: no-repeat; height: 100px"
                runat="server">
                <tr>
                    <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironmentZH">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <span style="font-size: 16pt; width: 950pt;">发现多于一个浏览器同时操作或不正当的操作，已中止目前的浏览器操作。<br />
            请按<a href="../login.aspx">此处</a> 重新登入。
            <br />
            或请按 <a href="javascript:window.close();">此处</a> 关闭目前的浏览器。
            <br />
        </span>
    </form>
</body>
</html>

