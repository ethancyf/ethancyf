<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputCommon.ascx.vb"
    Inherits="HCVU.ucInputCommon" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel runat="server" ID="pnlModify">
    <table runat="server" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td valign="top" style="width: 140px"></td>
                        <td valign="top" style="width: 360px">
                            <asp:Label ID="lblOriginalRecordText" Text="<%$ Resources:Text, OriginalRecord %>"
                                runat="server" /></td>
                        <td style="width: 400px;" valign="top">
                            <asp:Label ID="lblAmendingRecordText" Text="<%$ Resources:Text, AmendingRecord %>"
                                runat="server" /></td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 220px;">
                            <asp:Label ID="lblDocumentTypeOriginalText" runat="server"></asp:Label></td>
                        <td valign="top" >
                            <asp:Label ID="lblDocumentTypeOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td valign="top">
                            <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDocNoOriginalText" runat="server"></asp:Label></td>
                        <td valign="top">
                            <asp:Label ID="lblDocNoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px;" valign="top">
                            <asp:TextBox ID="txtDocNo" runat="server" MaxLength="6" Enabled="False"
                                Width="90px"></asp:TextBox>
                            <asp:Label ID="lblDocNo" runat="server" Visible="False" CssClass="tableText"></asp:Label></td>
                        <td valign="top"></td>
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
                        <td valign="top"></td>
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
                                        <asp:TextBox ID="txtDOB" runat="server" Enabled="False" MaxLength="10" Width="75px"
                                            onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                            onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                            onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                        <asp:Image ID="imgDOBError" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                </tr>

                            </table>
                        </td>
                        <td style="padding-bottom: 5px;" valign="top"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameSurname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameFirstname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOB" ValidChars="-"></cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel runat="server" ID="pnlNew">
    <table>
        <tr>
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblNewDocNoText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <asp:TextBox ID="txtNewDocNo" runat="server" MaxLength="11" onChange="convertToUpper(this);"></asp:TextBox>
                <asp:Label ID="lblNewDocNo_ModifyOneSide" runat="server" Visible="False" CssClass="tableText"></asp:Label>
                <asp:Image
                    ID="imgNewDocNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
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
                <asp:Label ID="lblNewGenderText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <asp:RadioButtonList ID="rboNewGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal" RepeatLayout="Flow" Style="position: relative; left: -3px">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>&nbsp;
                <asp:Image ID="imgNewGenderErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewDOBText" runat="server" Text="<%$ Resources:Text, DOB %>" Height="25px"></asp:Label></td>
            <td valign="top" style="width: 470px">
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td valign="top" colspan="4">
                            <asp:TextBox ID="txtNewDOB" runat="server" MaxLength="10" Width="75px"
                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                            <asp:Image ID="imgNewDOBErr" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredNewDocNo" runat="server" FilterType="Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtNewDocNo" ValidChars=""></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewSurname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredNewEnameGivename" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewGivenName" ValidChars="-' "></cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredNewDOB" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtNewDOB" ValidChars="-"></cc1:FilteredTextBoxExtender>
</asp:Panel>

