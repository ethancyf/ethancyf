<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputOTHER.ascx.vb"
    Inherits="HCSP.ucInputOTHER" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">



</script>

<asp:Panel ID="panInputOTHER" runat="server">
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
                            <asp:Label ID="lblDocumentNoText" runat="server" CssClass="tableTitle" Width="150px" />
                        </td>
                        <td style="width: 350px;" valign="top" class="tableCellStyleLite">
                            <asp:TextBox ID="txtDocumentNo" runat="server" MaxLength="11" Width="90px" />
                            <asp:Label ID="lblDocumentNo" runat="server" CssClass="tableText" />
                            <asp:Image ID="imgDocumentNoError" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" 
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
                    <tr style="display:none;">
                        <td style="width: 350px;vertical-align:top;" class="tableCellStyleLite">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" AutoPostBack="True">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;                            
                    </tr>
                    <tr id="trGenderImageInput" runat="server" style="height: 190px;">
                        <td style="width: 200px; vertical-align: top" class="tableCellStyleLite">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="180px"></asp:Label></td>
                        <td>
                            <div  id="divGender" runat="server" style="background-color: #ffff99; padding: 15px; width: 395px;">
                                <table id="tblIGender" runat="server" style="border-spacing: 0px; border-collapse: collapse; padding: 0px">
                                    <tr>
                                        <td>
                                            <div id="divFemale" runat="server" style="width: 180px; outline: solid; outline-width: 2px; outline-color: black; padding-left: 5px; background-color: white; position: relative; cursor: pointer;">
                                                <asp:Image ID="imgFemale" runat="server" AlternateText="<%$ Resources:AlternateText, Female%>"
                                                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, SmartICFemale%>" />
                                                <asp:Label ID="lblIFemale" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 65px" />
                                                <asp:Label ID="lblIFemaleChi" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 85px" />
                                            </div>
                                        </td>
                                        <td style="width: 20px"></td>
                                        <td>
                                            <div id="divMale" runat="server" style="width: 180px; outline: solid; outline-width: 2px; outline-color: black; padding-left: 5px; background-color: white; position: relative; cursor: pointer;">
                                                <asp:Image ID="imgMale" runat="server" AlternateText="<%$ Resources:AlternateText, Male%>"
                                                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, SmartICMale%>" />
                                                <asp:Label ID="lblIMale" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 65px" />
                                                <asp:Label ID="lblIMaleChi" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 85px" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText" Visible="false"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <asp:Image ID="imgGenderError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" />
                        </td>                       
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
