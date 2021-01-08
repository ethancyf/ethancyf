<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="reimbursementEnquiry.aspx.vb" Inherits="HCVU.reimbursement_enquiry"
    Title="<%$ Resources:Title, ReimbursementPaymentFileEnquiry %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="ClaimTransDetail.ascx" TagName="ClaimTransDetail" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp"
    TagPrefix="uc4" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        table.TableStyle1 td {
            vertical-align: top;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="300">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Image ID="imgReimbursementEnquiry" runat="server" AlternateText="<%$ Resources:AlternateText, ReimbursementPaymentEnquiryBanner %>"
                ImageUrl="<%$ Resources:ImageUrl, ReimbursementPaymentEnquiryBanner %>" />
            <div style="height: 4px"></div>
            <cc1:TabContainer ID="tcCore" runat="server" CssClass="m_ajax__tab_xp" ActiveTabIndex="0">
                <cc1:TabPanel ID="tpDisable" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="lblDHeader" runat="server" Text="<%$ Resources: Text, CurrentReimbursement %>"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div style="height: 5px"></div>
                        <cc2:InfoMessageBox ID="udcDInfoMessageBox" runat="server" Width="100%" />
                        <div style="height: 66px"></div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpCurrent" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="lblCHeader" runat="server" Text="<%$ Resources: Text, CurrentReimbursement %>"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>                         
                        <table class="TableStyle1">
                            <tr>
                                <td style="width: 150px">
                                    <asp:Label ID="lblCReimbursementIDText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCReimbursementID" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 14px"></tr>
                        </table>
                        <asp:Panel ID="panlCP" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblCPaymentFileRequired" runat="server" Text="<%$ Resources:Text, PaymentFileRequired %>">
                            </asp:Label>
                        </div>
                        <table class="TableStyle1">
                            <tr style="height: 2px"></tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblCPStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label>
                                </td>
                                <td style="width: 250px">
                                    <asp:Label ID="lblCPStatus" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td style="width: 180px">
                                    <asp:Label ID="lblCPBankPaymentDateText" runat="server" Text="<%$ Resources:Text, BankPaymentDay %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCPBankPaymentDate" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 4px"></tr>
                        </table>
                        <asp:GridView ID="gvCP" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                            AllowSorting="True" Width="945px" OnRowDataBound="gvCP_RowDataBound" OnPreRender="gvCP_PreRender"
                            OnSorting="gvCP_Sorting">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Display_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'>
                                        </asp:Label>
                                        <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Hold_Dtm" HeaderText="<%$ Resources:Text, AuthorizationHoldTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHoldTime" runat="server" Text='<%# Eval("Hold_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Hold_By" HeaderText="<%$ Resources:Text, AuthorizationHoldBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHoldBy" runat="server" Text='<%# Eval("Hold_By") %>'>
                                        </asp:Label>
                                        <asp:HiddenField ID="hfHoldBy" runat="server" Value='<%# Eval("Hold_By") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="First_Authorised_Dtm" HeaderText="<%$ Resources:Text, FirstAuthorizedTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstAuthTime" runat="server" Text='<%# Eval("First_Authorised_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="First_Authorised_By" HeaderText="<%$ Resources:Text, FirstAuthorizedBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstAuthBy" runat="server" Text='<%# Eval("First_Authorised_By") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Second_Authorised_Dtm" HeaderText="<%$ Resources:Text, SecondAuthorizedTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSecondAuthTime" runat="server" Text='<%# Eval("Second_Authorised_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Second_Authorised_By" HeaderText="<%$ Resources:Text, SecondAuthorizedBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSecondAuthBy" runat="server" Text='<%# Eval("Second_Authorised_By") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="height: 24px"></div>
                        </asp:Panel>
                        <asp:Panel ID="panlCN" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblCNoPaymentFileRequired" runat="server" Text="<%$ Resources:Text, NoPaymentFileRequired %>">
                            </asp:Label>
                        </div>
                        <table class="TableStyle1">
                            <tr style="height: 2px"></tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblCNStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label>
                                </td>
                                <td style="width: 250px">
                                    <asp:Label ID="lblCNStatus" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td style="width: 180px">
                                    <asp:Label ID="lblCNBankPaymentDateText" runat="server" Text="<%$ Resources:Text, BankPaymentDay %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCNBankPaymentDate" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 4px"></tr>
                        </table>
                        <asp:GridView ID="gvCN" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                            AllowSorting="True" Width="945px" OnRowDataBound="gvCN_RowDataBound" OnPreRender="gvCN_PreRender"
                            OnSorting="gvCN_Sorting">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Display_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'>
                                        </asp:Label>
                                        <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Hold_Dtm" HeaderText="<%$ Resources:Text, AuthorizationHoldTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHoldTime" runat="server" Text='<%# Eval("Hold_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Hold_By" HeaderText="<%$ Resources:Text, AuthorizationHoldBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHoldBy" runat="server" Text='<%# Eval("Hold_By") %>'>
                                        </asp:Label>
                                        <asp:HiddenField ID="hfHoldBy" runat="server" Value='<%# Eval("Hold_By") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="First_Authorised_Dtm" HeaderText="<%$ Resources:Text, FirstAuthorizedTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstAuthTime" runat="server" Text='<%# Eval("First_Authorised_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="First_Authorised_By" HeaderText="<%$ Resources:Text, FirstAuthorizedBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstAuthBy" runat="server" Text='<%# Eval("First_Authorised_By") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Second_Authorised_Dtm" HeaderText="<%$ Resources:Text, SecondAuthorizedTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSecondAuthTime" runat="server" Text='<%# Eval("Second_Authorised_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Second_Authorised_By" HeaderText="<%$ Resources:Text, SecondAuthorizedBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSecondAuthBy" runat="server" Text='<%# Eval("Second_Authorised_By") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>                       
                         <div style="height: 24px"></div>
                         </asp:Panel>
                        <asp:Panel ID="panlHA" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblHAPaymentFileRequired" runat="server" Text="<%$ Resources:Text, NoPaymentFileRequiredHA %>">
                            </asp:Label>
                        </div>
                        <table class="TableStyle1">
                            <tr style="height: 2px"></tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblHAStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label>
                                </td>
                                <td style="width: 250px">
                                    <asp:Label ID="lblHAStatus" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td style="width: 180px">
                                    <asp:Label ID="lblHABankPaymentDateText" runat="server" Text="<%$ Resources:Text, BankPaymentDay %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblHABankPaymentDate" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 4px"></tr>
                        </table>
                        <asp:GridView ID="gvHA" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                            AllowSorting="True" Width="945px" OnRowDataBound="gvHA_RowDataBound" OnPreRender="gvHA_PreRender"
                            OnSorting="gvHA_Sorting">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Display_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'>
                                        </asp:Label>
                                        <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Hold_Dtm" HeaderText="<%$ Resources:Text, AuthorizationHoldTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHoldTime" runat="server" Text='<%# Eval("Hold_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Hold_By" HeaderText="<%$ Resources:Text, AuthorizationHoldBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHoldBy" runat="server" Text='<%# Eval("Hold_By") %>'>
                                        </asp:Label>
                                        <asp:HiddenField ID="hfHoldBy" runat="server" Value='<%# Eval("Hold_By") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="First_Authorised_Dtm" HeaderText="<%$ Resources:Text, FirstAuthorizedTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstAuthTime" runat="server" Text='<%# Eval("First_Authorised_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="First_Authorised_By" HeaderText="<%$ Resources:Text, FirstAuthorizedBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFirstAuthBy" runat="server" Text='<%# Eval("First_Authorised_By") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Second_Authorised_Dtm" HeaderText="<%$ Resources:Text, SecondAuthorizedTime %>">
                                    <ItemStyle VerticalAlign="Top" Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSecondAuthTime" runat="server" Text='<%# Eval("Second_Authorised_Dtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Second_Authorised_By" HeaderText="<%$ Resources:Text, SecondAuthorizedBy %>">
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSecondAuthBy" runat="server" Text='<%# Eval("Second_Authorised_By") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </asp:Panel>
                        <div style="height: 10px"></div>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpPrevious" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="lblPHeader" runat="server" Text="<%$ Resources: Text, PreviousReimbursement %>"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="pnlSearchCriteriaReview" runat="server">
                            <table style="width: 90%">
                                <tr>
                                    <td style="width: 30%; vertical-align: top">
                                        <asp:Label ID="lblRPaymentFileDateFromText" runat="server" Text="<%$ Resources:Text, ReimbursementCompletionDate %>"></asp:Label>
                                    </td>
                                    <td style="width: 20%; vertical-align: top">
                                        <asp:Label ID="lblRPaymentFileDateFrom" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRPaymentFileDateToText" runat="server" Text="<%$ Resources:Text, To %>"></asp:Label>
                                    </td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRPaymentFileDateTo" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; vertical-align: top">
                                        <asp:Label ID="lblRSchemeCodeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                    </td>
                                    <td style="width: 20%; vertical-align: top">
                                        <asp:Label ID="lblRSchemeCode" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRReimburseIDText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label>
                                    </td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRReimburseID" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; vertical-align: top">
                                        <asp:Label ID="lblRSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"
                                            Visible="False"></asp:Label></td>
                                    <td style="width: 20%; vertical-align: top">
                                        <asp:Label ID="lblRSPName" runat="server" CssClass="tableText" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderIDStar %>"
                                            Visible="False"></asp:Label></td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRSPID" runat="server" CssClass="tableText" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; vertical-align: top">
                                        <asp:Label ID="lblRBankAccountText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"
                                            Visible="False"></asp:Label></td>
                                    <td style="width: 20%; vertical-align: top">
                                        <asp:Label ID="lblRBankAccount" runat="server" CssClass="tableText" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                            Visible="False"></asp:Label></td>
                                    <td style="width: 25%; vertical-align: top">
                                        <asp:Label ID="lblRPractice" runat="server" CssClass="tableText" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                        <cc2:InfoMessageBox ID="udcInfoBox" runat="server" Width="95%" />
                        <cc2:MessageBox ID="udcErrorBox" runat="server" Width="95%" />
                        <asp:MultiView ID="MultiViewReimbursementEnquiry" runat="server" ActiveViewIndex="0">
                            <asp:View ID="ViewFillCriteria" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rbtnLastFile" runat="server" Text="<%$ Resources:Text, LastXFiles %>"
                                                GroupName="groupSearchCriteria" /></td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlLastFile" runat="server" Width="300px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlLastFile_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-left: 20px; height: 20px; vertical-align: bottom">OR
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 260px">
                                            <asp:RadioButton ID="rbtnSubmissionDate" runat="server" Text="<%$ Resources:Text, ReimbursementCompletionDate %>"
                                                GroupName="groupSearchCriteria" /></td>
                                        <td style="width: 120px">
                                            <asp:TextBox ID="txtSubmissionDateFrom" runat="server" MaxLength="10" Width="70px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnSubmissionDateFrom" runat="server" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                            <asp:Image ID="imgAlertSubmissionDateFrom" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                Visible="False" />
                                            <cc1:CalendarExtender ID="calSubmissionDateFrom" CssClass="ajax_cal" runat="server"
                                                Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" PopupButtonID="ibtnSubmissionDateFrom"
                                                TargetControlID="txtSubmissionDateFrom" />
                                            <cc1:FilteredTextBoxExtender ID="filtereditSubmissionDateFrom" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtSubmissionDateFrom" ValidChars="-" />
                                        </td>
                                        <td style="width: 50px">
                                            <asp:Label ID="lblSubmissionDateTo" runat="server" Text="<%$ Resources:Text, To %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubmissionDateTo" runat="server" MaxLength="10" Width="70px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnSubmissionDateTo" runat="server" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                            <asp:Image ID="imgAlertSubmissionDateTo" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                Visible="False" />
                                            <cc1:CalendarExtender ID="calSubmissionDateTo" CssClass="ajax_cal" runat="server"
                                                Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" PopupButtonID="ibtnSubmissionDateTo"
                                                TargetControlID="txtSubmissionDateTo" />
                                            <cc1:FilteredTextBoxExtender ID="filtereditSubmissionDateTo" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtSubmissionDateTo" ValidChars="-" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: center; padding-top: 25px">
                                            <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewPaymentFileListing" runat="server">
                                <asp:GridView ID="gvPaymentFile" runat="server" AutoGenerateColumns="False" Width="100%"
                                    AllowPaging="True" AllowSorting="True" OnRowCommand="gvPaymentFile_RowCommand"
                                    OnRowDataBound="gvPaymentFile_RowDataBound" OnPageIndexChanging="gvPaymentFile_PageIndexChanging"
                                    OnSorting="gvPaymentFile_Sorting" OnPreRender="gvPaymentFile_PreRender">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PaymentFile %>" SortExpression="filePath">
                                            <ItemTemplate>
                                                <a href='<%# Eval("filePath") %>'>
                                                    <asp:Image ID="img_download" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReadyDownloadBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ReadyDownloadBtn %>" />
                                                </a>
                                                <asp:Image ID="img_processing" runat="server" ImageUrl="<%$ Resources:ImageUrl, ProcessingBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ProcessingBtn %>" />
                                                <asp:Label ID="lblPaymentFileNA" runat="server" Text="<%$ Resources:Text, N/A %>"
                                                    Enabled="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" SortExpression="scheme_code">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtn_SchemeCode" runat="server" CausesValidation="false" CommandName=""
                                                    Text='<%# Trim(Eval("Display_Code")) %>' />
                                                <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>' Visible="false"></asp:Label>
                                                <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ Resources:Text, ReimbursementID %>" ShowHeader="False"
                                            SortExpression="reimburseID">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtn_reimburseID" runat="server" CausesValidation="false" CommandName=""
                                                    Text='<%# Eval("reimburseID") %>' Visible="false"></asp:LinkButton>
                                                <asp:Label ID="lbl_reimburseID" runat="server" Text='<%# Eval("reimburseID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="noTran" HeaderText="<%$ Resources:Text, NoOfTransactions %>"
                                            SortExpression="noTran">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>" SortExpression="totalAmount">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountClaimed" runat="server" Text='<%# Eval("totalAmount")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, ReimbursementCompletionTime %>"
                                            ShowHeader="False"
                                            SortExpression="createDate">
                                            <ItemStyle Width="130px" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtn_submissionDate" runat="server" CausesValidation="false"
                                                    CommandName="" Text='<%# Eval("createDate") %>' Visible="false"></asp:LinkButton>
                                                <asp:Label ID="lbl_submissionDate" runat="server" Text='<%# Eval("createDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PaymentFileGenerationTime %>"
                                            SortExpression="completionTime">
                                            <ItemStyle Width="130px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompletionTime" runat="server" Text='<%# Eval("completionTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, DPAReport %>" SortExpression="lineNum">
                                            <ItemTemplate>
                                                <table style="width: 100%">
                                                    <div runat="server" id="div_EHCP">
                                                    <tr>
                                                        <td>
                                                            <asp:Image id ="imgPrinter_EHCP" AlternateText="<%$ Resources:AlternateText, ReprintDPAReportEHCPBtn %>" ImageUrl="<%$ Resources:ImageUrl, SmallPrinter %>" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton id="lbtnPrint_EHCPRpt" Text="<%$ Resources:Text, DPAReportEHCP %>" CommandName="ReprintDPAReportEHCP" CausesValidation="false"  runat="server"/>      
                                                        </td>
                                                    </tr>
                                                    </div>
                                                    <div runat="server" id="div_Practice">
                                                    <tr>
                                                        <td>
                                                           <asp:Image id ="imgPrinter_Practice" AlternateText="<%$ Resources:AlternateText, ReprintDPAReportPracticeBtn %>" ImageUrl="<%$ Resources:ImageUrl, SmallPrinter %>" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton id="lbtn_PrintPracticeRpt" Text="<%$ Resources:Text, DPAReportPractice %>" CommandName="ReprintDPAReportPractice" CausesValidation="false"  runat="server"/>
                                                        </td>
                                                    </tr>
                                                    </div>
                                                </table>                                             
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" Width="120px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBack_Click" />
                                        </td>
                                    </tr>
                                </table>          
                            </asp:View>
                            <asp:View ID="ViewDrillSPID" runat="server" EnableTheming="False">
                                <asp:GridView ID="gvDrillSPID" runat="server" EnableTheming="True" AutoGenerateColumns="False"
                                    OnRowDataBound="gvDrillSPID_RowDataBound" OnRowCommand="gvDrillSPID_RowCommand"
                                    AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvDrillSPID_PageIndexChanging"
                                    OnPreRender="gvDrillSPID_PreRender" OnSorting="gvDrillSPID_Sorting">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_selected" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, ServiceProviderIDStar %>" ShowHeader="False"
                                            SortExpression="spNum">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtn_spID" runat="server" CausesValidation="false" CommandName=""
                                                    Text='<%# Eval("spNum") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="spName" HeaderText="<%$ Resources:Text, ServiceProviderName %>"
                                            SortExpression="spName" />
                                        <asp:BoundField DataField="noTran" HeaderText="<%$ Resources:Text, NoOfTransactions %>"
                                            SortExpression="noTran">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="totalAmount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                            DataFormatString="{0:#,###}" HtmlEncode="False" SortExpression="totalAmount">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:Label ID="lblSPIDSPIDDesc" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                                <br />
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnSPIDBackToAuth" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>" OnClick="ibtnSPIDBackToAuth_Click"
                                                Visible="False" />
                                            <asp:ImageButton ID="ibtnSPIDBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnSPIDBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewDrillBankAccount" runat="server">
                                <asp:GridView ID="gvDrillBankAccount" runat="server" AutoGenerateColumns="False"
                                    OnRowCommand="gvDrillBankAccount_RowCommand" OnRowDataBound="gvDrillBankAccount_RowDataBound"
                                    AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvDrillBankAccount_PageIndexChanging"
                                    OnPreRender="gvDrillBankAccount_PreRender" OnSorting="gvDrillBankAccount_Sorting">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("blockNo") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_selected" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="bankAccount" HeaderText="<%$ Resources:Text, BankAccountNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtn_bankAccNo" runat="server" CausesValidation="false" CommandName=""></asp:LinkButton>
                                                <asp:Label ID="lblOriBank" runat="server" Text='<%# Eval("bankAccount") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="practice" HeaderText="<%$ Resources:Text, Practice %>"
                                            SortExpression="practice"></asp:BoundField>
                                        <asp:BoundField DataField="noTran" HeaderText="<%$ Resources:Text, NoOfTransactions %>"
                                            SortExpression="noTran">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="totalAmount" DataFormatString="{0:#,###}" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                            HtmlEncode="False" SortExpression="totalAmount">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:Label ID="lblBankAccountSPIDDesc" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                                <br />
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left">
                                            <asp:ImageButton ID="ibtnBankAccountBackToAuth" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>" OnClick="ibtnBankAccountBackToAuth_Click"
                                                Visible="False" />
                                            <asp:ImageButton ID="ibtnBankAccountBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBankAccountBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewDrillTransaction" runat="server">
                                <asp:GridView ID="gvDrillTransaction" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="True" AllowSorting="True" OnRowCommand="gvDrillTransaction_RowCommand"
                                    OnRowDataBound="gvDrillTransaction_RowDataBound" OnPageIndexChanging="gvDrillTransaction_PageIndexChanging"
                                    OnSorting="gvDrillTransaction_Sorting" OnPreRender="gvDrillTransaction_PreRender">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_selected" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>" ShowHeader="False"
                                            SortExpression="transNum">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtn_transNum" runat="server" CausesValidation="false" CommandName=""
                                                    Text='<%# Eval("transNum") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionDate %>" SortExpression="transDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionTime" runat="server" Text='<%# Eval("transDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ServiceProvider" HeaderText="<%$ Resources:Text, ServiceProviderName %>"
                                            Visible="False">
                                            <ItemStyle Width="120px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="spID" HeaderText="<%$ Resources:Text, ServiceProviderID %>"
                                            Visible="False"></asp:BoundField>
                                        <asp:BoundField DataField="bankAccount" HeaderText="<%$ Resources:Text, BankAccountNo %>"
                                            Visible="False">
                                            <ItemStyle Width="120px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="practice" HeaderText="<%$ Resources:Text, Practice %>"
                                            Visible="False">
                                            <ItemStyle Width="80px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VoucherValue" DataFormatString="{0:#,###}" HeaderText="<%$ Resources:Text, ValuePerVoucherSign %>"
                                            HtmlEncode="False" SortExpression="VoucherValue" Visible="false">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="totalAmount" DataFormatString="{0:#,###}" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                            HtmlEncode="False" SortExpression="totalAmount">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="transstatus" HeaderText="<%$ Resources:Text, TransactionStatus %>"
                                            Visible="False"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:Label ID="lblTransactionSPIDDesc" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                                <br />
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left">
                                            <asp:ImageButton ID="ibtnTransactionBackToAuth" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>" OnClick="ibtnTransactionBackToAuth_Click"
                                                Visible="False" />
                                            <asp:ImageButton ID="ibtnTransactionBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnTransactionBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewTranDetails" runat="server">
                                <uc3:ClaimTransDetail ID="ClaimTransDetail1" runat="server" />
                                <asp:HiddenField ID="hfCurrentDetailTransactionNo" runat="server" />
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:ImageButton ID="ibtnDetailBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnDetailBack_Click" />
                                        </td>
                                        <td>
                                            <center>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ibtnDetailPrevious" runat="server" ImageUrl="<%$ Resources:ImageUrl, PreviousRecordBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, PreviousRecordBtn %>" OnClick="ibtnDetailPrevious_Click" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:Panel ID="panRecordNo" runat="server" BorderStyle="Inset" Width="90px">
                                                                <asp:Label ID="lblCurrentRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                                <asp:Label ID="lblSlash" runat="server" Font-Size="Large" Text=" / "></asp:Label>
                                                                <asp:Label ID="lblMaxRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="ibtnDetailNext" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextRecordBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, NextRecordBtn %>" OnClick="ibtnDetailNext_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
                        <asp:Button Style="display: none" ID="btnHiddenDownload" runat="server" />
            <cc1:ModalPopupExtender ID="mpeDownload" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenDownload" PopupControlID="pnlDownload" RepositionMode="None"
                PopupDragHandleControlID="pnlDownloadHeading" />
            <asp:Panel ID="pnlDownload" runat="server" Style="display: none" defaultbutton="ibtnDownload">
                <div id="ctl00_ContentPlaceHolder1_pnlDownloadHeading" style="cursor: move">

                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="padding-left: 2px; font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDownloadTitle" runat="server" Text="<%$ Resources:Text, DownloadLatestReport %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position: relative; left: -2px"></td>
                        </tr>
                    </table>

                </div>

                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="background-color: #ffffff">
                            <br />
                            <cc2:MessageBox ID="udcDownloadErrorMessage" runat="server" Width="800px" />
                            <cc2:InfoMessageBox ID="udcDownloadInfoMessage" runat="server" Width="800px" />
                            <table cellpadding="0" cellspacing="0" style="width: 800px">
                                <tr>
                                    <td style="width: 150px; height: 36px" valign="top">
                                        <asp:Label ID="lblReportTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReportType %>" />
                                    </td>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblReportType" runat="server" CssClass="tableText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblNewPassword" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SetPassword %>" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="15" TextMode="Password" Width="200px" />
                                                    <asp:Image ID="imgErrNewPassword" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                                </td>
                                                <td valign="top">
                                                    <table cellpadding="0" style="width: 290px">
                                                        <tr>
                                                            <td colspan="5">
                                                                <div id="progressBar" style="border-right: white 1px solid; border-top: white 1px solid; font-size: 1px; border-left: white 1px solid; width: 290px; border-bottom: white 1px solid; height: 10px">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 290px">
                                                            <td align="center" style="width: 30%">
                                                                <span id="strength1"></span>
                                                            </td>
                                                            <td style="width: 5%">
                                                                <span id="direction1"></span>
                                                            </td>
                                                            <td align="center" style="width: 30%">
                                                                <span id="strength2"></span>
                                                            </td>
                                                            <td style="width: 5%">
                                                                <span id="direction2"></span>
                                                            </td>
                                                            <td align="center" style="width: 30%">
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
                                    <td valign="top" />
                                    <td valign="top">
                                        <asp:Label ID="lblFilePasswordTipsHeader" runat="server" Text="<%$ Resources:Text, FileDownloadPasswordTips %>" />
                                        <br />
                                        <asp:Label ID="lblFilePasswordTips1" runat="server" Text="<%$ Resources:Text, WebPasswordTips1-3Rule %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1a" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1b" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1c" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1d" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>" />
                                        <br />
                                        <asp:Label ID="lblFilePasswordTips2" runat="server" Text="<%$ Resources:Text, FilePasswordTips2 %>" />
                                        <br />
                                        <asp:Label ID="lblFilePasswordTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" valign="top" style="padding-top: 15px; padding-bottom: 10px">

                                        <asp:ImageButton ID="ibtnDownload" name="ibtnDownload" runat="server" ImageUrl="../Images/button/btn_download.png" alt="Download" Style="border-width: 0px;" />
                                        &nbsp;&nbsp;&nbsp;    
                                        <input type="image" name="ctl00$ContentPlaceHolder1$ibtnDownloadClose" id="ctl00_ContentPlaceHolder1_ibtnDownloadClose" src="../Images/button/btn_close.png" alt="Close" style="border-width: 0px;" />


                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position: relative; left: -2px"></td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
            if (args.get_error() && args.get_error().name == 'Sys.WebForms.PageRequestManagerTimeoutException') {
                //alert('Caught a timeout!'); 
                // remember to set errorHandled = true to keep from getting a popup from the AJAX library itself 
                args.set_errorHandled(true);
            }
        });
    </script>

</asp:Content>
