<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="index.aspx.vb" Inherits="HCSP.index2"
    ContentType="text/html; charset=UTF-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">

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

            <style type="text/css">
                .LinkStyle1 {
                    display: block;
                    width: 170px;
                    text-decoration: none;
                }

                .LinkTextStyle1 {
                    color: #000099;
                    font-size: 14pt;
                    font-weight: bold;
                }
            </style>

            <script type="text/javascript" src="../JS/Common.js"></script>

            <table border="0" cellpadding="0" cellspacing="0" style="width: 990px">
                <tr>
                    <td align="right" style="background-image: url(../Images/master/banner_header_tw.jpg); background-repeat: no-repeat; height: 100px"
                        valign="top">
                        <table style="width: 100%; height: 100%">
                            <tr>
                                <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironmentZH">
                                    <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                                </td>
                                <td align="right" valign="top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right" valign="top" style="white-space: nowrap">
                                                繁體&nbsp; <a href="../EN/index.aspx">English</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-position: center bottom; background-image: url(../Images/master/background.jpg); background-repeat: repeat-x; height: 450px;"
                        valign="top">
                        <table style="width: 100%; height: 450px;" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" style="width: 10px"></td>
                                <td valign="top" align="center">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <table width="75%">
                                                    <tr align="left">
                                                        <td colspan="2"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <span style="color: #000099; font-size: x-large"><strong>服務提供者</strong></span><br />
                                                            <br />
                                                            <span style="color: #000099; font-size: x-large"></span>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <a href="javascript:void(0)" onclick="openNewWin2('../login.aspx?lang=zh')">
                                                                <img src="../Images/others/sp_logon_tw.png" alt="" border="0" /></a>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 30px"></tr>
                                                    <tr>
                                                        <td align="center">
                                                            <a href="javascript:void(0)" onclick="openNewWin('../../Documents/FAQs_c.pdf#SP')" class="LinkStyle1">
                                                                <img src="../Images/others/button_FAQs.png" alt="" border="0" />
                                                                <br />
                                                                <span class="LinkTextStyle1">常見問題</span>
                                                            </a>
                                                        </td>
                                                        <td align="center">
                                                            <a href="javascript:void(0)" onclick="openNewWin('../../zh/ContactUs.htm')" class="LinkStyle1">
                                                                <img src="../Images/others/button_contactus.png" alt="" border="0" />
                                                                <br />
                                                                <span class="LinkTextStyle1">聯絡我們</span>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
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
                                <td style="height: 2px">
                                    <hr style="width: 98%; color: #ffffff; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;&nbsp;&nbsp; <a href="javascript:void(0)" onclick="openNewWin('../../ZH/PrivacyPolicy.htm')"
                                    class="footerText">私隱政策</a> <span class="footerText">| </span><a href="javascript:void(0)"
                                        onclick="openNewWin('../../ZH/Disclaimer.htm')" class="footerText">重要告示</a>
                                    <span class="footerText">| </span><a href="javascript:void(0)" onclick="openNewWin('../../ZH/SysMaint.htm')"
                                        class="footerText">系統維護</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>