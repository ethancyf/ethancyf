<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcSTAT00001Criteria.ascx.vb"
    Inherits="HCVU.udcSTAT00001Criteria" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <asp:Panel ID="panPeriodBreakDown" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblPeriodBreakDown" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BreakDownType %>" />
            </td>
            <td align="left" style="width: 600px">
                <asp:DropDownList ID="ddlPeriodBreakDown" runat="server" Width="200px" />
                <asp:Image ID="imgErrorPeriodBreakDown" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
</table>
<table>
    <asp:Panel ID="panTypeOfBreakDown" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblTypeOfBreakDown" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BreakDownType %>" />
            </td>
            <td align="left" style="width: 600px;">
                <asp:DropDownList ID="ddlTypeOfBreakDown" runat="server" Width="200px" AppendDataBoundItems="True"
                    AutoPostBack="True" />
                <asp:Image ID="imgErrorTypeOfBreakDown" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
</table>
<table>
    <asp:Panel ID="panExactDatePeriod_Creation" runat="server">
        <tr>
            <asp:Panel ID="panFromDate_Creation" runat="server">
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblFromDate_Creation" runat="server" CssClass="tableTitle" Text="From"></asp:Label>
                </td>
                <td style="width: 110px">
                    <asp:TextBox ID="txtFromDate_D_Creation" MaxLength="10" runat="server" Width="80px"
                        onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                        onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                        onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterFromDate_D_Creation" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtFromDate_D_Creation" ValidChars="-" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <asp:ImageButton ID="btnFromDate_D_Creation" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                    <cc1:CalendarExtender ID="calExFromDate_D_Creation" CssClass="ajax_cal" runat="server" PopupButtonID="btnFromDate_D_Creation"
                        TargetControlID="txtFromDate_D_Creation" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                    </cc1:CalendarExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panToDate_Creation" runat="server">
                <td style="padding-right: 10px; padding-left: 10px;">
                    <asp:Label ID="lblToDate_Creation" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>"></asp:Label>
                </td>
                <td style="width: 110px">
                    <asp:TextBox ID="txtToDate_D_Creation" MaxLength="10" runat="server" Width="80px"
                        onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                        onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                        onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterToDate_D_Creation" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtToDate_D_Creation" ValidChars="-" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <asp:ImageButton ID="btnToDate_D_Creation" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                    <cc1:CalendarExtender ID="calExToDate_D_Creation" CssClass="ajax_cal" runat="server" PopupButtonID="btnToDate_D_Creation"
                        TargetControlID="txtToDate_D_Creation" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                    </cc1:CalendarExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panErrorDate_Creation" runat="server">
                <td>
                    <asp:Image ID="imgErrorDate_D_Creation" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Style="vertical-align: top" />
                </td>
            </asp:Panel>
        </tr>
    </asp:Panel>
</table>
<table>
    <asp:Panel ID="panExactDatePeriod_AsAt" runat="server">
        <tr>
            <asp:Panel ID="panFromDate_AsAt" runat="server">
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblFromDate_AsAt" runat="server" CssClass="tableTitle" Text="From"></asp:Label>
                </td>
                <td style="width: 110px">
                    <asp:TextBox ID="txtFromDate_D_AsAt" MaxLength="10" runat="server" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterFromDate_D_AsAt" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtFromDate_D_AsAt" ValidChars="-" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <asp:ImageButton ID="btnFromDate_D_AsAt" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                    <cc1:CalendarExtender ID="calExFromDate_D_AsAt" CssClass="ajax_cal" runat="server" PopupButtonID="btnFromDate_D_AsAt"
                        TargetControlID="txtFromDate_D_AsAt" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                    </cc1:CalendarExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panToDate_AsAt" runat="server">
                <td style="padding-right: 10px; padding-left: 10px;">
                    <asp:Label ID="lblToDate_AsAt" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>"></asp:Label>
                </td>
                <td style="width: 110px">
                    <asp:TextBox ID="txtToDate_D_AsAt" MaxLength="10" runat="server" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterToDate_D_AsAt" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtToDate_D_AsAt" ValidChars="-" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <asp:ImageButton ID="btnToDate_D_AsAt" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                    <cc1:CalendarExtender ID="calExToDate_D_AsAt" CssClass="ajax_cal" runat="server" PopupButtonID="btnToDate_D_AsAt"
                        TargetControlID="txtToDate_D_AsAt" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                    </cc1:CalendarExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panErrorDate_AsAt" runat="server">
                <td>
                    <asp:Image ID="imgErrorDate_D_AsAt" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Style="vertical-align: top" />
                </td>
            </asp:Panel>
        </tr>
    </asp:Panel>
</table>
<table>
    <asp:Panel ID="panAgeRange" runat="server">
        <tr>
            <asp:Panel ID="panMinAge" runat="server">
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblMinAge" runat="server" CssClass="tableTitle" Text="Age Range"></asp:Label>
                </td>
                <td style="width: 46px">
                    <asp:TextBox ID="txtMinAge" MaxLength="3" runat="server" Width="40px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterMinAge" runat="server" FilterType="Numbers"
                        TargetControlID="txtMinAge" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panMaxAge" runat="server">
                <td style="padding-right: 10px; padding-left: 10px;">
                    <asp:Label ID="lblMaxAge" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>"></asp:Label>
                </td>
                <td style="width: 46px">
                    <asp:TextBox ID="txtMaxAge" MaxLength="3" runat="server" Width="40px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterMaxAge" runat="server" FilterType="Numbers"
                        TargetControlID="txtMaxAge" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panErrorAge" runat="server">
                <td>
                    <asp:Image ID="imgErrorAge" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Style="vertical-align: top" />
                </td>
            </asp:Panel>
        </tr>
    </asp:Panel>
</table>
<table>
    <asp:Panel ID="panScheme" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblScheme" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>" />
            </td>
            <td align="left" style="width: 600px;">
                <asp:DropDownList ID="ddlScheme" runat="server" Width="200px" AppendDataBoundItems="True"
                    AutoPostBack="True" />
                <asp:Image ID="imgErrorScheme" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="panTypeOfCount" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblTypeOfCount" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CountingItem %>" />
            </td>
            <td align="left" style="width: 600px;">
                <asp:DropDownList ID="ddlTypeOfCount" runat="server" Width="200px" AppendDataBoundItems="True"
                    AutoPostBack="True" />
                <asp:Image ID="imgErrorTypeOfCount" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="panSubsidy" runat="server" Visible="False">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblSubsidy" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Subsidy %>" />
            </td>
            <td align="left" style="width: 600px;">
                <asp:DropDownList ID="ddlSubsidy" runat="server" Width="200px" AppendDataBoundItems="True" />
                <asp:Image ID="imgErrorSubsidy" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="panDistrict" runat="server" Visible="False">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblDistrict" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label>
            </td>
            <td align="left" style="width: 600px;">
                <table style="width: 600px;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px; vertical-align: top">
                            <asp:RadioButtonList ID="rbtnDistrictType" runat="server" RepeatDirection="Horizontal"
                                CellSpacing="0" CellPadding="0" AutoPostBack="True">
                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value="<%$ Resources:Text, Any %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Text, Specific %>" Value="<%$ Resources:Text, Specific %>"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 120px; vertical-align: top">
                            <asp:ImageButton ID="imgAddDistrict" runat="server" ImageUrl="<%$ Resources:ImageUrl, SelectSBtn %>"
                                AlternateText="<%$ Resources:AlternateText, SelectSBtn %>" OnClick="ibtnAddDistrict_Click" />
                            <asp:Image ID="imgErrorDistrict" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" />
                        </td>
                        <td style="width: 360px">
                            <asp:Label ID="lblAddDistrictDisplay" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="panProfession" runat="server" Visible="False">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblProfession" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
            </td>
            <td align="left" style="width: 600px;">
                <table style="width: 600px;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px; vertical-align: top">
                            <asp:RadioButtonList ID="rbtnProfessionType" runat="server" RepeatDirection="Horizontal"
                                CellSpacing="0" CellPadding="0" AutoPostBack="True">
                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value="<%$ Resources:Text, Any %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Text, Specific %>" Value="<%$ Resources:Text, Specific %>"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 120px; vertical-align: top">
                            <asp:ImageButton ID="imgAddProfession" runat="server" ImageUrl="<%$ Resources:ImageUrl, SelectSBtn %>"
                                AlternateText="<%$ Resources:AlternateText, SelectSBtn %>" OnClick="ibtnAddProfession_Click" />
                            <asp:Image ID="imgErrorProfession" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: top" />
                        </td>
                        <td style="width: 360px">
                            <asp:Label ID="lblAddProfessionDisplay" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
</table>
<%-- Popup for multi selection, district (Start) --%>
<asp:Button ID="btnHiddenDistrict" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="popupDistrict" runat="server" TargetControlID="btnHiddenDistrict"
    PopupControlID="panPopupDistrict" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="None">
</cc1:ModalPopupExtender>
<asp:Panel ID="panPopupDistrict" runat="server" Style="display: none;">
    <%-- Panel header --%>
    <asp:Panel ID="panPopupDistrictHeading" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: auto">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblRejectTitle" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label>
                </td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                </td>
            </tr>
            <%-- Panel body --%>
            <tr>
                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                </td>
                <td style="background-color: #ffffff">
                    <%-- Panel body content --%>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblPleaseSelectDistrict" runat="server" Text="Please Select:" />
                            </td>
                            <td style="width: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: auto; height: 42px" valign="middle">
                                <asp:CheckBoxList ID="chkDistrict" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                </asp:CheckBoxList>
                            </td>
                            <td style="width: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:ImageButton ID="ibtnDistrictPopupOK" runat="server" ImageUrl="<%$ Resources:ImageUrl, OKBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, OKBtn %>" OnClick="ibtnDistrictPopupOK_Click" />
                                <asp:ImageButton ID="ibtnDistrictPopupCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnDistrictPopupCancel_Click" />
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
</asp:Panel>
<%-- Popup for multi selection, district (End) --%>
<%-- Popup for multi selection, profession (Start) --%>
<asp:Button ID="btnHiddenProfession" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="popupProfession" runat="server" TargetControlID="btnHiddenProfession"
    PopupControlID="panPopupProfession" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="None">
</cc1:ModalPopupExtender>
<asp:Panel ID="panPopupProfession" runat="server" Style="display: none;">
    <%-- Panel header --%>
    <asp:Panel ID="panPopupProfessionHeading" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: auto">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                </td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                </td>
            </tr>
            <%-- Panel body --%>
            <tr>
                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                </td>
                <td style="background-color: #ffffff">
                    <%-- Panel body content --%>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblPleaseSelectProfession" runat="server" Text="Please Select:" />
                            </td>
                            <td style="width: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: auto; height: 42px" valign="middle">
                                <asp:CheckBoxList ID="chkProfession" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                </asp:CheckBoxList>
                            </td>
                            <td style="width: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:ImageButton ID="ibtnProfessionPopupOK" runat="server" ImageUrl="<%$ Resources:ImageUrl, OKBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, OKBtn %>" OnClick="ibtnProfessionPopupOK_Click" />
                                <asp:ImageButton ID="ibtnProfessionPopupCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnProfessionPopupCancel_Click" />
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
</asp:Panel>
<%-- Popup for multi selection, profession (End) --%>
