<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputAdoption.ascx.vb" Inherits="HCSP.UIControl.DocTypeText.ucInputAdoption" %>
<asp:Panel ID="PanAdoption" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" style="padding-bottom: 2px">
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblEntryNoText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox ID="txtPerfixNo" runat="server" MaxLength="7" Width="75px" Enabled="False"></asp:TextBox>
                            <asp:Label ID="lblEntryNoSymbol" runat="server" CssClass="tableTitle" Text="/"></asp:Label>
                            <asp:TextBox ID="txtIdentityNo" runat="server" MaxLength="5" Width="40px" Enabled="False"></asp:TextBox><asp:Label ID="lblEntryNoError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblEntryNoTip" runat="server" CssClass="tableTitle"></asp:Label></td>
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
                            <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40"></asp:TextBox><asp:Label ID="lblGivenNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
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
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList><asp:Label ID="lblGenderError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox ID="txtDOB" runat="server" Width="75px" Enabled="False" MaxLength="10" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
