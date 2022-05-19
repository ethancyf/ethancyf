<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="login.aspx.vb" Inherits="HCSP.login" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelLogin">
        <ProgressTemplate>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanelLogin" runat="server">
        <ContentTemplate>    
            <style type="text/css">
                .messageText {
                    font-size: 14px;
                    color: #666666;
                    font-family: Arial;
                    text-align: left;
                    background-color: #dde4f6;
                    font-weight: normal;
                }

                a.RecoverAccountLink {
                    color: blue;
                }

                a.RecoverAccountLink:visited {
                    color: blue;
                }
            </style>

            <script type="text/javascript" src="JS/Common.js"></script>

            <script language="Javascript" src="JS/ideasComboLib4Ra.js" type="text/javascript"></script>

            <script language="Javascript" src="JS/ideasComboVersion.js" type="text/javascript"></script>

            <script type="text/javascript">
                function fnTrapKD(e) {
                    var btn = document.getElementById('<%=ibtnLogin.ClientID%>')
                    if (document.all) {
                        if (event.keyCode == 13) {
                            event.returnValue = false;
                            event.cancel = true;
                            try {
                                btn.focus();
                                btn.click();
                            } catch (err) {
                                // Do Nothing
                            }

                        }
                    }
                    else {
                        /*
                            if (e.which == 13)
                            { 
                                e.returnValue=false;
                                e.cancel = true;
                                btn.focus();
                                btn.click();
                            }
                        */
                    }
                }
            </script>

            <script type="text/javascript" language="javascript">
                function okClick() {
                    document.getElementById("<%=txtUserName.ClientID%>").focus();
                }

                function IsPopupBlocker() {
                    var wi = screen.width;
                    var he = screen.height;
                    var oWin = window.open("./blank.htm", "testpopupblocker", "width=" + wi + ",height=" + he + ",top=0,left=0");
                    //var oWin = window.open ("","testpopupblocker","width=100,height=50,top=5000,left=5000");

                    document.getElementById("<%=txtUserName.ClientID%>").focus();
    
                    if (oWin == null || typeof (oWin) == "undefined") {
                        return true;
                    } else {
                        oWin.close();
                        document.getElementById("<%=txtUserName.ClientID%>").focus();
                        return false;
                    }
                }

                var chkpopup = 0;
            </script>

            <script type="text/javascript">
                //function displayIDEASResult() {
                //    document.getElementById("btnDisplayResultComboiframe").click();
                //}

                // TODO _param to be confirmed
                function checkIdeasComboClientSuccessEHS(_param) {
                    //var param = { result: _param, ideasVer: "", artifactId: "" };
                    //checkIdeasComboClientSuccessCallback(param);
                    //document.getElementById("btnReadSmartICCombo").disabled = false;
                    //document.getElementById("btnReadSmartICComboiframe").disabled = false;
                    //console.log('Success:' + _param.result);
                    //console.log(_param);
                    //alert(_param.result);
                    var obj = document.getElementById("<%=txtIDEASComboResult.ClientID%>")
                    obj.value = _param.result
                    //updateIdeasComboClientResult(_param.result);
                }

                // TODO _param to be confirmed
                function checkIdeasComboClientFailureEHS(_param) {
                    //var param = { result: _param, ideasVer: "", artifactId: "" };
                    //checkIdeasComboClientFailureCallback(param);
                    //document.getElementById("btnReadSmartICCombo").disabled = true;
                    //document.getElementById("btnReadSmartICComboiframe").disabled = true;
                    //console.log('Failure:' + _param.result);
                    //console.log(_param);
                    //alert(_param.result);
                    var obj = document.getElementById("<%=txtIDEASComboResult.ClientID%>")
                    obj.value = _param.result
                    //updateIdeasComboClientResult(_param.result);
                }

                //checkIdeasComboClient(checkIdeasComboClientSuccessEHS, checkIdeasComboClientFailureEHS);

                <%--function updateIdeasComboClientResult(result) {
                    xmlHttpRequestIdeasResult = new XMLHttpRequest();
                    xmlHttpRequestIdeasResult.open("POST", "<%=GetUpdateIdeaResultPath()%>", true);
                    xmlHttpRequestIdeasResult.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
                    xmlHttpRequestIdeasResult.timeout = 5000;
                    xmlHttpRequestIdeasResult.onreadystatechange = function () {
                        //if (xmlHttpRequestIdeasResult != null) {
                        //    if (xmlHttpRequestIdeasResult.readyState == IC4RA_HTTP_READYSTATE_LOADED) {
                        //        var status = xmlHttpRequestIdeasResult.status;
                        //        xmlHttpRequestIdeasResult = null;
                        //        if (status == IC4RA_HTTP_STATUS_OK) {
                        //            checkIdeasComboClientSuccess(IC4RA_ERRORCODE_SUCCESS);
                        //        } else {
                        //            checkIdeasComboClientFailure(IC4RA_ERRORCODE_NOCLIENT);
                        //        }
                        //    } else {
                        //    }
                        //}
                    };
                    xmlHttpRequestIdeasResult.ontimeout = function () {
                        console.log("Update Result Timeout");
                    };
                    xmlHttpRequestIdeasResult.send(JSON.stringify({ "result": result }));
                }--%>

                function eHSSuccessCallbackFunc(IDEASComboVersion) { //Success get IDEAS Version
                    //alert(IDEASComboVersion);
                    var obj = document.getElementById("<%=txtIDEASComboVersion.ClientID%>")
                    obj.value = IDEASComboVersion
                }
                function eHSFailCallbackFunc() { //IDEAS Combo not yet be installed
                    //alert("eHS IDEAS Combo is not available.");
                    var obj = document.getElementById("<%=txtIDEASComboVersion.ClientID%>")
                    obj.value = ""
                }

                function checkIDEASComboClientAndVersion() {
                    <%--eHR: http://127.0.0.1:44827, eHS: http://127.0.0.1:44927--%>
                    <%--Return:<html><head><title>IDEAS Client Info</title></head><body><h1>IDEAS Client Information</h1>Version: 1.0</body></html>--%>

                    checkIdeasComboClient(checkIdeasComboClientSuccessEHS, checkIdeasComboClientFailureEHS);
                    getIDEASComboVersion();
                }
            </script>

            <asp:Literal ID="AntiForgeryToken" runat="server"></asp:Literal>
            <asp:TextBox ID="NonLoginPageKey" runat="server" Style="display: none" />
            <asp:TextBox ID="txtIDEASComboResult" runat="server" Style="display: none;" />
            <asp:TextBox ID="txtIDEASComboVersion" runat="server" Style="display: none;" />
            <asp:Button ID="btnHiddenPleaseWait" runat="server" Style="display: none;" />
            <asp:Panel ID="pnlPleaseWait" runat="server" Style="display: none; visibility: hidden">
                <table style="width: 150px; height: 150px; background-color: #ffffff" border="0"
                    cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            <asp:Image ID="imgLoading" runat="server" ImageUrl="<%$ Resources:ImageUrl, PleaseWait %>"
                                AlternateText="<%$ Resources:AlternateText, PleaseWait %>" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, Information %>"></asp:Label></td>
                            <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" style="width: 60px; height: 42px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/Information.png" /></td>
                                    <td align="left" style="height: 42px">
                                        <asp:Label ID="lblMsg" runat="server" Text="<%$ Resources:Text, PopupBlockerSuggestion %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, OKBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, OKBtn %>" /></td>
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

            <asp:Button runat="server" ID="btnHiddenLoginConfirmMsg" Style="display: none" />
            <asp:Panel ID="panLoginConfirmMsg" runat="server" Style="display: none;">
                <asp:Panel ID="panLoginConfirmMsgHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 480px">
                        <tr>
                            <td style="background-image: url(Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblLoginConfirmMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 480px">
                    <tr>
                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 60px; height: 42px" valign="middle">
                                        <asp:Image ID="imgLoginConfirmMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="labelDescribeConcurrentAccess" runat="server" Font-Bold="True" Text="<%$ Resources:Text, DescribeConcurrentAccess %>" />
                                        <asp:LinkButton ID="LinkButtonExplainConcurrentAccess" runat="server" Font-Bold="True"></asp:LinkButton>
                                        <asp:Label ID="labelConfirmConcurrentAccess" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmConcurrentAccess %>" />
                                    </td>
                                    <td align="left" style="width: 40px; height: 42px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3" style="height: 42px">
                                        <asp:ImageButton ID="ibtnLoginCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnLoginCancel_Click" />
                                        <asp:ImageButton ID="ibtnLoginProceed" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnLoginProceed_Click" />
                                    </td>
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

            <asp:Button runat="server" ID="btnHiddenReminderWindowsVersion" Style="display: none" />
            <asp:Panel Style="display: none" ID="panReminderWindowsVersion" runat="server" Width="540px">
                    <uc1:ucNoticePopUp ID="ucNoticePopUpReminderWindowsVersion" runat="server" NoticeMode="Custom" IconMode="Information" ButtonMode="OK" DialogImagePath="Images/dialog/"
                                     HeaderText="<%$ Resources:Text, ReminderTitle %>" MessageText="<%$ Resources:Text, ReminderWindowsVersion %>"/>
            </asp:Panel>

            <asp:Button runat="server" ID="btnHiddenShowCopyList" Style="display: none" />

            <cc2:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlPleaseWait"
                TargetControlID="btnHiddenPleaseWait" BackgroundCssClass="modalBackgroundTransparent" />

            <cc2:ModalPopupExtender ID="ModalPopupExtenderPopupWarning" runat="server" TargetControlID="btnHiddenShowCopyList"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                BehaviorID="mdlPopupBlocker" DropShadow="False" RepositionMode="None" OkControlID="ibtnDialogConfirm"
                OnOkScript="okClick()" />

            <cc2:ModalPopupExtender ID="ModalPopupExtenderConfirm" runat="server" TargetControlID="btnHiddenLoginConfirmMsg"
                PopupControlID="panLoginConfirmMsg" BehaviorID="mdlPopupConcurrentBrowser" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panLoginConfirmMsgHeading" />

            <cc2:ModalPopupExtender ID="ModalPopupExtenderReminderWindowsVersion" runat="server" TargetControlID="btnHiddenReminderWindowsVersion"
                PopupControlID="panReminderWindowsVersion" BehaviorID="mdlPopupReminderWindowsVersion" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panReminderWindowsVersionHeading" />

            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(Images/master/banner_header_en.jpg); width: 985px; background-repeat: no-repeat; height: 100px"
                runat="server">
                <tr>
                    <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironment">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                    <td align="right" valign="top" style="white-space: nowrap">
                        <asp:LinkButton ID="lnkbtnTextOnlyVersion" runat="server" CssClass="languageText"
                            PostBackUrl="~/text/login.aspx">Text Only Version</asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnTradChinese" runat="server" CssClass="languageText">繁體</asp:LinkButton><asp:LinkButton
                            ID="lnkbtnSimpChinese" runat="server" CssClass="languageText" Visible="false">简体</asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnEnglish" runat="server" CssClass="languageText">English</asp:LinkButton></td>
                </tr>
                <tr>
                    <td align="left" valign="bottom">
                        <asp:Label ID="lbl_welcome" runat="server" CssClass="bannerText"></asp:Label>
                        <asp:Label ID="lbl_loginName" runat="server" CssClass="bannerText"></asp:Label></td>
                </tr>
            </table>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 985px;">
                    <tr>
                        <td style="background-image: url(Images/master/background.jpg); background-position: bottom; background-repeat: repeat-x; height: 546px"
                            valign="top">
                            <asp:MultiView ID="loginMultiView" runat="server" ActiveViewIndex="0">
                                <asp:View ID="LoginView" runat="server">
                                    <table style="width: 100%; height: 100%;">
                                        <tr>
                                            <td style="width: 200px; border-right: #ffffff 2px solid;" valign="top">
                                                <asp:Menu ID="Menu1" runat="server" CssClass="menuUnselect" StaticSubMenuIndent=""
                                                    Width="100%" Visible="false">
                                                    <StaticMenuStyle CssClass="menuUnSelect" />
                                                    <StaticMenuItemStyle CssClass="menuUnSelect" />
                                                    <StaticSelectedStyle CssClass="menuSelect" />
                                                    <StaticHoverStyle CssClass="menuSelect" />
                                                </asp:Menu>
                                                <asp:GridView ID="gvMenu" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                                    ShowHeader="false" Width="100%" SkinID="GridViewSkin">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 45px" valign="middle">
                                                                            <asp:ImageButton ID="ibtnMenuItem" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' /></td>
                                                                        <td align="left" valign="middle">
                                                                            <asp:LinkButton ID="lnkbtnMenuItem" runat="server" Text="[CodeBehind]"
                                                                                CommandArgument='<%# Eval("URL") %>' CommandName="Redirect"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:HiddenField ID="hfMenuRole" runat="server" Value='<%# Eval("Role") %>' />
                                                                <asp:HiddenField ID="hfEffectiveDate" runat="server" Value='<%# Eval("Effective_Date") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Height="58px" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                            <td valign="top">

                                                <asp:Image ID="img_header" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/banner/banner_login.png" /><br />
                                                <table id="table2" cellpadding="0" cellspacing="0" border="0" style="width: 750px">
                                                    <tr>
                                                        <td style="width: 20px; height: 50px"></td>
                                                        <td class="tableText" colspan="3" style="height: 50px; width: 750px;" valign="top">
                                                            <asp:Label ID="lbl_loginfail_text" runat="server" CssClass="validateFail"></asp:Label>
                                                            <asp:Panel ID="pnlNewsMessage" runat="server" CssClass="messageText" BorderWidth="1px"
                                                                BorderColor="#666666" Width="700px">
                                                                &nbsp;<asp:Label ID="lblNewsMessageTitle" runat="server" Text="<%$ Resources:Text, SystemMessage %>"
                                                                    Font-Underline="True" Font-Bold="True" Height="20px" Font-Size="12pt"></asp:Label>
                                                                <asp:DataList ID="dlNewsMessage" runat="server" RepeatColumns="1" ShowFooter="False"
                                                                    ShowHeader="False">
                                                                    <ItemTemplate>
                                                                        <table border="0" style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 120px" valign="top">
                                                                                    <asp:Label ID="lblCreateDate" runat="server" Text='<%# Bind("CreateDtm") %>'></asp:Label>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </asp:Panel>
                                                            <br />
                                                            <cc1:MessageBox ID="udcMessageBox" runat="server"></cc1:MessageBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20px; height: 40px"></td>
                                                        <td class="tableText" style="width: 260px; height: 40px" valign="top">
                                                            <asp:Label ID="lblRoleText" runat="server" CssClass="tableTitle" Text="Account Type"></asp:Label></td>
                                                        <td colspan="2" style="height: 40px; width: 490px;" valign="top">
                                                            <asp:RadioButtonList ID="rbLoginRole" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                                                TabIndex="1" CellPadding="0" CellSpacing="0" CssClass="tableTitle" Width="305px">
                                                                <asp:ListItem Value="S" Selected="True">Service Provider</asp:ListItem>
                                                                <asp:ListItem Value="D">Data Entry Account</asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20px; height: 30px;"></td>
                                                        <td style="width: 260px; height: 30px;" class="tableText" valign="top">
                                                            <asp:Label ID="lblLoginAliasText" Text="Service Provider ID / Username" runat="server"
                                                                CssClass="tableTitle" Width="260px"></asp:Label>
                                                            <asp:Label ID="lblUsernameText" runat="server" CssClass="tableTitle" Text="Login ID"
                                                                Visible="False"></asp:Label></td>
                                                        <td style="width: 230px; height: 30px;" valign="top">
                                                            <asp:TextBox ID="txtUserName" runat="server" Width="150px" TabIndex="2" onblur="convertToUpper(this)"
                                                                MaxLength="20"></asp:TextBox>
                                                            <asp:Image ID="imgUserNameAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                        </td>
                                                        <td style="width: 260px; height: 30px;" valign="top">
                                                            <asp:HyperLink ID="hylCantAccessAccount" runat="server" NavigateUrl="~/RecoverLogin/RecoverLogin.aspx"
                                                                CssClass="RecoverAccountLink"></asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20px; height: 10px;" />
                                                        <td style="width: 260px; height: 10px;" class="tableText" valign="top" />
                                                        <td style="width: 230px; height: 10px;" valign="top" />
                                                        <td valign="top" rowspan="4" style="width: 260px;">
                                                            <asp:Image ID="imgToken" runat="server" ImageUrl="<%$ Resources:ImageUrl, Token %>"
                                                                AlternateText="<%$ Resources:AlternateText, Token %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20px; height: 40px"></td>
                                                        <td class="tableText" style="width: 260px; height: 40px" valign="top">
                                                            <asp:Label ID="lblPasswordText" runat="server" CssClass="tableTitle" Text="Password"></asp:Label></td>
                                                        <td style="width: 230px; height: 40px" valign="top">
                                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px" TabIndex="3"
                                                                MaxLength="20"></asp:TextBox>
                                                            <asp:Image ID="imgPasswordAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20px; height: 40px"></td>
                                                        <td class="tableText" style="width: 260px; height: 40px" valign="top">
                                                            <asp:Label ID="lblPinNoText" runat="server" CssClass="tableTitle" Text="PIN Code"
                                                                Visible="False"></asp:Label><asp:Label ID="lblSPIDText" runat="server" CssClass="tableTitle"
                                                                    Text="Service Provider ID / Username" Width="260px"></asp:Label></td>
                                                        <td style="width: 230px; height: 40px" valign="top">
                                                            <asp:TextBox ID="txtPinNo" runat="server" TextMode="Password" Visible="False" Width="150px"
                                                                TabIndex="4" MaxLength="6"></asp:TextBox>
                                                            <asp:Image ID="imgPinNoAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                            <asp:TextBox ID="txtSPID" runat="server" Width="150px" TabIndex="5" onblur="convertToUpper(this)"
                                                                MaxLength="20"></asp:TextBox>
                                                            <asp:Image ID="imgSPIDAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20px; height: 18px;" />
                                                        <td style="width: 260px; height: 18px;" class="tableText" valign="top" />
                                                        <td style="width: 230px; height: 18px;" valign="top" />
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20px; height: 40px;"></td>
                                                        <td class="tableText" style="width: 260px; height: 40px;"></td>
                                                        <td style="width: 230px; height: 40px;" valign="top">
                                                            <asp:ImageButton ID="ibtnLogin" runat="server" ImageUrl="~/Images/button/btn_login.png"
                                                                TabIndex="6" AlternateText="Login" />
                                                        </td>
                                                        <td style="width: 260px; height: 40px;"></td>
                                                    </tr>
                                                    <%--<tr>
                                            <td colspan="4" valign="bottom" align="right">
                                                <a href="http://www.chp.gov.hk/view_content.asp?lang=en&info_id=16615" target="_blank"><img src="Images/others/btn_swine.jpg" alt="" border="0"/></a> &nbsp; &nbsp;
                                                &nbsp; &nbsp;&nbsp; &nbsp;</td>
                                        </tr>--%>
                                                </table>
                                                <asp:Panel ID="paniAMSmart" runat="server">
                                                    <hr style="width:95%;" />
                                                    <br />
                                                    <asp:Button ID="btniAMSmart" runat="server" CssClass="dark"
                                                        Text ="<%$ Resources:Text, iAMSmartLogin %>"
                                                        TabIndex="7" 
                                                        AlternateText="Login"
                                                        style="width:200px;position:relative;left:280px" OnClick="btniAMSmart_Click" />
        <%--                                        <asp:Button ID="Button1" runat="server" CssClass="dark"
                                                        Text ="<%$ Resources:Text, iAMSmartLogin %>"
                                                        TabIndex="7" 
                                                        AlternateText="Login"
                                                        style="position:relative;left:280px" />--%>
                                                    <asp:HyperLink ID="lnkiAMSmart" runat="server" NavigateUrl="<%$ Resources:Url, iAMSmart %>" Target="_blank" Text ="<%$ Resources:Text, iAMSmartMoreInfo %>" 
                                                        style="position:relative;left:350px;top:10px;color:blue" rel="noopener noreferrer" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>

                                <asp:View ID="SPHashPWExpiredView" runat="server">
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Image ID="img_SPHashPWExpiredHeader" runat="server" AlternateText="<%$ Resources:AlternateText, LoginInfoBanner%>"
                                                    ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, LoginHeader%>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20px">
                                            </td>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:Label ID="lbl_SPHashPWExpiredMsg" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SPHashPWExpiredMsg %>"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            &nbsp
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 62px">
                                                            <asp:ImageButton ID="btn_SPHashPWExpiredBackToLogin" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="btn_SPProceedToResetPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ProceedToRecoverLoginBtn %>" 
                                                                AlternateText="<%$ Resources:AlternateText, ProceedToRecoverLoginBtn %>"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>


                                <asp:View ID="DEHashPWExpiredView" runat="server">
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Image ID="img_DEHashPWExpiredHeader" runat="server" AlternateText="<%$ Resources:AlternateText, LoginInfoBanner%>"
                                                    ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, LoginHeader%>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20px">
                                            </td>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:Label ID="lbl_DEHashPWExpired" runat="server" CssClass="tableText" Text="<%$ Resources:Text, DEHashPWExpiredMsg %>"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            &nbsp
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 62px">
                                                            <asp:ImageButton ID="btn_DEHashPWExpiredBackToLogin" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" />
                                                        </td>
                                                        <td align="center">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                            </asp:MultiView>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #d3d3d3" valign="top">
                            <table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2" style="height: 2px">
                                        <hr style="width: 98%; color: #ffffff; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkBtnPrivacyPolicy" runat="server" CssClass="footerText" Text="<%$ Resources:Text, PrivacyPolicy %>"
                                            TabIndex="-1"></asp:LinkButton>
                                        <asp:Label ID="lblseparator1" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                            ID="lnkBtnDisclaimer" runat="server" CssClass="footerText" Text="<%$ Resources:Text, ImportantNotices %>"
                                            TabIndex="-1"></asp:LinkButton>
                                        <asp:Label ID="lblseparator2" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                            ID="lnkBtnSysMaint" runat="server" CssClass="footerText" Text="<%$ Resources:Text, SysMaint %>"></asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblFooterCopyright" runat="server" CssClass="footerText" Text="© Copyright Hospital Authority . All rights reserved."
                                            Visible="False"></asp:Label>
                                        &nbsp; &nbsp; &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>

            <script type="text/javascript">
                if (document.all) {
                    top.window.resizeTo(screen.availWidth, screen.availHeight);
                }
                else if (document.layers || document.getElementById) {
                    if (top.window.outerHeight < screen.availHeight) {
                        top.window.outerHeight = screen.availHeight;
                    }
                    if (top.window.outerWidth < screen.availWidth) {
                        top.window.outerWidth = screen.availWidth;
                    }
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>