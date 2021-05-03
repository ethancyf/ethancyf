<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="eHSAccountMaint.aspx.vb" Inherits="HCVU.eHSAccountMaint" Title="<%$ Resources:Title, eHSAccountMaint %>" %>

<%@ Register Src="../UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<%@ Register Src="../UIControl/DocType/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ChooseCCCode.ascx" TagName="ChooseCCCode" TagPrefix="uc3" %>
<%@ Register Src="~/UIControl/Token/ucInputToken.ascx" TagName="ucInputToken" TagPrefix="uc4" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc5" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <%-- CRE11-007 Start: Handle multiple select--%>

    <script type="text/javascript">
        function SelectAll(gvid, id) {
            var grid = document.getElementById(gvid);
            var cell;

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[0];

                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>

    <%-- CRE11-007 End --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eHSAccountMaintBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, eHSAccountMaintBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel Style="display: none" ID="panChooseCCCode" runat="server">
                <asp:Panel ID="panChooseCCCodeHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, ChooseCCCodeHeading %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff" align="center">
                            <uc3:ChooseCCCode ID="udcCCCode" runat="server"></uc3:ChooseCCCode>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 400px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 400px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 60px; height: 42px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblMsgContent" runat="server" Font-Bold="True" /></td>
                                    <td align="left" style="width: 40px; height: 42px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3" style="height: 42px">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnDialogConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDialogCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
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
                        AutoPostBack="true" BorderStyle="None" Width="100%">
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
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlSearchDocTypeR2" runat="server">
                                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                                        </asp:DropDownList></td>
                                                    <td>
                                            <asp:Image ID="imgDocTypeError" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchIdentityNumR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, IdentityDocNo %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                        <td>
                                            <asp:TextBox ID="txtSearchIdentityNumR2" runat="server" MaxLength="18" Width="200px"
                                                onblur="convertToUpper(this)"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxIdentityNum" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtSearchIdentityNumR2"
                                                ValidChars='()/-'>
                                            </cc1:FilteredTextBoxExtender>

                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imgIdentityNumErr" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px" valign="top">
                                            <asp:Label ID="lblSearchENameR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInEnglish %>"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtSearchENameR2" runat="server" Width="200px" MaxLength="70" onblur="convertToUpper(this);"></asp:TextBox>
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
                                            <asp:ImageButton ID="ibtnNewAccountR2" runat="server" AlternateText="<%$ Resources:AlternateText, NewAccountBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, NewAccountBtn %>" OnClick="ibtnNewAccount_Click"
                                                Visible="false" />
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
                                            <asp:DropDownList ID="ddlAcctTypeR3" runat="server" Width="210px"  AutoPostBack="true">
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
                        <uc5:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview2" runat="server" TargetControlID="pnlSearchCriteriaReviewRoute2" />

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
                                        <asp:Label ID="lblAcctListCNameR2Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountNameInChinese %>"></asp:Label></td>
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
                                                <HeaderStyle VerticalAlign="Top" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="HeaderLevelCheckBox" runat="server" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="20px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID_ReferenceNo %>" HeaderStyle-VerticalAlign="Top"
                                                HeaderStyle-HorizontalAlign="Center" SortExpression="Voucher_Acc_ID">
                                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:LinkButton ID="lbtnAccountID" runat="server" Style="white-space: nowrap"></asp:LinkButton>
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
                                                <ItemStyle Width="170px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, VRName %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>'></asp:Label></br>
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
                                                    <asp:TextBox ID="hfAccountSource" runat="server" Style="display: none" Text='<%# Eval("Source") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Account_Status" HeaderText="<%$ Resources:Text, AccountStatus %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountStatus" runat="server" Text='<%# Eval("Account_Status") %>'></asp:Label>
                                                    <asp:TextBox ID="hfAccountStatus" runat="server" Style="display: none" Text='<%# Eval("Account_Status") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="70px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Public_Enquiry_Status" HeaderText="<%$ Resources:Text, EnquiryStatus %>"
                                                HeaderStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryStatus" runat="server" Text='<%# Eval("Public_Enquiry_Status") %>'></asp:Label>
                                                    <asp:TextBox ID="hfEnquiryStatus" runat="server" Style="display: none" Text='<%# Eval("Public_Enquiry_Status") %>'></asp:TextBox>
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
                        <uc5:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview3" runat="server" TargetControlID="pnlSearchCriteriaReviewRoute3" />
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
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>'></asp:Label></br>
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
                            <td style="width: 50px"></td>
                            <td style="width: 800px; text-align: center">
                                <asp:ImageButton ID="ibtnRReactivateSelectedAccount" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReactivateSelectedAccountsBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReactivateSelectedAccountsBtn %>"
                                    OnClick="ibtnRReactivateSelectedAccount_Click" />
                                <asp:ImageButton ID="ibtnRSuspendSelectedAccount" runat="server" ImageUrl="<%$ Resources: ImageUrl, SuspendSelectedAccountsBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SuspendSelectedAccountsBtn %>" OnClick="ibtnRSuspendSelectedAccount_Click" />
                                <asp:ImageButton ID="ibtnRTerminateSelectedAccount" runat="server" ImageUrl="<%$ Resources: ImageUrl, TerminateSelectedAccountsBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, TerminateSelectedAccountsBtn %>"
                                    OnClick="ibtnRTerminateSelectedAccount_Click" />
                                <asp:ImageButton ID="ibtnRSuspendEnquirySelectedAccount" runat="server"
                                    AlternateText="<%$ Resources: AlternateText, SuspendEnquiryforSelectedAccounts %>"
                                    OnClick="ibtnRSuspendEnquirySelectedAccount_Click" Visible="false" />
                                <asp:ImageButton ID="ibtnRReactivateEnquirySelectedAccount" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReactivateEnquiryforSelectedAccounts %>"
                                    AlternateText="<%$ Resources: AlternateText, ReactivateEnquiryforSelectedAccounts %>"
                                    OnClick="ibtnRReactivateEnquirySelectedAccount_Click" Visible="false" />
                            </td>
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
                            <asp:Label ID="lblTransactionInfoText" runat="server" Text="<%$ Resources:Text, TransactionInformation %>" Font-Bold="True" />
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
                    <asp:Panel ID="pnlAmendActionBtn" runat="server">
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnAmendSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnAmendSave_Click" />
                                    <asp:ImageButton ID="ibtnAmendCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnAmendCancel_Click" />
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
                                </td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="ibtnAmendRecord" runat="server" AlternateText="<%$ Resources:AlternateText, AmendRecordBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, AmendRecordBtn %>" OnClick="ibtnAmendRecord_Click" />
                                    <asp:ImageButton ID="ibtnWithdrawAmendment" runat="server" AlternateText="<%$ Resources:AlternateText, WithdrawAmendmentBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, WithdrawAmendmentBtn %>" OnClick="ibtnWithdrawAmendment_Click" />
                                    <asp:ImageButton ID="ibtnSuspend" runat="server" AlternateText="<%$ Resources:AlternateText, SuspendAcctBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SuspendAcctBtn %>" OnClick="ibtnSuspend_Click" />
                                    <asp:ImageButton ID="itbnReactivate" runat="server" AlternateText="<%$ Resources:AlternateText, ReactivateAcctBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReactivateAcctBtn %>" OnClick="itbnReactivate_Click" />
                                    <asp:ImageButton ID="ibtnTerminate" runat="server" AlternateText="<%$ Resources:AlternateText, TerminateAcctBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, TerminateAcctBtn %>" OnClick="ibtnTerminate_Click" />
                                    <asp:ImageButton ID="ibtnSuspendEnquiry" runat="server" AlternateText="<%$ Resources:AlternateText, SuspendEnquiryBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SuspendEnquiryBtn %>" OnClick="ibtnSuspendEnquiry_Click" />
                                    <asp:ImageButton ID="ibtnReactiveEnquiry" runat="server" Visible="False" AlternateText="<%$ Resources:AlternateText, ReactivateEnquiryBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReactivateEnquiryBtn %>" OnClick="ibtnReactiveEnquiry_Click" />
                                    <asp:ImageButton ID="ibtnMarkAsImmDValid" runat="server" AlternateText="<%$ Resources:AlternateText, MarkAsImmDValidatingBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, MarkAsImmDValidatingBtn %>" OnClick="ibtnMarkAsImmDValid_Click" />
                                    <asp:ImageButton ID="ibtnConfirmAsValidAcct" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmAsValidAcctBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ConfirmAsValidAcctBtn %>" OnClick="ibtnConfirmAsValidAcct_Click" />
                                    <asp:ImageButton ID="ibtnReleaseForRect" runat="server" AlternateText="<%$ Resources:AlternateText, ReleaseRectificationBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReleaseRectificationBtn %>" OnClick="ibtnReleaseForRect_Click" />
                                    <asp:ImageButton ID="ibtnCancelValidation" runat="server" AlternateText="<%$ Resources:AlternateText, CancelValidationBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelValidationBtn %>" OnClick="ibtnCancelValidation_Click" />
                                    <asp:ImageButton ID="ibtnRemove" runat="server" AlternateText="<%$ Resources:AlternateText, RemoveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>" OnClick="ibtnRemove_Click" />
                                    <asp:ImageButton ID="ibtnAmendHistory" runat="server" AlternateText="<%$ Resources:AlternateText, AmendHistoryBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, AmendHistoryBtn %>" OnClick="ibtnAmendHistory_Click" />
                                    <asp:ImageButton ID="ibtnRemoveTempAccountByBO" runat="server" AlternateText="<%$ Resources:AlternateText, RemoveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>" OnClick="ibtnRemoveTempAccountByBO_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlSuspendEnquiryInput" runat="server">
                        <table>
                            <tr>
                                <td style="width: 220px" valign="top">
                                    <asp:Label ID="lblSuspendPublicEnquiryInputText" runat="server" CssClass="tableTitle"
                                        Height="25px" Text="<%$ Resources:Text, PublicEnquirySuspendReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSuspendPublicEnquiryInput" runat="server" Width="540px" Height="20px"
                                        MaxLength="255"></asp:TextBox><asp:Image ID="imgSuspendPublicEnquiryInputErr" runat="server"
                                            ImageUrl="~/Images/others/icon_caution.gif" Visible="False" ImageAlign="AbsMiddle" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnSuspendPublicEnquirySave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSuspendPublicEnquirySave_Click" />
                                    <asp:ImageButton ID="ibtnSuspendPublicEnquiryCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnSuspendPublicEnquiryCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlReactiveEnquiryInput" runat="server">
                        <table>
                            <tr>
                                <td style="width: 220px" valign="top">
                                    <asp:Label ID="lblReactivePublicEnquiryInputText" runat="server" CssClass="tableTitle"
                                        Height="25px" Text="<%$ Resources:Text, PublicEnquiryReactivateReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReactivePublicEnquiryInput" runat="server" Width="540px" Height="20px"
                                        MaxLength="255"></asp:TextBox><asp:Image ID="imgReactivePublicEnquiryInputErr" runat="server"
                                            ImageUrl="~/Images/others/icon_caution.gif" Visible="False" ImageAlign="AbsMiddle" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnReactivePublicEnquirySave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnReactivePublicEnquirySave_Click" />
                                    <asp:ImageButton ID="ibtnReactivePublicEnquiryCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnReactivePublicEnquiryCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlSuspendAccountInput" runat="server">
                        <table>
                            <tr>
                                <td style="width: 220px" valign="top">
                                    <asp:Label ID="lblSuspendAccountInputText" runat="server" CssClass="tableTitle" Height="25px"
                                        Text="<%$ Resources:Text, AccountSuspendReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSuspendAccountInput" runat="server" Width="540px" Height="20px"
                                        MaxLength="255"></asp:TextBox><asp:Image ID="imgSuspendAccountInputErr" runat="server"
                                            ImageUrl="~/Images/others/icon_caution.gif" Visible="False" ImageAlign="AbsMiddle" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnSuspendAccountSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSuspendAccountSave_Click" />
                                    <asp:ImageButton ID="ibtnSuspendAccountCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnSuspendAccountCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlReactiveAccountInput" runat="server">
                        <table>
                            <tr>
                                <td style="width: 220px" valign="top">
                                    <asp:Label ID="lblReactiveAccountInputText" runat="server" CssClass="tableTitle"
                                        Height="25px" Text="<%$ Resources:Text, AccountReactivateReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReactiveAccountInput" runat="server" Width="540px" Height="20px"
                                        MaxLength="255"></asp:TextBox><asp:Image ID="imgReactiveAccountInputErr" runat="server"
                                            ImageUrl="~/Images/others/icon_caution.gif" Visible="False" ImageAlign="AbsMiddle" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnReactiveAccountSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnReactiveAccountSave_Click" />
                                    <asp:ImageButton ID="ibtnReactiveAccountCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnReactiveAccountCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlTerminateAccountInput" runat="server">
                        <table>
                            <tr>
                                <td style="width: 220px" valign="top">
                                    <asp:Label ID="lblTerminateAccountInputText" runat="server" CssClass="tableTitle"
                                        Height="25px" Text="<%$ Resources:Text, AccountTerminateReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTerminateAccountInput" runat="server" Width="540px" Height="20px"
                                        MaxLength="255"></asp:TextBox><asp:Image ID="imgTerminateAccountInputErr" runat="server"
                                            ImageUrl="~/Images/others/icon_caution.gif" Visible="False" ImageAlign="AbsMiddle" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnTerminateAccountSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnTerminateAccountSave_Click" />
                                    <asp:ImageButton ID="ibtnTerminateAccountCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnTerminateAccountCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vAmendmentHistory" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblAmendHistoryText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, AmendHistory %>"></asp:Label>
                    </div>
                    <br />
                    <uc1:ucReadOnlyDocumnetType ID="ucReadOnlyDocTypeAmendHistory" runat="server" />
                    <br />
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
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>'></asp:Label></br>
                                    <asp:Label ID="lblCName" runat="server" Text='<%# Eval("Chi_Name") %>' Font-Names="HA_MingLiu"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="270px" VerticalAlign="Top" />
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
                <asp:View ID="vConfirmAmendedAccount" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblConfirmAcctInfoText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmInfo %>"></asp:Label>
                    </div>
                    <uc1:ucReadOnlyDocumnetType ID="udcConfirmAccount" runat="server"></uc1:ucReadOnlyDocumnetType>
                    <asp:Panel ID="panConfrimBatchAccount" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="990px">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:CheckBox ID="chkMaskDocumentNoAC" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                                    AutoPostBack="True" OnCheckedChanged="chkMaskDocumentNoAC_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvAC" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                        AllowSorting="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID %>">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGAccountID" runat="server" Text='<%# Eval("Account_ID_F") %>' />
                                                    <asp:TextBox ID="hfAccountID" runat="server" Style="display: none" Value='<%# Eval("Account_ID") %>' />
                                                    <asp:TextBox ID="hfAccountSource" runat="server" Style="display: none" Text='<%# Eval("Account_Source") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, DocumentType %>" SortExpression="Document_Code">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGDocumentType" runat="server" Text='<%# Eval("Document_Code") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, IdentityDocNo %>" SortExpression="Document_No">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGDocumentNoM" runat="server" Text='<%# Eval("Document_No_FM") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, IdentityDocNo %>" SortExpression="Document_No" Visible="false">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGDocumentNoUnmask" runat="server" Text='<%# Eval("Document_No") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, VRName %>">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>' /></br>
                                                <asp:Label ID="lblCName" runat="server" Text='<%# Eval("Chi_Name") %>' Font-Names="HA_MingLiu" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Text, DOB %>" SortExpression="DOB">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Text, Gender %>" SortExpression="Sex">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSex" runat="server" Text='<%# Eval("Sex") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Text, AccountStatus %>" SortExpression="Account_Status">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" Width="100px" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountStatus" runat="server" Text='<%# Eval("Account_Status") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources: Text, WithSuspiciousClaims %>" SortExpression="With_Suspicious_Claim">
                                                <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                                <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGWithSuspiciousClaim" runat="server" Text='<%# Eval("With_Suspicious_Claim") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlShowAccCreateInfoConfirm_BySP" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td style="width: 220px; height: 25px;" valign="top">
                                    <asp:Label ID="lblConfirmCreationSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"
                                        Height="25px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblConfirmCreationSPName" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 220px; height: 25px;" valign="top">
                                    <asp:Label ID="lblConfirmCreationPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                        Height="25px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblConfirmCreationPractice" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlShowAccCreateInfoConfirm_ByBO" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td style="width: 220px" valign="top">
                                    <asp:Label ID="lblConfirmCreationCreatedByText" runat="server" CssClass="tableTitle"
                                        Text="<%$ Resources:Text, CreateBy %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmCreationCreatedBy" runat="server" CssClass="tableText"></asp:Label></td>
                                <td valign="top" style="width: 457px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlShowSchemeAccCreateConfirm" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td style="width: 220px; height: 25px;" valign="top">
                                    <asp:Label ID="lblConfirmCreationSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"
                                        Height="25px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblConfirmCreationScheme" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td style="width: 220px" valign="top">
                                <asp:Label ID="lblConfirmReasonText" runat="server" CssClass="tableTitle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblConfirmReason" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="ibtnConfirmAmendedAccount" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnConfirmAmendedAccount_Click" />
                                <asp:ImageButton ID="ibtnConfirmCancelAmendedAccont" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnConfirmCancelAmendedAccont_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vComplete" runat="server">
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vNewAccount" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblheadingEnterDetails" runat="server" Text="<%$ Resources:Text, EnterDetails %>"
                            Font-Bold="True"></asp:Label>
                    </div>
                    <br />
                    <asp:Panel ID="pnlDocumentTypeRadioButtonGroup" runat="server" Visible="false">
                        <cc2:DocumentTypeRadioButtonGroup ID="udcStep1DocumentTypeRadioButtonGroup" runat="server"
                            HeaderCss="tableText" HeaderText="<%$ Resources:Text, DocumentType%>" AutoPostBack="true"
                            LegendImageURL="<%$ Resources:ImageUrl, Infobtn%>" LegendImageALT="<%$ Resources:Text, AcceptedDocList%>" />
                        <br />
                    </asp:Panel>
                    <table width="100%">
                        <tr>
                            <td style="width: 220px; height: 25px;" valign="top">
                                <asp:Label ID="lblCreationDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"
                                    Height="25px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCreationDocumentType" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <uc2:ucInputDocumentType ID="ucInputDocumentType_NewAcc" runat="server" />
                    <table width="100%">
                        <asp:Panel ID="pnlCreationSPIDScheme" runat="server" Visible="false">
                            <tr>
                                <td style="width: 220px; height: 25px;" valign="top">
                                    <asp:Label ID="lblEnterCreationDetailSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>"
                                        Height="25px"></asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtEnterCreationDetailSPID" runat="server" MaxLength="8" Width="60px"></asp:TextBox>
                                    <asp:Image ID="imgEnterCreationDetailSPIDError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                    <asp:ImageButton ID="ibtnSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchSBtn %>" OnClick="ibtnSearchSP_Click" />
                                    <asp:ImageButton ID="ibtnClearSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, ClearBtn %>"
                                        Enabled="false" ImageUrl="<%$ Resources:ImageUrl, ClearDisableSBtn %>" OnClick="ibtnClearSearchSP_Click" />
                                    <asp:CheckBox ID="cboCreateByDH" runat="server" Checked="false" CssClass="tableText"
                                        AutoPostBack="True" OnCheckedChanged="cboCreateByDH_CheckedChanged" Text="DH" />
                                    <asp:Image ID="imgSPIDErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                        Visible="false" ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 220px; height: 25px;" valign="top">
                                    <asp:Label ID="lblEnterCreationDetailSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"
                                        Height="25px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEnterCreationDetailSPName" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 220px; height: 25px;" valign="top">
                                    <asp:Label ID="lblEnterCreationDetailPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                        Height="25px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEnterCreationDetailPractice" runat="server" Width="405px">
                                    </asp:DropDownList>
                                    <asp:Image ID="imgPracticeErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                        Visible="false" ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 220px; height: 25px;" valign="top">
                                    <asp:Label ID="lblEnterCreationSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"
                                        Height="25px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEnterCreationDetailScheme" runat="server" Width="405px">
                                    </asp:DropDownList>
                                    <asp:Image ID="imgSchemeErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                        Visible="false" ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td style="width: 10px;"></td>
                            <td align="center" valign="top">
                                <asp:ImageButton ID="ibtnNewAccountSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnNewAccountSave_Click" />
                                <asp:ImageButton ID="ibtnCreationCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnCreationCancel_Click"></asp:ImageButton>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vAccountActionSave" runat="server">
                    <%--[AS]--%>
                    <table>
                        <tr>
                            <td>
                                <div class="headingText">
                                    <asp:Label ID="lblASActionTitle" runat="server" Text="[Suspend Account/Terminate Account]"></asp:Label>
                                    <asp:TextBox ID="txtASActionType" runat="server" Style="display: none" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <table cellpadding="0" cellspacing="0" width="990px">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:CheckBox ID="chkMaskDocumentNoAS" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                            AutoPostBack="True" OnCheckedChanged="chkMaskDocumentNoAS_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvAS" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                    AllowSorting="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID %>">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGAccountID" runat="server" Text='<%# Eval("Account_ID_F") %>' />
                                                <asp:TextBox ID="hfAccountID" runat="server" Style="display: none" Value='<%# Eval("Account_ID") %>' />
                                                <asp:TextBox ID="hfAccountSource" runat="server" Style="display: none" Text='<%# Eval("Account_Source") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, DocumentType %>" SortExpression="Document_Code">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDocumentType" runat="server" Text='<%# Eval("Document_Code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, IdentityDocNo %>" SortExpression="Document_No">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDocumentNoM" runat="server" Text='<%# Eval("Document_No_FM") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, IdentityDocNo %>" SortExpression="Document_No" Visible="false">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGDocumentNoUnmask" runat="server" Text='<%# Eval("Document_No") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, VRName %>">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>' /></br>
                                                <asp:Label ID="lblCName" runat="server" Text='<%# Eval("Chi_Name") %>' Font-Names="HA_MingLiu" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, DOB %>" SortExpression="DOB">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, Gender %>" SortExpression="Sex">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSex" runat="server" Text='<%# Eval("Sex") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, AccountStatus %>" SortExpression="Account_Status">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" Width="100px" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccountStatus" runat="server" Text='<%# Eval("Account_Status") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources: Text, WithSuspiciousClaims %>" SortExpression="With_Suspicious_Claim">
                                            <HeaderStyle CssClass="GridViewHeaderPadding" VerticalAlign="top" />
                                            <ItemStyle CssClass="GridViewItemPadding" VerticalAlign="top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGWithSuspiciousClaim" runat="server" Text='<%# Eval("With_Suspicious_Claim") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr style="height: 13px">
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="width: 220px">
                                            <asp:Label ID="lblASActionReasonText" runat="server" Text="[Account Suspend Reason/Account Terminate Reason/Account Reativate Reason/Account Suspend Enquiry Reason]"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtASActionReason" runat="server" CssClass="tableText" Width="540px"
                                                Height="20px" MaxLength="255"></asp:TextBox>
                                            <asp:Image ID="imgASActionReasonErr" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                Visible="False" ImageAlign="AbsMiddle" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px">
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="width: 400px">
                                            <asp:ImageButton ID="ibtnASBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnASBack_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnASSave" runat="server" ImageUrl="<%$ Resources: ImageUrl, SaveBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, SaveBtn %>" OnClick="ibtnASSave_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
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
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 750px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, SearchServiceProvider %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 750px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <asp:Panel ID="pnlAdvancedSearchCritieria" runat="server">
                                <cc2:InfoMessageBox ID="udcInfoMsgAdvancedSearch" runat="server" Width="95%" Visible="false"></cc2:InfoMessageBox>
                                <cc2:MessageBox ID="udcSystemMsgAdvancedSearch" runat="server" Width="95%" Visible="false"></cc2:MessageBox>
                                <table>
                                    <tr>
                                        <td style="width: 180px">
                                            <asp:Label ID="lblAdvancedSearchSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                        <td style="width: 180px">
                                            <asp:TextBox ID="txtAdvancedSearchSPID" runat="server" MaxLength="8"></asp:TextBox></td>
                                        <td style="width: 180px">
                                            <asp:Label ID="lblAdvancedSearchHKICText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtAdvancedSearchHKIC" runat="server" MaxLength="11" onBlur="formatHKID(this)"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px">
                                            <asp:Label ID="lblAdvancedSearchNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label>
                                        </td>
                                        <td style="width: 180px">
                                            <asp:TextBox ID="txtAdvancedSearchName" runat="server" MaxLength="70" onBlur="Upper(event,this)"
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
                            </asp:Panel>
                            <asp:Panel ID="pnlAdvancedSearchResult" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 180px; height: 25px">
                                            <asp:Label ID="lblAdvancedSearchResultSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"
                                                Height="25px"></asp:Label></td>
                                        <td style="width: 180px; height: 25px">
                                            <asp:Label ID="lblAdvancedSearchResultSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 180px; height: 25px">
                                            <asp:Label ID="lblAdvancedSearchResultHKICText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"
                                                Height="25px"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblAdvancedSearchResultHKIC" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px; height: 25px">
                                            <asp:Label ID="lblAdvancedSearchResultNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"
                                                Height="25px"></asp:Label>
                                        </td>
                                        <td style="width: 180px; height: 25px">
                                            <asp:Label ID="lblAdvancedSearchResultName" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                        <td style="width: 180px; height: 25px">
                                            <asp:Label ID="lblAdvancedSearchResultPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"
                                                Height="25px"></asp:Label>
                                        </td>
                                        <td>
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
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnAdvancedSearchSPID" runat="server" Text='<%# Eval("SP_ID") %> '
                                                    CommandArgument='<%# Eval("SP_ID") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="SP_HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvancedSearchSPHKID" runat="server" Text='<%# Eval("SP_HKID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="SP_Eng_Name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvancedSearchEname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                                <asp:Label ID="lblAdvancedSearchCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Phone_Daytime" HeaderText="<%$ Resources:Text, ContactNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvancedSearchPhone" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, SchemeName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdvancedSearchScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center" colspan="2" style="width: 100px">
                                            <asp:ImageButton ID="ibtnAdvancedSearchResultBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnAdvancedSearchResultBack_Click" /></td>
                                        <td align="center" colspan="1">
                                            <asp:ImageButton ID="ibtnAdvancedSearchResultClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CloseBtn %>" OnClick="ibtnAdvancedSearchSPClose_Click" /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Advanced Search of SP --%>
            <asp:Button Style="display: none" ID="btnHiddenCCCode" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderChooseCCCode" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenCCCode" PopupControlID="panChooseCCCode" RepositionMode="None"
                PopupDragHandleControlID="panChooseCCCodeHeading">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnHiddenShowDialog" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirm" runat="server" TargetControlID="btnHiddenShowDialog"
                PopupControlID="panConfirmMsg" BehaviorID="mdlPopup" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
            <%-- Popup for DocType Help --%>
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
                <uc4:ucInputToken ID="udcPUInputToken" runat="server"></uc4:ucInputToken>
            </asp:Panel>
            <%-- End of Pop up for Unmask --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
