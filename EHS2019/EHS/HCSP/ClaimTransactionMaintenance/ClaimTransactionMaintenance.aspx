<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="ClaimTransactionMaintenance.aspx.vb" Inherits="HCSP.ClaimTransactionMaintenance"
    Title="<%$ Resources: Title, ReimbursementClaimTransMgt%>" %>

<%@ Register Src="../ClaimTranEnquiry.ascx" TagName="ClaimTranEnquiry" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/SchemeLegend.ascx" TagName="SchemeLegend" TagPrefix="uc2" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc3" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Image ID="imgHeader" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources: AlternateText, ClaimTranManagementBanner %>"
                            ImageUrl="<%$ Resources: ImageURL, ClaimTranManagementBanner %>" /><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc2:InfoMessageBox ID="udcInfoMsgBox" runat="server" />
                        <cc2:MessageBox ID="udcMsgBox" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="MultiViewClaimTranManagement" runat="server">
                            <asp:View ID="ViewInputSearch" runat="server">
                                <table id="TABLE2" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td valign="top">
                                                        <asp:Label ID="lblSearchRecordText" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, SearchClaimRecord %>"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 20px">
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr style="height: 28px">
                                                                <td style="width: 200px; vertical-align: top">
                                                                    <asp:Label ID="lblSearchPracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Practice %>"></asp:Label></td>
                                                                <td style="vertical-align: top">
                                                                    <asp:DropDownList ID="ddlSearchPractice" runat="server" Width="450px" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="ddlSearchPractice_SelectedIndexChange">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 28px">
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblSearchStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Status %>"></asp:Label></td>
                                                                <td style="vertical-align: top">
                                                                    <asp:DropDownList ID="ddlSearchStatus" runat="server" Width="310px">
                                                                       <%-- <asp:ListItem Value="M">Manual Reimbursed Claim</asp:ListItem>--%>
                                                                        <asp:ListItem Value="U">Incomplete</asp:ListItem>                                                             
                                                                        <asp:ListItem Value="P">Pending Confirmation</asp:ListItem>
                                                                        <asp:ListItem Value="V">Pending Voucher Account Validation</asp:ListItem>
                                                                        <asp:ListItem Value="A">Ready to Reimburse</asp:ListItem>
                                                                        <asp:ListItem Value="R">Reimbursed</asp:ListItem>
                                                                        <asp:ListItem Value="S">Suspended</asp:ListItem>
                                                                        <asp:ListItem Value="I">Voided</asp:ListItem>  
                                                                        <asp:ListItem Value="J">Joined</asp:ListItem>                                                                                                                                                     
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 28px">
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblSearchTransactionDateText" runat="server" CssClass="tableTitle"
                                                                        Text="<%$ Resources: Text, TransactionDate %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top">
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblSearchTransactionDateFromText" runat="server" CssClass="tableText"
                                                                                                Text="<%$ Resources: Text, From %>"></asp:Label></td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtSearchTransactionDateFrom" runat="server" Width="70px" MaxLength="10"
                                                                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                                                                onblur="filterDateInput(this);"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ibtnSearchTransactionDateFrom" runat="server" ImageUrl="~/Images/button/icon_button/btn_calender.png"
                                                                                                AlternateText="Calender" ImageAlign="Middle" />
                                                                                            <cc1:CalendarExtender ID="calExtTransactionDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnSearchTransactionDateFrom"
                                                                                                TargetControlID="txtSearchTransactionDateFrom">
                                                                                            </cc1:CalendarExtender>
                                                                                            <cc1:FilteredTextBoxExtender ID="filtereditSearchTransactionDateFrom" runat="server"
                                                                                                FilterType="Custom, Numbers" TargetControlID="txtSearchTransactionDateFrom" ValidChars="-">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td style="width: 30px">
                                                                            </td>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblSearchTransactionDateToText" runat="server" CssClass="tableText"
                                                                                                Text="<%$ Resources: Text, To %>"></asp:Label></td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtSearchTransactionDateTo" runat="server" Width="70px" MaxLength="10"
                                                                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                                                                onblur="filterDateInput(this);"></asp:TextBox></td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ibtnSearchTransactionDateTo" runat="server" ImageUrl="~/Images/button/icon_button/btn_calender.png"
                                                                                                AlternateText="Calender" ImageAlign="Middle" /></td>
                                                                                        <td>
                                                                                            <cc1:CalendarExtender ID="calExtTranDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnSearchTransactionDateTo"
                                                                                                TargetControlID="txtSearchTransactionDateTo">
                                                                                            </cc1:CalendarExtender>
                                                                                            <asp:Image ID="imgSearchTransactionDateError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
                                                                                            <cc1:FilteredTextBoxExtender ID="filtereditSearchTransactionDateTo" runat="server"
                                                                                                FilterType="Custom, Numbers" TargetControlID="txtSearchTransactionDateTo" ValidChars="-">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 28px">
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblSearchTranNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, TransactionNo %>"></asp:Label></td>
                                                                <td style="vertical-align: top">
                                                                    <asp:TextBox ID="txtSearchTranNoPrefix" runat="server" Width="89px" MaxLength="7"></asp:TextBox>
                                                                    <asp:Label ID="lblSearchTranNoSep1" runat="server" Text="-"></asp:Label>
                                                                    <asp:TextBox ID="txtSearchTranNoContent" runat="server" Width="80px" MaxLength="8"></asp:TextBox>
                                                                    <asp:Label ID="lblSearchTranNoSep2" runat="server" Text="-"></asp:Label>
                                                                    <asp:TextBox ID="txtSearchTranNochkdgt" runat="server" Width="24px" MaxLength="1"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 28px">
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblSearchSchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Scheme %>"></asp:Label></td>
                                                                <td style="vertical-align: top">
                                                                    <asp:DropDownList ID="ddlScheme" runat="server" Width="430px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 10px">
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td style="vertical-align: top">
                                                                        <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="<%$ Resources: ImageURL, SearchBtn %>"
                                                                            AlternateText="<%$ Resources: AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                                                    </td>
                                                                </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewTransactionList" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 690px">
                                                <tr style="height: 28px">
                                                    <td colspan="2" style="vertical-align: top">
                                                        <asp:Label ID="lblClaimRecordText" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, ClaimRecord %>"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 28px">
                                                    <td style="width: 180px; vertical-align: top">
                                                        <asp:Label ID="lblTargetPracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Practice %>"></asp:Label></td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetPractice" runat="server" CssClass="tableText"></asp:Label>
                                                        <asp:Label ID="lblTargetPracticeChi" runat="server" CssClass="tableTextChi"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 28px">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Status %>"></asp:Label></td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 28px">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetTranDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, TransactionDate %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetTranDate" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 28px">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetTranNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, TransactionNo %>"></asp:Label></td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetTranNo" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 28px">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetSchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Scheme %>"></asp:Label></td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblTargetScheme" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvTranList" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                                AllowSorting="true" Width="1668">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle Width="20px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>" SortExpression="SchemeCode_TransactionID">
                                                        <ItemStyle Wrap="false" Width="136px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Bind("Scheme_Code") %>'></asp:Label><br />
                                                            <asp:LinkButton ID="lbtn_transactionNum" runat="server" Text='<%# Eval("Transaction_ID") %>' CommandArgument='<%# Eval("Transaction_ID") %>'></asp:LinkButton>
                                                            <%--<asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Bind("Scheme_Code") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionDate %>" SortExpression="Transaction_Dtm">
                                                        <ItemStyle Width="84px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListTranDtm" runat="server" Text='<%# Bind("Transaction_Dtm") %>'></asp:Label>
                                                            <asp:Label ID="lblTranListTranTime" runat="server"/>
                                                            <%--<asp:HiddenField ID="hfTranListTranDtm" runat="server" Value='<%# Bind("Transaction_Dtm") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionDate %>" SortExpression="Transaction_Dtm">
                                                        <ItemStyle Width="104px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListTranDtm_Chi" runat="server" Text='<%# Bind("Transaction_Dtm") %>'></asp:Label>
                                                            <asp:Label ID="lblTranListTranTime_Chi" runat="server"/>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" SortExpression="Display_Code" Visible="False">
                                                        <ItemStyle Width="70px" />
                                                        
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, DocTypeIDNL %>" SortExpression="DocCode_IdentityNum">
                                                        <ItemStyle Width="148px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocCode" runat="server" Text='<%# Bind("Doc_Display_Code") %>'></asp:Label><br />
                                                            <asp:Label ID="lblTranListHKID" runat="server"></asp:Label>
                                                            <%--<asp:HiddenField ID="hfDocCode" runat="server" Value='<%# Bind("Doc_Code") %>' />
                                                            <asp:HiddenField ID="hfTranListIDNo1" runat="server" Value='<%# Bind("IDNo") %>' />
                                                            <asp:HiddenField ID="hfTranListIDNo2" runat="server" Value='<%# Bind("IDNo2") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, IdentityDocNo %>" SortExpression="IDNo" Visible="false">
                                                        <ItemStyle Width="130px" />
                                                        <ItemTemplate>    
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Name %>" SortExpression="Eng_Name">
                                                        <ItemStyle Width="220px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListEname" runat="server" Text='<%# Bind("Eng_Name") %>'></asp:Label><br />
                                                            <asp:Label ID="lblTranListCname" runat="server" Text='<%# Bind("Chi_Name") %>' Font-Names="HA_MingLiu"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, VRAcctType %>" SortExpression="AccountType">
                                                        <ItemStyle Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListAccountType" runat="server" Text='<%# Bind("AccountType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, VRAcctType %>" SortExpression="AccountType">
                                                        <ItemStyle Width="80px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListAccountType_Chi" runat="server" Text='<%# Bind("AccountType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, VRAcctType %>" SortExpression="AccountType">
                                                        <ItemStyle Width="78px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListAccountType_CN" runat="server" Text='<%# Bind("AccountType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- <asp:BoundField DataField="Voucher_Claim" SortExpression="Voucher_Claim" HeaderText="<%$ Resources:Text, RedeemAmount %>">
                                                        <ItemStyle Width="70px" HorizontalAlign="Right" />
                                                    </asp:BoundField>--%>
                                                    <%--<asp:TemplateField SortExpression="Total_Claim_Amount" HeaderText='<%$Resources:Text, TotalAmountSign %>'>--%>                                                                            
                                                    <asp:TemplateField SortExpression="Total_Claim_Amount_RMB" HeaderText='<%$Resources:Text, TotalRedeemAmountSignRMB %>'>
                                                        <ItemStyle HorizontalAlign="Right" Width="68px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("Total_Claim_Amount_RMB") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>     
                                                    <asp:TemplateField SortExpression="Total_Claim_Amount" HeaderText='<%$Resources:Text, TotalRedeemAmountSign %>'>
                                                        <ItemStyle HorizontalAlign="Right" Width="72px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Claim_Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>  
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>" SortExpression="Record_Status">
                                                        <ItemStyle Width="175px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListTranStatusEng" runat="server" Text='<%# Bind("Record_Status") %>'></asp:Label>
                                                            <%--<asp:HiddenField ID="hfTranListTranStatusEng" runat="server" Value='<%# Bind("Record_Status") %>' />--%>
                                                            <%--<asp:HiddenField ID="hfTranListManualReimburse" runat="server" Value='<%# Bind("Manual_Reimburse") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>" SortExpression="Record_Status">
                                                        <ItemStyle Width="175px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListTranStatusChi" runat="server" Text='<%# Bind("Record_Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>" SortExpression="Record_Status">
                                                        <ItemStyle Width="175px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListTranStatusCN" runat="server" Text='<%# Bind("Record_Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, OtherInformationNL %>">
                                                        <ItemStyle Width="56px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListOtherInformation" runat="server" Text="<%$ Resources:Text, Details %>"
                                                                ForeColor="Blue"></asp:Label>
                                                            <%--<asp:HiddenField ID="hfTranListServiceType" runat="server" Value='<%# Bind("Service_Type") %>' />
                                                            <asp:HiddenField ID="hfTranListSubsidizeType" runat="server" Value='<%# Bind("Subsidize_Type") %>' />--%>
                                                            <%--<asp:HiddenField ID="hfTranListInformationCode" runat="server" Value='<%# Bind("Information_Code") %>' />
                                                            <asp:HiddenField ID="hfTranListInformationCodeChi" runat="server" Value='<%# Bind("Information_Code_Chi") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, OtherInformationNL %>">
                                                        <ItemStyle Width="56px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListOtherInformationChi" runat="server" Text="<%$ Resources:Text, Details %>"
                                                                ForeColor="Blue"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, OtherInformationNL %>">
                                                        <ItemStyle Width="56px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListOtherInformationCN" runat="server" Text="<%$ Resources:Text, Details %>"
                                                                ForeColor="Blue"></asp:Label>
                                                            <%--<asp:HiddenField ID="hfTranListInformationCodeCN" runat="server" Value='<%# Bind("Information_Code_CN") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeName %>" SortExpression="Practice_Name">
                                                        <ItemStyle Width="180px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListPracticeName" runat="server" Text='<%# Bind("Practice_Name") %>' ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeName %>" SortExpression="Practice_Name">
                                                        <ItemStyle Width="180px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListPracticeNameChi" runat="server" Text='<%# Bind("Practice_Name_Chi") %>' CssClass="textChi"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, BankAccountNo %>" SortExpression="Bank_Account_No">
                                                        <ItemStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListBankAcct" runat="server" Text='<%# Bind("Bank_Account_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="<%$ Resources:Text, DataEntry %>" SortExpression="DataEntry_By">
                                                        <ItemStyle Width="90px" />
                                                        <ItemTemplate>
                                                             <asp:Label ID="lblIsUpload" runat="server" Text='<%# Bind("IsUpload") %>'  Visible="false"></asp:Label>
                                                             <asp:Label ID="lblTranListVia" runat="server" Text='<%$ Resources:Text, Via %>' ForeColor="Brown" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTranListDataEntry_By" runat="server" Text='<%# Bind("DataEntry_By") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, DataEntry %>" SortExpression="DataEntry_By">
                                                        <ItemStyle Width="90px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListVia_Chi" runat="server" Text='Via' ForeColor="Brown"  Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTranListDataEntry_By_Chi" runat="server" Text='<%# Bind("DataEntry_By") %>'></asp:Label>
                                                            <asp:Label ID="lblInvalidation" runat="server" Text='<%# Bind("Invalidation") %>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:Text, DataEntry %>" SortExpression="DataEntry_By">
                                                        <ItemStyle Width="90px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTranListVia_CN" runat="server" Text='Via' ForeColor="Brown"  Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTranListDataEntry_By_CN" runat="server" Text='<%# Bind("DataEntry_By") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px">
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panRecordSummary" runat="server" Visible="false">
                                                <asp:Label ID="lblRecordSummaryText" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, RecordSummary %>"></asp:Label><br />
                                                <asp:Table ID="tblRecordSummary" runat="server" cellpadding="0" cellspacing="0" BorderWidth="2" style="background-color: white; text-align: center">
                                                    <asp:TableRow ID="trRecordSummaryTitle" runat="server" style="height: 30px" BorderWidth="2">
                                                        <asp:TableCell style="width: 110px" BorderWidth="2" runat="server" ID="tdSummarySchemeTitle">
                                                            <asp:Label ID="lblSummarySchemeText" runat="server" CssClass="tableTitle"
                                                                Text="<%$ Resources: Text, Scheme %>"></asp:Label></asp:TableCell>
                                                        <asp:TableCell style="width: 180px; border-top-style: none; border-left-style: none" ID="tdSummaryClaimTitle">
                                                            <asp:Label ID="lblSummaryClaimText" runat="server" CssClass="tableTitle"
                                                                Text="<%$ Resources: Text, ClaimVoucher %>"></asp:Label></asp:TableCell>
                                                       <%--  <td style="width: 140px">
                                                            <asp:Label ID="lblSummaryManualReimbursedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, ManualReimbursed %>"></asp:Label></td>--%>
                                                        <asp:TableCell style="width: 110px" BorderWidth="2" ID="tdSummaryIncompleteTitle">
                                                            <asp:Label ID="lblSummaryIncompleteText" runat="server" CssClass="tableTitle"
                                                                Text="<%$ Resources: Text, Incomplete %>"></asp:Label></asp:TableCell>
                                                        <asp:TableCell style="width: 110px" BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingComfirmText" runat="server" CssClass="tableTitle"
                                                                Text="<%$ Resources: Text, PendingConfirmation %>"></asp:Label></asp:TableCell>
                                                        <asp:TableCell style="width: 210px" BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingVRAcctValidateText" runat="server" CssClass="tableTitle"
                                                                Text="<%$ Resources: Text, PendingVoucherAccountValidation %>"></asp:Label></asp:TableCell>
                                                        <asp:TableCell style="width: 110px" BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReadyToReimburseText" runat="server" CssClass="tableTitle"
                                                                Text="<%$ Resources: Text, ReadytoReimburse %>"></asp:Label></asp:TableCell>
                                                        <asp:TableCell style="width: 110px" BorderWidth="2">
                                                            <asp:Label ID="lblSummaryVoidedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Voided %>"></asp:Label></asp:TableCell>
                                                        <asp:TableCell style="width: 110px" BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReimbursedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Reimbursed %>"></asp:Label></asp:TableCell>
                                                        <asp:TableCell style="width: 150px" BorderWidth="2">
                                                            <asp:Label ID="lblSummarySuspendedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, ClaimSuspended %>"></asp:Label></asp:TableCell>
                                                       
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="trRecordSummaryTotalAmountRMB" runat="server" style="display:none" BorderWidth="1">
                                                        <asp:TableCell BorderWidth="2" RowSpan="2" runat="server" ID="tdSummarySchemeHCVSCHN">
                                                            <asp:Label ID="lblSummaryHCVSCHN" runat="server" CssClass="tableTitle"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryTotalAmountTextRMB" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TotalVoucherAmountClaimedSignRMB %>"></asp:Label></asp:TableCell>
                                                        <%--<asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryIncompleteRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>--%>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingComfirmRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingVRAcctValidateRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReadyToReimburseRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryVoidedRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReimbursedRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummarySuspendedRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                      
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="trRecordSummaryTotalAmount" runat="server" BorderWidth="1">
                                          
                                                        <%--<td>
                                                            <asp:Label ID="lblSummaryTotalAmountText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TotalAmountSign %>"></asp:Label></td>--%>
                                                        <%--<td>
                                                            <asp:Label ID="lblSummaryTotalAmountText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TotalRedeemAmountSign %>"></asp:Label></td>--%>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryTotalAmountText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TotalVoucherAmountClaimedSign %>"></asp:Label></asp:TableCell>
                                                        <%--<td>
                                                            <asp:Label ID="lblManualReimbursed" runat="server" CssClass="tableText"></asp:Label></td>--%>
                                                        <asp:TableCell BorderWidth="2" ID="tdSummaryIncomplete">
                                                            <asp:Label ID="lblSummaryIncomplete" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingComfirm" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingVRAcctValidate" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReadyToReimburse" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryVoided" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReimbursed" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummarySuspended" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                      
                                                    </asp:TableRow>

                                                    <asp:TableRow ID="trRecordSummaryTotalAmountRMB_SSSCMC" runat="server" style="display:none" BorderWidth="1">
                                                        <asp:TableCell BorderWidth="2" runat="server" ID="tdSummarySchemeSSSCMC">
                                                            <asp:Label ID="lblSummarySSSCMC" runat="server" CssClass="tableTitle"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryTotalAmountTextRMB_SSSCMC" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TotalAmountClaimedSignRMB %>"></asp:Label></asp:TableCell>
                                                        <%--<asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryIncompleteRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>--%>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingComfirmRMB_SSSCMC" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryPendingVRAcctValidateRMB_SSSCMC" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReadyToReimburseRMB_SSSCMC" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryVoidedRMB_SSSCMC" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummaryReimbursedRMB_SSSCMC" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                        <asp:TableCell BorderWidth="2">
                                                            <asp:Label ID="lblSummarySuspendedRMB_SSSCMC" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                                      
                                                    </asp:TableRow>
                                                </asp:Table>
                                                <br />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:ImageButton ID="ibtnBack" runat="server" ImageUrl="<%$ Resources: ImageURL, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnBack_Click" />
                                
                            </asp:View>
                            <asp:View ID="ViewDetail" runat="server">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="height: 30px">
                                            <asp:Label ID="lblClaimInfo" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, ClaimInfo %>"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ClaimTranEnquiry ID="udcClaimTranEnquiry" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:MultiView ID="MultiViewDetailAction" runat="server">
                                                <asp:View ID="ViewDetailButton" runat="server">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 205px">
                                                            </td>
                                                            <td>
                                                                <cc2:CustomImageButton ID="ibtnReturnBtn" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturntoeHealthAccountRectificationBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ReturntoeHealthAccountRectificationBtn %>"  ShowRedirectImage="false"/>
                                                                <asp:ImageButton ID="ibtnDetailBack" runat="server" ImageUrl="<%$ Resources: ImageURL, BackBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDetailBack_Click" />
                                                                <asp:ImageButton ID="ibtnVoid" runat="server" ImageUrl="<%$ Resources: ImageURL, VoidBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, VoidBtn %>" OnClick="ibtnVoid_Click" />
                                                                <asp:ImageButton ID="ibtnModify" runat="server" ImageUrl="<%$ Resources:ImageUrl, ModifyBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, ModifyBtn %>" OnClick="ibtnModify_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="ViewDetailVoid" runat="server">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 205px">
                                                                <asp:Label ID="lblVoidReasonText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, VoidReason %>"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="txtVoidReason" runat="server" Width="500px" MaxLength="255" CssClass="textChi"></asp:TextBox>
                                                                <asp:Image ID="imgAlertVoidReason" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" /></td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ibtnVoidCancel" runat="server" ImageUrl="<%$ Resources: ImageURL, CancelBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnVoidCancel_Click" />
                                                                <asp:ImageButton ID="ibtnVoidConfirm" runat="server" ImageUrl="<%$ Resources: ImageURL, ConfirmVoidBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, ConfirmVoidBtn %>" OnClick="ibtnVoidConfirm_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="ViewDetailModify" runat="server">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 205px">
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ibtnModifyCancel" runat="server" ImageUrl="<%$ Resources: ImageURL, CancelBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClientClick="return ReasonForVisitInitialComplete();" OnClick="ibtnModifyCancel_Click" />
                                                                <asp:ImageButton ID="ibtnModifyNext" runat="server" ImageUrl="<%$ Resources: ImageURL, NextBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClientClick="return ReasonForVisitInitialComplete();" OnClick="ibtnModifyNext_Click" />
                                                                
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="ViewDetailModifyConfirm" runat="server">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 205px">
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ibtnModifyConfirmBack" runat="server" ImageUrl="<%$ Resources: ImageURL, BackBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnModifyConfirmBack_Click" />
                                                                <asp:ImageButton ID="ibtnModifyConfirmConfirm" runat="server" ImageUrl="<%$ Resources: ImageURL, SaveAndConfirmBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, SaveAndConfirmBtn %>" OnClick="ibtnModifyConfirmSaveConfirm_Click" />
                                                                <asp:ImageButton ID="ibtnModifyConfirmSave" runat="server" ImageUrl="<%$ Resources: ImageURL, SaveBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, SaveBtn %>" OnClick="ibtnModifySave_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="ViewCompleteModify" runat="server">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 205px">
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ibtnCompleteModifyReturn" runat="server" ImageUrl="<%$ Resources: ImageURL, ReturnBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnCompleteModifyReturn_Click" />
                                                                <cc2:CustomImageButton ID="ibtnReturnBtnCompleteModify" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturntoeHealthAccountRectificationBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ReturntoeHealthAccountRectificationBtn %>"  ShowRedirectImage="false"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                            </asp:MultiView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewCompleteVoid" runat="server">
                                <table cellpadding="0" cellspacing="0" style="width: 767px">
                                    <tr>
                                        <td style="width: 745px; height: 25px">
                                            <asp:Label ID="lblCompleteVoidText" runat="server" CssClass="tableCaption" Text="Void Completed"
                                                Width="596px" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 745px;" valign="top">
                                            <table cellpadding="0" cellspacing="0" style="width: 762px">
                                                <tr>
                                                    <td style="width: 205px; height: 30px" valign="top">
                                                        <asp:Label ID="lblCompleteVoidDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, VoidTranTime %>"></asp:Label></td>
                                                    <td style="height: 30px" valign="top">
                                                        <asp:Label ID="lblCompleteVoidDate" runat="server" CssClass="tableText"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 205px; height: 45px" valign="top">
                                                        <asp:Label ID="lblCompleteReferenceNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, VoidTranID %>"></asp:Label></td>
                                                    <td style="height: 45px" valign="top">
                                                        <asp:Label ID="lblCompleteReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources: ImageURL, ReturnBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                                                        <cc2:CustomImageButton ID="ibtnReturnBtnCompleteVoid" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturntoeHealthAccountRectificationBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, ReturntoeHealthAccountRectificationBtn %>"  ShowRedirectImage="false"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
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
                                <uc2:SchemeLegend ID="udcSchemeLegend" runat="server" />
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
</asp:Content>
