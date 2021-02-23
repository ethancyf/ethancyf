<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="spTokenManagement.aspx.vb" Inherits="HCVU.spTokenManagement" Title="<%$ Resources:Title, SPTokenSchemeManagement %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="spPrintFunction.ascx" TagName="spPrintFunction" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/PCDIntegration/ucTypeOfPracticePopup.ascx" TagName="ucTypeOfPracticePopup" TagPrefix="uc2" %>
<%@ Register Src="../UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/PCDIntegration/ucEnrolmentCopyPopup.ascx" TagName="ucEnrolmentCopyPopup" TagPrefix="uc4" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc5" %>
<%@ Register Src="../UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc6" %>
<%@ Register Src="~/UIControl/PCDIntegration/ucPCDWarningPopUp.ascx" TagName="ucPCDWarningPopUp" TagPrefix="uc7" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        .PrintFunctionCheckboxList input {
            margin-bottom: 5px;
        }

        Table.TableStyle1 {
            width: 100%;
        }

            Table.TableStyle1 td {
                vertical-align: top;
            }

        tr.TrStyle1 {
            height: 24px;
        }

        td.TdStyle1 {
            width: 175px;
        }

        .ChkEHRSSConsent {
            padding-left: 20px;
            text-indent: -20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <script type="text/javascript">
        function InitJQuery() {
            $(document).ready(function () {

                $("[component='TokenSerialNo']").bind('input propertychange', function () {
                    if ($(this).val() == '') {
                        $("[component='TokenIssuedBy']").find("input:radio").attr('checked', false);
                        $("[component='IsShareToken']").find("input:radio").attr('checked', false);
                    } else {
                        $("[component='TokenIssuedBy']").find("input:radio[value=EHS]").attr('checked', 'checked');
                        $("[component='IsShareToken']").find("input:radio[value=N]").attr('checked', 'checked');
                    }
                });

                $("[component='GetFromEHRSS']").click(function () {
                    $("[component='GetFromEHRSSConsent']").find(":checkbox").attr('checked', false);
                    $("[component='GetFromEHRSSConfirm']").hide();
                    $("[component='GetFromEHRSSConfirmDisable']").show();
                });

                $("[component='GetFromEHRSSConsent']").find(":checkbox").change(function () {
                    if ($(this).is(":checked")) {
                        $("[component='GetFromEHRSSConfirm']").show();
                        $("[component='GetFromEHRSSConfirmDisable']").hide();
                    } else {
                        $("[component='GetFromEHRSSConfirm']").hide();
                        $("[component='GetFromEHRSSConfirmDisable']").show();
                    }
                });

            });

        }

        Sys.Application.add_load(InitJQuery);
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, TokenSchemeManagementBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, SPTokenSchemeManagementBanner %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 60px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/information.png" /></td>
                                    <td style="height: 60px; text-align: justify">
                                        <asp:CheckBox ID="chkGetFromEHRSS" runat="server" Font-Bold="True" Text="<%$ Resources: Text, GetFromPPIEPRConsent %>"
                                            Width="500px" class="ChkEHRSSConsent" component="GetFromEHRSSConsent"></asp:CheckBox></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnDialogConfirm_Click"
                                            component="GetFromEHRSSConfirm" /><asp:ImageButton ID="ibtnDialogConfirmDisable" runat="server"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmDisableBtn %>" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                Enabled="false" component="GetFromEHRSSConfirmDisable" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" />

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
            <cc1:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="90%" />
            <cc1:MessageBox ID="udcMessageBox" runat="server" Width="90%" />
            <asp:Panel ID="pnlTokenManagement" runat="server">
                <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                    Height="0px" Width="0px" OnClientClick="return false;" />
                <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vInputSearch" runat="server">
                        <table>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblEnrolRefNoText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtEnrolRefNo" runat="server" MaxLength="17" onBlur="Upper(event,this)"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtSPID" runat="server" MaxLength="8"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtSPHKID" runat="server" MaxLength="11" onBlur="formatHKID(this)"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtSPName" runat="server" MaxLength="40" ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>"
                                        onChange="Upper(event,this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPPhone" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="20"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblTokenSerialNo" runat="server" Text="<%$ Resources:Text, TokenSerialNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtTokenSerialNo" runat="server" MaxLength="20"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Text, ServiceProviderStatus %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True" Width="155">
                                    </asp:DropDownList></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPHealthProf" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlSPHealthProf" runat="server" AppendDataBoundItems="True">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblScheme" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlScheme" runat="server" AppendDataBoundItems="True" Width="155">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td align="center" style="height: 35px">
                                    <asp:ImageButton ID="ibtnSSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSSearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredERN" runat="server" TargetControlID="txtEnrolRefNo"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="-">
                            

                        </cc2:FilteredTextBoxExtender>
                        <cc2:FilteredTextBoxExtender ID="FilteredSPID" runat="server" TargetControlID="txtSPID"
                            FilterType="Custom, Numbers">
                            

                        </cc2:FilteredTextBoxExtender>
                        <cc2:FilteredTextBoxExtender ID="FilteredSPHKID" runat="server" TargetControlID="txtSPHKID"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="()">
                            

                        </cc2:FilteredTextBoxExtender>
                        <cc2:FilteredTextBoxExtender ID="FilteredSPName" runat="server" TargetControlID="txtSPName"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters " ValidChars=",'.- ">
                            

                        </cc2:FilteredTextBoxExtender>
                    </asp:View>
                    <asp:View ID="vSearchResult" runat="server">
                        <uc5:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panSearchCriteriaReview" />
                        <asp:Panel ID="panSearchCriteriaReview" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultERN" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
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
                                        <asp:Label ID="lblResultSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblResultSPName" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultPhone" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultTokenSerialNoText" runat="server" Text="<%$ Resources:Text, TokenSerialNo %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultTokenSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblResultHealthProf" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblResultScheme" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td colspan="2"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="true" Width="950px" OnRowDataBound="gvResult_RowDataBound" OnRowCommand="gvResult_RowCommand"
                            OnSorting="gvResult_Sorting" OnPageIndexChanging="gvResult_PageIndexChanging" OnPreRender="gvResult_PreRender">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Enrolment_Ref_No" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnERN" runat="server" CommandArgument='<%# Eval("Enrolment_Ref_No") %>'> </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblRSPID" runat="server" Text='<%# Eval("SP_ID") %>' CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRSPHKID" runat="server" Text='<%# Eval("SP_HKID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
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
                                <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, Status %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRStatus" runat="server" Text='<%# Eval("Record_Status") %>'></asp:Label>
                                        <asp:Label ID="lblRStatusCode" runat="server" Text='<%# Eval("Record_Status") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
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
                    <asp:View ID="vDetail" runat="server">
                        <asp:HiddenField ID="hfDCurrentAction" runat="server" />
                        <asp:HiddenField ID="hfDEnrolmentAction" runat="server" />
                        <table class="TableStyle1">
                            <tr class="TrStyle1">
                                <td class="TdStyle1">
                                    <asp:Label ID="lblDSPNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblDSPNameEN" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblDSPNameCH" runat="server" CssClass="TextChi"></asp:Label></td>
                            </tr>
                            <tr class="TrStyle1">
                                <td>
                                    <asp:Label ID="lblDSPIDText" runat="server" Text="<%$ Resources: Text, ServiceProviderID %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblDSPID" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDSPStatusText" runat="server" Text="<%$ Resources: Text, ServiceProviderStatus %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblDSPStatus" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trDNewEnrolledScheme" runat="server" class="TrStyle1">
                                <td>
                                    <asp:Label ID="lblDNewEnrolledSchemeText" runat="server" Text="<%$ Resources: Text, NewEnrolledScheme %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblDNewEnrolledScheme" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDPCDStatus" runat="server" class="TrStyle1">
                                <td>
                                    <asp:Label ID="lblPCDStatusText" runat="server" Text="<%$ Resources: Text, PCDStatus %>"></asp:Label></td>
                                 <td >
                                     <table cellpadding="0" cellspacing ="0" >
                                         <tr>
                                             <td>
                                                 <asp:Label ID="lblPCDStatus" runat="server" CssClass="tableText" Text=""></asp:Label>
                                             </td>
                                             <td style="padding-left:10px">
                                                <asp:ImageButton ID="ibtnDJoinPCD" runat="server" ImageUrl="<%$ Resources: ImageUrl, JoinPCDBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, JoinPCDBtn_Short %>"
                                                ToolTip="<%$ Resources: AlternateText, JoinPCDBtn_Short %>" OnClick="ibtnJoinPCD_Click" />
                                                <asp:ImageButton ID="ibtnDJoinPCDDisable" runat="server" ImageUrl="<%$ Resources:ImageUrl, JoinPCD_DBtn %>"  style="display:block;"
                                                AlternateText="<%$ Resources:AlternateText, JoinPCDBtn_Short %>" Enabled="false"
                                                ToolTip="<%$ Resources:AlternateText, JoinPCDBtn_Short %>" OnClientClick="return false;" />                                         
                                                 <asp:HiddenField ID="hfDPCDWarningPopupType" runat="server" />
                                             </td>
                                         </tr>
                                     </table>
                                </td>
                            </tr>
                            <tr id="trDPCDProfessional" runat="server" class="TrStyle1">
                                <td>
                                    <asp:Label ID="lblPCDProfessionalText" runat="server" Text="<%$ Resources: Text, PCDProfessional %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPCDProfessional" runat="server" CssClass="tableText" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDUserDeclareJoinPCD" runat="server" class="TrStyle1">
                                <td></td>
                                <td>
                                    <asp:Label ID="lblDUserDeclareJoinPCD" runat="server" CssClass="tableText" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="panDExistingToken" runat="server" GroupingText="" Style="width: 70%">
                            <table class="TableStyle1">
                                <tr class="TrStyle1">
                                    <td style="width: 175px;">
                                        <asp:Label ID="lblDTokenSerialNoText" runat="server" Text="<%$ Resources: Text, TokenSerialNo %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDTokenSerialNo" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:TextBox ID="txtDTokenSerialNo" runat="server" MaxLength="20" component="TokenSerialNo"></asp:TextBox>
                                        <asp:Image ID="imgDTokenSerialNoAlert" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="AbsMiddle" />
                                        &nbsp;
                                        <asp:ImageButton ID="ibtnDGetFromEHRSS" runat="server" ImageUrl="<%$ Resources: ImageUrl, GetFromEHRSSBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, GetFromEHRSSBtn %>" ImageAlign="AbsMiddle" component="GetFromEHRSS" />
                                        <asp:ImageButton ID="ibtnDClear" runat="server" ImageUrl="<%$ Resources: ImageUrl, ClearBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ClearBtn %>" ImageAlign="AbsMiddle" OnClick="ibtnDClear_Click" />
                                        <cc2:FilteredTextBoxExtender ID="fteDTokenSerialNo" runat="server" TargetControlID="txtDTokenSerialNo"
                                            FilterType="Numbers">
                                        </cc2:FilteredTextBoxExtender>
                                        <asp:HiddenField ID="hfDObtainFromEHRSS" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trDEHRSSReplacementToken" runat="server" class="TrStyle1">
                                    <td >
                                        <asp:Label ID="lblDEHRSSReplacementTokenText" runat="server" Text="<%$ Resources: Text, ReplacementTokenSerialNo %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDEHRSSReplacementToken" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:TextBox ID="txtDEHRSSReplacementToken" runat="server" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trDUserDeclareEHRSSUser" runat="server" class="TrStyle1">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblDUserDeclareEHRSSUser" runat="server" CssClass="tableText" Text="<%$ Resources: Text, UserDeclaredAsEHRSSUser %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trTokenIssueBy" runat="server" class="TrStyle1">
                                    <td >
                                        <asp:Label ID="lblDTokenIssuedByText" runat="server" Text="<%$ Resources: Text, TokenIssueBy %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDTokenIssuedBy" runat="server" Text="" CssClass="tableText"></asp:Label>
                                        <table id="tblDTokenIssuedBy" runat="server" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblDTokenIssuedBy" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" CssClass="tableText" Enabled="false" component="TokenIssuedBy">
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgDTokenIssuedByAlert" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="AbsMiddle" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr  id="trDShareTokenStatus" runat="server"  class="TrStyle1">
                                    <td >
                                        <asp:Label ID="lblDIsShareTokenText" runat="server" Text="<%$ Resources: Text, IsShareToken %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDIsShareToken" runat="server" Text="" CssClass="tableText"></asp:Label>
                                        <table id="tblDIsShareToken" runat="server" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblDIsShareToken" runat="server" RepeatDirection="Horizontal" CssClass="tableText"
                                                        Enabled="false" component="IsShareToken">
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgDIsShareTokenAlert" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="AbsMiddle" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trDTokenStatus" runat="server" class="TrStyle1">
                                    <td  >
                                        <asp:Label ID="lblDTokenStatusText" runat="server" Text="<%$ Resources: Text, TokenStatus %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblDTokenStatus" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblDTokenNewStatus" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="panDNewToken" runat="server" GroupingText="<%$ Resources: Text, NewToken %>" Style="width: 70%; padding-top: 16px">
                            <table class="TableStyle1" style="width: 100%">
                                <tr class="TrStyle1">
                                    <td>
                                        <asp:Label ID="lblDRTokenSerialNoText" runat="server" Text="<%$ Resources: Text, TokenSerialNo %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDRTokenSerialNo" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:TextBox ID="txtDRTokenSerialNo" runat="server" MaxLength="20"></asp:TextBox>
                                        <asp:Image ID="imgDRTokenSerialNoAlert" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="AbsMiddle" />
                                        <cc2:FilteredTextBoxExtender ID="fteDRTokenSerialNo" runat="server" TargetControlID="txtDRTokenSerialNo" FilterType="Numbers"></cc2:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr class="TrStyle1">
                                    <td class="TdStyle1">
                                        <asp:Label ID="lblDRTokenIssuedByText" runat="server" Text="<%$ Resources: Text, TokenIssueBy %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDRTokenIssuedBy" runat="server" Text="" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="TrStyle1">
                                    <td>
                                        <asp:Label ID="lblDRIsShareTokenText" runat="server" Text="<%$ Resources: Text, IsShareToken %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDRIsShareToken" runat="server" Text="" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="TrStyle1">
                                    <td>
                                        <asp:Label ID="lblDRReplacementReasonText" runat="server" Text="<%$ Resources: Text, TokenReplaceReason %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDRReplacementReason" runat="server" CssClass="tableText"></asp:Label>
                                        <table id="tblDRReplacementReason" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblDRReplacementReason" runat="server" CssClass="tableText">
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Image ID="imgDRReplacementReasonAlert" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="AbsBottom" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table class="TableStyle1">
                            <tr id="trDDeactivationTime" runat="server" class="TrStyle1">
                                <td class="TdStyle1">
                                    <asp:Label ID="lblDDeactivationTimeText" runat="server" Text="<%$ Resources: Text, DeactivationTime %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDDeactivationTime" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDDeactivationApprovedBy" runat="server" class="TrStyle1">
                                <td class="TdStyle1">
                                    <asp:Label ID="lblDDeactivationApprovedByText" runat="server" Text="<%$ Resources: Text, DeactivationApprovedBy %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDDeactivationApprovedBy" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDDeactivateReason" runat="server" class="TrStyle1">
                                <td class="TdStyle1">
                                    <asp:Label ID="lblDInputDeactiveReasonText" runat="server" Text="<%$ Resources: Text, TokenDeactivateReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDDeactivateReason" runat="server" CssClass="tableText"></asp:Label>
                                    <table id="tblDDeactivateReason" runat="server" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:RadioButtonList ID="rblDDeactivateReason" runat="server" CssClass="tableText">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Image ID="imgDDeactiveReasonAlert" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="AbsBottom" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:MultiView ID="mvDButton" runat="server">
                            <asp:View ID="vNoButton" runat="server">
                            </asp:View>
                            <asp:View ID="vBackOnly" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; text-align: left">
                                            <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vManageToken" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; text-align: left">
                                            <asp:ImageButton ID="ibtnDManageTokenBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnDManageTokenDeactivate" runat="server" ImageUrl='<%$ Resources: ImageUrl, DeactivateTokenBtn %>'
                                                AlternateText='<%$ Resources: AlternateText, DeactivateTokenBtn %>' OnClick="ibtnDManageTokenDeactivate_Click" />
                                            <asp:ImageButton ID="ibtnDManageTokenReplace" runat="server" ImageUrl='~/Images/button/btn_replaceToken.png'
                                                AlternateText='<%$ Resources: AlternateText, ReplaceTokenBtn %>' OnClick="ibtnDManageTokenReplace_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vSaveCancel" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnDSave" runat="server" ImageUrl="<%$ Resources: ImageUrl, SaveBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, SaveBtn %>" OnClick="ibtnDSave_Click" />
                                            <asp:ImageButton ID="ibtnDCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnDCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vConfirmBack" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnDConfirmConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnDConfirmConfirm_Click" />
                                            <asp:ImageButton ID="ibtnDConfirmBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDConfirmBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vComplete" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnDReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnDReturn_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vNewEnrolment" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; text-align: left">
                                            <asp:ImageButton ID="ibtnDNewEnrolmentBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnDNewEnrolmentProceed" runat="server" ImageUrl="<%$ Resources: ImageUrl, ProceedBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ProceedBtn %>" OnClick="ibtnDProceed_Click" />
                                            <asp:ImageButton ID="ibtnDNewEnrolmentReject" runat="server" AlternateText="<%$ Resources:AlternateText, RejectBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" OnClick="ibtnReject_Click" />
                                            <asp:ImageButton ID="ibtnDNewEnrolmentReturnForAmendment" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentBtn %>" OnClick="ibtnReturnForAmendment_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vSchemeEnrolment" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; text-align: left">
                                            <asp:ImageButton ID="ibtnDSchemeEnrolmentBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnDConfirmSchemeEnrolment" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmSchEnrolBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ConfirmSchEnrolBtn %>" OnClick="ibtnDConfirmSchemeEnrolment_Click" />
                                            <asp:ImageButton ID="ibtnDSchemeEnrolmentReject" runat="server" AlternateText="<%$ Resources:AlternateText, RejectBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" OnClick="ibtnReject_Click" />
                                            <asp:ImageButton ID="ibtnDSchemeEnrolmentReturnForAmendment" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentBtn %>" OnClick="ibtnReturnForAmendment_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vCompleteNewEnrolment" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; text-align: left">
                                            <asp:ImageButton ID="ibtnDCompleteNewEnrolmentReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnDReturn_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnViewRecordReprint" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReprintBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ReprintBtn %>"
                                                OnClick="ibtnDNewEnrolmentSchemeEnrolmentPrint_Click" />
                                            <asp:ImageButton ID="ibtnJoinPCD" runat="server" ImageUrl="<%$ Resources: ImageUrl, JoinPCDBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, JoinPCDBtn_Short %>"
                                                ToolTip="<%$ Resources: AlternateText, JoinPCDBtn_Short %>" OnClick="ibtnJoinPCD_Click" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vCompleteSchemeEnrolment" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; text-align: left">
                                            <asp:ImageButton ID="ibtnDCompleteSchemeEnrolmentReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnDReturn_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnDCompleteSchemeEnrolmentPrint" runat="server" ImageUrl="<%$ Resources: ImageUrl, PrintBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, PrintBtn %>" OnClick="ibtnDNewEnrolmentSchemeEnrolmentPrint_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vCompleteReplaceToken" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; text-align: left">
                                            <asp:ImageButton ID="ibtnDCompleteReplaceTokenReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnDReturn_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnDCompleteReplaceTokenPrint" runat="server" ImageUrl="<%$ Resources: ImageUrl, PrintBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, PrintBtn %>" OnClick="ibtnDCompleteReplaceTokenPrint_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vRefreshToken" runat="server">
                                <%--<asp:ImageButton ID="ibtnViewRecordRefresh" runat="server" ImageUrl='<%$ Resources:ImageUrl, RefreshTokenBtn %>'
                                            AlternateText='<%$ Resources:AlternateText, RefreshTokenBtn%>' />--%>
                            </asp:View>
                        </asp:MultiView>


                        <asp:ImageButton ID="ibtnConfirmPrintLetter" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmBtn %>'
                            AlternateText='<%$ Resources:AlternateText, ConfirmBtn %>' Visible="False" />


                        </td>
                        </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="vError" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtnDErrorBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnDErrorBack_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="vPrintLetter" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-color: #ffffff" align="left">
                                    <uc1:spPrintFunction ID="udcPrintFunction" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="vCompletePage" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtnCompleteBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnDBack_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupExtenderConfirmConsent" runat="server" TargetControlID="ibtnDGetFromEHRSS"
                PopupControlID="panConfirmMsg" BehaviorID="mdlPopup" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" CancelControlID="ibtnDialogCancel" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc2:ModalPopupExtender>
            <asp:Panel ID="panPrintSchemeEnrolmentLetter" runat="server" Style="display: none;">
                <asp:Panel ID="panReprintSchemeEnrolmentLetterHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblReprintSchemeEnrolmentLetterHeading" runat="server" Text="Print Scheme Enrolment Letter"></asp:Label></td>
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
                                        <uc1:spPrintFunction ID="udcPrintSchemeEnrolmentLetter" runat="server" />
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
            <asp:Button ID="btnHiddenReprintSchemeEnrolmentLetter" runat="server" Style="display: none" />
            <cc2:ModalPopupExtender ID="popupPrintSchemeEnrolmentLetter" runat="server" TargetControlID="btnHiddenReprintSchemeEnrolmentLetter"
                PopupControlID="panPrintSchemeEnrolmentLetter" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panReprintSchemeEnrolmentLetterHeading">
            </cc2:ModalPopupExtender>


            <%-- Popup for Join PCD --%>
            <asp:Panel ID="panTypeOfPracticePopup" runat="server" Width="900px" Style="display: none;">
                <uc2:ucTypeOfPracticePopup ID="ucTypeOfPracticePopup" runat="server" />
                <asp:Button ID="btnTypeOfPracticePopupDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupExtenderTypeOfPractice" runat="server" TargetControlID="btnTypeOfPracticePopupDummy"
                PopupControlID="panTypeOfPracticePopup" BehaviorID="mdlPopup1" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc2:ModalPopupExtender>
            <%-- End of Popup for Join PCD --%>

            <%-- Popup for successful submission of PCD information --%>
            <asp:Panel ID="panNoticePopup" runat="server" Width="500px" Height="100px" Style="display: none">
                <uc3:ucNoticePopUp ID="ucNoticePopup" runat="server" ButtonMode="OK" NoticeMode="Notification" />
                <asp:Button ID="btnNoticePopupDummy" runat="server" Style="display: none" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="btnNoticePopupDummy"
                PopupControlID="panNoticePopup" BehaviorID="mdlPopup2" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc2:ModalPopupExtender>
            <%-- End of successful submission of PCD information --%>

            <%-- Popup for Enrolment Copy --%>
            <asp:Panel ID="panEnrolmentCopyPopup" runat="server" Width="900px" Style="display: none">
                <uc4:ucEnrolmentCopyPopup ID="ucEnrolmentCopyPopup" runat="server" />
                <asp:Button ID="btnEnrolmentCopyPopup" runat="server" Style="display: none" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupExtenderEnrolmentCopy" runat="server" TargetControlID="btnEnrolmentCopyPopup"
                PopupControlID="panEnrolmentCopyPopup" BehaviorID="mdlPopup3" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc2:ModalPopupExtender>
            <%-- End of Popup for Enrolment Copy --%>

            <%-- Popup for suspend scheme if service provider is suspended --%>
            <asp:Panel ID="panNewEnrolSuspend" runat="server" Width="500px" Height="100px" Style="display: none">
                <uc6:ucNoticePopUp ID="ucNewEnrolSuspendPopup" runat="server" ButtonMode="OK" NoticeMode="Notification" />
                <asp:Button ID="btnNewEnrolSuspend" runat="server" Style="display: none" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupNewEnrolSuspend" runat="server" TargetControlID="btnNewEnrolSuspend"
                PopupControlID="panNewEnrolSuspend" BehaviorID="mdlPopup4" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc2:ModalPopupExtender>
            <%-- End of suspend scheme if service provider is suspended --%>

            <%-- Popup for PCD Warning --%>
            <asp:Panel Style="display: none" ID="panPopupPCDWarning" runat="server" Width="600px">
                <uc7:ucPCDWarningPopup ID="ucPCDWarningPopup" runat="server" />
            </asp:Panel>
            <asp:Button runat="server" ID="btnHiddenShowPCDWarning" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModelPopupExtenderPCDWarning" runat="server" TargetControlID="btnHiddenShowPCDWarning"
                PopupControlID="panPopupPCDWarning" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="">
            </cc2:ModalPopupExtender>
            <%-- End of Popup for PCD Warning --%>

            <%-- Popup for Enrolment Action --%>
            <asp:Panel ID="panPopupEnrolmentAction" runat="server" Width="500px" Height="100px" Style="display: none">
                <uc6:ucNoticePopUp ID="ucEnrolmentActionPopup" runat="server" ButtonMode="ConfirmCancel" NoticeMode="Confirmation" MessageAlignment="Center" MessageWidth="95%" />
                <asp:Button ID="btnEnrolmentAction" runat="server" Style="display: none" />                 
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupEnrolmentAction" runat="server" TargetControlID="btnEnrolmentAction"
                PopupControlID="panPopupEnrolmentAction" BehaviorID="mdlPopup5" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc2:ModalPopupExtender>
            <%-- End of Popup for Reject Enrolment --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
