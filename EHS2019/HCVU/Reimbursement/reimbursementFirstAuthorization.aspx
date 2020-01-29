<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="reimbursementFirstAuthorization.aspx.vb" Inherits="HCVU.reimbursement_new"
    Title="<%$ Resources:Title, ReimbursementFirstAuthorization %>" %>

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

    <asp:ScriptManager ID="smScriptManager" runat="server" AsyncPostBackTimeout="300">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Image ID="imgReimFirstAuthorization" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReimbursementFirstAuthorizationBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReimbursementFirstAuthorization %>">
            </asp:Image>
            <asp:Panel ID="panSearchCriteriaReview" runat="server" Width="100%">
                <table width="90%" class="TableStyle1 ">
                    <tr>
                        <td>
                            <asp:Label ID="lblSCRReimbursementCutoffDateText" runat="server" Text="<%$ Resources:Text, ReimbursementCutoffDate %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRReimbursementCutoffDate" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSCRSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRScheme" runat="server" CssClass="tableText"></asp:Label>
                            <asp:HiddenField ID="hfSchemeCode" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblSCRReimburseIDText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCRReimburseID" runat="server" CssClass="tableText"></asp:Label>
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
                <asp:View ID="vSelectScheme" runat="server">
                    <table style="width: 800px">
                        <tbody>
                            <tr>
                                <td>
                                    <table style="width: 750px" cellspacing="0" cellpadding="0">
                                        <tbody>
                                            <tr>
                                                <td style="width: 220px; height: 30px; vertical-align: top">
                                                    <asp:Label ID="lblReimCutoffDateText" runat="server" Width="190px" Text="<%$ Resources:Text, ReimbursementCutoffDate %>"></asp:Label>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblReimCutoffDate" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:HiddenField ID="hfReimbursementID" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px; height: 30px; vertical-align: top">
                                                    <asp:Label ID="lblSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlScheme" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                        Enabled="False" OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="<%$ Resources:Text, PleaseSelect %>" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Image ID="imgAlertScheme" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ibtnSearchAndHold" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchNHoldTranDisableBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, SearchNHoldTransactionBtn %>" OnClick="ibtnSearchAndHold_Click"
                                    Enabled="False"></asp:ImageButton>
                                <asp:ImageButton ID="ibtnContinue" runat="server" ImageUrl="<%$ Resources:ImageUrl, ContinueDisableBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ContinueDisableBtn %>" OnClick="ibtnContinue_Click"
                                    Enabled="False"></asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vGroupByReimburseID" runat="server">
                    <br />
                    <asp:GridView ID="gvGroupByReimburseID" runat="server" OnRowDataBound="gvGroupByReimburseID_RowDataBound"
                        OnRowCommand="gvGroupByReimburseID_RowCommand" AutoGenerateColumns="False" AllowPaging="True"
                        AllowSorting="True" OnPageIndexChanging="gvGroupByReimburseID_PageIndexChanging"
                        OnPreRender="gvGroupByReimburseID_PreRender" OnSorting="gvGroupByReimburseID_Sorting">
                        <Columns>
                            <asp:TemplateField ShowHeader="False" Visible="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_transNum" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Eval("details") %>' Width="60px"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="60px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, ReimbursementID %>" SortExpression="reimburseID">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_reimburseID" runat="server" CausesValidation="false" CommandName=""
                                        Text='<%# Eval("reimburseID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="reimburseID" Visible="False" HeaderText="<%$ Resources:Text, ReimbursementID %>" />
                            <asp:BoundField DataField="noTran" HeaderText="<%$ Resources:Text, NoOfTransactions %>"
                                SortExpression="noTran">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="noSP" HeaderText="<%$ Resources:Text, NoOfServiceProviderStar %>"
                                SortExpression="noSP">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="totalAmount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                DataFormatString="{0:#,###}" HtmlEncode="False" SortExpression="totalAmount">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="TotalAmountRMB">
                                <ItemTemplate>
                                    <asp:Label ID="lblGTotalAmountRMB" runat="server" Text='<%# Eval("TotalAmountRMB") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblRSPIDRemark" runat="server" Text="<%$ Resources:Text, SPIDDesc %>"></asp:Label><br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:ImageButton ID="ibtnRBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnRBack_Click" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="ibtnRPrintReport" runat="server" AlternateText="<%$ Resources:AlternateText, PrintReportBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, PrintReportBtn %>" OnClick="ibtnRPrintReport_Click" />
                                <asp:ImageButton ID="ibtnRConfirmFirstAuthorization" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmFirstAuthorizationBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ConfirmFirstAuthorizationBtn %>"
                                    OnClick="ibtnRConfirmFirstAuthorization_Click" />
                                <asp:ImageButton ID="ibtnRRelease" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReleaseBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ReleaseBtn %>" OnClick="ibtnRRelease_Click" />
                            </td>
                        </tr>
                    </table>
                    <%-- Pop up for Confirm Release --%>
                    <asp:Button ID="btnHiddenRelease" runat="server" Style="display: none" />
                    <cc1:ModalPopupExtender ID="popupRelease" runat="server" TargetControlID="btnHiddenRelease"
                        PopupControlID="panRelease" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                        RepositionMode="None" PopupDragHandleControlID="panReleaseHeading">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="panRelease" runat="server" Style="display: none;">
                        <asp:Panel ID="panReleaseHeading" runat="server" Style="cursor: move;">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                    </td>
                                    <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                        color: #ffffff; background-repeat: repeat-x; height: 35px">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label>
                                    </td>
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
                                                <asp:Image ID="imgRelease" runat="server" ImageUrl="~/Images/others/questionMark.png" />
                                            </td>
                                            <td align="center" style="height: 42px">
                                                <asp:Label ID="lblRelease" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmReleaseTransaction %>"></asp:Label>
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:ImageButton ID="ibtnReleaseConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnReleaseConfirm_Click" />
                                                <asp:ImageButton ID="ibtnReleaseCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnReleaseCancel_Click" />
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
                                    height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                                    background-position-x: right; height: 7px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%-- End of Pop up for Confirm Release --%>
                    <%-- Pop up for Confirm First Authorization --%>
                    <asp:Button ID="btnHiddenShowDialog" runat="server" Style="display: none" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmAuthorize" runat="server" TargetControlID="btnHiddenShowDialog"
                        PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panConfirmMsgHeading">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                        <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/top-left.png); width: 8px; height: 35px">
                                    </td>
                                    <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                        color: #ffffff; background-repeat: repeat-x; height: 35px">
                                        <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label>
                                    </td>
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
                                        <tr style="height: 42px">
                                            <td align="left" style="width: 40px" valign="middle">
                                                <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmFirstAuthorization %>"></asp:Label>
                                            </td>
                                            <td style="width: 40px"></td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:ImageButton ID="ibtnFirstAuthorizeConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnFirstAuthorizeConfirm_Click" />
                                                <asp:ImageButton ID="ibtnFirstAuthorizeCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnFirstAuthorizeCancel_Click" />
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
                                    height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-right.png); background-position-x: right;
                                    width: 7px; height: 7px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%-- End of Pop up for Confirm First Authorization --%>
                </asp:View>
                <asp:View ID="vGroupBySP" runat="server">
                    <br />
                    <asp:GridView ID="gvGroupBySP" runat="server" OnRowDataBound="gvGroupBySP_RowDataBound"
                        OnRowCommand="gvGroupBySP_RowCommand"
                        AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvGroupBySP_PageIndexChanging"
                        OnPreRender="gvGroupBySP_PreRender" OnSorting="gvGroupBySP_Sorting">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, ServiceProviderIDStar %>" ShowHeader="False"
                                SortExpression="spNum">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_transNum" runat="server" CausesValidation="false" CommandName=""
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
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="TotalAmountRMB">
                                <ItemTemplate>
                                    <asp:Label ID="lblGTotalAmountRMB" runat="server" Text='<%# Eval("TotalAmountRMB") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblSSPIDRemark" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnSBackToAuthorizePage" runat="server" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>" OnClick="ibtnBackToAuthorizePage_Click_Shared" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vGroupByBankAccount" runat="server">
                    <br />
                    <asp:GridView ID="gvGroupByBankAccount" runat="server" OnRowCommand="gvGroupByBankAccount_RowCommand"
                        AutoGenerateColumns="False" OnRowDataBound="gvGroupByBankAccount_RowDataBound"
                        AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvGroupByBankAccount_PageIndexChanging"
                        OnPreRender="gvGroupByBankAccount_PreRender"
                        OnSorting="gvGroupByBankAccount_Sorting">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("blockNo") %>' Visible="false"></asp:Label>
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
                                SortExpression="practice" />
                            <asp:BoundField DataField="noTran" HeaderText="<%$ Resources:Text, NoOfTransactions %>"
                                SortExpression="noTran">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="totalAmount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                DataFormatString="{0:#,###}" HtmlEncode="False" SortExpression="totalAmount">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="TotalAmountRMB">
                                <ItemTemplate>
                                    <asp:Label ID="lblGTotalAmountRMB" runat="server" Text='<%# Eval("TotalAmountRMB") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
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
                                <asp:ImageButton ID="ibtnBBackToAuthorizePage" runat="server" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>" OnClick="ibtnBackToAuthorizePage_Click_Shared" />
                                <asp:ImageButton ID="ibtnBBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vGroupByTransaction" runat="server">
                    &nbsp;<br />
                    <asp:GridView ID="gvGroupByTransaction" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        OnPageIndexChanging="gvGroupByTransaction_PageIndexChanging" OnRowCommand="gvGroupByTransaction_RowCommand"
                        OnRowDataBound="gvGroupByTransaction_RowDataBound" AllowSorting="True" OnSorting="gvGroupByTransaction_Sorting"
                        OnPreRender="gvGroupByTransaction_PreRender">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
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
                            <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>" SortExpression="transNum"
                                ShowHeader="False">
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
                            <asp:BoundField DataField="ServiceProvider" HeaderText="Service Provider Name" Visible="False">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="spID" HeaderText="Service Provider ID" Visible="False" />
                            <asp:BoundField DataField="bankAccount" HeaderText="Bank Account No." Visible="False" />
                            <asp:BoundField DataField="practice" HeaderText="Practice" Visible="False">
                                <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VoucherValue" HeaderText="<%$ Resources:Text, ValuePerVoucherSign %>"
                                SortExpression="VoucherValue" DataFormatString="{0:#,###}" Visible="False">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="totalAmount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>"
                                SortExpression="totalAmount" DataFormatString="{0:#,###}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, AmountClaimedRMB %>" SortExpression="TotalAmountRMB">
                                <ItemTemplate>
                                    <asp:Label ID="lblGTotalAmountRMB" runat="server" Text='<%# Eval("TotalAmountRMB") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblTSPIDRemark" runat="server" Text='<%$ Resources:Text, SPIDDesc %>'></asp:Label>
                    <br />
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnTBackToAuthorizePage" runat="server" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackToAuthorizePageBtn %>" OnClick="ibtnBackToAuthorizePage_Click_Shared" />
                                <asp:ImageButton ID="ibtnTBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnTBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vTransactionDetail" runat="server">
                    <uc3:ClaimTransDetail ID="ClaimTransDetail1" runat="server" />
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 305px">
                                <asp:ImageButton ID="ibtnDBackToAuthorizedPage" runat="server" AlternateText="<%$ Resources:AlternateText, BackToAuthorizePage %>"
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
                                            <asp:Panel ID="panDRecordNo" runat="server" BorderStyle="Inset" Width="90px">
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
                <asp:View ID="vComplete" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="btn_Return" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnCReturn_Click" />
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
