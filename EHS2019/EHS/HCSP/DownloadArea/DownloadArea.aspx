<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DownloadArea.aspx.vb" Inherits="HCSP.DownloadArea" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" <%=PageLanguage%>>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="pragma" content="no-cache" />
    <base id="basetag" runat="server" />
    <title><asp:Literal ID="litTitle" runat="server" Text="<%$ Resources: Title, DownloadArea %>" /></title>
    <link href="../CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript" src="../JS/Common.js?ver=1"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <table border="0" style="padding:0px 0px 0px 0px;border-collapse:collapse; width: 990px">
                <tr>
                    <td id="tdHeader" runat="server" style="background-image: url(../Images/master/banner_header.jpg); background-repeat: no-repeat; height: 100px"
                        valign="top">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="background-position: center bottom; background-image: url(../Images/master/background.jpg);
                        background-repeat: repeat-x; height: 500px;">
                        <table style="width: 100%; height: 540px;padding:0px 0px 0px 0px;border-collapse:collapse;" border="0">
                            <tr>
                                <td style="width: 10px;vertical-align:top"></td>
                                <td style="vertical-align:top">
                                    <table style="width: 100%;"">
                                        <tr>
                                            <td style="text-align:left">
                                                <asp:Image ID="imgBanner" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadAreaBanner %>"
                                                    AlternateText="<%$ Resources: AlternateText, DownloadArea %>" />
                                            </td>
                                        </tr>
                                        <tr id="trEngVersion" runat="server">
                                            <td style="text-align:left">
                                                <table style="border-width:0px;border-spacing:1px;border-collapse:separate;background-color: #87ceeb;width:800px">
                                                    <tr>
                                                        <td style="text-align:left;font-weight: bold; font-size: 16px; color: #ffffff">
                                                            Forms
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left;background-color: #ffffff">
                                                            Please click on the following hyperlink to forms.<br />
                                                            <br />
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 10px"></td>
                                                                    <td>
                                                                        <img alt="Health Care Voucher Scheme" src="../Images/others/page.png" border="0"
                                                                             style="vertical-align: middle" />
                                                                        <a href="http://www.hcv.gov.hk" target="_blank">
                                                                            Health
                                                                            Care Voucher Scheme
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 10px"></td>
                                                                    <td>
                                                                        <img alt="Vaccination Subsidy Scheme" src="../Images/others/page.png"
                                                                             border="0" style="vertical-align: middle" />
                                                                        <a href="http://www.chp.gov.hk/en/view_content/45851.html" target="_blank">
                                                                            Vaccination Subsidy Scheme
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table style="border-width:0px;border-spacing:1px;border-collapse:separate;width:800px;background-color: #87ceeb">
                                                    <tr>
                                                        <td style="text-align:left;font-weight: bold; font-size: 16px; color: #ffffff">
                                                            Software
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left;background-color: #ffffff">
                                                            The following item may be required for the application use.<br />
                                                            <br />
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 10px"></td>
                                                                    <td>
                                                                        <table style="width:700px;border-spacing:1px;border-collapse:separate;background-color: silver">
                                                                            <tr>
                                                                                <td style="vertical-align:top;background-color: #ffffff; width: 20px;padding:5px;">
                                                                                    1.
                                                                                </td>
                                                                                <td style="vertical-align:top;background-color: #ffffff;padding:5px;">
                                                                                    <img src="../Images/others/acrobat.png" alt="Adobe reader" />
                                                                                    <a href="http://www.adobe.com/products/acrobat/readstep2.html" target="_blank" style="font-weight: bold">
                                                                                        Adobe reader 7 or above
                                                                                    </a>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trEngSmartIDCardReader" runat="server">
                                                                                <td style="vertical-align:top;background-color: #ffffff; width: 20px;padding:5px;">
                                                                                    2.
                                                                                </td>
                                                                                <td style="vertical-align:top;background-color: #ffffff;padding:5px;">
                                                                                    <img src="../Images/others/SmartIDCardReader.png" alt="Software for reading Smart ID Card" />
                                                                                    <a style="font-weight:bold;" href="#" onclick="JavaScript:showUpdateNow('en-us');">
                                                                                        Software for reading Smart ID Card
                                                                                    </a>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr id="trChiVersion" runat="server">
                                            <td style="text-align:left">
                                                <table style="border-width:0px;border-spacing:1px;border-collapse:separate;background-color: #87ceeb;width:800px">
                                                    <tr>
                                                        <td style="text-align:left;font-weight: bold; font-size: 16px; color: #ffffff">
                                                            表格
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left;background-color: #ffffff">
                                                            請進入以下網頁連結下載表格。<br />
                                                            <br />
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 10px"></td>
                                                                    <td>
                                                                        <img alt="醫療券計劃" src="../Images/others/page.png" border="0" style="vertical-align: middle" />
                                                                        <a href="http://www.hcv.gov.hk" target="_blank">醫療券計劃</a>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 10px"></td>
                                                                    <td>
                                                                        <img alt="疫苗資助計劃" src="../Images/others/page.png" border="0" style="vertical-align: middle" />
                                                                        <a href="http://www.chp.gov.hk/tc/view_content/45851.html" target="_blank">
                                                                            疫苗資助計劃
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table style="border-width:0px;border-spacing:1px;border-collapse:separate;width:800px;background-color: #87ceeb">
                                                    <tr>
                                                        <td style="text-align:left;font-weight: bold; font-size: 16px; color: #ffffff">
                                                            軟件
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:left;background-color: #ffffff">
                                                            使用此系統時或需有下列配套。<br />
                                                            <br />
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 10px"></td>
                                                                    <td>
                                                                        <table style="width:750px;border-spacing:1px;border-collapse:separate;background-color: silver">
                                                                            <tr>
                                                                                <td style="vertical-align:top;background-color: #ffffff; width: 20px;padding:5px;">
                                                                                    1.
                                                                                </td>
                                                                                <td style="vertical-align:top;background-color: #ffffff;padding:5px;">
                                                                                    <img src="../Images/others/acrobat.png" alt="Adobe reader 7 或以上" />
                                                                                    <a href="http://www.adobe.com/products/acrobat/readstep2.html" target="_blank" style="font-weight: bold">
                                                                                        Adobe reader 7 或以上
                                                                                    </a>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trChiSmartIDCardReader" runat="server">
                                                                                <td style="vertical-align:top;background-color: #ffffff; width: 20px;padding:5px;">
                                                                                    2.
                                                                                </td>
                                                                                <td style="vertical-align:top;background-color: #ffffff;padding:5px;">
                                                                                    <img src="../Images/others/SmartIDCardReader.png" alt="讀取智能身份證軟件" />
                                                                                    <a style="font-weight: bold" href="#" onclick="JavaScript:showUpdateNow('zh-tw');">
                                                                                        讀取智能身份證軟件
                                                                                    </a>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:center">
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
                    <td style="text-align:center;background-color: #d6d6d6;">
                        <table style="width: 98%;border-width:0px;padding:0px;border-collapse:collapse">
                            <tr>
                                <td colspan="2" style="height: 2px">
                                    <hr style="width: 98%; color: #ffffff; border-top-style: none; border-right-style: none;
                                        border-left-style: none; height: 1px; border-bottom-style: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:left">
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
