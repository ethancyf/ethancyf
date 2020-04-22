<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProperPractice.aspx.vb"
    Inherits="HCSP.ProperPractice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" <%=PageLanguage%>>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="pragma" content="no-cache" />
    <base id="basetag" runat="server" />
    <title>
        <asp:Literal ID="litTitle" runat="server" Text="<%$ Resources: Title, ProperPractice %>"></asp:Literal>
    </title>
    <link href="../CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        tr.TrStyle1 {
            height: 30px;
        }

        a.LinkToDH {
            color: #0066CC;
            font-size: 16px;
            font-weight: bold;
        }

            a.LinkToDH:visited {
                color: #0066CC;
            }
    </style>
    <script language="javascript" src="../JS/Common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 982px">
                <tr>
                    <td id="tdHeader" runat="server" style="background-image: url(../Images/master/banner_header.jpg); background-repeat: no-repeat; height: 100px"
                        valign="top">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="background-position: center bottom; background-image: url(../Images/master/background.jpg); background-repeat: repeat-x; height: 500px;"
                        valign="top">
                        <table style="width: 100%; height: 500px;" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" style="width: 10px"></td>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Image ID="imgBanner" runat="server" ImageUrl="<%$ Resources: ImageUrl, ProperPracticeBanner %>"
                                                    AlternateText="<%$ Resources: AlternateText, ProperPracticeBanner %>" />
                                            </td>
                                        </tr>
                                        <tr style="height: 5px">
                                        </tr>
                                        <tr id="trPP01" runat="server">
                                            <td>
                                                <table style="width: 100%">
                                                    <tr class="TrStyle1">
                                                        <td style="width: 30px">
                                                            <img src="../Images/others/world.png" alt="" />
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lbtnPP01" runat="server" CssClass="LinkToDH"
                                                                Text="<%$ Resources: Text, ProperPractice_PP01_Text %>"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trPP02" runat="server">
                                            <td>
                                                <table style="width: 100%">
                                                    <tr class="TrStyle1">
                                                        <td style="width: 30px">
                                                            <img src="../Images/others/world.png" alt="" />
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lbtnPP02" runat="server" CssClass="LinkToDH"
                                                                Text="<%$ Resources: Text, ProperPractice_PP02_Text %>"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 40px">
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnClose" runat="server" ImageUrl="<%$ Resources: ImageUrl, CloseBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, CloseBtn %>"
                                                    OnClientClick="javascript:window.close();"
                                                    Style="cursor: pointer" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: #d6d6d6;" align="center">
                        <table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="2" style="height: 2px">
                                    <hr style="width: 98%; color: #ffffff; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:LinkButton ID="lnkBtnPrivacyPolicy" runat="server" CssClass="footerText" Text="<%$ Resources:Text, PrivacyPolicy %>"
                                        TabIndex="-1"></asp:LinkButton>
                                    <asp:Label ID="lblseparator1" runat="server" CssClass="footerText" Text=" | "></asp:Label>
                                    <asp:LinkButton ID="lnkBtnDisclaimer" runat="server" CssClass="footerText" Text="<%$ Resources:Text, ImportantNotices %>"
                                        TabIndex="-1"></asp:LinkButton>
                                    <asp:Label ID="lblseparator2" runat="server" CssClass="footerText" Text=" | "></asp:Label>
                                    <asp:LinkButton ID="lnkBtnSysMaint" runat="server" CssClass="footerText" Text="<%$ Resources:Text, SysMaint %>"
                                        TabIndex="-1"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <script type="text/javascript">
                function ResizeScreen() {
                    var w = screen.availWidth || screen.width;
                    var h = screen.availHeight || screen.height;

                    window.moveTo(0, 0);
                    window.resizeTo(w, h);
                }

                ResizeScreen();
            </script>

        </div>
    </form>
</body>
</html>
