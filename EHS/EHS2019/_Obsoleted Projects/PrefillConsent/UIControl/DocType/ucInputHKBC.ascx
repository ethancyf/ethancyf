<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKBC.ascx.vb"
    Inherits="PrefillConsent.ucInputHKBC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Panel ID="panInputHKBC" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr runat="server" id="trReferenceNo_M">
                        <td style="height: 19px; width: 90px" valign="top">
                            <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 330px;" valign="top">
                            <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 19px; width: 90px" valign="top">
                            <asp:Label ID="lblRegistrationNoText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 330px;" valign="top">
                            <asp:TextBox ID="txtRegistrationNo" runat="server" MaxLength="11" Width="80px" onChange="formatHKID(this);"></asp:TextBox>
                            <asp:Label ID="lblRegistrationNo" runat="server" Visible="False" CssClass="tableText"></asp:Label>
                            <asp:Image ID="imgRegNoError" runat="server" />
                            </td>
                        <td valign="top">
                            <asp:Label ID="lblRegTip" runat="server" CssClass="tableTip"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 90px">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 5px; width: 330px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" colspan="4">
                                        <asp:RadioButton ID="rbDOB" runat="server" AutoPostBack="True" GroupName="DOBType" />
                                        <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" onfocus="clickHKBC_DOB();" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                        <asp:Image ID="imgDOBError" runat="server" ImageAlign="AbsMiddle" /></td>
                                </tr>
                                <tr>
                                    <td colspan="4" valign="top">
                                        <asp:RadioButton ID="rbDOBInWord" runat="server" AutoPostBack="True" GroupName="DOBType"
                                            Width="90px" CssClass="tableText" />
                                        <asp:DropDownList ID="ddlDOBinWordType" runat="server" Width="120px"></asp:DropDownList>
                                        <asp:TextBox ID="txtDOBInWord" runat="server" MaxLength="10" Width="75px" onfocus="clickHKBC_DOBInWord();" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                        <asp:Image ID="imgDOBInWordError" runat="server" ImageAlign="AbsMiddle" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblDOBTip" runat="server" CssClass="tableTip"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 90px">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 3px; width: 330px;">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox>
                                <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>
                            <asp:Image ID="imgENameError" runat="server" ImageAlign="Top" />
                        </td>
                        <td valign="top">
                            <asp:Label ID="txtENameTips" runat="server" CssClass="tableTip"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 90px"></td>
                        <td valign="top" style="padding-bottom: 5px; width: 330px">
                            <asp:Label ID="lblSurname" runat="server" CssClass="tableTip" Width="118px"></asp:Label>
                            <asp:Label ID="lblGivenName" runat="server" CssClass="tableTip"></asp:Label>
                        </td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 90px">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 280px">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderError" runat="server" ImageAlign="Top" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr id="trTransactionNo_M" runat="server">
                        <td style="height: 19px; width: 90px;" valign="top">
                            <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 330px;" valign="top">
                            <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameSurname" ValidChars="-' ">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameFirstname" ValidChars="-' ">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOB" ValidChars="-">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOBInWord" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOBInWord" ValidChars="-">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditRegNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
    TargetControlID="txtRegistrationNo" ValidChars="()">
</cc1:FilteredTextBoxExtender>
