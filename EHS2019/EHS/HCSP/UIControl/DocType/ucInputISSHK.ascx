<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputISSHK.ascx.vb" Inherits="HCSP.ucInputISSHK" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="panInputISSHK" runat="server">
    <table style="border-width:0px;border-collapse:collapse;padding:0px 0px 0px 0px">
        <tr runat="server" id="trReferenceNo_M">
            <td style="height: 19px; width: 200px;vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblReferenceNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 350px;vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblReferenceNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 200px;vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblTDNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px;vertical-align:top" class="tableCellStyle">
                <asp:TextBox ID="txtTDNo" runat="server" MaxLength="11" onChange="convertToUpper(this);" Width="120px" />
                <asp:Label ID="lblTDNo" runat="server" CssClass="tableText" />
                <asp:Image ID="imgTDNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top" Visible="false" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 750px;vertical-align:top" class="tableCellStyle">
                <table style="border-width:0px;border-collapse:collapse;padding:0px 0px 0px 0px">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="80" onChange="convertToUpper(this);" Width="340px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblENameComma" runat="server" CssClass="largeText"></asp:Label>
                        </td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="80" onChange="convertToUpper(this);" Width="340px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;<asp:Image ID="imgEName" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                            ImageAlign="Top" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSurname" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td></td>
                        <td>&nbsp;<asp:Label ID="lblGivenName" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px;vertical-align:top" class="tableCellStyle">
                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>&nbsp;
                <asp:Image ID="imgGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top" Visible="false" /></td>
        </tr>
        <tr>
            <td style="vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="width: 350px;vertical-align:top" class="tableCellStyle">
                <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Image ID="imgDOBDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    ImageAlign="Top" Visible="false" /></td>
        </tr>
<%--        <tr>
            <td style="vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblNationality" runat="server" CssClass="tableTitle"
                    Width="200px"></asp:Label>
            </td>
            <td >
                <asp:DropDownList ID="ddlNationality" runat="server" Width="300px" />
                <asp:Image ID="imgNationality" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
              ImageAlign="Top"  Visible="false"/>
            </td>
        </tr>--%>
        <tr id="trTransactionNo_M" runat="server">
            <td style="height: 19px;vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblTransactionNoText_M" runat="server" CssClass="tableTitle"
                    Width="150px"></asp:Label></td>
            <td style="height: 19px; width: 350px;vertical-align:top" class="tableCellStyle">
                <asp:Label ID="lblTransactionNo_M" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>

<cc1:FilteredTextBoxExtender ID="filtereditTDNo" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Numbers"
    TargetControlID="txtTDNo"></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameSurname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
    TargetControlID="txtENameFirstname" ValidChars="-' "></cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="filtereditDOB" runat="server" FilterType="Numbers, Custom"
    TargetControlID="txtDOB" ValidChars="-"></cc1:FilteredTextBoxExtender>




