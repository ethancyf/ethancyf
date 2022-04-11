<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="ClaimCreation.aspx.vb" Inherits="HCVU.ClaimCreation"
    EnableEventValidation="false" Title="<%$ Resources:Title, ClaimCreation %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<%@ Register Src="../UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/ucInputEHSClaim.ascx" TagName="ucInputEHSClaim" TagPrefix="uc2" %>
<%@ Register Src="~/Reimbursement/ClaimTransDetail.ascx" TagName="ClaimTransDetail" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/RVPHomeListSearch.ascx" TagName="RVPHomeListSearch" TagPrefix="uc4" %>
<%@ Register Src="../UIControl/VaccinationRecord/ucVaccinationRecord.ascx" TagName="ucVaccinationRecord" TagPrefix="uc5" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc6" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc7" %>
<%@ Register Src="~/UIControl/SchoolListSearch.ascx" TagName="SchoolListSearch" TagPrefix="uc8" %>
<%@ Register Src="~/UIControl/OutreachListSearch.ascx" TagName="OutreachListSearch" TagPrefix="uc9" %>

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
            <asp:Image ID="imgClaimTransactionManagement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClaimCreationBanner %>"
                AlternateText="<%$ Resources:AlternateText, ClaimCreationBanner %>" />
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
                <cc2:MessageBox ID="udcMessageBox" runat="server" Width="95%"></cc2:MessageBox>
            </asp:Panel>
            <asp:Panel ID="panReimbursementClaimTransManagement" runat="server" Width="1000px">
                <asp:MultiView ID="mvNewClaimTransaction" runat="server">
                    <asp:View ID="viewSearchResult" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblSearchAccountResult" runat="server" Text="<%$ Resources:Text, eHealthAccountRecord %>"
                                Font-Bold="True"></asp:Label></div>
                        <table>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblSerachAccountResultDocTypeText" runat="server" CssClass="tableTitle"
                                        Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSerachAccountResultDocType" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblSerachAccountResultIdentityNumText" runat="server" CssClass="tableTitle"
                                        Text="<%$ Resources:Text, IdentityDocNo %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSerachAccountResultIdentityNum" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblSerachAccountResultEHSAccountIDText" runat="server" CssClass="tableTitle"
                                        Text="<%$ Resources:Text, AccountID %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSerachAccountResultEHSAccountID" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblSerachAccountResultEHSAccountRefNoText" runat="server" CssClass="tableTitle"
                                        Text="<%$ Resources:Text, RefNo %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSerachAccountResultEHSAccountRefNo" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvSearchAccount" runat="server" AllowPaging="True" AllowSorting="True"
                            Width="1090px" BackColor="White" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSearchIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="20px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Voucher_Acc_ID" HeaderText="<%$ Resources:Text, AccountID_ReferenceNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnEHSAccountID" runat="server"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="130px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Doc_Code" HeaderText="<%$ Resources:Text, DocumentType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocType" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnIdentityNum" runat="server"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="EName" HeaderText="<%$ Resources:Text, VRName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server"></asp:Label></br>
                                        <asp:Label ID="lblCName" runat="server" CssClass="TextChineseName"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="DOB" HeaderText="<%$ Resources:Text, DOB %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Sex" HeaderText="<%$ Resources:Text, Gender %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSex" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" HorizontalAlign="Center" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, AccountStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountStatus" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Create_Dtm" HeaderText="<%$ Resources:Text, CreationTime %>"
                                    HeaderStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreateDtm" runat="server" Style="white-space: nowrap"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Create_By" HeaderText="<%$ Resources:Text, CreateBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="90px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:GridView>
                        <table width="100%">
                            <tr>
                                <td style="width: 100px" align="left" valign="top">
                                    <asp:ImageButton ID="ibtnSearchAccountResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchAccountResultBack_Click">
                                    </asp:ImageButton>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="viewEnterClaimDetails" runat="server">
                        <table style="width: 100%" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:MultiView ID="mvEnterDetails" runat="server">
                                        <asp:View ID="viewCreationDetails" runat="server">
                                            <div class="headingText">
                                                <asp:Label ID="lblEnterCreationDetailHeading" runat="server" Text="<%$ Resources:Text, EnterDetails %>" Font-Bold="True" />
                                            </div>
                                            <table style="width: 100%;padding-left:22px" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td style="vertical-align: top;" colspan="2">
                                                        <asp:Label ID="lblEnterCreationDetailAccountInfoHeading" runat="server" Font-Underline="True"
                                                            Text="<%$ Resources:Text, PersonalParticulars %>" CssClass="tableText" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetaileHSAccountTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>" style="position:relative;top:2px"/>
                                                    </td>
                                                    <td style="height:25px;padding-top:8px;vertical-align:top">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlEnterCreationDetaileHSAccountType" runat="server" AppendDataBoundItems="True" style="height:22px;width:485px;position:relative;top:-1px" />
                                                                </td>
                                                                <td>
                                                                    <asp:Image ID="imgEnterCreationDetaileHSAccountTypeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                                </td>
                                                            </tr>
                                                        </table>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetaileHSAccountDocNoText" runat="server" Text="<%$ Resources:Text, IdentityDocNo %>" style="position:relative;top:2px"/>
                                                    </td>
                                                    <td style="height:25px;padding-top:8px;vertical-align:top">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtEnterCreationDetaileHSAccountDocNo" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="20"/>
                                                                </td>
                                                                <td>
                                                                    <asp:Image ID="imgEnterCreationDetaileHSAccountDocNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:3px"/>
                                                                </td>
                                                            </tr>
                                                        </table>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetaileHSAccountIDText" runat="server" Text="<%$ Resources:Text, AccountID %>" style="position:relative;top:2px"/>
                                                    </td>
                                                    <td style="height:25px;padding-top:8px;vertical-align:top">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEnterCreationDetaileHSAccountIDPrefix" runat="server" CssClass="tableTitle" Text="" Width="32px" Height="18" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEnterCreationDetaileHSAccountID" runat="server" Width="144" onChange="convertToUpper(this)" MaxLength="9"/>
                                                                </td>
                                                                <td>
                                                                    <asp:Image ID="imgEnterCreationDetaileHSAccountIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:3px"/>
                                                                </td>
                                                            </tr>
                                                        </table>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetaileHSAccountRefNoText" runat="server" Text="<%$ Resources:Text, RefNo %>" style="position:relative;top:2px"/>
                                                    </td>
                                                    <td style="height:25px;padding-top:8px;vertical-align:top">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtEnterCreationDetaileHSAccountRefNo" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="18"/>
                                                                </td>
                                                                <td>
                                                                    <asp:Image ID="imgEnterCreationDetaileHSAccountRefNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:3px"/>
                                                                </td>
                                                            </tr>
                                                        </table>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top;padding-top:18px;" colspan="2">
                                                        <asp:Label ID="lblEnterCreationDetailClaimInfoHeading" runat="server" Font-Underline="True"
                                                            Text="<%$ Resources:Text, ClaimCreationInfo %>" CssClass="tableText" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetailSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>"
                                                            Height="25px"></asp:Label>
                                                    </td>
                                                    <td style="height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:TextBox ID="txtEnterCreationDetailSPID" runat="server" MaxLength="8" Width="80px" style="position:relative;top:-5px;left:1px;" />&nbsp;
                                                        <asp:Image ID="imgEnterCreationDetailSPIDError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />&nbsp;
                                                        <asp:ImageButton ID="ibtnSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, SearchSBtn %>" OnClick="ibtnSearchSP_Click" />&nbsp;
                                                        <asp:ImageButton ID="ibtnClearSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, ClearBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ClearSBtn %>" OnClick="ibtnClearSearchSP_Click" />&nbsp;
                                                        <cc1:FilteredTextBoxExtender ID="FilteredSearchSPID" runat="server" FilterType="Custom, Numbers"
                                                            TargetControlID="txtEnterCreationDetailSPID">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredSearcheHSAccountDocNo" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                                            TargetControlID="txtEnterCreationDetaileHSAccountDocNo" ValidChars="()/-">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredSearcheHSAccountID" runat="server" FilterType="Custom, Numbers"
                                                            TargetControlID="txtEnterCreationDetaileHSAccountID">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredSearchRefNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="txtEnterCreationDetaileHSAccountRefNo" ValidChars='-'>
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetailSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"
                                                            Height="25px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEnterCreationDetailSPName" runat="server" CssClass="tableText"></asp:Label><asp:Label
                                                            ID="lblEnterCreationDetailSPStatus" runat="server" CssClass="tableText" ForeColor="red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetailPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                                            Height="25px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEnterCreationDetailPractice" runat="server" style="width:485px;height:22px" AutoPostBack="true" />
                                                        <asp:Label ID="lblEnterCreationDetailPracticeStatus" runat="server" CssClass="tableText" ForeColor="red" style="position:relative;top:-2px" />
                                                        <asp:Image ID="imgEnterCreationDetailPractice" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:2px;left:1px"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetailSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"
                                                            Height="25px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEnterCreationDetailScheme" runat="server" style="width:485px;height:22px" AutoPostBack="true" />
                                                        <asp:Label ID="lblEnterCreationDetailSchemeStatus" runat="server" CssClass="tableText" ForeColor="red" style="position:relative;top:-2px" />
                                                        <asp:Image ID="imgEnterCreationDetailScheme" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:2px;left:1px"/>
                                                    </td>
                                                </tr>
                                                <asp:panel ID="panEnterCreationDetailNonClinicSetting" runat="server" Visible="false">
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblEnterCreationDetailNonClinicSetting" runat="server" CssClass="tableText" />
                                                    </td>
                                                </tr>
                                                </asp:panel>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetailCreationReasonText" runat="server" Text="<%$ Resources:Text, CreationReason %>"
                                                            Height="25px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEnterCreationDetailCreationReason" runat="server" style="width:485px;height:22px" />
                                                        <asp:Image ID="imgEnterCreationDetailCreationReason" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" style="position:relative;top:-4px;left:1px"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:5px;vertical-align:top">
                                                    </td>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width:80px;height:25px;padding-top:3px;vertical-align:top">
                                                                    <asp:Label ID="lblEnterCreationDetailRemarksText" runat="server" Text="<%$ Resources:Text, Remarks %>" Height="25px" style="position:relative;top:2px;"/>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEnterCreationDetailRemarks" runat="server" MaxLength="255" Width="400px"
                                                                        CssClass="TextBoxChi" Height="16px" style="position:relative;left:1px;" />
                                                                    <asp:Image ID="imgEnterCreationDetailRemarks" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" style="position:relative;top:2px;left:1px"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                        <asp:Label ID="lblEnterCreationDetailPaymentSettlementText" runat="server" Text="<%$ Resources:Text, PaymentSettlement %>"
                                                            Height="25px" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEnterCreationDetailPaymentSettlement" runat="server" style="width:485px;height:22px" />
                                                        <asp:Image ID="imgEnterCreationDetailPaymentSettlement" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" style="position:relative;top:-4px;left:1px"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:200px;height:25px;padding-top:8px;vertical-align:top">
                                                    </td>
                                                    <td valign="top">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width:80px;height:25px;padding-top:3px;vertical-align:top">
                                                                    <asp:Label ID="lblEnterCreationDetailPaymentRemarksText" runat="server" Text="<%$ Resources:Text, Remarks %>"
                                                                        Height="25px" style="position:relative;top:2px;"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEnterCreationDetailPaymentRemarks" runat="server" MaxLength="255"
                                                                        Width="400px" CssClass="TextBoxChi" Height="16" style="position:relative;left:1px;" />
                                                                    <asp:Image ID="imgEnterCreationDetailPaymentRemarks" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" style="position:relative;top:1px;left:1px"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td style="column-span:all;padding-top:8px;text-align:center;vertical-align:top">
                                                        <asp:ImageButton ID="ibtnNewClaimTransaction" runat="server" AlternateText="<%$ Resources:AlternateText, NewClaimTransactionBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, NewClaimTransactionBtn %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="viewClaimDetails" runat="server">
                                            <div class="headingText">
                                                <asp:Label ID="lblEnterClaimDetailHeading" runat="server" Text="<%$ Resources:Text, EnterClaimDetails %>" Font-Bold="True" />
                                                <asp:ImageButton ID="ibtnVaccinationRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, VaccinationRecordSmallBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, VaccinationRecordSmallBtn %>" OnClick="ibtnVaccinationRecord_Click" style="padding-left:5px;position:relative;top:4px" />
                                            </div>
                                            <table style="width: 100%;padding-left:22px" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblEnterClaimDetailAccountHeading" runat="server" Font-Underline="True"
                                                            Text="<%$ Resources:Text, VRInformation %>" CssClass="tableText" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top">
                                                        <div style="position:relative;left:-3px">
                                                            <uc1:ucReadOnlyDocumnetType ID="udceHealthAccountInfo" runat="server" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>  
                                            
                                            <asp:Panel ID="panStep2aDischargeRecord" runat="server" Visible="false">
                                                <table cellspacing="0" cellpadding="0" border="0" style="padding-left:22px">
                                                    <tr>
                                                        <td style="vertical-align: top;height:22px">
                                                            <asp:Label ID="lblCDischargeRecordHeading" runat="server" Font-Underline="True" Text="<%$ Resources: Text, DischargeRecordForCOVID19 %>" CssClass="tableText"/>
                                                            <asp:Label ID="lblCDischargeRecordStatus" runat="server" CssClass="tableText" style="color:red" Visible="false"/>
                                                            <asp:Button ID="btnModalPopupDischargeRecordRemark" runat="server" BorderWidth="0" BorderStyle="None" Width="17px" Height="17px"
                                                                AlternateText="<%$ Resources:Text, Remarks%>" 
                                                                style="vertical-align:top;position:relative;top:2px;background-color: transparent;background-image:url('../Images/others/info.png');background-repeat: no-repeat;"
                                                                onmouseover="this.style.cursor='pointer'" Visible="false"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table style="border:1px solid;padding:0px;border-spacing:1px;border-collapse:collapse;border-color:#CCCCCC;width:930px">
                                                                <tr style="background-color:#70ae47">
                                                                    <td style="width:465px;vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                                        <asp:Label ID="lblCDischargeDateText" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, HospitalDischargeDate %>" />
                                                                    </td>
                                                                    <td style="vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                                        <asp:Label ID="lblCDischargeRemarkText" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, Remarks %>" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="background-color:white">
                                                                    <td style="vertical-align:middle;border:1px solid;border-color:#61615b;height:20px">
                                                                        <asp:Label ID="lblCDischargeDate" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px" />
                                                                    </td>
                                                                    <td style="vertical-align:middle;border:1px solid;border-color:#61615b;height:20px">
                                                                        <asp:Label ID="lblCDischargeRemark" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-bottom:10px">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>                
                                            
                                            <asp:Panel ID="panStep2aMedicalExemptionRecord" runat="server" Visible="false">
                                                <table cellspacing="0" cellpadding="0" border="0" style="padding-left:22px">
                                                    <tr>
                                                        <td class="eHSTableHeading" style="padding-bottom:3px">
                                                            <asp:Label ID="lblMedicalExemptionRecordHeading" runat="server" Font-Underline="True" Text="<%$ Resources:Text, MedicalExemptionRecordsHeading%>" CssClass="tableText" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="panMedicalExemptionRecord" runat="server">
                                                                <table style="border:0px solid;padding:0px;border-spacing:2px;border-collapse:separate;border-color:#CCCCCC;width:930px">
                                                                    <tr style="background-color:#70AD47">
                                                                        <td style="width:125px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                                            <asp:Label ID="lblNoMedicalExemptionRecordDOI" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, DateOfIssue %>" />
                                                                        </td>
                                                                        <td style="width:125px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                                            <asp:Label ID="lblNoMedicalExemptionRecordIssuer" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, Issuer %>" />
                                                                        </td>
                                                                        <td style="width:125px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                                            <asp:Label ID="lblNoMedicalExemptionRecordValidUntil" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, ValidUntil %>" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color:#D5E3CF">
                                                                        <td style="vertical-align:middle;border:0px solid;border-color:transparent;height:20px" colspan="3">
                                                                            <asp:Label ID="lblNoMedicalExemptionRecord" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px"
                                                                                Text="<%$ Resources:Text, NoRecordsFound%>" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <asp:GridView ID="gvMedicalExemptionRecord" runat="server" AutoGenerateColumns="False" Width="930px" BorderColor="Transparent"
                                                                AllowSorting="True"  AllowPaging="False"  OnRowDataBound="gvMedicalExemptionRecord_RowDataBound"
                                                                OnPreRender="gvMedicalExemptionRecord_PreRender" OnSorting="gvMedicalExemptionRecord_Sorting">
                                                                <Columns>
                                                                    <asp:TemplateField SortExpression="DateOfIssueDtmSorting" HeaderText="<%$ Resources: Text, DateOfIssue %>">
                                                                        <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#70AD47" BorderColor="Transparent"/>                                  
                                                                        <ItemStyle Width="125px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMedicalExemptionRecordDOI" runat="server" Text='<%# Bind("DateOfIssueDtm")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="ServiceProviderSorting" HeaderText="<%$ Resources: Text, Issuer %>">
                                                                        <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#70AD47" BorderColor="Transparent"/>
                                                                        <ItemStyle Width="280px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMedicalExemptionRecordSP" runat="server" Text='<%# Bind("ServiceProviderEng")%>' />
                                                                            <asp:Label ID="lblMedicalExemptionRecordSPChi" runat="server" Text='<%# Bind("ServiceProviderChi")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField SortExpression="ValidUntilDtmSorting" HeaderText="<%$ Resources: Text, ValidUntil %>">
                                                                        <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#70AD47" BorderColor="Transparent"/>
                                                                        <ItemStyle Width="280px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMedicalExemptionRecordValidUntil" runat="server" Text='<%# Bind("ValidUntilDtm")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle BackColor="#D5E3CF" ForeColor="Black"  />                              
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-bottom:10px">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>                                                                        
                                                                                                                                
                                            <table style="width: 100%;padding-left:22px" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td style="vertical-align: bottom; height: 25px;">
                                                        <asp:Label ID="lblTransactionHeading" runat="server" Font-Underline="True" Text="<%$ Resources:Text, TransactionInformation %>" CssClass="tableText" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%;padding-left:22px" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                        <asp:Label ID="lblEnterClaimDetailSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProvider %>" style="position:relative;top:4px" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblEnterClaimDetailSPName" runat="server" CssClass="tableText" />
                                                        <asp:Label ID="lblEnterClaimDetailSPID" runat="server" CssClass="tableText" />
                                                        <asp:Label ID="lblEnterClaimDetailSPStatus" runat="server" CssClass="tableText" ForeColor="red" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                        <asp:Label ID="lblEnterClaimDetailPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>" style="position:relative;top:4px" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblEnterClaimDetailPractice" runat="server" CssClass="tableText" />
                                                        <asp:Label ID="lblEnterClaimDetailPracticeStatus" runat="server" CssClass="tableText" ForeColor="red" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                        <asp:Label ID="lblEnterClaimDetailCreationReasonText" runat="server" Text="<%$ Resources:Text, CreationReason %>" style="position:relative;top:4px" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblEnterClaimDetailCreationReason" runat="server" CssClass="tableText" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                        <asp:Label ID="lblEnterClaimDetailPaymentMethodText" runat="server" Text="<%$ Resources:Text, PaymentSettlement %>" style="position:relative;top:4px" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblEnterClaimDetailPaymentMethod" runat="server" CssClass="tableText" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                        <asp:Label ID="lblEnterClaimDetailServiceDateText" runat="server" Text="<%$ Resources:Text, ServiceDate %>" style="position:relative;top:4px" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:TextBox ID="txtEnterClaimDetailServiceDate" runat="server" Width="71px" MaxLength="10"
                                                            OnTextChanged="txtEnterClaimDetailServiceDate_TextChanged" AutoPostBack="true" />
                                                        <asp:ImageButton ID="ibtnEnterClaimDetailServiceDate" runat="server" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" style="position:relative;top:2px" />
                                                        <asp:Image ID="imgEnterClaimDetailServiceDateErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:4px" />
                                                        <cc1:CalendarExtender ID="EnterClaimDetailServiceDate" runat="server" TargetControlID="txtEnterClaimDetailServiceDate"
                                                            PopupButtonID="ibtnEnterClaimDetailServiceDate" Format="dd-MM-yyyy" />
                                                        <cc1:FilteredTextBoxExtender ID="filteredEnterClaimDetailServiceDate" runat="server"
                                                            TargetControlID="txtEnterClaimDetailServiceDate" FilterType="Custom, Numbers"
                                                            ValidChars="-" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                        <asp:Label ID="lblEnterClaimDetailSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>" style="position:relative;top:4px" />
                                                    </td>
                                                    <td valign="middle">
                                                        <%--<asp:DropDownList ID="ddlEnterClaimDetailsSchemeText" AutoPostBack="true" runat="server"
                                                            Width="430px" OnSelectedIndexChanged="ddlEnterClaimDetailsSchemeText_SelectedIndexChanged" style="margin-top:1px;">
                                                        </asp:DropDownList>--%>
                                                        <asp:Label ID="lblEnterClaimDetailScheme" runat="server" CssClass="tableText" />
                                                        <asp:Label ID="lblEnterClaimDetailSchemeStatus" runat="server" CssClass="tableText" ForeColor="red" Visible="false" />
                                                    </td>
                                                </tr>
                                                <asp:panel ID="panEnterClaimDetailNonClinicSetting" runat="server" Visible="false">
                                                <tr>
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblEnterClaimDetailNonClinicSetting" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                </asp:panel>
                                                <asp:panel ID="panHKICSymbol" runat="server" Visible="false">
                                                <tr runat="server">
                                                    <td style="width: 200px; height: 25px;" valign="top">
                                                        <asp:Label ID="lblHKICSymbolText" runat="server" Text="<%$ Resources:Text, HKICSymbolLong %>" Height="25px" />
                                                    </td>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblHKICSymbol" runat="server" AutoPostBack="False" Enabled="true"
                                                                            CssClass="tableText" RepeatDirection="Horizontal" Style="position:relative;left:-7px;top:-2px;display:inline-block"/>
                                                                    </td >
                                                                <td width="30px">
                                                                    <asp:Image ID="imgErrHKICSymbol" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Top" Style="position:relative;top:-2px;"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                </asp:panel>
                                            </table>
                                            <table style="width: 100%;padding-left:22px;padding-bottom:10px" cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <uc2:ucInputEHSClaim ID="udInputEHSClaim" runat="server"></uc2:ucInputEHSClaim>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%;padding-left:22px">
                                                <tr>
                                                    <td style="width: 195px" align="left" valign="top">
                                                        <asp:ImageButton ID="ibtnEnterClaimDetailBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClientClick="return ReasonForVisitInitialComplete();" OnClick="ibtnEnterClaimDetailBack_Click">
                                                        </asp:ImageButton>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:ImageButton ID="ibtnEnterClaimDetailSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClientClick="return ReasonForVisitInitialComplete();" OnClick="ibtnEnterClaimDetailSave_Click" /></td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="viewConfirmClaimCreation" runat="server">
                        <uc3:ClaimTransDetail ID="udcConfirmClaimCreation" runat="server" />
                        <table width="100%">
                            <tr>
                                <td align="center" valign="top">
                                    <asp:ImageButton ID="ibtnConfirmClaimCreationConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnConfirmClaimCreationConfirm_Click">
                                    </asp:ImageButton>
                                    <asp:ImageButton ID="ibtnConfirmClaimCreationCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnConfirmClaimCreationCancel_Click">
                                    </asp:ImageButton></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="viewCompleteClaimCreation" runat="server">
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:ImageButton ID="ibtnCompleteClaimCreationReturn" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnCompleteClaimCreationReturn_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>

                <%-- Pop up for Advanced Search of SP --%>
                <asp:Button ID="btnHiddenSearchSP" runat="server" style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupSearchSP" runat="server" TargetControlID="btnHiddenSearchSP"
                    PopupControlID="panSearchSP" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panSearchSPHeading">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panSearchSP" runat="server" Style="display: none">
                    <asp:Panel ID="panSearchSPHeading" runat="server" Style="cursor: move">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, SearchServiceProvider %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff">
                                <asp:Panel ID="pnlAdvancedSearchCritieria" runat="server">
                                    <cc2:InfoMessageBox ID="udcInfoMsgAdvancedSearch" runat="server" Width="95%" Visible="false">
                                    </cc2:InfoMessageBox>
                                    <cc2:MessageBox ID="udcSystemMsgAdvancedSearch" runat="server" Width="95%" Visible="false">
                                    </cc2:MessageBox>
                                    <table>
                                        <tr>
                                            <td style="width: 180px">
                                                <asp:Label ID="lblAdvancedSearchSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                            <td style="width: 220px">
                                                <asp:TextBox ID="txtAdvancedSearchSPID" runat="server" MaxLength="8"></asp:TextBox>
                                                <asp:Image ID="imgAdvancedSearchSPIDErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                            <td style="width: 180px">
                                                <asp:Label ID="lblAdvancedSearchHKICText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtAdvancedSearchHKIC" runat="server" MaxLength="11" onBlur="formatHKID(this)"></asp:TextBox>
                                                <asp:Image ID="imgAdvancedSearchHKICErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px">
                                                <asp:Label ID="lblAdvancedSearchNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label>
                                            </td>
                                            <td style="width: 220px">
                                                <asp:TextBox ID="txtAdvancedSearchName" runat="server" MaxLength="82" onBlur="Upper(event,this)"
                                                    ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>"></asp:TextBox>
                                            </td>
                                            <td style="width: 180px">
                                                <asp:Label ID="lblAdvancedSearchPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAdvancedSearchPhone" runat="server" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:ImageButton ID="ibtnAdvancedSearchSP" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnAdvancedSearchSP_Click" />
                                                <asp:ImageButton ID="ibtnAdvancedSearchSPClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CloseBtn %>" OnClick="ibtnAdvancedSearchSPClose_Click" /></td>
                                        </tr>
                                    </table>
                                    <cc1:FilteredTextBoxExtender ID="FilteredAdvancedSPID" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtAdvancedSearchSPID">
                                    </cc1:FilteredTextBoxExtender>
                                    <cc1:FilteredTextBoxExtender ID="FilteredAdvancedSPHKID" runat="server" TargetControlID="txtAdvancedSearchHKIC"
                                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="()">
                                    </cc1:FilteredTextBoxExtender>
                                    <cc1:FilteredTextBoxExtender ID="FilteredAdvancedSPName" runat="server" TargetControlID="txtAdvancedSearchName"
                                        FilterType="Custom, LowercaseLetters, UppercaseLetters " ValidChars="'.-, ">
                                    </cc1:FilteredTextBoxExtender>
                                </asp:Panel>
                                <asp:Panel ID="pnlAdvancedSearchResult" runat="server" Visible="false" ScrollBars="Vertical">
                                    <table width="760px">
                                        <tr>
                                            <td valign="top">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 180px; height: 25px; vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"
                                                                Height="25px"></asp:Label></td>
                                                        <td style="width: 180px; height: 25px; vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                                        <td style="width: 180px; height: 25px; vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultHKICText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"
                                                                Height="25px"></asp:Label></td>
                                                        <td style="vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultHKIC" runat="server" CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 180px; height: 25px; vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"
                                                                Height="25px"></asp:Label>
                                                        </td>
                                                        <td style="width: 180px; height: 25px; vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultName" runat="server" CssClass="tableText"></asp:Label>
                                                        </td>
                                                        <td style="width: 180px; height: 25px; vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"
                                                                Height="25px"></asp:Label>
                                                        </td>
                                                        <td style="vertical-align: top">
                                                            <asp:Label ID="lblAdvancedSearchResultPhone" runat="server" CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:GridView ID="gvAdvancedSearchSP" runat="server" AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdvancedSearchResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" Width="10px" />
                                                            <HeaderStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnAdvancedSearchSPID" runat="server" Text='<%# Eval("SP_ID") %> '
                                                                    CommandArgument='<%# Eval("SP_ID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" Width="70px" />
                                                            <HeaderStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="SP_HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdvancedSearchSPHKID" runat="server" Text='<%# Eval("SP_HKID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" Width="90px" />
                                                            <HeaderStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="SP_Eng_Name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdvancedSearchEname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label>
                                                                <asp:Label ID="lblAdvancedSearchCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" />
                                                            <HeaderStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Phone_Daytime" HeaderText="<%$ Resources:Text, ContactNo %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdvancedSearchPhone" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" Width="80px" />
                                                            <HeaderStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdvancedSearchScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                                            <HeaderStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td align="center" style="width: 100px">
                                                            <asp:ImageButton ID="ibtnAdvancedSearchResultBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnAdvancedSearchResultBack_Click" /></td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="ibtnAdvancedSearchResultClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, CloseBtn %>" OnClick="ibtnAdvancedSearchSPClose_Click" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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
                <%-- End of Pop up for Advanced Search of SP --%>

                <%-- Pop up for RVP Home List --%>
                <asp:Button ID="btnModalPopupRVPHomeListSearch" runat="server" style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtenderRVPHomeListSearch" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                    TargetControlID="btnModalPopupRVPHomeListSearch" PopupControlID="panPopupRVPHomeListSearch"
                    PopupDragHandleControlID="panRCHSearchHomeListHeading" RepositionMode="None">
                </cc1:ModalPopupExtender>
                <asp:Panel Style="display: none" ID="panPopupRVPHomeListSearch" runat="server">
                    <asp:Panel Style="cursor: move" ID="panRCHSearchHomeListHeading" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                    <asp:Label ID="lblRCHSearchFormTitle" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 980px" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                </td>
                                <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                                    <asp:Panel ID="panRCHRecord" runat="server">
                                        <uc4:RVPHomeListSearch ID="udcRVPHomeListSearch" runat="server"></uc4:RVPHomeListSearch>
                                    </asp:Panel>
                                </td>
                                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                </td>
                                <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                    <asp:ImageButton ID="ibtnPopupRVPHomeListSearchCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPopupRVPHomeListSearchCancel_Click">
                                    </asp:ImageButton>
                                    <asp:ImageButton ID="ibtnPopupRVPHomeListSearchSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" OnClick="ibtnPopupRVPHomeListSearchSelect_Click" />
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
                        </tbody>
                    </table>
                </asp:Panel>
                <%-- End of Pop up for RVP Home List --%>

                <%-- Pop up for School List --%>
                <asp:Button ID="btnModalPopupSchoolListSearch" runat="server" style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtenderSchoolListSearch" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                    TargetControlID="btnModalPopupSchoolListSearch" PopupControlID="panPopupSchoolListSearch"
                    PopupDragHandleControlID="panSearchSchoolListHeading" RepositionMode="None">
                </cc1:ModalPopupExtender>
                <asp:Panel Style="display: none" ID="panPopupSchoolListSearch" runat="server">
                    <asp:Panel Style="cursor: move" ID="panSearchSchoolListHeading" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                    <asp:Label ID="lblSearchSchoolTitle" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 980px" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                </td>
                                <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                                    <asp:Panel ID="panSchoolList" runat="server">
                                        <uc8:SchoolListSearch ID="udcSchoolListSearch" runat="server"></uc8:SchoolListSearch>
                                    </asp:Panel>
                                </td>
                                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                </td>
                                <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                    <asp:ImageButton ID="ibtnPopupSchoolListSearchCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPopupSchoolListSearchCancel_Click">
                                    </asp:ImageButton>
                                    <asp:ImageButton ID="ibtnPopupSchoolListSearchSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" OnClick="ibtnPopupSchoolListSearchSelect_Click" />
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
                        </tbody>
                    </table>
                </asp:Panel>
                <%-- End of Pop up for School List --%>

                <%-- Pop up for Outreach List --%>
                <asp:Button ID="btnModalPopupOutreachListSearch" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtenderOutreachListSearch" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                    TargetControlID="btnModalPopupOutreachListSearch" PopupControlID="panPopupOutreachListSearch"
                    PopupDragHandleControlID="panOutreachListHeading" RepositionMode="None">
                </cc1:ModalPopupExtender>
                <asp:Panel Style="display: none" ID="panPopupOutreachListSearch" runat="server">
                    <asp:Panel Style="cursor: move" ID="panOutreachListHeading" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                    <asp:Label ID="lblOutreachSearchTitle" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 980px" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                                    <asp:Panel ID="panOutreachRecord" runat="server">
                                        <uc9:OutreachListSearch ID="udcOutreachSearch" runat="server"></uc9:OutreachListSearch>
                                    </asp:Panel>
                                </td>
                                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                    <asp:ImageButton ID="btnPopupOutreachListSearchCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPopupOutreachListSearchCancel_Click"></asp:ImageButton>
                                    <asp:ImageButton ID="btnPopupOutreachListSearchSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" OnClick="ibtnPopupOutreachListSearchSelect_Click" />
                                </td>
                                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <%-- End of Pop up for Outreach List --%>
            </asp:Panel>
            
            <%-- Pop up for Warning Message --%>
            <asp:Button ID="btnModalPopupWarningMessage" runat="server" style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderWarningMessage" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupWarningMessage" PopupControlID="panPopupWarningMessage"
                PopupDragHandleControlID="panWarningMessageHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupWarningMessage" runat="server" Height="450px">
                <asp:Panel Style="cursor: move" ID="panWarningMessageHeading" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                <asp:Label ID="lblWarningMessageHeading" runat="server" Text="<%$ Resources:Text, Warning %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 680px" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                            <cc2:MessageBox ID="udcOverrideReasonMsgBox" runat="server" Visible="false"></cc2:MessageBox>
                            <asp:Panel ID="pnlWarningMsgContent" runat="server" ScrollBars="Vertical">
                                <table width="638px">
                                    <tr>
                                        <td>
                                            <cc2:WarningMessageBox ID="udcWarningMessageBox" runat="server" ShowHeader="false" />
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td style="width: 120px; height: 25px" valign="top">
                                            <asp:Label ID="lblConfirmClaimCreationOverrideReasonText" runat="server" Text="<%$ Resources:Text, OverrideReason %>"
                                                Height="25px"></asp:Label>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtConfirmClaimCreationOverrideReason" runat="server" MaxLength="255"
                                                            Width="480px" CssClass="TextBoxChi"></asp:TextBox></td>
                                                    <td>
                                                        <asp:Image ID="imgConfirmClaimCreationOverrideReason" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle"
                                            colspan="2">
                                            <asp:ImageButton ID="ibtnWarningMessageConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnWarningMessageConfirm_Click" />
                                            <asp:ImageButton ID="ibtnWarningMessageCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnWarningMessageCancel_Click">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
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
            <%-- End of Pop up for Warning Message --%>

            <%-- Pop up for Vaccination Record --%>
            <asp:Button ID="btnHiddenVaccinationRecord" runat="server" style="display: none" />
            <cc1:ModalPopupExtender ID="popupVaccinationRecord" runat="server" TargetControlID="btnHiddenVaccinationRecord"
                PopupControlID="panVaccinationRecord" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panVaccinationRecordHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panVaccinationRecord" runat="server" Style="display: none">
                <asp:Panel ID="panVaccinationRecordHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 965px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblVaccinationRecordHeader" runat="server" Text="<%$ Resources:Text, VaccinationRecord %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 965px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #FFFFFF">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="panVaccinationRecordContent" runat="server" ScrollBars="Auto" Width="940px"
                                            Height="538px">
                                            <uc5:ucVaccinationRecord ID="ucVaccinationRecord" runat="server"></uc5:ucVaccinationRecord>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ibtnVaccinationRecordClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CloseBtn %>" OnClick="ibtnVaccinationRecordClose_Click" />
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
            <%-- End of Pop up for Vaccination Record --%>

            <%-- Popup for DocType Help --%>
            <asp:Button ID="btnHiddenDocTypeHelp" runat="server" style="display: none" />
            <cc1:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
            </cc1:ModalPopupExtender>
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
                            <asp:Panel ID="panDocTypeContent" runat="server" ScrollBars="vertical" Height="300px">
                                <uc7:DocTypeLegend ID="udcDocTypeLegend" runat="server" />
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
            <%-- End of Popup for DocType --%>

            <%-- Popup for Discharge Record Remark --%>
            <cc1:ModalPopupExtender ID="ModalPopupDischargeRecordRemark" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupDischargeRecordRemark" PopupControlID="panDischargeRecordRemark" BehaviorID="mpeDischargeRecordRemark"
                PopupDragHandleControlID="" OkControlID="btnDischargeRecordRemarkClose" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <%--<asp:Button ID="btnModalPopupDischargeRecordRemark" runat="server" Style="display: none" >
            </asp:Button>--%>
            <asp:Panel Style="display: none" ID="panDischargeRecordRemark" runat="server">
                <asp:Panel ID="panDischargeRecordRemarkHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblpanDischargeRecordRemarkHeading" runat="server" Text="<%$ Resources:Text, Remarks %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panDischargeRecordRemarkContent" runat="server" ScrollBars="None">
                                <div id="divDischargeRecordRemarkTitle" runat="server" style="width: 620px; margin: 14px 2px 0px 2px">
                                </div>
                                <div id="divDischargeRecordRemark" runat="server" style="width: 620px; margin: 6px 2px 2px 2px">
                                    <asp:Label ID="lblpanDischargeRecordRemarkContent" runat="server" Text="<%$ Resources:Text, DischargeRecordForCOVID19Remark %>" />                                    
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnDischargeRecordRemarkClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Popup for Other Vaccination Record Remark --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
