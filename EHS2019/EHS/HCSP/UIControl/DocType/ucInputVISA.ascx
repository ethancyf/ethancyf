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