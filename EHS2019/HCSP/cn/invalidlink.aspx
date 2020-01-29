<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="invalidlink.aspx.vb" Inherits="HCSP.CN.InvalidLink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" lang="zh">
<head id="Head1" runat="server">
    <title>医健通(资助)系統 - 未能找到网页</title>
    <base id="basetag" runat="server" /> 
    <link href="../CSS/CommonStyle_cn.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/Common.js"></script>
</head>
<body onload=setTimeout("location.href='../zh/index.aspx'",15000)>
    <form id="form1" runat="server">
    <div>
        <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header_cn.jpg);
            width: 990px; background-repeat: no-repeat; height: 100px" runat="server">
            <tr>
                <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironmentZH">
                    <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <span style="font-size: 16pt;">未能找到网页。 <br /> 系统将于15秒后自动返回<a href="../cn/index.aspx">首页</a>。</span>
    </form>
</body>
</html>