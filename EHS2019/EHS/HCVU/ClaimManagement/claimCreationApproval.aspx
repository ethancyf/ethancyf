<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="claimCreationApproval.aspx.vb" Inherits="HCVU.claimCreationApproval"
    EnableEventValidation="false" Title="<%$ Resources:Title, ReimbursementClaimCreationApproval %>" %>

<%@ Register Src="~/Reimbursement/ClaimTransDetail.ascx" TagName="ClaimTransDetail" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="300">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <asp:Image ID="imgClaimTransEnquiry" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClaimCreationApprovalBanner %>"
                AlternateText="<%$ Resources:AlternateText, ClaimCreationApprovalBanner %>" />
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:MessageBox ID="udcErrorMessage" runat="server" Visible="False" Width="95%"></cc2:MessageBox>
                <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
            </asp:Panel>
            <asp:Panel ID="panClaimTransEnquiry" runat="server">
                <asp:MultiView ID="MultiViewClaimTransEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwSearch" runat="server">
                        <table>
                            <tr style="height: 30px">
                                <td style="vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, ServiceProviderID %>" /></td>
                                </td>
                                <td style="vertical-align: top; width: 291px">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceProviderSPID" runat="server" Width="176" MaxLength="8"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Image ID="imgServiceProviderSPIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top; width: 178px"></td>
                                <td style="vertical-align: top; width: 235px"></td>
                            </tr>
                            <tr style="height: 30px">
                                <td style="vertical-align: top; width: 25%">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, ServiceProviderNameInEnglish %>" /></td>
                                </td>
                                <td style="vertical-align: top; width: 25%">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceProviderSPName" runat="server" Width="176" onChange="convertToUpper(this)"
                                                    ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>" MaxLength="40" />
                                            </td>
                                            <td>
                                                <asp:Image ID="imgServiceProviderSPNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top; width: 25%">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, ServiceProviderNameInChinese %>" maxlength="6" /></td>
                                </td>
                                <td style="vertical-align: top; width: 25%">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceProviderSPChiName" runat="server" Width="176" onChange="convertToUpper(this)"
                                                    MaxLength="6" />
                                            </td>
                                            <td>
                                                <asp:Image ID="imgServiceProviderSPChiNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </tr>
                            <tr style="height: 30px">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTabServiceProviderTypeOfDateText" runat="server" Text="<%$ Resources: Text, TypeOfDate %>"></asp:Label></td>
                                <td style="vertical-align: top" colspan="3">
                                    <asp:RadioButtonList ID="rblTabServiceProviderTypeOfDate" Style="height: 22px; position: relative; left: -4px; top: -4px" runat="server" AppendDataBoundItems="false" RepeatDirection="Horizontal">
                                        <asp:ListItem style="position: relative; top: -3px" Text="<%$ Resources:Text, ServiceDate %>" Value="SD" />
                                        <asp:ListItem style="position: relative; top: -3px" Text="<%$ Resources:Text, TransactionDateVU %>" Value="TD" Selected="True" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>

                            <tr style="height: 30px">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblServiceProviderDateText" runat="server" Text="<%$ Resources: Text, Date %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceProviderDateFrom" runat="server" Width="70" MaxLength="10" Style="position: relative; top: -4px"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnServiceProviderCalenderDateFrom" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: -1px" />
                                                <cc1:CalendarExtender ID="CalExtServiceProviderDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnServiceProviderCalenderDateFrom"
                                                    TargetControlID="txtServiceProviderDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="TxtExtServiceProviderDateFrom" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtServiceProviderDateFrom" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 30px; text-align: center">
                                                <asp:Label ID="lblTabServiceProviderToText" runat="server" Text="<%$ Resources: Text, To_S %>" Style="position: relative; top: -4px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceProviderDateTo" runat="server" Width="70" MaxLength="10" Style="position: relative; top: -4px"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnServiceProviderCalenderDateTo" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: -1px" />
                                                <cc1:CalendarExtender ID="CalExtServiceProviderDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnServiceProviderCalenderDateTo"
                                                    TargetControlID="txtServiceProviderDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="TxtExtServiceProviderDateTo" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtServiceProviderDateTo" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Image ID="imgServiceProviderDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -5px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblServiceProviderSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:DropDownList ID="ddlServiceProviderScheme" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 180px; position: relative; top: -4px"
                                        AutoPostBack="True">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                        <div style="text-align: center; margin-top: 10px">
                            <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/Images/button/btn_search.png" />
                        </div>
                        <br />
                        <br />
                    </asp:View>
                    <asp:View ID="ViewTransaction" runat="server">

                        <uc4:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panSearchCriteriaReview" />

                        <asp:Panel ID="panSearchCriteriaReview" runat="server" Width="1000px">
                            <table style="width: 917px">
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top"></td>
                                    <td style="vertical-align: top"></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInEnglish %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPName" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPChiNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInChinese %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPChiName" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRDateText" runat="server"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRDate" runat="server" CssClass="tableText"></asp:Label></td>
                                     <td style="vertical-align: top">
                                        <asp:Label ID="lblRSchemeCodeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                     </td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSchemeCode" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" /><asp:GridView ID="gvTransaction" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="gvTransaction_PageIndexChanging"
                                OnPreRender="gvTransaction_PreRender" OnSorting="gvTransaction_Sorting" AllowPaging="True" Width="1085px"
                                AllowSorting="true" OnRowDataBound="gvTransaction_RowDataBound" OnRowCommand="gvTransaction_RowCommand">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="10px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>" ShowHeader="False"
                                        SortExpression="transNum">
                                        <ItemStyle VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="white-space: nowrap">
                                                        <asp:LinkButton ID="lbtn_transNum" runat="server" CausesValidation="false" CommandName=""
                                                            Text='<%# Eval("transNum") %>'></asp:LinkButton></td>
                                                    <td align="right" style="padding-left: 5px;">
                                                        <asp:Image ID="imgOverride" runat="server" ImageUrl="<%$ Resources:ImageUrl, Override %>"
                                                            AlternateText="<%$ Resources:AlternateText, Override %>" /></td>
                                                </tr>
                                            </table>
                                            <asp:HiddenField ID="hfTransactionNo" runat="server" Value='<%# Eval("transNum") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Service_Receive_Dtm" HeaderText="<%$ Resources:Text, ServiceDate %>">
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle Width="110px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceReceiveDtm" runat="server" Text='<%# Eval("Service_Receive_Dtm") %>'></asp:Label><br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="ServiceProvider" HeaderText="<%$ Resources:Text, ServiceProviderName %> ">
                                        <ItemStyle Width="270px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblREname" runat="server" Text='<%# Eval("ServiceProvider") %>'></asp:Label><br />
                                            <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("spChiName") %>' CssClass="TextGridChi"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, SPIDPracticeID %>" SortExpression="spid_practiceid">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SPID") %>'></asp:Label>
                                            (<asp:Label ID="lblPracticeID" runat="server" Text='<%# Eval("practiceid") %>'></asp:Label>)
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="practice" HeaderText="<%$ Resources:Text, Practice %>"
                                        SortExpression="practice">
                                        <ItemStyle Width="125px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" SortExpression="Display_Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'></asp:Label>
                                            <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="45px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="VoucherRedeem" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>"
                                    SortExpression="voucherRedeem">
                                    <ItemStyle Width="15px" VerticalAlign="Top" />
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="75px" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:BoundField>--%>
                                    <asp:TemplateField SortExpression="totalAmount" HeaderText='<%$ Resources:Text, TotalRedeemAmountSign %>'>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("totalAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="90px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Creation_Reason" HeaderText="<%$ Resources:Text, CreationReason %>">
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle Width="180px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreationReason" runat="server" Text='<%# Eval("Creation_Reason") %>'></asp:Label><br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField Visible="false" DataField="Create_By" HeaderText="<%$ Resources:Text, CreateBy %>"
                                        SortExpression="Create_By">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="75px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:BoundField>
                                    <asp:TemplateField SortExpression="Create_Dtm" HeaderText="<%$ Resources:Text, CreationTime %>">
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle Width="110px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreateDtm" runat="server" Text='<%# Eval("Create_Dtm") %>'></asp:Label><br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--
                                   <asp:BoundField DataField="Create_Dtm" HeaderText="<%$ Resources:Text, CreateDtm %>"
                                    SortExpression="Create_Dtm">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="75px" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:BoundField>                  
                                    --%>
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
                    <asp:View ID="ViewDetail" runat="server">
                        <asp:Panel ID="panDetail" runat="server" Width="950px">
                            <uc1:ClaimTransDetail ID="udcClaimTransDetail" runat="server" />
                            <br />
                            <asp:HiddenField ID="HiddenFieldAction" runat="server" />
                            <asp:HiddenField ID="hfCurrentDetailTransactionNo" runat="server" />
                        </asp:Panel>
                        <asp:MultiView ID="MultiViewDetailAction" runat="server" ActiveViewIndex="0">
                            <asp:View ID="ViewApproveReview" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:ImageButton ID="ibtnApproveReviewBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnApproveReviewBack_Click" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnApprove" runat="server" ImageUrl="<%$ Resources:ImageUrl, ApproveBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ApproveBtn %>" OnClick="ibtnApprove_Click" />
                                            <asp:ImageButton ID="ibtnReject" runat="server" ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, RejectBtn %>" OnClick="ibtnReject_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewInputReason" runat="server">
                                <table style="width: 100%" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="vertical-align: top; width: 200px">
                                            <asp:Label ID="lblReasonText" runat="server"></asp:Label>
                                        </td>
                                        <td style="vertical-align: top">
                                            <asp:TextBox ID="txtReason" runat="server" Width="300px" CssClass="TextBoxChi"></asp:TextBox>
                                            <asp:Image ID="imgAlertReason" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr style="height: 10px">
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnInputReasonBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnInputReasonBack_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnInputReasonSave" runat="server" ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, SaveBtn %>" OnClick="ibtnInputReasonSave_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewConfirmReason" runat="server">
                                <table style="width: 100%" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td style="width: 1%">
                                        </td>
                                        <td style="vertical-align: top; width: 200px">
                                            <asp:Label ID="lblConfirmReasonText" runat="server"></asp:Label>
                                        </td>
                                        <td style="vertical-align: top">
                                            <asp:Label ID="lblConfirmReason" runat="server" Width="300px" CssClass="TextChi"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px">
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnConfirmReasonBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnConfirmReasonBack_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnConfirmReasonConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnConfirmReasonConfirm_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                        <%-- Pop up for Confirm Approve --%>
                        <asp:Button ID="btnHiddenApprove" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="popupApprove" runat="server" TargetControlID="btnHiddenApprove"
                            PopupControlID="panApprove" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                            RepositionMode="None" PopupDragHandleControlID="panApproveHeading">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="panApprove" runat="server" Style="display: none;">
                            <asp:Panel ID="panApproveHeading" runat="server" Style="cursor: move;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
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
                                                <td align="left" style="width: 40px; height: 42px" valign="middle">
                                                    <asp:Image ID="imgApprove" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                                <td align="center" style="height: 42px">
                                                    <asp:Label ID="lblApprove" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmApprove %>"></asp:Label></td>
                                                <td style="width: 40px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:ImageButton ID="ibtnApproveConfirmPopup" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnApproveConfirm_Click" />
                                                    <asp:ImageButton ID="ibtnApproveCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnApproveCancel_Click" /></td>
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
                        <%-- End of Pop up for Confirm Approve --%>
                        <%-- Pop up for Confirm Reject --%>
                        <asp:Button ID="btnHiddenReject" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="popupReject" runat="server" TargetControlID="btnHiddenReject"
                            PopupControlID="panReject" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                            RepositionMode="None" PopupDragHandleControlID="panRejectHeading">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="panReject" runat="server" Style="display: none;">
                            <asp:Panel ID="panRejectHeading" runat="server" Style="cursor: move;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
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
                                                <td align="left" style="width: 40px; height: 42px" valign="middle">
                                                    <asp:Image ID="imgReject" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                                <td align="center" style="height: 42px">
                                                    <asp:Label ID="lblReject" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmRejectQ %>"></asp:Label></td>
                                                <td style="width: 40px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:ImageButton ID="ibtnRejectConfirmPopup" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnRejectConfirm_Click" />
                                                    <asp:ImageButton ID="ibtnRejectCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnRejectCancel_Click" /></td>
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
                        <%-- End of Pop up for Confirm Reject --%>
                    </asp:View>
                    <asp:View ID="ViewFinish" runat="server">
                        <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToSearchResultBtn %>"
                            AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnReturn_Click" />
                        <br />
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
