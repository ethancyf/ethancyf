<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputEC.ascx.vb"
    Inherits="PrefillConsent.ucInputEC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel ID="panEnterDetail" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr id="trTransactionNo_M" runat="server">
            <td style="width: 200px" valign="top">
                <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                    Width="150px"></asp:Label></td>
            <td style="width: 350px" valign="top">
                <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
            <td></td>
        </tr>
        <tr runat="server" id="trReferenceNo_M">
            <td style="height: 19px; width: 200px;" valign="top">
                <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 350px;" valign="top">
                <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
            <td></td>
        </tr>
        <tr id="trECHKIDModication" runat="server">
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblECHKIDModificationText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px">
                <asp:Label ID="lblECHKICModification" runat="server"  CssClass="tableText"></asp:Label></td>
            <td></td>        
        </tr>  
        <tr>
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblECSerialNo" runat="server" CssClass="tableTitle"
                    Height="28px"  Width="150px"></asp:Label></td>
            <td style="width: 350px" valign="top">
                <asp:TextBox ID="txtECSerialNo" runat="server" MaxLength="8" onChange="convertToUpper(this);"
                    Width="100px"></asp:TextBox>
                <asp:Image ID="imgECSerialNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top">
                <asp:Label ID="lblECSerialNoHints" runat="server" CssClass="tableTitle"
                    ></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblECReference" runat="server" CssClass="tableTitle"
                    Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px">
                <asp:TextBox ID="txtECRefence1" runat="server" MaxLength="4" onChange="convertToUpper(this);"
                    Width="60px"></asp:TextBox>
                <asp:Label ID="lblReferenceSep1" runat="server" CssClass="tableTitle"
                    Text="-"></asp:Label>
                <asp:TextBox ID="txtECRefence2" runat="server" MaxLength="7" onChange="convertToUpper(this);"
                    Width="60px"></asp:TextBox>
                <asp:Label ID="lblReferenceSep2" runat="server" CssClass="tableTitle"
                    Text="-"></asp:Label>
                <asp:TextBox ID="txtECRefence3" runat="server" MaxLength="2" onChange="convertToUpper(this);"
                    Width="20px"></asp:TextBox>
                <asp:Label ID="lblReferenceSep3" runat="server" CssClass="tableTitle"
                    Text="("></asp:Label>
                <asp:TextBox ID="txtECRefence4" runat="server" MaxLength="1" onChange="convertToUpper(this);"
                    Width="12px"></asp:TextBox>
                <asp:Label ID="lblReferenceSep4" runat="server" CssClass="tableTitle"
                    Text=")"></asp:Label><asp:Image ID="imgECRefence" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                        ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top">
                <asp:Label ID="lblECReferenceHints" runat="server" CssClass="tableTitle"
                    ></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblECDate" runat="server" CssClass="tableTitle" Height="28px"
                     Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px">
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
            <td style="height: 19px; width: 200px;" valign="top">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Height="28px"
                     Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 350px;" valign="top">
                <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                    Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"
                        ></asp:Label>
                <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40"
                    onChange="convertToUpper(this);"></asp:TextBox>&nbsp;<asp:Image ID="imgEName"
                        runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageAlign="Top"
                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td></td>            
        </tr>
        <tr>
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblCName" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px">
                <asp:TextBox ID="txtCName" runat="server" MaxLength="6"></asp:TextBox></td>
            <td></td>    
        </tr>
        <tr>
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px">
                <asp:RadioButtonList ID="rbECGender" runat="server" CssClass="tableText"
                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>&nbsp;
                <asp:Image ID="imgECGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td></td>        
        </tr>
        <tr id="trECHKIDCreation" runat="server">
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblECHKID" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px">
                <asp:TextBox ID="txtECHKIC" runat="server" AutoCompleteType="Disabled"
                    MaxLength="11" onChange="formatHKID(this);" Width="85px" Enabled="False"></asp:TextBox>
            </td>
            <td></td>        
        </tr>
    </table>
    <table id="tbDOBCreate" runat="server"  cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="width:200px">
                <asp:Label ID="lblECDOB" runat="server" CssClass="tableTitle" Height="28px"
                     Width="150px"></asp:Label></td>
            <td valign="top">
                 <asp:TextBox ID="txtECDOB" runat="server" MaxLength="10" Enabled="False" Width="75px"></asp:TextBox><asp:Panel
                    ID="panECDOA" runat="server">
                    <asp:Label ID="lblAge" runat="server" CssClass="tableTitle" ></asp:Label>
                    <asp:TextBox ID="txtECAge" runat="server" MaxLength="3" Width="40px"
                        Enabled="False"></asp:TextBox>
                    <asp:Label ID="lblRegisterOn" runat="server" CssClass="tableTitle"></asp:Label>
                    <asp:TextBox ID="txtECDOAge" runat="server" MaxLength="10"
                        Enabled="False"></asp:TextBox></asp:Panel>
            </td>
        </tr>
    </table>
    <table id="tbDOBModify" runat="server" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="width: 200px">
                <asp:Label ID="lblECDOBText_M" runat="server" CssClass="tableTitle" Height="28px" Width="150px" ></asp:Label></td>
            <td style="width: 350px" valign="top" align="left">
                <asp:RadioButton ID="rbECDOBDate_M" runat="server" AutoPostBack="True" Checked="true" CssClass="tableTitle"
                    GroupName="groupECDOB" Text="" />
                <asp:TextBox ID="txtDOBDate_M" runat="server" AutoCompleteType="Disabled" Enabled="true"
                    MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Label ID="lblECDOBDateText_M" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;
                <asp:Label ID="lblECDOBOr1Text_M" runat="server" CssClass="tableTitle"></asp:Label>
                <asp:Image ID="imgECDOBerror_M" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>            
        </tr>
        <tr>
            <td valign="top" style="width: 200px;Height:28px"></td>
            <td style="width: 350px" valign="top">
                <asp:RadioButton ID="rbECDOByear_M" runat="server" AutoPostBack="True" Checked="true" CssClass="tableTitle"
                    GroupName="groupECDOB" Text="" />
                <asp:TextBox ID="txtDOByear_M" runat="server" AutoCompleteType="Disabled" Enabled="true"
                    MaxLength="4" Width="50px"></asp:TextBox>
                <asp:Label ID="lblECDOBReportText_M" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;    
                <asp:Label ID="lblECDOBOr2Text_M" runat="server" CssClass="tableTitle"></asp:Label>
                <asp:Image ID="imgECDOByearerror_M" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>            
        </tr>
        <tr>
            <td valign="top" style="width: 200px;Height:28px"></td>
            <td style="width: 400px" valign="top">
                <asp:RadioButton ID="rbECDOBtravel_M" runat="server" AutoPostBack="True" Checked="true" CssClass="tableTitle"
                    GroupName="groupECDOB" Text="" />
                <asp:TextBox ID="txtDOBtravel_M" runat="server" AutoCompleteType="Disabled" Enabled="true"
                    MaxLength="10" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Label ID="lblECDOBTravelText_M" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;    
                <asp:Label ID="lblECDOBOr3Text_M" runat="server" CssClass="tableTitle"></asp:Label>
                <asp:Image ID="imgECDOBTravelerror_M" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>            
        </tr>
        <tr>
            <td valign="top" style="Height:28px; width: 200px;"></td>
            <td style="width: 350px" valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <asp:RadioButton ID="rbECDOA_M" runat="server" AutoPostBack="True" CssClass="tableTitle"
                                GroupName="groupECDOB" />&nbsp;</td>
                        <td align="left">
                            <asp:Label ID="lblECDOAText_M" runat="server" CssClass="tableTitle"></asp:Label> 
                            <asp:TextBox ID="txtECDOAAge_M" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="3" Width="30px"></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblECDOAOnText_M" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                        <td align="center">
                            <asp:TextBox ID="txtECDOADayEn_M" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="2" Width="20px"></asp:TextBox>
                            <asp:TextBox ID="txtECDOAYearChi_M" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="4" Width="36px"></asp:TextBox>
                            <asp:Label ID="lblECDOAYearChiText_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            &nbsp;<asp:DropDownList ID="ddlECDOAMonth_M" runat="server" Enabled="True">
                            </asp:DropDownList><asp:Label ID="lblECDOAMonthChiText_M" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                        <td align="center">
                            <asp:TextBox ID="txtECDOAYearEn_M" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="4" Width="36px"></asp:TextBox>
                            <asp:TextBox ID="txtECDOADayChi_M" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="2" Width="20px"></asp:TextBox>
                            <asp:Label ID="lblECDOADayChiText_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            <asp:Image ID="imgECDOAerror_M" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
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
                            <asp:Label ID="lblECDOADayEnText_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            <asp:Label ID="lblECDOAMonthEnText_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            <asp:Label ID="lblECDOAYearEnText_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>    
    </table> 

    <asp:Panel ID="panECDOBType" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top" style="width: 200px;Height:28px">
                    <asp:Label ID="lblDOBType" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
                <td>
                <asp:RadioButtonList ID="rbDOBType" runat="server" CssClass="tableText"
                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="D">DOB</asp:ListItem>
                    <asp:ListItem Value="Y">Year of Birth reported</asp:ListItem>
                    <asp:ListItem Value="T">DOB reported on travel document</asp:ListItem>
                </asp:RadioButtonList><asp:Image ID="imgECDOBType" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />

                </td>
            </tr>
        </table>
    </asp:Panel>
    

</asp:Panel>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server"
        FilterType="UppercaseLetters, LowercaseLetters, Custom" TargetControlID="txtENameSurname"
        ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server"
        FilterType="UppercaseLetters, LowercaseLetters, Custom" TargetControlID="txtENameFirstname"
        ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECSerialNo" runat="server"
        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtECSerialNo">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence1" runat="server"
        FilterType="Custom, UppercaseLetters, LowercaseLetters" TargetControlID="txtECRefence1">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence2" runat="server"
        FilterType="Custom, Numbers" TargetControlID="txtECRefence2">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence3" runat="server"
        FilterType="Custom, Numbers" TargetControlID="txtECRefence3">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECRefence4" runat="server"
        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtECRefence4">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDateDay" runat="server"
        FilterType="Custom, Numbers" TargetControlID="txtECDateDay">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDateYear" runat="server"
        FilterType="Custom, Numbers" TargetControlID="txtECDateYear">
    </cc1:FilteredTextBoxExtender>
    
    <cc1:FilteredTextBoxExtender ID="filtereditDOBDate_M" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOBDate_M" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOByear_M" runat="server" FilterType="Numbers"
        TargetControlID="txtDOByear_M">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOBtravel_M" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtDOBtravel_M" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOAAge_M" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOAAge_M">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOADayEn_M" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOADayEn_M">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOAYearChi_M" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOAYearChi_M">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOAYearEn_M" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOAYearEn_M">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditECDOADayChi_M" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOADayChi_M">
    </cc1:FilteredTextBoxExtender>
