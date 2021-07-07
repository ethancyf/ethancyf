<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKID.ascx.vb"
    Inherits="HCVU.ucInputHKID" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel runat="server" ID="pnlModify">
    <table runat="server" border="0" cellspacing="0" cellpadding="0" >
        <tr>
            <td valign="top">
                <table runat="server">
                    <tr>
                        <td valign="top" style="width: 220px">
                        </td>
                        <td style="width: 250px">
                            <asp:Label ID="lblOriginalRecordText" runat="server" Text="<%$ Resources:Text, OriginalRecord %>" /></td>
                        <td style="width: 350px;">
                            <asp:Label ID="lblAmendingRecordText" Text="<%$ Resources:Text, AmendingRecord %>"
                                runat="server" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 220">
                            <asp:Label ID="lblDocumentTypeOriginalText" runat="server"></asp:Label></td>
                        <td colspan="3">
                            <asp:Label ID="lblDocumentTypeOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblHKICOriginalText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblHKICOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:Label ID="lblHKICNo" runat="server" MaxLength="9" Width="100px" CssClass="tableText"></asp:Label></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOBOriginalText" runat="server"></asp:Label></td>
                        <td style="height: 25">
                            <asp:Label ID="lblDOBOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblNameOriginalText" runat="server"></asp:Label></td>
                        <td style="height: 25; width: 350px; word-wrap: break-word; word-break:break-all">
                            <asp:Label ID="lblENameOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px; ">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>&nbsp;<asp:Image
                                ID="imgENameError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="height: 20">
                        </td>
                        <td>
                            <asp:Label ID="lblCCCodeOrginal" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td style="width: 470px;">
                            <asp:TextBox ID="txtCCCode1" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode2" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode3" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>&nbsp;
                            <asp:TextBox ID="txtCCCode4" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode5" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode6" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:ImageButton ID="btnSearchCCCode" ImageUrl="<%$ Resources:ImageUrl, ChineseNameSBtn %>"
                                runat="server" Visible="true" OnClick="btnSearchCCCode_Click" />
                            <asp:Image ID="imgCCCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="height: 28">
                        </td>
                        <td>
                            <asp:Label ID="lblCNameOriginal" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"
                                Text=""></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:Label ID="lblCName" runat="server" MaxLength="10" Width="150px" Font-Names="HA_MingLiu"
                                CssClass="tableText"></asp:Label></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblGenderOrignialText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblGenderOrignial" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
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
                            <asp:Label ID="lblHKIDIssueDateOriginalText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblHKIDIssueDateOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:TextBox ID="txtDOI" runat="server" MaxLength="8" Width="55px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOIError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr id="trCreationMethodOriginal" runat="server">
                        <td valign="top">
                            <asp:Label ID="lblCreationMethodOriginalText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblCreationMethodOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameSurname" ValidChars="-' .">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameFirstname" ValidChars="-' .">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode1" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode1">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode2" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode2">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode3" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode3">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode4" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode4">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode5" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode5">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode6" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode6">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOI" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOI" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel runat="server" ID="pnlNew">
    <table id="Table1" runat="server">
        <tr>
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblNewHKICText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <asp:Label ID="lblNewHKIC" runat="server" Visible="False" CssClass="tableText" /> 
                <asp:TextBox ID="txtNewHKIC" runat="server" MaxLength="11" onChange="convertToUpper(this);"></asp:TextBox><asp:Image
                    ID="imgNewHKIDErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 32px;">
                <asp:Label ID="lblNewDOBText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px; height: 32px;">
                <asp:TextBox ID="txtNewDOB" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgNewDOBErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblNewENameText" runat="server" Height="25px"></asp:Label></td>
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
                            <asp:Image
                    ID="imgNewENameErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
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
            <td valign="top" style="width: 220px; height: 25px;">
            <asp:Label ID="lblNewCCCodeText" runat="server" Height="25px"></asp:Label>
            </td>
            <td style="width: 470px;">
                <asp:TextBox ID="txtNewCCCode1" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                    Width="40px"></asp:TextBox>
                <asp:TextBox ID="txtNewCCCode2" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                    Width="40px"></asp:TextBox>
                <asp:TextBox ID="txtNewCCCode3" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                    Width="40px"></asp:TextBox>&nbsp;
                <asp:TextBox ID="txtNewCCCode4" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                    Width="40px"></asp:TextBox>
                <asp:TextBox ID="txtNewCCCode5" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                    Width="40px"></asp:TextBox>
                <asp:TextBox ID="txtNewCCCode6" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                    Width="40px"></asp:TextBox>
                <asp:ImageButton ID="ibtnNewCCCode" ImageUrl="<%$ Resources:ImageUrl, ChineseNameSBtn %>"
                    runat="server" Visible="true" OnClick="ibtnNewCCCode_Click" />
                <asp:Image ID="imgNewCCCodeErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px;">
            <asp:Label ID="lblNewCNameText" runat="server" Height="25px"></asp:Label>
            </td>            
            <td style="width: 470px;">
                <asp:Label ID="lblNewCName" runat="server" MaxLength="10" Width="150px" Font-Names="HA_MingLiu"
                    CssClass="tableText"></asp:Label></td>
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
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblNewDOIText" runat="server" Height="25px"></asp:Label></td>            
            <td style="width: 470px;">
                <asp:TextBox ID="txtNewDOI" runat="server" MaxLength="8" Width="55px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgNewDOIErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
        </tr> 
        <tr id="trDOD" runat="server">
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblDODText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <asp:Label ID="lblDOD" runat="server" CssClass="tableText" /> 
                <asp:Image ID="imgDOD" runat="server" AlternateText="<%$ Resources:AlternateText, DeathRecord %>" 
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>" />
            </td>          
        </tr>   
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredNewHKIC" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
    TargetControlID="txtNewHKIC" ValidChars="()">
</cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewSurname" ValidChars="-' .">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewEnameGivename" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewGivenName" ValidChars="-' .">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode1" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode1">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode2" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode2">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode3" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode3">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode4" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode4">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode5" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode5">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode6" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode6">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewDOI" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOI" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
