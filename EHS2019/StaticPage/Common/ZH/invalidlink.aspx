<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="invalidlink.aspx.vb" Inherits="HCSP.invalidlink1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>醫健通(資助)系統 - 未能找到網頁</title>
    <base id="basetag" runat="server" /> 
    <link href="../CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/Common.js"></script>
</head>
<body onload=setTimeout("location.href='../zh/index.htm'",15000)>
    <form id="form1" runat="server">
    <div>
        <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Common/Images/banner/banner_header.jpg);
            width: 990px; background-repeat: no-repeat; height: 100px" runat="server">
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    <br />
    <span style="font-size: 16pt;">未能找到網頁。 <br /> 系統將於15秒後自動返回<a href="../zh/index.aspx">首頁</a>。</span>
    </form>
</body>
</html>