<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKBC.ascx.vb"
    Inherits="HCSP.ucInputHKBC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">



</script>

<asp:Panel ID="panInputHKBC" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr runat="server" id="trReferenceNo_M">
                        <td style="width: 200px" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 200px" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblRegistrationNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyleLite">
                            <asp:TextBox ID="txtRegistrationNo" runat="server" MaxLength="11" Width="90px" />
                            <asp:Label ID="lblRegistrationNo" runat="server" Visible="False" CssClass="tableText" />
                            <asp:Image ID="imgRegistrationNoError" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" 
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top"   Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 200px" class="tableCellStyleLite">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 5px; width: 350px;" class="tableCellStyleLite">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" colspan="4">
                                        <asp:RadioButton ID="rbDOB" runat="server" AutoPostBack="True" GroupName="DOBType" />
                                        <asp:TextBox ID="txtDOB" runat="server" Enabled="False" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                        <asp:Image ID="imgDOBError" runat="server" ImageAlign="AbsBottom" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                </tr>
                                <tr id="trDOBInWord" visible="false" runat="server">
                                    <td colspan="4" valign="top" style="padding-top: 2px">
                                        <asp:RadioButton ID="rbDOBInWord" runat="server" AutoPostBack="True" GroupName="DOBType"
                                            Width="90px" CssClass="tableText" />&nbsp;<asp:DropDownList ID="ddlDOBinWordType"
                                                runat="server" Width="120px">
                                            </asp:DropDownList><asp:TextBox ID="txtDOBInWord" runat="server" Enabled="False"
                                                MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                        <asp:Image ID="imgDOBInWordError" runat="server" ImageAlign="AbsBottom" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 200px" class="tableCellStyleLite">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 5px; width: 350px;" class="tableCellStyleLite">
                            <table id="TABLE1" border="0" cellpadding="0" cellspacing="0" language="javascript" >
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                            Width="105px"></asp:TextBox></td>
                                    <td>
                                        <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label></td>
                                    <td>
                                        &nbsp;<asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox></td>
                                    <td>
                                        &nbsp;<asp:Image ID="imgENameError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                            ImageAlign="Top"  /></td>
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
                        <td valign="top" style="width: 200px" class="tableCellStyleLite">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 350px" class="tableCellStyleLite">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderError" runat="server" ImageAlign="AbsBottom" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                    <tr id="trTransactionNo_M" runat="server">
                        <td style="width: 200px;" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyleLite">
                            <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
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
