<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVISA.ascx.vb" Inherits="PrefillConsent.ucInputVISA" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel ID="PanVISA" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr runat="server" id="trReferenceNo_M">
            <td style="height: 19px ;width: 160px" valign="top">
                <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 320px;" valign="top">
                <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>    
        <tr>
            <td style="width: 160px" valign="top">
                <asp:Label ID="lblVISANoText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td style="width: 320px" valign="top">
                <asp:TextBox ID="txtVISANo1" runat="server" MaxLength="4" onChange="convertToUpper(this);"
                    Width="60px"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol1" runat="server" CssClass="tableTitle"
                    Text="-"></asp:Label>
                <asp:TextBox ID="txtVISANo2" runat="server" MaxLength="7" onChange="convertToUpper(this);"
                    Width="50px"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol2" runat="server" CssClass="tableTitle"
                    Text="-"></asp:Label>
                <asp:TextBox ID="txtVISANo3" runat="server" MaxLength="2" onChange="convertToUpper(this);"
                    Width="20px"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol3" runat="server" CssClass="tableTitle"
                    Text="("></asp:Label>
                <asp:TextBox ID="txtVISANo4" runat="server" MaxLength="1" onChange="convertToUpper(this);"
                    Width="12px"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol4" runat="server" CssClass="tableTitle"
                    Text=")"></asp:Label>                     
                <asp:Label ID="lblVISANo" runat="server"  Visible="False" CssClass="tableText"></asp:Label>     
                <asp:Image ID="imgVISANo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top">
                <asp:Label ID="lblVISANoTip" runat="server" CssClass="tableTip"></asp:Label>
            </td>
        </tr>
        <tr>
             <td valign="top">
                <asp:Label ID="lblPassportNoText" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td style="width: 320px" valign="top">
                <asp:TextBox ID="txtPassportNo" runat="server" MaxLength="20" onChange="convertToUpper(this);" Width="180px"></asp:TextBox> 
                <asp:Image ID="imgPassportNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top"></td>    
        </tr>       
        <tr>
            <td valign="top">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td style="width: 320px" valign="top">
                <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                    Width="105px"></asp:TextBox><asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox>
                &nbsp;<asp:Image ID="imgEName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top"><asp:Label ID="lblENameTips" runat="server" CssClass="tableTip"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top"></td>
            <td valign="top" style="padding-bottom: 5px; width: 320px">
                <asp:Label ID="lblSurname" runat="server" CssClass="tableTip" Width="118px"></asp:Label>
                <asp:Label ID="lblGivenName" runat="server" CssClass="tableTip"></asp:Label>
            </td>
            <td valign="top"></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 320px">
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
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Height="28px" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 320px">
                <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgDOBDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
            <td valign="top">
                <asp:Label ID="lblDOBTip" runat="server" CssClass="tableTip"></asp:Label></td>
        </tr>
        <tr id="trTransactionNo_M" runat="server">
            <td style="height: 19px" valign="top">
                <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle" Height="28px"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 320px;" valign="top">
                <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>

<cc1:FilteredTextBoxExtender ID="filtereditVISANo1" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
    TargetControlID="txtVISANo1" >
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditVISANo2" runat="server" FilterType="Numbers"
    TargetControlID="txtVISANo2" >
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditVISANo3" runat="server" FilterType="Numbers"
    TargetControlID="txtVISANo3" >
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditVISANo4" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
    TargetControlID="txtVISANo4" >
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