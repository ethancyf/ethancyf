<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="monthlystatement.aspx.vb" Inherits="HCSP.monthlystatement" Title="<%$ Resources:Title, MonthlyStatement %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/SchemeLegend.ascx" TagName="SchemeLegend" TagPrefix="uc1" %>
<%@ Register Src="../ClaimTranEnquiry.ascx" TagName="ClaimTranEnquiry" TagPrefix="uc2" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:Image ID="imgHeaderMonthyStatement" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, MonthlyReimbursementStatementBanner %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="780px" />
                        <asp:MultiView ID="MultiViewMonthlyStatement" runat="server" ActiveViewIndex="0">
                            <asp:View ID="ViewSummary" runat="server">
                                <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr style="height: 40px">
                                        <td colspan="2">
                                            <asp:Label ID="lblMonthlyStatement" runat="server" CssClass="tableCaption" Text="<%$ Resources:Text, MonthlyStatementSummary %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td style="width: 185px">
                                            <asp:Label ID="lblBankAccountText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Practice %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBankAccount" runat="server" Width="350px" DataTextField="Display_Text"
                                                DataValueField="PracticeID" AutoPostBack="true" OnSelectedIndexChanged="ddlBankAccount_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px">
                                        <td>
                                            <asp:Label ID="lblStatementText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Statement %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatement" runat="server" Width="350px" DataTextField="Display_Text"
                                                DataValueField="Reimburse_ID" AutoPostBack="True" OnSelectedIndexChanged="ddlStatement_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px">
                                        <td>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl='<%$ Resources:ImageUrl, SearchBtn %>'
                                                AlternateText='<%$ Resources:AlternateText, SearchBtn %>' />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="panSummary" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Width="650px" HorizontalAlign="Center" Visible="False">
                                    <table cellpadding="0" cellspacing="0" style="width: 600px">
                                        <tr style="height: 15px">
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; padding-left: 30px">
                                                <table cellpadding="0" cellspacing="0" style="width: 570px">
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblStatementHeader1" runat="server" CssClass="boldText" Text="<%$ Resources:Text, eHealthSystem %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblStatementHeader2" runat="server" CssClass="boldText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 30px">
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom: black 2px solid">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr style="height: 20px">
                                                                    <td style="width: 168px">
                                                                        <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>" />:
                                                                    </td>
                                                                    <td style="width: 140px">
                                                                        <asp:Label ID="lblSPID" runat="server" CssClass="boldText" />
                                                                    </td>
                                                                    <td style="width: 172px; text-align: right">
                                                                        <asp:Label ID="lblStatementIssueDateText" runat="server" Text="<%$ Resources:Text, StatementIssueDate %>" />:
                                                                    </td>
                                                                    <td style="width: 110px; text-align: right">
                                                                        <asp:Label ID="lblStatementIssueDate" runat="server" CssClass="boldText" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 20px">
                                                                    <td style="vertical-align:top;padding-top:2px">
                                                                        <asp:Label ID="lblSPName" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>" />:
                                                                    </td>
                                                                    <td colspan="3" style="vertical-align:top;padding-top:2px">
                                                                        <asp:Label ID="lblSPEngName" runat="server" CssClass="boldText" Width="422px" Style="word-wrap:break-word" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 20px">
                                                                    <td>
                                                                        <asp:Label ID="lblPracticeNoText" runat="server" Text="<%$ Resources:Text, PracticeNo %>" />:
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <asp:Label ID="lblPracticeNo" runat="server" CssClass="boldText" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 20px">
                                                                    <td style="vertical-align:top;padding-top:2px">
                                                                        <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label>:
                                                                    </td>
                                                                    <td colspan="3" style="vertical-align:top;padding-top:2px">
                                                                        <asp:Label ID="lblPracticeName" runat="server" CssClass="boldText"  Width="422px" Style="word-wrap:break-word" />
                                                                        <asp:Label ID="lblPracticeName_Chi" runat="server" CssClass="boldTextChi" Width="422px" Style="word-wrap:break-word" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 20px">
                                                                    <td>
                                                                        <asp:Label ID="lblBankAccountNoText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>" />:
                                                                    </td>
                                                                    <td colspan="3">
                                                                        <asp:Label ID="lblBankAccount" runat="server" CssClass="boldText" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 20px">
                                                                    <td style="vertical-align:top;padding-top:2px">
                                                                        <asp:Label ID="lblBankAcctName" runat="server" Text="<%$ Resources:Text, BankOwner %>" />:
                                                                    </td>
                                                                    <td colspan="3" style="vertical-align:top;padding-top:2px">
                                                                        <asp:Label ID="lblBankAccountName" runat="server" CssClass="boldText" Width="422px" Style="word-wrap:break-word" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 15px">
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 15px">
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="panDynamicStatement" runat="server">
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 15px">
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr style="height: 40px">
                                        <td>
                                            <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="<%$ Resources:ImageUrl, PrintBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, PrintBtn %>" Visible="false" />
                                            <asp:ImageButton ID="ibtnViewDetails" runat="server" ImageUrl="<%$ Resources:ImageUrl, ViewDetailsBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ViewDetailsBtn %>" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewScheme" runat="server">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr style="height: 40px">
                                        <td>
                                            <asp:Label ID="lblSClaimRecord" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, ClaimRecord %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr style="height: 30px">
                                                    <td style="width: 150px">
                                                        <asp:Label ID="lblSPracticeText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Practice %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblSPractice" runat="server" CssClass="tableText"></asp:Label>
                                                        <asp:Label ID="lblSPractice_Chi" runat="server" CssClass="tableTextChi"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 30px">
                                                    <td>
                                                        <asp:Label ID="lblSStatementText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Statement %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblSStatement" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px">
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvScheme" runat="server" AllowPaging="True" AllowSorting="true"
                                                Width="620px" BackColor="White" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle Width="20px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Display_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <HeaderStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'
                                                                CommandArgument='<%# Eval("Scheme_Code") %>'></asp:LinkButton>
                                                            <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="No_of_Transaction" HeaderText="<%$ Resources:Text, NoOfTransaction %>">
                                                        <HeaderStyle VerticalAlign="Top" />
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNoOfTransaction" runat="server" Text='<%# Eval("No_of_Transaction") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField SortExpression="Total_Amount" HeaderText="<%$ Resources:Text, TotalAmountSign %>">>--%>
                                                    <asp:TemplateField SortExpression="Total_Amount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>">                                                    
                                                        <HeaderStyle VerticalAlign="Top" />
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <%-- <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount", "{0:#,###}") %>'></asp:Label> --%>
                                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px">
                                        <td>
                                            <asp:ImageButton ID="ibtnSchemeBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnSchemeBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewTransaction" runat="server">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr style="height: 40px">
                                        <td>
                                            <asp:Label ID="lblClaimRecord" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, ClaimRecord %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr style="height: 30px">
                                                    <td style="width: 150px">
                                                        <asp:Label ID="lblTPracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblTPractice" runat="server" CssClass="tableText"></asp:Label>
                                                        <asp:Label ID="lblTPractice_Chi" runat="server" CssClass="tableTextChi"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 30px">
                                                    <td>
                                                        <asp:Label ID="lblTStatementText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Statement %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblTStatement" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 30px">
                                                    <td>
                                                        <asp:Label ID="lblTSchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblTScheme" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <asp:Panel ID="panlTSchool" runat="server">
                                                <tr style="height: 30px">
                                                    <td>
                                                        <asp:Label ID="lblTSchoolCodeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SchoolCode %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblTSchoolCode" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 30px">
                                                    <td>
                                                        <asp:Label ID="lblTSchoolNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SchoolName %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblTSchoolEngName" runat="server" CssClass="tableText"></asp:Label>
                                                        <br/>
                                                        <asp:Label ID="lblTSchoolChiName" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
												</asp:Panel>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px">
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvTransaction" runat="server" AllowPaging="True" AllowSorting="true"
                                                Width="903px" BackColor="White" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle Width="25px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClaimRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="SchemeCode_TransactionID" HeaderText="<%$ Resources:Text, TransactionNo %>">
                                                        <ItemStyle Wrap="false" Width="150px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'></asp:Label><br />
                                                            <asp:LinkButton ID="lbtnTransactionID" runat="server" CommandArgument='<%# Eval("Transaction_ID") %>'></asp:LinkButton>
                                                            <asp:HiddenField ID="hfInvalidation" runat="server" Value='<%# Eval("Invalidation") %>' />
                                                            <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Transaction_Dtm" HeaderText="<%$ Resources:Text, TransactionTime %>">
                                                        <ItemStyle Width="114px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTransactionDtm" runat="server" Text='<%# Eval("Transaction_Dtm") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Service_Receive_Dtm" HeaderText="<%$ Resources:Text, ServiceDate %>">
                                                        <ItemStyle Width="130px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceDate" runat="server" Text='<%# Eval("Service_Receive_Dtm") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Display_Code" HeaderText="<%$ Resources:Text, Scheme %>" Visible="false">
                                                        <ItemStyle Width="60px" />
                                                        <ItemTemplate>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="DocCode_IdentityNum" HeaderText="<%$ Resources:Text, DocTypeIDNL %>">
                                                        <ItemStyle Width="150px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocCode" runat="server" Text='<%# Eval("Doc_Display_Code") %>'></asp:Label><br />
                                                            <asp:Label ID="lblIDNo" runat="server"></asp:Label>
                                                            <asp:HiddenField ID="hfDocCode" runat="server" Value='<%# Eval("Doc_Code") %>' />
                                                            <asp:HiddenField ID="hfIDNo" runat="server" Value='<%# Eval("IDNo") %>' />
                                                            <asp:HiddenField ID="hfIDNo2" runat="server" Value='<%# Eval("IDNo2") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="IDNo" HeaderText="<%$ Resources:Text, IdentityDocNo %>" Visible="false">
                                                        <ItemStyle Width="100px" />
                                                        <ItemTemplate>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, Name %>">
                                                         <ItemStyle Width="230px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server"></asp:Label><br />
                                                            <asp:Label ID="lblNameChi" runat="server" Font-Names="HA_MingLiu"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- <asp:TemplateField SortExpression="Total_Unit" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>">
                                                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVoucherClaim" runat="server" Text='<%# Eval("Total_Unit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <%-- <asp:TemplateField SortExpression="Total_Amount" HeaderText="<%$ Resources:Text, TotalAmountSign %>">--%>
                                                    <asp:TemplateField SortExpression="Total_Amount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>">
                                                        <ItemStyle HorizontalAlign="Right" Width="85px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px">
                                        <td>
                                            <asp:ImageButton ID="ibtnTransactionBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnTransactionBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewDetail" runat="server">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblClaimInfo" runat="server" CssClass="tableCaption" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc2:ClaimTranEnquiry ID="udcClaimTranEnquiry" runat="server"></uc2:ClaimTranEnquiry>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="height: 40px">
                                        <td style="width: 160px">
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnDetailBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnDetailBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewSchool" runat="server">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr style="height: 40px">
                                        <td>
                                            <asp:Label ID="lblSchoolList" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, ClaimRecord %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr style="height: 30px">
                                                    <td style="width: 150px">
                                                        <asp:Label ID="lblSLPracticeText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Practice %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblSLPractice" runat="server" CssClass="tableText"></asp:Label>
                                                        <asp:Label ID="lblSLPractice_Chi" runat="server" CssClass="tableTextChi"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 30px">
                                                    <td>
                                                        <asp:Label ID="lblSLStatementText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Statement %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblSLStatement" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                 <tr style="height: 30px">
                                                    <td>
                                                        <asp:Label ID="lblSLSchemeText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Scheme %>'></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblSLScheme" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvSchool" runat="server" AllowPaging="True" AllowSorting="true"
                                                Width="750px" BackColor="White" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle Width="20px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="School_Code" HeaderText="<%$ Resources:Text, SchoolCode %>">
                                                        <HeaderStyle VerticalAlign="Top" />
                                                        <ItemStyle Width="190px" />
                                                        <ItemTemplate>                                                            
                                                            <asp:LinkButton ID="lbtSchoolCode" runat="server" Text='<%# Eval("School_Code")%>'
                                                                ></asp:LinkButton>                                                                
                                                            <%--<asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="SchoolName_Eng" HeaderText="<%$ Resources:Text, SchoolName %>">
                                                        <HeaderStyle VerticalAlign="Top" />
                                                        <ItemStyle Width="800px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbSchoolEngName" runat="server" Text='<%# Eval("SchoolName_Eng")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lbSchoolChiName" runat="server" Text='<%# Eval("SchoolName_Chi")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="No_of_Transaction" HeaderText="<%$ Resources:Text, NoOfTransaction %>">
                                                        <HeaderStyle VerticalAlign="Top" />
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNoOfTransaction" runat="server" Text='<%# Eval("No_of_Transaction")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                    
                                                    <asp:TemplateField SortExpression="Total_Amount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>">                                                    
                                                        <HeaderStyle VerticalAlign="Top" />
                                                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                                                        <ItemTemplate>                                                            
                                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px">
                                        <td>
                                            <asp:ImageButton ID="ibtnSchoolBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnSchoolBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                        <%-- Popup for Scheme Name Help --%>
                        <asp:Panel ID="panSchemeNameHelp" runat="server" Style="display: none;">
                            <asp:Panel ID="panSchemeNameHelpHeading" runat="server" Style="cursor: move;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="lblSchemeNameHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                                        <asp:Panel ID="panSchemeNameHelpContent" runat="server" ScrollBars="vertical" Height="330px">
                                            <uc1:SchemeLegend ID="udcSchemeLegend" runat="server" />
                                        </asp:Panel>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                        <asp:ImageButton ID="ibtnCloseSchemeNameHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseSchemeNameHelp_Click" /></td>
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
                        <asp:Button runat="server" ID="btnHiddenSchemeNameHelp" Style="display: none" />
                        <cc1:ModalPopupExtender ID="popupSchemeNameHelp" runat="server" TargetControlID="btnHiddenSchemeNameHelp"
                            PopupControlID="panSchemeNameHelp" BackgroundCssClass="modalBackgroundTransparent"
                            DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panSchemeNameHelpHeading">
                        </cc1:ModalPopupExtender>
                        <%-- End of Popup for Scheme Name Help --%>
                        <%-- Popup for DocType Help --%>
                        <asp:Panel ID="panDocTypeHelp" runat="server" Style="display: none;">
                            <asp:Panel ID="panDocTypeHelpHeading" runat="server" Style="cursor: move;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="lblDocTypeHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                                        <asp:Panel ID="panDocTypeContent" runat="server" ScrollBars="vertical" Height="290px">
                                            <uc3:DocTypeLegend ID="udcDocTypeLegend" runat="server" />
                                        </asp:Panel>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                        <asp:ImageButton ID="ibtnCloseDocTypeHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseDocTypeHelp_Click" /></td>
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
                        <asp:Button runat="server" ID="btnHiddenDocTypeHelp" Style="display: none" />
                        <cc1:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                            PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                            DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
                        </cc1:ModalPopupExtender>
                        <%-- End of Popup for DocType --%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
