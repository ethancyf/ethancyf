<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputID235B.ascx.vb" Inherits="HCSP.ucInputID235B" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel ID="panInputID235B" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr runat="server" id="trReferenceNo_M">
                        <td style="width: 201px" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle"
                                Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>                   
                    <tr>
                        <td style="width: 201px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblBENoText" runat="server" CssClass="tableTitle"
                                Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyle">
                            <asp:TextBox ID="txtBENo" runat="server" MaxLength="9" Width="75px" />
                            <asp:Label ID="lblBENo" runat="server"  Visible="False" CssClass="tableText" />   
                            <asp:Image ID="imgBENoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top"  Visible="false" />
                        </td>
                        <td valign="top" class="tableCellStyle">
                            <asp:Label ID="lblBENoTip" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 201px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableTitle"
                                Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyle">
                            <table id="TABLE1" border="0" cellpadding="0" cellspacing="0" language="javascript">
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
                         <td valign="top" class="tableCellStyle"></td>
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
                    <tr>
                        <td valign="top" style="width: 201px" class="tableCellStyle">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle"
                                Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 350px" class="tableCellStyle">
                            <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgDOBError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top" Visible="false" /></td>
                        <td valign="top" class="tableCellStyle">
                            </td>
                    </tr>
                    <tr>
                       <td valign="top" style="width: 201px" class="tableCellStyle">
                            <asp:Label ID="lblPermitRemain" runat="server" CssClass="tableTitle"
                              Width="200px"></asp:Label></td>
                       <td valign="top" style="width: 350px" class="tableCellStyle">  
                            <asp:TextBox ID="txtPermitRemain" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="imgPermitRemainError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                ImageAlign="Top"  Visible="false" /></td>             
                       <td valign="top" class="tableCellStyle">
                            <asp:Label ID="lblPermitRemainTip" runat="server" CssClass="tableTitle"></asp:Label></td>                             
                    </tr>
                    <tr id="trTransactionNo_M" runat="server">
                        <td style="width: 201px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle"
                                Width="150px"></asp:Label></td>
                        <td style="width: 350px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
            </td>
            <td style="width: 100px">
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
<cc1:FilteredTextBoxExtender ID="filtereditBENo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
    TargetControlID="txtBENo" >
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server"
    FilterType="Custom, Numbers" TargetControlID="txtDOB"
    ValidChars="-">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditPermitRemain" runat="server"
    FilterType="Custom, Numbers" TargetControlID="txtPermitRemain"
    ValidChars="-">
</cc1:FilteredTextBoxExtender>
