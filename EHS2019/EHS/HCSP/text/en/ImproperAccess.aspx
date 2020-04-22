<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start--->

<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ImproperAccess.aspx.vb"
    Inherits="HCSP.Text.EN.ImproperAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>eHealth System (Subsidies) - Concurrent access or improper access detected</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server" style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="eHealth System (Subsidies)" Font-Underline="True"
                            Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">
                            Concurrent access or improper access detected, the action of this browser window is aborted. Please refer to FAQs for details.<br />
                            Please click <a href="../login.aspx">here</a> to back to login page,<br />
                            or click <a href="javascript:window.close();">here</a> to close this browser window.<br />
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <br />
    </form>
</body>
</html>
<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] End--->
