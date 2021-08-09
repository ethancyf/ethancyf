<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="eHSAccountEnquiryCallCentre.aspx.vb" Inherits="HCVU.eHSAccountEnquiryCallCentre" Title="<%$ Resources:Title, eHSAccountEnquiryCallCentre %>" %>

<%@ Register Src="../UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<%@ Register Src="../UIControl/DocType/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/VaccinationRecord/ucVaccinationRecord.ascx" TagName="ucVaccinationRecord"
    TagPrefix="uc4" %>
<%@ Register Src="~/UIControl/Token/ucInputToken.ascx" TagName="ucInputToken" TagPrefix="uc5" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc6" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eHSAccountEnquiryCallCentreBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, eHSAccountEnquiryCallCentreBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:InfoMessageBox ID="udcInfoMsgBox" runat="server" Width="95%" />
                <cc2:MessageBox ID="udcMsgBox" runat="server" Width="95%" />
            </asp:Panel>
            <asp:MultiView ID="mveHSAccount" runat="server" ActiveViewIndex="0">
                <asp:View ID="viewSearch" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblSearchAcctText" runat="server" Text="<%$ Resources:Text, SearchAccount %>"
                            Font-Bold="True"></asp:Label>
                    </div>
                    <br />
                    <cc1:TabContainer ID="tcSearchRoute" runat="server" ActiveTabIndex="0" CssClass="m_ajax__tab_xp"
                        BorderStyle="None" Width="950px">
                        <cc1:TabPanel ID="tabSearchByParticulars" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblVCSearch" runat="server" Text="<%$ Resources:Text, SearchByParticulars %>"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchDocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:DropDownList ID="ddlSearchDocType" runat="server" style="min-width:204px">
                                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchIdentityNumText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, IdentityDocNo %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchIdentityNum" runat="server" MaxLength="20" Width="200px"
                                                onblur="convertToUpper(this);"></asp:TextBox>
                                            <asp:Image ID="imgSearchIdentityNumError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                Visible="False" ImageAlign="AbsMiddle" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxIdentityNum" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtSearchIdentityNum"
                                                ValidChars='()/-'>
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchENameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInEnglish %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchEName" runat="server" Width="200px" MaxLength="82" onChange="convertToUpper(this);"></asp:TextBox>
                                            <asp:Image ID="imgENameError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                Visible="False" ImageAlign="AbsMiddle" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxEname" runat="server" Enabled="True"
                                                FilterType="Custom, UppercaseLetters, LowercaseLetters" TargetControlID="txtSearchEName"
                                                ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchCNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInChinese %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchCName" runat="server" Width="200px" MaxLength="6" ></asp:TextBox>
                                            <asp:Image ID="imgCNameError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                Visible="False" ImageAlign="AbsMiddle" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchDOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 240px;" valign="top">
                                                        <asp:TextBox ID="txtSearchDOB" runat="server" MaxLength="10" Width="200px"  onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                            onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                            onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="filtereditSDOB" runat="server" Enabled="True"
                                                            FilterType="Custom, Numbers" TargetControlID="txtSearchDOB" ValidChars='-'>
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:Image ID="imgDOBError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                                                    </td>
                                                    <td style="width:80px;" valign="top">
                                                        <asp:Label ID="lblSearchDOBInputTipsText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, InputTipsTitle %>"></asp:Label>
                                                    </td>
                                                    <td style="width:500px;" valign="top">
                                                        <asp:Label ID="lblSearchDOBInputTips" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBHintEnquiryCallCentre %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchGenderText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:RadioButtonList ID="rbSearchGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                                RepeatLayout="Flow">
                                                <asp:ListItem Value="">Any</asp:ListItem>
                                                <asp:ListItem Value="F">Female</asp:ListItem>
                                                <asp:ListItem Value="M">Male</asp:ListItem>
                                            </asp:RadioButtonList>&nbsp;
                                            <asp:Image ID="imgSearchGenderError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                Visible="False" ImageAlign="AbsMiddle" />
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchAccountBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SearchAccountBtn %>" OnClick="ibtnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>                        
                    </cc1:TabContainer>
                </asp:View>
                <asp:View ID="vSearchResult" runat="server">
                    <asp:Panel ID="pnlSearchCriteriaRoute2" runat="server">
                        <uc6:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview2" runat="server" TargetControlID="pnlSearchCriteriaReviewRoute2" />
                        <asp:Panel ID="pnlSearchCriteriaReviewRoute2" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListDocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                    </td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListDocType" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListIdentityNumText" runat="server" CssClass="tableTitle"
                                            Text="<%$ Resources:Text, IdentityDocNo %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListIdentityNum" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListENameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInEnglish %>"></asp:Label></td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListEName" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListDOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListDOB" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListECameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInChinese %>"></asp:Label></td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListCName" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListGenderText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Gender %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListGender" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <table>
                            <tr style="height: 20px">
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="990px">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:CheckBox ID="chkMaskDocumentNo" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                                    AutoPostBack="True" OnCheckedChanged="chkMaskDocumentNo_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvAcctList" runat="server" AllowPaging="True" AllowSorting="True"
                                        Width="1270px" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTempAcctRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID_ReferenceNo %>" HeaderStyle-VerticalAlign="Top"
                                                HeaderStyle-HorizontalAlign="Center" SortExpression="Voucher_Acc_ID">
                                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnAccountID" runat="server" Style="white-space: nowrap"></asp:LinkButton>
                                                    <asp:TextBox ID="hfAccountID" runat="server" Style="display: none" Value='<%# Eval("Voucher_Acc_ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Doc_Display_code" HeaderText="<%$ Resources:Text, DocumentType %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("Doc_Display_code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdentityNum" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="170px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>"
                                                HeaderStyle-VerticalAlign="Top" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdentityNumUnmask" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="170px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, VRName %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>'></asp:Label>
                                                    <br></br>
                                                    <asp:Label ID="lblCName" runat="server" Text='<%# Eval("Chi_Name") %>' Font-Names="HA_MingLiu"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="270px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="DOB" HeaderText="<%$ Resources:Text, DOB %>" HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="110px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Sex" HeaderText="<%$ Resources:Text, Gender %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSex" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="AccountTypeDesc" HeaderText="<%$ Resources:Text, AccountType %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountType" runat="server" Text='<%# Eval("Source") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Account_Status" HeaderText="<%$ Resources:Text, AccountStatus %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountStatus" runat="server" Text='<%# Eval("Account_Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Public_Enquiry_Status" HeaderText="<%$ Resources:Text, EnquiryStatus %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryStatus" runat="server" Text='<%# Eval("Public_Enquiry_Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="PersonalInformation_Status" HeaderText="<%$ Resources:Text, AmendmentStatus %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmendmentStatus" runat="server" Text='<%# Eval("PersonalInformation_Status") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Create_Dtm" HeaderText="<%$ Resources:Text, CreationTime %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreateDtm" runat="server" Text='<%# Eval("Create_Dtm") %>' Style="white-space: nowrap"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Create_By" HeaderText="<%$ Resources:Text, CreateBy %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreate_By" runat="server"></asp:Label>
                                                    <asp:Label ID="lblCreate_By_DH" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>                    
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnSearchResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click" /></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vAccountDetail" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblPersonalParticularsText" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"
                            Font-Bold="True"></asp:Label>
                    </div>
                    <uc1:ucReadOnlyDocumnetType ID="ucReadOnlyDocumnetType" runat="server"></uc1:ucReadOnlyDocumnetType>
                    <uc2:ucInputDocumentType ID="ucInputDocumentType" runat="server" />
                    <asp:Panel ID="pnlAmendingSmartID" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <br />
                                    <asp:Label ID="lblAmendingStar" runat="server" ForeColor="red" Font-Bold="true" Text="*"></asp:Label>
                                    <asp:Label ID="lblAmendingSmartID" runat="server" Font-Italic="true" ForeColor="red"
                                        Text="<%$ Resources:Text, AmendingBySmartID %>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div class="headingText">
                        <asp:Label ID="lblAcctInfoText" runat="server" Text="<%$ Resources:Text, VoucherAccountInfo %>"
                            Font-Bold="True"></asp:Label>
                    </div>
                    <asp:PlaceHolder ID="phReadOnlyAccountInfo" runat="server" />
                    <asp:TextBox ID="txtDocCode" runat="server" Style="display: none"></asp:TextBox>
                    
                    <asp:Panel ID="pnlSchemeInfo" runat="server">                        
                        <div class="headingText" style="padding-top:8px;">
                            <asp:Label ID="lblSchemeInfoText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, PersonalSchemeInfo %>"></asp:Label>
                        </div>
                        <asp:Panel ID="pnlScheme" runat="server">
                            <asp:Panel ID="pnlVoucher" runat="server" Visible="false">
                                <asp:Panel ID="pnlVoucherUtilization" runat="server">
                                    <div style="padding-top:10px;">
                                        <asp:Label ID="lblVoucherUtilization" runat="server" Font-Bold="True" Text="<%$ Resources:Text, VoucherUtilization %>"></asp:Label>
                                    </div>                                    
                                    <table>
                                        <tr>
                                            <td style="width:220px" valign="middle">
                                                <asp:Label ID="lblAvailableVoucherText" runat="server" Text="<%$ Resources:Text, AvailableVoucher %>" CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td valign="middle">
                                                <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trAvailableQuota" runat="server">
                                            <td valign="middle">
                                                <asp:Label ID="lblAvailableQuotaText" runat="server" CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="lblAvailableQuota" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblVoucherUsageText" runat="server" Text="<%$ Resources:Text, VoucherUsage %>" CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Image ID="imgCollapsiblePanelController_v4_HCVS" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShowVoucherUsageSBtn %>"
                                                    ImageAlign="AbsMiddle" onmouseover="this.style.cursor='pointer';" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnlVoucherUsage" runat="server"></asp:Panel>
                                    <cc1:CollapsiblePanelExtender ID="collapsiblePanelExtender_v4_HCVS" runat="server"
                                        TargetControlID="pnlVoucherUsage" CollapsedSize="0" Collapsed="True"
                                        ExpandControlID="imgCollapsiblePanelController_v4_HCVS" CollapseControlID="imgCollapsiblePanelController_v4_HCVS"
                                        ImageControlID="imgCollapsiblePanelController_v4_HCVS" AutoCollapse="False" AutoExpand="False"
                                        ScrollContents="False" ExpandedImage="<%$ Resources:ImageUrl, HideVoucherUsageSBtn %>"
                                        CollapsedImage="<%$ Resources:ImageUrl, ShowVoucherUsageSBtn %>" ExpandDirection="Vertical">
                                    </cc1:CollapsiblePanelExtender>
                                </asp:Panel>                            
                                <asp:Panel ID="pnlVoucherTransHistory" runat="server">
                                    <hr style="BORDER-TOP-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 1px; WIDTH: 99%; COLOR: #ff8080; BORDER-RIGHT-STYLE: none" />
                                    <asp:Label ID="lblVoucherTransHistory" runat="server" Font-Bold="True" Text="<%$ Resources:Text, VoucherTransHistory %>"></asp:Label>
                                    <br />
                                    <table>
                                        <tr id="trVoucherTransHistoryNoRecord" runat="server">
                                            <td>
                                                <asp:Label ID="lblVoucherTransHistoryNoRecord" runat="server" Text="<%$ Resources:Text, NoTransactionRecord %>"></asp:Label>
                                            </td>                                        
                                        </tr>
                                        <tr>
                                            <asp:GridView ID="gvVoucherTransHistory" runat="server" CssClass="gvTable" Width="1500px" AutoGenerateColumns="False" AllowPaging="True"
                                                AllowSorting="True" RowStyle-VerticalAlign="Top" HeaderStyle-Font-Bold="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceDate %>" ItemStyle-Width="80" HeaderStyle-VerticalAlign="Top" SortExpression="Service_Receive_Dtm"><ItemTemplate><asp:Label ID="lblGServiceDate" runat="server" Text='<%# Eval("Service_Receive_Dtm") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Scheme %>" ItemStyle-Width="60" HeaderStyle-VerticalAlign="Top" SortExpression="Scheme_Code"><ItemTemplate><asp:Label ID="lblGScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Amount_claimed_Sign %>" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right" HeaderStyle-VerticalAlign="Top" SortExpression="Total_Amount"><ItemTemplate><asp:Label ID="lblGAmount" runat="server" Text='<%# Eval("Total_Amount")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, ConversionRate %>" ItemStyle-Width="50" HeaderStyle-VerticalAlign="Top" SortExpression="ExchangeRate_Value"><ItemTemplate><asp:Label ID="lblGConversionRate" runat="server" Text='<%# Eval("ExchangeRate_Value")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionNo %>" ItemStyle-Width="100" HeaderStyle-VerticalAlign="Top" SortExpression="Transaction_ID"><ItemTemplate><asp:Label ID="lblGTransactionNo" runat="server" Text='<%# Eval("Transaction_ID") %>'/></ItemTemplate></asp:TemplateField>                                                    
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Cancelled %>" ItemStyle-Width="50" HeaderStyle-VerticalAlign="Top" SortExpression="Cancelled"><ItemTemplate><asp:Label ID="lblGCancelled" runat="server" Text='<%# Eval("Cancelled")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionStatus %>" ItemStyle-Width="80" HeaderStyle-VerticalAlign="Top" SortExpression="Transaction_Status" Visible="false"><ItemTemplate><asp:Label ID="lblGTransactionStatus" runat="server" Text='<%# Eval("Transaction_Status")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, InvalidationStatus %>" ItemStyle-Width="80" HeaderStyle-VerticalAlign="Top" SortExpression="Invalidation_Status" Visible="false"><ItemTemplate><asp:Label ID="lblGInvalidation_Status" runat="server" Text='<%# Eval("Invalidation_Status")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SPID %>" ItemStyle-Width="80" HeaderStyle-VerticalAlign="Top" SortExpression="SP_ID"><ItemTemplate><asp:Label ID="lblGSPID" runat="server" Text='<%# Eval("SP_ID")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, HealthProf %>" ItemStyle-Width="120" HeaderStyle-VerticalAlign="Top" SortExpression="Service_Category_Desc"><ItemTemplate><asp:Label ID="lblGHealthProf" runat="server" Text='<%# Eval("Service_Category_Desc")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceProviderName %>" ItemStyle-Width="120" HeaderStyle-VerticalAlign="Top" SortExpression="SP_Name"><ItemTemplate><asp:Label ID="lblGSPName" runat="server" Text='<%# Eval("SP_Name")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Practice %>" ItemStyle-Width="150" HeaderStyle-VerticalAlign="Top" SortExpression="Practice_Name"><ItemTemplate><asp:Label ID="lblGPracticeName" runat="server" Text='<%# Eval("Practice_Name")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, PracticeAddress %>" ItemStyle-Width="180" HeaderStyle-VerticalAlign="Top" SortExpression="Practice_Address"><ItemTemplate><asp:Label ID="lblGPracticeAddress" runat="server" Text='<%# Eval("Practice_Address")%>' /></ItemTemplate></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, PracticeTel %>" ItemStyle-Width="80" HeaderStyle-VerticalAlign="Top" SortExpression="Phone_Daytime"><ItemTemplate><asp:Label ID="lblGPracticeTelNo" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </tr>
                                    </table>                                
                                </asp:Panel>
                            </asp:Panel>
                            <br/>
                            <asp:Label ID="lblFooterRemark" runat="server" Text="<%$ Resources:Text, SchemeInfoBaseOnIDDoc %>"></asp:Label>
                        </asp:Panel>
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="pnlAccountDetailsActionBtn" runat="server">
                        <br />
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtnAccountDetailsBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnAccountDetailsBack_Click"></asp:ImageButton>
                                    <asp:HiddenField ID="hfIsRedirect" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <%-- Popup for DocType Help --%>
            <asp:Panel ID="panDocTypeHelp" runat="server" Style="display: none;">
                <asp:Panel ID="panDocTypeHelpHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDocTypeHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panDocTypeContent" runat="server" ScrollBars="vertical" Height="300px">
                                <uc3:DocTypeLegend ID="udcDocTypeLegend" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="ibtnCloseDocTypeHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseDocTypeHelp_Click" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button runat="server" ID="btnHiddenDocTypeHelp" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
            </cc1:ModalPopupExtender>
            <%-- End of Popup for DocType --%>
            <%-- Pop up for Unmask --%>
            <asp:Button ID="btnHiddenUnmask" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupUnmask" runat="server" TargetControlID="btnHiddenUnmask"
                PopupControlID="panUnmask" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panUnmask" runat="server" Style="display: none">
                <uc5:ucInputToken ID="udcPUInputToken" runat="server"></uc5:ucInputToken>
            </asp:Panel>
            <%-- End of Pop up for Unmask --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
