<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="MyProfileV2.aspx.vb" Inherits="HCSP.MyProfileV2" Title="<%$ Resources: Title, MyProfile %>" %>

<%@ Register Src="../UIControl/PCDIntegration/ucPCDEnrolledPopup.ascx" TagName="ucPCDEnrolledPopup"
    TagPrefix="uc5" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UIControl/SchemeLegend.ascx" TagName="SchemeLegend" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/PCDIntegration/ucTypeOfPracticePopup.ascx" TagName="ucTypeOfPracticePopup"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/Token/ucInputTokenPopUp.ascx" TagName="ucInputTokenPopUp" TagPrefix="uc4" %>
<%@ Register Src="../UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucReturnCodeHandlingPopUp" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .MyProfilePracticeScheme td {
            border: 1px solid #AAAAAA;
        }

        .watermarkText {
            color: gray;
            font-size: 12px;
        }

        
        .ChkEHRSSConsent {
            padding-left: 25px;
            padding-top: 5px;
            text-indent: -20px;
        }

    </style>

    <script language="javascript" src="../JS/Common.js" type="text/javascript"></script>

    <script type="text/javascript">

        function convertToUpper(textbox) { textbox.value = textbox.value.toUpperCase(); }

        function activateIVRSPWD(chkbox) {
            var val = chkbox.checked
            if (val) {
                document.getElementById('<%=PanelIVRSPasswordSettings.ClientID %>').className = "SettingsPanelStyle";
                document.getElementById('<%=PanelIVRSPassword01.ClientID %>').style["display"] = "block";
                document.getElementById('<%=PanelIVRSPassword02.ClientID %>').style["display"] = "block";
                document.getElementById('<%=PanelIVRSPasswordTips.ClientID %>').style["display"] = "block";

                document.getElementById('<%=tdIVRSPwdCell01.ClientID %>').className = "SettingTDRightStyle";
                document.getElementById('<%=tdIVRSPwdCell02.ClientID %>').className = "SettingTDRightStyle";
                document.getElementById('<%=tdIVRSPwdCell03.ClientID %>').className = "SettingTDRightStyle";

                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').disabled = false;
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').disabled = false;

                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
            }
            else {
                document.getElementById('<%=PanelIVRSPasswordSettings.ClientID %>').className = "";
                document.getElementById('<%=PanelIVRSPassword01.ClientID %>').style["display"] = "none";
                document.getElementById('<%=PanelIVRSPassword02.ClientID %>').style["display"] = "none";
                document.getElementById('<%=PanelIVRSPasswordTips.ClientID %>').style["display"] = "none";

                document.getElementById('<%=tdIVRSPwdCell01.ClientID %>').className = "";
                document.getElementById('<%=tdIVRSPwdCell02.ClientID %>').className = "";
                document.getElementById('<%=tdIVRSPwdCell03.ClientID %>').className = "";

                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').disabled = true;
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').disabled = true;

                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').value = '';
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').value = '';

                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').style.backgroundColor = "#E0E0E0";
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').style.backgroundColor = "#E0E0E0";
            }
            return false;
        }

        function changeIVRSPWD(chkbox) {
            var val = chkbox.checked
            if (val) {
                document.getElementById('<%=PanelIVRSPasswordSettings.ClientID %>').className = "SettingsPanelStyle";
                document.getElementById('<%=PanelIVRSPassword01.ClientID %>').style["display"] = "block";
                document.getElementById('<%=PanelIVRSPassword02.ClientID %>').style["display"] = "block";
                document.getElementById('<%=PanelIVRSPasswordTips.ClientID %>').style["display"] = "block";

                document.getElementById('<%=tdIVRSPwdCell01.ClientID %>').className = "SettingTDRightStyle";
                document.getElementById('<%=tdIVRSPwdCell02.ClientID %>').className = "SettingTDRightStyle";
                document.getElementById('<%=tdIVRSPwdCell03.ClientID %>').className = "SettingTDRightStyle";

                document.getElementById('<%=txtIVRSOldPwd.ClientID%>').disabled = false;
                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').disabled = false;
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').disabled = false;

                document.getElementById('<%=txtIVRSOldPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
            }
            else {
                document.getElementById('<%=PanelIVRSPasswordSettings.ClientID %>').className = "";
                document.getElementById('<%=PanelIVRSPassword01.ClientID %>').style["display"] = "none";
                document.getElementById('<%=PanelIVRSPassword02.ClientID %>').style["display"] = "none";
                document.getElementById('<%=PanelIVRSPasswordTips.ClientID %>').style["display"] = "none";

                document.getElementById('<%=tdIVRSPwdCell01.ClientID %>').className = "";
                document.getElementById('<%=tdIVRSPwdCell02.ClientID %>').className = "";
                document.getElementById('<%=tdIVRSPwdCell03.ClientID %>').className = "";

                document.getElementById('<%=txtIVRSOldPwd.ClientID%>').disabled = true;
                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').disabled = true;
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').disabled = true;

                document.getElementById('<%=txtIVRSOldPwd.ClientID%>').value = '';
                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').value = '';
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').value = '';

                document.getElementById('<%=txtIVRSOldPwd.ClientID%>').style.backgroundColor = "#E0E0E0";
                document.getElementById('<%=txtIVRSNewPwd.ClientID%>').style.backgroundColor = "#E0E0E0";
                document.getElementById('<%=txtConfirmNewIVRSPwd.ClientID%>').style.backgroundColor = "#E0E0E0";

                //        checkPassword(document.getElementById('<%=txtNewWebPwd.ClientID%>').value,'strength1a','strength2a','strength3a','progressBar2', '', '', '','direction2a','direction1a');
            }
            return false;
        }

        function changeWebPWD(chkbox) {
            var val = chkbox.checked
            if (val) {
                document.getElementById('<%=PanelWebPasswordSettings.ClientID %>').className = "SettingsPanelStyle";
                document.getElementById('<%=tdPwdCell01.ClientID %>').className = "SettingTDRightStyle";
                document.getElementById('<%=tdPwdCell02.ClientID %>').className = "SettingTDRightStyle";
                document.getElementById('<%=tdPwdCell03.ClientID %>').className = "SettingTDRightStyle";
                document.getElementById('<%=PanelWebPassword01.ClientID %>').style["display"] = "block";
                document.getElementById('<%=PanelWebPassword02.ClientID %>').style["display"] = "block";
                document.getElementById('<%=PanelWebPasswordTips.ClientID %>').style["display"] = "block";

                document.getElementById('<%=txtOldWebPwd.ClientID%>').disabled = false;
                document.getElementById('<%=txtNewWebPwd.ClientID%>').disabled = false;
                document.getElementById('<%=txtConfirmNewWebPwd.ClientID%>').disabled = false;

                document.getElementById('<%=txtOldWebPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
                document.getElementById('<%=txtNewWebPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
                document.getElementById('<%=txtConfirmNewWebPwd.ClientID%>').style.backgroundColor = "#FFFFFF";
            }
            else {

                document.getElementById('<%=PanelWebPasswordSettings.ClientID %>').className = "";
                document.getElementById('<%=tdPwdCell01.ClientID %>').className = "";
                document.getElementById('<%=tdPwdCell02.ClientID %>').className = "";
                document.getElementById('<%=tdPwdCell03.ClientID %>').className = "";
                document.getElementById('<%=PanelWebPassword01.ClientID %>').style["display"] = "none";
                document.getElementById('<%=PanelWebPassword02.ClientID %>').style["display"] = "none";
                document.getElementById('<%=PanelWebPasswordTips.ClientID %>').style["display"] = "none";

                document.getElementById('<%=txtOldWebPwd.ClientID%>').value = '';
                document.getElementById('<%=txtNewWebPwd.ClientID%>').value = '';
                document.getElementById('<%=txtConfirmNewWebPwd.ClientID%>').value = '';

                clearPassword('strength1a', 'strength2a', 'strength3a', 'progressBar2', 'direction2a', 'direction1a')

                document.getElementById('<%=txtOldWebPwd.ClientID%>').disabled = true;
        document.getElementById('<%=txtNewWebPwd.ClientID%>').disabled = true;
                document.getElementById('<%=txtConfirmNewWebPwd.ClientID%>').disabled = true;

                document.getElementById('<%=txtOldWebPwd.ClientID%>').style.backgroundColor = "#E0E0E0";
                document.getElementById('<%=txtNewWebPwd.ClientID%>').style.backgroundColor = "#E0E0E0";
                document.getElementById('<%=txtConfirmNewWebPwd.ClientID%>').style.backgroundColor = "#E0E0E0";


            }
            return false;
        }

        function changeDEPWD(chkbox, changeBy) {
            var val = chkbox.checked
            if (val) {
                if (changeBy == "SP") {
                    document.getElementById('<%=PanelDEWebPasswordSettings.ClientID %>').className = "SettingsPanelStyle";
            document.getElementById('<%=PanelDEWebPassword.ClientID %>').style["display"] = "block";

            document.getElementById('<%=txtDENewPWD.ClientID%>').disabled = false;
            document.getElementById('<%=txtDEConfirmNewPWD.ClientID%>').disabled = false;

            document.getElementById('<%=txtDENewPWD.ClientID%>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=txtDEConfirmNewPWD.ClientID%>').style.backgroundColor = "#FFFFFF";
        } else {
            document.getElementById('<%=PanelDEChangeWebPasswordSettings.ClientID %>').className = "SettingsPanelStyle";
            document.getElementById('<%=PanelDEChangeWebPassword01.ClientID %>').style["display"] = "block";
            document.getElementById('<%=PanelDEChangeWebPassword02.ClientID %>').style["display"] = "block";
            document.getElementById('<%=PanelDEChangeWebPasswordTips.ClientID %>').style["display"] = "block";

            document.getElementById('<%=tdDEChgPwdCell01.ClientID %>').className = "SettingTDRightStyle";
            document.getElementById('<%=tdDEChgPwdCell02.ClientID %>').className = "SettingTDRightStyle";
            document.getElementById('<%=tdDEChgPwdCell03.ClientID %>').className = "SettingTDRightStyle";

            //Data entry change        
            document.getElementById('<%=txtDEOLDPassword.ClientID%>').disabled = false;
            document.getElementById('<%=txtDENEWPassword.ClientID%>').disabled = false;
            document.getElementById('<%=txtDEConfirmNEWPassword.ClientID%>').disabled = false;

            document.getElementById('<%=txtDEOLDPassword.ClientID%>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=txtDENEWPassword.ClientID%>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%=txtDEConfirmNEWPassword.ClientID%>').style.backgroundColor = "#FFFFFF";
        }
    }
    else {
        if (changeBy == "SP") {
            document.getElementById('<%=PanelDEWebPasswordSettings.ClientID %>').className = "";
              document.getElementById('<%=PanelDEWebPassword.ClientID %>').style["display"] = "none";

              document.getElementById('<%=txtDENewPWD.ClientID%>').disabled = true;
              document.getElementById('<%=txtDEConfirmNewPWD.ClientID%>').disabled = true;

              document.getElementById('<%=txtDENewPWD.ClientID%>').value = '';
              document.getElementById('<%=txtDEConfirmNewPWD.ClientID%>').value = '';

              document.getElementById('<%=txtDENewPWD.ClientID%>').style.backgroundColor = "#E0E0E0";
              document.getElementById('<%=txtDEConfirmNewPWD.ClientID%>').style.backgroundColor = "#E0E0E0";

              clearPassword('strength1', 'strength2', 'strength3', 'progressBar', 'direction2', 'direction1')

              //checkPassword(document.getElementById('<%=txtDENewPWD.ClientID%>').value,'strength1','strength2','strength3','progressBar', '', '', '','direction2','direction1');
          } else {

              document.getElementById('<%=PanelDEChangeWebPasswordSettings.ClientID %>').className = "";
              document.getElementById('<%=PanelDEChangeWebPassword01.ClientID %>').style["display"] = "none";
              document.getElementById('<%=PanelDEChangeWebPassword02.ClientID %>').style["display"] = "none";
              document.getElementById('<%=PanelDEChangeWebPasswordTips.ClientID %>').style["display"] = "none";

              document.getElementById('<%=tdDEChgPwdCell01.ClientID %>').className = "";
              document.getElementById('<%=tdDEChgPwdCell02.ClientID %>').className = "";
              document.getElementById('<%=tdDEChgPwdCell03.ClientID %>').className = "";

              //Data entry change        
              document.getElementById('<%=txtDEOLDPassword.ClientID%>').disabled = true;
              document.getElementById('<%=txtDENEWPassword.ClientID%>').disabled = true;
              document.getElementById('<%=txtDEConfirmNEWPassword.ClientID%>').disabled = true;

              document.getElementById('<%=txtDEOLDPassword.ClientID%>').value = '';
              document.getElementById('<%=txtDENEWPassword.ClientID%>').value = '';
              document.getElementById('<%=txtDEConfirmNEWPassword.ClientID%>').value = '';

              document.getElementById('<%=txtDEOLDPassword.ClientID%>').style.backgroundColor = "#E0E0E0";
              document.getElementById('<%=txtDENEWPassword.ClientID%>').style.backgroundColor = "#E0E0E0";
              document.getElementById('<%=txtDEConfirmNEWPassword.ClientID%>').style.backgroundColor = "#E0E0E0";

              clearPassword('strength1DE', 'strength2DE', 'strength3DE', 'progressBarDE', 'direction2DE', 'direction1DE')

              //checkPassword(document.getElementById('<%=txtDENEWPassword.ClientID%>').value,'strength1','strength2','strength3','progressBar', '', '', '','direction2','direction1');
          }

    }
    return false;
}

function tabChanges(sender, e) {
    var activetabindex = sender.get_activeTab().get_tabIndex();
    document.getElementById('<%=txtTabIndex.ClientID%>').value = activetabindex;
  }

        // For popup of Get Username from eHRSS
        function changeEHRSSConsent(chkbox) {
            var val = chkbox.checked;

            if (val) {
                if (document.getElementById('<%=ibtnconsentconfirm.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirm.clientid %>').style.display = "inline-block";
                }
                if (document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>').style.display = "none";
                }
            }
            else {
                if (document.getElementById('<%=ibtnconsentconfirm.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirm.clientid %>').style.display = "none";
                }
                if (document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>').style.display = "inline-block";
                }
            }
            return false;
        }
    </script>

    <asp:Image ID="imgHeader" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources: AlternateText, MyProfileBanner %>"
        ImageUrl="<%$ Resources: ImageURL, MyProfileBanner %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TextBox ID="txtTabIndex" runat="Server" Style="display: none" />
            <cc2:InfoMessageBox ID="udcInfoMsgBox" runat="server" Width="90%" />
            <cc2:MessageBox ID="udcErrMsgBox" runat="server" Width="90%" />
            <asp:MultiView ID="mvMyProfile" runat="server" ActiveViewIndex="0">
                <asp:View ID="vServiceProvider" runat="server">
                    <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="m_ajax__tab_xp" ActiveTabIndex="5" OnClientActiveTabChanged="tabChanges"
                        EnableTheming="False">
                        <cc1:TabPanel ID="tabPersonalParticulars" runat="server" HeaderText="<%$ Resources:Text, PersonalParticulars %>">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0" width="950px">
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSPNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSPEname" runat="server" CssClass="tableText"></asp:Label>
                                            <asp:Label ID="lblSPCname" runat="server" CssClass="tableTextChi"></asp:Label></td>
                                    </tr>
                                    <tr style="display: none">
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSPHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSPAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSPAddress" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSPEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSPEmail" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSPContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSPContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSPFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSPFax" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSPEnrolledSchemeText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:GridView ID="gvSPEnrolledScheme" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvSPEnrolledScheme_RowDataBound"
                                                Width="95%" BackColor="#777777" BorderColor="#999999" BorderStyle="None" BorderWidth="0px"
                                                CellPadding="4" CellSpacing="1" UseAccessibleHeader="False" GridLines="Both">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, SchemeName %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSPSchemeName" runat="server" Text='<%# Eval("SchemeCode") %>' CssClass="tableText" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Status  %>">
                                                        <ItemStyle VerticalAlign="Top" Width="50px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSPRecordStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                                CssClass="tableText" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveDate  %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEffectiveTime" runat="server" Text='<%# Eval("EffectiveDtm") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Top" Width="125px" BackColor="#ffffff" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingDate  %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDelistTime" runat="server" Text='<%# Eval("DelistDtm") %>' CssClass="tableText"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Top" Width="125px" BackColor="#ffffff" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#F7F7DE" />
                                                <RowStyle BackColor="White" CssClass="tableText" />
                                                <HeaderStyle BackColor="White" ForeColor="#666666" VerticalAlign="Top" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabMOInfo" runat="server" HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkMOShowActiveOnly" runat="server" AutoPostBack="True" Checked="True"
                                    OnCheckedChanged="chkShowActive_CheckedChanged" Text="<%$ Resources:Text, ShowActiveOnly %>" /><br />
                                <asp:GridView ID="gvMOInfo" runat="server" AutoGenerateColumns="False" Width="950px"
                                    BackColor="#777777" BorderColor="#999999" BorderStyle="None" BorderWidth="0px"
                                    CellPadding="4" CellSpacing="1" UseAccessibleHeader="False" GridLines="Both">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, MONo %>">
                                            <ItemStyle VerticalAlign="Top" Width="60px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblMODispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblConfirmMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblConfirmMOEName" runat="server" Text='<%# Eval("MOEngName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:Label ID="lblConfirmMOCName" runat="server" Text='<%# formatChineseString(Eval("MOChiName")) %>'
                                                                CssClass="tableTextChi"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblConfirmMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblConfirmMOBRCode" runat="server" Text='<%# Eval("BrCode") %>' CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblConfirmMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblConfirmMOContactNo" runat="server" Text='<%# Eval("PhoneDaytime") %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblConfirmMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblConfirmMOEmail" runat="server" Text='<%# Eval("Email") %>' CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblConfirmMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblConfirmMOFax" runat="server" Text='<%# Eval("Fax") %>' CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblConfirmMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblConfirmMOEAddress" runat="server" Text='<%# formatAddress(Eval("MOAddress.Room"), Eval("MOAddress.Floor"), Eval("MOAddress.Block"), Eval("MOAddress.Building"), Eval("MOAddress.District"), Eval("MOAddress.AreaCode"))%>'
                                                                CssClass="tableText"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lblConfirmMOCAddress" runat="server" Text='<%#  formatAddressChi(Eval("MOAddress.Room"), Eval("MOAddress.Floor"), Eval("MOAddress.Block"), Eval("MOAddress.ChiBuilding"), Eval("MOAddress.District"), Eval("MOAddress.AreaCode")) %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblConfirmMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"
                                                                Visible="True"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblConfirmMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <!--GetPracticeTypeName(Eval("MORelation")) -->
                                                            <asp:Label ID="lblConfirmMORelationRemark" runat="server" Text='<%# formatChineseString(Eval("RelationshipRemark")) %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                        <!--formatChineseString(Eval("PracticeRemark")) -->
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 250px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblMOStatusText" runat="server" Text="<%$ Resources:Text, StatusOfMO %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblMOStatus" runat="server" Text='' CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle BackColor="White" />
                                    <HeaderStyle BackColor="White" ForeColor="#666666" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:GridView>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabPracticeInfo" runat="server" HeaderText="<%$ Resources:Text, PracticeInfo %>">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkShowActive" runat="server" AutoPostBack="True" Checked="True"
                                    OnCheckedChanged="chkShowActive_CheckedChanged" Text="<%$ Resources:Text, ShowActiveOnly %>" /><br />
                                <asp:GridView ID="gvPracticeInfo" runat="server" AutoGenerateColumns="False" Width="950px"
                                    BackColor="#777777" BorderColor="#999999" BorderStyle="None" BorderWidth="0px"
                                    CellPadding="4" CellSpacing="1" UseAccessibleHeader="False" GridLines="Both">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                                            <ItemStyle VerticalAlign="Top" Width="60px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPracticeDisplaySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                                            <ItemTemplate>
                                                <table style="width: 100%" cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMO" runat="server" Text='<%# GetMOName(Eval("MODisplaySeq")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:Label ID="lblChiMO" runat="server" Text='<%# formatChineseString(GetChiMoName(Eval("MODisplaySeq"))) %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:Label ID="lblPracticeChiName" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lblPracticeChiAddress" runat="server" Text='<%# formatAddressChi(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.ChiBuilding"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRegCode" runat="server" Text='<%# Eval("Professional.RegistrationCode") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblPhoneNoText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPhoneNo" runat="server" Text='<%# Eval("PhoneDaytime") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblPracticeMobileClinicText" runat="server" Text="<%$ Resources:Text, SPSResultMobileClinic %>"></asp:Label>
                                                        <td>
                                                            <asp:Label ID="lblPracticeMobileClinic" runat="server" CssClass="tableText" Text='<%# Eval("MobileClinic") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblPracticeRemarksText" runat="server" Text="<%$ Resources:Text, Remarks %>"></asp:Label>
                                                        <td>
                                                            <asp:Label ID="lblPracticeRemarks" runat="server" Text="" CssClass="tableText"></asp:Label><br />
                                                            <asp:Label ID="lblPracticeRemarksChi" runat="server" Text="" CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;" valign="top">
                                                            <asp:Label ID="lblSchemeText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:GridView ID="gvPracticeSchemeInfo" runat="server" CssClass="MyProfilePracticeScheme" AutoGenerateColumns="False"
                                                                Width="100%" OnRowDataBound="gvPracticeSchemeInfo_RowDataBound" OnPreRender="gvPracticeSchemeInfo_PreRender"
                                                                BackColor="#777777" BorderColor="#999999" BorderStyle="None" BorderWidth="0px"
                                                                CellPadding="4" CellSpacing="1" UseAccessibleHeader="False" GridLines="Both">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, SchemeName %>">
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblMSchemeHeader" runat="server" Text='<%$ Resources:Text, Scheme %>'></asp:Label>
                                                                            <asp:ImageButton ID="btnSchemeNameHelp" runat="server" AlternateText="<%$ Resources:Text, Scheme %>"
                                                                                ImageUrl="<%$ Resources:ImageURL, infoicon %>" ImageAlign="AbsMiddle" OnClick="btnSchemeNameHelp_Click" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMScheme" runat="server" Text='<%# Eval("SchemeDisplayCode") %>' CssClass="tableText"></asp:Label>
                                                                            <asp:HiddenField ID="hfMScheme" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                                            <asp:HiddenField ID="hfGIsCategoryHeader" runat="server" Value='<%# Eval("IsCategoryHeader") %>' />
                                                                            <asp:HiddenField ID="hfGCategoryName" runat="server" Value='<%# Eval("CategoryName") %>' />
                                                                            <asp:HiddenField ID="hfGCategoryNameChi" runat="server" Value='<%# Eval("CategoryNameChi") %>' />
                                                                            <asp:HiddenField ID="hfGCategoryNameCN" runat="server" Value='<%# Eval("CategoryNameCN") %>' />
                                                                            <asp:HiddenField ID="hfGAllNotProvideService" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="100px" BackColor="#ffffff" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblSubsidyHeader" runat="server" Text='<%$ Resources: Text, SubsidyAndServiceFee %>'></asp:Label>
                                                                            <asp:ImageButton ID="btnSubsidyHelp" runat="server" AlternateText="<%$ Resources:Text, Scheme %>"
                                                                                ImageUrl="<%$ Resources:ImageURL, infoicon %>" ImageAlign="AbsMiddle" OnClick="btnSubsidyHelp_Click" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblScheme" runat="server" Text='<%# Eval("SubsidizeDisplayCode") %>' CssClass="tableText"></asp:Label>
                                                                            <asp:HiddenField ID="hfScheme" runat="server" Value='<%# Eval("SubsidizeCode") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="65px" BackColor="#ffffff" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblServiceFee" runat="server" CssClass="tableText"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" BackColor="#ffffff" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Status  %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server" CssClass="tableText"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="50px" BackColor="#ffffff" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveDate  %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEffectiveTime" runat="server"
                                                                                CssClass="tableText"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="125px" BackColor="#ffffff" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingDate  %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDelistTime" runat="server" CssClass="tableText"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="125px" BackColor="#ffffff" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <FooterStyle BackColor="#F7F7DE" />
                                                                <RowStyle BackColor="White" CssClass="tableText" />
                                                                <HeaderStyle BackColor="White" ForeColor="#666666" VerticalAlign="Top" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle BackColor="White" />
                                    <HeaderStyle BackColor="White" ForeColor="#666666" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:GridView>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabBankInfo" runat="server" HeaderText="<%$ Resources:Text, BankInfo %>">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkBankAcctShowActiveOnly" runat="server" AutoPostBack="True" Checked="True"
                                    Text="<%$ Resources:Text, ShowActiveOnly %>" OnCheckedChanged="chkBankAcctShowActiveOnly_CheckedChanged" /><br />
                                <asp:GridView ID="gvBankInfo" runat="server" AutoGenerateColumns="False" Width="950px"
                                    BackColor="#777777" BorderColor="#999999" BorderStyle="None" BorderWidth="0px"
                                    CellPadding="4" CellSpacing="1" UseAccessibleHeader="False" GridLines="Both">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                                            <ItemStyle VerticalAlign="Top" Width="60px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPracticeBankDisplaySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, BankInfo %>">
                                            <ItemTemplate>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;padding-top:2px" valign="top">
                                                            <asp:Label ID="lblBankPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label></ br>
                                                            <asp:Label ID="lblBankPracticeChiName" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #fffafa;padding-top:2px" valign="top">
                                                            <asp:Label ID="lblBankPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lblBankPracticeChiAddress" runat="server" Text='<%#  formatAddressChi(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.ChiBuilding"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:20px">
                                                        <td style="width: 200px; background-color: #fffafa;padding-top:2px" valign="top">
                                                            <asp:Label ID="lblBankPracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblBankPracticeStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:20px">
                                                        <td style="width: 200px; background-color: #fffafa;padding-top:2px" valign="top">
                                                            <asp:Label ID="lblBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankAcct.BankName") %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:20px">
                                                        <td style="width: 200px; background-color: #fffafa;padding-top:2px" valign="top">
                                                            <asp:Label ID="lblBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BankAcct.BranchName") %>'
                                                                CssClass="tableTextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:20px">
                                                        <td style="width: 200px; background-color: #fffafa;padding-top:2px" valign="top">
                                                            <asp:Label ID="lblBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBankAcc" runat="server" Text='<%# Eval("BankAcct.BankAcctNo") %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr style="height:20px">
                                                        <td style="width: 200px; background-color: #fffafa;padding-top:2px" valign="top">
                                                            <asp:Label ID="lblBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBankOwner" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                                CssClass="tableText" Width="700px" Style="word-wrap:break-word"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle BackColor="White" />
                                    <HeaderStyle BackColor="White" ForeColor="#666666" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:GridView>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabSystemInfo" runat="server" HeaderText="<%$ Resources:Text, SystemInfo %>"
                            Width="950px">
                            <ContentTemplate>
                                <table style="width: 87%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tableHeading" colspan="3">
                                            <asp:Label ID="lblSPLoginInformation" runat="server" Text="<%$ Resources:Text, LoginInformation %>"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 250px;" valign="middle">
                                            <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>" Height="30px"
                                                Style="font-weight: bold"></asp:Label></td>
                                        <td colspan="1" valign="top" style="width: 370px">
                                            <asp:Label ID="lblSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td colspan="1" style="width: 315px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 250px;" valign="top">
                                            <asp:Label ID="lblUsernameText" runat="server" Text="<%$ Resources:Text, Username %>"
                                                Style="font-weight: bold"></asp:Label>
                                        </td>
                                        <td colspan="2" valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding-right: 5px;">
                                                        <asp:Label ID="lblUsername" runat="server" Visible="False" CssClass="tableText"></asp:Label><asp:TextBox
                                                            ID="txtUsername" runat="server" onChange="convertToUpper(this);" MaxLength="20"></asp:TextBox>
                                                        <asp:Image ID="imgUsernameError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                            Visible="False" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnChkAvail" runat="server" AlternateText="<%$ Resources:AlternateText, CheckAvailabilityBtn %>"
                                                            ImageUrl="<%$ Resources:ImageURL, CheckAvailabilityBtn %>" ImageAlign="AbsMiddle"
                                                            OnClick="btnChkAvail_Click" />
                                                    </td>
                                                </tr>
                                                <tr id="trGetUsernameFromEHRSS" runat="server">
                                                    <td></td>
                                                    <td style="padding-top:4px;">
                                                        <asp:ImageButton ID="btnGetUsernameFromEHRSS" runat="server" ImageUrl="<%$ Resources: ImageUrl, GetUsernameFromEHRSSBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, GetUsernameFromEHRSSBtn %>" ImageAlign="AbsMiddle" OnClick="btnGetUsernameFromEHRSS_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 250px; height: 10px" valign="top"></td>
                                        <td id="tdUsernameTips" runat="server" colspan="2" valign="top" style="width: 315px; display: none">
                                            <asp:Label ID="lblSPUsernameTip" runat="server" Text="<%$ Resources:Text, UsernameTips %>"
                                                Visible="False"></asp:Label><br />
                                            <asp:Label ID="lblSPUsernameTip1" runat="server" Text="<%$ Resources:Text, UsernameTips1 %>"
                                                Visible="False"></asp:Label><br />
                                            <asp:Label ID="lblSPUsernameTip2" runat="server" Text="<%$ Resources:Text, UsernameTips2 %>"
                                                Visible="False"></asp:Label><br />
                                            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                            <asp:Label ID="lblSPUsernameTip2a" runat="server" Text="<%$ Resources:Text, UsernameTips2a %>"
                                                Visible="False"></asp:Label><br />
                                            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                            <asp:Label ID="lblSPUsernameTip2b" runat="server" Text="<%$ Resources:Text, UsernameTips2b %>"
                                                Visible="False"></asp:Label><br />
                                            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                            <asp:Label ID="lblSPUsernameTip2c" runat="server" Text="<%$ Resources:Text, UsernameTips2c %>"
                                                Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 250px; height: 10px" valign="top"></td>
                                        <td colspan="1" style="height: 10px; width: 370px" valign="top"></td>
                                        <td colspan="1" style="height: 10px; width: 315px;" valign="top"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 250px;" valign="middle">
                                            <asp:Label ID="lblTokenSerialNoText" runat="server" Text="<%$ Resources:Text, TokenSerialNo %>"
                                                Height="30px" Style="font-weight: bold"></asp:Label></td>
                                        <td colspan="2" valign="top" style="width: 685px">
                                            <asp:Label ID="lblTokenSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>

                                    <asp:Panel ID="PanelSPSystemSettings" runat="server">
                                        <tr>
                                            <td class="tableHeading" colspan="3">
                                                <asp:Label ID="lblSPSystemSettings" runat="server" Text="<%$ Resources:Text, SystemSettings %>"></asp:Label></td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelSPDefaultLang" runat="server">
                                        <tr>
                                            <td style="width: 250px;" valign="top">
                                                <asp:Label ID="lblDefaultLangText" runat="server" Text="<%$ Resources:Text, DefaultLang %>"
                                                    Height="30px" Width="250px" Style="font-weight: bold"></asp:Label></td>
                                            <td colspan="1" valign="top" style="width: 370px">
                                                <asp:DropDownList ID="ddlDefaultLang" runat="server">
                                                    <asp:ListItem Value="E"></asp:ListItem>
                                                    <asp:ListItem Value="C"></asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td colspan="1" style="width: 315px"></td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelPrintingOptionSetting" runat="server">
                                        <tr>
                                            <td colspan="3" style="height: 16px; width: 87%" valign="top">
                                                <asp:Label ID="lblDefaultPrintOptionText" runat="server" Height="30px" Text="<%$ Resources:Text, DefaultPrintOption %>"
                                                    Style="font-weight: bold"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblSelectPrintOptionText" runat="server" Text="<%$ Resources:Text, SelectPrintFormOption %>"></asp:Label>
                                                <asp:Image ID="imgSPSelectPrintOptionError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 5px"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Table ID="tblPrintOption" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0" CssClass="SettingTableStyle" Width="935">
                                                    <asp:TableRow ID="trNotToPrint" runat="server">
                                                        <asp:TableCell runat="server" VerticalAlign="top" Style="width: 250px" CssClass="SettingTableCellStyle">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbNotPrint" runat="server" GroupName="PrintOption"
                                                                Height="30px" Text="<%$ Resources:Text, NotToPrint %>" />
                                                        </asp:TableCell>
                                                        <asp:TableCell VerticalAlign="top" ColumnSpan="2" Style="width: 685px" CssClass="SettingTableCellStyle">
                                                            <asp:Label runat="server" ID="txtNotPrintRemind" Text="<%$ Resources:Text, NotToPrintRemind %>"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="trPrintFullVersion" runat="server">
                                                        <asp:TableCell runat="server" VerticalAlign="top" Style="width: 250px" CssClass="SettingTableCellStyle">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbPrintFull" runat="server" GroupName="PrintOption"
                                                                Height="30px" Text="<%$ Resources:Text, PrintFullVersion %>" />
                                                        </asp:TableCell>
                                                        <asp:TableCell runat="server" VerticalAlign="top" ColumnSpan="2" CssClass="SettingTableCellStyle">&nbsp;</asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="trPrintCondensed" runat="server">
                                                        <asp:TableCell runat="server" VerticalAlign="top" Style="width: 250px" CssClass="SettingTableCellStyle">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbPrintCondensed" runat="server" GroupName="PrintOption"
                                                                Height="30px" Text="<%$ Resources:Text, PrintCondensedVersion %>" />
                                                        </asp:TableCell>
                                                        <asp:TableCell runat="server" VerticalAlign="top" ColumnSpan="2" CssClass="SettingTableCellStyle">
                                                            <asp:Label ID="txtPrintCondensedRemind" runat="server" Text="<%$ Resources:Text, PrintCondensedVersionRemind %>"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td style="width: 250px; height: 10px" valign="top"></td>
                                        <td colspan="2" style="height: 10px" valign="top"></td>
                                    </tr>

                                    <tr>
                                        <td class="tableHeading" colspan="3" style="height: 15px" valign="top">
                                            <asp:Label ID="lblSPPasswordSetting" runat="server" Text="<%$ Resources:Text, PasswordSettings %>"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="1" valign="top">
                                            <asp:Panel ID="PanelWebPasswordSettings" runat="server" Width="935px">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 190px">
                                                            <asp:CheckBox ID="chkChgWebPWD" runat="server" Text="<%$ Resources:Text, ChgWebPWD %>"
                                                                OnCheckedChanged="chkChgWebPWD_CheckedChanged" Enabled="False" onclick="changeWebPWD(this);"
                                                                Style="font-weight: bold" /></td>
                                                        <td style="width: 355px" class="SettingTDRightStyle" runat="server" id="tdPwdCell01">&nbsp;</td>
                                                        <td style="width: 390px" rowspan="2" valign="top" class="SettingTDRightStyle" runat="server"
                                                            id="tdPwdCell02">
                                                            <asp:Panel ID="PanelWebPasswordTips" runat="server" Width="370px" Style="display: none">
                                                                <asp:Label ID="lblWebPwdTipsText" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"></asp:Label><br />
                                                                <asp:Label ID="lblWebPwdTips1Text" runat="server" Text="<%$ Resources:Text, WebPasswordTips1 %>"></asp:Label><br />
                                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblWebPwdTips1aText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label><br />
                                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblWebPwdTips1bText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label><br />
                                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblWebPwdTips1cText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label><br />
                                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lblWebPwdTips1dText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                                                <asp:Label ID="lblWebPwdTips2Text" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label><br />
                                                                <asp:Label ID="lblWebPwdTips3Text" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label><br />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" style="width: 190px">
                                                            <asp:Panel ID="PanelWebPassword01" runat="server" Width="190px" Style="display: none">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td style="width: 20px; height: 10px"></td>
                                                                        <td style="width: 170px; height: 10px"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 20px; height: 35px;"></td>
                                                                        <td style="width: 170px">
                                                                            <asp:Label ID="lblOldWebPwdText" runat="server" Text="<%$ Resources:Text, OldPassword %>"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 20px; height: 35px;"></td>
                                                                        <td style="width: 170px;">
                                                                            <asp:Label ID="lblNewWebPwdText" runat="server" Text="<%$ Resources:Text, NewPassword %>"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 20px; height: 35px;"></td>
                                                                        <td style="width: 170px">
                                                                            <asp:Label ID="lblConfirmNewWebPwdText" runat="server" Text="<%$ Resources:Text, ConfirmPW %>"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td valign="top" style="width: 355px" class="SettingTDRightStyle" runat="server"
                                                            id="tdPwdCell03">
                                                            <asp:Panel ID="PanelWebPassword02" runat="server" Width="355px" Style="display: none">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="height: 100%">
                                                                    <tr>
                                                                        <td style="height: 10px"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 35px">
                                                                            <asp:TextBox ID="txtOldWebPwd" runat="server" TextMode="Password" MaxLength="20"
                                                                                Width="130px" BackColor="LightGray"></asp:TextBox><asp:Image ID="imgSPOldPWDError"
                                                                                    runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                    Visible="False" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 35px">
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtNewWebPwd" runat="server" TextMode="Password" MaxLength="20"
                                                                                            Width="130px" BackColor="LightGray"></asp:TextBox><asp:Image ID="imgSPNewPWDError"
                                                                                                runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                                Visible="False" /></td>
                                                                                    <td>
                                                                                        <table cellpadding="0" style="width: 210px">
                                                                                            <tr>
                                                                                                <td colspan="5">
                                                                                                    <div id="progressBar2" style="font-size: 1px; height: 10px; width: 210px; border: 1px solid white;" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="width: 210px">
                                                                                                <td align="center" style="width: 30%; height: 2px">
                                                                                                    <span id="strength1a"></span>
                                                                                                </td>
                                                                                                <td style="width: 5%; height: 2px">
                                                                                                    <span id="direction1a"></span>
                                                                                                </td>
                                                                                                <td align="center" style="width: 30%; height: 2px">
                                                                                                    <span id="strength2a"></span>
                                                                                                </td>
                                                                                                <td style="width: 5%; height: 2px">
                                                                                                    <span id="direction2a"></span>
                                                                                                </td>
                                                                                                <td align="center" style="width: 30%; height: 2px">
                                                                                                    <span id="strength3a"></span>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 35px">
                                                                            <asp:TextBox ID="txtConfirmNewWebPwd" runat="server" TextMode="Password" MaxLength="20"
                                                                                Width="130px" BackColor="LightGray"></asp:TextBox><asp:Image ID="imgSPConfirmNewPWDError"
                                                                                    runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                    Visible="False" /></td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" rowspan="1" style="height: 10px" valign="top"></td>
                                    </tr>
                                    <asp:Panel ID="PanelIVRSPasswordSettings" runat="server" Width="935px">
                                        <tr>
                                            <td colspan="3" rowspan="1" style="height: 15px" valign="top">

                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 545px">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="width: 190px">
                                                                        <asp:CheckBox ID="chkChgIVRSPwd" runat="server" Text="<%$ Resources:Text, ChgIVRSPassword %>"
                                                                            Visible="False" OnCheckedChanged="chkChgIVRSPwd_CheckedChanged" Enabled="False"
                                                                            onclick="changeIVRSPWD(this)" Height="30px" Style="font-weight: bold" />
                                                                        <asp:CheckBox ID="chkActivateIVRSPwd" runat="server" Text="<%$ Resources:Text, ActivateIVRSPassword %>"
                                                                            OnCheckedChanged="chkActivateIVRSPwd_CheckedChanged" Visible="False" Enabled="False"
                                                                            onclick="activateIVRSPWD(this)" Height="30px" Style="font-weight: bold" /></td>
                                                                    <td style="width: 355px" runat="server" id="tdIVRSPwdCell01">&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 190px">
                                                                        <asp:Panel ID="PanelIVRSPassword01" runat="server" Style="display: none" Width="190px">
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td style="width: 20px; height: 10px"></td>
                                                                                    <td style="width: 170px; height: 10px"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 20px"></td>
                                                                                    <td style="width: 170px">
                                                                                        <asp:Label ID="lblIVRSOldPwdText" runat="server" Text="<%$ Resources:Text, OldPassword %>"
                                                                                            Height="30px"></asp:Label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 20px; height: 10px"></td>
                                                                                    <td style="width: 170px; height: 10px"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 20px"></td>
                                                                                    <td style="width: 170px">
                                                                                        <asp:Label ID="lblIVRSNewPwdText" runat="server" Text="<%$ Resources:Text, NewPassword %>"
                                                                                            Height="30px"></asp:Label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 20px; height: 10px"></td>
                                                                                    <td style="width: 170px; height: 10px"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 20px"></td>
                                                                                    <td style="width: 170px">
                                                                                        <asp:Label ID="lblConfirmNewIVRSPwdText" runat="server" Text="<%$ Resources:Text, ConfirmPW %>"
                                                                                            Height="30px"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td style="width: 355px" runat="server" id="tdIVRSPwdCell02">
                                                                        <asp:Panel ID="PanelIVRSPassword02" runat="server" Style="display: none" Width="355px">
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td style="width: 195px">
                                                                                        <asp:TextBox ID="txtIVRSOldPwd" runat="server" TextMode="Password" MaxLength="6"
                                                                                            Width="60px" BackColor="LightGray"></asp:TextBox>
                                                                                        <asp:Image ID="imgSPOldIVRSPWDError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                            Visible="False" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 195px; height: 10px"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 195px; height: 30px;">
                                                                                        <asp:TextBox ID="txtIVRSNewPwd" runat="server" TextMode="Password" MaxLength="6"
                                                                                            Width="60px" BackColor="LightGray"></asp:TextBox>
                                                                                        <asp:Image ID="imgSPNewIVRSPWDError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                            Visible="False" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 195px; height: 10px"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 195px">
                                                                                        <asp:TextBox ID="txtConfirmNewIVRSPwd" runat="server" TextMode="Password" MaxLength="6"
                                                                                            Width="60px" BackColor="LightGray"></asp:TextBox>
                                                                                        <asp:Image ID="imgSPConfirmNewIVRSPWDError" runat="server" ImageAlign="AbsMiddle"
                                                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 370px" valign="top" runat="server" id="tdIVRSPwdCell03">
                                                            <asp:Panel ID="PanelIVRSPasswordTips" runat="server" Width="370px" Style="display: none">
                                                                <asp:Label ID="lblIVRSPwdTipsText" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips %>"></asp:Label><br />
                                                                <asp:Label ID="lblIVRSPwdTips1Text" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips1 %>"></asp:Label><br />
                                                                <asp:Label ID="lblIVRSPwdTips2Text" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips2 %>"></asp:Label><br />
                                                                <asp:Label ID="lblIVRSPwdTips3Text" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips3 %>"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                </table>
                                <table style="width: 87%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center" style="height: 15px" valign="top"></td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top" style="height: 33px">
                                            <asp:ImageButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" ImageUrl="<%$ Resources:ImageURL, EditBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, EditBtn %>" />&nbsp;&nbsp;<asp:ImageButton
                                                    ID="btnSave" runat="server" ImageUrl="<%$ Resources:ImageURL, SaveBtn %>" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                    OnClick="btnSave_Click" Visible="False" />&nbsp;&nbsp;<asp:ImageButton ID="btnCancel"
                                                        runat="server" ImageUrl="<%$ Resources:ImageURL, CancelBtn %>" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                        OnClick="btnCancel_Click" Visible="False" /></td>
                                    </tr>
                                </table>
                                <cc1:FilteredTextBoxExtender ID="FilteredUserName" runat="server" TargetControlID="txtUsername"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>

                                <%--<cc1:FilteredTextBoxExtender ID="FilteredOldWebPwd" runat="server" TargetControlID="txtOldWebPwd"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>--%>
                                <cc1:FilteredTextBoxExtender ID="FilteredNewWebPwd" runat="server" TargetControlID="txtNewWebPwd"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredConfirmNewWebPwd" runat="server" TargetControlID="txtConfirmNewWebPwd"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>

                                <cc1:FilteredTextBoxExtender ID="FilteredIVRSOldPwd" runat="server" TargetControlID="txtIVRSOldPwd"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredIVRSNewPwd" runat="server" TargetControlID="txtIVRSNewPwd"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredConfirmNewIVRSPwd" runat="server" TargetControlID="txtConfirmNewIVRSPwd"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>

                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabDataEntryInfo" runat="server" HeaderText="<%$ Resources:Text, DataEntryMaint %>">
                            <ContentTemplate>
                                <table width="950px" cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td colspan="2" valign="top" style="width: 30%; border-right: #b0c4de 1px solid;">
                                            <table style="width: 100%; height: 100%; border-right-width: 2px; border-right-color: #e6e6fa"
                                                border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td colspan="3">
                                                                    <div class="tableHeading">
                                                                        <asp:Label ID="lblDEListText" runat="server" Text="<%$ Resources:Text, DataEntryAcctList %>"
                                                                            Style="white-space: nowrap"></asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tbDEFilter" style="width: 100%" runat="server" valign="top">
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtDEFilter" runat="server" MaxLength="20" onChange="convertToUpper(this);" Style="vertical-align: top; width: 105px">
                                                                    </asp:TextBox>
                                                                    <asp:HiddenField ID="hfDEFilter" runat="server" Value="" />
                                                                    <asp:Image ID="imgDEFilterError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                                                    <asp:ImageButton ID="btnFilterDEAcct" runat="server" ImageUrl="<%$ Resources:ImageURL, FilterDEAcctBtn %>"
                                                                        AlternateText="<%$ Resources:ImageURL, FilterDEAcctBtn %>" OnClick="btnFilterDEAcct_Click" />
                                                                    <asp:ImageButton ID="btnClearDEAcctSearch" runat="server" ImageUrl="<%$ Resources:ImageURL, ClearDEAcctSearchBtn %>"
                                                                        AlternateText="<%$ Resources:ImageURL, ClearDEAcctSearchBtn %>" OnClick="btnClearDEAcctSearch_Click" />

                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtDEFilter" WatermarkText="<%$ Resources:Text, UsernameWatermark %>" WatermarkCssClass="watermarkText" />
                                                                </td>
                                                            </tr>
                                                            <tr style="white-space: nowrap" id="trUsername" align="left">
                                                                <td style="width: 10px">&nbsp;
                                                                </td>
                                                                <td style="font-weight: bold;">
                                                                    <asp:Label ID="lblUsernameHeader" runat="server" Text="<%$ Resources:Text, Username %>"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr style="white-space: nowrap" id="trUsernameFound" visible="false">
                                                                <td style="width: 10px">&nbsp;
                                                                </td>
                                                                <td colspan="2" style="font-weight: bold;">
                                                                    <asp:Label ID="lblUsernameFound" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="white-space: nowrap" id="trUsernameNotFound" visible="false">
                                                                <td colspan="3" style="font-weight: bold;">
                                                                    <asp:Label ID="lblUsernameNotFound" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:GridView ID="gvdataEntryAcc" runat="server" AllowPaging="True" AutoGenerateColumns="False" ShowHeader="False" BorderStyle="none" Border="0px"
                                                            Width="80%" CellPadding="2" BackColor="#BCBCBC" BorderColor="#999999" CellSpacing="1" UseAccessibleHeader="False" GridLines="Both" PageSize="20">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRowNum" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkbtnDEUsername" Text='<%# Bind("Data_Entry_Account") %>' runat="server"
                                                                            CommandArgument='<%# Bind("Data_Entry_Account") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle BackColor="White" />
                                                            <HeaderStyle BackColor="White" ForeColor="#666666" VerticalAlign="Top" HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top" style="width: 70%" align="center">
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <div class="tableHeading">
                                                            <asp:Label ID="lblDEAcctInfoText" runat="server" Text="<%$ Resources:Text, DataEntryAcctInfo %>"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="border-right: silver 1px solid; border-top: silver 1px solid; border-left: silver 1px solid; width: 95%; border-bottom: silver 1px solid; height: 100%"
                                                cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="height: 16px" valign="top" width="10"></td>
                                                    <td align="left" style="width: 150px; height: 16px;" valign="top"></td>
                                                    <td colspan="2" align="left" style="height: 16px; width: 473px;" valign="top"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1" valign="top" width="10"></td>
                                                    <td align="left" class="tableHeading" colspan="3" valign="top">
                                                        <asp:Label ID="lblDELoginInformation" runat="server" Text="<%$ Resources:Text, LoginInformation %>"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" width="10"></td>
                                                    <td align="left" style="width: 150px;" valign="top">
                                                        <asp:Label ID="lblDELoginIDText" runat="server" Text="<%$ Resources:Text, Username %>"
                                                            Style="font-weight: bold"></asp:Label></td>
                                                    <td align="left" valign="top" style="width: 185px">
                                                        <asp:Label ID="lblDELoginID" runat="server" CssClass="tableText" Style="vertical-align: bottom; padding-left: 5px"></asp:Label>
                                                        <asp:TextBox
                                                            ID="txtDEloginID" runat="server" MaxLength="20" Visible="False" onChange="convertToUpper(this);"
                                                            BackColor="LightGray" Enabled="False" Style="vertical-align: top"></asp:TextBox>
                                                        <asp:Image ID="imgDELoginIDError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                            Visible="False" /></td>
                                                    <td id="tdDEUsernameTip" runat="server" align="left" valign="middle">
                                                        <asp:Label ID="lblDEUsernameTip" runat="server" Text="<%$ Resources:Text, UsernameTips %>">
                                                        </asp:Label><br />
                                                        <asp:Label ID="lblDEUsernameTip1" runat="server" Text="<%$ Resources:Text, UsernameTips1 %>">
                                                        </asp:Label><br />
                                                        <asp:Label ID="lblDEUsernameTip2" runat="server" Text="<%$ Resources:Text, UsernameTips2 %>"
                                                            Visible="False"></asp:Label><br />
                                                        <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        <asp:Label ID="lblDEUsernameTip2a" runat="server" Text="<%$ Resources:Text, UsernameTips2a %>"
                                                            Visible="False"></asp:Label><br />
                                                        <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        <asp:Label ID="lblDEUsernameTip2b" runat="server" Text="<%$ Resources:Text, UsernameTips2b %>"
                                                            Visible="False"></asp:Label><br />
                                                        <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        <asp:Label ID="lblDEUsernameTip2c" runat="server" Text="<%$ Resources:Text, UsernameTips2c %>"
                                                            Visible="False" Height="25px"></asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" height="2" />
                                                    <%--<td align="left" valign="top" height="2" />--%>
                                                    <td style="width: 150px; padding-top:5px;" align="left" valign="top">
                                                        <asp:Label ID="lblPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                                            Style="font-weight: bold;"></asp:Label>
                                                    </td>
                                                    <td colspan="2" rowspan="2" align="left" valign="top" style="width: 473px; border-spacing: 0px">
                                                        <table cellpadding="0" cellspacing="0" style="height: 29px; vertical-align: top">
                                                            <tr>
                                                                <td rowspan="2" valign="top" align="left" style="width: auto">
                                                                    <asp:CheckBoxList ID="chkPracticeList" runat="server" Enabled="False" Style="vertical-align: top; text-wrap: none">
                                                                    </asp:CheckBoxList>
                                                                    <asp:Label ID="lblDENoPractice" runat="server" CssClass="tableText" Visible="False"
                                                                        Text="<%$ Resources:Text, NoActivePractice %>"></asp:Label></td>
                                                                <td align="left" valign="top" height="2" />
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Image ID="imgDEPracticeError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                        Visible="False" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td align="left" valign="top" width="10"></td>
                                                    <td style="width: 150px;" align="left" valign="top">
                                                        <asp:Label ID="lblPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                                            Style="font-weight: bold;"></asp:Label>
                                                    </td>
                                                </tr>--%>
                                                <%--
                                                <asp:Panel ID="PanelPrintingOptionSettingAcc" runat="server">
                                                    <tr>
                                                        <td align="left" colspan="1" style="padding-left: 3px" valign="top" width="10">
                                                        </td>
                                                        <td align="left" style="padding-left: 3px;" valign="top" class="tableHeading" colspan="2">
                                                            <asp:Label ID="lblDESystemSettings" runat="server" Text="<%$ Resources:Text, SystemSettings %>"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="1" style="padding-left: 3px" valign="top" width="10">
                                                        </td>
                                                        <td align="left" colspan="2" style="padding-left: 3px">
                                                            <asp:Label ID="lblDEDefaultPrintOption" runat="server" Height="30px" Style="font-weight: bold"
                                                                Text="<%$ Resources:Text, DefaultPrintOption %>"></asp:Label>
                                                            <asp:Image ID="imgDefaultPritingOptionError" runat="server" ImageAlign="AbsMiddle"
                                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="1" style="padding-left: 3px" valign="top" width="10">
                                                        </td>
                                                        <td align="left" colspan="2" style="padding-left: 3px" valign="top" height="40">
                                                            <asp:Label ID="lblDESelectPrintOptionText" runat="server" Height="30px" Text="<%$ Resources:Text, SelectPrintFormOption %>"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="1" valign="top" width="10">
                                                        </td>
                                                        <td align="left" colspan="2" valign="top">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 580px" class="SettingTableStyle">
                                                                <tr>
                                                                    <td class="SettingTableCellStyle">
                                                                        <table cellpadding="0" cellspacing="0" style="width: 580px">
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:RadioButton ID="rbDENotPrint" runat="server" GroupName="DEPrintOption" Height="25px"
                                                                                        Text="<%$ Resources:Text, NotToPrint %>" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 20px">
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="txtDENotPrintRemind" runat="server" Text="<%$ Resources:Text, NotToPrintRemind %>"></asp:Label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="SettingTableCellStyle">
                                                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:RadioButton ID="rbDEPrintFull" runat="server" GroupName="DEPrintOption" Height="25px"
                                                                                        Text="<%$ Resources:Text, PrintFullVersion %>" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 20px">
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="SettingTableCellStyle">
                                                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:RadioButton ID="rbDEPrintCondensed" runat="server" GroupName="DEPrintOption"
                                                                                        Height="25px" Text="<%$ Resources:Text, PrintCondensedVersion %>" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 20px">
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="txtDEPrintCondensedRemind" runat="server" Text="<%$ Resources:Text, PrintCondensedVersionRemind %>"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                                --%>
                                                <tr>
                                                    <td align="left" colspan="1" style="height: 15px" valign="top" width="10"></td>
                                                    <td align="left" colspan="3" style="height: 15px" valign="top"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1" style="padding-left: 3px" valign="top" width="10"></td>
                                                    <td align="left" class="tableHeading" colspan="3" style="padding-left: 3px" valign="top">
                                                        <asp:Label ID="lblDEPasswordSettings" runat="server" Text="<%$ Resources:Text, PasswordSettings %>"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1" valign="top" width="10"></td>
                                                    <td align="left" colspan="3" valign="top">
                                                        <asp:Panel ID="PanelDEWebPasswordSettings" runat="server" Width="585px">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkChgDEPWD" runat="server" Text="<%$ Resources:Text, ChgWebPWD %>"
                                                                            OnCheckedChanged="chkChgDEPWD_CheckedChanged" Enabled="False" onclick="changeDEPWD(this, 'SP');"
                                                                            Height="30px" Style="font-weight: bold" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="PanelDEWebPassword" runat="server" Width="550px">
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td style="width: 20px"></td>
                                                                                    <td style="width: 130px" valign="middle">
                                                                                        <asp:Label ID="lblDENewPWDText" runat="server" Text="<%$ Resources:Text, NewPassword %>"
                                                                                            Height="22px"></asp:Label></td>
                                                                                    <td>
                                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td style="height: 34px">
                                                                                                    <asp:TextBox ID="txtDENewPWD" runat="server" TextMode="Password" BackColor="LightGray"
                                                                                                        MaxLength="20" Width="130px"></asp:TextBox>
                                                                                                    <asp:Image ID="imgDENewPWDError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                                        Visible="False" /></td>
                                                                                                <td>
                                                                                                    <table cellpadding="0" style="width: 210px">
                                                                                                        <tr>
                                                                                                            <td colspan="5">
                                                                                                                <div id="progressBar" style="font-size: 1px; height: 10px; width: 210px; border: 1px solid white;" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="width: 210px">
                                                                                                            <td align="center" style="width: 30%; height: 2px;">
                                                                                                                <span id="strength1"></span>
                                                                                                            </td>
                                                                                                            <td style="width: 5%; height: 2px;">
                                                                                                                <span id="direction1"></span>
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 30%; height: 2px;">
                                                                                                                <span id="strength2"></span>
                                                                                                            </td>
                                                                                                            <td style="width: 5%; height: 2px;">
                                                                                                                <span id="direction2"></span>
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 30%; height: 2px;">
                                                                                                                <span id="strength3"></span>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 20px"></td>
                                                                                    <td style="width: 130px">
                                                                                        <asp:Label ID="lblDEConfirmNewPWDText" runat="server" Text="<%$ Resources:Text, ConfirmPW %>"
                                                                                            Height="30px"></asp:Label></td>
                                                                                    <td valign="top">
                                                                                        <asp:TextBox ID="txtDEConfirmNewPWD" runat="server" TextMode="Password" BackColor="LightGray"
                                                                                            Width="130px" MaxLength="20"></asp:TextBox>
                                                                                        <asp:Image ID="imgDEConfirmNewPWDError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                            Visible="False" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 20px"></td>
                                                                                    <td style="width: 130px"></td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblDEWebPwdTipsText" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"></asp:Label>&nbsp;<br />
                                                                                        <asp:Label ID="lblDEWebPwdTips1Text" runat="server" Text="<%$ Resources:Text, WebPasswordTips1-3Rule %>"></asp:Label>&nbsp;<br />
                                                                                        &nbsp; &nbsp;&nbsp;
                                                                                        <asp:Label ID="lblDEWebPwdTips1aText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label>&nbsp;<br />
                                                                                        &nbsp; &nbsp;&nbsp;
                                                                                        <asp:Label ID="lblDEWebPwdTips1bText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label>&nbsp;<br />
                                                                                        &nbsp; &nbsp;&nbsp;
                                                                                        <asp:Label ID="lblDEWebPwdTips1cText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label>&nbsp;<br />
                                                                                        &nbsp; &nbsp;&nbsp;
                                                                                        <asp:Label ID="lblDEWebPwdTips1dText" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                                                                        <asp:Label ID="lblDEWebPwdTips2Text" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label>
                                                                                        <br />
                                                                                        <asp:Label ID="lblDEWebPwdTips3Text" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1" style="height: 10px" valign="top" width="10"></td>
                                                    <td align="left" colspan="3" style="height: 10px" valign="top"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 3px; height: 16px" valign="middle" width="10"></td>
                                                    <td align="left" class="tableHeading" colspan="3" style="padding-left: 3px; height: 16px"
                                                        valign="middle">
                                                        <asp:Label ID="lblDEAccountStatus" runat="server" Text="<%$ Resources:Text, AccountStatus %>"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 3px" valign="middle" width="10"></td>
                                                    <td align="left" style="width: 150px; padding-left: 3px;" valign="middle">
                                                        <asp:Label ID="lblDEAcctStatusText" runat="server" Text="<%$ Resources:Text, AccountStatus %>"
                                                            Style="font-weight: bold"></asp:Label></td>
                                                    <td colspan="2" align="left" style="height: 30px; width: 473px;" valign="middle">
                                                        <asp:CheckBox ID="chkDESuspend" runat="server" Text="<%$ Resources:Text, Suspended %>"
                                                            Enabled="False" /></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 3px" valign="middle" width="10"></td>
                                                    <td align="left" style="padding-left: 3px; width: 150px" valign="middle">
                                                        <asp:Label ID="lblDEAcctLockedText" runat="server" Text="<%$ Resources:Text, AccountLocked %>"
                                                            Style="font-weight: bold"></asp:Label></td>
                                                    <td colspan="2" align="left" style="height: 30px; width: 473px;" valign="middle">
                                                        <asp:CheckBox ID="chkDEAccountLocked" runat="server" Text="<%$ Resources:Text, Locked %>"
                                                            Enabled="False" /></td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="1" width="10"></td>
                                                    <td align="center" colspan="3">&nbsp;</td>
                                                </tr>
                                            </table>
                                            <asp:ImageButton ID="btnCancelDEAcct" runat="server" ImageUrl="<%$ Resources:ImageURL, CancelBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="btnCancelDEAcct_Click"></asp:ImageButton>&nbsp;
                                            <asp:ImageButton ID="btnAddDEAcct" runat="server" ImageUrl="<%$ Resources:ImageURL, AddBtn %>"
                                                AlternateText="<%$ Resources:ImageURL, AddBtn %>" OnClick="btnAddDEAcct_Click" />&nbsp;
                                            <asp:ImageButton ID="btnEditDEAcct" runat="server" ImageUrl="<%$ Resources:ImageURL, EditBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, EditBtn %>" OnClick="btnEditDEAcct_Click" /><asp:ImageButton
                                                    ID="btnSaveDEAcct" runat="server" ImageUrl="<%$ Resources:ImageURL, SaveBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, SaveBtn %>" OnClick="btnSaveDEAcct_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <%--<cc1:FilteredTextBoxExtender ID="FilteredDEFilter" runat="server" TargetControlID="txtDEFilter"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>--%>

                                <cc1:FilteredTextBoxExtender ID="FilteredDEloginID" runat="server" TargetControlID="txtDEloginID"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredDENewPWD" runat="server" TargetControlID="txtDENewPWD"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="FilteredDEConfirmNewPWD" runat="server" TargetControlID="txtDEConfirmNewPWD"
                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                </cc1:FilteredTextBoxExtender>

                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                    <!-- Join PCD -->
                    <div style="height: 8px; overflow: hidden;">&nbsp;</div>
                    <asp:ImageButton ID="ibtnJoinPCD" runat="server" ImageUrl="<%$ Resources:ImageURL, JoinOrUpdatePCDBtn %>"
                        AlternateText="<%$ Resources:AlternateText, JoinOrUpdatePCDBtn %>" ToolTip="<%$ Resources:AlternateText, JoinOrUpdatePCDBtn %>" />

                </asp:View>
                <asp:View ID="vDataEntry" runat="server">
                    <asp:Panel ID="panDataEntryChgPwd" runat="server" Visible="true">
                        <table cellpadding="0" cellspacing="0" width="950">
                            <tr>
                                <td align="left" style="padding-left: 5px; width: 200px; height: 1px;" valign="top"></td>
                                <td style="width: 300px; height: 1px" valign="top"></td>
                                <td style="height: 1px"></td>
                            </tr>
                            <tr>
                                <td align="left" class="tableHeading" colspan="3" style="padding-left: 5px; height: 1px"
                                    valign="top">
                                    <asp:Label ID="lblDEChangeLoginInformation" runat="server" Text="<%$ Resources:Text, LoginInformation %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="padding-left: 5px; height: 30px;" align="left" valign="top" colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblDEUsernameText" runat="server" Text="<%$ Resources:Text, Username %>"
                                                    Height="30px" Style="font-weight: bold"></asp:Label></td>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblDEUsername" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="height: 30px"></td>
                            </tr>
                            <tr>
                                <td align="left" style="padding-left: 5px; width: 200px;" valign="top"></td>
                                <td style="width: 300px;" valign="top"></td>
                                <td></td>
                            </tr>
                            <asp:Panel ID="PanelDataEntryPrintingOptionSetting" runat="server">
                                <tr>
                                    <td align="left" class="tableHeading" colspan="3" style="padding-left: 5px" valign="top">
                                        <asp:Label ID="lblDEChangeSystemSettings" runat="server" Text="<%$ Resources:Text, SystemSettings %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 16px" valign="top">
                                        <asp:Label ID="lblDEChangeDefaultPrintOption" runat="server" Height="30px" Text="<%$ Resources:Text, DefaultPrintOption %>"
                                            Style="font-weight: bold"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblDEChangeSelectPrintOptionText" runat="server" Text="<%$ Resources:Text, SelectPrintFormOption %>"></asp:Label>
                                        <asp:Image ID="imgDESelectPrintOptionError" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 5px"></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Table ID="tblDEPrintOption" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0" class="SettingTableStyle" Width="935">
                                            <asp:TableRow ID="trDENotToPrint" runat="server">
                                                <asp:TableCell VerticalAlign="top" Style="width: 250px" CssClass="SettingTableCellStyle">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbDEChangeNotPrint" runat="server" GroupName="DEPrintOption"
                                                        Height="30px" Text="<%$ Resources:Text, NotToPrint %>" />
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top" ColumnSpan="2" Width="685px" CssClass="SettingTableCellStyle">
                                                    <asp:Label ID="txtDEChangeNotPrintRemind" runat="server" Text="<%$ Resources:Text, NotToPrintRemind %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="trDEPrintFullVersion" runat="server">
                                                <asp:TableCell VerticalAlign="top" CssClass="SettingTableCellStyle" Style="width: 250px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbDEChangePrintFull" runat="server"
                                                        GroupName="DEPrintOption" Height="30px" Text="<%$ Resources:Text, PrintFullVersion %>" />
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top" ColumnSpan="2" CssClass="SettingTableCellStyle">&nbsp;</asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="trDEPrintCondensed" runat="server">
                                                <asp:TableCell VerticalAlign="top" CssClass="SettingTableCellStyle" Style="width: 250px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbDEChangePrintCondensed" runat="server"
                                                        GroupName="DEPrintOption" Height="30px" Text="<%$ Resources:Text, PrintCondensedVersion %>" />
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top" ColumnSpan="2" CssClass="SettingTableCellStyle">
                                                    <asp:Label ID="txtDEChangePrintCondensedRemind" runat="server" Text="<%$ Resources:Text, PrintCondensedVersionRemind %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td align="left" style="padding-left: 5px; width: 200px; height: 10px" valign="top"></td>
                                <td style="width: 300px; height: 10px" valign="top"></td>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td align="left" class="tableHeading" colspan="3" style="padding-left: 5px" valign="top">
                                    <asp:Label ID="lblDEChangePasswordSetting" runat="server" Text="<%$ Resources:Text, PasswordSettings %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="left" style="height: 40px" valign="top" colspan="3">
                                    <asp:Panel ID="PanelDEChangeWebPasswordSettings" runat="server" Width="930px">
                                        <table border="0" cellpadding="0" cellspacing="0" width="930">
                                            <tr>
                                                <td width="190">
                                                    <asp:CheckBox ID="chkDEChangePassword" runat="server" Text="<%$ Resources:Text, ChgWebPWD %>"
                                                        onclick="changeDEPWD(this, 'DE');" OnCheckedChanged="chkDEChangePassword_CheckedChanged"
                                                        Height="35px" Style="font-weight: bold" /></td>
                                                <td style="width: 355px;" runat="server" id="tdDEChgPwdCell01">&nbsp;
                                                </td>
                                                <td rowspan="2" width="370" valign="top" runat="server" id="tdDEChgPwdCell03">
                                                    <asp:Panel ID="PanelDEChangeWebPasswordTips" runat="server" Width="370px">
                                                        <asp:Label ID="lblDEWebPWDTips" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblDEWebPWDTips1" runat="server" Text="<%$ Resources:Text, WebPasswordTips1-3Rule %>"></asp:Label>
                                                        <br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="lblDEWebPWDTips1a" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label>
                                                        <br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="lblDEWebPWDTips1b" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label>
                                                        <br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="lblDEWebPWDTips1c" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label>
                                                        <br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="lblDEWebPWDTips1d" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblDEWebPWDTips2" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblDEWebPWDTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Panel ID="PanelDEChangeWebPassword01" runat="server" Width="190px">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 20px; height: 10px"></td>
                                                                <td style="width: 170px; height: 10px"></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="20"></td>
                                                                <td>
                                                                    <asp:Label ID="lblDEOLDPasswordText" runat="server" Text="<%$ Resources:Text, OldPassword %>"
                                                                        Height="35px"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="20"></td>
                                                                <td>
                                                                    <asp:Label ID="lblDENEWPasswordText" runat="server" Text="<%$ Resources:Text, NewPassword %>"
                                                                        Height="35px"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="20"></td>
                                                                <td>
                                                                    <asp:Label ID="lblDEConfirmNEWPasswordText" runat="server" Text="<%$ Resources:Text, ConfirmPW %>"
                                                                        Height="35px"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 355px;" runat="server" id="tdDEChgPwdCell02" valign="top">
                                                    <asp:Panel ID="PanelDEChangeWebPassword02" runat="server">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="height: 35px;">
                                                                    <asp:TextBox ID="txtDEOLDPassword" runat="server" MaxLength="20" TextMode="Password"
                                                                        Width="130px" Enabled="False"></asp:TextBox><asp:Image ID="imgDEOLDPasswordError"
                                                                            runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                            Visible="False" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 35px">
                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtDENEWPassword" runat="server" MaxLength="20" TextMode="Password"
                                                                                    Width="130px" Enabled="False"></asp:TextBox><asp:Image ID="imgDENEWPasswordError"
                                                                                        runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                        Visible="False" /></td>
                                                                            <td>
                                                                                <table cellpadding="0" style="width: 210px">
                                                                                    <tr>
                                                                                        <td colspan="5">
                                                                                            <div id="progressBarDE" style="font-size: 1px; height: 10px; width: 210px; border: 1px solid white;" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="width: 210px">
                                                                                        <td align="center" style="width: 30%">
                                                                                            <span id="strength1DE"></span>
                                                                                        </td>
                                                                                        <td style="width: 5%">
                                                                                            <span id="direction1DE"></span>
                                                                                        </td>
                                                                                        <td align="center" style="width: 30%">
                                                                                            <span id="strength2DE"></span>
                                                                                        </td>
                                                                                        <td style="width: 5%">
                                                                                            <span id="direction2DE"></span>
                                                                                        </td>
                                                                                        <td align="center" style="width: 30%">
                                                                                            <span id="strength3DE"></span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 35px;">
                                                                    <asp:TextBox ID="txtDEConfirmNEWPassword" runat="server" MaxLength="20" TextMode="Password"
                                                                        Width="130px" Enabled="False"></asp:TextBox><asp:Image ID="imgDEConfirmNEWPasswordError"
                                                                            runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                            Visible="False" /></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3" style="padding-left: 5px; height: 15px" valign="top"></td>
                            </tr>
                            <tr>
                                <td align="left" style="padding-left: 5px; width: 200px" valign="top"></td>
                                <td align="center" style="width: 300px">
                                    <asp:ImageButton ID="btnDECancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageURL, CancelBtn %>" OnClick="btnDECancel_Click" />
                                    <asp:ImageButton ID="btnDEEdit" runat="server" ImageUrl="<%$ Resources:ImageURL, EditBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, EditBtn %>" OnClick="btnDEEdit_Click" />
                                    <asp:ImageButton ID="btnDESave" runat="server" ImageUrl="<%$ Resources:ImageURL, SaveBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, SaveBtn %>" OnClick="btnSaveDE_Click" /></td>
                                <td rowspan="1" style="height: 20px"></td>
                            </tr>
                        </table>

                        <%--<cc1:FilteredTextBoxExtender ID="FilteredDEOLDPassword" runat="server" TargetControlID="txtDEOLDPassword"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                        </cc1:FilteredTextBoxExtender>--%>
                        <cc1:FilteredTextBoxExtender ID="FilteredDENEWPassword" runat="server" TargetControlID="txtDENEWPassword"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="FilteredDEConfirmNEWPassword" runat="server" TargetControlID="txtDEConfirmNEWPassword"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                        </cc1:FilteredTextBoxExtender>

                    </asp:Panel>
                    <asp:ImageButton ID="btnDataEntryChgPWDReturn" Visible="false" runat="server" ImageUrl="<%$ Resources:ImageURL, ReturnBtn %>"
                        AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"></asp:ImageButton>
                </asp:View>
            </asp:MultiView>
            <%-- Popup for Scheme Name Help --%>
            <asp:Panel ID="panSchemeNameHelp" runat="server" Style="display: none;">
                <asp:Panel ID="panSchemeNameHelpHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSchemeNameHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panSchemeNameHelpContent" runat="server" ScrollBars="vertical" Height="330px">
                                <uc1:SchemeLegend ID="udcSchemeLegend" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="ibtnCloseSchemeNameHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseSchemeNameHelp_Click" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button runat="server" ID="btnHiddenSchemeNameHelp" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupSchemeNameHelp" runat="server" TargetControlID="btnHiddenSchemeNameHelp"
                PopupControlID="panSchemeNameHelp" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panSchemeNameHelpHeading">
            </cc1:ModalPopupExtender>
            <%-- End of Popup for Scheme Name Help --%>



            <%-- Token Poup--%>
            <asp:Panel ID="panTokenPopup" runat="server" Width="400px" Style="display: none;">
                <uc4:ucInputTokenPopUp ID="ucInputTokenPopup" runat="server" TitleText="<%$ Resources:Text, PCDTokenPopupHeader %>" Message="<%$ Resources:Text, PCDTokenPopupMessage %>" />
                <asp:Button ID="btnTokenPopupDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderToken" runat="server" TargetControlID="btnTokenPopupDummy"
                PopupControlID="panTokenPopup" BehaviorID="mdlPopup4" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="none" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of Token Poup--%>

            <%-- Popup for Join PCD --%>

            <asp:Panel ID="panTypeOfPracticePopup" runat="server" Width="950px" Style="display: none">
                <uc2:ucTypeOfPracticePopup ID="ucTypeOfPracticePopup" runat="server" />
                <asp:Button ID="btnTypeOfPracticePopupDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderTypeOfPractice" runat="server" TargetControlID="btnTypeOfPracticePopupDummy"
                PopupControlID="panTypeOfPracticePopup" BehaviorID="mdlPopup1" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of Popup for Join PCD --%>

            <%-- PCD Enrolled Popup--%>
            <asp:Panel ID="panPCDEnrolledPopup" runat="server" Width="600px" Style="display: none;">
                <uc5:ucPCDEnrolledPopup ID="ucPCDEnrolledPopup" runat="server" />
                <asp:Button ID="btnPCDEnrolledPopupDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderPCDEnrolled" runat="server" TargetControlID="btnPCDEnrolledPopupDummy"
                PopupControlID="panPCDEnrolledPopup" BehaviorID="mdlPopup2" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="none" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of PCD Enrolled Popup--%>

            <%-- Notice Popup for account activation--%>
            <asp:Panel ID="panNoticePopup" runat="server" Width="400px" Style="display: none;">
                <uc3:ucNoticePopUp ID="ucNoticePopUp" runat="server" NoticeMode="Custom" ButtonMode="YesNo" IconMode="Question"
                    HeaderText="" MessageText="<%$ Resources:Text, ActivatePCD %>" />
                <asp:Button ID="btnNoticePopupDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="btnNoticePopupDummy"
                PopupControlID="panNoticePopup" BehaviorID="mdlPopup3" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="none" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of Notice Popup for account activation--%>

            <%-- Notice Popup for Return Code Handling--%>
            <asp:Panel ID="panReturnCodeHandlingPopup" runat="server" Width="400px" Style="display: none;">
                <uc6:ucReturnCodeHandlingPopUp ID="ucReturnCodeHandlingPopUp" runat="server" NoticeMode="ExclamationConfirmation" ButtonMode="OK" />
                <asp:Button ID="btnReturnCodeHandlingPopUpDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderReturnCodeHandling" runat="server" TargetControlID="btnReturnCodeHandlingPopUpDummy"
                PopupControlID="panReturnCodeHandlingPopup" BehaviorID="mdlPopup6" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="none" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of Notice Popup for Return Code Handling--%>

            <%-- Notice Popup for logout eHS and login PCD--%>
            <asp:Panel ID="panNoticeLogoutEHSPopUp" runat="server" Style="display: none; width: 540px;">
                <uc3:ucNoticePopUp ID="ucNoticeLogoutEHSPopUp" runat="server" NoticeMode="Custom" ButtonMode="YesNo" IconMode="Question"
                    HeaderText="" MessageText="<%$ Resources:Text, LogoutEHSLoginPCDMessage %>" />
                <asp:Button ID="btnNoticeLogoutEHSPopupDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderNoticeLogoutEHS" runat="server" TargetControlID="btnNoticeLogoutEHSPopupDummy"
                PopupControlID="panNoticeLogoutEHSPopUp" BehaviorID="MyProfile_Popup_NoticeLogoutEHS" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="none" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of Notice Popup for logout eHS and login PCD--%>

            <%-- Consent of Get Username from eHRSS--%>
            <asp:Panel ID="panGetUsernameFromeHRSS" runat="server" Style="display: none;">
                <asp:Panel ID="panGetUsernameFromeHRSSHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, GetUsernameFromEHRSSTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="padding-left:10px; height: 60px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/information.png" /></td>
                                    <td style="height: 60px; text-align: justify">
                                        <asp:CheckBox ID="chkGetUsernameFromEHRSS" runat="server" Text="<%$ Resources: Text, GetUsernameFromEHRSSConsent %>"
                                            Width="380px" class="ChkEHRSSConsent" onclick="changeEHRSSConsent(this);"></asp:CheckBox></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnConsentCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnConsentCancel_Click" />
                                        <asp:ImageButton ID="ibtnConsentConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnConsentConfirm_Click" style="display:none" />
                                        <asp:ImageButton ID="ibtnConsentConfirmDisable" runat="server" 
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmDisableBtn %>" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
                <asp:Button ID="btnGetUsernameFromEHRSSDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmConsent" runat="server" TargetControlID="btnGetUsernameFromEHRSSDummy"
                PopupControlID="panGetUsernameFromeHRSS" BehaviorID="mdlPopup" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panGetUsernameFromeHRSSHeading">
            </cc1:ModalPopupExtender>
            <%-- End of Consent of Get Username from eHRSS--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
