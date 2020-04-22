<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start--->

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImproperAccess.aspx.vb" Inherits="HCSP.ZH.ImproperAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>醫健通(資助)系統 - 發現多於一個瀏覽器同時操作或不正當的操作</title>
    <link href="../CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/Common.js"></script>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header_tw.jpg); width: 990px; background-repeat: no-repeat; height: 100px"
                runat="server">
                <tr>
                    <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironmentZH">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <span style="font-size: 16pt; width: 950pt;">發現多於一個瀏覽器同時操作或不正當的操作，已中止目前的瀏覽器操作。詳情請參考<asp:LinkButton ID="LinkButtonFAQs" runat="server">常見問題</asp:LinkButton>。<br />
            請按 <a href="../login.aspx">此處</a> 重新登入。
            <br />
            或請按 <a href="javascript:window.close();">此處</a> 關閉目前的瀏覽器。
            <br />
        </span>
    </form>
</body>
</html>

<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] End--->
