﻿<%@ Page Language="vb" AutoEventWireup="false" Codebehind="invalidlink.aspx.vb" Inherits="HCSP.Text.ZH.invalidlink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>醫健通(資助)系統 - 未能找到網頁</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server" style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="醫健通(資助)系統"
                            Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">未能找到網頁。
                            <br />
                            請按 <a href="../login.aspx">此處</a>
                            重新登入。</span>
                    </td>
                </tr>
            </table>
        </div>
        <br />
    </form>
</body>
</html>
