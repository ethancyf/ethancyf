<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="spBankAccountVerification.aspx.vb" Inherits="HCVU.spBankAccountVerification"
    Title="<%$ Resources:Title, SPBankAccountVerification %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" src="../JS/Common.js" type="text/javascript"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <asp:Panel ID="panRemindDelistPractice" runat="server" Style="display: none;">
                <asp:Panel ID="panRemindDelistPracticeHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, ReminderPopupTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" style="width: 44px; height: 42px; padding-top: 4px" valign="top">
                                        <asp:Image ID="imgRemindDelistPractice" runat="server" ImageUrl="~/Images/others/Information.png" /></td>
                                    <td align="left" style="height: 80px; padding-top: 3px" valign="top">
                                        <asp:Label ID="lblRemindDelistPractice" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ReminderPopupDelistPracticeContent %>"></asp:Label></td>

                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnDialogRemindDelistPracticeConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnDialogRemindDelistPracticeConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDialogRemindDelistPracticeCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDialogRemindDelistPracticeCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                            height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>


            <asp:Image ID="img_banner" runat="server" AlternateText="<%$ Resources:AlternateText, BankAccountVerificationBanner %>"
                ImageUrl="<%$ Resources:ImageUrl, BankAccountVerificationBanner %>" />
            <cc2:MessageBox ID="udcErrorMessage" runat="server" Visible="False"></cc2:MessageBox>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Visible="False"></cc2:InfoMessageBox>
            <asp:TextBox ID="txt_SchemeCode" runat="server" Visible="False"></asp:TextBox><asp:TextBox
                ID="txt_recordCount" runat="server" Visible="False"></asp:TextBox><asp:TextBox ID="txt_currentERN"
                    runat="server" Visible="False"></asp:TextBox>
            <asp:Panel ID="pnlBkVerification" runat="server">
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vSearchCriteria" runat="server">
                        <table width="100%">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblEnrolmentNoText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txt_enrolmentNo" onChange="convertToUpper(this)" runat="server"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txt_SPID" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txt_SPhkid" runat="server" MaxLength="11" onChange="formatHKID(this)"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txt_serviceProvider" onChange="convertToUpper(this)" runat="server"
                                        ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPPhone" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txt_dayTimeContact" runat="server"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPHealthProf" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlSPHealthProf" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True" Width="155px">
                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblScheme" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlScheme" runat="server" AppendDataBoundItems="True" Width="155px">
                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td align="center" style="padding-top: 15px">
                                    <asp:ImageButton ID="btn_search" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        OnClick="btn_search_Click" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" /></td>
                            </tr>
                        </table>
                        <cc1:FilteredTextBoxExtender ID="FilteredERN" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                            TargetControlID="txt_enrolmentNo" ValidChars="-">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="FilteredSPID" runat="server" FilterType="Custom, Numbers"
                            TargetControlID="txt_SPID">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="FilteredSPHKID" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                            TargetControlID="txt_SPhkid" ValidChars="()">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="FilteredSPName" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters "
                            TargetControlID="txt_serviceProvider" ValidChars=",'.- ">
                        </cc1:FilteredTextBoxExtender>
                    </asp:View>
                    <asp:View ID="vSearchResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />
                            
                        <uc1:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panel_searchCriteria" />
                        
                        <asp:Panel ID="panel_searchCriteria" runat="server" Width="100%">
                            <table width="90%">
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lbl_enrolmentNo" runat="server" Font-Bold="True" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_SPID" runat="server" Font-Bold="True" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; height: 7px;">
                                        <asp:Label ID="lblResultSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td style="width: 200px; height: 7px;">
                                        <asp:Label ID="lbl_SPhkic" runat="server" Font-Bold="True" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px; height: 7px;">
                                        <asp:Label ID="lblResultSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                    <td style="height: 7px">
                                        <asp:Label ID="lbl_spValue" runat="server" Font-Bold="True" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultPhone" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl_serviceType" runat="server" Font-Bold="True" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultStatu" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblResultScheme" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <asp:GridView ID="gvBankVerificationList" runat="server" AutoGenerateColumns="False"
                            Width="100%" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                            OnRowDataBound="gvBankVerificationList_RowDataBound" OnRowCommand="GridView1_RowCommand"
                            AllowSorting="True" OnSorting="gvBankVerificationList_Sorting" OnPreRender="gvBankVerificationList_PreRender">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, EnrolRefNo %>" SortExpression="enrolRefNo">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_enrolRef" runat="server" CausesValidation="false" CommandName=""
                                            Text='<%# Eval("enrolRefNo") %>'></asp:LinkButton>
                                        <asp:HiddenField ID="hfERN" runat="server" Value='<%# Eval("enrolRefNo") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="120px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, ServiceProviderID %>" SortExpression="SPID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_spid" runat="server" CausesValidation="false" CommandName=""
                                            Text='<%# Eval("SPID") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Vetting_Dtm" HeaderText="<%$ Resources:Text, VettingProcessTime %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVettingProcessingDtm" runat="server" Text='<%# Eval("Vetting_Dtm") %> '></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="90px"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, ServiceProviderHKID %>" SortExpression="SPHKID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRSPHKID" runat="server" Text='<%# Eval("SPHKID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="90px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="spName" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblREname" runat="server" Text='<%# Eval("spName") %>'></asp:Label><br />
                                        <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("spChiName") %>' CssClass="TextGridChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" width="270px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, ContactNo %>" SortExpression="DaytimeContact">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRPhone" runat="server" Text='<%# Eval("DaytimeContact") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px"/>
                                </asp:TemplateField>
                                <asp:BoundField DataField="bankStatus" HeaderText="<%$ Resources:Text, Status %>"
                                    SortExpression="bankStatus">
                                    <ItemStyle VerticalAlign="Top" Width="60px"/>
                                </asp:BoundField>
                                <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px"/>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="width: 101px">
                                    <asp:ImageButton ID="btn_cancel" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        OnClick="btn_cancel_Click" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" />
                                </td>
                                <td align="center">
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="vActionResult" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="height: 15px">
                                    <asp:ImageButton ID="btn_back1" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btn_back1_Click" />
                                    <asp:ImageButton ID="btn_nextSearch" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="btn_nextSearch_Click"
                                        Visible="False" />
                                </td>
                            </tr>
                        </table>
                        <table style="width: 814px">
                            <tr>
                                <td>
                                    <table style="width: 807px">
                                        <tr>
                                            <td style="width: 200px">
                                                <asp:Label ID="lbl_referenceNo_text" runat="server" Width="205px" Visible="False">Reference Number:</asp:Label></td>
                                            <td colspan="2" style="width: 534px">
                                                <asp:Label ID="lbl_referenceNo" runat="server" Width="524px" Visible="False">TXXX01-01-1</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="vToBeDeleted" runat="server">
                    </asp:View>
                    <asp:View ID="vBankDetails" runat="server">
                        <asp:HiddenField ID="hfEnrolmentAction" runat="server" />
                        <table>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblSubmissionDateText" runat="server" Text="<%$ Resources:Text, SubmissionDtmTime %>"
                                        Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblSubmissionDate" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblHKID" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"
                                        Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblAddress" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblEmailText" runat="server" Text="<%$ Resources:Text, Email %>" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblEmail" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"
                                        Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblContactNo" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblFax" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                            </tr>
                        </table>
                        <table width="90%">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblEnrolRefNoText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblEnrolRefNo" runat="server" CssClass="tableText"></asp:Label></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblEname" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblCname" runat="server" CssClass="TextChi"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px; height: 7px;">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                <td style="width: 200px; height: 7px;">
                                    <asp:Label ID="lblStatus2" runat="server" CssClass="tableText"></asp:Label></td>
                                <td style="width: 200px; height: 7px;">
                                    <asp:Label ID="lblSPIDLabel" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"
                                        Visible="False"></asp:Label></td>
                                <td style="height: 7px">
                                    <asp:Label ID="lblSPID" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvCompletePracticeBank" runat="server" AutoGenerateColumns="False"
                            ShowHeader="False" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompletePracticeBankIndex" runat="server" Text='<%# Eval("Value.DisplaySeq") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="16px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblCompletePracticeTitle" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="padding-left: 15px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOText"  runat="server" Width="190px" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblMO" runat="server" Text='<%# GetMOName(Eval("Value.MODisplaySeq")) %>'
                                                                    CssClass="tableText" Width="700px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblCompletePracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCompletePracticeName" runat="server" Text='<%# Eval("Value.PracticeName") %>'
                                                                    CssClass="tableText"></asp:Label>
                                                                <asp:Label ID="lblPracticeNameChi" runat="server" Text='<%# formatChineseString(Eval("Value.PracticeNameChi")) %>'
                                                                    CssClass="TextChi"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblCompletePracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCompletePracticeAddress" runat="server" Text='<%# formatAddress(Eval("Value.PracticeAddress.Room"), Eval("Value.PracticeAddress.Floor"), Eval("Value.PracticeAddress.Block"), Eval("Value.PracticeAddress.Building"), Eval("Value.PracticeAddress.District"), Eval("Value.PracticeAddress.AreaCode")) %>'
                                                                    CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;" valign="top">
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCompletePracticeChiAddress" runat="server" Text='<%# formatChiAddress(Eval("Value.PracticeAddress.Room"), Eval("Value.PracticeAddress.Floor"), Eval("Value.PracticeAddress.Block"), Eval("Value.PracticeAddress.ChiBuilding"), Eval("Value.PracticeAddress.District"), Eval("Value.PracticeAddress.AreaCode")) %>'
                                                                    CssClass="TextChi"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 1px">
                                                    <hr style="width: 100%; color: #ff8080; border-top-style: none; border-right-style: none;
                                                        border-left-style: none; height: 1px; border-bottom-style: none;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblCompleteBankTitle" runat="server" Text="<%$ Resources:Text, Bank %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="padding-left: 15px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;">
                                                                <asp:Label ID="lblCompleteBankNameText" runat="server" Width="190px" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCompleteBankName" runat="server" Text='<%# Eval("Value.BankAcct.BankName") %>'
                                                                    CssClass="TextChi" Width="700px"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;">
                                                                <asp:Label ID="lblCompleteBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCompleteBranchName" runat="server" Text='<%# Eval("Value.BankAcct.BranchName") %>'
                                                                    CssClass="TextChi"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;">
                                                                <asp:Label ID="lblCompleteBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblCompleteBankAcc" runat="server" Text='<%# Eval("Value.BankAcct.BankAcctNo") %>'
                                                                    CssClass="tableText"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 190px; background-color: #f7f7de;vertical-align:top">
                                                                <asp:Label ID="lblCompleteBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                            <td style="vertical-align:top">
                                                                <asp:Label ID="lblCompleteBankOwner" runat="server" Text='<%# Eval("Value.BankAcct.BankAcctOwner") %>'
                                                                    CssClass="tableText" Width="700px" Style="word-wrap:break-word"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="width: 99px">
                                    <asp:ImageButton ID="btn_backFromTemp" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btn_backFromTemp_Click" />
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="btnAccept" runat="server" AlternateText="<%$ Resources:AlternateText, AcceptBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, AcceptBtn %>" OnClick="btn_markSecondVerify_Click" /><asp:ImageButton
                                            ID="btnAccept_Disabled" runat="server" AlternateText="<%$ Resources:AlternateText, AcceptBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, AcceptDisabledBtn %>" Enabled="False" />&nbsp;<asp:ImageButton
                                                ID="btnDefer" runat="server" AlternateText="<%$ Resources:AlternateText, DeferBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, DeferBtn %>" OnClick="btnDefer_Click" /><asp:ImageButton
                                                    ID="btnDefer_Disabled" runat="server" AlternateText="<%$ Resources:AlternateText, DeferBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, DeferDisabledBtn %>" Enabled="False" />&nbsp;<asp:ImageButton
                                                        ID="btn_Reject" runat="server" AlternateText="<%$ Resources:AlternateText, RejectBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" OnClick="btn_Reject_Click" /><asp:ImageButton
                                                            ID="btn_Reject_Disabled" runat="server" AlternateText="<%$ Resources:AlternateText, RejectBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, RejectDisabledBtn %>" Enabled="False" />
                                    <asp:ImageButton ID="btnReturnForAmendment" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentBtn %>" OnClick="btn_ReturnForAmendment_Click" /><asp:ImageButton
                                            ID="btnReturnForAmendment_Disabled" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentDisabledBtn %>" Enabled="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewError" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnErrorBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnErrorBack_Click" /></asp:View>
                </asp:MultiView>
            </asp:Panel>
            <asp:Button ID="btnHiddenShowDialogRemindDelistPractice" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderRemindDelistPractice" runat="server" TargetControlID="btnHiddenShowDialogRemindDelistPractice"
                PopupControlID="panRemindDelistPractice" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panRemindDelistPracticeHeading">
            </cc1:ModalPopupExtender>

            <%-- Popup for Enrolment Action --%>
            <asp:Panel ID="panPopupEnrolmentAction" runat="server" Width="500px" Height="100px" Style="display: none">
                <uc2:ucNoticePopUp ID="ucEnrolmentActionPopup" runat="server" ButtonMode="ConfirmCancel" NoticeMode="Confirmation" MessageAlignment="Center" MessageWidth="95%" />
                <asp:Button ID="btnEnrolmentAction" runat="server" Style="display: none" />                 
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupEnrolmentAction" runat="server" TargetControlID="btnEnrolmentAction"
                PopupControlID="panPopupEnrolmentAction" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of Popup for Reject Enrolment --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
