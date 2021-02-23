<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="main.aspx.vb" Inherits="eForm.main" Title="<%$ Resources:Title, eFormIndex %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="JS/Common.js"></script>

    <script type="text/javascript" language="javascript">

<%--    function chkChanged()
    {
        var chkeHS = document.getElementById('<%=cboeHSEnrolment.ClientID%>');
        var chkPPIePR = document.getElementById('<%=cboPPIePREnrolment.ClientID%>');
        var ibtn = document.getElementById('<%=ibtnDownload.ClientID %>');
        
        if (chkeHS.checked || chkPPIePR.checked)
        {
            ibtn.disabled=false;
            ibtn.src = document.getElementById('<%=txtDownloadImageUrl.ClientID%>').value.replace(/~/, ".");
        }
        else
        {
            ibtn.disabled=true;
            ibtn.src = document.getElementById('<%=txtDownloadDisableImageUrl.ClientID%>').value.replace(/~/, ".");
        }
    }--%>
   
    function CancelBubble(evt)
    {
        if (window.event)
            window.event.cancelBubble = true;
        else            
            evt.cancelBubble = true;
    }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MultiViewEnrol" runat="server" ActiveViewIndex="0">
                <asp:View ID="ViewSelectEnrol" runat="server">

                    <!---[CRE17-015] Disallow public using WinXP [2018-04-01] Start--->
                    <asp:Button runat="server" ID="btnHiddenReminderWindowsVersion" Style="display: none" />
                    <asp:Panel Style="display: none" ID="panReminderWindowsVersion" runat="server" Width="540px">
                            <uc1:ucNoticePopUp ID="ucNoticePopUpReminderWindowsVersion" runat="server" NoticeMode="Custom" IconMode="Information" ButtonMode="OK" DialogImagePath="Images/dialog/"
                                             HeaderText="<%$ Resources:Text, ReminderTitle %>" MessageText="<%$ Resources:Text, ReminderWindowsVersion %>" />
                    </asp:Panel>

                    <cc2:ModalPopupExtender ID="ModalPopupExtenderReminderWindowsVersion" runat="server" TargetControlID="btnHiddenReminderWindowsVersion"
                        PopupControlID="panReminderWindowsVersion" BehaviorID="mdlPopup3" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panReminderWindowsVersionHeading">
                    </cc2:ModalPopupExtender>
                    <!---[CRE17-015] Disallow public using WinXP [2018-04-01] End--->

                    <table style="width: 100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:ImageButton ID="ibtnDocToRead" runat="server" ImageUrl="<%$ Resources:ImageUrl, DocToReadBtn %>"
                                    AlternateText="<%$ Resources:Text, DocumentToRead %>" />
                                <asp:LinkButton ID="lnkBtnDocToRead" runat="server" CssClass="titleText" Text="<%$ Resources:Text, DocumentToRead %>"
                                    Font-Bold="True" Font-Size="14pt" OnClick="lnkBtnDocToRead_Click" Visible="False"></asp:LinkButton></td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" style="height: 500px; width: 50%;">
                                <asp:Button ID="btnHiddenOnlineEnrol" runat="server" Style="display:none" />
                                <table class="unhighlightTable" cellpadding="2" cellspacing="2" onmouseover="this.className ='highlightTable'"
                                    onmouseout="this.className='unhighlightTable'" style="height: 100%; width: 95%;">
                                    <tr onclick="javascript:goNewWin()">
                                        <td valign="top" align="center" colspan="2">
                                            <asp:Image ID="imgOnlineEnrol" runat="server" ImageUrl="<%$ Resources:ImageUrl, OnlineEnrolBanner %>"
                                                AlternateText="<%$ Resources:AlternateText, OnlineEnrolBanner %>" /><br />
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="background-color: #ffe4e1;" align="left">
                                                        <asp:Label ID="lblEnrolPrintStatement" runat="server" Text="<%$ Resources:Text, OnlineEnrolmentStatement %>"></asp:Label></td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%;" cellpadding="3" cellspacing="3">
                                                <tr onclick="javascript:goNewWin()">
                                                    <td valign="top" style="width: 50px">
                                                        <asp:Image ID="imgPrinter" runat="server" AlternateText="<%$ Resources:AlternateText, PrinterImg %>"
                                                            ImageUrl="~/Images/others/printer.png" /></td>
                                                    <td style="text-align: justify">
                                                        <asp:Label ID="lblPrinter" runat="server" Font-Bold="True" Text="<%$ Resources:Text, Printer %>"></asp:Label><br />
                                                        <asp:Label ID="lblPrintStatement" runat="server" Text="<%$ Resources:Text, PrintStatement %>"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%;" cellpadding="3" cellspacing="3">
                                                <tr>
                                                    <td valign="top" style="width: 50px">
                                                        <asp:Image ID="imgAcrobat" runat="server" AlternateText="<%$ Resources:AlternateText, AcrobatImg %>"
                                                            ImageUrl="~/Images/others/acrobat.png" /></td>
                                                    <td style="text-align: justify">
                                                        <asp:LinkButton ID="lblAcrobat" runat="server" Font-Bold="True" Text="<%$ Resources:Text, Acrobat %>"
                                                            OnClientClick="javascript:window.open('http://www.adobe.com/products/acrobat/readstep2.html');CancelBubble(event);"></asp:LinkButton>
                                                        <asp:Label ID="lblAcrobatStatement" runat="server" Text="<%$ Resources:Text, AcrobatStatement %>"
                                                            Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="<%$ Resources:Text, FreeDownload %>"
                                                            Visible="false"></asp:LinkButton></td>
                                                </tr>
                                            </table>                                         
                                            <table style="width: 100%;" cellpadding="3" cellspacing="3">
                                                <tr onclick="javascript:goNewWin()">
                                                    <td valign="top" style="width: 50px">
                                                        <asp:Image ID="imgDoc" runat="server" AlternateText="<%$ Resources:AlternateText, DocImg %>"
                                                            ImageUrl="~/Images/others/doc.png" /></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDocIncluded" runat="server" Text="<%$ Resources:Text, DocIncluded %>"
                                                            Font-Bold="True"></asp:Label><br />
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 10px" valign="top">
                                                                    1.</td>
                                                                <td style="text-align: justify" valign="top">
                                                                    <asp:Label ID="lblProfRegCert" runat="server" Text="<%$ Resources:Text, ProfRegCert %>"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 10px" valign="top">
                                                                    2.</td>
                                                                <td style="text-align: justify" valign="top">
                                                                    <asp:Label ID="lblBankAccDoc" runat="server" Text="<%$ Resources:Text, BankAccDoc %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 10px" valign="top">
                                                                    3.</td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblPhotocopyBRCert" runat="server" Text="<%$ Resources:Text, PhotocopyBRCert %>"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td align="center" valign="bottom" colspan="2">
                                            <asp:ImageButton ID="ibtnEasyGuide" runat="server" AlternateText="<%$ Resources:AlternateText, eFormEasyGuideBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, eFormEasyGuideBtn %>" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td align="center" valign="top" style="height: 500px; width: 50%;">
                                <table cellpadding="0" cellspacing="0" class="unhighlightTableWithoutCursor" style="height: 100%;
                                    width: 95%;">
                                    <tr>
                                        <td valign="top" align="center" height="50px">
                                            <asp:Image ID="imgPaperEnrol" runat="server" ImageUrl="<%$ Resources:ImageUrl, PaperEnrolBanner %>"
                                                AlternateText="<%$ Resources:AlternateText, PaperEnrolBanner %>" />
                                         </td>
                                    </tr>
                                    <tr style="height:100px">
                                        <td align="center">
                                            <table style="width: 88%" cellpadding="4" cellspacing="4">
                                                <tr>
                                                    <td align="left">
                                                        <table cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lbleHSEnrolment" runat="server" Text="<%$ Resources:Text, ApplyToEnrolIn %>" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 5px"></td>
                                                                <td>
                                                                    <asp:Label ID="lblPaperDLHCVS" runat="server" Text="<%$ Resources:Text, HCVSScheme %>"
                                                                        Font-Bold="True"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblPaperDLVS" runat="server" Text="<%$ Resources:Text, VaccinationSubsidyScheme %>"
                                                                        Font-Bold="True"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblPaperPCD" runat="server" Text="<%$ Resources:Text, PCD %>" Font-Bold="True"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table style="width: 88%" cellpadding="4" cellspacing="4">
                                                <tr>
                                                    <td align="left" style="text-align: justify">
                                                        <asp:Label ID="lblPaperStatement" runat="server" Text="<%$ Resources:Text, PaperEnrolStatement%>"></asp:Label>
                                                        <asp:Panel runat="server" ID="pnlShowClosedMsg">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                    			<tr>
                                        			<td valign="top" align="left">
                                                		<table cellpadding="3" cellspacing="2" style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                                            border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 105%; padding-bottom: 5px; padding-left: 5px;padding-right: 10px; margin-left: -10px" >
		                                                    <tr>
		                                                        <td style="width: 65%;" valign="top">
		                                                        </td>
		                                                        <td align="center" style="width: 35%" valign="top">
		                                                            <asp:Image ID="imgeHSPrinter" runat="server" AlternateText="<%$ Resources:AlternateText, PrinterImg %>"
		                                                                ImageUrl="~/Images/others/printer_small.png" />                                                            
		                                                        </td>
		                                                    </tr>
		                                                    <tr>
		                                                        <td valign="top">
		                                                            <asp:Label ID="lbleHSForm" runat="server" Text="<%$ Resources:Text, ApplicationForm %>"></asp:Label>
		                                                        </td>
		                                                        <td valign="top" align="center">
		                                                            <asp:LinkButton ID="lnkBtneHSE" runat="server" Text="<%$ Resources:Text, English %>"
		                                                                OnClientClick="javascript:openNewWin('Doc/AppendixA.pdf');return false;"></asp:LinkButton>
		                                                            /
		                                                            <asp:LinkButton ID="lnkBtneHSC" runat="server" Text="<%$ Resources:Text, Chinese %>"
		                                                                OnClientClick="javascript:openNewWin('Doc/AppendixA_CHI.pdf');return false;"></asp:LinkButton></td>
		                                                    </tr>
		                                                    <tr>
		                                                        <td valign="top" >
		                                                            <asp:Label ID="lblAuthorityForPaymentToABank" runat="server" Text="<%$ Resources:Text, AuthorityForPaymentToABank %>"></asp:Label>
		                                                        </td>
		                                                        <td align="center" valign="top">
		                                                            <asp:LinkButton ID="lbkAuthorityForPaymentToABankE" runat="server" Text="<%$ Resources:Text, English %>"
		                                                                OnClientClick="javascript:openNewWin('Doc/AppendixB.pdf');return false;"></asp:LinkButton>
		                                                            /
		                                                            <asp:LinkButton ID="lbkAuthorityForPaymentToABankC" runat="server" Text="<%$ Resources:Text, Chinese %>"
		                                                                OnClientClick="javascript:openNewWin('Doc/AppendixB_CHI.pdf');return false;"></asp:LinkButton></td>
		                                                    </tr>
		                                                    <tr id="trEHRSSConsentForm" runat="server">
		                                                        <td valign="top" >
		                                                            <asp:Label ID="lblEHRSSConsentForm" runat="server" Text="<%$ Resources:Text, EHRSSConsentFom %>"></asp:Label></td>
		                                                        <td valign="top" align="center">
		                                                            <asp:LinkButton ID="lnkBtnEHRSSConsentE" runat="server" Text="<%$ Resources:Text, English %>"
		                                                                OnClientClick="javascript:openNewWin('Doc/TokenSharingConsent.pdf');return false;"></asp:LinkButton>
		                                                            /
		                                                            <asp:LinkButton ID="lnkBtnEHRSSConsentC" runat="server" Text="<%$ Resources:Text, Chinese %>"
		                                                                OnClientClick="javascript:openNewWin('Doc/TokenSharingConsent_CHI.pdf');return false;"></asp:LinkButton>
		                                                        </td>
		                                                    </tr>
                                                            <tr>
                                                                <td align="left" style="text-align: justify; font-style:italic;" colspan="2">
                                                                    <br />
                                                                    <asp:Label ID="lblDownloadStatementStart" runat="server" Text="<%$ Resources:Text, DownloadStatement %>"></asp:Label>
                                                                    <asp:LinkButton ID="lnkDownloadView" runat="server" OnClientClick="javascript:window.open('http://www.adobe.com/products/acrobat/readstep2.html')"
                                                                                    Text="<%$ Resources:Text, Acrobat %>"></asp:LinkButton>
                                                                    <asp:Label ID="lblDownloadEnd" runat="server" Text="<%$ Resources:Text, DownloadStatementEnd %>"></asp:Label>
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
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblReminder" runat="server" Font-Bold="True" Text="<%$ Resources:Text, EnrolReminder%>"></asp:Label></td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <%--    <asp:HiddenField ID="hfDownloadImageUrl" runat="server" />
                                <asp:HiddenField ID="hfDownloadDisableImageUrl" runat="server" />--%>
            <asp:TextBox ID="txtDownloadImageUrl" runat="server" Style="display: none"></asp:TextBox>
            <asp:TextBox ID="txtDownloadDisableImageUrl" runat="server" Style="display: none"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
		
		function ResizeScreen() {
			var w = screen.availWidth||screen.width;
			var h = screen.availHeight||screen.height;
				
			window.moveTo(0,0);
			window.resizeTo(w,h);
		}
		
		ResizeScreen();
		
    </script>

</asp:Content>
