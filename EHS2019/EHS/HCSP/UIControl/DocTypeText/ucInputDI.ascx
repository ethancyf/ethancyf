<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputDI.ascx.vb" Inherits="HCSP.UIControl.DocTypeText.ucInputDI" %>
<asp:Panel ID="panInputDI" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr runat="server" id="trReferenceNoText">
            <td  valign="top">
                <asp:Label ID="lblReferenceNoText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr runat="server" id="trReferenceNo">
            <td valign="top">
                <asp:Label ID="lblReferenceNo" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td  valign="top">
                <asp:Label ID="lblTDNoText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td  valign="top">
                <asp:TextBox ID="txtTDNo" runat="server" MaxLength="9" Width="120px" Enabled="False"></asp:TextBox><asp:Label ID="lblTDNoError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" Width="105px"></asp:TextBox><asp:Label ID="lblSurNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblSurname" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40"></asp:TextBox><asp:Label ID="lblGivenNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:Label ID="lblGivenName" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList><asp:Label ID="lblGenderError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="lblDOBError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:Label ID="lblDOI" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" >
                <asp:TextBox ID="txtDOI" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="lblDOIError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
