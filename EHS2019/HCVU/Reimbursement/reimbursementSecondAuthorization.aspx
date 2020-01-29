<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="reimbursementSecondAuthorization.aspx.vb" Inherits="HCVU.reimbursementSecondAuthorization"
    Title="<%$ Resources:Title, ReimbursementSecondAuthorization %>" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="ClaimTransDetail.ascx" TagName="ClaimTransDetail" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        table.TableStyle1 td {
            vertical-align: top;
            width: 25%;
        }
    </style>
</asp:Content>

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
            <asp:Image ID="img_banner_level1" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReimbursementSecondAuthorizationBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReimbursementSecondAuthorization %>">
            </asp:Image>
            <asp:Panel ID="panSearchCriteriaReview" runat="server" Width="100%">
                <table width="90%" class="TableStyle1">
                    <tr>
                        <td>
                            <asp:Label ID="lblSCRSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRScheme" runat="server" CssClass="tableText"></asp:Label>
                            <asp:HiddenField ID="hfSchemeCode" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblSCRReimbursementIDText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRReimbursementID" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSCRFirstAuthorizedTimeText" runat="server" Text="<%$ Resources:Text, FirstAuthorizedTime %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRFirstAuthorizedTime" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRFirstAuthorizedByText" runat="server" Text="<%$ Resources:Text, FirstAuthorizedBy %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRFirstAuthorizedBy" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSCRSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRSPName" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderIDStar %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRSPID" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSCRBankAccountText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRBankAccount" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRPractice" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc2:MessageBox ID="udcErrorMessage" runat="server" Visible="False" Width="95%">
            </cc2:MessageBox>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Visible="False" Width="95%">
            </cc2:InfoMessageBox>
            <asp:MultiView ID="mvCore" runat="server" OnActiveViewChanged="mvCore_ActiveViewChanged">
                <asp:View ID="vNoRecord" runat="server">
                </asp:View>
                <asp:View ID="vGroupByScheme" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblSTitle" runat="server" Text="<%$ Resources:Text, SecondAuthorizationSummaryTitle %>"></asp:Label>
                    </div>
                    <asp:GridView ID="gvGroupByScheme" runat="server" EnableTheming="True" AutoGenerateColumns="False"
                        OnRowCommand="gvGroupByScheme_RowCommand" OnRowDataBound="gvGroupByScheme_RowDataBound"
                        OnSelectedIndexChanged="gvGroupByScheme_SelectedIndexChanged" AllowPaging="True"
                        AllowSorting="True" OnPageIndexChanging="gvGroupByScheme_PageIndexChanging" OnPreRender="gvGroupByScheme_PreRender"
                        OnSorting="gvGroupByScheme_Sorting">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" SortExpression="Display_Code">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_SchemeCode" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Trim(Eval("Display_Code")) %>' />
                                    <asp:Label ID="lbl_SchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'
                                        Visible="false"></asp:Label>
                                    <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, ReimbursementID %>" SortExpression="reimburseID">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_reimburseID" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Eval("reimburseID") %>' Visible="false" />
                                    <asp:Label ID="lbl_reimburseID" runat="server" Text='<%# Eval("reimburseID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, FirstAuthorizedTime %>" SortExpression="v1Date">
                                <ItemStyle Width="130px" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_v1Date" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Eval("v1Date") %>' Visible="false" />
                                    <asp:Label ID="lbl_v1Date" runat="server" Text='<%# Eval("v1Date") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, FirstAuthorizedBy %>" SortExpression="v1By">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_v1By" runat="server" Text='<%# Eval("v1By") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="noTran" HeaderText="<%$ Resources:Text, NoOfTransactions %>"
                                SortExpression="noTran">
                                <ItemStyle HorizontalAlign="Right" Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="noSP" HeaderText="<%$ Resources:Text, NoOfServiceProviderStar %>"
                                SortExpression="noSP">
                                <ItemStyle HorizontalAlign="Right" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="totalAmount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                DataFormatString="{0:#,###}" HtmlEncode="False" SortExpression="totalAmount">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                <ItemTemplate>
                                    <asp:Label ID="lblGAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblSPIDDescActionPage" runat="server" Text="<%$ Resources:Text, SPIDDesc %>"></asp:Label><br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ibtnSConfirmSecondAuthorization" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmSecondAuthorizationBtn %>"
                                    OnClick="ibtnSConfirmSecondAuthorization_Click" AlternateText="<%$ Resources:AlternateText, ConfirmSecondAuthorizationBtn %>" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vGroupBySP" runat="server">
                    <asp:GridView ID="gvGroupBySP" runat="server" EnableTheming="True" AutoGenerateColumns="False"
                        OnRowDataBound="gvGroupBySP_RowDataBound" OnRowCommand="gvGroupBySP_RowCommand"
                        AllowPaging="True" AllowSorting="True"
                        OnPageIndexChanging="gvGroupBySP_PageIndexChanging"
                        OnPreRender="gvGroupBySP_PreRender" OnSorting="gvGroupBySP_Sorting">
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
                                <ItemTemplate>
                                    <asp:Label ID="lblGAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblSPIDDesc" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnSBackToAuthorizePage" OnClick="ibtnBackToAuthorizePage_Click_Shared"
                                    runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>">
                                </asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vGroupByBankAccount" runat="server">
                    <asp:GridView ID="gvGroupByBankAccount" runat="server" AutoGenerateColumns="False"
                        OnRowCommand="gvGroupByBankAccount_RowCommand"
                        OnRowDataBound="gvGroupByBankAccount_RowDataBound" AllowPaging="True" AllowSorting="True"
                        OnPageIndexChanging="gvGroupByBankAccount_PageIndexChanging" OnPreRender="gvGroupByBankAccount_PreRender"
                        OnSorting="gvGroupByBankAccount_Sorting">
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
                                <ItemTemplate>
                                    <asp:Label ID="lblGAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblBSPIDRemark" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnBBackToAuthorizedPage" OnClick="ibtnBackToAuthorizePage_Click_Shared"
                                    runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnBBack" runat="server" OnClick="ibtnBBack_Click"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" AlternateText="<%$ Resources:AlternateText, BackBtn %>">
                                </asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vGroupByTransaction" runat="server">
                    <asp:GridView ID="gvGroupByTransaction" runat="server" AutoGenerateColumns="False"
                        OnRowCommand="gvGroupByTransaction_RowCommand" OnRowDataBound="gvGroupByTransaction_RowDataBound"
                        AllowPaging="True" OnPageIndexChanging="gvGroupByTransaction_PageIndexChanging"
                        AllowSorting="True"
                        OnSorting="gvGroupByTransaction_Sorting" OnPreRender="gvGroupByTransaction_PreRender">
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
                                <ItemTemplate>
                                    <asp:Label ID="lblGAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="transstatus" HeaderText="<%$ Resources:Text, TransactionStatus %>"
                                Visible="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblTSPIDRemark" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnTBackToAuthorizePage" OnClick="ibtnBackToAuthorizePage_Click_Shared"
                                    runat="server" ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnTBack" runat="server" OnClick="ibtnTBack_Click"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" AlternateText="<%$ Resources:AlternateText, BackBtn %>">
                                </asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vTransactionDetail" runat="server">
                    <uc3:ClaimTransDetail ID="ClaimTransDetail1" runat="server"></uc3:ClaimTransDetail>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 306px">
                                <asp:ImageButton ID="ibtnDBackToAuthorizePage" runat="server" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>" OnClick="ibtnBackToAuthorizePage_Click_Shared" />
                                <asp:ImageButton ID="ibtnDBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td align="center" valign="middle">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnDPreviousRecord" runat="server" AlternateText="<%$ Resources:AlternateText, PreviousRecordBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, PreviousRecordBtn %>" OnClick="ibtnDPreviousRecord_Click" />
                                        </td>
                                        <td align="center">
                                            <asp:Panel ID="panRecordNo" runat="server" BorderStyle="Inset" Width="90px">
                                                <asp:Label ID="lbl_recordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                <asp:Label ID="lblDSlash" runat="server" Font-Size="Large">/</asp:Label>
                                                <asp:Label ID="lbl_recordMax" runat="server" Font-Size="Medium"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnDNextRecord" runat="server" AlternateText="<%$ Resources:AlternateText, NextRecordBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, NextRecordBtn %>" OnClick="ibtnDNextRecord_Click" />
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vConfirmSecondAuthorization" runat="server">
                    <asp:GridView ID="gvConfirmSecondAuthorization" runat="server" EnableTheming="True"
                        AutoGenerateColumns="False" OnRowDataBound="gvConfirmSecondAuthorization_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="lineNum" />
                            <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" SortExpression="Display_Code">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_SchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'></asp:Label>
                                    <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="reimburseID" HeaderText="<%$ Resources:Text, ReimbursementID %>"
                                SortExpression="ReimburseID" />
                            <asp:TemplateField HeaderText="<%$ Resources:Text, FirstAuthorizedTime %>" SortExpression="v1Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblFirstAuthorizedTime" runat="server" Text='<%# Eval("v1Date") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="v1By" HeaderText="<%$ Resources:Text, FirstAuthorizedBy %>" />
                            <asp:BoundField DataField="noTran" HeaderText="<%$ Resources:Text, NoOfTransactions %>">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="noSP" HeaderText="<%$ Resources:Text, NoOfServiceProviderStar %>"
                                SortExpression="noSP">
                                <ItemStyle HorizontalAlign="Right" Width="100px" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="vouchersClaimed" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="totalAmount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                DataFormatString="{0:#,###}" HtmlEncode="False">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="totalAmountRMB">
                                <ItemTemplate>
                                    <asp:Label ID="lblGAmountClaimedRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblCSPIDRemark" runat="server" Text="<%$ Resources:Text, SPIDDesc %>"></asp:Label><br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 100px">
                                <asp:ImageButton ID="ibtnCBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnCBack_Click" />
                            </td>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ibtnCConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnCConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vComplete" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>

                                <asp:ImageButton ID="ibtnCompleteReturn" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnCompleteReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vError" runat="server">
                    <asp:ImageButton ID="ibtnErrorBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnErrorBack_Click" />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
