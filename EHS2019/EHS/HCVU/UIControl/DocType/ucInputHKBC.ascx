<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKBC.ascx.vb"
    Inherits="HCVU.ucInputHKBC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel runat="server" ID="pnlModify">
    <table runat="server" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td valign="top" style="width: 220px">
                        </td>
                        <td valign="top" style="width: 250px">
                            <asp:Label ID="lblOriginalRecordText" Text="<%$ Resources:Text, OriginalRecord %>"
                                runat="server" /></td>
                        <td style="width: 350px;" valign="top">
                            <asp:Label ID="lblAmendingRecordText" Text="<%$ Resources:Text, AmendingRecord %>"
                                runat="server" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 220px;">
                            <asp:Label ID="lblDocumentTypeOriginalText" runat="server"></asp:Label></td>
                        <td valign="top" colspan="3">
                            <asp:Label ID="lblDocumentTypeOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td valign="top">
                            <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblRegNoOriginalText" runat="server"></asp:Label></td>
                        <td valign="top">
                            <asp:Label ID="lblRegNoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;" valign="top">
                            <asp:TextBox ID="txtRegistrationNo" runat="server" MaxLength="6" Enabled="False"
                                Width="90px"></asp:TextBox>
                            <asp:Label ID="lblRegistrationNo" runat="server" Visible="False" CssClass="tableText"></asp:Label></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOBOriginalText" runat="server"></asp:Label></td>
                        <td valign="top">
                            <asp:Label ID="lblDOBOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 5px; width: 350px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <asp:RadioButton ID="rbDOB" runat="server" AutoPostBack="True" GroupName="DOBType" />
                                        <asp:TextBox ID="txtDOB" runat="server" Enabled="False" MaxLength="10" Width="75px"
                                            onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                            onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                            onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                        <asp:Image ID="imgDOBError" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:RadioButton ID="rbDOBInWord" runat="server" AutoPostBack="True" GroupName="DOBType"
                                            Width="90px" CssClass="tableText" /><asp:DropDownList ID="ddlDOBinWordType" runat="server"
                                                Width="120px">
                                            </asp:DropDownList><asp:TextBox ID="txtDOBInWord" runat="server" Enabled="False"
                                                MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                        <asp:Image ID="imgDOBInWordError" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding-bottom: 5px;" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="height: 25px">
                            <asp:Label ID="lblNameOriginalText" runat="server"></asp:Label></td>
                        <td valign="top" style="width: 350px; word-wrap: break-word; word-break:break-all;">
                            <asp:Label ID="lblNameOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 5px; width: 350px;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 1px">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                            Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                                        <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>
                                        <asp:Image ID="imgENameError" runat="server" ImageAlign="Top" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding-bottom: 5px;" valign="top">
                            <asp:Label ID="txtENameTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="height: 35px">
                            <asp:Label ID="lblGenderOriginalText" runat="server"></asp:Label>
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblGenderOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td valign="top" style="width: 350px">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderError" runat="server" ImageAlign="Top" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
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
    <cc1:FilteredTextBoxExtender ID="filtereditDOBInWord" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOBInWord" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel runat="server" ID="pnlNew">
    <table>
        <tr>
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblNewRegNoText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <asp:TextBox ID="txtNewRegNo" runat="server" MaxLength="11" onChange="convertToUpper(this);"></asp:TextBox>
                <asp:Label ID="lblNewRegNo_ModifyOneSide" runat="server"  Visible="False" CssClass="tableText"></asp:Label>  <asp:Image
                    ID="imgNewRegNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewDOBText" runat="server" Text="<%$ Resources:Text, DOB %>" Height="25px"></asp:Label></td>
            <td valign="top" style="width: 470px">
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td valign="top" colspan="4">
                            <asp:RadioButton ID="rboNewDOBType" runat="server" CssClass="tableTitle" GroupName="DOBType"
                                AutoPostBack="True"></asp:RadioButton>
                            <asp:TextBox ID="txtNewDOB" runat="server" Width="75px" Enabled="False" MaxLength="10"
                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                            <asp:Image ID="imgNewDOBErr" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="4">
                            <asp:RadioButton ID="rboNewDOBInWord" runat="server" Width="90px" CssClass="tableText"
                                GroupName="DOBType" AutoPostBack="True"></asp:RadioButton><asp:DropDownList ID="ddlNewDOBInWord"
                                    runat="server" Width="120px">
                                </asp:DropDownList><asp:TextBox ID="txtNewDOBInWord" runat="server" Width="75px"
                                    Enabled="False" MaxLength="10" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgNewDOBInWordErr" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"></asp:Image></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewNameText" runat="server" Text="<%$ Resources:Text, Name %>"
                    Height="25px"></asp:Label></td>
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
    <cc1:FilteredTextBoxExtender ID="filteredNewRegNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtNewRegNo" ValidChars="()">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewSurname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewEnameGivename" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewGivenName" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredNewDOB" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtNewDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredNewDOBInWord" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtNewDOBInWord" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
