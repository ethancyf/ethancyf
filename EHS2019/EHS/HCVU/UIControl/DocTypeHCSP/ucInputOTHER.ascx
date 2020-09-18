<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputOTHER.ascx.vb"
    Inherits="HCVU.UIControl.DocTypeHCSP.ucInputOTHER" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">



</script>

<asp:Panel ID="panInputOTHER" runat="server">
<%--    <table border="0" style="border-collapse:collapse;border-spacing:0px;padding: 0px 0px 0px 0px">
        <tr>
            <td style="vertical-align:top">--%>
                <table style="border-collapse:collapse;border-spacing:0px;padding: 0px 0px 0px 0px">
                    <tr runat="server" id="trReferenceNo_M">
                        <td style="vertical-align:top;width:200px;padding: 0px 0px 0px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="vertical-align:top;padding-bottom: 5px; width: 350px;padding: 0px 0px 0px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;width:200px;padding: 0px 0px 0px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblDocumentNoText" runat="server" CssClass="tableTitle" Width="150px" />
                        </td>
                        <td style="vertical-align:top;width:350px;padding:0px 0px 5px 0px" class="tableCellStyleLite">
                            <asp:TextBox ID="txtDocumentNo" runat="server" MaxLength="11" Width="90px" />
                            <asp:Label ID="lblDocumentNo" runat="server" CssClass="tableText" />
                            <asp:Image ID="imgDocumentNoError" runat="server"  AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" 
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top"   Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;width:200px;padding:0px 0px 0px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="vertical-align:top;width:350px;padding:0px 0px 5px 0px" class="tableCellStyleLite">
                            <table border="0" style="border-collapse:collapse;border-spacing:0px;padding:0px 0px 0px 0px">
                                <tr>
                                    <td style="vertical-align:top;padding: 0px 0px 0px 0px" colspan="4">
                                        <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                        <asp:Image ID="imgDOBError" runat="server" ImageAlign="AbsBottom" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;width:200px;padding: 0px 0px 0px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="vertical-align:top;width:350px;padding: 0px 0px 5px 0px" class="tableCellStyleLite">
                            <table id="TABLE1" border="0" style="border-collapse:collapse;border-spacing:0px;padding: 0px 0px 0px 0px">
                                <tr>
                                    <td style="padding:0px 0px 0px 0px">
                                        <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);" Width="105px"></asp:TextBox></td>
                                    <td style="padding:0px 0px 0px 0px">
                                        <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label></td>
                                    <td style="padding:0px 0px 0px 0px">
                                        &nbsp;<asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox></td>
                                    <td style="padding:0px 0px 0px 0px">
                                        &nbsp;<asp:Image ID="imgENameError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                            ImageAlign="Top"  /></td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 0px 0px 0px">
                                        <asp:Label ID="lblSurname" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td style="padding:0px 0px 0px 0px">
                                    </td>
                                    <td style="padding:0px 0px 0px 0px">
                                        &nbsp;<asp:Label ID="lblGivenName" runat="server" CssClass="tableTitle"></asp:Label></td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;width:200px;padding: 0px 0px 0px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="vertical-align:top;width:350px;padding: 0px 0px 5px 0px" class="tableCellStyleLite">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" style="position:relative;left:-4px">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
                            <asp:Image ID="imgGenderError" runat="server" ImageAlign="AbsBottom" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    </tr>
                    <tr id="trTransactionNo_M" runat="server">
                        <td style="vertical-align:top;width: 200px;padding: 0px 0px 0px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="vertical-align:top;width: 350px;padding: 0px 0px 5px 0px" class="tableCellStyleLite">
                            <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
<%--            </td>
        </tr>
    </table>--%>
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
