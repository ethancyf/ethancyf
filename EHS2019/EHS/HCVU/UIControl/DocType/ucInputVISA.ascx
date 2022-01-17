<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputVISA.ascx.vb"
    Inherits="HCVU.ucInputVISA" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel runat="server" ID="pnlModify">
    <table>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td valign="top" style="width: 220px">
                        </td>
                        <td valign="top" style="width: 200px">
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
                            <asp:Label ID="lblDocumentTypeOriginalText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblDocumentTypeOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblVISANoOriginalText" runat="server" Text="<%$ Resources:Text, VisaRefNo %>"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblVISANoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtVISANo1" runat="server" MaxLength="4" onChange="convertToUpper(this);"
                                Width="60px" Enabled="False"></asp:TextBox>
                            <asp:Label ID="lblVISANoSymbol1" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                            <asp:TextBox ID="txtVISANo2" runat="server" MaxLength="7" onChange="convertToUpper(this);"
                                Width="50px" Enabled="False"></asp:TextBox>
                            <asp:Label ID="lblVISANoSymbol2" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                            <asp:TextBox ID="txtVISANo3" runat="server" MaxLength="2" onChange="convertToUpper(this);"
                                Width="20px" Enabled="False"></asp:TextBox>
                            <asp:Label ID="lblVISANoSymbol3" runat="server" CssClass="tableTitle" Text="("></asp:Label>
                            <asp:TextBox ID="txtVISANo4" runat="server" MaxLength="1" onChange="convertToUpper(this);"
                                Width="12px" Enabled="False"></asp:TextBox>
                            <asp:Label ID="lblVISANoSymbol4" runat="server" CssClass="tableTitle" Text=")"></asp:Label>
                            <asp:Label ID="lblVISANo" runat="server" Visible="False" CssClass="tableText"></asp:Label>
                            <asp:Image ID="imgVISANo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblPassportNoOriginalText" runat="server" Text="<%$ Resources:Text, TravelDocNo %>"></asp:Label></td>
                        <td style="height: 23px">
                            <asp:Label ID="lblPassportNoOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtPassportNo" runat="server" MaxLength="20" onChange="convertToUpper(this);"
                                Width="200px"></asp:TextBox>
                            <asp:Image ID="imgPassportNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblNameOrignialText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                        <td style="height: 23px">
                            <asp:Label ID="lblNameOrignial" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>
                            &nbsp;<asp:Image ID="imgEName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
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
                            <asp:Label ID="lblGenderOriginalText" runat="server" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                        </td>
                        <td style="height: 25px">
                            <asp:Label ID="lblGenderOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOBOriginalText" runat="server" Text="<%$ Resources:Text, DOB %>"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblDOBOriginal" runat="server" CssClass="tableText"></asp:Label></td>
                        <td style="width: 350px">
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False"
                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filtereditVISANo1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
        TargetControlID="txtVISANo1">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditVISANo2" runat="server" FilterType="Numbers"
        TargetControlID="txtVISANo2">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditVISANo3" runat="server" FilterType="Numbers"
        TargetControlID="txtVISANo3">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditVISANo4" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
        TargetControlID="txtVISANo4">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameSurname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtENameFirstname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filtereditPassportNo" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
        TargetControlID="txtPassportNo">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel runat="server" ID="pnlNew">
    <table>        
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewVisaRefNoText" runat="server" Text="<%$ Resources:Text, VisaRefNo %>"></asp:Label></td>          
            <td style="width: 470px">
                <asp:Label ID="lblNewVisaNo" runat="server" Visible="False" CssClass="tableText" /> 
                <asp:TextBox ID="txtNewVISANo1" runat="server" MaxLength="4" onChange="convertToUpper(this);"
                    Width="60px" ></asp:TextBox>
                <asp:Label ID="lblNewVISANoSymbol1" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                <asp:TextBox ID="txtNewVISANo2" runat="server" MaxLength="7" onChange="convertToUpper(this);"
                    Width="50px" ></asp:TextBox>
                <asp:Label ID="lblNewVISANoSymbol2" runat="server" CssClass="tableTitle" Text="-"></asp:Label>
                <asp:TextBox ID="txtNewVISANo3" runat="server" MaxLength="2" onChange="convertToUpper(this);"
                    Width="20px" ></asp:TextBox>
                <asp:Label ID="lblNewVISANoSymbol3" runat="server" CssClass="tableTitle" Text="("></asp:Label>
                <asp:TextBox ID="txtNewVISANo4" runat="server" MaxLength="1" onChange="convertToUpper(this);"
                    Width="12px" ></asp:TextBox>
                <asp:Label ID="lblNewVISANoSymbol4" runat="server" CssClass="tableTitle" Text=")"></asp:Label>                
                <asp:Image ID="imgNewVISANoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            
        </tr>
        <tr>
            <td valign="top" style="width: 220px; height: 25px">
                <asp:Label ID="lblNewPassportNoText" runat="server" Text="<%$ Resources:Text, TravelDocNo %>" Height="25px"></asp:Label></td>           
            <td style="width: 470px">
                <asp:TextBox ID="txtNewPassportNo" runat="server" MaxLength="20" onChange="convertToUpper(this);"
                    Width="200px"></asp:TextBox>
                <asp:Image ID="imgNewPassportNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
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
    </table>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewVisaNo1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
        TargetControlID="txtNewVISANo1">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewVisaNo2" runat="server" FilterType="Numbers"
        TargetControlID="txtNewVISANo2">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewVisaNo3" runat="server" FilterType="Numbers"
        TargetControlID="txtNewVISANo3">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewVisaNo4" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
        TargetControlID="txtNewVISANo4">
    </cc1:FilteredTextBoxExtender>
     <cc1:FilteredTextBoxExtender ID="FilteredTextNewENameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewSurname" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewENameGivenname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
        TargetControlID="txtNewGivenName" ValidChars="-' ">
    </cc1:FilteredTextBoxExtender>
   <cc1:FilteredTextBoxExtender ID="FilteredTextNewDOB" runat="server" FilterType="Numbers, Custom"
        TargetControlID="txtNewDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="FilteredTextNewPassportNo" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
        TargetControlID="txtNewPassportNo">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
