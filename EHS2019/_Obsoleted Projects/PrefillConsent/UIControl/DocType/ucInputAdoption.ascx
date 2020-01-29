<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputAdoption.ascx.vb" Inherits="PrefillConsent.ucInputAdoption" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Panel ID="PanVISA" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr runat="server" id="trReferenceNo_M">
            <td style="height: 19px ;width: 150px" valign="top">
                <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 330px;" valign="top">
                <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>       
        <tr>
            <td style="width: 150px" valign="top">
                <asp:Label ID="lblEntryNoText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td style="width: 330px" valign="top">
                <asp:TextBox ID="txtPerfixNo" runat="server" MaxLength="7" onChange="convertToUpper(this);"
                    Width="75px"></asp:TextBox>
                <asp:Label ID="lblEntryNoSymbol" runat="server" CssClass="tableTitle"
                    Text="/"></asp:Label>  
               <asp:TextBox ID="txtIdentityNo" runat="server" MaxLength="5" onChange="convertToUpper(this);"
                    Width="40px"></asp:TextBox>
               <asp:Label ID="lblEntryNo" runat="server"  Visible="False" CssClass="tableText"></asp:Label>     
                <asp:Image ID="imgEntryNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top">
                <asp:Label ID="lblEntryNoTip" runat="server" CssClass="tableTip"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 150px">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td style="width: 330px" valign="top">
                <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                    Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>
                &nbsp;<asp:Image ID="imgEName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top"><asp:Label ID="lblENameTips" runat="server" CssClass="tableTip"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" style="width: 150px"></td>
            <td valign="top" style="padding-bottom: 5px; width: 330px">
                <asp:Label ID="lblSurname" runat="server" CssClass="tableTip" Width="118px"></asp:Label>
                <asp:Label ID="lblGivenName" runat="server" CssClass="tableTip"></asp:Label>
            </td>
            <td valign="top"></td>
        </tr>
        <tr>
            <td valign="top" style="width: 150px">
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 330px">
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
            <td valign="top" style="width: 150px">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 330px">
               <table cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:RadioButton id="rbDOB" runat="server" CssClass="tableTitle" GroupName="DOBType" AutoPostBack="True"></asp:RadioButton> 
                                <asp:TextBox id="txtDOB" runat="server" Width="75px" MaxLength="10" onfocus="clickADOPC_DOB();" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                <asp:Image ID="imgDOBError" runat="server" ImageAlign="AbsMiddle" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:RadioButton id="rbDOBInWord" runat="server" Width="90px" CssClass="tableText" GroupName="DOBType" AutoPostBack="True"></asp:RadioButton>
                                <asp:DropDownList id="ddlDOBinWordType" runat="server" Width="120px"></asp:DropDownList>
                                <asp:TextBox id="txtDOBInWord" runat="server" Width="75px" MaxLength="10" onfocus="clickADOPC_DOBInWord();" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                <asp:Image id="imgDOBInWordError" runat="server" ImageAlign="AbsMiddle"></asp:Image>
                            </td>
                        </tr>
                    </tbody>
                 </table>
               </td>
            <td valign="top">
                <asp:Label ID="lblDOBTip" runat="server" CssClass="tableTip"></asp:Label></td>
        </tr>
        <tr id="trTransactionNo_M" runat="server">
            <td style="height: 19px; width: 150px;" valign="top">
                <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 330px;" valign="top">
                <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<cc1:FilteredTextBoxExtender ID="filtereditEntryNo1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
    TargetControlID="txtPerfixNo">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEntryNo2" runat="server" FilterType="Numbers"
    TargetControlID="txtIdentityNo">
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
<cc1:FilteredTextBoxExtender ID="filtereditDOBInWord" runat="server" FilterType="Custom, Numbers"
    TargetControlID="txtDOBInWord" ValidChars="-">
</cc1:FilteredTextBoxExtender>
