<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVISA.ascx.vb" Inherits="HCSP.ucInputVISA" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel ID="PanVISA" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr runat="server" id="trReferenceNo_M">
            <td style="width: 200px" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="width: 350px;" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>    
        <tr>
            <td style="width: 200px" valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblVISANoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px" valign="top" class="tableCellStyleLite">
                <asp:TextBox ID="txtVISANo1" runat="server" MaxLength="4" onChange="convertToUpper(this);" Width="60px" />
                <asp:Label ID="lblVISANoSymbol1" runat="server" CssClass="tableTitle" Text="-" />
                <asp:TextBox ID="txtVISANo2" runat="server" MaxLength="7" onChange="convertToUpper(this);" Width="50px" />
                <asp:Label ID="lblVISANoSymbol2" runat="server" CssClass="tableTitle" Text="-" />
                <asp:TextBox ID="txtVISANo3" runat="server" MaxLength="2" onChange="convertToUpper(this);" Width="20px" />
                <asp:Label ID="lblVISANoSymbol3" runat="server" CssClass="tableTitle" Text="(" />
                <asp:TextBox ID="txtVISANo4" runat="server" MaxLength="1" onChange="convertToUpper(this);" Width="12px" />
                <asp:Label ID="lblVISANoSymbol4" runat="server" CssClass="tableTitle" Text=")" />                     
                <asp:Label ID="lblVISANo" runat="server"  Visible="False" CssClass="tableText" />     
                <asp:Image ID="imgVISANo" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top" Visible="false" />
            </td>
        </tr>
        <tr>
             <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblPassportNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px" valign="top" class="tableCellStyleLite">
                <asp:TextBox ID="txtPassportNo" runat="server" MaxLength="20" onChange="convertToUpper(this);" Width="200px"></asp:TextBox> 
                <asp:Image ID="imgPassportNo" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top"   Visible="false" /></td>
        </tr>       
        <tr>
            <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px" valign="top" class="tableCellStyleLite">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                Width="105px"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label></td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox></td>
                        <td>
                            &nbsp;<asp:Image ID="imgEName" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top"   /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSurname" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td>
                        </td>
                        <td>
                            &nbsp;<asp:Label ID="lblGivenName" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td>
                        </td>
                    </tr>
                </table>
                </td>
        </tr>
        <tr>
            <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px" class="tableCellStyleLite">
                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>&nbsp;
                <asp:Image ID="imgGender" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top"   Visible="false" /></td>
        </tr>
        <tr>
            <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 350px" class="tableCellStyleLite">
                <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>                
                <asp:Image ID="imgDOBDate" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top"   Visible="false" /></td>
        </tr>
        <tr id="trTransactionNo_M" runat="server">
            <td valign="top" class="tableCellStyleLite">
                <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="width: 350px;" valign="top" class="tableCellStyleLite">
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
<cc1:FilteredTextBoxExtender ID="filtereditPassportNo" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
    TargetControlID="txtPassportNo" >
</cc1:FilteredTextBoxExtender>