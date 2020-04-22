<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="preFill.aspx.vb" Inherits="PrefillConsent.preFill" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="./UIControl/DocType/ucInputDocumentType.ascx" TagName="ucInputDocumentType" TagPrefix="uc2" %>
<%@ Register Src="./UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType" TagPrefix="uc3" %>
<%@ Register Src="./UIControl/ChooseCCCode.ascx" TagName="ChooseCCCode" TagPrefix="uc4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title id="PageTitle" runat="server"></title>
    <base id="basetag" runat="server" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="JS/Common.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        //  keeps track of the delete button for the row
        //  that is going to be removed
        var _source;
        // keep track of the popup div
        var _popup;
        function showConfirm(source) {
            this._source = source;
            this._popup = $find('mdlPopup');
            //  find the confirm ModalPopup and show it    
            this._popup.show();
        }
        function okClick() {
            //  find the confirm ModalPopup and hide it    
            this._popup.hide();
            //  use the cached button as the postback source
            __doPostBack(this._source.name, '');
        }
        function cancelClick() {
            //  find the confirm ModalPopup and hide it 
            this._popup.hide();
            //  clear the event source
            this._source = null;
            this._popup = null;
        }


    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off" defaultbutton="btnNext">
        <cc2:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </cc2:ToolkitScriptManager>
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
        <table border="0" cellpadding="0" cellspacing="0" style="width: 990px;">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td align="left">
                                        <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                                            <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 550px">
                                                    <tr>
                                                        <td style="background-image: url(Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                                        <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                                            <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                                                        <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 550px">
                                                <tr>
                                                    <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                                    <td style="background-color: #ffffff">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td align="left" style="width: 40px; height: 42px" valign="middle">
                                                                    <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                                                <td align="center" style="height: 42px">
                                                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label></td>
                                                                <td align="center" style="width: 40px; height: 42px"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, YesBtn %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, YesBtn %>" />
                                                                    <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, NoBtn %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, NoBtn %>" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="background-image: url(Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                                                </tr>
                                                <tr>
                                                    <td style="background-image: url(Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                                                    <td style="background-image: url(Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                                                    <td style="background-image: url(Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <cc2:ModalPopupExtender ID="ModalPopupExtenderConfirmClose" runat="server" TargetControlID="panConfirmMsg"
                                            PopupControlID="panConfirmMsg" BehaviorID="mdlPopup" BackgroundCssClass="modalBackgroundTransparent"
                                            DropShadow="False" RepositionMode="RepositionOnWindowScroll" OkControlID="ibtnDialogConfirm"
                                            CancelControlID="ibtnDialogCancel" OnOkScript="okClick();" OnCancelScript="cancelClick();"
                                            PopupDragHandleControlID="panConfirmMsgHeading">
                                        </cc2:ModalPopupExtender>

                                        <!--Chinese CCCode -------------------------------------------------------------------------->
                                        <asp:Panel Style="display: none" ID="panChooseCCCode" runat="server">
                                            <asp:Panel ID="panChooseCCCodeHeading" runat="server" Style="cursor: move">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 350px">
                                                    <tr>
                                                        <td style="background-image: url(Images/dialog/top-left.png); width: 9px; height: 35px"></td>
                                                        <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, ChooseCCCodeHeading %>"></asp:Label></td>
                                                        <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <table style="width: 350px" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                                        <td style="background-color: #ffffff" align="center">
                                                            <uc4:ChooseCCCode ID="udcChooseCCCode" runat="server"></uc4:ChooseCCCode>
                                                        </td>
                                                        <td style="background-image: url(Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-image: url(Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                                                        <td style="background-image: url(Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                                                        <td style="background-image: url(Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <!--Chinese CCCode End----------------------------------------------------------------------->
                                        <cc2:ModalPopupExtender ID="ModalPopupExtenderChooseCCCode" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                                            TargetControlID="btnChooseCCCode" PopupControlID="panChooseCCCode" RepositionMode="RepositionOnWindowScroll"
                                            PopupDragHandleControlID="panChooseCCCodeHeading">
                                        </cc2:ModalPopupExtender>
                                        <asp:Button Style="display: none" ID="btnChooseCCCode" runat="server"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <cc1:MessageBox ID="udcMsgBoxErr" runat="server" Width="950px" Visible="false"></cc1:MessageBox>
                                        <cc1:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" Visible="false"></cc1:InfoMessageBox>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <table border="0" cellpadding="0" cellspacing="0" width="450">
                                            <tr>
                                                <td align="left">
                                                    <asp:Panel ID="panStep1" runat="server">
                                                        <asp:Label ID="lblStep1a" Text="<%$ Resources:Text, preFillFormStep1 %>" runat="server"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                                <td align="left">
                                                    <asp:Panel ID="panStep2" runat="server">
                                                        <asp:Label ID="lblStep2a" Text="<%$ Resources:Text, preFillFormStep2 %>" runat="server"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                                <td align="left">
                                                    <asp:Panel ID="panStep3" runat="server">
                                                        <asp:Label ID="lblStep3a" Text="<%$ Resources:Text, preFillFormStep3 %>" runat="server"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="background-image: url(Images/master/top_left.jpg); width: 30px; height: 30px"></td>
                                    <td style="background-image: url(Images/master/top.jpg); background-repeat: repeat-x"></td>
                                    <td style="background-image: url(Images/master/top_right.jpg); width: 30px; height: 30px"></td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(Images/master/left.jpg); width: 30px; background-repeat: repeat-y"></td>
                                    <td style="background-color: #fcfcfc" align="left">
                                        <asp:MultiView ID="mvPreFill" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="vFill" runat="server">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Image ID="ImgBulb" runat="server" ImageUrl="~/Images/others/Bulb.png" />
                                                                    </td>
                                                                    <td valign="top" style="background-color: yellow">
                                                                        <asp:Label ID="lblFillInfo" runat="server" Text="<%$ Resources:Text, preFillStep1Info %>" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-left-color: goldenrod; border-right-color: goldenrod; border-top-color: goldenrod; border-bottom-color: goldenrod; border-top-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-style: solid">
                                                            <cc1:DocumentTypeRadioButtonGroup ID="udcStep1DocumentTypeRadioButtonGroup" runat="server"
                                                                HeaderCss="tableHeading" AutoPostBack="true" HeaderText="<%$ Resources:Text, DocumentType%>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblInputInfoText" runat="server" CssClass="tableText"></asp:Label>&nbsp;
                                                            <asp:ImageButton ID="btnInputTips" runat="server" ImageAlign="AbsMiddle" />
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <uc2:ucInputDocumentType ID="udcStep1b1InputDocumentType" runat="server"></uc2:ucInputDocumentType>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="btnFillBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn%>" ImageUrl="<%$ Resources:ImageUrl, BackBtn%>"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="btnNext" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </asp:View>
                                            <asp:View ID="vConfirm" runat="server">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblConfirm" Text="<%$ Resources:Text, preFillStep2Info %>" runat="server" Width="750px" Font-Bold="true" Font-Size="20px"></asp:Label><br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <table style="width: 60%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <uc3:ucReadOnlyDocumnetType ID="udcStep1a2ReadOnlyDocumnetType" runat="server"></uc3:ucReadOnlyDocumnetType>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr></tr>
                                                    <tr>
                                                        <td align="center">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="btnConfirmBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn%>" ImageUrl="<%$ Resources:ImageUrl, BackBtn%>"></asp:ImageButton>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="btnConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn%>" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn%>" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:View>
                                            <asp:View ID="vPrint" runat="server">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 120px">
                                                                        <asp:Label ID="lblCompleteSubmissionDateText" runat="server" Text="<%$ Resources:Text, SubmissionDtmTime %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCompleteSubmissionDate" runat="server" CssClass="tableText" Font-Bold="true"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr></tr>
                                                    <tr>
                                                        <td>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblThingPlease" runat="server" Text="<%$ Resources:Text, ThingYouPlease %>" ForeColor="#ffcc00" Font-Size="30px" Font-Bold="true"></asp:Label>
                                                                        <asp:Label ID="lblThingNeed" runat="server" Text="<%$ Resources:Text, ThingYouNeed %>"></asp:Label>
                                                                        <asp:Label ID="lblThingDo" runat="server" Text="<%$ Resources:Text, ThingYouDo %>" ForeColor="#ffcc00" Font-Size="30px" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <table style="width: 95%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblPoint1" runat="server" Text="<%$ Resources:Text, preFillStep3Point1 %>"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblPoint2" runat="server" Text="<%$ Resources:Text, preFillStep3Point2 %>"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblPoint3a" runat="server" Text="<%$ Resources:Text, preFillStep3Point3a %>"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <table style="width: 98%">
                                                                                        <tr>
                                                                                            <td align="left" valign="top" style="width: 2%">
                                                                                                <b>(a) </b>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblPoint3b" runat="server" Text="<%$ Resources:Text, preFillStep3Point3b %>"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" valign="top" style="width: 2%">
                                                                                                <b>(b) </b>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblPoint3c" runat="server" Text="<%$ Resources:Text, preFillStep3Point3c %>"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" valign="top" style="width: 2%">
                                                                                                <b>(c) </b>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblPoint3d" runat="server" Text="<%$ Resources:Text, preFillStep3Point3d %>"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" valign="top" style="width: 2%">
                                                                                                <b>(d) </b>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="lblPoint3e" runat="server" Text="<%$ Resources:Text, preFillStep3Point3e %>"></asp:Label>
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
                                                    <tr></tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvPrintOut" runat="server" AutoGenerateColumns="False" ShowHeader="true"
                                                                GridLines="Both" ShowFooter="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, ConsentPreFillNo %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPreFillNo" runat="server" CssClass="tableText" Text='<%# Eval("Pre_Fill_Consent_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="250px" ForeColor="Black" />
                                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Height="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Name %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" CssClass="tableText" Text='<%# Eval("EnglishName") %>'></asp:Label>
                                                                            <asp:Label ID="lblChiName" runat="server" CssClass="tableText" Text='<%# Eval("ChineseName") %>' Font-Names="HA_MingLiu"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="300px" />
                                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Height="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblConsentForm" runat="server" Text="<%$ Resources:Text, ConsentForm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Image ID="imgConsentForm" runat="server" ImageUrl="~/Images/others/printer_small.png" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnEngConsentForm" runat="server" Text="<%$ Resources:Text, English %>"></asp:LinkButton>
                                                                            /
                                                                            <asp:LinkButton ID="lnkBtnChiConsentForm" runat="server" Text="<%$ Resources:Text, Chinese %>"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="200px" HorizontalAlign="Center" />
                                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Height="30px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkBtnCheckList" runat="server" Text="<%$ Resources:Text, ConsentCheckList  %>" OnClientClick="javascript:window.open('Doc/CheckList.pdf')"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="200px" HorizontalAlign="Center" />
                                                                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" Height="30px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ImageButton ID="ibtnCompleteClose" runat="server" AlternateText="<%$ Resources:AlternateText, CompleteBtn %>"
                                                                ImageUrl="<%$ Resources:ImageUrl, CompleteBtn %>" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:View>
                                        </asp:MultiView>
                                    </td>
                                    <td style="background-image: url(Images/master/right.jpg); width: 30px; background-repeat: repeat-y"></td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(Images/master/botton_left.jpg); width: 30px; height: 30px"></td>
                                    <td style="background-image: url(Images/master/botton.jpg); background-repeat: repeat-x"></td>
                                    <td style="background-image: url(Images/master/botton_right.jpg); width: 30px; height: 30px"></td>
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
</html>
