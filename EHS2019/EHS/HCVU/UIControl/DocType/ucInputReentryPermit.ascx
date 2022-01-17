<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputReentryPermit.ascx.vb"
    Inherits="HCVU.ucInputReentryPermit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel runat="server" ID="pnlModify">
    <table border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td valign="top" style="width: 220px">
                        </td>
                        <td valign="top" style="width: 250px">
                            <asp:Label ID="lblOriginalRecordText" Text="<%$ Resources:Text, OriginalRecord %>"
                                runat="server" /></td>
                        <td style="width: 350px;">
                            <asp:Label ID="lblAmendingRecordText" Text="<%$ Resources:Text, AmendingRecord %>"
                                runat="server" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDocumentTypeOriginalText" runat="server"></asp:Label></td>
                        <td colspan="3">
                            <asp:Label ID="lblDocumentTypeOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblReentryPermitNoOriginalText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblReentryPermitNoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:Label ID="lblReentryPermitNo" runat="server" Visible="False" CssClass="tableText"></asp:Label>
                            <asp:Image ID="imgTravelDocNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblNameOriginalText" runat="server"></asp:Label></td>
                        <td style="width: 350px; word-wrap: break-word; word-break:break-all;">
                            <asp:Label ID="lblNameOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>&nbsp;<asp:Image
                                ID="imgENameError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px;vertical-align:top">
                            <asp:label ID="lblCNameOriginalText" runat="server"/>
                        </td>
                        <td style="width: 350px;vertical-align:top; word-wrap: break-word; word-break:break-all;">
                            <asp:Label ID="lblCNameOriginal" runat="server" CssClass="tableText" />
                        </td>
                        <td style="padding-bottom: 5px; width: 350px;vertical-align:top">
                            <asp:TextBox ID="txtCName" runat="server" MaxLength="12" CssClass="TextBoxChineseName" onChange="convertToUpper(this);" Width="150px" />
                            <asp:Image ID="imgCNameError" runat="server" ImageAlign="Top" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblGenderOriginalText" runat="server"></asp:Label>
                        </td>
                        <td style="height: 23px">
                            <asp:Label ID="lblGenderOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOBOriginalText" runat="server"></asp:Label></td>
                        <td style="height: 23px">
                            <asp:Label ID="lblDOBOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False"
                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOIOriginalText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblDOIOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtDOI" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOIError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameSurname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameFirstname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditIssueDate" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOI" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel runat="server" ID="pnlNew">
    <table>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewPermitNoText" runat="server"></asp:Label></td>
            <td style="width: 470px;">
                <asp:Label ID="lblNewPermitNo" runat="server" Visible="False" CssClass="tableText" /> 
                <asp:TextBox ID="txtNewPermitNo" runat="server" MaxLength="9" Width="75px" onChange="convertToUpper(this);"></asp:TextBox>
                <asp:Image ID="imgNewPermitNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top">
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewNameText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNewSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="lblNewEnameComma" runat="server" CssClass="tableText"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtNewGivenName" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox></td>
                        <td>
                            <asp:Image ID="imgNewENameErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                    <tr>
                        <td>
                            (<asp:Label ID="lblNewENameSurnameTips" runat="server"></asp:Label>)</td>
                        <td>
                        </td>
                        <td>
                            (<asp:Label ID="lblNewENameGivenNameTips" runat="server"></asp:Label>)</td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewCNameText" runat="server" Text="<%$ Resources:Text, Name %>" Height="25px" />
            </td>
            <td style="width: 470px;">
                <asp:TextBox ID="txtNewCName" runat="server" MaxLength="12" CssClass="TextBoxChineseName" onChange="convertToUpper(this);" Width="150px" />
                <asp:Image ID="imgNewCNameErr" runat="server" ImageAlign="Top" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblNewGenderText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <asp:RadioButtonList ID="rboNewGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>&nbsp;
                <asp:Image ID="imgNewGenderErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewDOBText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px">
                <asp:TextBox ID="txtNewDOB" runat="server" MaxLength="10" Width="75px" 
                    onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                    onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                    onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgNewDOBErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewDOIText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px">
                <asp:TextBox ID="txtNewDOI" runat="server" MaxLength="10" Width="75px" 
                    onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                    onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                    onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgNewDOIErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filterNewEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtNewSurname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewGivenName" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewTravelDocNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtNewPermitNo">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewIssueDate" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOI" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
