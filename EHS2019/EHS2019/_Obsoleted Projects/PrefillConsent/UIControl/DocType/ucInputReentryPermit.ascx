<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputReentryPermit.ascx.vb" Inherits="PrefillConsent.ucInputReentryPermit" %>
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
                        <td style="height: 19px;width: 160px;" valign="top">
                            <asp:Label ID="lblReferenceNoText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 320px;" valign="top">
                            <asp:Label ID="lblReferenceNo" runat="server"></asp:Label></td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 19px;width: 160px;" valign="top">
                            <asp:Label ID="lblTravelDocNoText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 320px;" valign="top">
                            <asp:TextBox ID="txtTravelDocNo" runat="server" MaxLength="9" Width="75px" onChange="convertToUpper(this);" ></asp:TextBox>
                            <asp:Label ID="lblTravelDocNo" runat="server"  Visible="False" CssClass="tableText"></asp:Label> 
                            <asp:Image ID="imgTravelDocNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                            <asp:Label ID="lblTDNoTip" runat="server" CssClass="tableTip"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="height: 19px;width: 160px;" valign="top">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 320px;" valign="top">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server"
                                    CssClass="largeText"></asp:Label>
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40"
                                onChange="convertToUpper(this);"></asp:TextBox>&nbsp;<asp:Image ID="imgENameError"
                                    runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageAlign="Top"
                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                         <td valign="top"><asp:Label ID="lblENameTips" runat="server" CssClass="tableTip"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 160px"></td>
                        <td valign="top" style="padding-bottom: 5px; width: 320px">
                            <asp:Label ID="lblSurname" runat="server" CssClass="tableTip" Width="118px"></asp:Label>
                            <asp:Label ID="lblGivenName" runat="server" CssClass="tableTip"></asp:Label>
                        </td>
                        <td valign="top"></td>
                    </tr>
                    <tr>
                        <td style="height: 19px;width: 160px;" valign="top">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 320px">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText"
                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                         <td></td>       
                    </tr>
                    <tr>
                        <td style="height: 19px;width: 160px;" valign="top">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 320px;">
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        <td valign="top">
                            <asp:Label ID="lblDOBTip" runat="server" CssClass="tableTip"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="height: 19px;width: 160px;" valign="top">
                            <asp:Label ID="lblDOI" runat="server" CssClass="tableTitle" Height="28px"
                              Width="150px"></asp:Label></td>
                       <td valign="top" style="width: 320px">  
                            <asp:TextBox ID="txtDOI" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOIError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>             
                       <td valign="top">
                            <asp:Label ID="lblDOITip" runat="server" CssClass="tableTip"></asp:Label></td>                             
                    </tr>
                    <tr ID="trTransactionNo" runat="server">
                        <td style="height: 19px;width: 160px;" valign="top">
                            <asp:Label ID="lblTransactionNoText" runat="server" CssClass="tableTitle" Height="28px"
                                Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 320px;" valign="top">
                            <asp:Label ID="lblTransactionNo" runat="server"></asp:Label></td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server"
    FilterType="UppercaseLetters, LowercaseLetters, Custom" TargetControlID="txtENameSurname"
    ValidChars="-' ">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server"
    FilterType="UppercaseLetters, LowercaseLetters, Custom" TargetControlID="txtENameFirstname"
    ValidChars="-' ">
</cc1:FilteredTextBoxExtender>  
<cc1:FilteredTextBoxExtender ID="filtereditTravelDocNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
    TargetControlID="txtTravelDocNo" >
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server"
    FilterType="Custom, Numbers" TargetControlID="txtDOB"
    ValidChars="-">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditIssueDate" runat="server"
    FilterType="Custom, Numbers" TargetControlID="txtDOI"
    ValidChars="-">
</cc1:FilteredTextBoxExtender>