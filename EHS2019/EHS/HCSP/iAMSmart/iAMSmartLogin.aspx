<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNonLogin.master" CodeBehind="iAMSmartLogin.aspx.vb" Inherits="HCSP.iAMSmartLogin" 
    title="<%$ Resources:Title, iAMSmartLogin %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var pollingTimer;
        var pollingTimeout;
        var rtnValue;	
        var intCount = 1;
        var strTicketID;

        function waitForMsg(businessID) {
            
            $.ajax({
                async: true,
                type: 'POST',
                url: "./iAMSmartLogin.aspx/CheckBussinesID",
                dataType: 'json',
                //data: "{\"BusinessID\":\"" + businessID + "\"}",
                data : "{'BusinessID':'"+businessID+"','intCount':'"+intCount+"'}",
                //data: "{\"BusinessID\":\"3e47be25-66a6-43fb-89f6-7e2dd138aff8\"}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    rtnValue = data.d;
                    intCount=intCount+1
                },
                complete: function () {
                    if (rtnValue == "Found") {
                        StopPolling();

                        var btn = document.getElementById('<%=btnHandleProfileCallback.ClientID%>');
                        btn.click();
                    }

                    if (rtnValue == "Timeout") {
                        RunPollingTimeout();
                    }
                },
                error: function () {
                    //alert("An error has occured!!!");
                    var btnError = docu.g('<%=btnHandleProfileCallbackError.ClientID%>');
                    btnError.click();
                }
            });
            //return rtnValue;
        }

        function SetPolling(businessID) {
            pollingTimer = setInterval(function () { waitForMsg(businessID); }, <%=PollingInterval%>); // 3 seconds
        }

        function StopPolling() {
            clearInterval(pollingTimer);
        }

        function RunPollingTimeout() {
            clearInterval(pollingTimer);
            var btn = document.getElementById('<%=btnHandleProfileCallbackTimeout.ClientID%>');
            btn.click();
        }

        function OpeniAMSmartApp() {
            var strUrl = '<%=OpeniAMSmartAppLink%>';
			
            GetTicketID(strUrl);

            //openNewHTML(url);

            //var wi = screen.width;
            //var he = screen.height;
    
            //if (isMobileClient())
            //{
            //    window.open(link, '', 'width=50px,height=50px,location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
            //}else{
            //    window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
            //}

        }
		
        function GetTicketID(strUrl) {
            $.ajax({
                async: true,
                type: 'POST',
                url: "./iAMSmartLogin.aspx/GetTicketID",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    strTicketID = data.d;
                },
                complete: function () {
                    strUrl = strUrl + strTicketID
                    window.open(strUrl);
                },
                error: function () {
                    //alert("An error has occured!!!");
                    var btnError = docu.g('<%=btnHandleProfileCallbackError.ClientID%>');
                    btnError.click();
                }
            });
            //return strTicketID;
        }		
    </script>

    &nbsp;
    <asp:Image ID="imgHeaderRecoverLogin" runat="server" AlternateText="<%$ Resources:AlternateText, iAMSmartLoginBanner%>"
        ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, iAMSmartLoginBanner%>" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%;background-image: url(../Images/master/background.jpg);background-position:bottom; background-repeat:repeat-x;min-height:546px" border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td style="width: 10px"></td>
                    <td>
                        <cc2:MessageBox ID="udcErrorMessage" runat="server" />
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" />

                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:MultiView ID="mvStepForiAMSmart" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="vBackToLogin" runat="server">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td valign="top" style="width: 950px;"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" valign="top">
                                                        <asp:imageButton ID="ibtnBackToLoginBack" runat="server"
                                                            ImageUrl="<%$ Resources:ImageURL, BackToLoginBtn %>"
                                                            TabIndex="1"
                                                            AlternateText="<%$ Resources:AlternateText, BackToLoginBtn %>"
                                                            OnClick="btnBackToLoginBack_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </asp:View>
                                        <asp:View ID="vGetProfile" runat="server">
                                            <div style="position:relative">
                                                <div style="margin:auto;text-align:center;padding-top:20px;width: 800px">
                                                    <table style="width: 800px;margin:auto;" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 100%;vertical-align:top;text-align:center">
                                                                <asp:Label ID="lblLinkupSPAccount" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, iAMSmartLinkUp %>" 
                                                                    style="font-size:20px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                
                                                    <table style="width: 540px;margin:auto;border-width:medium;border-style:solid;border-color:#38beff" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="vertical-align:top;text-align:left;padding-left:10px;padding-top:10px;padding-bottom:10px">
                                                                <asp:Label ID="lblRequestProfileStatement" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, iAMSmartRequestProfileStatement %>"
                                                                    style="font-size:18px;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 56px;vertical-align:top" >
                                                                <div style="margin:auto">
                                                                    <div id="divRequestProfileFields" runat="server" style="padding-left:40px;text-align:left">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:Button ID="btniAMSmartRequestProfile" runat="server" CssClass="dark"
                                                        Text="<%$ Resources:Text, iAMSmartProfile %>"
                                                        TabIndex="7"
                                                        AlternateText="Login"
                                                        Style="width: 320px; position: relative; left: 240px" OnClick="btniAMSmartRequestProfile_Click" />
                                                </div>
                                            </div>
                                            <table style="width: 100%; padding-top: 15px">
                                                <tr>
                                                    <%--<td style="width: 250px">
                                                        <asp:ImageButton ID="ibtnBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" AlternateText="<%$ Resources:AlternateText, BackBtn %>" />
                                                    </td>--%>
                                                    <td colspan="4" valign="top">                                
                                                        <asp:imageButton ID="ibtnGetProfileBackToLogin" runat="server"
                                                            ImageUrl="<%$ Resources:ImageURL, BackToLoginBtn %>"
                                                            TabIndex="1"
                                                            AlternateText="<%$ Resources:AlternateText, BackToLoginBtn %>"
                                                            style="position:relative;left:10px" />
                                                    </td>
                                                </tr>
                                            </table>                                                                                     
                                        </asp:View>
                                        <asp:View ID="vLinkupSPAccount" runat="server">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td valign="top" style="width: 950px;"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" valign="top">
                                                        <asp:imageButton ID="ibtnGoToHome" runat="server"
                                                            ImageUrl="<%$ Resources:ImageURL, LoginNowBtn %>"
                                                            TabIndex="1"
                                                            AlternateText="<%$ Resources:AlternateText, LoginNowBtn %>"
                                                            />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </asp:View>
                                        <asp:View ID="vGoToEnrolment" runat="server">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td colspan="2" valign="top" style="width: 950px;"></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:imageButton ID="ibtnGoToEnrolmentBackToLogin" runat="server"
                                                            ImageUrl="<%$ Resources:ImageURL, BackToLoginBtn %>"
                                                            TabIndex="1"
                                                            AlternateText="<%$ Resources:AlternateText, BackToLoginBtn %>"
                                                            OnClick="btnGoToEnrolmentBackToLogin_Click" />
                                                    </td>
                                                    <td valign="top">
                                                        <asp:imageButton ID="ibtnGoEnrol" runat="server"
                                                            ImageUrl="<%$ Resources:ImageURL, GoToEnrolmentBtn %>"
                                                            TabIndex="2"
                                                            AlternateText="<%$ Resources:AlternateText, GoToEnrolmentBtn %>" 
                                                            OnClick="ibtnGoEnrol_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </asp:View>
                                        <asp:View ID="vSimulate" runat="server">
                                      
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbliAMSmartOpenIDText" Text="iAM Smart Open ID" runat="server"></asp:Label>:
                                                        &nbsp; 
                                                        <asp:TextBox ID="txtOpenID" Width="600px" runat="server"></asp:TextBox>
                                                        &nbsp; 
                                                    <asp:Button ID="btnSimulateRandom" runat="server" Text="Random" OnClick="btnSimulateRandom_Click" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 15px">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSimulateConnect" runat="server" Text="Connect" OnClick="btnSimulateConnect_Click" />
                                                    </td>
                                                </tr>
                                               <tr style="height: 15px">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSimulateBack" runat="server" Text="<%$ Resources: Text, Back %>" OnClick="btnSimulateBack_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                     
                                    </asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <%-- ----------------- --%>
            <%-- Popup for confirm --%>
            <%-- ----------------- --%>
            <asp:Panel Style="display: none" ID="panPopupConfirmCancel" runat="server" Width="500px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpConfirm" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo"
                    MessageText="<%$ Resources:Text, ConfirmCancelRecoverLogin %>" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupConfirmCancel" PopupControlID="panPopupConfirmCancel"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupConfirmCancel" runat="server"></asp:Button>
            <%-- ----------------- --%>
            <%-- ----------------- --%>
            <%-- Popup for Authorise iAM Smart --%>
            <%-- ----------------- --%>
            <asp:Panel Style="display: none" ID="panPopupAuthoriseiAMSmart" runat="server" Width="500px">
                 <table border="0" cellpadding="0" cellspacing="0" style="width: 500px;background-color: #ffffff; border: 2px solid #E9E3CD;">
                    <tr style="background-color: #E9E3CD;">
                        <td style="width:440px;padding: 10px 10px 10px 10px;" valign="middle">
                            <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, iAMSmartAuthoriseTitle %>" style="font-weight: bold;font-size:12pt"></asp:Label>
                        </td>
                        <td style="text-align:right">
                            <asp:ImageButton ID="ibtniAMSmartPopupCancel" runat="server" style="background-color: #E9E3CD;" ImageUrl =""
                                    AlternateText="" OnClick="ibtniAMSmartPopupCancel_Click" OnClientClick="JavaScript:StopPolling();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="background-color: #ffffff;padding: 10px;" valign="middle">
                            <asp:Label ID="lblInstruction" runat="server" Text="<%$ Resources:Text, iAMSmartAuthoriseInstStep %>" style="font-size:12pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="background-color: #ffffff;padding: 10px;" valign="middle">
                            <asp:Label ID="lblStep1" runat="server" Text="<%$ Resources:Text, iAMSmartAuthoriseInstStep1 %>" style="color:#2B7366;font-size:12pt"></asp:Label><br/>
                            <asp:Image ID="imgiAMSmartApp" runat="server" ImageUrl="<%$ Resources:ImageUrl, iAMSmartApp %>"
                            AlternateText="<%$ Resources:AlternateText, iAMSmartApp %>" />
                        </td>
                    </tr>
                    <tr> 
                        <td colspan="2" style="background-color: #ffffff;padding: 10px;" valign="middle">
                            <asp:Label ID="lblStep2" runat="server" Text="<%$ Resources:Text, iAMSmartAuthoriseInstStep2 %>" style="font-size:12pt"></asp:Label>
                        </td>     
                    </tr>
                    <tr>   
                        <td colspan="2" style="background-color: #ffffff;padding: 10px;" valign="middle">
                            <asp:Label ID="lblStep3" runat="server" Text="<%$ Resources:Text, iAMSmartAuthoriseInstStep3 %>" style="font-size:12pt"></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="lblTicketID" runat="server" style="font-size:12pt"></asp:Label>
                        </td> 
                    </tr>
                    <tr>
                        <td colspan="2" style="background-color: #ffffff; padding: 10px;" align="left">
                            <div style="display: inline-flex">
                                <asp:Button ID="btnCalliAMSmartApp" runat="server" CssClass="dark" Text="<%$ Resources:Text, iAMSmartOpenApp %>"
                                    AlternateText="<%$ Resources:Text, iAMSmartOpenApp %>" style="width: 170px" OnClientClick="OpeniAMSmartApp();return false" />
                                <asp:Button ID="btnHandleProfileCallback" runat="server" style="display: none" OnClick="btnHandleProfileCallback_Click" />
                                <asp:Button ID="btnHandleProfileCallbackTimeout" runat="server" style="display: none" OnClick="btnHandleProfileCallbackTimeout_Click" />
                                <asp:Button ID="btnHandleProfileCallbackError" runat="server" style="display: none" OnClick="btnHandleProfileCallback_Click" />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupAuthoriseiAMSmart" runat="server" BackgroundCssClass="modalBackground"
                TargetControlID="btnModalPopupAuthoriseiAMSmart" PopupControlID="panPopupAuthoriseiAMSmart"
                PopupDragHandleControlID="" RepositionMode="None" DropShadow="False">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupAuthoriseiAMSmart" runat="server"></asp:Button>
            <%-- ----------------- --%>

             <%-- ----------------- --%>
            <%-- Popup for Get HKID  --%>
            <%-- ----------------- --%>
            <asp:Panel Style="display: none" ID="panPopupStimulateProfile" runat="server" Width="500px">
                 <table border="0" cellpadding="0" cellspacing="0" style="width: 500px;background-color: #ffffff; border: 2px solid #E9E3CD;">
                    <tr>
                        <td colspan="2" style="background-color: #ffffff; padding: 10px;" align="left">
                            <div style="display: inline-flex">
                                <br />
                                HKIC:  <asp:TextBox ID="txtHKIC" Width="160px" runat="server"></asp:TextBox>
                                <asp:Button ID="btnSimulateHKICSubmit" runat="server" OnClick="btnSimulateHKICSubmit_Click" text="Submit"/>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupStimulateProfile" runat="server" BackgroundCssClass="modalBackground"
                TargetControlID="btnModalPopupStimulateProfile" PopupControlID="panPopupStimulateProfile"
                PopupDragHandleControlID="" RepositionMode="None" DropShadow="False">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupStimulateProfile" runat="server"></asp:Button>
            <%-- ----------------- --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

