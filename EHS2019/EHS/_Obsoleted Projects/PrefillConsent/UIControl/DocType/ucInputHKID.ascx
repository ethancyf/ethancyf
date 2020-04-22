<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKID.ascx.vb"
    Inherits="PrefillConsent.ucInputHKID" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel ID="panEnterDetailCreation" runat="server">
    <asp:Panel ID="panl1" runat="server">
        <table cellpadding="0" cellspacing="0" style="width: 772px">
            <tbody>
                <tr>
                    <td id="hkid_cell" runat="server" style="background-image: url(Images/HKID/HKID_Empty.jpg);
                        background-repeat: no-repeat; height: 245px" valign="top">
                        <table cellpadding="0" cellspacing="0" style="width: 465px; height: 60px">
                            <tbody>
                                <tr>
                                    <td valign="bottom">
                                        &nbsp;&nbsp;<asp:Label ID="lblCName" runat="server" CssClass="largeChiText" Font-Names="HA_MingLiu"></asp:Label></td>
                                </tr>
                            </tbody>
                        </table>
                        <table cellpadding="0" cellspacing="0" style="width: 465px; height: 15px">
                            <tbody>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 345px" valign="top">
                                                        &nbsp;&nbsp;
                                                        <asp:TextBox ID="txtENameSurname" runat="server" onChange="convertToUpper(this);"
                                                            Width="105px" Height="15px"></asp:TextBox>
                                                        <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                                                        <asp:TextBox ID="txtENameFirstrname" runat="server" onChange="convertToUpper(this);" Height="15px"></asp:TextBox>
                                                        <asp:Image ID="imgENameError" runat="server" Height="15px"  Visible="False" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        &nbsp;</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table cellpadding="0" cellspacing="0" style="width: 465px">
                            <tbody>
                                <tr>
                                    <td style="width: 150px; height: 15px">
                                    </td>
                                    <td style="width: 401px; height: 15px">
                                        <asp:TextBox ID="txtCCCode1" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                            Width="40px" Height="15px"></asp:TextBox>
                                        <asp:TextBox ID="txtCCCode2" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                            Width="40px" Height="15px"></asp:TextBox>
                                        <asp:TextBox ID="txtCCCode3" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                            Width="40px" Height="15px"></asp:TextBox>&nbsp;
                                        <asp:Image ID="imgCCCodeError" runat="server"  Visible="False" Height="15px" />
                                        <asp:ImageButton ID="btnSearchCCCode" runat="server" Height="15px"  />
                                        <asp:Label ID="lblCCCTail" runat="server" CssClass="tableText" Font-Names="HA_MingLiu" Height="15px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px; height: 15px">
                                    </td>
                                    <td style="width: 401px; height: 15px" valign="bottom">
                                        <asp:TextBox ID="txtCCCode4" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                            Width="40px" Height="15px"></asp:TextBox>
                                        <asp:TextBox ID="txtCCCode5" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                            Width="40px" Height="15px"></asp:TextBox>
                                        <asp:TextBox ID="txtCCCode6" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                            Width="40px" Height="15px"></asp:TextBox>
                                            </td>
                                </tr>
                                <%--<tr style="height: 10px">
                                    <td style="width: 150px; height: 10px">
                                    </td>
                                    <td style="width: 401px; height: 10px">
                                        <asp:Label ID="Label1" runat="server" Height="10px"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="width: 150px; height: 38px">
                                    </td>
                                    <td style="width: 401px; height: 38px" valign="bottom">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 15px">
                                            <tr>
                                                <td style="width: 100px; height: 15px">
                                                    <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Height="15px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                                    <asp:Image ID="imgDOBError" runat="server" Height="15px" ImageAlign="AbsMiddle" />
                                                </td>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 126px; height: 15px" valign="top">
                                                                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="HKIDGenderText" RepeatDirection="Horizontal" AutoPostBack="True" Height="15px">
                                                                    <asp:ListItem Value="F">Female</asp:ListItem>
                                                                    <asp:ListItem Value="M">Male</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                </td>
                                                            <td style="height: 15px" valign="top">
                                                                <asp:Image ID="imgGenderError" runat="server" Height="15px"  Visible="False" />
                                                                </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                     </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px">
                                    </td>
                                    <td style="width: 401px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td style="width: 90px; height: 75px" valign="bottom">
                                                    <asp:TextBox ID="txtHKIDIssueDate" runat="server" MaxLength="8" Width="55px" Height="15px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                                    <asp:Image ID="imgHKIDIssueDateError" runat="server"  Visible="False" Height="15px" /></td>
                                                <td align="center" style="width: 316px; height: 75px" valign="bottom">
                                                    <asp:TextBox ID="txtHKID" runat="server" MaxLength="11" onChange="formatHKID(this);" Width="90px" Height="15px"></asp:TextBox>
                                                    <asp:Image ID="imgHKIDError" runat="server" Height="15px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Panel>
<cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameSurname" ValidChars="-' .">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameFirstrname" ValidChars="-' .">
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
<cc1:FilteredTextBoxExtender ID="filtererditCCcode5" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode5">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode4" runat="server" FilterType="Numbers"
    TargetControlID="txtCCcode4">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode6" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode6">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOB" ValidChars="-">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditHKID" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
    TargetControlID="txtHKID" ValidChars="()">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditHKIDIssueDate" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtHKIDIssueDate" ValidChars="-">
</cc1:FilteredTextBoxExtender>
<asp:Panel ID="panEnterDetailModify" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">     
                    <tr runat="server" id="trReferenceNoModification">
                        <td style="height: 19px;width:200px" valign="top">
                            <asp:Label ID="lblReferenceNoModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 350px;" valign="top">
                            <asp:Label ID="lblReferenceNoModification" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="height: 19px;width:200px" valign="top">
                            <asp:Label ID="lblHKICNoModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 350px;" valign="top">
                            <asp:Label ID="lblHKICNoModification" runat="server" MaxLength="9" Width="100px" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOBModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 350px">
                            <asp:TextBox ID="txtDOBModification" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBModificationError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                    <tr>
                        <td style="height: 19px" valign="top">
                            <asp:Label ID="lblENameModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 350px;" valign="top">
                            <asp:TextBox ID="txtENameSurnameModification" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox><asp:Label ID="lblENameCommaModification" runat="server"
                                    CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstnameModification" runat="server" MaxLength="40"
                                onChange="convertToUpper(this);"></asp:TextBox>&nbsp;<asp:Image ID="imgENameModificationError"
                                    runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageAlign="Top"
                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                    <tr>
                        <td style="height: 19px" valign="top">
                            <asp:Label ID="lblCCCodeModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                ></asp:Label></td>
                        <td style="width: 450px; height: 19px" valign="top">
                            <asp:TextBox ID="txtCCCode1Modification" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode2Modification" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode3Modification" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>&nbsp;                          
                            <asp:TextBox ID="txtCCCode4Modification" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode5Modification" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txtCCCode6Modification" runat="server" AutoCompleteType="Disabled" MaxLength="4"
                                Width="40px"></asp:TextBox>  
                            <asp:Image ID="imgCCCodeModificationError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />                                              
                            <asp:ImageButton ID="btnSearchCCCodeModification" runat="server"  />

                          </td>
                    </tr>
                    <tr>
                        <td style="height: 19px" valign="top">
                            <asp:Label ID="lblCNameModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 350px">
                            <asp:Label ID="lblCNameModification" runat="server" MaxLength="10" Width="75px" CssClass="tableText"></asp:Label></td>
                    </tr>               
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblGenderModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 450px">
                            <asp:RadioButtonList ID="rbGenderModification" runat="server" CssClass="tableText"
                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderModificationError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>      
                    </tr>
                    <tr>
                       <td valign="top">
                            <asp:Label ID="lblDOIModificationText" runat="server" CssClass="tableTitle" Height="28px"
                              Width="150px"></asp:Label></td>
                       <td valign="top" style="width: 350px">  
                            <asp:TextBox ID="txtDOIModification" runat="server" MaxLength="8" Width="55px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOIModificationError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>                                
                    </tr>
                    <tr id="trTransactionNoModification" runat="server">
                        <td style="height: 19px" valign="top">
                            <asp:Label ID="lblTransactionNoModificationText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 350px;" valign="top">
                            <asp:Label ID="lblTransactionNoModification" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel> 
<cc1:FilteredTextBoxExtender ID="filtereditEnameSurnameModification" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameSurnameModification" ValidChars="-' .">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameFirstnameModification" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameFirstnameModification" ValidChars="-' .">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode1Modification" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode1Modification">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode2Modification" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode2Modification">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode3Modification" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode3Modification">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode4Modification" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode4Modification">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode5Modification" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode5Modification">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtererditCCcode6Modification" runat="server" FilterType="Numbers"
    TargetControlID="txtCCCode6Modification">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOBModification" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOBModification" ValidChars="-">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOIModification" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOIModification" ValidChars="-">
</cc1:FilteredTextBoxExtender>
