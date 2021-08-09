<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="claimTransManagement.aspx.vb" Inherits="HCVU.claimTransManagement"
    EnableEventValidation="false" Title="<%$ Resources:Title, ReimbursementClaimTransMgt %>" %>

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
            <asp:Image ID="imgClaimTransactionManagement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReimbursementClaimTransMgtBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReimbursementClaimTransMgtBanner %>" />
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
                <cc2:MessageBox ID="udcMessageBox" runat="server" Width="95%"></cc2:MessageBox>
            </asp:Panel>
            <asp:Panel ID="panReimbursementClaimTransManagement" runat="server" Width="1000px">
                <asp:MultiView ID="MultiViewReimClaimTransManagement" runat="server">
                    <asp:View ID="ViewInputCriteria" runat="server">

                        <cc1:TabContainer ID="TabContainerCTM" runat="server" CssClass="m_ajax__tab_xp" Width="990px" ActiveTabIndex="1" EnableTheming="False" AutoPostBack="true" OnActiveTabChanged="TabContainerCTM_ActiveTabChanged">
                            <cc1:TabPanel ID="tabTransaction" runat="server" HeaderText="<%$ Resources:Text, Transaction %>">
                                <ContentTemplate>
                                    <asp:Panel ID="panTransaction" runat="server">
                                        <table width="100%">
                                            <tr style="height:30px">
                                                <td style="vertical-align: top; width:220px">
                                                    <asp:Label ID="lblTabTransactionTransactionNoText" runat="server" Text="<%$ Resources:Text, TransactionNo %>" style="position:relative;top:2px"></asp:Label></td>
                                                <td style="vertical-align: top; width:305px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabTransactionTransactionNo" runat="server" Width="176" onChange="convertToUpper(this)"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabTransactionTransactionNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>        
                                                </td>
                                                <td style="vertical-align: top; width:175px;padding-left:0px">
                                                    <asp:Label ID="lblTabTransactionHealthProfessionText" runat="server" Text="<%$ Resources:Text, HealthProf %>" style="position:relative;top:2px"></asp:Label></td>
                                                <td style="vertical-align: top;">
                                                    <asp:DropDownList ID="ddlTabTransactionHealthProfession" runat="server" AppendDataBoundItems="True" style="height:22px;width:280px;position:relative;top:-1px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabTransactionVaccinesText" runat="server" Text="<%$ Resources:Text, Vaccines %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionVaccines" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="vertical-align: top;padding-left:0px">
                                                    <asp:Label ID="lblTabTransactionDoseText" runat="server" Text="<%$ Resources:Text, Dose %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionDose" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr style="height: 20px">
                                                <td style="vertical-align: top;" colspan="4">
                                                    <hr style="position:relative;top:5px;margin-bottom:0px;margin-top:0px;color:#999999;border-style:solid;border-width:1px 0px 0px 0px" /></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabTransactionTypeOfDateText" runat="server" Text="<%$ Resources: Text, TypeOfDate %>"></asp:Label></td>
                                                <td style="vertical-align: top" colspan ="3">
                                                    <asp:RadioButtonList ID="rblTabTransactionTypeOfDate" style="height:22px;position:relative;;left:-4px; top:-4px" runat="server" AppendDataBoundItems="false" RepeatDirection="Horizontal">
                                                        <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, ServiceDate %>" Value="SD" />
                                                        <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, TransactionDateVU %>" Value="TD" Selected="True" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabTransactionDateText" runat="server" Text="<%$ Resources: Text, Date %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabTransactionDateFrom" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnTabTransactionCalenderDateFrom" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                                <cc1:CalendarExtender ID="CalExtTabTransactionDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabTransactionCalenderDateFrom"
                                                                    TargetControlID="txtTabTransactionDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="TxtExtTabTransactionDateFrom" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtTabTransactionDateFrom" ValidChars="-" >
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="width: 30px; text-align: center">
                                                                <asp:Label ID="lblTabTransactionToText" runat="server" Text="<%$ Resources: Text, To_S %>" style="position:relative;top:-4px"/>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTabTransactionDateTo" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnTabTransactionCalenderDateTo" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                                <cc1:CalendarExtender ID="CalExtTabTransactionDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabTransactionCalenderDateTo"
                                                                    TargetControlID="txtTabTransactionDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="TxtExtTabTransactionDateTo" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtTabTransactionDateTo" ValidChars="-" >
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabTransactionDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-5px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align: top;padding-left:0px">
                                                    <asp:Label ID="lblTabTransactionSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionScheme" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged">
                                                    </asp:DropDownList></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabTransactionTransactionStatusText" runat="server" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionTransactionStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:280px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionStatus_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="vertical-align: top;padding-left:0px">
                                                    <asp:Label ID="lblTabTransactionAuthorizedStatusText" runat="server" Text="<%$ Resources:Text, AuthorizedStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionAuthorizedStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlAuthorizedStatus_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabTransactionInvalidationStatusText" runat="server" Text="<%$ Resources:Text, InvalidationStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionInvalidationStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px" 
                                                        AutoPostBack="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="vertical-align: top;padding-left:0px">
                                                    <asp:Label ID="lblTabTransactionReimbursementMethodText" runat="server" Text="<%$ Resources:Text, ReimbursementMethod %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionReimbursementMethod" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabTransactionMeansOfInputText" runat="server" Text="<%$ Resources: Text, MeansOfInput %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabTransactionMeansOfInput" runat="server" AppendDataBoundItems="False" style="height:22px;width:180px;position:relative;top:-4px">
                                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value="" />                                                
                                                    </asp:DropDownList></td>
                                                <td style="vertical-align: top;padding-left:0px">
                                                    <asp:Label ID="lblTabTransactionRCHRodeText" runat="server" Text="<%$ Resources: Text, SchoolRCHCode %>"/></td>
                                                <td style="vertical-align: top">
                                                    <asp:TextBox ID="txtTabTransactionRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="30" style="position:relative;top:-4px" /></td>
                                            </tr>
                                            <tr style="height: 20px">
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="4">
                                                    <asp:ImageButton ID="ibtnTabTransactionSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                            </tr>
                                        </table>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabTransactionTransactionNo" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                            TargetControlID="txtTabTransactionTransactionNo" ValidChars="-">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabTransactionRCHRode" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                            TargetControlID="txtTabTransactionRCHRode" ValidChars="">
                                        </cc1:FilteredTextBoxExtender>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            
                            <cc1:TabPanel ID="TabServiceProvider" runat="server" HeaderText="<%$ Resources:Text, ServiceProvider_gp %>">
                                <ContentTemplate>
                                    <asp:Panel ID="panServiceProvider" runat="server">
                                        <table>
                                            <tr style="height: 30px">
                                                <td style="vertical-align: top; width:225px">
                                                    <asp:Label ID="lblTabServiceProviderSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top; width:301px">
                                                                                                <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabServiceProviderSPID" runat="server" Width="176" MaxLength="8"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabServiceProviderSPIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                                <td style="vertical-align: top; width:230px">
                                                    <asp:Label ID="lblTabServiceProviderSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top; width:180px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabServiceProviderSPHKID" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="11"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabServiceProviderSPHKIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                            </tr>
                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInEnglish %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabServiceProviderSPName" runat="server" Width="176" onChange="convertToUpper(this)"
                                                                    ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>" MaxLength="40"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabServiceProviderSPNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>                                                
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderSPChiNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderNameInChinese %>"  maxlength ="6" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabServiceProviderSPChiName" runat="server" Width="176" onChange="convertToUpper(this)"
                                                                    MaxLength="6"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabServiceProviderSPChiNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>                                                
                                                </td>
                                            </tr>                                            
                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderBankAccountNoText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabServiceProviderBankAccountNo" runat="server" Width="176" MaxLength="17"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabServiceProviderBankAccountNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                            </tr>

                                            <tr style="height: 20px">
                                                <td style="vertical-align: top;" colspan="4">
                                                    <hr style="position:relative;top:5px;width:auto;margin-bottom:0px;margin-top:0px;color:#999999;border-style:solid;border-width:1px 0px 0px 0px" /></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderTypeOfDateText" runat="server" Text="<%$ Resources: Text, TypeOfDate %>"></asp:Label></td>
                                                <td style="vertical-align: top" colspan ="3">
                                                    <asp:RadioButtonList ID="rblTabServiceProviderTypeOfDate" style="height:22px;position:relative;left:-4px; top:-4px" runat="server" AppendDataBoundItems="false" RepeatDirection="Horizontal">
                                                        <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, ServiceDate %>" Value="SD" />
                                                        <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, TransactionDateVU %>" Value="TD" Selected="True" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderDateText" runat="server" Text="<%$ Resources: Text, Date %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabServiceProviderDateFrom" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnTabServiceProviderCalenderDateFrom" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                                <cc1:CalendarExtender ID="CalExtTabServiceProviderDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabServiceProviderCalenderDateFrom"
                                                                    TargetControlID="txtTabServiceProviderDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="TxtExtTabServiceProviderDateFrom" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtTabServiceProviderDateFrom" ValidChars="-" >
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="width: 30px; text-align: center">
                                                                <asp:Label ID="lblTabServiceProviderToText" runat="server" Text="<%$ Resources: Text, To_S %>" style="position:relative;top:-4px" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTabServiceProviderDateTo" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnTabServiceProviderCalenderDateTo" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                                <cc1:CalendarExtender ID="CalExtTabServiceProviderDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabServiceProviderCalenderDateTo"
                                                                    TargetControlID="txtTabServiceProviderDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="TxtExtTabServiceProviderDateTo" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtTabServiceProviderDateTo" ValidChars="-" >
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabServiceProviderDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-5px"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabServiceProviderScheme" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged">
                                                    </asp:DropDownList></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderTransactionStatusText" runat="server" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabServiceProviderTransactionStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:280px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionStatus_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderAuthorizedStatusText" runat="server" Text="<%$ Resources:Text, AuthorizedStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabServiceProviderAuthorizedStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlAuthorizedStatus_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderInvalidationStatusText" runat="server" Text="<%$ Resources:Text, InvalidationStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabServiceProviderInvalidationStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px" 
                                                        AutoPostBack="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderReimbursementMethodText" runat="server" Text="<%$ Resources:Text, ReimbursementMethod %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabServiceProviderReimbursementMethod" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderMeansOfInputText" runat="server" Text="<%$ Resources: Text, MeansOfInput %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabServiceProviderMeansOfInput" runat="server" AppendDataBoundItems="False" style="height:22px;width:180px;position:relative;top:-4px">                                              
                                                    </asp:DropDownList></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabServiceProviderRCHRodeText" runat="server" Text="<%$ Resources: Text, SchoolRCHCode %>"/></td>
                                                <td style="vertical-align: top">
                                                    <asp:TextBox ID="txtTabServiceProviderRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="30" style="position:relative;top:-4px" /></td>
                                            </tr>
                                        
                                            <tr style="height: 20px">
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="4">
                                                    <asp:ImageButton ID="ibtnTabServiceProviderSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabServiceProviderSPID" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtTabServiceProviderSPID">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabServiceProviderBankAccNo" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtTabServiceProviderBankAccountNo" ValidChars="-">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabServiceProviderSPHKID" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                            TargetControlID="txtTabServiceProviderSPHKID" ValidChars="()">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabServiceProviderSPName" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters"
                                            TargetControlID="txtTabServiceProviderSPName" ValidChars=",'.- ">
                                        </cc1:FilteredTextBoxExtender>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            
                            <cc1:TabPanel ID="tabEHSAccount" runat="server" HeaderText="<%$ Resources:Text, VoucherAccountMaintenance_gp %>">
                                <ContentTemplate>
                                    <asp:Panel ID="panEHSAccount" runat="server">
                                    <table>
                                        <tr style="height: 30px">
                                            <td style="vertical-align: top; width:205px">
                                                <asp:Label ID="lblTabeHSAccountDocTypeText" runat="server" Text="<%$ Resources:Text, eHealthAccountIdentityDocumentType %>" style="position:relative;top:2px"></asp:Label></td>
                                            <td style="vertical-align: top; width:700px" colspan="3">
                                                <asp:DropDownList ID="ddlTabeHSAccountDocType" runat="server" AppendDataBoundItems="True" style="height:22px;width:657px;position:relative;top:-2px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="height: 30px">
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountDocNoText" runat="server" Text="<%$ Resources:Text, eHealthAccountIdentityDocumentNo %>" style="position:relative;top:2px"/>
                                            </td>
                                            <td style="vertical-align: top; width:291px">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtTabeHSAccountDocNo" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="20"/>
                                                        </td>
                                                        <td>
                                                            <asp:Image ID="imgTabeHSAccountDocNo" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                        </td>
                                                    </tr>
                                                </table>  
                                            </td>
                                            <td style="vertical-align: top; width:178px">
                                                <asp:Label ID="lblTabeHSAccountIDText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VoucherAccountID %>" style="position:relative;top:2px"/></td>
                                            <td style="vertical-align: top; width:235px">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTabeHSAccountIDPrefix" runat="server" CssClass="tableTitle"
                                                                Text="" Width="32px" Height="18"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtTabeHSAccountID" runat="server" MaxLength="9" Width="144px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Image ID="imgTabeHSAccountIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 30px">
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountNameText" runat="server" Text="<%$ Resources:Text, AccountNameInEnglish %>" style="position:relative;top:2px"/></td>
                                            <td style="vertical-align: top">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtTabeHSAccountName" runat="server" Width="176" onChange="convertToUpper(this)"
                                                                ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>" MaxLength="82"/>
                                                        </td>
                                                        <td>
                                                            <asp:Image ID="imgTabeHSAccountNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                        </td>
                                                    </tr>
                                                </table>                                                
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountChiNameText" runat="server" Text="<%$ Resources:Text, AccountNameInChinese %>" style="position:relative;top:2px"/></td>
                                            <td style="vertical-align: top">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtTabeHSAccountChiName" runat="server" Width="176" onChange="convertToUpper(this)"
                                                                MaxLength="6"/>
                                                        </td>
                                                        <td>
                                                            <asp:Image ID="imgTabeHSAccountChiNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False"  style="position:relative;top:-1px;left:1px"/>
                                                        </td>
                                                    </tr>
                                                </table>                                                
                                            </td>
                                        </tr>   

                                        <tr style="height: 20px">
                                            <td style="vertical-align: top;" colspan="4">
                                                <hr style="position:relative;top:5px;left:-27px;width:867px;margin-bottom:0px;margin-top:0px;color:#999999;border-style:solid;border-width:1px 0px 0px 0px" /></td>
                                        </tr>

                                        <tr style="height: 30px">
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountTypeOfDateText" runat="server" Text="<%$ Resources: Text, TypeOfDate %>"></asp:Label></td>
                                            <td style="vertical-align: top" colspan ="3">
                                                <asp:RadioButtonList ID="rblTabeHSAccountTypeOfDate" style="height:22px;position:relative;left:-4px; top:-4px" runat="server" AppendDataBoundItems="false" RepeatDirection="Horizontal">
                                                    <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, ServiceDate %>" Value="SD" />
                                                    <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, TransactionDateVU %>" Value="TD" Selected="True" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <tr style="height: 30px">
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountDateText" runat="server" Text="<%$ Resources: Text, Date %>"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtTabeHSAccountDateFrom" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                            <asp:ImageButton ID="ibtnTabeHSAccountCalenderDateFrom" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                            <cc1:CalendarExtender ID="CalExtTabeHSAccountDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabeHSAccountCalenderDateFrom"
                                                                TargetControlID="txtTabeHSAccountDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="TxtExtTabeHSAccountDateFrom" runat="server" FilterType="Custom, Numbers"
                                                                 TargetControlID="txtTabeHSAccountDateFrom" ValidChars="-" >
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td style="width: 30px; text-align: center">
                                                            <asp:Label ID="lblTabeHSAccountToText" runat="server" Text="<%$ Resources: Text, To_S %>" style="position:relative;top:-4px" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTabeHSAccountDateTo" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                            <asp:ImageButton ID="ibtnTabeHSAccountCalenderDateTo" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                            <cc1:CalendarExtender ID="CalExtTabeHSAccountDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabeHSAccountCalenderDateTo"
                                                                TargetControlID="txtTabeHSAccountDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="TxtExtTabeHSAccountDateTo" runat="server" FilterType="Custom, Numbers"
                                                                 TargetControlID="txtTabeHSAccountDateTo" ValidChars="-" >
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Image ID="imgTabeHSAccountDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-5px"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <asp:DropDownList ID="ddlTabeHSAccountScheme" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                        </tr>

                                        <tr style="height: 30px">
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountTransactionStatusText" runat="server" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <asp:DropDownList ID="ddlTabeHSAccountTransactionStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:280px;position:relative;top:-4px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionStatus_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountAuthorizedStatusText" runat="server" Text="<%$ Resources:Text, AuthorizedStatus %>"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <asp:DropDownList ID="ddlTabeHSAccountAuthorizedStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlAuthorizedStatus_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr style="height: 30px">
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountInvalidationStatusText" runat="server" Text="<%$ Resources:Text, InvalidationStatus %>"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <asp:DropDownList ID="ddlTabeHSAccountInvalidationStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px" 
                                                    AutoPostBack="False">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountReimbursementMethodText" runat="server" Text="<%$ Resources:Text, ReimbursementMethod %>"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <asp:DropDownList ID="ddlTabeHSAccountReimbursementMethod" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                    AutoPostBack="False">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr style="height: 30px">
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountMeansOfInputText" runat="server" Text="<%$ Resources: Text, MeansOfInput %>"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <asp:DropDownList ID="ddlTabeHSAccountMeansOfInput" runat="server" AppendDataBoundItems="False" style="height:22px;width:180px;position:relative;top:-4px">                                               
                                                </asp:DropDownList></td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblTabeHSAccountRCHRodeText" runat="server" Text="<%$ Resources: Text, SchoolRCHCode %>"/></td>
                                            <td style="vertical-align: top">
                                                <asp:TextBox ID="txtTabeHSAccountRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="30" style="position:relative;top:-4px" /></td>
                                        </tr>
                                        
                                        <tr style="height: 20px">
                                        </tr>
                                        <tr>
                                            <td style="text-align: center" colspan="4">
                                                <asp:ImageButton ID="ibtnTabeHSAccountSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
<%--                                                <asp:ImageButton ID="ibtnNewClaimTransaction" runat="server" ImageUrl="<%$ Resources:ImageUrl, NewClaimTransactionBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, NewClaimTransactionBtn %>" OnClick="ibtnNewClaimTransaction_Click" />--%>
                                                <asp:ImageButton ID="ibtnNewClaimTransaction" runat="server" ImageUrl="<%$ Resources:ImageUrl, NewClaimTransactionBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, NewClaimTransactionBtn %>" Visible="false" />
                                            </td>                                            
                                        </tr>
                                    </table>
                                    <cc1:FilteredTextBoxExtender ID="txtExtTabeHSAccountDocNo" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                        TargetControlID="txtTabeHSAccountDocNo" ValidChars="()/-">
                                    </cc1:FilteredTextBoxExtender>
                                    <cc1:FilteredTextBoxExtender ID="txtExtTabeHSAccountID" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtTabeHSAccountID">
                                    </cc1:FilteredTextBoxExtender>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>

                            <cc1:TabPanel ID="tabAdvancedSearch" runat="server" HeaderText="<%$ Resources:Text, AdvancedSearch %>">
                                <ContentTemplate>
                                    <asp:Panel ID="panAdvancedSearch" runat="server">
                                        <table width="100%">
                                            <tr style="height:30px">
                                                <td style="vertical-align: top; width:229px">
                                                    <asp:Label ID="lblTabAdvancedSearchTransactionNoText" runat="server" Text="<%$ Resources:Text, TransactionNo %>" style="position:relative;top:2px"></asp:Label></td>
                                                <td style="vertical-align: top; width:306px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchTransactionNo" runat="server" Width="176" onChange="convertToUpper(this)"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchTransactionNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                                <td style="vertical-align: top; width:180px">
                                                    <asp:Label ID="lblTabAdvancedSearchHealthProfessionText" runat="server" Text="<%$ Resources:Text, HealthProf %>" style="position:relative;top:2px"></asp:Label></td>
                                                <td style="vertical-align: top; width:235px">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchHealthProfession" runat="server" AppendDataBoundItems="True" style="height:22px;width:280px;position:relative;top:-1px">
                                                    </asp:DropDownList></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchTypeOfDateText" runat="server" Text="<%$ Resources: Text, TypeOfDate %>"></asp:Label></td>
                                                <td style="vertical-align: top" colspan ="3">
                                                    <asp:RadioButtonList ID="rblTabAdvancedSearchTypeOfDate" style="height:22px;position:relative;;left:-4px; top:-4px" runat="server" AppendDataBoundItems="false" RepeatDirection="Horizontal">
                                                        <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, ServiceDate %>" Value="SD" />
                                                        <asp:ListItem style="position:relative;top:-3px" Text="<%$ Resources:Text, TransactionDateVU %>" Value="TD" Selected="True" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchDateText" runat="server" Text="<%$ Resources: Text, Date %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchDateFrom" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnTabAdvancedSearchCalenderDateFrom" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                                <cc1:CalendarExtender ID="CalExtTabAdvancedSearchDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabAdvancedSearchCalenderDateFrom"
                                                                    TargetControlID="txtTabAdvancedSearchDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="TxtExtTabAdvancedSearchDateFrom" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtTabAdvancedSearchDateFrom" ValidChars="-" >
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="width: 30px; text-align: center">
                                                                <asp:Label ID="lblTabAdvancedSearchToText" runat="server" Text="<%$ Resources: Text, To_S %>" style="position:relative;top:-4px"/>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchDateTo" runat="server" Width="70" MaxLength="10" style="position:relative;top:-4px"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnTabAdvancedSearchCalenderDateTo" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:-1px"/>
                                                                <cc1:CalendarExtender ID="CalExtTabAdvancedSearchDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnTabAdvancedSearchCalenderDateTo"
                                                                    TargetControlID="txtTabAdvancedSearchDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="TxtExtTabAdvancedSearchDateTo" runat="server" FilterType="Custom, Numbers"
                                                                     TargetControlID="txtTabAdvancedSearchDateTo" ValidChars="-" >
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-5px"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchScheme" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlScheme_SelectedIndexChanged">
                                                    </asp:DropDownList></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchTransactionStatusText" runat="server" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchTransactionStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:280px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionStatus_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchAuthorizedStatusText" runat="server" Text="<%$ Resources:Text, AuthorizedStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchAuthorizedStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlAuthorizedStatus_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchInvalidationStatusText" runat="server" Text="<%$ Resources:Text, InvalidationStatus %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchInvalidationStatus" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px" 
                                                        AutoPostBack="False">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchReimbursementMethodText" runat="server" Text="<%$ Resources:Text, ReimbursementMethod %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchReimbursementMethod" runat="server" AppendDataBoundItems="True" style="height:22px;width:180px;position:relative;top:-4px"
                                                        AutoPostBack="False">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchMeansOfInputText" runat="server" Text="<%$ Resources: Text, MeansOfInput %>"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchMeansOfInput" runat="server" AppendDataBoundItems="False" style="height:22px;width:180px;position:relative;top:-4px">                                               
                                                    </asp:DropDownList></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchRCHRodeText" runat="server" Text="<%$ Resources: Text, SchoolRCHCode %>"/></td>
                                                <td style="vertical-align: top">
                                                    <asp:TextBox ID="txtTabAdvancedSearchRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="30" style="position:relative;top:-4px" /></td>
                                            </tr>

                                            <tr style="height: 20px">
                                                <td style="vertical-align: top;" colspan="4">
                                                    <hr style="position:relative;top:5px;margin-bottom:0; margin-top:0; color:#999999; border-style:solid; border-width:1px 0 0 0;" /></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchSPID" runat="server" Width="176" MaxLength="8"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchSPIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchSPHKID" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="11"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchSPHKIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                            </tr>
                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchSPName" runat="server" Width="176" onChange="convertToUpper(this)"
                                                                    ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>" MaxLength="40"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchSPNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchBankAccountNoText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchBankAccountNo" runat="server" Width="176" MaxLength="17"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchBankAccountNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr style="height: 20px">
                                                <td style="vertical-align: top;" colspan="4">
                                                    <hr style="position:relative;top:5px;margin-bottom:0;margin-top:0;color:#999999;border-style:solid;border-width:1px 0 0 0;" /></td>
                                            </tr>

                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchDocTypeText" runat="server" Text="<%$ Resources:Text, eHealthAccountIdentityDocumentType %>" style="position:relative;top:2px"></asp:Label></td>
                                                <td style="vertical-align: top; width:730px" colspan="3">
                                                    <asp:DropDownList ID="ddlTabAdvancedSearchDocType" runat="server" AppendDataBoundItems="True" style="height:22px;width:657px;position:relative;top:-1px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr style="height: 30px">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchDocNoText" runat="server" Text="<%$ Resources:Text, eHealthAccountIdentityDocumentNo %>" style="position:relative;top:2px"/>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchDocNo" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="20"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchDocNo" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/>
                                                            </td>
                                                        </tr>
                                                    </table>  
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblTabAdvancedSearchAccountIDText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VoucherAccountID %>" style="position:relative;top:2px"/></td>
                                                <td style="vertical-align: top">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblTabAdvancedSearchAccountIDPrefix" runat="server" CssClass="tableTitle"
                                                                    Text="" Width="32px" Height="18"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="txtTabAdvancedSearchAccountID" runat="server" MaxLength="9" Width="144px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgTabAdvancedSearchAccountIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="position:relative;top:-1px;left:1px"/></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr style="height: 20px">
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="4">
                                                    <asp:ImageButton ID="ibtnTabAdvancedSearchSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                            </tr>
                                        </table>                                    
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabAdvancedSearchSPID" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtTabAdvancedSearchSPID">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabAdvancedSearchTransNo" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                            TargetControlID="txtTabAdvancedSearchTransactionNo" ValidChars="-">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabAdvancedSearchBankAccNo" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtTabAdvancedSearchBankAccountNo" ValidChars="-">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabAdvancedSearchSPHKID" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                            TargetControlID="txtTabAdvancedSearchSPHKID" ValidChars="()">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabAdvancedSearchSPName" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters"
                                            TargetControlID="txtTabAdvancedSearchSPName" ValidChars=",'.- ">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabAdvancedSearchDocNo" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"
                                            TargetControlID="txtTabAdvancedSearchDocNo" ValidChars="()/-">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtExtTabAdvancedSearchAccountID" runat="server" FilterType="Custom, Numbers"
                                            TargetControlID="txtTabAdvancedSearchAccountID">
                                        </cc1:FilteredTextBoxExtender>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer> 

                    </asp:View>
                    <asp:View ID="ViewTransaction" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />
                        
                        <uc6:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panSearchCriteriaReview"/>
                        
                        <asp:Panel ID="panSearchCriteriaReview" runat="server" width="1000px" >
                            <table style="width: 917px">
                                <tr>
                                    <td style="width: 230px;vertical-align: top">
                                        <asp:Label ID="lblAspectText" runat="server" Text="<%$ Resources:Text, SearchBy %>"></asp:Label></td>
                                    <td style="width: 250px;vertical-align: top">
                                        <asp:Label ID="lblAspect" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px;vertical-align: top" />
                                    <td style="vertical-align: top" />
                                </tr>
                                <tr>
                                    <td style="width: 230px;vertical-align: top">
                                        <asp:Label ID="lblRTransactionNoText" runat="server" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                                    <td style="width: 250px;vertical-align: top">
                                        <asp:Label ID="lblRTransactionNo" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 230px;vertical-align: top">
                                        <asp:Label ID="lblRHealthProfessionText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRHealthProfession" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 230px;vertical-align: top">
                                        <asp:Label ID="lblRVaccineText" runat="server" Text="<%$ Resources:Text, Vaccines %>"></asp:Label></td>
                                    <td style="width: 250px;vertical-align: top">
                                        <asp:Label ID="lblRVaccine" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 230px;vertical-align: top">
                                        <asp:Label ID="lblRDoseText" runat="server" Text="<%$ Resources:Text, Dose %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRDose" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>

                                <tr style="height: 10px">
                                     <td style="vertical-align: top;" colspan="4">
                                         <hr style="position:relative;top:5px;margin-bottom:0px;margin-top:0px;color:#999999;border-style:solid;border-width:1px 0px 0px 0px" /></td>
                                </tr>                              
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRDateText" runat="server"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRDate" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSchemeCodeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSchemeCode" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRTransactionStatusText" runat="server" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRTransactionStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRAuthorizedStatusText" runat="server" Text="<%$ Resources:Text, AuthorizedStatus %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRAuthorizedStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRInvalidationStatusText" runat="server" Text="<%$ Resources:Text, InvalidationStatus %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRInvalidationStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRReimbursementMethodText" runat="server" Text="<%$ Resources:Text, ReimbursementMethod %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRReimbursementMethod" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRMeansOfInputText" runat="server" Text="<%$ Resources:Text, MeansOfInput %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRMeansOfInput" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSchoolRCHCodeText" runat="server" Text="<%$ Resources:Text, SchoolRCHCode %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                 <tr style="height: 10px">
                                     <td style="vertical-align: top;" colspan="4">
                                         <hr style="position:relative;top:5px;margin-bottom:0px;margin-top:0px;color:#999999;border-style:solid;border-width:1px 0px 0px 0px" /></td>
                                </tr> 
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRSPHKID" runat="server" CssClass="tableText"></asp:Label></td>
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
                                        <asp:Label ID="lblRBankAccountNoText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRBankAccountNo" runat="server" Font-Bold="True" CssClass="tableText"></asp:Label></td>                                </tr>

                                 <tr style="height: 10px">
                                     <td style="vertical-align: top;" colspan="4">
                                         <hr style="position:relative;top:5px;margin-bottom:0px;margin-top:0px;color:#999999;border-style:solid;border-width:1px 0px 0px 0px" /></td>
                                </tr> 
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblREHealthDocTypeText" runat="server" Text="<%$ Resources:Text, eHealthAccountIdentityDocumentType %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblREHealthDocType" runat="server" CssClass="tableText"></asp:Label></td>                                    
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblREHealthDocNoText" runat="server" Text="<%$ Resources:Text, eHealthAccountIdentityDocumentNo %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblREHealthDocNo" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblREHealthAccountIDText" runat="server" Text="<%$ Resources:Text, VoucherAccountID %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblREHealthAccountID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblReHANameText" runat="server" Text="<%$ Resources:Text, AccountNameInEnglish %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblReHAName" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblReHAChiNameText" runat="server" Text="<%$ Resources:Text, AccountNameInChinese %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblReHAChiName" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvTransaction" runat="server" AutoGenerateColumns="False" Width="1295px"
                            AllowPaging="True" AllowSorting="True" OnRowCommand="gvTransaction_RowCommand"
                            OnPageIndexChanging="gvTransaction_PageIndexChanging" OnRowDataBound="gvTransaction_RowDataBound"
                            OnPreRender="gvTransaction_PreRender" OnSorting="gvTransaction_Sorting">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="HeaderLevelCheckBox" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_selected" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" VerticalAlign="Top" HorizontalAlign="Center" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>" ShowHeader="False"
                                    SortExpression="transNum">
                                    <ItemStyle Wrap="false" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top">
                                                    <asp:LinkButton ID="lbtn_transNum" runat="server" CausesValidation="false" CommandName=""
                                                        Text='<%# Eval("transNum") %>'></asp:LinkButton>
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:Image ID="imgWarning" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                        Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hfTransactionNo" runat="server" Value='<%# Eval("transNum") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="transDate" HeaderText="<%$ Resources:Text, TransactionTime %>">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle Width="85px" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGTransactionTime" runat="server" Text='<%# Eval("transDate") %>'></asp:Label><br />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ServiceProvider" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblREname" runat="server" Text='<%# Eval("ServiceProvider") %>'></asp:Label><br />
                                        <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("spChiName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="270px" VerticalAlign="Top" />
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
                                <asp:TemplateField HeaderText="<%$ Resources:Text, BankAccountNo %>" SortExpression="bankAccount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaskedBank" runat="server"></asp:Label>
                                        <asp:Label ID="lblOriBank" runat="server" Text='<%# Eval("bankAccount") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px" />
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
                                    <ItemStyle Width="55px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="VoucherRedeem" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>"
                                    SortExpression="voucherRedeem">
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="75px" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:BoundField>--%>
                                 <asp:TemplateField SortExpression="totalAmount" HeaderText='<%$ Resources:Text, TotalRedeemAmountSign %>'>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("totalAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="90px"  VerticalAlign="Top" />
                                      <HeaderStyle VerticalAlign="Top" /> 
                                 </asp:TemplateField>     
                                 <asp:TemplateField SortExpression="totalAmountRMB" HeaderText='<%$ Resources:Text, TotalRedeemAmountSignRMB %>'>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("totalAmountRMB") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="90px"  VerticalAlign="Top" />
                                      <HeaderStyle VerticalAlign="Top" /> 
                                 </asp:TemplateField>   
                                <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionStatus %>" SortExpression="transstatus">
                                    <ItemStyle Width="120px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGTransactionStatus" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfGTransactionStatus" runat="server" Value='<%# Eval("transstatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, AuthorizedStatus %>" SortExpression="authorizedStatus">
                                    <ItemStyle Width="100px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGAuthorisedStatus" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfGAuthorizedStatus" runat="server" Value='<%# Eval("authorizedStatus") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, InvalidationStatus %>" SortExpression="Invalidation">
                                    <ItemStyle Width="70px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGInvalidation" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfGInvalidation" runat="server" Value='<%# Eval("Invalidation") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, MeansOfInput %>" SortExpression="Means_Of_Input">
                                    <ItemStyle Width="70px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGMeansOfInput" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfGMeansOfInput" runat="server" Value='<%# Eval("Means_Of_Input") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, SchoolRCHCode %>" SortExpression="SchoolOrRCH_Code">
                                    <ItemStyle Width="70px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGRCHCode" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfGRCHCode" runat="server" Value='<%# Eval("SchoolOrRCH_Code")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Panel ID="panRecordSummary" runat="server">
                            <asp:Label ID="lblRecordSummaryText" runat="server" Text="<%$ Resources: Text, SearchSummary %>" style="font-size:16px;font-family:Arial;color:#000000;font-weight:bold"></asp:Label><br />
                            <asp:Table ID="tblRecordSummary" runat="server" cellpadding="0" cellspacing="0" BorderWidth="2" style="background-color: white; text-align: center;width:1105px">
                                <asp:TableRow ID="trRecordSummaryTitle" runat="server" style="height: 30px" BorderWidth="2">
                                    <asp:TableCell style="width: 135px; border-top-style: none; border-left-style: none" />
                                    <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingComfirmText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, PendingConfirmation %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell style="width: 200px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingEHSAcctValidationText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, PendingVoucherAccountValidation %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryReadyToReimburseText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, ReadytoReimburse %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryReimbursedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Reimbursed %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummarySuspendedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, ClaimSuspended %>"></asp:Label></asp:TableCell>                                         
                                    <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryVoidedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Voided %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell style="width: 130px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingApprovalBOText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, PendingApprovalBO %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell style="width: 100px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryRemovedBOText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, RemovedBO %>"></asp:Label></asp:TableCell>                                    
                                    <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryIncompleteText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Incomplete %>"></asp:Label></asp:TableCell>
                                   <%-- <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryJoinedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Joined %>"></asp:Label></asp:TableCell>--%>                                
                                </asp:TableRow>

                                <asp:TableRow ID="trRecordSummaryTotalAmount" runat="server" BorderWidth="1">
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryTotalAmountText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TotalAmountClaimedSign %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingComfirm" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingEHSAcctValidation" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryReadyToReimburse" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryReimbursed" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummarySuspended" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryVoided" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingApprovalBO" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryRemovedBO" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2" ID="tdSummaryIncomplete">
                                        <asp:Label ID="lblSummaryIncomplete" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>     
                                    <%--<asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryJoined" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>   --%>                                                                                  
                                </asp:TableRow>

<%--                                <asp:TableRow ID="trRecordSummaryTotalAmountRMB" runat="server" BorderWidth="1">
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryTotalAmountTextRMB" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TotalAmountClaimedSignRMB %>"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingComfirmRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryPendingEHSAcctValidationRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryReadyToReimburseRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryReimbursedRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummarySuspendedRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">                                    
                                        <asp:Label ID="lblSummaryVoidedRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">                                    
                                        <asp:Label ID="lblSummaryPendingApprovalBORMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">                                    
                                        <asp:Label ID="lblSummaryRemovedBORMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>
                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryIncompleteRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>  
                                    <asp:TableCell BorderWidth="2">                                    
                                        <asp:Label ID="lblSummaryJoinedRMB" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>                                                                                                                            
                                </asp:TableRow>--%>
                            </asp:Table>
                            <br />
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="panActionButton" runat="server" Width="950px">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 100px">
                                        <asp:ImageButton ID="ibtnBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBack_Click" />
                                    </td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ibtnBatchReactivate" runat="server" ImageUrl="<%$ Resources:ImageUrl, MarkSelectedReactivateBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, MarkSelectedReactivateBtn %>" OnClick="ibtnBatchReactivate_Click" />
                                        <asp:ImageButton ID="ibtnBatchSuspend" runat="server" ImageUrl="<%$ Resources:ImageUrl, MarkSelectedSuspendBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, MarkSelectedSuspendBtn %>" OnClick="ibtnBatchSuspend_Click" />
                                        <asp:ImageButton ID="ibtnBatchVoid" runat="server" ImageUrl="<%$ Resources:ImageUrl, MarkSelectedVoidBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, MarkSelectedVoidBtn %>" OnClick="ibtnBatchVoid_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="ViewBatchActionConfirm" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblBatchActionHeading" runat="server"></asp:Label></div>
                        <asp:GridView ID="gvBatchActionConfirm" runat="server" AutoGenerateColumns="False"
                            OnRowDataBound="gvBatchActionConfirm_RowDataBound" Width="300px">
                            <Columns>
                                <asp:BoundField DataField="lineNum" Visible="False" />
                                <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBTransactionNo" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfBTransactionNo" runat="server" Value='<%# Eval("transNum") %>' />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBTransactionDate" runat="server" Text='<%# Eval("transDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:MultiView ID="MultiViewBatchAction" runat="server">
                            <asp:View ID="ViewBatchInputReason" runat="server">
                                <table style="width: 814px">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:Label ID="lblBatchReasonText" runat="server"></asp:Label></td>
                                                    <td>
                                                        <asp:TextBox ID="txtBatchReason" runat="server" Width="200px" CssClass="TextBoxChi"></asp:TextBox>
                                                        <asp:Image ID="imgAlertBatchReason" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                                </tr>
                                                <tr style="height: 10px">
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="ibtnBatchInputReasonCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnBatchInputReasonCancel_Click" />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:ImageButton ID="ibtnBatchInputReasonSave" runat="server" ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, SaveBtn %>" OnClick="ibtnBatchInputReasonSave_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewBatchConfirmReason" runat="server">
                                <table style="width: 814px">
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 200px">
                                                        <asp:Label ID="lblConfirmBatchReasonText" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblConfirmBatchReason" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px">
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="ibtnBatchConfirmReasonCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBatchConfirmReasonCancel_Click" />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:ImageButton ID="ibtnBatchConfirmReasonConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnBatchConfirmReasonConfirm_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </asp:View>
                    <asp:View ID="ViewComplete" runat="server">
                        <asp:Panel ID="panVoidResult" runat="server">
                            <asp:GridView ID="gvVoidResult" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvVoidResult_RowDataBound">
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblLineNum" runat="server" Text='<%# Eval("lineNum") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>">
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle Width="150px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransactionNo" runat="server" Text='<%# Eval("transNum") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, VoidTranID %>">
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle Width="150px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblVoidReference" runat="server" Text='<%# Eval("voidRef") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, VoidTranTime %>">
                                        <HeaderStyle VerticalAlign="Top" />
                                        <ItemStyle Width="150px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblVoidTime" runat="server" Text='<%# Eval("void_dtm") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <br />
                        </asp:Panel>
                        <table>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewDetail" runat="server">
                        <asp:Panel ID="panDetail" runat="server" Width="950px">
                            <uc3:ClaimTransDetail ID="udcClaimTransDetail" runat="server" />
                            <asp:HiddenField ID="hfCurrentDetailTransactionNo" runat="server" />
                            <asp:MultiView ID="MultiViewDetailAction" runat="server">
                                <asp:View ID="ViewButton" runat="server">
                                    <table style="width: 955px" border="0">
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <center>
                                                    <table style="width: 10px" border="0">
                                                        <tr>
                                                            <td style="width: 1px">
                                                                <asp:ImageButton ID="ibtnPreviousRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, PreviousRecordBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, PreviousRecordBtn %>" OnClick="ibtnPreviousRecord_Click" />
                                                            </td>
                                                            <td style="width: 1px; text-align: center">
                                                                <asp:Panel ID="panRecordNo" runat="server" BorderStyle="Inset" Width="90px">
                                                                    <asp:Label ID="lblCurrentRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                                    <asp:Label ID="lblSeparator" runat="server" Font-Size="Large" Text="/"></asp:Label>
                                                                    <asp:Label ID="lblMaxRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                                </asp:Panel>
                                                            </td>
                                                            <td style="width: 1px">
                                                                <asp:ImageButton ID="ibtnNextRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextRecordBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, NextRecordBtn %>" OnClick="ibtnNextRecord_Click" />
                                                            </td>
                                                    </table>
                                                </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="ibtnDetailBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnDetailBack_Click" />
                                                <asp:ImageButton  ID="btnFocus" runat="server" ImageUrl="~/Images/others/arrowblank.png" Width="0" Height="0" />
                                            </td>
                                            <td align="center">
                                                <asp:Panel ID="pnlNormalTransactionBtnAction" runat="server">
                                                    <asp:ImageButton ID="ibtnReactivate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReactivateBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ReactivateBtn %>" OnClick="ibtnReactivate_Click" />
                                                    <asp:ImageButton ID="ibtnSuspend" runat="server" ImageUrl="<%$ Resources:ImageUrl, SuspendBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, SuspendBtn %>" OnClick="ibtnSuspend_Click" />
                                                    <asp:ImageButton ID="ibtnVoid" runat="server" ImageUrl="<%$ Resources:ImageUrl, VoidBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, VoidBtn %>" OnClick="ibtnVoid_Click" />
                                                    <asp:ImageButton ID="ibtnCancelInvalidation" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelInvalidationBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, CancelInvalidationBtn %>" OnClick="ibtnCancelInvalidation_Click" />
                                                    <asp:ImageButton ID="ibtnMarkInvalid" runat="server" ImageUrl="<%$ Resources:ImageUrl, MarkInvalidBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, MarkInvalidBtn %>" OnClick="ibtnMarkInvalid_Click" />
                                                    <asp:ImageButton ID="ibtnConfirmInvalid" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmInvalidBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ConfirmInvalidBtn %>" OnClick="ibtnConfirmInvalid_Click" />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlTransactionCreatedByBoBtnAction" runat="server">
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeleteBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, DeleteBtn %>" OnClick="ibtnDelete_Click" />
                                                </asp:Panel>
                                                
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
                                    <table style="width: 100%" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td style="width: 50px">
                                                <asp:ImageButton ID="ibtnInputReasonCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnInputReasonCancel_Click" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="ibtnInputReasonSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnInputReasonSave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="ViewConfirmReason" runat="server">
                                    <table style="width: 100%;" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td style="width: 1%">
                                            </td>
                                            <td style="vertical-align: top; width: 200px">
                                                <asp:Label ID="lblConfirmReasonText" runat="server"></asp:Label>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblConfirmReason" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px">
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 50px">
                                                <asp:ImageButton ID="ibtnConfirmReasonCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnConfirmReasonCancel_Click" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="ibtnConfirmReasonConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnConfirmReasonConfirm_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="ViewInputInvalidationReason" runat="server">
                                    <table style="width: 100%" border="0" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td style="width: 1%">
                                                &nbsp;
                                            </td>
                                            <td style="vertical-align: top; width: 200px">
                                                <asp:Label ID="lblInvalidationReasonText" runat="server" Text="<%$ Resources:Text, InvalidationReason %>"></asp:Label>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:DropDownList ID="ddlInvalidationReason" runat="server" Width="507px" AppendDataBoundItems="True">
                                                    <asp:ListItem Text="<%$ Resources:Text, PleaseSelect %>" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Image ID="imgAlertInvalidationReason" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="vertical-align: top; width: 200px">
                                            </td>
                                            <td style="vertical-align: top">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="bottom" style="width: 200px">
                                                            <asp:Label ID="lblReasonRemarkText" runat="server" Text="<%$ Resources:Text, InvalidationReasonRemark %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReasonRemark" runat="server" Width="300px" MaxLength="255" CssClass="TextBoxChi"></asp:TextBox>
                                                            <asp:Image ID="imgAlertReasonRemark" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px">
                                        </tr>
                                    </table>
                                    <table style="width: 100%" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td style="width: 50px">
                                                <asp:ImageButton ID="ibtnInputInvalidationReasonCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnInputInvalidationReasonCancel_Click" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="ibtnInputInvalidationReasonSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnInputInvalidationReasonSave_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="ViewConfirmInvalidationReason" runat="server">
                                    <table style="width: 100%;" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td style="width: 1%">
                                            </td>
                                            <td style="vertical-align: top; width: 200px">
                                                <asp:Label ID="lblConfirmInvalidationReasonText" runat="server" Text="<%$ Resources:Text, InvalidationReason %>"></asp:Label>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblConfirmInvalidationReason" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 1%">
                                            </td>
                                            <td style="vertical-align: top; width: 200px">
                                                <asp:Label ID="lblConfirmReasonRemarkText" runat="server" Text="<%$ Resources:Text, InvalidationReasonRemark %>"></asp:Label>
                                            </td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblConfirmReasonRemark" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px">
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 50px">
                                                <asp:ImageButton ID="ibtnConfirmInvalidationReasonCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnConfirmInvalidationReasonCancel_Click" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="ibtnConfirmInvalidationReasonConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnConfirmInvalidationReasonConfirm_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                            </asp:MultiView>
                            <asp:HiddenField ID="HiddenFieldAction" runat="server" />
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="ViewCompleteInvalidation" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtnReturnDetail" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewNewClaimTransaction" runat="server">
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
                                                Text="<%$ Resources:Text, VoucherAccountID %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSerachAccountResultEHSAccountID" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gvSearchAccount" runat="server" AllowPaging="True" AllowSorting="True"
                                    Width="950px" BackColor="White" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSearchIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="20px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Voucher_Acc_ID" HeaderText="<%$ Resources:Text, VoucherAccountID %>">
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
                                                <asp:Label ID="lblName" runat="server"></asp:Label><br />
                                                <asp:Label ID="lblCName" runat="server" Font-Names="HA_MingLiu"></asp:Label>
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
                                <div class="headingText">
                                    <asp:Label ID="lblEnterClaimDetails" runat="server" Text="<%$ Resources:Text, EnterDetails %>"
                                        Font-Bold="True"></asp:Label>
                                    <asp:ImageButton ID="ibtnVaccinationRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, VaccinationRecordSmallBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, VaccinationRecordSmallBtn %>" OnClick="ibtnVaccinationRecord_Click" />
                                </div>
                                <table style="width: 100%" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: top">
                                            <asp:Label ID="lblVVoucherRecipientHeading" runat="server" Font-Underline="True"
                                                Text="<%$ Resources:Text, VRInformation %>" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ucReadOnlyDocumnetType ID="udceHealthAccountInfo" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <asp:MultiView ID="mvEnterDetails" runat="server">
                                                <asp:View ID="viewCreationDetails" runat="server">
                                                    <table style="width: 100%" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td style="vertical-align: top; height: 25px;">
                                                                <asp:Label ID="lblEnterCreationDetailTransactionHeading" runat="server" Font-Underline="True"
                                                                    Text="<%$ Resources:Text, SPPaymentInfo %>" CssClass="tableText"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterCreationDetailSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEnterCreationDetailSPID" runat="server" MaxLength="8" Width="60px"></asp:TextBox>
                                                                <asp:Image ID="imgEnterCreationDetailSPIDError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                                                <asp:ImageButton ID="ibtnSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, SearchSBtn %>" OnClick="ibtnSearchSP_Click" />
                                                                <asp:ImageButton ID="ibtnClearSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, ClearBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ClearSBtn %>" OnClick="ibtnClearSearchSP_Click" />
                                                                <cc1:FilteredTextBoxExtender ID="FilteredSearchSPID" runat="server" FilterType="Custom, Numbers"
                                                                    TargetControlID="txtEnterCreationDetailSPID">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterCreationDetailSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblEnterCreationDetailSPName" runat="server" CssClass="tableText"></asp:Label><asp:Label
                                                                    ID="lblEnterCreationDetailSPStatus" runat="server" CssClass="tableText" ForeColor="red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterCreationDetailPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlEnterCreationDetailPractice" runat="server" Width="485px"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList><asp:Label ID="lblEnterCreationDetailPracticeStatus" runat="server"
                                                                    CssClass="tableText" ForeColor="red"></asp:Label>
                                                                <asp:Image ID="imgEnterCreationDetailPractice" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterCreationDetailCreationReasonText" runat="server" Text="<%$ Resources:Text, CreationReason %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlEnterCreationDetailCreationReason" runat="server" Width="485px">
                                                                </asp:DropDownList>
                                                                <asp:Image ID="imgEnterCreationDetailCreationReason" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;">
                                                            </td>
                                                            <td valign="top">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td valign="bottom" style="width: 80px">
                                                                            <asp:Label ID="lblEnterCreationDetailRemarksText" runat="server" Text="<%$ Resources:Text, Remarks %>"
                                                                                Height="25px"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEnterCreationDetailRemarks" runat="server" MaxLength="255" Width="400px"
                                                                                CssClass="TextBoxChi"></asp:TextBox>
                                                                            <asp:Image ID="imgEnterCreationDetailRemarks" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterCreationDetailPaymentMethodText" runat="server" Text="<%$ Resources:Text, PaymentSettlement %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlEnterCreationDetailPaymentMethod" runat="server" Width="485px">
                                                                </asp:DropDownList>
                                                                <asp:Image ID="imgEnterCreationDetailPaymentMethod" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;">
                                                            </td>
                                                            <td valign="top">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td valign="bottom" style="width: 80px">
                                                                            <asp:Label ID="lblEnterCreationDetailPaymentRemarksText" runat="server" Text="<%$ Resources:Text, Remarks %>"
                                                                                Height="25px"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEnterCreationDetailPaymentRemarks" runat="server" MaxLength="255"
                                                                                Width="400px" CssClass="TextBoxChi"></asp:TextBox>
                                                                            <asp:Image ID="imgEnterCreationDetailPaymentRemarks" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 100px" align="left" valign="top">
                                                                <asp:ImageButton ID="ibtnEnterCreationDetailBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnEnterCreationDetailBack_Click">
                                                                </asp:ImageButton>
                                                            </td>
                                                            <td align="center" valign="top">
                                                                <asp:ImageButton ID="ibtnEnterCreationDetailNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnEnterCreationDetailNext_Click" /></td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="viewClaimDetails" runat="server">
                                                    <table style="width: 100%" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td style="vertical-align: top; height: 25px;">
                                                                <asp:Label ID="lblTransactionHeading" runat="server" Font-Underline="True" Text="<%$ Resources:Text, TransactionInformation %>"
                                                                    CssClass="tableText"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 100%" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterClaimDetailSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProvider %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblEnterClaimDetailSPName" runat="server" CssClass="tableText"></asp:Label>
                                                                <asp:Label ID="lblEnterClaimDetailSPID" runat="server" CssClass="tableText"></asp:Label>
                                                                <asp:Label ID="lblEnterClaimDetailSPStatus" runat="server" CssClass="tableText" ForeColor="red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterClaimDetailPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblEnterClaimDetailPractice" runat="server" CssClass="tableText"></asp:Label>
                                                                <asp:Label ID="lblEnterClaimDetailPracticeStatus" runat="server" CssClass="tableText"
                                                                    ForeColor="red"></asp:Label>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterClaimDetailCreationReasonText" runat="server" Text="<%$ Resources:Text, CreationReason %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblEnterClaimDetailCreationReason" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterClaimDetailPaymentMethodText" runat="server" Text="<%$ Resources:Text, PaymentSettlement %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblEnterClaimDetailPaymentMethod" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterClaimDetailServiceDateText" runat="server" Text="<%$ Resources:Text, ServiceDate %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox ID="txtEnterClaimDetailServiceDate" runat="server" Width="71px" MaxLength="10"
                                                                    OnTextChanged="txtEnterClaimDetailServiceDate_TextChanged" AutoPostBack="true"></asp:TextBox> <asp:ImageButton
                                                                        ID="ibtnEnterClaimDetailServiceDate" runat="server" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" ></asp:ImageButton>
                                                                <asp:Image ID="imgEnterClaimDetailServiceDateErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Middle" />
                                                                <cc1:CalendarExtender ID="EnterClaimDetailServiceDate" runat="server" TargetControlID="txtEnterClaimDetailServiceDate"
                                                                    PopupButtonID="ibtnEnterClaimDetailServiceDate" Format="dd-MM-yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="filteredEnterClaimDetailServiceDate" runat="server"
                                                                    TargetControlID="txtEnterClaimDetailServiceDate" FilterType="Custom, Numbers"
                                                                    ValidChars="-">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                                <asp:Label ID="lblEnterClaimDetailSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"
                                                                    Height="25px"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:DropDownList ID="ddlEnterClaimDetailsSchemeText" AutoPostBack="true" runat="server"
                                                                    Width="430px" OnSelectedIndexChanged="ddlEnterClaimDetailsSchemeText_SelectedIndexChanged" style="margin-top:1px;">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblEnterClaimDetailsSchemeStatus" runat="server" CssClass="tableText" ForeColor="red" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                       <asp:panel ID="panNonClinicSetting" runat="server" Visible="false">
                                                        <tr>
                                                            <td style="width: 200px; height: 25px;" valign="top">
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblNonClinicSetting" runat="server" CssClass="tableText"></asp:Label>
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
                                                                        <td width="150px">
                                                                            <asp:RadioButtonList ID="rblHKICSymbol" runat="server" AutoPostBack="False" Enabled="true"
                                                                                 CssClass="tableText" Width="150px" RepeatDirection="Horizontal" Style="position:relative;left:-7px;top:-2px;display:inline-block"/>
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
                                                    <table style="width: 100%" cellpadding="1" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <uc2:ucInputEHSClaim ID="udInputEHSClaim" runat="server"></uc2:ucInputEHSClaim>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 100px" align="left" valign="top">
                                                                <asp:ImageButton ID="ibtnEnterClaimDetailBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClientClick="return ReasonForVisitInitialComplete();" OnClick="ibtnEnterClaimDetailBack_Click">
                                                                </asp:ImageButton>
                                                            </td>
                                                            <td align="center" valign="top">
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
                    </asp:View>
                </asp:MultiView>
                <%-- Pop up for Advanced Search of SP --%>
                <asp:Button ID="btnHiddenSearchSP" runat="server" Style="display: none" />
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

                <%-- Pop up for Invalidation --%>
                <asp:Button ID="btnHiddenInvalidation" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="popupInvalidation" runat="server" TargetControlID="btnHiddenInvalidation"
                    PopupControlID="panInvalidation" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panInvalidationHeading">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="panInvalidation" runat="server" Style="display: none">
                    <asp:Panel ID="panInvalidationHeading" runat="server" Style="cursor: move">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblInvalidationHeader" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #FFFFFF">
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 40px; height: 42px" valign="middle">
                                            <asp:Image ID="imgPopupInvalidation" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                        <td align="center" style="height: 42px">
                                            <asp:Label ID="lblPopupInvalidationText" runat="server" Font-Bold="True" Text=""></asp:Label></td>
                                        <td style="width: 40px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:ImageButton ID="ibtnPopupInvalidationConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnPopupInvalidationConfirm_Click" />
                                            <asp:ImageButton ID="ibtnPopupInvalidationCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnPopupInvalidationCancel_Click" /></td>
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
                <%-- End of Pop up for Invalidation --%>

                <%-- Pop up for RVP Home List --%>
                <asp:Button Style="display: none" ID="btnModalPopupRVPHomeListSearch" runat="server">
                </asp:Button>
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
                <asp:Button Style="display: none" ID="btnModalPopupSchoolListSearch" runat="server" />
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

            </asp:Panel>
            
            <%-- Pop up for Warning Message --%>
            <asp:Button Style="display: none" ID="btnModalPopupWarningMessage" runat="server"></asp:Button>
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
            <%-- Pop up for Confirm Delete --%>
            <asp:Button ID="btnHiddenDelete" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmDelete" runat="server" TargetControlID="btnHiddenDelete"
                PopupControlID="panConfirmDelete" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panConfirmDeleteHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panConfirmDelete" runat="server" Style="display: none;">
                <asp:Panel ID="panConfirmDeleteHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblConfirmDeleteHeading" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgConfirmDelete" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblConfirmDelete" runat="server" Font-Bold="True"></asp:Label></td>
                                    <td style="width: 40px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnlblConfirmDeleteConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnlblConfirmDeleteConfirm_Click" />
                                        <asp:ImageButton ID="ibtnlblConfirmDeleteCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnlblConfirmDeleteCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 9px; height: 7px">
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
            <%-- Pop up for Vaccination Record --%>
            <asp:Button ID="btnHiddenVaccinationRecord" runat="server" Style="display: none" />
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
            <asp:Button runat="server" ID="btnHiddenDocTypeHelp" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
            </cc1:ModalPopupExtender>
            <%-- End of Popup for DocType --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
