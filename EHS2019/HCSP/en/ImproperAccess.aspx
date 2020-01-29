<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start--->

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImproperAccess.aspx.vb" Inherits="HCSP.EN.ImproperAccess" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>eHealth System (Subsidies) - Concurrent access or improper access detected</title>
    <link href="../CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/Common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header.jpg); width: 990px; background-repeat: no-repeat; height: 100px"
                runat="server">
                <tr>
                    <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironment">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <span style="font-size: 16pt; width: 950pt;">Concurrent access or improper access detected, the action of this browser window is aborted. Please refer<br />
            to 
        <asp:LinkButton ID="LinkButtonFAQs" runat="server">FAQs</asp:LinkButton>
            for details.
            <br />
            Please click <a href="../login.aspx">here</a> to back to login page,
            <br />
            or click <a href="javascript:window.close();">here</a> to close this browser window.
            <br />
        </span>
    </form>
</body>
</html>

<!---[CRE11-016] Concurrent Browser Handling [2010-02-01] End--->
