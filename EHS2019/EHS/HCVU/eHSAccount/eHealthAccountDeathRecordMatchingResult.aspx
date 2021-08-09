<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="eHealthAccountDeathRecordMatchingResult.aspx.vb" Inherits="HCVU.eHealthAccountDeathRecordMatchingResult"
    Title="<%$ Resources: Title, eHealthAccountDeathRecordMatchingResult %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumentType"
    TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/Token/ucInputToken.ascx" TagName="ucInputToken" TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript">
        function SelectAll(id) {    
            var grid = document.getElementById("<%= gvMatchResult.ClientID %>");
            var cell;
            
            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];
                    
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type =="checkbox") {
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, eHealthAccountDeathRecordMatchingResultBanner %>"
        AlternateText="<%$ Resources: AlternateText, eHealthAccountDeathRecordMatchingResultBanner %>">
    </asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="975px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="975px" />
            <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vSearchCriteria" runat="server">
                    <%--[S]--%>
                    <asp:Panel ID="panS" runat="server" DefaultButton="ibtnSSearch">
                        <table>
                            <tr style="height: 1px">
                                <td style="width: 160px">
                                </td>
                                <td style="width: 240px">
                                </td>
                                <td style="width: 160px">
                                </td>
                                <td style="width: 240px">
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td>
                                    <asp:Label ID="lblSDocumentTypeText" runat="server" Text="<%$ Resources: Text, DocumentType %>"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlSDocumentType" runat="server" Width="588" AppendDataBoundItems="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td>
                                    <asp:Label ID="lblSDocumentNoText" runat="server" Text="<%$ Resources: Text, IdentityDocNo %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSDocumentNo" runat="server" Width="174" onChange="convertToUpper(this)"
                                        MaxLength="20"></asp:TextBox>
                                    <asp:Image ID="imgSDocumentNo" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Visible="False" />
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td>
                                    <asp:Label ID="lblSAccountTypeText" runat="server" Text="<%$ Resources: Text, AccountType %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSAccountType" runat="server" Width="180" AppendDataBoundItems="False"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlSAccountType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSAccountStatusText" runat="server" Text="<%$ Resources: Text, AccountStatus %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSAccountStatus" runat="server" Width="180" AppendDataBoundItems="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td>
                                    <asp:Label ID="lblSWithClaimText" runat="server" Text="<%$ Resources: Text, WithClaims %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSWithClaim" runat="server" Width="180" AppendDataBoundItems="False"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlSWithClaim_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSWithSuspiciousClaimText" runat="server" Text="<%$ Resources: Text, WithSuspiciousClaims %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSWithSuspiciousClaim" runat="server" Width="180" AppendDataBoundItems="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 25px">
                                <td>
                                    <asp:Label ID="lblSNameMatchText" runat="server" Text="<%$ Resources: Text, NameMatched %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSNameMatch" runat="server" Width="180" AppendDataBoundItems="False">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSYearOfBirthText" runat="server" Text="<%$ Resources: Text, YearOfBirth %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSYearOfBirthFrom" runat="server" Width="74" MaxLength="4"></asp:TextBox>
                                    <asp:Label ID="lblSYearOfBirthTo" runat="server" Text="<%$ Resources: Text, To_S %>"></asp:Label>
                                    <asp:TextBox ID="txtSYearOfBirthTo" runat="server" Width="74" MaxLength="4"></asp:TextBox>
                                    <asp:Image ID="imgSYearOfBirth" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Visible="False" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredYearOfBirthFrom" runat="server" FilterType="Numbers"
                                        TargetControlID="txtSYearOfBirthFrom" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredYearOfBirthTo" runat="server" FilterType="Numbers"
                                        TargetControlID="txtSYearOfBirthTo" />
                                </td>
                            </tr>
                            <tr style="height: 25px">
                            </tr>
                            <tr>
                                <td style="text-align: center" colspan="4">
                                    <asp:ImageButton ID="ibtnSSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, SearchBtn %>" OnClick="ibtnSSearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:Label ID="lblSNote" runat="server" Text="<%$ Resources: Text, DeathRecordMatchingNote %>"></asp:Label>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vSearchResult" runat="server">
                    <%--[R]--%>
                    
                    <uc3:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panSearchCriteriaReview" />
                    
                    <asp:Panel ID="panSearchCriteriaReview" runat="server">
                        <table>
                            <tr style="height: 1px">
                                <td style="width: 120px">
                                </td>
                                <td style="width: 240px">
                                </td>
                                <td style="width: 170px">
                                </td>
                                <td style="width: 240px">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRDocumentTypeText" runat="server" Text="<%$ Resources: Text, DocumentType %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRDocumentType" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRDocumentNoText" runat="server" Text="<%$ Resources: Text, IdentityDocNo %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRDocumentNo" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRAccountTypeText" runat="server" Text="<%$ Resources: Text, AccountType %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRAccountType" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRAccountStatusText" runat="server" Text="<%$ Resources: Text, AccountStatus %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRAccountStatus" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRWithClaimText" runat="server" Text="<%$ Resources: Text, WithClaims %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRWithClaim" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRWithSuspiciousClaimText" runat="server" Text="<%$ Resources: Text, WithSuspiciousClaims %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRWithSuspiciousClaim" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRNameMatchText" runat="server" Text="<%$ Resources: Text, NameMatched %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRNameMatch" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRYearOfBirthText" runat="server" Text="<%$ Resources: Text, YearOfBirth %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRYearOfBirth" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 990px">
                        <tr style="height: 20px">
                            <td style="text-align: right">
                                <asp:CheckBox ID="cboMaskDocumentNo" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                    AutoPostBack="True" OnCheckedChanged="cboMaskDocumentNo_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvMatchResult" runat="server" Width="990px" AutoGenerateColumns="False"
                                    AllowPaging="True" AllowSorting="True" OnRowDataBound="gvMatchResult_RowDataBound"
                                    OnRowCommand="gvMatchResult_RowCommand" OnPageIndexChanging="gvMatchResult_PageIndexChanging"
                                    OnSorting="gvMatchResult_Sorting" OnPreRender="gvMatchResult_PreRender">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle VerticalAlign="Top" />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="HeaderLevelCheckBox" runat="server" />
                                            </HeaderTemplate>
                                            <ItemStyle Width="20px" VerticalAlign="Top" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cboSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, AccountType %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="Account_Type">
                                            <ItemStyle Width="100px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGAccountType" runat="server" Text='<%# Eval("Account_Type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="Account_ID">
                                            <ItemStyle Width="130px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnGAccountID" runat="server" CausesValidation="False" CommandName=""
                                                    Text='<%# Eval("Account_ID_F") %>'></asp:LinkButton>
                                                <asp:HiddenField ID="hfGAccountID" runat="server" Value='<%# Eval("Account_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, DocumentType %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="Document_Code">
                                            <ItemStyle Width="80px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDocumentType" runat="server" Text='<%# Eval("Document_Code") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, IdentityDocNo %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="Document_No_F">
                                            <ItemStyle Width="120px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDocumentNo" runat="server" Text='<%# Eval("Document_No_F") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, IdentityDocNo %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="Document_No_FM">
                                            <ItemStyle Width="120px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDocumentNoM" runat="server" Text='<%# Eval("Document_No_FM") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, DOB %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="DOB">
                                            <ItemStyle Width="100px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDOB" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, DateOfDeath %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="DOD">
                                            <ItemStyle Width="100px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDOD" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, AccountStatus %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="Account_Status_F">
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGAccountStatus" runat="server" Text='<%# Eval("Account_Status_F") %>'></asp:Label>
                                                <asp:HiddenField ID="hfGAccountStatus" runat="server" Value='<%# Eval("Account_Status") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, WithSuspiciousClaims %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="With_Suspicious_Claim">
                                            <ItemStyle Width="110px" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGWithSuspiciousClaim" runat="server" Text='<%# Eval("With_Suspicious_Claim") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px">
                                <table>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:ImageButton ID="ibtnRBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnRBack_Click" />
                                        </td>
                                        <td style="width: 600px; text-align: center">
                                            <asp:ImageButton ID="ibtnRManageSelectedAccount" runat="server" ImageUrl="<%$ Resources: ImageUrl, ManageSelectedAccountsBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ManageSelectedAccountsBtn %>" OnClick="ibtnRManageSelectedAccount_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vDetail" runat="server">
                    <%--[D]--%>
                    <table>
                        <tr style="height: 0px">
                            <td style="width: 180px">
                            </td>
                            <td style="width: 250px">
                            </td>
                            <td style="width: 180px">
                            </td>
                            <td style="width: 250px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div class="headingText">
                                                <asp:Label ID="lblDEHealthAccountInformation" runat="server" Text="<%$ Resources: Text, VoucherAccountInfo %>"></asp:Label>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cboDMaskDocumentNo" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                                AutoPostBack="True" OnCheckedChanged="cboDMaskDocumentNo_CheckedChanged" />
                                            <asp:HiddenField ID="hfDMaskDocumentNo" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <uc1:ucReadOnlyDocumentType ID="udcReadOnlyDocumentType" runat="server">
                                </uc1:ucReadOnlyDocumentType>
                                <asp:HiddenField ID="hfDEHealthAccountID" runat="server" />
                                <asp:HiddenField ID="hfDEHealthAccountDocumentType" runat="server" />
                                <asp:HiddenField ID="hfDAccountType" runat="server" />
                            </td>
                        </tr>
                        <tr style="height: 5px">
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="headingText">
                                    <asp:Label ID="lblDDeathRecordInformation" runat="server" Text="<%$ Resources: Text, DeathRecordInformation %>"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px">
                                <asp:Label ID="lblDNameText" runat="server" Text="<%$ Resources: Text, Name %>"></asp:Label>
                            </td>
                            <td style="width: 100px; word-wrap: break-word; word-break:break-all">
                                <asp:Label ID="lblDName" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Image ID="imgDName" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                    ToolTip="<%$ Resources: Text, NotMatchedWithTheNameInEhealthAccount %>" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px">
                                <asp:Label ID="lblDDateOfDeathText" runat="server" Text="<%$ Resources: Text, DateOfDeath %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDDateOfDeath" runat="server" CssClass="tableText"></asp:Label>
                                <asp:HiddenField ID="hfDDateOfDeathType" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblDDateOfRegistrationText" runat="server" Text="<%$ Resources: Text, DateOfRegistration %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDDateOfRegistration" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px">
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="headingText">
                                    <asp:Label ID="lblDMatchingInformation" runat="server" Text="<%$ Resources: Text, MatchingInformation %>"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px">
                                <asp:Label ID="lblDMatchTimeText" runat="server" Text="<%$ Resources: Text, LastUpdateTime %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDMatchTime" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px">
                                <asp:Label ID="lblDWithClaimText" runat="server" Text="<%$ Resources: Text, WithClaims %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDWithClaim" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDWithSuspiciousClaimText" runat="server" Text="<%$ Resources: Text, WithSuspiciousClaims %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDWithSuspiciousClaim" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px">
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2">
                                <div class="headingText">
                                    <asp:Label ID="lblDTransactionInformation" runat="server" Text="<%$ Resources: Text, TransactionInformation %>"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 180px; padding-left: 4px; vertical-align: top">
                                <asp:Label ID="lblDSuspiciousTransactionText" runat="server" Text="<%$ Resources: Text, SuspiciousTransaction %>"></asp:Label>
                                <asp:Label ID="lblDTransactionText" runat="server" Text="<%$ Resources: Text, Transaction %>"></asp:Label>
                            </td>
                            <td style="width: 700px; vertical-align: top">
                                <asp:Label ID="lblDNoTransaction" runat="server" CssClass="tableText" Text="<%$ Resources: Text, NIL %>"></asp:Label>
                                <asp:GridView ID="gvDST" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                    AllowSorting="False" OnRowDataBound="gvDST_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, Scheme %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" />
                                            <ItemStyle VerticalAlign="Top" CssClass="GridViewItemPadding" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionNo %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" />
                                            <ItemStyle VerticalAlign="Top" CssClass="GridViewItemPadding" />
                                            <ItemTemplate>
                                                <cc2:CustomLinkButton ID="clbtnGTransactionNo" runat="server"></cc2:CustomLinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionStatus %>" HeaderStyle-VerticalAlign="Top"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" />
                                            <ItemStyle VerticalAlign="Top" CssClass="GridViewItemPadding" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGTransactionStatus" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td style="width: 150px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td style="width: 600px; text-align: center">
                                <asp:ImageButton ID="ibtnDRecheck" runat="server" ImageUrl="<%$ Resources: ImageUrl, RecheckAndUpdateMatchingInformationBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RecheckAndUpdateMatchingInformationBtn %>"
                                    OnClick="ibtnDRecheck_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vAccountActionFinish" runat="server">
                    <%--[AF]--%>
                    <table>
                        <tr>
                            <td style="padding-top: 10px">
                                <asp:ImageButton ID="ibtnAFReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnAFReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <%-- Pop up for Unmask --%>
            <asp:Button ID="btnHiddenUnmask" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupUnmask" runat="server" TargetControlID="btnHiddenUnmask"
                PopupControlID="panUnmask" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panUnmask" runat="server" Style="display: none">
                <uc2:ucInputToken ID="udcPUInputToken" runat="server"></uc2:ucInputToken>
            </asp:Panel>
            <%-- End of Pop up for Unmask --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
