<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputAdoption.ascx.vb" Inherits="HCSP.ucInputAdoption" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel ID="PanVISA" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr runat="server" id="trReferenceNo_M">
            <td style="height: 19px ;width: 200px" valign="top" class="tableCellStyle">
                <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 430px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>       
        <tr>
            <td style="width: 200px" valign="top" class="tableCellStyle">
                <asp:Label ID="lblEntryNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 430px" valign="top" class="tableCellStyle">
                <asp:TextBox ID="txtPerfixNo" runat="server" MaxLength="7" onChange="convertToUpper(this);" Width="75px" />
                <asp:Label ID="lblEntryNoSymbol" runat="server" CssClass="tableTitle" Text="/" />  
                <asp:TextBox ID="txtIdentityNo" runat="server" MaxLength="5" onChange="convertToUpper(this);" Width="40px" />
                <asp:Label ID="lblEntryNo" runat="server"  Visible="False" CssClass="tableText" />     
                <asp:Image ID="imgEntryNo" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top"   />

            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 200px" class="tableCellStyle">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 430px" valign="top" class="tableCellStyle">
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
            <td style="width: 200px;vertical-align:top" class="tableCellStyleLite">
                <asp:Label ID="lblCName" runat="server" CssClass="tableTitle" Width="150px" />
            </td>
            <td style="padding-bottom: 5px; width: 350px;vertical-align:top" class="tableCellStyleLite">
                <asp:TextBox ID="txtCName" runat="server" MaxLength="12" CssClass="TextBoxChineseName" onChange="convertToUpper(this);" Width="150px" />
                <asp:Image ID="imgCNameError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" />
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
                <asp:Image ID="imgGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" />
            </td>                       
        </tr>
        <tr>
            <td valign="top" style="width: 200px" class="tableCellStyle">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" style="width: 430px" class="tableCellStyle">
               <table cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:RadioButton id="rbDOB" runat="server" CssClass="tableTitle" GroupName="DOBType" AutoPostBack="True"></asp:RadioButton> 
                                <asp:TextBox id="txtDOB" runat="server" Width="75px" Enabled="False" MaxLength="10" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                <asp:Image ID="imgDOBError" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                        </tr>
                        <tr id="trDOBInWord" visible="false" runat="server">
                            <td valign="top" colspan="4" style="padding-top: 2px">
                                <asp:RadioButton id="rbDOBInWord" runat="server" Width="90px" CssClass="tableText" GroupName="DOBType" AutoPostBack="True"></asp:RadioButton>&nbsp;<asp:DropDownList id="ddlDOBinWordType" runat="server" Width="120px">
                            </asp:DropDownList>
                                <asp:TextBox id="txtDOBInWord" runat="server" Width="75px" Enabled="False" MaxLength="10" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image id="imgDOBInWordError" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"></asp:Image></td>
                        </tr>
                    </tbody>
                 </table>
               </td>
        </tr>
        <tr id="trTransactionNo_M" runat="server">
            <td style="height: 19px; width: 200px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 430px;" valign="top" class="tableCellStyle">
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