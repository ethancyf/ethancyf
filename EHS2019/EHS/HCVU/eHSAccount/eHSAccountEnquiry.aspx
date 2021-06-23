<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="eHSAccountEnquiry.aspx.vb" Inherits="HCVU.eHSAccountEnquiry" Title="<%$ Resources:Title, eHSAccountEnquiry %>" %>

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
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eHSAccountEnquiryBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, eHSAccountEnquiryBanner %>"></asp:Image>
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
                        BorderStyle="None" Width="100%">
                        <cc1:TabPanel ID="tabSearchRoute2" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblVCSearch" runat="server" Text="<%$ Resources:Text, SearchByParticulars %>"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchDocTypeR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:DropDownList ID="ddlSearchDocTypeR2" runat="server">
                                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchIdentityNumR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, IdentityDocNo %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchIdentityNumR2" runat="server" MaxLength="20" Width="200px"
                                                onblur="convertToUpper(this);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxIdentityNum" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtSearchIdentityNumR2"
                                                ValidChars='()/-'>
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchENameR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInEnglish %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchENameR2" runat="server" Width="200px" MaxLength="70" onChange="convertToUpper(this);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxEname" runat="server" Enabled="True"
                                                FilterType="Custom, UppercaseLetters, LowercaseLetters" TargetControlID="txtSearchENameR2"
                                                ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchCNameR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInChinese %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchCNameR2" runat="server" Width="200px" MaxLength="6" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchDOBR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                        <td>
                                            <asp:TextBox ID="txtSearchDOBR2" runat="server" MaxLength="10" Width="200px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filtereditSDOBR2" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers" TargetControlID="txtSearchDOBR2" ValidChars='-'>
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                                    <td>
                                                        <asp:Image ID="imgDOBError" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchAccountIDText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountID %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                        <td>
                                            <asp:Label ID="lblSearchAccountIDR2Prefix" runat="server" CssClass="tableTitle" Text=""
                                                Width="32px" Height="18"></asp:Label><asp:TextBox ID="txtSearchAccountIDR2" runat="server"
                                                    MaxLength="100" Width="168px" onblur="convertToUpper(this);" ToolTip="<%$ Resources:ToolTip, MultipleAccountID %>"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filtereditSRefNoR2" runat="server" Enabled="True"
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=","
                                                TargetControlID="txtSearchAccountIDR2">
                                            </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                            <asp:Image ID="imgSearchAccountIDR2Error" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                Visible="False" />
                                        </td>
                                    </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchRefNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RefNo %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchRefNo" runat="server" MaxLength="17" Width="200px" onblur="convertToUpper(this);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filtereditRefNoText" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtSearchRefNo"
                                                ValidChars='-'>
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchAcctTypeR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountType %>"></asp:Label></td>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td valign="top">
                                                        <asp:DropDownList ID="ddlSearchAcctTypeR2" runat="server" Width="210px" OnSelectedIndexChanged="ddlSearchAcctTypeR2_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList></td>
                                                    <td valign="top">
                                                        <asp:Panel ID="pnlAdvTempSearchR2" runat="server">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="width: 60px" valign="top" align="center">
                                                                        <asp:Label ID="lblTempSearchText" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                                                                    <td valign="top">
                                                                        <asp:DropDownList ID="ddlSearchTempAcct" runat="server" AppendDataBoundItems="true">
                                                                            <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchCreationDateR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CreationDate %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                        <td>
                                            <asp:Label ID="lblSearchCreationDateFromR2Text" runat="server" Text="From"></asp:Label>
                                            <asp:TextBox ID="txtSearchCreationDateFromR2" runat="server" MaxLength="10" Width="75px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            &nbsp;<asp:ImageButton ID="btnSearchCreationDateFromR2" runat="server" ImageAlign="AbsMiddle"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                            <asp:Label ID="lblSearchCreationDateToR2Text" runat="server" Text="To"></asp:Label>
                                            <asp:TextBox ID="txtSearchCreationDateToR2" runat="server" MaxLength="10" Width="75px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            &nbsp;<asp:ImageButton ID="btnSearchCreationDateToR2" runat="server" ImageAlign="AbsMiddle"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" /><cc1:CalendarExtender ID="calenderExtSearchCreationDateFromR2" CssClass="ajax_cal"
                                                    runat="server" PopupButtonID="btnSearchCreationDateFromR2" TargetControlID="txtSearchCreationDateFromR2"
                                                    Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                                                </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="calenderExtSearchCreationDateToR2" CssClass="ajax_cal" runat="server" PopupButtonID="btnSearchCreationDateToR2"
                                                TargetControlID="txtSearchCreationDateToR2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="filtereditSearchCreationDateFromR2" runat="server"
                                                FilterType="Custom, Numbers" TargetControlID="txtSearchCreationDateFromR2" ValidChars="-"
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:FilteredTextBoxExtender ID="filtereditSearchCreationDateToR2" runat="server"
                                                FilterType="Custom, Numbers" TargetControlID="txtSearchCreationDateToR2" ValidChars="-"
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                                    <td>
                                                        <asp:Image ID="imgDateError" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" /></td>
                                                </tr>
                                            </table>
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
                                            <asp:ImageButton ID="ibtnSearchR2" runat="server" AlternateText="<%$ Resources:AlternateText, SearchAccountBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SearchAccountBtn %>" OnClick="ibtnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabSearchRoute3" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblSearchManualValidAcc" runat="server" Text="<%$ Resources:Text, SearchManualValidationAccounts %>"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                                <table id="tblMaunalValidAcc">
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblServiceProviderIDR3" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label>
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                            <asp:TextBox ID="txtSPIDR3" runat="server" Width="196px" MaxLength="8"></asp:TextBox>
                                        </td>
                                        <cc1:FilteredTextBoxExtender ID="filteredExtSPID" runat="server" TargetControlID="txtSPIDR3"
                                            FilterType="Custom, Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                                    <td>
                                                        <asp:Image ID="imgSPIDErrorR3" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblManualValidStatusR3Text" runat="server" Text="<%$ Resources:Text, ManualValidationStatus %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlManualValidStatusR3" runat="server" Width="200px">
                                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="txtCreationDateR3" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CreationDate %>"></asp:Label>
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                            <asp:Label ID="lblCreationDateFromR3Text" runat="server" Text="From"></asp:Label>
                                            <asp:TextBox ID="txtCreationDateFromR3" runat="server" MaxLength="10" Width="75px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            &nbsp;<asp:ImageButton ID="btnCreationDateFromR3" runat="server" ImageAlign="AbsMiddle"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                            <asp:Label ID="lblCreationDateToR3Text" runat="server" Text="To"></asp:Label>
                                            <asp:TextBox ID="txtCreationDateToR3" runat="server" MaxLength="10" Width="75px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>
                                            &nbsp;<asp:ImageButton ID="btnCreationDateToR3" runat="server" ImageAlign="AbsMiddle"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                            <cc1:CalendarExtender ID="calenderExtCreationDateFromR3" CssClass="ajax_cal"
                                                runat="server" PopupButtonID="btnCreationDateFromR3" TargetControlID="txtCreationDateFromR3"
                                                Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                                            </cc1:CalendarExtender>
                                            <cc1:CalendarExtender ID="calenderExtCreationDateToR3" CssClass="ajax_cal" runat="server" PopupButtonID="btnCreationDateToR3"
                                                TargetControlID="txtCreationDateToR3" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="filteredExtCreationDateFromR3" runat="server"
                                                FilterType="Custom, Numbers" TargetControlID="txtCreationDateFromR3" ValidChars="-"
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                            <cc1:FilteredTextBoxExtender ID="filteredExtCreationDateToR3" runat="server"
                                                FilterType="Custom, Numbers" TargetControlID="txtCreationDateToR3" ValidChars="-"
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                                    <td>
                                                        <asp:Image ID="imgCreationDateErrorR3" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblWithClaimsR3Text" runat="server" Text="With Claims"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td valign="top">
                                                        <asp:DropDownList ID="ddlWithClaimsR3" runat="server" Width="200px" OnSelectedIndexChanged="ddlWithClaimsR3_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:Panel ID="pnlSchemeR3" runat="server">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="width: 60px" valign="top" align="center">
                                                                        <asp:Label ID="lblSchemeR3Text" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                                                    <td valign="top">
                                                                        <asp:DropDownList ID="ddlSchemeR3" runat="server" Width="200px" AppendDataBoundItems="true">
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblDeceasedR3Text" runat="server" Text="<%$ Resources:Text, Deceased %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDeceasedR3" runat="server" Width="200px" OnSelectedIndexChanged="ddlDeceasedR3_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblDateofDeathR3Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DateOfDeath %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlDateofDeathR3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                <asp:Label ID="lblDateofDeathFromR3Text" runat="server" Text="From"></asp:Label>
                                                                <asp:TextBox ID="txtDateofDeathFromR3" runat="server" MaxLength="10" Width="75px"
                                                                    onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                                    onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                                    onblur="filterDateInput(this);"></asp:TextBox>
                                                                &nbsp;<asp:ImageButton ID="btnDateofDeathFromR3" runat="server" ImageAlign="AbsMiddle"
                                                                    ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                                <asp:Label ID="lblDateofDeathToR3Text" runat="server" Text="To"></asp:Label>
                                                                <asp:TextBox ID="txtDateofDeathToR3" runat="server" MaxLength="10" Width="75px"
                                                                    onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                                    onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                                    onblur="filterDateInput(this);"></asp:TextBox>
                                                                &nbsp;<asp:ImageButton ID="btnDateofDeathToR3" runat="server" ImageAlign="AbsMiddle"
                                                                    ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                                <cc1:CalendarExtender ID="calendarExtDateofDeathFromR3" CssClass="ajax_cal"
                                                                    runat="server" PopupButtonID="btnDateofDeathFromR3" TargetControlID="txtDateofDeathFromR3"
                                                                    Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                                                                </cc1:CalendarExtender>
                                                                <cc1:CalendarExtender ID="calendarExtDateofDeathToR3" CssClass="ajax_cal" runat="server" PopupButtonID="btnDateofDeathToR3"
                                                                    TargetControlID="txtDateofDeathToR3" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="filteredExtDateofDeathFromR3" runat="server"
                                                                    FilterType="Custom, Numbers" TargetControlID="txtDateofDeathFromR3" ValidChars="-"
                                                                    Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <cc1:FilteredTextBoxExtender ID="filteredExtDateofDeathToR3" runat="server"
                                                                    FilterType="Custom, Numbers" TargetControlID="txtDateofDeathToR3" ValidChars="-"
                                                                    Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Image ID="imgDateofDeathErrorR3" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                Visible="False" />

                                                            </td>
                                                        </tr>
                                                    </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDeceasedR3" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblAccountTypeR3Text" runat="server" Text="<%$ Resources:Text, AccountType %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAcctTypeR3" runat="server" Width="210px" AutoPostBack="true">
                                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                            </asp:DropDownList>
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
                                            <asp:ImageButton ID="ibtnSearchR3" runat="server" AlternateText="<%$ Resources:AlternateText, SearchAccountBtn %>"
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
                                        <asp:Label ID="lblAcctListSearchByR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SearchBy %>"></asp:Label>
                                    </td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListSearchByR2" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SearchByParticulars %>"></asp:Label>
                                    </td>
                                    <td colspan="2" />
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListDocTypeR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                    </td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListDocTypeR2" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListIdentityNumR2Text" runat="server" CssClass="tableTitle"
                                            Text="<%$ Resources:Text, IdentityDocNo %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListIdentityNumR2" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListENameR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInEnglish %>"></asp:Label></td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListENameR2" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListDOBR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListDOBR2" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListECameR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInChinese %>"></asp:Label></td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListCNameR2" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListRefNoR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RefNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListRefNoR2" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListAccountIDR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountID %>"></asp:Label></td>
                                    <td style="width: 270px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr valign="top">
                                                <td>
                                                    <asp:Label ID="lblAcctListAccountIDR2" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                                <td style="vertical-align: middle; padding-left: 5px">
                                                    <asp:ImageButton ID="ibtnAcctListAccountIDR2Multiple" runat="server" ImageUrl="<%$ Resources:ImageUrl, ExpandBtn %>" />
                                                </td>
                                            </tr>
                                        </table>
                                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtenderAccountIDR2" runat="server"
                                            TargetControlID="panlblAcctListAccountIDR2" CollapsedSize="0" Collapsed="True"
                                            ExpandControlID="ibtnAcctListAccountIDR2Multiple" CollapseControlID="ibtnAcctListAccountIDR2Multiple"
                                            ImageControlID="ibtnAcctListAccountIDR2Multiple" AutoCollapse="False" AutoExpand="False"
                                            ScrollContents="False" ExpandedImage="<%$ Resources:ImageUrl, CollapseBtn %>"
                                            CollapsedImage="<%$ Resources:ImageUrl, ExpandBtn %>" ExpandDirection="Vertical"
                                            Enabled="False">
                                        </cc1:CollapsiblePanelExtender>
                                        <asp:Panel ID="panlblAcctListAccountIDR2" runat="server" CssClass="tableText">
                                            <asp:Label ID="txtAcctListAccountIDR2Multiple" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListCreateDateR2Text" runat="server" CssClass="tableTitle"
                                            Text="<%$ Resources:Text, CreationDate %>"></asp:Label></td>
                                    <td valign="top">
                                            <asp:Label ID="lblAcctListCreateDateR2" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListAcctTypeR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountType %>"></asp:Label>
                                    </td>
                                    <td style="width: 270px" valign="top">
                                        <asp:Label ID="lblAcctListAcctTypeR2" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 180px" valign="top">
                                        </td>
                                    <td valign="top">
                                        </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <table>
                            <tr style="height: 20px">
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="990px">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:CheckBox ID="chkMaskDocumentNoR2" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                                    AutoPostBack="True" OnCheckedChanged="chkMaskDocumentNo_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvAcctListR2" runat="server" AllowPaging="True" AllowSorting="True"
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
                    <asp:Panel ID="pnlSearchCriteriaRoute3" runat="server">
                        <uc6:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview3" runat="server" TargetControlID="pnlSearchCriteriaReviewRoute3" />
                        <asp:Panel ID="pnlSearchCriteriaReviewRoute3" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListSearchByR3Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SearchBy %>"></asp:Label>
                                    </td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListSearchByR3" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SearchManualValidationAccounts %>"></asp:Label>
                                    </td>
                                    <td colspan="2" />
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListSPIDR3Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label>
                                    </td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListSPIDR3" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListManualValidStatusR3Text" runat="server" CssClass="tableTitle"
                                            Text="<%$ Resources:Text, ManualValidationStatus %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListManualValidStatusR3" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListCreationDateR3Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CreationDate %>"></asp:Label>
                                    </td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListCreationDateR3" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListWithClaimsR3Text" runat="server" CssClass="tableTitle"
                                            Text="With Claims"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListWithClaimsR3" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListDeceasedR3Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Deceased %>"></asp:Label>
                                    </td>
                                    <td style="width: 250px" valign="top">
                                        <asp:Label ID="lblAcctListDeceasedR3" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListDateofDeathR3Text" runat="server" CssClass="tableTitle"
                                            Text="<%$ Resources:Text, DateOfDeath %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblAcctListDateofDeathR3" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px" valign="top">
                                        <asp:Label ID="lblAcctListAcctTypeR3Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountType %>"></asp:Label>
                                    </td>
                                    <td style="width: 270px" valign="top">
                                        <asp:Label ID="lblAcctListAcctTypeR3" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td colspan="2" />
                                </tr>
                            </table>
                        </asp:Panel>
                        <table>
                            <tr style="height: 20px">
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="990px">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:CheckBox ID="chkMaskDocumentNoR3" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                                    AutoPostBack="True" OnCheckedChanged="chkMaskDocumentNo_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvAcctListR3" runat="server" AllowPaging="True" AllowSorting="True"
                                        Width="1270px" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle VerticalAlign="Top" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="HeaderLevelCheckBox" runat="server" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="20px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, ReferenceNo %>" HeaderStyle-VerticalAlign="Top"
                                                HeaderStyle-HorizontalAlign="Center" SortExpression="Voucher_Acc_ID">
                                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:LinkButton ID="lbtnAccountID" runat="server"></asp:LinkButton>
                                                            </td>
                                                            <td style="padding-left: 5px">
                                                                <asp:Image ID="imgWarning" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                                    Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                                <ItemStyle Width="150px" VerticalAlign="Top" />
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
                                            <asp:TemplateField SortExpression="ManualValidationStatus" HeaderText="<%$ Resources:Text, ManualValidationStatus %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblManualValidationStatus" runat="server" Text='<%# Eval("ManualValidationStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" VerticalAlign="Top" />
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

                    <asp:Panel ID="pnlTransactionInfo" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblTransactionInfoText" runat="server" Text="<%$ Resources:Text, TransactionInformation %>" Font-Bold="True"/>
                        </div>
                        <table>
                            <tr>
                                <td style="width: 220px" valign="top">
                                    <asp:Label ID="lblTransactionIDText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:Label ID="lblTransactionID" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
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
                                <td style="text-align: center">
                                    <asp:ImageButton ID="ibtnAmendHistory" runat="server" AlternateText="<%$ Resources:AlternateText, AmendHistoryBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, AmendHistoryBtn %>" OnClick="ibtnAmendHistory_Click" />
                                    <asp:ImageButton ID="ibtnSchemeInfo" runat="server" AlternateText="<%$ Resources:AlternateText, SchemeInformationBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SchemeInformationBtn %>" OnClick="ibtnSchemeInfo_Click" />
                                    <asp:ImageButton ID="ibtnVaccinationRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, VaccinationRecordBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, VaccinationRecordBtn %>" OnClick="ibtnVaccinationRecord_Click" />
                                    <asp:ImageButton ID="ibtnManagement" runat="server" ImageUrl="<%$ Resources: ImageUrl, ManagementBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ManagementBtn %>" OnClick="ibtnManagement_Click"></asp:ImageButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vAmendmentHistory" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblPersonalParticularsText_v3" runat="server" Font-Bold="true" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
                    </div>
                    <br />
                    <uc1:ucReadOnlyDocumnetType ID="ucReadOnlyDocTypeAmendHistory" runat="server" />
                    <br />
                    <div class="headingText">
                        <asp:Label ID="lblAmendHistoryText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, AmendHistory %>"></asp:Label>
                    </div>
                    <asp:GridView ID="gvAmendHistory" runat="server" AllowPaging="True" AllowSorting="True"
                        Width="100%" BackColor="White" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmendmentRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="20px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Amend_Dtm" HeaderText="<%$ Resources:Text, AmendDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmendDtm" runat="server" Text='<%# Eval("Amend_Dtm") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, Name %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>'></asp:Label>
                                    <br></br>
                                    <asp:Label ID="lblCName" runat="server" Text='<%# Eval("Chi_Name") %>' Font-Names="HA_MingLiu"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="230px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="DOB" HeaderText="<%$ Resources:Text, DOB %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="120px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Sex" HeaderText="<%$ Resources:Text, Gender %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblSex" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Date_Of_Issue" HeaderText="<%$ Resources:Text, DateOfIssue %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfIssue" runat="server" Text='<%# Eval("Date_of_Issue") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EC_Serial_No" HeaderText="<%$ Resources:Text, ECSerialNo %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblECSN" runat="server" Text='<%# Eval("EC_Serial_No") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EC_Reference_No" HeaderText="<%$ Resources:Text, ECReference %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblECRef" runat="server" Text='<%# Eval("EC_Reference_No") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Foreign_Passport_No" HeaderText="<%$ Resources:Text, ForeignPassport %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblForeignPassportNo" runat="server" Text='<%# Eval("Foreign_Passport_No") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="PASS_Issue_Region" HeaderText="<%$ Resources:Text, PassportIssueRegion %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPASS_Issue_Region" runat="server" Text='<%# Eval("PASS_Issue_Region")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>


                            <asp:TemplateField SortExpression="Permit_To_Remain_Until" HeaderText="<%$ Resources:Text, PermitToRemain %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPermitToRemainUntil" runat="server" Text='<%# Eval("Permit_To_Remain_Until") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Create_By_SmartID" HeaderText="<%$ Resources:Text, CreationMethod %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreationMethod" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Update_By" HeaderText="<%$ Resources:Text, Amend_By %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdate_By" runat="server" Text='<%# Eval("Update_By") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="120px" VerticalAlign="Top" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, RecordStatus %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmendmentStatus" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnAmendmentHistoryBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnAmendmentHistoryBack_Click"></asp:ImageButton></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vSchemeInfo" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblPersonalParticularsText_v4" runat="server" Font-Bold="true" Text="<%$ Resources:Text, VRInformation %>"></asp:Label>
                    </div>
                    <uc1:ucReadOnlyDocumnetType ID="ucReadOnlyDocTypeSchemeInfo" runat="server" />
                    <br />
                    <div class="headingText">
                        <asp:Label ID="lblSchemeInfoText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, PersonalSchemeInfo %>"></asp:Label>
                    </div>
                    <asp:Panel ID="pnlScheme" runat="server">
                    </asp:Panel>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnSchemeInfoBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSchemeInfoBack_Click"></asp:ImageButton></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vVaccinationRecord" runat="server">
                    <uc4:ucVaccinationRecord ID="udcVaccinationRecord" runat="server"></uc4:ucVaccinationRecord>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnVaccinationRecordBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnVaccinationRecordBack_Click" />
                            </td>
                        </tr>
                    </table>
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
