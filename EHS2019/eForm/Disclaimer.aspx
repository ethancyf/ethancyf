<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Disclaimer.aspx.vb" Inherits="eForm.Disclaimer" 
    title="<%$ Resources:Title, eFormDisclaimer %>" %>
<%@ Register Assembly="WebControlCaptcha" Namespace="WebControlCaptcha" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc3" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript">
    function chkChanged()
    {
        var chk = document.getElementById('<%=chkAgreeDisclaimer.ClientID%>');
        var ibtn = document.getElementById('<%=ibtnProceed.ClientID %>');
        
        if (chk.checked)
        {
            ibtn.disabled=false;
            ibtn.src = document.getElementById('<%=txtProceedImageUrl.ClientID%>').value.replace(/~/, ".");
        }
        else
        {
            ibtn.disabled=true;
            ibtn.src = document.getElementById('<%=txtProceedDisableImageUrl.ClientID%>').value.replace(/~/, ".");
        }
    }
</script>
        <!---[CRE17-015] Disallow public using WinXP [2018-04-01] Start--->
        <asp:Button runat="server" ID="btnHiddenReminderWindowsVersion" Style="display: none" />
        <asp:Panel Style="display: none" ID="panReminderWindowsVersion" runat="server" Width="540px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpReminderWindowsVersion" runat="server" NoticeMode="Custom" IconMode="Information" ButtonMode="OK" DialogImagePath="Images/dialog/"
                                    HeaderText="<%$ Resources:Text, ReminderTitle %>" MessageText="<%$ Resources:Text, ReminderWindowsVersion %>" />
        </asp:Panel>

        <cc3:ModalPopupExtender ID="ModalPopupExtenderReminderWindowsVersion" runat="server" TargetControlID="btnHiddenReminderWindowsVersion"
            PopupControlID="panReminderWindowsVersion" BehaviorID="mdlPopup3" BackgroundCssClass="modalBackgroundTransparent"
            DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panReminderWindowsVersionHeading">
        </cc3:ModalPopupExtender>
        <!---[CRE17-015] Disallow public using WinXP [2018-04-01] End--->

        <table style="width: 100%; height: 360px" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" colspan="2" valign="top" style="height: 40px">
                <asp:LinkButton ID="lnkBtnDocToRead" runat="server" CssClass="titleText" Text="<%$ Resources:Text, DocumentToRead %>"
                    Font-Bold="True" Font-Size="14pt" Visible="False"></asp:LinkButton><asp:ImageButton
                        ID="ibtnDocToRead" runat="server" AlternateText="<%$ Resources:Text, DocumentToRead %>"
                        ImageUrl="<%$ Resources:ImageUrl, DocToReadBtn %>" /></td>
        </tr>
        <tr>
            <td align="center" colspan="2" valign="top">
                <asp:Label ID="lblDisclaimerText" runat="server" CssClass="tableText" Text="<%$ Resources:Text, Disclaimer %> "></asp:Label></td>
        </tr>
                        <tr>
                            <td align="center" colspan="2" valign="top">
                                        <div style="height: 260px; border-right: #cccccc 1px solid; padding-right: 1px; border-top: #cccccc 1px solid; padding-left: 1px; padding-bottom: 1px; border-left: #cccccc 1px solid; width: 750px; padding-top: 1px; border-bottom: #cccccc 1px solid; background-color: #ffffff;">
                                            <table style="padding-right: 1px;
                                                padding-left: 1px; padding-bottom: 1px; width: 97%;
                                                padding-top: 1px; border-top-width: 1px; border-left-width: 1px; border-left-color: #666666; border-bottom-width: 1px; border-bottom-color: #666666; border-top-color: #666666; border-right-width: 1px; border-right-color: #666666; text-align: left;">
                                                <tr>
                                                    <td valign="top" style="text-align:justify">
                                                        <asp:Label runat="server" ID="lblDisclaimer" Text="<%$ Resources:Text, eFormDisclaimer %>" Font-Size="18px"></asp:Label>                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                            </div><br/>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <cc2:messagebox id="msgBox" runat="server" Width="90%"></cc2:messagebox>
                                <table style="width: 990px">
                                    <tr>
                                       <td align="center" valign="top" >
                                           <table>
                                                <tr>
                                                    <td rowspan="2">
                                                        <cc1:captchacontrol id="CaptchaControl1" runat="server" CaptchaLineNoise="Low" LayoutStyle="Vertical" CaptchaMinTimeout="3" Text="<%$ Resources:Text, Captcha %>" CaptchaMaxTimeout="120"></cc1:captchacontrol>
                                                    </td>
                                                    <td valign="top">
                                            <asp:LinkButton ID="lnkbtnTryDiffImg" runat="server" ForeColor="Blue" Text="<%$ Resources:Text, CaptchaTryDiffImage %>"></asp:LinkButton></td>
                                                </tr>
                                               <tr>
                                                   <td valign="bottom" align="left"><asp:Image ID="imgCaptchaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                               </tr>
                                            </table></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:CheckBox ID="chkAgreeDisclaimer" runat="server" Text="<%$ Resources:Text, AgreeDisclaimer %>" onclick="chkChanged()" />
                                            <asp:Image ID="imgAgreeDisclaimerAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" ImageAlign="AbsMiddle" /></td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="1" >
                                            <asp:ImageButton ID="ibtnProceed" runat="server" ImageUrl="<%$ Resources:ImageUrl, ProceedDisableBtn %>" AlternateText="<%$ Resources:AlternateText, ProceedBtn %>" Enabled="false" OnClick="ibtnProceed_Click"/>
                                            <asp:ImageButton ID="ibtnClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" AlternateText="<%$ Resources:AlternateText, CloseBtn %>" /></td>
                                    </tr>
                                </table>
                                <asp:TextBox ID="txtProceedImageUrl" runat="server" Style="display: none"></asp:TextBox>
                                <asp:TextBox ID="txtProceedDisableImageUrl" runat="server" Style="display: none"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                     <script type="text/javascript">
    if (document.all) {
        top.window.resizeTo(screen.availWidth,screen.availHeight);
    }
    else if (document.layers||document.getElementById) {
        if (top.window.outerHeight<screen.availHeight){
            top.window.outerHeight = screen.availHeight;
        }
        if(top.window.outerWidth<screen.availWidth){
            top.window.outerWidth = screen.availWidth;
        }
    }
    </script>
</asp:Content>
