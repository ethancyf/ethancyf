<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputEC.ascx.vb"
    Inherits="HCVU.ucInputEC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel runat="server" ID="pnlModify">
    <table runat="server" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top">
                <table runat="server">
                    <tr>
                        <td valign="top" style="width: 220px">
                        </td>
                        <td valign="top" style="width: 250px">
                            <asp:Label ID="lblOriginalRecordText" Text="<%$ Resources:Text, OriginalRecord %>"
                                runat="server" /></td>
                        <td style="width: 400px;">
                            <asp:Label ID="lblAmendingRecordText" Text="<%$ Resources:Text, AmendingRecord %>"
                                runat="server" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblDocumentTypeOriginalText" runat="server"></asp:Label></td>
                        <td colspan="3">
                            <asp:Label ID="lblDocumentTypeOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblECHKIDOriginalText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblECHKIDOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblECHKIC" runat="server" CssClass="tableText"></asp:Label></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblECSerialNoOriginalText" runat="server"></asp:Label></td>
                        <td style="height: 23">
                            <asp:Label ID="lblECSerialNoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td valign="top">
                            <asp:TextBox ID="txtECSerialNo" runat="server" MaxLength="8" onChange="convertToUpper(this);"
                                Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="cboECSerialNoNotProvided" runat="server" Text="<%$ Resources:Text, NotProvided %>"
                                CssClass="tableText" AutoPostBack="True" OnCheckedChanged="cboECSerialNoNotProvided_CheckedChanged" />
                            <asp:Image ID="imgECSerialNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblECReferenceNoOriginalText" runat="server"></asp:Label></td>
                        <td style="height: 23">
                            <asp:Label ID="lblECReferenceNoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="vertical-align: top">
                            <asp:TextBox ID="txtECRefence1" runat="server" MaxLength="4" onChange="convertToUpper(this);"
                                Width="60px"></asp:TextBox>
                            <asp:Label ID="lblReferenceSep1" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                            <asp:TextBox ID="txtECRefence2" runat="server" MaxLength="7" onChange="convertToUpper(this);"
                                Width="60px"></asp:TextBox>
                            <asp:Label ID="lblReferenceSep2" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                            <asp:TextBox ID="txtECRefence3" runat="server" MaxLength="2" onChange="convertToUpper(this);"
                                Width="20px"></asp:TextBox>
                            <asp:Label ID="lblReferenceSep3" runat="server" CssClass="tableTitle" Text="("></asp:Label>
                            <asp:TextBox ID="txtECRefence4" runat="server" MaxLength="1" onChange="convertToUpper(this);"
                                Width="12px"></asp:TextBox>
                            <asp:Label ID="lblReferenceSep4" runat="server" CssClass="tableTitle" Text=")"></asp:Label>
                            <asp:TextBox ID="txtECRefFree" runat="server" MaxLength="40" Width="250px" onChange="convertToUpper(this);"></asp:TextBox>
                            <asp:ImageButton ID="ibtnOtherFormat" runat="server" ImageUrl="<%$ Resources:ImageUrl, OtherFormatsBtn %>"
                                AlternateText="<%$ Resources:AlternateText, OtherFormatsBtn %>" OnClick="ibtnOtherFormat_Click" />
                            <asp:ImageButton ID="ibtnSpecificFormat" runat="server" ImageUrl="<%$ Resources:ImageUrl, SpecificFormatBtn %>"
                                AlternateText="<%$ Resources:AlternateText, SpecificFormatBtn %>" OnClick="ibtnSpecificFormat_Click" />
                            <asp:Image ID="imgECRefence" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblECDateOriginalText" runat="server"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblECDateOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtECDateDay" runat="server" MaxLength="2" Width="20px"></asp:TextBox>
                            <asp:DropDownList ID="ddlECDateMonth" runat="server">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtECDateYear" runat="server" MaxLength="4" Width="36px"></asp:TextBox>
                            <asp:Image ID="imgECDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                            <asp:Label ID="lblECIssueDateHints" runat="server" CssClass="tableTitle"></asp:Label></td>        
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Label ID="lblNameOriginalText" runat="server"></asp:Label></td>
                        <td style="height: 23; width: 350px; word-wrap: break-word; word-break:break-all;">
                            <asp:Label ID="lblENameOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="height: 19px">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>&nbsp;<asp:Image
                                ID="imgEName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblCNameOriginalText" runat="server"></asp:Label></td>
                        <td style="height: 23">
                            <asp:Label ID="lblCNameOriginal" runat="server" CssClass="tableText TextChineseName"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtCName" runat="server" CssClass="TextBoxChineseName" MaxLength="12"></asp:TextBox>
                            <asp:Image ID="imgCName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblGenderOriginalText" runat="server"></asp:Label>
                        </td>
                        <td style="height: 23">
                            <asp:Label ID="lblGenderOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td>
                            <asp:RadioButtonList ID="rbECGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgECGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Label ID="lblDOBOriginalText" runat="server"></asp:Label></td>
                        <td valign="top">
                            <asp:Label ID="lblDOBOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 350px" valign="top" align="left">
                                        <asp:RadioButton ID="rbECDOBDate" runat="server" AutoPostBack="True" Checked="true"
                                            CssClass="tableTitle" GroupName="groupECDOB" Text="" />
                                        <asp:TextBox ID="txtDOBDate" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                            MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                            onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                            onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                        <asp:Label ID="lblECDOBDateText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;
                                        <asp:Label ID="lblECDOBOr1Text" runat="server" CssClass="tableTitle"></asp:Label>
                                        <asp:Image ID="imgECDOBerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 350px" valign="top">
                                        <asp:RadioButton ID="rbECDOByear" runat="server" AutoPostBack="True" Checked="true"
                                            CssClass="tableTitle" GroupName="groupECDOB" Text="" />
                                        <asp:TextBox ID="txtDOByear" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                            MaxLength="4" Width="50px"></asp:TextBox>
                                        <asp:Label ID="lblECDOBReportText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;
                                        <asp:Label ID="lblECDOBOr2Text" runat="server" CssClass="tableTitle"></asp:Label>
                                        <asp:Image ID="imgECDOByearerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 400px" valign="top">
                                        <asp:RadioButton ID="rbECDOBtravel" runat="server" AutoPostBack="True" Checked="true"
                                            CssClass="tableTitle" GroupName="groupECDOB" Text="" />
                                        <asp:TextBox ID="txtDOBtravel" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                            MaxLength="10" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                            onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                            onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                        <asp:Label ID="lblECDOBTravelText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;
                                        <asp:Label ID="lblECDOBOr3Text" runat="server" CssClass="tableTitle"></asp:Label>
                                        <asp:Image ID="imgECDOBTravelerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 350px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:RadioButton ID="rbECDOA" runat="server" AutoPostBack="True" CssClass="tableTitle"
                                                        GroupName="groupECDOB" />&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="lblECDOAText" runat="server" CssClass="tableTitle"></asp:Label>
                                                    <asp:TextBox ID="txtECDOAAge" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                                        MaxLength="3" Width="30px"></asp:TextBox>
                                                    <asp:Image ID="imgECAgeerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblECDOAOnText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtECDOADayEn" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                                        MaxLength="2" Width="20px"></asp:TextBox>
                                                    <asp:TextBox ID="txtECDOAYearChi" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                                        MaxLength="4" Width="36px"></asp:TextBox>
                                                    <asp:Label ID="lblECDOAYearChiText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                                <td align="center">
                                                    &nbsp;<asp:DropDownList ID="ddlECDOAMonth" runat="server" Enabled="True">
                                                    </asp:DropDownList><asp:Label ID="lblECDOAMonthChiText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtECDOAYearEn" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                                        MaxLength="4" Width="36px"></asp:TextBox>
                                                    <asp:TextBox ID="txtECDOADayChi" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                                        MaxLength="2" Width="20px"></asp:TextBox>
                                                    <asp:Label ID="lblECDOADayChiText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Image ID="imgECDORerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                </td>
                                                <td align="center">
                                                </td>
                                                <td align="center">
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblECDOADayEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Label ID="lblECDOAMonthEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                                <td align="center">
                                                    <asp:Label ID="lblECDOAYearEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
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
    <cc1:FilteredTextBoxExtender ID="filtereditECSerialNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtECSerialNo">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence1" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtECRefence1">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence2" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECRefence2">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence3" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECRefence3">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence4" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtECRefence4">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDateDay" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDateDay">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDateYear" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDateYear">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOBDate" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOBDate" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOByear" runat="server" FilterType="Numbers"
        TargetControlID="txtDOByear">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOBtravel" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOBtravel" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOAAge" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOAAge">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOADayEn" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOADayEn">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOAYearChi" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOAYearChi">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOAYearEn" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOAYearEn">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOADayChi" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOADayChi">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel runat="server" ID="pnlNew">
    <table>
        <tr>
            <td valign="top" style="width: 220px; height: 25px;">
                <asp:Label ID="lblNewHKICText" runat="server" Height="25px"></asp:Label></td>
            <td style="width: 470px;">
                <asp:Label ID="lblNewHKIC" runat="server" Visible="False" CssClass="tableText" /> 
                <asp:TextBox ID="txtNewHKIC" runat="server" MaxLength="11"></asp:TextBox><asp:Image
                    ID="imgNewRegNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
            </td>
        </tr>
        <tr>
            <td style="width: 220px; height: 25px;" valign="top">
                <asp:Label ID="lblNewSerialNoText" runat="server" Height="25px"></asp:Label></td>
            <td valign="top" style="width: 470px">
                <asp:TextBox ID="txtNewSerialNo" runat="server" MaxLength="8" onChange="convertToUpper(this);"
                    Width="100px"></asp:TextBox>
                <asp:CheckBox ID="cboNewECSerialNoNotProvided" runat="server" Text="<%$ Resources:Text, NotProvided %>"
                    CssClass="tableText" AutoPostBack="True" OnCheckedChanged="cboNewECSerialNoNotProvided_CheckedChanged" />
                <asp:Image ID="imgSerialNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td><asp:Label ID="lblECSerialNoHints" runat="server" CssClass="tableTitle"></asp:Label></td>       
        </tr>
        <tr>
            <td style="width: 220px; height: 25px;" valign="top">
                <asp:Label ID="lblNewRefNoText" runat="server" Height="25px"></asp:Label></td>           
            <td style="width: 470px;">
                <asp:TextBox ID="txtNewRefNo1" runat="server" MaxLength="4" onChange="convertToUpper(this);"
                    Width="60px"></asp:TextBox>
                <asp:Label ID="lblNewRefSep1" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                <asp:TextBox ID="txtNewRefNo2" runat="server" MaxLength="7" onChange="convertToUpper(this);"
                    Width="60px"></asp:TextBox>
                <asp:Label ID="lblNewRefSep2" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                <asp:TextBox ID="txtNewRefNo3" runat="server" MaxLength="2" onChange="convertToUpper(this);"
                    Width="20px"></asp:TextBox>
                <asp:Label ID="lblNewRefSep3" runat="server" CssClass="tableTitle" Text="("></asp:Label>
                <asp:TextBox ID="txtNewRefNo4" runat="server" MaxLength="1" onChange="convertToUpper(this);"
                    Width="12px"></asp:TextBox>
                <asp:Label ID="lblNewRefSep4" runat="server" CssClass="tableTitle" Text=")"></asp:Label>
                <asp:TextBox ID="txtNewRefNoFree" runat="server" MaxLength="40" Width="250px" onChange="convertToUpper(this);"></asp:TextBox>
                <asp:ImageButton ID="ibtnNewRefNoOtherFormat" runat="server" ImageUrl="<%$ Resources:ImageUrl, OtherFormatsBtn %>"
                    AlternateText="<%$ Resources:AlternateText, OtherFormatsBtn %>" OnClick="ibtnNewRefNoOtherFormat_Click" />
                <asp:ImageButton ID="ibtnNewRefMoSpecificFormat" runat="server" ImageUrl="<%$ Resources:ImageUrl, SpecificFormatBtn %>"
                    AlternateText="<%$ Resources:AlternateText, SpecificFormatBtn %>" OnClick="ibtnNewRefMoSpecificFormat_Click" />
                <asp:Image ID="imgRefNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
            </td>
            <td valign="top">
                            <asp:Label ID="lblECReferenceHints" runat="server" CssClass="tableTitle"></asp:Label></td>         
        </tr>
        <tr>
            <td style="width: 220px; height: 25px;" valign="top">
                <asp:Label ID="lblNewDOIText" runat="server" Height="25px"></asp:Label></td>         
            <td style="width: 470px">
                <asp:TextBox ID="txtNewDOIDay" runat="server" MaxLength="2" Width="20px"></asp:TextBox>
                <asp:DropDownList ID="ddlNewDOIMonth" runat="server">
                </asp:DropDownList>
                <asp:TextBox ID="txtNewDOIYear" runat="server" MaxLength="4" Width="36px"></asp:TextBox>
                <asp:Image ID="imgNewDOIErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>           
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
            <td style="width: 220px; height: 25px;" valign="top">
                <asp:Label ID="lblNewCNameText" runat="server" Height="25px"></asp:Label></td>           
            <td style="width: 470px">
                <asp:TextBox ID="txtNewCName" runat="server" MaxLength="12" CssClass="TextBoxChineseName"></asp:TextBox>
                <asp:Image ID="imgNewCNameErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
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
                <asp:Label ID="lblDOD" runat="server" Visible="False" CssClass="tableText" /> 
                <asp:Image ID="imgDOD" runat="server" AlternateText="<%$ Resources:AlternateText, DeathRecord %>" 
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>" />
            </td>          
        </tr> 
        <tr>
            <td style="width: 220px; height: 25px;" valign="top">
                <asp:Label ID="lblNewDOBText" runat="server" Text="<%$ Resources:Text, DOB %>" Height="25px"></asp:Label></td>           
            <td style="width: 470px">
                <table>
                    <tr>
                        <td style="width: 350px" valign="top" align="left">
                            <asp:RadioButton ID="rbNewECDOBDate" runat="server" AutoPostBack="True" Checked="true"
                                CssClass="tableTitle" GroupName="groupNewECDOB" Text="" />
                            <asp:TextBox ID="txtNewDOBDate" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Label ID="lblNewECDOBDateText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;
                            <asp:Label ID="lblNewECDOBOr1Text" runat="server" CssClass="tableTitle"></asp:Label>
                            <asp:Image ID="imgNewECDOBerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                    </tr>
                    <tr>
                        <td style="width: 350px" valign="top">
                            <asp:RadioButton ID="rbNewECDOByear" runat="server" AutoPostBack="True" Checked="true"
                                CssClass="tableTitle" GroupName="groupNewECDOB" Text="" />
                            <asp:TextBox ID="txtNewDOByear" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                MaxLength="4" Width="50px"></asp:TextBox>
                            <asp:Label ID="lblNewECDOBReportText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;
                            <asp:Label ID="lblNewECDOBOr2Text" runat="server" CssClass="tableTitle"></asp:Label>
                            <asp:Image ID="imgNewECDOByearerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                    </tr>
                    <tr>
                        <td style="width: 400px" valign="top">
                            <asp:RadioButton ID="rbNewECDOBtravel" runat="server" AutoPostBack="True" Checked="true"
                                CssClass="tableTitle" GroupName="groupNewECDOB" Text="" />
                            <asp:TextBox ID="txtNewDOBtravel" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                MaxLength="10" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Label ID="lblNewECDOBTravelText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;
                            <asp:Label ID="lblNewECDOBOr3Text" runat="server" CssClass="tableTitle"></asp:Label>
                            <asp:Image ID="imgNewECDOBTravelerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                    </tr>
                    <tr>
                        <td style="width: 350px" valign="top">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <asp:RadioButton ID="rbNewECDOA" runat="server" AutoPostBack="True" CssClass="tableTitle"
                                            GroupName="groupNewECDOB" />&nbsp;</td>
                                    <td align="left">
                                        <asp:Label ID="lblNewECDOAText" runat="server" CssClass="tableTitle"></asp:Label>
                                        <asp:TextBox ID="txtNewECDOAAge" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                            MaxLength="3" Width="30px"></asp:TextBox>
                                        <asp:Image ID="imgNewECAgeerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblNewECDOAOnText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewECDOADayEn" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                            MaxLength="2" Width="20px"></asp:TextBox>
                                        <asp:TextBox ID="txtNewECDOAYearChi" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                            MaxLength="4" Width="36px"></asp:TextBox>
                                        <asp:Label ID="lblNewECDOAYearChiText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td align="center">
                                        &nbsp;<asp:DropDownList ID="ddlNewECDOAMonth" runat="server" Enabled="True">
                                        </asp:DropDownList><asp:Label ID="lblNewECDOAMonthChiText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewECDOAYearEn" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                            MaxLength="4" Width="36px"></asp:TextBox>
                                        <asp:TextBox ID="txtNewECDOADayChi" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                            MaxLength="2" Width="20px"></asp:TextBox>
                                        <asp:Label ID="lblNewECDOADayChiText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td align="center">
                                        <asp:Image ID="imgNewECDORerror" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                    <td align="center">
                                    </td>
                                    <td align="center">
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblNewECDOADayEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="lblNewECDOAMonthEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td align="center">
                                        <asp:Label ID="lblNewECDOAYearEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                                </tr>                                        
                            </table>
                        </td>
                    </tr>            
                </table>
            </td>            
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filterNewEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewSurname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewGivenName" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECSerialNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtNewSerialNo">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECRefence1" runat="server" FilterType="Custom, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtNewRefNo1">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECRefence2" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewRefNo2">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECRefence3" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewRefNo3">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECRefence4" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtNewRefNo4">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECDateDay" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOIDay">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECDateYear" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOIYear">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewDOBDate" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOBDate" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewDOByear" runat="server" FilterType="Numbers"
        TargetControlID="txtNewDOByear">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewDOBtravel" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtNewDOBtravel" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECDOAAge" runat="server" FilterType="Numbers"
        TargetControlID="txtNewECDOAAge">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECDOADayEn" runat="server" FilterType="Numbers"
        TargetControlID="txtNewECDOADayEn">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECDOAYearChi" runat="server" FilterType="Numbers"
        TargetControlID="txtNewECDOAYearChi">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECDOAYearEn" runat="server" FilterType="Numbers"
        TargetControlID="txtNewECDOAYearEn">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filterNewECDOADayChi" runat="server" FilterType="Numbers"
        TargetControlID="txtNewECDOADayChi">
    </cc1:FilteredTextBoxExtender>    
</asp:Panel>
