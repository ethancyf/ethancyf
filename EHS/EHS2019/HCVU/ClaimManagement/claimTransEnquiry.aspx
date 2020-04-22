<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="claimTransEnquiry.aspx.vb" Inherits="HCVU.claimTransEnquiry" EnableEventValidation="false"
    Title="<%$ Resources:Title, ReimbursementClaimTransEnquiry %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/Reimbursement/ClaimTransDetail.ascx" TagName="ClaimTransDetail" TagPrefix="uc3" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc4" %>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <asp:Image ID="imgClaimTransEnquiry" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReimbursementClaimTransEnquiryBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReimbursementClaimTransEnquiryBanner %>" />
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
                <cc2:MessageBox ID="udcMessageBox" runat="server" Width="95%"></cc2:MessageBox>
            </asp:Panel>
            <asp:Panel ID="panClaimTransEnquiry" runat="server" Width="1000px">
                <asp:MultiView ID="MultiViewClaimTransEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">

                        <cc1:TabContainer ID="TabContainerCTM" runat="server" CssClass="m_ajax__tab_xp" Width="990px" ActiveTabIndex="0" EnableTheming="False" AutoPostBack="true" OnActiveTabChanged="TabContainerCTM_ActiveTabChanged">
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
                                                    <asp:Label ID="lblTabTransactionRCHRodeText" runat="server" Text="<%$ Resources: Text, RCHCode %>"/></td>
                                                <td style="vertical-align: top">
                                                    <asp:TextBox ID="txtTabTransactionRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="10" style="position:relative;top:-4px" /></td>
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
                                                    <asp:Label ID="lblTabServiceProviderRCHRodeText" runat="server" Text="<%$ Resources: Text, RCHCode %>"/></td>
                                                <td style="vertical-align: top">
                                                    <asp:TextBox ID="txtTabServiceProviderRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="10" style="position:relative;top:-4px" /></td>
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
                                                                <asp:TextBox ID="txtTabeHSAccountDocNo" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="18"/>
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
                                                                ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>" MaxLength="40"/>
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

                                            <tr style="height:20px">
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
                                                    <asp:Label ID="lblTabeHSAccountRCHRodeText" runat="server" Text="<%$ Resources: Text, RCHCode %>"/></td>
                                                <td style="vertical-align: top">
                                                    <asp:TextBox ID="txtTabeHSAccountRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="10" style="position:relative;top:-4px" /></td>
                                            </tr>
                                        
                                            <tr style="height: 20px">
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" colspan="4">
                                                    <asp:ImageButton ID="ibtnTabeHSAccountSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
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
                                                    <asp:Label ID="lblTabAdvancedSearchRCHRodeText" runat="server" Text="<%$ Resources: Text, RCHCode %>"/></td>
                                                <td style="vertical-align: top">
                                                    <asp:TextBox ID="txtTabAdvancedSearchRCHRode" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="10" style="position:relative;top:-4px" /></td>
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
                                                                <asp:TextBox ID="txtTabAdvancedSearchDocNo" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="18"/>
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
                        
                        <uc4:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panSearchCriteriaReview" />
                        
                        <asp:Panel ID="panSearchCriteriaReview" runat="server" width="1000px" >
                            <table style="width: 917px">
                                <tr>
                                    <td style="width: 230px;vertical-align: top">
                                        <asp:Label ID="lblAspectText" runat="server" Text="<%$ Resources:Text, SearchBy %>"></asp:Label></td>
                                    <td style="width: 250px;vertical-align: top">
                                        <asp:Label ID="lblAspect" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 230px;vertical-align: top" />
                                    <td style="vertical-align: top" />
                                </tr>
                                <tr>
                                    <td style="width: 212px;vertical-align: top">
                                        <asp:Label ID="lblRTransactionNoText" runat="server" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                                    <td style="width: 250px;vertical-align: top">
                                        <asp:Label ID="lblRTransactionNo" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 180px;vertical-align: top">
                                        <asp:Label ID="lblRHealthProfessionText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRHealthProfession" runat="server" CssClass="tableText"></asp:Label></td>
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
                                        <asp:Label ID="lblRRCHCodeText" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
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
                                        <asp:Label ID="lblREHealthDocTypeText" runat="server" Text="<%$ Resources:Text, eHealthAccountIdentityDocumentType %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblREHealthDocType" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRBankAccountNoText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                    <td style="vertical-align: top">
                                        <asp:Label ID="lblRBankAccountNo" runat="server" Font-Bold="True" CssClass="tableText"></asp:Label></td>
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
                        <asp:GridView ID="gvTransaction" runat="server" AutoGenerateColumns="False" Width="1255px"
                            OnRowCommand="gvTransaction_RowCommand" AllowPaging="True" OnPageIndexChanging="gvTransaction_PageIndexChanging"
                            OnRowDataBound="gvTransaction_RowDataBound" AllowSorting="True" OnPreRender="gvTransaction_PreRender"
                            OnSorting="gvTransaction_Sorting">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_selected" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>" ShowHeader="False"
                                    SortExpression="transNum">
                                    <ItemStyle Wrap="false" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_transNum" runat="server" CausesValidation="false" CommandName=""
                                            Text='<%# Eval("transNum") %>'></asp:LinkButton>
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
                                        <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("spChiName") %>' CssClass="TextGridChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="270px" />
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
                                <asp:TemplateField HeaderText="<%$ Resources: Text, RCHCode %>" SortExpression="RCH_Code">
                                    <ItemStyle Width="70px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGRCHCode" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfGRCHCode" runat="server" Value='<%# Eval("RCH_Code") %>' />
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
<%--                                    <asp:TableCell style="width: 90px" BorderWidth="2">
                                        <asp:Label ID="lblSummaryJoinedText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, Joined %>"></asp:Label></asp:TableCell>   --%>                             
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
<%--                                    <asp:TableCell BorderWidth="2">
                                        <asp:Label ID="lblSummaryJoined" runat="server" CssClass="tableText"></asp:Label></asp:TableCell>       --%>                                                                              
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
                        </asp:Panel>
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
                            <uc3:ClaimTransDetail ID="udcClaimTransDetail" runat="server" />
                            <asp:Panel ID="panel_RecordAction" runat="server" Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <center>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ibtnPreviousRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, PreviousRecordBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, PreviousRecordBtn %>" OnClick="ibtnPreviousRecord_Click" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Panel ID="panRecordNo" runat="server" BorderStyle="Inset" Width="90px">
                                                                <asp:Label ID="lblCurrentRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                                <asp:Label ID="lblSlash" runat="server" Font-Size="Large" Text="/"></asp:Label>
                                                                <asp:Label ID="lblMaxRecordNo" runat="server" Font-Size="Medium"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="ibtnNextRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextRecordBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, NextRecordBtn %>" OnClick="ibtnNextRecord_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnDetailBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnDetailBack_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnSuspendHistory" runat="server" ImageUrl="<%$ Resources:ImageUrl, SuspendHistoryBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, SuspendHistoryBtn %>" OnClick="ibtnSuspendHistory_Click" />
                                            <asp:ImageButton ID="ibtnManagement" runat="server" ImageUrl="<%$ Resources: ImageUrl, ManagementBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ManagementBtn %>" OnClick="ibtnManagement_Click" />
                                            <asp:ImageButton  ID="btnFocus" runat="server" ImageUrl="~/Images/others/arrowblank.png" Width="0" Height="0"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:HiddenField ID="HiddenFieldAction" runat="server" />
                            <asp:HiddenField ID="hfCurrentDetailTransactionNo" runat="server" />
                            <%-- Popup --%>
                            <asp:Button ID="btnHiddenShowDialog" runat="server" Style="display: none" />
                            <cc1:ModalPopupExtender ID="popupSuspendHistory" runat="server" TargetControlID="btnHiddenShowDialog"
                                PopupControlID="panSuspendHistory" BackgroundCssClass="modalBackgroundTransparent"
                                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panConfirmMsgHeading">
                            </cc1:ModalPopupExtender>
                            <br />
                            <asp:Panel ID="panSuspendHistory" runat="server" Style="display: none;">
                                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                                        <tr>
                                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                            </td>
                                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, SuspendHistory %>"></asp:Label></td>
                                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                                        </td>
                                        <td style="background-color: #ffffff">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="text-align: center; padding-top: 10px; padding-bottom: 10px">
                                                        <asp:GridView ID="gvSuspendHistory" Width="95%" runat="server" AutoGenerateColumns="False"
                                                            OnRowDataBound="gvSuspendHistory_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField ShowHeader="False">
                                                                    <HeaderStyle VerticalAlign="Top" />
                                                                    <ItemStyle Width="15px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Text, UpdatedTime %>">
                                                                    <HeaderStyle VerticalAlign="Top" />
                                                                    <ItemStyle Width="130px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSystemTime" runat="server" Text='<%# Eval("System_Dtm") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Text, UpdatedBy %>">
                                                                    <HeaderStyle VerticalAlign="Top" />
                                                                    <ItemStyle Width="100px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUpdateBy" runat="server" Text='<%# Eval("Update_By") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionStatus %>">
                                                                    <HeaderStyle VerticalAlign="Top" />
                                                                    <ItemStyle Width="240px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRecordStatus" runat="server" Text='<%# Eval("Record_Status") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Text, Remarks %>">
                                                                    <HeaderStyle VerticalAlign="Top" />
                                                                    <ItemStyle VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:ImageButton ID="ibtnClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, CloseBtn %>" OnClick="ibtnClose_Click" />
                                                    </td>
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
                            <%-- End of Popup --%>
                        </asp:Panel>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
