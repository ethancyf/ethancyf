<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputReentryPermit.ascx.vb"
    Inherits="HCVU.UIControl.DocTypeHCSP.ucInputReentryPermit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel ID="panInputReentryPermit" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr runat="server" id="trReferenceNo">
                        <td valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblReferenceNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblReferenceNo" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblTravelDocNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="width: 350px;padding-bottom: 5px;" valign="top" class="tableCellStyleLite">
                            <asp:TextBox ID="txtTravelDocNo" runat="server" MaxLength="9" Width="75px" />
                            <asp:Label ID="lblTravelDocNo" runat="server" Visible="False" CssClass="tableText" />
                            <asp:Image ID="imgTravelDocNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top" Visible="false" />

                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyleLite">
                            <table id="TABLE1" border="0" cellpadding="0" cellspacing="0" language="javascript">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                            Width="105px"></asp:TextBox></td>
                                    <td>
                                        <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label></td>
                                    <td>
                                        &nbsp;<asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox></td>
                                    <td>
                                        &nbsp;<asp:Image ID="imgENameError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                            ImageAlign="Top" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSurname" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;<asp:Label ID="lblGivenName" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;vertical-align:top" class="tableCellStyleLite">
                            <asp:Label ID="lblCName" runat="server" CssClass="tableTitle" Width="150px" />
                        </td>
                        <td style="padding-bottom: 5px; width: 350px;vertical-align:top" class="tableCellStyleLite">
                            <asp:TextBox ID="txtCName" runat="server" MaxLength="12" CssClass="TextBoxChineseName" onChange="convertToUpper(this);" Width="150px" />
                            <asp:Image ID="imgCNameError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 350px;padding-bottom: 5px;" class="tableCellStyleLite">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top"  Visible="false" /></td>
                    </tr>
                    <tr>
                        <td valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 350px;padding-bottom: 5px;" class="tableCellStyleLite">
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top"  Visible="false" /></td>
                    </tr>
                    <tr>
                        <td valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblDOI" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 350px;padding-bottom: 5px;" class="tableCellStyleLite">
                            <asp:TextBox ID="txtDOI" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOIError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top" Visible="false" /></td>
                    </tr>
                    <tr id="trTransactionNo" runat="server">
                        <td valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblTransactionNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="width: 350px; padding-bottom: 5px;" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblTransactionNo" runat="server"></asp:Label></td>
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
<cc1:FilteredTextBoxExtender ID="filtereditTravelDocNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
    TargetControlID="txtTravelDocNo">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOB" ValidChars="-">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditIssueDate" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOI" ValidChars="-">
</cc1:FilteredTextBoxExtender>
