<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start--->

<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ImproperAccess.aspx.vb"
    Inherits="HCSP.Text.ZH.ImproperAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>醫健通(資助)系統 - 發現多於一個瀏覽器同時操作或不正當的操作</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="Label1" runat="server" Text="醫健通(資助)系統" Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <span style="font-size: 12pt;">
                            發現多於一個瀏覽器同時操作或不正當的操作，已中止目前的瀏覽器操作。詳情請參考常見問題。<br />
                            請按 <a href="../login.aspx">此處</a> 重新登入。<br />
                            或請按 <a href="javascript:window.close();">此處</a> 關閉目前的瀏覽器。<br />
                        </span>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] End--->
