<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputROP140.ascx.vb" Inherits="HCVU.ucInputROP140" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel runat="server" ID="pnlModify">
    <table>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td valign="top" style="width: 220px"></td>
                        <td style="width: 250px">
                            <asp:Label ID="lblOriginalRecordText" Text="<%$ Resources:Text, OriginalRecord %>"
                                runat="server" /></td>
                        <td style="width: 350px;">
                            <asp:Label ID="lblAmendingRecordText" Text="<%$ Resources:Text, AmendingRecord %>"
                                runat="server" /></td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDocumentTypeOriginalText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                        <td >
                            <asp:Label ID="lblDocumentTypeOriginal" runat="server"   CssClass="tableText"></asp:Label>
                        </td>
                        <td valign="top"></td><td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblTravelDocNoOriginalText" runat="server" Text="<%$ Resources:Text, TravelDocNo %>"></asp:Label></td>
                        <td style="height: 25px">
                            <asp:Label ID="lblTravelDocNoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtTDNo" runat="server" MaxLength="11" onChange="convertToUpper(this);"
                                Width="120px" Enabled="False"></asp:TextBox>
                            <asp:Label ID="lblTDNo" runat="server" Visible="False" CssClass="tableText"></asp:Label>
                            <asp:Image ID="imgTDNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblNameOrignialText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                        <td style="width: 350px; word-wrap: break-word; word-break:break-all">
                            <asp:Label ID="lblNameOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>
                            &nbsp;<asp:Image ID="imgEName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top" style="height: 20"></td>                        
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
                        <td></td>
                    </tr>
                    <tr>
                        <td valign="top" style="height: 28"></td>
                        <td>
                            <asp:Label ID="lblCNameOriginal" runat="server" CssClass="tableText TextChineseName"
                                Text=""></asp:Label></td>
                        <td style="width: 350px;">
                            <asp:Label ID="lblCName" runat="server" MaxLength="10" Width="150px"
                                CssClass="tableText TextChineseName"></asp:Label></td>
                        <td></td>
                    </tr>

                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblGenderOriginalText" runat="server" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                        </td>
                        <td style="height: 23px">
                            <asp:Label ID="lblGenderOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOBOriginalText" runat="server" Text="<%$ Resources:Text, DOB %>"></asp:Label></td>
                        <td style="height: 23px">
                            <asp:Label ID="lblDOBOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False"
                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOIOriginalText" runat="server" Text="<%$ Resources:Text, ECDate %>"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblDOIOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtDOI" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOIDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top"></td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filtereditTDNo" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
        TargetControlID="txtTDNo"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameSurname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameFirstname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode1" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode1"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode2" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode2"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode3" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode3"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode4" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode4"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode5" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode5"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtererditCCcode6" runat="server" FilterType="Numbers"
        TargetControlID="txtCCCode6"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtDOB" ValidChars="-"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOI" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtDOI" ValidChars="-"></cc1:FilteredTextBoxExtender>

</asp:Panel>
<asp:Panel runat="server" ID="pnlNew">
    <table>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewTravelDocNoText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px">
                <asp:Label ID="lblNewTravelDocNo" runat="server" Visible="False" CssClass="tableText" />
                <asp:TextBox ID="txtNewTravelDocNo" runat="server" MaxLength="11" onChange="convertToUpper(this);"
                    Width="120px"></asp:TextBox>
                <asp:Image ID="imgNewTravelDocNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
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
                        <td>(<asp:Label ID="lblNewENameSurnameTips" runat="server"></asp:Label>)</td>
                        <td></td>
                        <td>(<asp:Label ID="lblNewENameGivenNameTips" runat="server"></asp:Label>)</td>
                        <td></td>
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
                <asp:Label ID="lblNewCName" runat="server" MaxLength="10" Width="150px"
                    CssClass="tableText TextChineseName"></asp:Label></td>
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
                <asp:TextBox ID="txtNewDOI" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgNewDOIErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewENameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewSurname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewENameGivenname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewGivenName" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode1" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode1"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode2" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode2"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode3" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode3"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode4" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode4"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode5" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode5"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewCCCode6" runat="server" FilterType="Numbers"
        TargetControlID="txtNewCCCode6"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewDOB" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtNewDOB" ValidChars="-"></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextBNewBOI" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtNewDOI" ValidChars="-"></cc1:FilteredTextBoxExtender>
</asp:Panel>
