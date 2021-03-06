<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="index.aspx.vb" Inherits="eForm.index1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>eHealth System (Subsidies)</title>

    <script type="text/javascript" src="../JS/Common.js"></script>

    <base id="basetag" runat="server" />

    <script type="text/javascript">

        var wi = screen.availWidth || screen.width;
        var he = screen.availHeight || screen.height;

        window.moveTo(0, 0);
        window.resizeTo(wi, he);

        function goNewWin(l) {

            var win;
            var tmp;
            var w = screen.availWidth || screen.width;
            var h = screen.availHeight || screen.height;

            w = 0;
            h = 0;

            var opts;

            opts = 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0,width=' + w + ',height=' + h;
            win = window.open(l, '_blank', opts);

            while (!win.open) { }

            window.self.opener = window.self;
            /*window.self.close();*/
        }
    </script>

    <link href="../CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 990px">
            <tr>
                <td align="right" style="background-image: url(../Images/master/banner_header.jpg); background-repeat: no-repeat; height: 100px"
                    valign="top">
                    <table style="width: 100%; height: 100%">
                        <tr>
                            <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironment">
                                <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                            </td>
                            <td align="right" valign="top" style="white-space: nowrap">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="right" valign="top">
                                            <a href="../Zh/index.aspx">繁體</a>&nbsp; English
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="background-position: center bottom; background-image: url(../Images/master/body_background.jpg); background-repeat: repeat-x; height: 450px;"
                    valign="top" align="center">
                    <table style="width: 100%; height: 450px;" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" valign="top">
                                <%-- <table style="border-right: #0000ff 1px solid; border-top: #0000ff 1px solid; border-left: #0000ff 1px solid;
                                            border-bottom: #0000ff 1px solid">
                                            <tr>
                                                <td class="tableText" style="font-size: large; color: #0000ff;">
                                                    Important Message:<br />
                                                    There will be a maintenance schedule on October 12, 2008(Sunday) from 1a.m. to 5
                                                    a.m..<br />
                                                    We apologize for any inconvenience caused.</td>
                                            </tr>
                                        </table>
                                        <br />
                            <table width="100%">--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="75%">
                                    <tr align="left">
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <span style="color: #000099; font-size: x-large"><strong>Service Provider</strong></span><br />
                                            <br />
                                            <span style="color: #000099; font-size: x-large"></span>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <a href="javascript:void(0)" onclick="openNewWin('../main.aspx?lang=en')">
                                                <img src="../Images/others/enrolment.png" alt="" border="0" /></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <a href="javascript:void(0)" onclick="openNewWin('../../Documents/FAQs_e.pdf#Enrolment')">
                                                <img src="../Images/others/button_FAQs.png" alt="" border="0" /></a></td>
                                        <td align="center">
                                            <a href="javascript:void(0)" onclick="openNewWin('../../en/ContactUs.htm')">
                                                <img src="../Images/others/button_contactus.png" alt="" border="0" /></a></td>
                                        <%-- <td align="center">
                                            <a href="http://www.chp.gov.hk/view_content.asp?lang=en&info_id=16615" target="_blank">
                                                <img src="../Images/others/btn_swine.jpg" alt="" border="0" /></a></td>
                                        --%>
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
                            <td style="height: 2px">
                                <hr style="width: 98%; color: #ffffff; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">&nbsp;&nbsp;&nbsp; <a href="javascript:void(0)" onclick="openNewWin('../../EN/PrivacyPolicy.htm')"
                                class="footerText">Privacy Policy</a> <span class="footerText">| </span><a href="javascript:void(0)"
                                    onclick="openNewWin('../../EN/Disclaimer.htm')" class="footerText">Important Notices</a>
                                <span class="footerText">| </span><a href="javascript:void(0)" onclick="openNewWin('../../EN/SysMaint.htm')"
                                    class="footerText">System Maintenance</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
