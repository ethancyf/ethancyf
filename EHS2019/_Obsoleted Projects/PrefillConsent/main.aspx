<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="PrefillConsent.main" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="WebControlCaptcha" Namespace="WebControlCaptcha" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title id="PageTitle" runat="server"></title>
    <base id="basetag" runat="server" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="JS/Common.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function chkChanged() {
            var chk = document.getElementById('<%=chkAgreeDisclaimer.ClientID%>');
            var ibtn = document.getElementById('<%=ibtnProceed.ClientID %>');

            if (chk.checked) {
                ibtn.disabled = false;
                ibtn.src = document.getElementById('<%=txtProceedImageUrl.ClientID%>').value.replace(/~/, ".");
            }
            else {
                ibtn.disabled = true;
                ibtn.src = document.getElementById('<%=txtProceedDisableImageUrl.ClientID%>').value.replace(/~/, ".");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off" defaultbutton="ibtnProceed">
        <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </cc1:ToolkitScriptManager>
        <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(Images/master/banner_header.jpg); width: 994px; background-repeat: no-repeat; height: 100px"
            runat="server">
            <tr>
                <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironment">
                    <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                </td>
                <td align="right" valign="top" style="white-space: nowrap">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkbtnTradChinese" runat="server" CssClass="languageText">繁體</asp:LinkButton><asp:LinkButton
                                    ID="lnkbtnSimpChinese" runat="server" CssClass="languageText" Visible="false">简体</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkbtnEnglish" runat="server" CssClass="languageText">English</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 1000px;">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td align="left"></td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="panDocTypeHelp" runat="server" Style="display: none;">
                                <asp:Panel ID="panDocTypeHelpHeading" runat="server" Style="cursor: move;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 650px">
                                        <tr>
                                            <td style="background-image: url(Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                            <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                                <asp:Label ID="lblDocTypeHelpHeading" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                                            <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 650px">
                                    <tr>
                                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                                            <asp:Panel ID="panDocTypeContent" runat="server" Height="200px">
                                                <uc1:DocTypeLegend ID="udcDocTypeLegend" runat="server" />
                                            </asp:Panel>
                                        </td>
                                        <td style="background-image: url(Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                                    </tr>
                                    <tr>
                                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                            <asp:ImageButton ID="ibtnCloseDocTypeHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseDocTypeHelp_Click" /></td>
                                        <td style="background-image: url(Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                                    </tr>
                                    <tr>
                                        <td style="background-image: url(Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                                        <td style="background-image: url(Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                                        <td style="background-image: url(Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Button runat="server" ID="btnHiddenDocTypeHelp" Style="display: none" />
                            <cc1:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                                PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
                            </cc1:ModalPopupExtender>
                            <cc3:MessageBox ID="udcMessageBox" runat="server" Width="950px" Visible="false"></cc3:MessageBox>
                            <cc3:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" Visible="false"></cc3:InfoMessageBox>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="background-color: #fcfcfc">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="middle">
                                                    <table style="height: 360px" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Image ID="Img3Step" runat="server" ImageUrl="<%$ Resources:ImageUrl, 3Steps %>" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="2" cellspacing="2" style="height: 100%; width: 95%;">
                                                                    <tr>
                                                                        <td valign="top" align="center" colspan="2">
                                                                            <table style="width: 100%;" cellpadding="3" cellspacing="3">
                                                                                <tr>
                                                                                    <td valign="top">
                                                                                        <asp:Image ID="ImgQuestion" runat="server" ImageUrl="~/Images/others/QuestionMark.png" />
                                                                                    </td>
                                                                                    <td align="left" style="font-weight: bold">
                                                                                        <asp:Label ID="lblWhatNeed" runat="server" Text="<%$ Resources:Text, WhatYouNeed %>" Font-Bold="True" Font-Size="18px"></asp:Label>...
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="bottom" style="width: 50px">
                                                                                        <asp:Image ID="imgDoc" runat="server" ImageUrl="~/Images/others/Document.png" /></td>
                                                                                    <td align="left" style="width: 500px">
                                                                                        <asp:Label ID="lblDocIncluded" runat="server" Text="<%$ Resources:Text, DocIdentity %>" Font-Bold="True"></asp:Label>
                                                                                        &nbsp;
                                                                                        <asp:ImageButton ID="ibtnInfo" runat="server" ImageUrl="~/Images/others/information.png" />
                                                                                        <br />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="bottom" style="width: 50px">
                                                                                        <asp:Image ID="imgPrinter" runat="server" ImageUrl="~/Images/others/Printer.png" /></td>
                                                                                    <td style="text-align: justify">
                                                                                        <asp:Label ID="lblPrinter" runat="server" Font-Bold="True" Text="<%$ Resources:Text, Printer %>"></asp:Label><br />
                                                                                        <asp:Label ID="lblPrintStatement" runat="server" Text="<%$ Resources:Text, PrintStatement %>"
                                                                                            Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="bottom" style="width: 50px">
                                                                                        <asp:Image ID="imgAcrobat" runat="server" ImageUrl="~/Images/others/icon_downloadArea.png" /></td>
                                                                                    <td style="text-align: left; vertical-align:middle">
                                                                                        <asp:Label ID="lblDownload" Font-Bold="True" runat="server"></asp:Label>
                                                                                        <asp:ImageButton ID="ibtnFloopy" runat="server" ImageUrl="~/Images/others/floopy.png" Style="vertical-align:bottom" />
                                                                                        <asp:Label ID="lblAcrobatStatement" runat="server" Text="<%$ Resources:Text, AcrobatStatement %>"
                                                                                            Visible="false"></asp:Label>
                                                                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="<%$ Resources:Text, FreeDownload %>"
                                                                                            Visible="false"></asp:LinkButton></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <table style="height: 360px" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="center" colspan="2" valign="top">
                                                                <asp:Label ID="lblDisclaimerText" runat="server" CssClass="tableText" Text="<%$ Resources:Text, Disclaimer %> "></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2" valign="top">
                                                                <div style="height: 210px; border-right: #cccccc 1px solid; padding-right: 1px; border-top: #cccccc 1px solid; padding-left: 1px; padding-bottom: 1px; border-left: #cccccc 1px solid; width: 450px; padding-top: 1px; border-bottom: #cccccc 1px solid; background-color: #ffffff;">
                                                                    <table style="padding-right: 1px; padding-left: 1px; padding-bottom: 1px; width: 97%; padding-top: 1px; border-top-width: 1px; border-left-width: 1px; border-left-color: #666666; border-bottom-width: 1px; border-bottom-color: #666666; border-top-color: #666666; border-right-width: 1px; border-right-color: #666666; text-align: left;">
                                                                        <tr>
                                                                            <td valign="top" style="text-align: justify">
                                                                                <asp:Label runat="server" ID="lblDisclaimer" Text="<%$ Resources:Text, preFillFormDisclaimer %>" Font-Size="16px"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <br />
                                                                <table style="width: 570px">
                                                                    <tr>
                                                                        <td align="center" valign="top">
                                                                            <table style="width: 85%">
                                                                                <tr>
                                                                                    <td align="center" valign="top">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td rowspan="2">
                                                                                                    <cc2:CaptchaControl ID="CaptchaControl" runat="server" CaptchaLineNoise="Low" CaptchaBackgroundNoise="None" LayoutStyle="Vertical"
                                                                                                        CaptchaHeight="35" CaptchaWidth="240" CaptchaChars="123" Text="<%$ Resources:Text, Captcha %>"></cc2:CaptchaControl>
                                                                                                </td>
                                                                                                <td valign="top">
                                                                                                    <asp:LinkButton ID="lnkbtnTryDiffImg" runat="server" ForeColor="Blue" Text="<%$ Resources:Text, CaptchaTryDiffImage %>"></asp:LinkButton>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td valign="bottom" align="left">
                                                                                                    <asp:Image ID="imgCaptchaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:CheckBox ID="chkAgreeDisclaimer" runat="server" Text="<%$ Resources:Text, AgreeDisclaimer %>" onclick="chkChanged()" />
                                                                                        <asp:Image ID="imgAgreeDisclaimerAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" ImageAlign="AbsMiddle" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" colspan="1">
                                                                            <asp:ImageButton ID="ibtnExit" runat="server" ImageUrl="<%$ Resources:ImageUrl, ExitBtn %>" AlternateText="<%$ Resources:AlternateText, ExitBtn %>" />
                                                                            <asp:ImageButton ID="ibtnProceed" runat="server" ImageUrl="<%$ Resources:ImageUrl, ProceedDisableBtn %>" AlternateText="<%$ Resources:AlternateText, ProceedBtn %>" Enabled="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:TextBox ID="txtProceedImageUrl" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtProceedDisableImageUrl" runat="server" Style="display: none"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>


                        </ContentTemplate>

                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="height: 145px" valign="bottom" align="center">
                    <table style="width: 100%">
                        <tr>
                            <td style="height: 140px" valign="top" align="center">
                                <asp:ImageButton ID="ibtnEasyGuide" runat="server" ImageUrl="<%$Resources:ImageUrl, PreFillEasyGuideBtn%>"
                                    AlternateText="<%$Resources:AlternateText, PreFillEasyGuideBtn%>" /></td>
                            <td style="height: 140px" valign="top" align="center">
                                <asp:ImageButton ID="ibtnFAQ" runat="server" ImageUrl="<%$Resources:ImageUrl, FAQsBtn%>"
                                    AlternateText="<%$Resources:AlternateText, FAQsBtn%>" /></td>
                            <td style="height: 140px" valign="top" align="center">
                                <asp:ImageButton ID="ibtnContactUs" runat="server" ImageUrl="<%$Resources:ImageUrl, ContactUsBtn%>"
                                    AlternateText="<%$Resources:AlternateText, ContactUsBtn%>" /></td>
                            <%--<td style="height: 140px" valign="top">
                            <a href="http://www.chp.gov.hk/view_content.asp?lang=en&info_id=16615" target="_blank"><img src="Images/others/btn_swine.jpg" alt="" border="0"/></a></td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="background-repeat: no-repeat; height: 8px; background-color: #d6d6d6; font-size: 14px"
                    valign="bottom">
                    <table style="width: 100%">
                        <tr>
                            <td align="left" style="height: 18px">&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnPrivacyPolicy" runat="server" CssClass="footerText" Text="<%$ Resources:Text, PrivacyPolicy %>"></asp:LinkButton>
                                <asp:Label ID="lblseparator1" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                    ID="lnkBtnDisclaimer" runat="server" CssClass="footerText" Text="<%$ Resources:Text, ImportantNotices %>"></asp:LinkButton>
                                <asp:Label ID="lblseparator2" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                    ID="lnkBtnSysMaint" runat="server" CssClass="footerText" Text="<%$ Resources:Text, SysMaint %>"></asp:LinkButton>
                            </td>
                            <td align="right" style="height: 18px">
                                <asp:Label ID="lblFooterCopyright" runat="server" CssClass="footerText" Text="© Copyright Hospital Authority . All rights reserved."
                                    Visible="False"></asp:Label>
                                &nbsp; &nbsp; &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
<script type="text/javascript">

    function ResizeScreen() {
        var w = screen.availWidth || screen.width;
        var h = screen.availHeight || screen.height;

        window.moveTo(0, 0);
        window.resizeTo(w, h);
    }

    ResizeScreen();

</script>

</html>
