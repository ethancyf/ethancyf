<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="spMaintenance.aspx.vb" Inherits="HCVU.spMaintenance" Title="<%$ Resources:Title, SPMaintenance %>" %>

<%@ Register Src="MOPracticeLists.ascx" TagName="MOPracticeLists" TagPrefix="uc3" %>
<%@ Register Src="spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<%@ Register Src="spPrintFunction.ascx" TagName="spPrintFunction" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/PCDIntegration/ucTypeOfPracticePopup.ascx" TagName="ucTypeOfPracticePopup"
    TagPrefix="uc4" %>
<%@ Register Src="../UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp"
    TagPrefix="uc5" %>
<%@ Register Src="../UIControl/PCDIntegration/ucEnrolmentCopyPopup.ascx" TagName="ucEnrolmentCopyPopup"
    TagPrefix="uc6" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview"
    TagPrefix="uc7" %>
<%@ Register Src="../UIControl/PCDIntegration/ucPCDWarningPopUp.ascx" TagName="ucPCDWarningPopUp"
    TagPrefix="uc8" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        .PrintFunctionCheckboxList input {
            margin-bottom: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript" language="javascript">
        function getKey(SPID) {
            document.getElementById('<%=hfSPID.ClientID%>').value = SPID;
            document.getElementById('<%=btnSpDetails.ClientID %>').click();
            return false;
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, SPMaintenanceBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, SPMaintenanceBanner %>" /><asp:UpdatePanel ID="UpdatePanel1"
            runat="server">
            <ContentTemplate>
                <asp:Panel ID="panExistingSPProfile" runat="server" Style="display: none;">
                    <asp:Panel ID="panExistingSPProfileHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblExistingSPProfileTitle" runat="server" Text="<%$ Resources:Text, ShowSPAmendProfileTitle %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff" align="left">
                                <asp:Panel ID="panExistingSPProfileContent" ScrollBars="Vertical" Height="500px"
                                    runat="server" Width="786px">
                                    <uc1:spSummaryView ID="udcExistingSPProfile" runat="server" />
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                                <asp:ImageButton ID="ibtnExistingSPProfileClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnExistingSPProfileClose_Click" /></td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panDuplicated" runat="server" Style="display: none;">
                    <asp:Panel ID="panDuplicatedHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblDuplicatedTitle" runat="server" Text="<%$ Resources:Text, Duplicationlist %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff" align="left">
                                <asp:Panel ID="panDuplicatedContent" ScrollBars="Vertical" Height="300px" runat="server"
                                    Width="97%">
                                    <uc3:MOPracticeLists ID="MOPracticeLists1" runat="server" />
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                                <asp:ImageButton ID="ibtnDuplicatedClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnDuplicatedClose_Click" /></td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <cc2:InfoMessageBox ID="CompleteMsgBox" runat="server" Width="93%" />
                <cc2:MessageBox ID="msgBox" runat="server" Width="93%" />
                <asp:Panel ID="pnlMaintenance" runat="server">
                    <asp:MultiView ID="MultiViewMaintenance" runat="server" ActiveViewIndex="0">
                        <asp:View ID="ViewSearchCriteria" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblEnrolRefNoText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td style="width: 230px">
                                        <asp:TextBox ID="txtEnrolRefNo" runat="server" MaxLength="17" onChange="Upper(event,this)"></asp:TextBox></td>
                                    <td style="width: 230px">
                                        <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtSPID" runat="server" MaxLength="8"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td style="width: 230px">
                                        <asp:TextBox ID="txtSPHKID" runat="server" MaxLength="11" onChange="formatHKID(this)"></asp:TextBox></td>
                                    <td style="width: 230px">
                                        <asp:Label ID="lblSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInEnglish %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtSPName" runat="server" MaxLength="40" ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>"
                                            onChange="Upper(event,this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSPPhone" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td style="width: 230px">
                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="20"></asp:TextBox></td>
                                    <td style="width: 230px">
                                        <asp:Label ID="lblSPChiNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInChinese %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtSPChiName" runat="server" MaxLength="6" ToolTip=""
                                            onChange=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblScheme" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                    <td style="width: 230px">
                                        <asp:DropDownList ID="ddlScheme" runat="server" AppendDataBoundItems="True" Width="155px">
                                            <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td style="width: 230px">
                                        <asp:Label ID="lblSPHealthProf" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlSPHealthProf" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" style="padding-top: 15px">
                                        <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" /></td>
                                </tr>
                            </table>
                            <cc1:FilteredTextBoxExtender ID="FilteredERN" runat="server" TargetControlID="txtEnrolRefNo"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="-">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredSPID" runat="server" TargetControlID="txtSPID"
                                FilterType="Custom, Numbers">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredSPHKID" runat="server" TargetControlID="txtSPHKID"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="()">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredSPName" runat="server" TargetControlID="txtSPName"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters " ValidChars="'.-, ">
                            </cc1:FilteredTextBoxExtender>
                        </asp:View>
                        <asp:View ID="ViewSearchResult" runat="server">
                            <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                                Height="0px" Width="0px" OnClientClick="return false;" />
                            <uc7:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server"
                                TargetControlID="panSearchCriteriaReview" />
                            <asp:Panel ID="panSearchCriteriaReview" runat="server">
                                <table>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultERN" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultSPHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInEnglish %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultSPName" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultPhone" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSPChiNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInChinese %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultSPChiName" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultScheme" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultHealthProf" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultStatusText" runat="server" Text="<%$ Resources:Text, Status %>"
                                                Visible="false"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultStatus" runat="server" CssClass="tableText" Visible="false"></asp:Label></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                AllowSorting="true" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Enrolment_Ref_No" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnERN" runat="server" Text='<%# Eval("Enrolment_Ref_No") %> '
                                                CommandArgument='<%# Eval("SP_ID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnRSPID" runat="server" CommandArgument='<%# Eval("SP_ID") %>'
                                                Text='<%# Eval("SP_ID") %> '></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SP_HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRSPHKID" runat="server" Text='<%# Eval("SP_HKID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SP_Eng_Name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                            <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>' CssClass="TextGridChi"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Phone_Daytime" HeaderText="<%$ Resources:Text, ContactNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRPhone" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, ServiceProviderStatus %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRSPStatus" runat="server" Text='<%# Eval("Record_Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="200px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%">
                                <tr>
                                    <td align="left">
                                        <asp:ImageButton ID="ibtnSearchResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click" /></td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewDetails" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td valign="top">
                                        <div class="headingText">
                                            <asp:Label ID="lblPersonalParticulars" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
                                        </div>
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="ibtnAmendedRecord" runat="server" OnClick="ibtnAmendedRecord_Click"
                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ShowPendingAmendBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ShowPendingAmendBtn %>" /></td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblERN" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblDetailsSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblDetailsSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblSPUsernameText" runat="server" Text="<%$ Resources:Text, AccAlias %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblSPUsername" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblEffectivateDateText" runat="server" Text="<%$ Resources:Text, EffectiveTime %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblEffectivateDate" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblEname" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblCname" runat="server" CssClass="TextChi"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblAddress" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblEmail" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trPendingEmail" runat="server" visible="false">
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblPendingEmailText" runat="server" Text="<%$ Resources:Text, PendingEmailAddress %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblPendingEmail" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Image ID="imgEditEmail" runat="server" ImageUrl="~/Images/others/small_edit.png"
                                            ToolTip="<%$ Resources:ToolTip, WaitingEmailConfirmation %>" AlternateText="<%$ Resources:AlternateText, WaitingEmailConfirmation %>" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblFax" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblRecordStatusText" runat="server" Text="<%$ Resources:Text, ServiceProviderStatus %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblRecordStatus" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:HiddenField ID="hfRecordStatus" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblAccountStatusText" runat="server" Text="<%$ Resources:Text, AccountStatus %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblAccountStatus" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:HiddenField ID="hfAccountStatus" runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trPCDStatus">
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblPCDStatusText" runat="server" Text="<%$ Resources:Text, PCDStatus %>"></asp:Label></td>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="uplPCDRecordStatus" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:Label ID="lblPCDStatus" runat="server" CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ibtnJoinPCD" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr runat="server" id="trPCDProfessional">
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblPCDProfessionalText" runat="server" Text="<%$ Resources:Text, PCDProfessional %>"></asp:Label></td>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="uplPCDProfessional" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:Label ID="lblPCDProfessional" runat="server" CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ibtnJoinPCD" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblTokenSNText" runat="server" Text="<%$ Resources:Text, TokenSN %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblTokenSN" runat="server" CssClass="tableText"></asp:Label>
                                        <%--<asp:Label ID="lblTokenIssueDate" runat="server" CssClass="tableText"></asp:Label>--%>
                                        <asp:Image ID="imgShareToken" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn_Large %>" />
                                        <asp:Image ID="imgTokenActivateDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClockBtn %>" />
                                        <asp:Label ID="lblTokenRemark" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblTokenReplacedSNText" runat="server" Text="<%$ Resources:Text, ReplacementTokenSerialNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblTokenReplacedSN" runat="server" CssClass="tableText"></asp:Label>
                                        <%--<asp:Label ID="lblTokenReplacedDate" runat="server" CssClass="tableText"></asp:Label>--%>
                                        <asp:Image ID="imgShareTokenReplacement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn_Large %>" />
                                        <asp:Image ID="imgTokenAssignDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClockBtn %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblEnrolledSchemeText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:GridView ID="gvEnrolledScheme" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                                            SkinID="SchemeGridview" Width="95%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblESchemeName" runat="server" Text='<%# Eval("SchemeCode") %>' CssClass="tableText" />
                                                        <asp:HiddenField ID="hfESchemeName" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status [Remarks] (To be controlled code-behind)">
                                                    <ItemStyle VerticalAlign="Top" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblERecordStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                            CssClass="tableText" />
                                                        <asp:Label ID="lblERemark" runat="server" Text='<%# Eval("Remark") %>' Visible="False" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveTime %>">
                                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEEffectiveDtm" runat="server" Text='<%# Eval("EffectiveDtm") %>'
                                                            CssClass="tableText"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingTime %>">
                                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEDelistDtm" runat="server" Text='<%# Eval("DelistDtm") %>' CssClass="tableText"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, LogoReturnDate %>">
                                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblELogoReturnDate" runat="server" Text='<%# Eval("LogoReturnDtm") %>'
                                                            CssClass="tableText" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblTokenReturnText" runat="server" Text="<%$ Resources:Text, TokenReturnDate %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblTokenReturn" runat="server" Text='<%# Eval("TokenReturnDtm") %>'
                                            CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:Panel ID="panSuspend" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSuspendSchemeText" runat="server" Text="<%$ Resources:Text, SuspendScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvSuspendScheme" runat="server" AutoGenerateColumns="False" ShowHeader="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cboSchemeCode" runat="server" Text='<%# Eval("DisplayCode") %>'
                                                                OnCheckedChanged="cboSchemeCode_SuspendScheme_CheckedChanged" AutoPostBack="True" />
                                                            <asp:Image ID="imgAlertSchemeCode" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <asp:HiddenField ID="hfSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Remarks %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemark" runat="server" Enabled="False" />
                                                            <asp:Image ID="imgAlertRemark" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnSuspendSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSuspendSave_Click" />
                                            <asp:ImageButton ID="ibtnSuspendCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnSuspendCancel_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panReactivate" runat="server" Visible="False">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblReactivateSchemeText" runat="server" Text="<%$ Resources:Text, ReactivateScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvReactivateScheme" runat="server" AutoGenerateColumns="False"
                                                ShowHeader="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cboSchemeCode" runat="server" Text='<%# Eval("DisplayCode") %>' />
                                                            <asp:Image ID="imgAlertSchemeCode" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <asp:HiddenField ID="hfSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnReactivateSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnReactivateSave_Click" />
                                            <asp:ImageButton ID="ibtnReactivateCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnReactivateCancel_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panDelisting" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblDelistingSchemeText" runat="server" Text="<%$ Resources:Text, DelistingScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvDelistScheme" runat="server" AutoGenerateColumns="False" ShowHeader="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cboSchemeCode" runat="server" Text='<%# Eval("DisplayCode") %>'
                                                                OnCheckedChanged="cboSchemeCode_DelistScheme_CheckedChanged" AutoPostBack="True" />
                                                            <asp:Image ID="imgAlertSchemeCode" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <asp:HiddenField ID="hfSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingType %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:RadioButtonList ID="rbDelistType" runat="server" RepeatDirection="Horizontal"
                                                                            Enabled="False">
                                                                            <asp:ListItem Value="V">Voluntary</asp:ListItem>
                                                                            <asp:ListItem Value="I">Involuntary</asp:ListItem>
                                                                        </asp:RadioButtonList></td>
                                                                    <td>
                                                                        <asp:Image ID="imgAlertDelistType" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Remarks %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemark" runat="server" Enabled="False" />
                                                            <asp:Image ID="imgAlertRemark" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, LogoReturnDate %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLogoReturn" runat="server" MaxLength="10" Width="75px" Enabled="False"
                                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                                onblur="filterDateInput(this);"></asp:TextBox>
                                                            <asp:ImageButton ID="ibtnLogoReturn" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/button/icon_button/btn_calender.png"
                                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Enabled="False" />
                                                            <asp:Image ID="imgAlertLogoReturn" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <cc1:CalendarExtender ID="CalendarExtLogoReturnDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnLogoReturn"
                                                                TargetControlID="txtLogoReturn" Enabled="True" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                                            <cc1:FilteredTextBoxExtender ID="filtereditLogoReturnDate" runat="server" FilterType="Custom, Numbers"
                                                                TargetControlID="txtLogoReturn" ValidChars="-" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblTokenReturnDateText" runat="server" Text="<%$ Resources:Text, TokenReturnDate %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTokenReturn" runat="server" MaxLength="10" Width="75px" Enabled="False"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnTokenReturn" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/button/icon_button/btn_calender.png"
                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                            <asp:Image ID="imgAlertTokenReturn" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnDelistingSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnDelistingSave_Click" />
                                            <asp:ImageButton ID="ibtnDelistingCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDelistingCancel_Click" /></td>
                                    </tr>
                                </table>
                                <cc1:CalendarExtender ID="calenderExtTokenReturnDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTokenReturn"
                                    TargetControlID="txtTokenReturn" Enabled="True" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                <cc1:FilteredTextBoxExtender ID="filtereditTokenReturnDate" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="txtTokenReturn" ValidChars="-" />
                            </asp:Panel>
                            <asp:Panel ID="panInputDtm" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblInputLogoReturnText" runat="server" Text="<%$ Resources:Text, LogoReturnDate %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvInputDtm" runat="server" AutoGenerateColumns="False" ShowHeader="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("DisplayCode") %>' />
                                                            <asp:HiddenField ID="hfSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, LogoReturnDate %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtInputLogoReturn" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                                            <asp:ImageButton ID="ibtnInputLogoReturn" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/button/icon_button/btn_calender.png"
                                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                                            <asp:Image ID="imgAlertInputLogoReturn" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <cc1:CalendarExtender ID="CalendarExtInputLogoReturn" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnInputLogoReturn"
                                                                TargetControlID="txtInputLogoReturn" Enabled="True" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                                            <cc1:FilteredTextBoxExtender ID="filtereditInputLogoReturn" runat="server" FilterType="Custom, Numbers"
                                                                TargetControlID="txtInputLogoReturn" ValidChars="-" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblInputTokenReturnText" runat="server" Text="<%$ Resources:Text, TokenReturnDate %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInputTokenReturn" runat="server" MaxLength="10" Width="75px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnInputTokenReturn" runat="server" ImageAlign="AbsMiddle"
                                                ImageUrl="~/Images/button/icon_button/btn_calender.png" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                            <asp:Image ID="imgAlertInputTokenReturn" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnEditReturnInfoSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnEditReturnInfoSave_Click" />
                                            <asp:ImageButton ID="ibtnEditReturnInfoCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnEditReturnInfoCancel_Click" /></td>
                                    </tr>
                                </table>
                                <cc1:CalendarExtender ID="calenderExtInputTokenReturn" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnInputTokenReturn"
                                    TargetControlID="txtInputTokenReturn" Enabled="True" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                <cc1:FilteredTextBoxExtender ID="filtereditInputTokenReturn" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="txtInputTokenReturn" ValidChars="-" />
                            </asp:Panel>
                            <asp:Panel ID="panReleaseIVSSClaim" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblReleaseDtm" runat="server" Text="Service Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReleaseDtmFromText" runat="server" Text="From" CssClass="tableText"></asp:Label>
                                            <asp:TextBox ID="txtReleaseDtmFrom" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                            <asp:ImageButton ID="imgReleaseDtmFrom" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/button/icon_button/btn_calender.png"
                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                            <asp:Label ID="lblReleaseDtmToText" runat="server" Text="To" CssClass="tableText"></asp:Label>
                                            <asp:TextBox ID="txtReleaseDtmTo" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                            <asp:ImageButton ID="imgReleaseDtmTo" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/button/icon_button/btn_calender.png"
                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                            <asp:Image ID="imgAlertReleaseDtm" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblCompletedBeforeText" runat="server" Text="Completed before"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCompletedBefore" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnReleaseIVSSClaimSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnReleaseIVSSClaimSave_Click" />
                                            <asp:ImageButton ID="ibtnReleaseIVSSClaimCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnReleaseIVSSClaimCancel_Click" /></td>
                                    </tr>
                                </table>
                                <cc1:CalendarExtender ID="CalendarExtender1" CssClass="ajax_cal" runat="server" PopupButtonID="imgReleaseDtmFrom"
                                    TargetControlID="txtReleaseDtmFrom" Enabled="True" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                <cc1:CalendarExtender ID="CalendarExtender2" CssClass="ajax_cal" runat="server" PopupButtonID="imgReleaseDtmTo"
                                    TargetControlID="txtReleaseDtmTo" Enabled="True" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                <cc1:FilteredTextBoxExtender ID="filtereditReleaseDtmFrom" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="txtReleaseDtmFrom" ValidChars="-" />
                                <cc1:FilteredTextBoxExtender ID="filtereditReleaseDtmTo" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="txtReleaseDtmTo" ValidChars="-" />
                            </asp:Panel>
                            <asp:Panel ID="panActionBtn" runat="server">
                                <br />
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnEditReturnInfo" runat="server" AlternateText="<%$ Resources:AlternateText, EditReturnInfoBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ReturnDtmBtn %>" OnClick="ibtnEditReturnInfo_Click" />
                                            <asp:ImageButton ID="ibtnReactivate" runat="server" AlternateText="<%$ Resources:AlternateText, ReactivateBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ReactivateBtn %>" OnClick="ibtnReactivate_Click" />
                                            <asp:ImageButton ID="ibtnDelisting" runat="server" AlternateText="<%$ Resources:AlternateText, DelistingBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, DelistingBtn %>" OnClick="ibtnDelisting_Click" />
                                            <asp:ImageButton ID="ibtnSuspend" runat="server" AlternateText="<%$ Resources:AlternateText, SuspendBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SuspendBtn %>" OnClick="ibtnSuspend_Click" />
                                            <asp:ImageButton ID="ibtnUnlock" runat="server" AlternateText="<%$ Resources:AlternateText, UnlockBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, UnlockBtn %>" OnClick="ibtnUnlock_Click" />
                                            <asp:ImageButton ID="ibtnCheckToken" runat="server" AlternateText="<%$ Resources:AlternateText, CheckTokenBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CheckTokenBtn %>" OnClick="ibtnCheckToken_Click" />
                                            <asp:ImageButton ID="ibtnReleaseIVSSClaim" runat="server" AlternateText="<%$ Resources:AlternateText, ReleaseIVSSClaimBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ReleaseIVSSClaimBtn %>" OnClick="ibtnReleaseIVSSClaim_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnResendEmail" runat="server" AlternateText="<%$ Resources:AlternateText, ResendEmailBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ResendEmailBtn %>" OnClick="ibtnResendEmail_Click" />
                                            <asp:ImageButton ID="ibtnReprintLetter" runat="server" AlternateText="<%$ Resources:AlternateText, ReprintLetterBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ReprintLetterBtn %>" OnClick="ibtnReprintLetter_Click" />
                                            <asp:ImageButton ID="ibtnJoinPCD" runat="server" ImageUrl='<%$ Resources:ImageUrl, JoinPCDBtn %>'
                                                AlternateText='<%$ Resources:AlternateText, JoinPCDBtn_Short %>' ToolTip='<%$ Resources:AlternateText, JoinPCDBtn_Short %>' />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <div class="headingText">
                                <asp:Label ID="lblMOTitle" runat="server" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"></asp:Label>
                            </div>
                            <asp:Label ID="lblMONA" runat="server" Text="<%$ Resources:Text, N/A %>" CssClass="tableText"
                                Height="40px" Style="padding-left: 50px;" Visible="False"></asp:Label>
                            <asp:GridView ID="gvMO" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblMOIndex" runat="server" Text='<%# Bind("DisplaySeq") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEngName") %>' CssClass="tableText"></asp:Label>
                                                        <asp:ImageButton ID="ibtnDuplicateMO" runat="server" ImageUrl="~/Images/others/info.png"
                                                            AlternateText="<%$ Resources:Text, DuplicateMO %>" Visible='<%# Eval("IsDuplicated") %>'
                                                            OnClick="ibtnDuplicateMO_Click" /><br />
                                                        <asp:Label ID="lblMOCName" runat="server" Text='<%# formatChineseString(Eval("MOChiName")) %>'
                                                            CssClass="TextChi"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMOBRCode" runat="server" Text='<%# Bind("BrCode") %>' CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMOContactNo" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                            CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMOEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMOFax" runat="server" Text='<%# Bind("Fax") %>' CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMOAddress" runat="server" Text='<%# formatAddress(Eval("MOAddress")) %>'
                                                            CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                            CssClass="tableText"></asp:Label>
                                                        <asp:Label ID="lblMORelationRemark" runat="server" Text='<%# formatChineseString(Eval("RelationshipRemark")) %>'
                                                            CssClass="TextChi"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                        <asp:Label ID="lblMOStatusText" runat="server" Text="<%$ Resources:Text, MOStatus %>"></asp:Label></td>
                                                    <td style="width: 650px">
                                                        <asp:Label ID="lblMOStatus" runat="server" Text='<%# Bind("RecordStatus") %>' CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <div class="headingText">
                                <asp:Label ID="lblPracticeBankInfo" runat="server" Text="<%$ Resources:Text, PracticeBankInfo %>"></asp:Label>
                            </div>
                            <asp:GridView ID="gvPracticeBank" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPracticeBankIndex" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            <asp:Image ID="imgPracticeBankIndexAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblPracticeTitle" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-left: 15px">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 190px; background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>" Width="190px"></asp:Label></td>
                                                                <td style="width: 600px">
                                                                    <asp:Label ID="lblPracticeMO" runat="server" Text='<%# Eval("MODisplaySeq") %>' CssClass="tableText" Width="600px"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                        CssClass="tableText"></asp:Label>
                                                                    <asp:ImageButton ID="ibtnDuplicatePractice" runat="server" ImageUrl="~/Images/others/info.png"
                                                                        AlternateText="<%$ Resources:Text, DuplicatePractice %>" Visible='<%# Eval("IsDuplicated") %>'
                                                                        OnClick="ibtnDuplicatePractice_Click" /><br />
                                                                    <asp:Label ID="lblPracticeNameChi" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                                        CssClass="TextChi"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress")) %>'
                                                                        CssClass="tableText"></asp:Label><br />
                                                                    <asp:Label ID="lblPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("PracticeAddress"))) %>'
                                                                        CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticeHealthProf" runat="server" CssClass="tableText" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticeRegCode" runat="server" CssClass="tableText" Text='<%# Eval("Professional.RegistrationCode") %>'></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticePhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticePhone" runat="server" CssClass="tableText" Text='<%# Eval("PhoneDayTime") %>'></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticeStatus" runat="server" CssClass="tableText" Text='<%# Eval("RecordStatus") %>'></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeSchemeText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:GridView ID="gvPracticeSchemeInfo" runat="server" OnRowDataBound="gvPracticeSchemeInfo_RowDataBound"
                                                                        OnPreRender="gvPracticeSchemeInfo_PreRender" AutoGenerateColumns="false" SkinID="SchemeGridview"
                                                                        Width="100%">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="13%" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeCode" runat="server" Text='<%# Eval("SchemeDisplayCode") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                    <asp:HiddenField ID="hfPracticeSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                                                    <asp:HiddenField ID="hfGIsCategoryHeader" runat="server" Value='<%# Eval("IsCategoryHeader") %>' />
                                                                                    <asp:HiddenField ID="hfGCategoryName" runat="server" Value='<%# Eval("CategoryName") %>' />
                                                                                    <asp:HiddenField ID="hfGAllNotProvideService" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, SubsidyAndServiceFee %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="10%" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSubsidizeCode" runat="server" Text='<%# Eval("SubsidizeDisplayCode") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                    <asp:HiddenField ID="hfPracticeSubsidizeCodeReal" runat="server" Value='<%# Eval("SubsidizeCode") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemStyle VerticalAlign="Top" Width="23%" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeServiceFee" runat="server" CssClass="tableText">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Resources: Text, Status (Controlled in CodeBehind)">
                                                                                <ItemStyle VerticalAlign="Top" Width="28%" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeStatus" runat="server" CssClass="tableText">
                                                                                    </asp:Label>
                                                                                    <asp:HiddenField ID="hfPracticeSchemeStatusReal" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveTime %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="13%" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeEffectiveDtm" runat="server" CssClass="tableText"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingTime %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="13%" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeDelistDtm" runat="server" CssClass="tableText"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <hr style="width: 100%; color: #ff8080; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblBankTitle" runat="server" Text="<%$ Resources:Text, Bank %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-left: 15px">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 190px; background-color: #f7f7de;">
                                                                    <asp:Label ID="lblBankNameText" runat="server" Width="190px" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                                <td style="width: 600px;">
                                                                    <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankAcct.BankName") %>'
                                                                        CssClass="TextChi" Width="600px"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;">
                                                                    <asp:Label ID="lblBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BankAcct.BranchName") %>'
                                                                        CssClass="TextChi"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;">
                                                                    <asp:Label ID="lblBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblBankAcc" runat="server" Text='<%# Eval("BankAcct.BankAcctNo") %>'
                                                                        CssClass="tableText"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de; vertical-align: top">
                                                                    <asp:Label ID="lblBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblBankOwner" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                                        CssClass="tableText" Width="600px" Style="word-wrap: break-word"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnSPracticeDelisting" runat="server" AlternateText="<%$ Resources:AlternateText, DelistingBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, DelistingSBtn %>" OnClick="ibtnSPracticeDelisting_Click" />
                                            <asp:ImageButton ID="ibtnSPracticeSuspend" runat="server" AlternateText="<%$ Resources:AlternateText, SuspendBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SuspendSBtn %>" OnClick="ibtnSPracticeSuspend_Click" />
                                            <asp:ImageButton ID="ibtnSPracticeReactivate" runat="server" AlternateText="<%$ Resources:AlternateText, ReactivateBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ReactiveSBtn %>" OnClick="ibtnSPracticeReactivate_Click" />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibtnDetailsBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnDetailsBack_Click" /></td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewSPActionDetails" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionERN" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionDetailsSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionDetailsSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionSPUsernameText" runat="server" Text="<%$ Resources:Text, AccAlias %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionSPUsername" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionEffectivateDateText" runat="server" Text="<%$ Resources:Text, EffectiveTime %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionEffectivateDate" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionEname" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblActionCname" runat="server" CssClass="TextChi"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionAddress" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionEmail" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr id="trActionPendingEmail" runat="server" visible="false">
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionPendingEmailText" runat="server" Text="<%$ Resources:Text, PendingEmailAddress %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionPendingEmail" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Image ID="imgActionEditEmail" runat="server" ImageUrl="~/Images/others/small_edit.png"
                                            ToolTip="<%$ Resources:ToolTip, WaitingEmailConfirmation %>" AlternateText="<%$ Resources:AlternateText, WaitingEmailConfirmation %>" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionFax" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionRecordStatusText" runat="server" Text="<%$ Resources:Text, ServiceProviderStatus %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblActionRecordStatus" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionAccountStatusText" runat="server" Text="<%$ Resources:Text, AccountStatus %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblActionAccountStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionTokenSNText" runat="server" Text="<%$ Resources:Text, TokenSN %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblActionTokenSN" runat="server" CssClass="tableText"></asp:Label>
                                        <%--<asp:Label ID="lblActionTokenIssueDate" runat="server" CssClass="tableText"></asp:Label>--%>
                                        <asp:Image ID="imgActionShareToken" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn_Large %>" />
                                        <asp:Image ID="imgActionTokenActivateDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClockBtn %>" />
                                        <asp:Label ID="lblActionTokenRemark" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px" valign="top">
                                        <asp:Label ID="lblActionTokenReplacedSNText" runat="server" Text="<%$ Resources:Text, ReplacementTokenSerialNo %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblActionTokenReplacedSN" runat="server" CssClass="tableText"></asp:Label>
                                        <%--<asp:Label ID="lblActionTokenReplacedDate" runat="server" CssClass="tableText"></asp:Label>--%>
                                        <asp:Image ID="ImgActionShareTokenReplacement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn_Large %>" />
                                        <asp:Image ID="ImgActionTokenAssignDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClockBtn %>" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panActionInputDtm" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionInputLogoReturnText" runat="server" Text="<%$ Resources:Text, LogoReturnDate %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvActionInputLogoReturn" runat="server" SkinID="SchemeGridview">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionInputTokenReturnText" runat="server" Text="<%$ Resources:Text, TokenReturnDate %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblActionInputTokenReturn" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 200px; padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionInputDtmBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionInputDtmBack_Click" /></td>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionInputDtmConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionInputDtmConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panActionSuspend" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionSuspendSchemeText" runat="server" Text="<%$ Resources:Text, SuspendScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvActionSuspendScheme" runat="server" SkinID="SchemeGridview">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 200px; padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionSuspendBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionSuspendBack_Click" /></td>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionSuspendConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionSuspendConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panActionDelisting" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionDelistingSchemeText" runat="server" Text="<%$ Resources:Text, DelistingScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvActionDelistScheme" runat="server" SkinID="SchemeGridview">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionTokenReturnDateText" runat="server" Text="<%$ Resources:Text, TokenReturnDate %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblActionTokenReturnDate" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 200px; padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionDelistingBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionDelistingBack_Click" /></td>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionDelistingConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionDelistingConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panActionReactivate" runat="server" Visible="false">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionReactivateSchemeText" runat="server" Text="<%$ Resources:Text, ReactivateScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblActionReactivateScheme" runat="server" CssClass="tableText"></asp:Label>
                                            <asp:HiddenField ID="hfActionReactivateScheme" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 200px; padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionReactivateBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionReactivateBack_Click" /></td>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnActionReactivateConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionReactivateConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panActionReleaseIVSSClaim" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionServiceDateText" runat="server" Text="Service Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblActionServiceDateFromText" runat="server" Text="From"></asp:Label>
                                            <asp:Label ID="lblActionServiceDateFrom" runat="server" CssClass="tableText"></asp:Label>
                                            <asp:Label ID="lblActionServiceDateToText" runat="server" Text="To"></asp:Label>
                                            <asp:Label ID="lblActionServiceDateTo" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionCompleteDateBeforeText" runat="server" Text="Completed Before"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblActionCompleteDateBefore" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 200px">
                                            <asp:ImageButton ID="ibtnActionReleaseIVSSClaimBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionReleaseIVSSClaimBack_Click" /></td>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnActionReleaseIVSSClaimConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionReleaseIVSSClaimConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="ViewPracticeActionDetails" runat="server">
                            <div class="headingText">
                                <asp:Label ID="lblActionPracticeHeading" runat="server" Text="[To be controlled in CodeBehind]"></asp:Label>
                            </div>
                            <asp:GridView ID="gvActionPracticeBank" runat="server" AutoGenerateColumns="False"
                                ShowHeader="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblActionPracticeBankIndex" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="15px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblActionPracticeTitle" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-left: 15px">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 25%; background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblActionPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionPracticeMO" runat="server" Text='<%# Eval("MODisplaySeq") %>'
                                                                        CssClass="tableText"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblActionPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                        CssClass="tableText"></asp:Label><br />
                                                                    <asp:Label ID="lblPracticeNameChi" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                                        CssClass="TextChi"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblActionPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress")) %>'
                                                                        CssClass="tableText"></asp:Label><br />
                                                                    <asp:Label ID="lblActionPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("PracticeAddress"))) %>'
                                                                        CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblActionPracticeHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionPracticeHealthProf" runat="server" CssClass="tableText" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblActionPracticeRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionPracticeRegCode" runat="server" CssClass="tableText" Text='<%# Eval("Professional.RegistrationCode") %>'></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticePhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticePhone" runat="server" CssClass="tableText" Text='<%# Eval("PhoneDayTime") %>'></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblPracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPracticeStatus" runat="server" CssClass="tableText" Text='<%# Eval("RecordStatus") %>'></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;" valign="top">
                                                                    <asp:Label ID="lblActionPracticeSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:GridView runat="server" ID="gvActionPracticeSchemeInfo" OnRowDataBound="gvActionPracticeSchemeInfo_RowDataBound"
                                                                        OnPreRender="gvActionPracticeSchemeInfo_PreRender" AutoGenerateColumns="false"
                                                                        SkinID="SchemeGridview" Width="85%">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="80px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeCode" runat="server" Text='<%# Eval("SchemeDisplayCode") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                    <asp:HiddenField ID="hfPracticeSchemeCode" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                                                    <asp:HiddenField ID="hfGIsCategoryHeader" runat="server" Value='<%# Eval("IsCategoryHeader") %>' />
                                                                                    <asp:HiddenField ID="hfGCategoryName" runat="server" Value='<%# Eval("CategoryName") %>' />
                                                                                    <asp:HiddenField ID="hfGAllNotProvideService" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, SubsidyAndServiceFee %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="80px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSubsidizeCode" runat="server" Text='<%# Eval("SubsidizeDisplayCode") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                    <asp:HiddenField ID="hfPracticeSubsidizeCode" runat="server" Value='<%# Eval("SubsidizeCode") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemStyle VerticalAlign="Top" Width="120px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeServiceFee" runat="server" CssClass="tableText">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Resources:Text, Status (To be controlled CodeBehind)">
                                                                                <ItemStyle VerticalAlign="Top" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeStatus" runat="server" CssClass="tableText">
                                                                                    </asp:Label>
                                                                                    <asp:HiddenField ID="hfPracticeSchemeStatusReal" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveTime %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeEffectiveDtm" runat="server" CssClass="tableText"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingTime %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPracticeSchemeDelistDtm" runat="server" CssClass="tableText"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <hr style="width: 100%; color: #ff8080; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblActionBankTitle" runat="server" Text="<%$ Resources:Text, Bank %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-left: 15px">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 25%; background-color: #f7f7de;">
                                                                    <asp:Label ID="lblActionBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionBankName" runat="server" Text='<%# Eval("BankAcct.BankName") %>'
                                                                        CssClass="TextChi"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;">
                                                                    <asp:Label ID="lblActionBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionBranchName" runat="server" Text='<%# Eval("BankAcct.BranchName") %>'
                                                                        CssClass="TextChi"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;">
                                                                    <asp:Label ID="lblActionBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionBankAcc" runat="server" Text='<%# Eval("BankAcct.BankAcctNo") %>'
                                                                        CssClass="tableText"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="background-color: #f7f7de;">
                                                                    <asp:Label ID="lblActionBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblActionBankOwner" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                                        CssClass="tableText"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Panel ID="panPracticeReactivate" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblReactivatePracticeSchemeText" runat="server" Text="<%$ Resources:Text, ReactivateScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvReactivatePracticeScheme" runat="server" AutoGenerateColumns="False"
                                                ShowHeader="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cboSchemeCode" runat="server" Text='<%# Eval("DisplayCode") %>' />
                                                            <asp:Image ID="imgAlertSchemeCode" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <asp:HiddenField ID="hfSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="imgReactivatePracticeSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnPracticeReactivateSave_Click" />
                                            <asp:ImageButton ID="imgReactivatePracticeCancal" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPracticeReactivateCancel_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panPracticeSuspend" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSuspendPracticeText" runat="server" Text="<%$ Resources:Text, SuspendScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvSuspendPracticeScheme" runat="server" AutoGenerateColumns="False"
                                                ShowHeader="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cboSchemeCode" runat="server" Text='<%# Eval("DisplayCode") %>'
                                                                OnCheckedChanged="cboSchemeCode_SuspendScheme_CheckedChanged" AutoPostBack="True" />
                                                            <asp:Image ID="imgAlertSchemeCode" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <asp:HiddenField ID="hfSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Remarks %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemark" runat="server" Enabled="False" />
                                                            <asp:Image ID="imgAlertRemark" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center" style="padding-top: 10px">
                                            <asp:ImageButton ID="ibtnPracticeSuspendSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnPracticeSuspendSave_Click" />
                                            <asp:ImageButton ID="ibtnPracticeSuspendCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPracticeSuspendCancel_Click" />
                                            <asp:Button ID="btnSuspendHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                                                Height="0px" Width="0px" OnClientClick="return false;" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panPracticeDelisting" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblDelistingPracticeSchemeText" runat="server" Text="<%$ Resources:Text, DelistingScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvDelistPracticeScheme" runat="server" AutoGenerateColumns="False"
                                                ShowHeader="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cboSchemeCode" runat="server" Text='<%# Eval("DisplayCode") %>'
                                                                OnCheckedChanged="cboSchemeCode_DelistScheme_CheckedChanged" AutoPostBack="True" />
                                                            <asp:Image ID="imgAlertSchemeCode" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            <asp:HiddenField ID="hfSchemeCodeReal" runat="server" Value='<%# Eval("SchemeCode") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingType %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:RadioButtonList ID="rbDelistType" runat="server" RepeatDirection="Horizontal"
                                                                            Enabled="False">
                                                                            <asp:ListItem Value="V">Voluntary</asp:ListItem>
                                                                            <asp:ListItem Value="I">Involuntary</asp:ListItem>
                                                                        </asp:RadioButtonList></td>
                                                                    <td>
                                                                        <asp:Image ID="imgAlertDelistType" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Remarks %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemark" runat="server" Enabled="False" />
                                                            <asp:Image ID="imgAlertRemark" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnPracticeDelistingSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnPracticeDelistingSave_Click" />
                                            <asp:ImageButton ID="ibtnPracticeDelistingCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPracticeDelistingCancel_Click" />
                                            <asp:Button ID="btnDelistingHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                                                Height="0px" Width="0px" OnClientClick="return false;" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panActionPracticeSuspend" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 200px">
                                            <asp:Label ID="lblActionPracticeSuspendSchemeText" runat="server" Text="<%$ Resources:Text, SuspendScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvActionPSuspendScheme" runat="server">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 70%">
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:ImageButton ID="ibtnActionPracitceSuspendBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionPracitceSuspendBack_Click" /></td>
                                        <td align="right">
                                            <asp:ImageButton ID="ibtnActionPracitceSuspendConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionPracitceSuspendConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panActionPracticeDelisting" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 200px">
                                            <asp:Label ID="lblActionPracticeDelistingSchemeText" runat="server" Text="<%$ Resources:Text, DelistingScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:GridView ID="gvActionPDelistScheme" runat="server">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                </table>
                                <table style="width: 70%">
                                    <tr>
                                        <td align="left">
                                            <asp:ImageButton ID="ibtnActionPracticeDelistingBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionPracticeDelistingBack_Click" /></td>
                                        <td align="right">
                                            <asp:ImageButton ID="ibtnActionPracticeDelistingConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionPracticeDelistingConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panActionPracticeReactivate" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblActionPReactivateSchemeText" runat="server" Text="<%$ Resources:Text, ReactivateScheme %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblActionPReactivateScheme" runat="server" CssClass="tableText"></asp:Label>
                                            <asp:HiddenField ID="hfActionPReactivateScheme" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                </table>
                                </table>
                                <table style="width: 70%">
                                    <tr>
                                        <td align="left">
                                            <asp:ImageButton ID="ibtnActionPracticeReactivateBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnActionPracticeReactivateBack_Click" /></td>
                                        <td align="right">
                                            <asp:ImageButton ID="ibtnActionPracticeReactivateConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnActionPracticeReactivateConfirm_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="ViewComplete" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibtnCompleteBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewError" runat="server">
                            <asp:ImageButton ID="ibtnErrorBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnErrorBack_Click" />
                        </asp:View>
                    </asp:MultiView>
                </asp:Panel>
                <asp:HiddenField ID="hfSPID" runat="server" />
                <asp:HiddenField ID="hfERN" runat="server" />
                <asp:HiddenField ID="hfUnderModify" runat="server" />
                <asp:Button ID="btnSpDetails" runat="server" Text="" Style="display: none" OnClick="btnSpDetails_Click" />
                <asp:Button runat="server" ID="btnHiddenShowExistingSPProfile" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtenderSPProfile" runat="server" TargetControlID="btnHiddenShowExistingSPProfile"
                    PopupControlID="panExistingSPProfile" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panExistingSPProfileHeading">
                </cc1:ModalPopupExtender>
                <%-- Popup for Resend Email --%>
                <asp:Panel ID="panResendEmail" runat="server" Style="display: none;">
                    <asp:Panel ID="panResendEmailHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblResendEmailHeading" runat="server" Text="<%$ Resources:Text, ResendEmail %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff; text-align: center">
                                <table>
                                    <tr>
                                        <td style="text-align: left">
                                            <div class="headingText">
                                                <asp:Label ID="lblResendEmailHeadingText" runat="server" Text="<%$ Resources:Text, SelectionOfEmail %>"></asp:Label>
                                            </div>
                                            <asp:RadioButtonList ID="rblResendEmail" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblResendEmail_SelectedIndexChange">
                                                <asp:ListItem Text="<%$ Resources:Text, ActivationEmail %>" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Text, SchemeEnrolmentEmail %>" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Text, ConfirmationEmail %>" Value="C"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Text, DelistEmail %>" Value="D"></asp:ListItem>
                                            </asp:RadioButtonList>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px"></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnResendEmailSend" runat="server" AlternateText="<%$ Resources:AlternateText, SendBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SendBtn %>" OnClick="ibtnResendEmailSend_Click" />
                                            <asp:ImageButton ID="ibtnResendEmailCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnResendEmailCancel_Click" />
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
                </asp:Panel>
                <asp:Button ID="btnHiddenResendEmail" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="popupResendEmail" runat="server" TargetControlID="btnHiddenResendEmail"
                    PopupControlID="panResendEmail" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panResendEmailHeading">
                </cc1:ModalPopupExtender>
                <%-- End of Popup for Resend Email --%>
                <%-- Popup for Reprint Letter --%>
                <asp:Panel ID="panReprintLetter" runat="server" Style="display: none;">
                    <asp:Panel ID="panReprintLetterHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblReprintLetterHeading" runat="server" Text="<%$ Resources:Text, ReprintLetter %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff; text-align: center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td style="text-align: left">
                                            <div class="headingText">
                                                <asp:Label ID="lblReprintLetterHeadingText" runat="server" Text="<%$ Resources:Text, SelectionOfLetter %>"></asp:Label>
                                            </div>
                                            <asp:RadioButtonList ID="rblReprintLetter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblReprintLetter_SelectedIndexChanged">
                                                <asp:ListItem Text="<%$ Resources:Text, AcknowledgementLetter %>" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Text, SchemeEnrolmentLetter %>" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Text, TokenReplacementLetter %>" Value="T"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px"></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnReprintLetterPrint" runat="server" AlternateText="<%$ Resources:AlternateText, PrintBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, PrintBtn %>" OnClick="ibtnReprintLetterPrint_Click" />
                                            <asp:ImageButton ID="ibtnReprintLetterCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnReprintLetterCancel_Click" />
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
                </asp:Panel>
                <asp:Button ID="btnHiddenReprintLetter" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="popupReprintLetter" runat="server" TargetControlID="btnHiddenReprintLetter"
                    PopupControlID="panReprintLetter" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panReprintLetterHeading">
                </cc1:ModalPopupExtender>
                <%-- End of Popup for Reprint Letter --%>
                <%-- Popup for common control - spPrintFunction --%>
                <asp:Panel ID="panPrintFunction" runat="server" Style="display: none;">
                    <asp:Panel ID="panPrintFunctionHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 650px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblPrintFunction" runat="server"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 650px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff; text-align: center">
                                <table>
                                    <tr>
                                        <td style="text-align: left">
                                            <uc2:spPrintFunction ID="udcPrintFunction" runat="server" />
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
                </asp:Panel>
                <asp:Button ID="btnHiddenPrintFunction" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="popupPrintFunction" runat="server" TargetControlID="btnHiddenPrintFunction"
                    PopupControlID="panPrintFunction" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panPrintFunctionHeading">
                </cc1:ModalPopupExtender>
                <%-- End of Popup for common control - spPrintFunction --%>
                <%-- Popup for Check Token --%>
                <asp:Panel ID="panCheckToken" runat="server" Style="display: none;">
                    <asp:Panel ID="panCheckTokenHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 520px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblCheckTokenHeading" runat="server" Text="[CodeBehind]"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 520px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff; text-align: center; padding: 12px">
                                <cc2:InfoMessageBox ID="udcCTInfoMessageBox" runat="server" Width="100%" />
                                <cc2:MessageBox ID="udcCTMessageBox" runat="server" Width="100%" />
                                <asp:MultiView ID="mvCT" runat="server">
                                    <asp:View ID="vCTC" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <tr>
                                                            <td style="width: 150px; text-align: left">
                                                                <asp:Label ID="lblCTSPIDText" runat="server" Text="<%$ Resources: Text, ServiceProviderID %>"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:Label ID="lblCTSPID" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left">
                                                                <asp:Label ID="lblCTTokenSerialNoText" runat="server" Text="<%$ Resources: Text, TokenSerialNo %>"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:Label ID="lblCTSerialNo" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left">
                                                                <asp:Label ID="lblCTResultText" runat="server" Text="<%$ Resources: Text, Result %>"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:Label ID="lblCTResult" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ibtnCheckTokenRepair" runat="server" AlternateText="<%$ Resources: AlternateText, RepairTokenBtn %>"
                                                        ImageUrl="<%$ Resources: ImageUrl, RepairTokenBtn %>" OnClick="ibtnCheckTokenRepair_Click" />
                                                    <asp:ImageButton ID="ibtnCheckTokenClose" runat="server" AlternateText="<%$ Resources: AlternateText, CloseBtn %>"
                                                        ImageUrl="<%$ Resources: ImageUrl, CloseBtn %>" OnClick="ibtnCheckTokenClose_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vCTR" runat="server">
                                        <asp:Panel ID="panCTR" runat="server" DefaultButton="ibtnCTRNext">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: left" colspan="2">
                                                        <asp:Label ID="lblCTRMessage" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top">
                                                        <table>
                                                            <tr style="height: 30px">
                                                                <td style="vertical-align: top; text-align: left; width: 170px">
                                                                    <asp:Label ID="lblCTRServiceProviderIDText" runat="server" Text="<%$ Resources: Text, ServiceProviderID %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top; text-align: left">
                                                                    <asp:Label ID="lblCTRServiceProviderID" runat="server" CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 30px">
                                                                <td style="vertical-align: top; text-align: left">
                                                                    <asp:Label ID="lblCTRTokenSerialNoText" runat="server" Text="<%$ Resources: Text, TokenSerialNo %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top; text-align: left">
                                                                    <asp:Label ID="lblCTRTokenSerialNo" runat="server" CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 30px">
                                                                <td style="vertical-align: top; text-align: left">
                                                                    <asp:Label ID="lblCTRTokenPasscodeText" runat="server" Text="<%$ Resources: Text, FirstTokenPasscode %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top; text-align: left">
                                                                    <asp:TextBox ID="txtCTRTokenPasscode" runat="server" Width="70px" MaxLength="6" TextMode="Password"></asp:TextBox>
                                                                    <asp:Image ID="imgCTRTokenPasscode" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                                                    <asp:Image ID="imgCTRTokenPasscodeOK" runat="server" ImageUrl="<%$ Resources: ImageUrl, ImportSuccess %>"
                                                                        AlternateText="<%$ Resources: AlternateText, ImportSuccess %>" />
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 30px">
                                                                <td style="vertical-align: top; text-align: left">
                                                                    <asp:Label ID="lblCTRNextPasscodeText" runat="server" Text="<%$ Resources: Text, SecondTokenPasscode %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top; text-align: left">
                                                                    <asp:TextBox ID="txtCTRNextPasscode" runat="server" Width="70px" MaxLength="6" TextMode="Password"></asp:TextBox>
                                                                    <asp:Image ID="imgCTRNextPasscode" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:ImageButton ID="ibtnCTRNext" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnCTRNext_Click" />
                                                        <asp:ImageButton ID="ibtnCTRCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnCTRCancel_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </asp:View>
                                </asp:MultiView>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Button ID="btnHiddenCheckToken" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="popupCheckToken" runat="server" TargetControlID="btnHiddenCheckToken"
                    PopupControlID="panCheckToken" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panCheckTokenHeading">
                </cc1:ModalPopupExtender>
                <%-- End of Popup for Check Token --%>
                <asp:HiddenField ID="hfPopupStatus" runat="server" />
                <cc1:ModalPopupExtender ID="ModalPopupExtenderDuplicated" runat="server" TargetControlID="btnHiddenDuplicated"
                    PopupControlID="panDuplicated" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDuplicatedHeading">
                </cc1:ModalPopupExtender>
                <asp:Button runat="server" ID="btnHiddenDuplicated" Style="display: none" />
                <%-- Popup for Join PCD --%>
                <asp:Panel ID="panTypeOfPracticePopup" runat="server" Width="900px" Style="display: none;">
                    <uc4:ucTypeOfPracticePopup ID="ucTypeOfPracticePopup" runat="server" />
                    <asp:Button ID="btnTypeOfPracticePopupDummy" runat="server" Style="display: none" />
                </asp:Panel>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderTypeOfPractice" runat="server" TargetControlID="btnTypeOfPracticePopupDummy"
                    PopupControlID="panTypeOfPracticePopup" BehaviorID="mdlPopup1" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                </cc1:ModalPopupExtender>
                <%-- End of Popup for Join PCD --%>
                <%-- Popup for successful submission of PCD information --%>
                <asp:Panel ID="panNoticePopup" runat="server" Width="500px" Height="100px" Style="display: none">
                    <uc5:ucNoticePopUp ID="ucNoticePopup" runat="server" ButtonMode="OK" NoticeMode="Notification" />
                    <asp:Button ID="btnNoticePopupDummy" runat="server" Style="display: none" />
                </asp:Panel>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="btnNoticePopupDummy"
                    PopupControlID="panNoticePopup" BehaviorID="mdlPopup2" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                </cc1:ModalPopupExtender>
                <%-- End of successful submission of PCD information --%>
                <%-- Popup for Enrolment Copy --%>
                <asp:Panel ID="panEnrolmentCopyPopup" runat="server" Width="900px" Style="display: none">
                    <uc6:ucEnrolmentCopyPopup ID="ucEnrolmentCopyPopup" runat="server" />
                    <asp:Button ID="btnEnrolmentCopyPopup" runat="server" Style="display: none" />
                </asp:Panel>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderEnrolmentCopy" runat="server" TargetControlID="btnEnrolmentCopyPopup"
                    PopupControlID="panEnrolmentCopyPopup" BehaviorID="mdlPopup3" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                </cc1:ModalPopupExtender>
                <%-- End of Popup for Enrolment Copy --%>
                <%-- Popup for PCDAccountStatusCheck --%>
                <asp:Panel ID="panPCDWarningPopup" runat="server" Width="580px" Height="100px" Style="display: none">
                    <uc8:ucPCDWarningPopup ID="ucPCDWarningPopup" runat="server" ShowCheckbox="False"/>
                    <asp:Button ID="btnPCDWarningPopupDummy" runat="server" Style="display: none" />
                </asp:Panel>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderPCDWarning" runat="server" TargetControlID="btnPCDWarningPopupDummy"
                    PopupControlID="panPCDWarningPopup" BehaviorID="mdlPopup8" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                </cc1:ModalPopupExtender>
                <%-- End of Popup for PCDAccountStatusCheck --%>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
