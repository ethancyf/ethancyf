<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKBC.ascx.vb"
    Inherits="HCSP.UIControl.DocTypeText.ucInputHKBC" %>
<asp:Panel ID="panInputHKBC" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblRegistrationNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox ID="txtRegistrationNo" runat="server" MaxLength="6" Enabled="False"
                                Width="90px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblRegistrationNo" runat="server" Visible="False" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox ID="txtDOB" runat="server" Enabled="False" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableTitle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" Width="105px"></asp:TextBox><asp:Label ID="lblSurNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblSurname" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40"></asp:TextBox>
                            <asp:Label ID="lblGivenNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblGivenName" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList><asp:Label ID="lblGenderError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
