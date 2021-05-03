<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputROP140.ascx.vb" Inherits="HCSP.ucInputROP140" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel ID="panInputROP140" runat="server">
    <table style="padding:0px;border-collapse:collapse;border-spacing:0px">
        <tr runat="server" id="trReferenceNo_M">
            <td style="height: 19px; width: 200px;vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 350px;vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 200px;vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblTDNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px;vertical-align:top;" class="tableCellStyle">
                <asp:TextBox ID="txtTDNo" runat="server" MaxLength="11" onChange="convertToUpper(this);" Width="120px" />
                <asp:Label ID="lblTDNo" runat="server" CssClass="tableText" />
                <asp:Image ID="imgTDNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top" Visible="false" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px;vertical-align:top;" class="tableCellStyle">
                <table style="border-width:0px;padding:0px;border-collapse:collapse;border-spacing:0px">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label></td>
                        <td>&nbsp;<asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox></td>
                        <td>&nbsp;<asp:Image ID="imgEName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                            ImageAlign="Top" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSurname" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td></td>
                        <td>&nbsp;<asp:Label ID="lblGivenName" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tableTitle">
                <asp:Label ID="lblCCCodeText" runat="server" CssClass="tableTitle" />
            </td>
            <td style="vertical-align:top;width: 350px" class="tableCellStyle">
                <asp:TextBox ID="txtCCCode1" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="40px" />
                <asp:TextBox ID="txtCCCode2" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="40px" />
                <asp:TextBox ID="txtCCCode3" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="40px" />
                <asp:TextBox ID="txtCCCode4" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="40px" />
                <asp:TextBox ID="txtCCCode5" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="40px" />
                <asp:TextBox ID="txtCCCode6" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="40px" />
                <asp:Image ID="imgCCCodeError" runat="server"  Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                <asp:ImageButton ID="btnSearchCCCode" runat="server" style="position:relative;top:5px" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <asp:Label ID="lblCNameText" runat="server" CssClass="tableTitle"  Height="25px" />
            </td>            
            <td style="width: 470px;">
                <asp:Label ID="lblCName" runat="server" Width="150px" Font-Names="HA_MingLiu" CssClass="tableText" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="vertical-align:top;width: 350px" class="tableCellStyle">
                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>&nbsp;
                <asp:Image ID="imgGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top" Visible="false" /></td>
        </tr>
        <tr>
            <td style="vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="vertical-align:top;width: 350px" class="tableCellStyle">
                <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgDOBDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top" Visible="false" /></td>
        </tr>
        <tr>
            <td style="vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblDOI" runat="server" CssClass="tableTitle" Width="150px" />
            </td>
            <td style="vertical-align:top;width:700px" class="tableCellStyle">
                <table style="padding:0px;border-collapse:collapse;border-spacing:0px">
                    <tr>
                        <td style="vertical-align:top;width: 310px">
                            <asp:TextBox ID="txtDOI" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOIDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top" Visible="false" />
                        </td>
                        <td style="vertical-align:top;">
                            <asp:Label ID="lblDOIROP140Hint" runat="server" CssClass="tableTitle"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trTransactionNo_M" runat="server">
            <td style="height: 19px;vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 350px;vertical-align:top;" class="tableCellStyle">
                <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<cc1:FilteredTextBoxExtender ID="filtereditTDNo" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
    TargetControlID="txtTDNo"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameSurname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameFirstname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Numbers, Custom"
    TargetControlID="txtDOB" ValidChars="-"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOI" runat="server" FilterType="Numbers, Custom"
    TargetControlID="txtDOI" ValidChars="-"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode1" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode1"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode2" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode2"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode3" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode3"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode4" runat="server" FilterType="Numbers"
    TargetControlID="txtCCcode4"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode5" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode5"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode6" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode6"></cc1:FilteredTextBoxExtender>


