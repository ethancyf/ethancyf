<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="reimbursementCancelAuthorization.aspx.vb" Inherits="HCVU.reimbursement_void"
    Title="<%$ Resources:Title, ReimbursementCancelAuthorization %>" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="ClaimTransDetail.ascx" TagName="ClaimTransDetail" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript">
        function SelectAll(id) {
            var frm = document.forms[0];
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox") {
                    frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="300">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Image ID="imgReimCancelAuthorization" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReimbursementCancelBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReimbursementCancelBanner %>"></asp:Image>
            <cc2:MessageBox ID="udcErrorBox" runat="server" Width="95%"></cc2:MessageBox>
            <cc2:InfoMessageBox ID="udcInfoBox" runat="server" Width="95%"></cc2:InfoMessageBox>
            <asp:Panel ID="pnlDrillCriteriaReview" runat="server" Width="100%">
                <table width="90%">
                    <tr>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRSchemeCodeText" runat="server" Text="<%$ Resources:Text, Scheme %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRSchemeCode" runat="server" CssClass="tableText" Visible="False"></asp:Label>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRReimIDText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRReimID" runat="server" CssClass="tableText" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRFirstAuthDtmText" runat="server" Text="<%$ Resources:Text, FirstAuthorizedTime %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRFirstAuthDtm" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRFirstAuthByText" runat="server" Text="<%$ Resources:Text, FirstAuthorizedBy %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRFirstAuthBy" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRSecondAuthDtmText" runat="server" Text="<%$ Resources:Text, SecondAuthorizedTime %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRSecondAuthDtm" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRSecondAuthByText" runat="server" Text="<%$ Resources:Text, SecondAuthorizedBy %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRSecondAuthBy" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
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
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRBankAcctText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 25%; vertical-align: top">
                            <asp:Label ID="lblRBankAcct" runat="server" CssClass="tableText" Visible="False"></asp:Label>
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
            <asp:MultiView ID="MultiViewReimCancelAuthorization" runat="server" ActiveViewIndex="0">
                <asp:View ID="ViewNoRecord" runat="server">
                </asp:View>
                <asp:View ID="ViewTransactionSummary" runat="server" EnableTheming="False">
                    <div class="headingText">
                        <asp:Label ID="lblTransactionSummaryHeading" runat="server" Text="<%$ Resources:Text, ReimbursementAuthorizationSummary %>"></asp:Label>
                    </div>
                    <asp:GridView ID="gvTransactionSummary" runat="server" EnableTheming="True" AutoGenerateColumns="False"
                        AllowPaging="True" AllowSorting="True" OnRowDataBound="gvTransactionSummary_RowDataBound"
                        OnRowCommand="gvTransactionSummary_RowCommand" OnPreRender="gvTransactionSummary_PreRender"
                        OnSorting="gvTransactionSummary_Sorting" OnPageIndexChanging="gvTransactionSummary_PageIndexChanging"
                        OnSelectedIndexChanged="gvTransactionSummary_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="HeaderLevelCheckBox" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_selected" runat="server" />
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" SortExpression="Display_Code">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSchemeCode" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Trim(Eval("Display_Code")) %>' />
                                    <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>' Visible="false"></asp:Label>
                                    <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>'>
                                    </asp:HiddenField>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, ReimbursementID %>" SortExpression="Reimburse_ID">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnReimID" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Eval("Reimburse_ID") %>' Visible="false"></asp:LinkButton>
                                    <asp:Label ID="lblreimburseID" runat="server" Text='<%# Eval("Reimburse_ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AuthorizationTime %>" SortExpression="Authorised_Dtm">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnAuthorisedDtm" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Eval("Authorised_Dtm") %>' Visible="false"></asp:LinkButton>
                                    <asp:Label ID="lblAuthorisedDtm" runat="server" Text='<%# Eval("Authorised_Dtm") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="130px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AuthorizedBy %>" SortExpression="Authorised_By">
                                <ItemTemplate>
                                    <asp:Label ID="lblAuthorisedBy" runat="server" Text='<%# Eval("Authorised_By") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, NoOfTransactions %>" SortExpression="Total_Transaction">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalTransaction" runat="server" Text='<%# Eval("Total_Transaction") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>" SortExpression="Total_Voucher">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalVoucher" runat="server" Text='<%# Eval("Total_Voucher") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>" SortExpression="Total_Amount">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="Total_Amount_RMB">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("Total_Amount_RMB") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AuthorizedStatus %>" SortExpression="Authorised_Status">
                                <ItemStyle VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAuthorisedStatus" runat="server" Text='<%# Eval("Authorised_Status") %>'></asp:Label>
                                    <asp:HiddenField ID="hfAuthorisedStatus" runat="server" Value='<%# Eval("Authorised_Status") %>'>
                                    </asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Panel ID="pnlReason" runat="server" Width="100%" Visible="false">
                        <table>
                            <tr>
                                <td style="width: 150px">
                                    <asp:Label ID="lblReasonText" runat="server" Text="<%$ Resources:Text, EnterCancelReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReason" runat="server" Width="300px" CssClass="TextBoxChi"></asp:TextBox>
                                    <asp:Image ID="imgAlertReason" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ibtnCancelAuthorization" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelAuthorizationBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelAuthorizationBtn %>" OnClick="ibtnCancelAuthorization_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewConfirmCancel" runat="server">
                    <asp:GridView ID="gvConfirmCancel" runat="server" EnableTheming="True" AutoGenerateColumns="False"
                        OnRowDataBound="gvConfirmCancel_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblIndex" runat="server" Text='<%# Eval("Index") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="HeaderLevelCheckBox" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_selected" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" SortExpression="Scheme_Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'></asp:Label>
                                    <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>'>
                                    </asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, ReimbursementID %>" SortExpression="Reimburse_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblReimID" runat="server" Text='<%# Eval("Reimburse_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AuthorizationTime %>" SortExpression="Authorised_Dtm">
                                <ItemStyle Width="130px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAuthorisedDtm" runat="server" Text='<%# Eval("Authorised_Dtm") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AuthorizedBy %>" SortExpression="Authorised_By">
                                <ItemTemplate>
                                    <asp:Label ID="lblAuthorisedBy" runat="server" Text='<%# Eval("Authorised_By") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, NoOfTransactions %>" SortExpression="Total_Transaction">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalTransaction" runat="server" Text='<%# Eval("Total_Transaction") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>" SortExpression="Total_Voucher">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalVoucher" runat="server" Text='<%# Eval("Total_Voucher") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>" SortExpression="Total_Amount">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="Total_Amount_RMB">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("Total_Amount_RMB") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionStatus %>" SortExpression="Authorised_Status">
                                <ItemStyle Width="120px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAuthorisedStatus" runat="server" Text='<%# Eval("Authorised_Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <table>
                        <tr>
                            <td style="width: 150px">
                                <asp:Label ID="lblConfirmReasonText" runat="server" Text="<%$ Resources:Text, CancelReason %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblConfirmReason" runat="server" CssClass="TextChi"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%">
                        <tr style="height: 20px">
                        </tr>
                        <tr>
                            <td style="width: 5%; text-align: left">
                                <asp:ImageButton ID="ibtnReasonBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnReasonBack_Click" />
                            </td>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ibtnReasonConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnReasonConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewComplete" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnReturn" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewDrillSPID" runat="server" EnableTheming="False">
                    <asp:GridView ID="gvDrillSPID" runat="server" EnableTheming="True" AutoGenerateColumns="False"
                        AllowPaging="True" AllowSorting="True" OnRowDataBound="gvDrillSPID_RowDataBound"
                        OnRowCommand="gvDrillSPID_RowCommand" OnPageIndexChanging="gvDrillSPID_PageIndexChanging"
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
                            <%--<asp:BoundField DataField="vouchersClaimed" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>"
                                SortExpression="vouchersClaimed">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="totalAmount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                DataFormatString="{0:#,###}" HtmlEncode="False" SortExpression="totalAmount">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblSPSPIDDesc" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: left">
                                <asp:ImageButton ID="ibtnSPBackToAuth" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToCancelAuthorizeBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackToCancelAuthorizeBtn %>" OnClick="ibtnSPBackToAuth_Click">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnSPBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" Visible="False" OnClick="ibtnSPBack_Click">
                                </asp:ImageButton></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewDrillBankAccount" runat="server">
                    <asp:GridView ID="gvDrillBankAccount" runat="server" AutoGenerateColumns="False"
                        AllowPaging="True" AllowSorting="True" OnRowDataBound="gvDrillBankAccount_RowDataBound"
                        OnRowCommand="gvDrillBankAccount_RowCommand" OnPageIndexChanging="gvDrillBankAccount_PageIndexChanging"
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
                            <%--<asp:BoundField DataField="vouchersClaimed" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>"
                                SortExpression="vouchersClaimed">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="totalAmount" DataFormatString="{0:#,###}" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                HtmlEncode="False" SortExpression="totalAmount">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblBankSPIDDesc" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnBankBackToAuth" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToCancelAuthorizeBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackToCancelAuthorizeBtn %>" OnClick="ibtnBankBackToAuth_Click">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnBankBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBankBack_Click">
                                </asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewDrillTransaction" runat="server">
                    <asp:GridView ID="gvDrillTransaction" runat="server" AutoGenerateColumns="False"
                        AllowPaging="True" AllowSorting="True" OnRowDataBound="gvDrillTransaction_RowDataBound"
                        OnRowCommand="gvDrillTransaction_RowCommand" OnPageIndexChanging="gvDrillTransaction_PageIndexChanging"
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
                            <%--<asp:BoundField DataField="VoucherRedeem" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>"
                                SortExpression="VoucherRedeem">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="VoucherValue" DataFormatString="{0:#,###}" HeaderText="<%$ Resources:Text, ValuePerVoucherSign %>"
                                HtmlEncode="False" SortExpression="VoucherValue" Visible="false">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="totalAmount" DataFormatString="{0:#,###}" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                HtmlEncode="False" SortExpression="totalAmount">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="transstatus" HeaderText="<%$ Resources:Text, TransactionStatus %>"
                                Visible="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblTransSPIDDesc" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnTransBackToAuth" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToCancelAuthorizeBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackToCancelAuthorizeBtn %>" OnClick="ibtnTransBackToAuth_Click">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnTransBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnTransBack_Click">
                                </asp:ImageButton></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewTranDetails" runat="server">
                    <uc3:ClaimTransDetail ID="udcClaimTransDetail" runat="server"></uc3:ClaimTransDetail>
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnDetailBackToAuth" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToCancelAuthorizeBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackToCancelAuthorizeBtn %>" OnClick="ibtnDetailBackToAuth_Click">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnDetailBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnDetailBack_Click" />
                            </td>
                            <td style="text-align: center">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnDetailPrevious" runat="server" AlternateText="<%$ Resources:AlternateText, PreviousRecordBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, PreviousRecordBtn %>" OnClick="ibtnDetailPrevious_Click" />
                                        </td>
                                        <td>
                                            <asp:Panel ID="pnlDetailRecordNo" runat="server" BorderStyle="Inset" Width="90px">
                                                <asp:Label ID="lblCurrentRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                <asp:Label ID="lblSlash" runat="server" Font-Size="Large" Text="/"></asp:Label>
                                                <asp:Label ID="lblMaxRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnDetailNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextRecordBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, NextRecordBtn %>" OnClick="ibtnDetailNext_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <asp:HiddenField ID="hfReimID" runat="server" />
            <asp:HiddenField ID="hfAuthorisedStatus" runat="server" />
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
