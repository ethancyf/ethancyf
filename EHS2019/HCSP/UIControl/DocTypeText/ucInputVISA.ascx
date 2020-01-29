<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVISA.ascx.vb" Inherits="HCSP.UIControl.DocTypeText.ucInputVISA" %>
<asp:Panel ID="PanVISA" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td valign="top">
                <asp:Label ID="lblVISANoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtVISANo1" runat="server" MaxLength="4" Width="60px" Enabled="False"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol1" runat="server" CssClass="tableTitle"
                    Text="-"></asp:Label>
                <asp:TextBox ID="txtVISANo2" runat="server" MaxLength="7" Width="50px" Enabled="False"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol2" runat="server" CssClass="tableTitle"
                    Text="-"></asp:Label>
                <asp:TextBox ID="txtVISANo3" runat="server" MaxLength="2" Width="20px" Enabled="False"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol3" runat="server" CssClass="tableTitle"
                    Text="("></asp:Label>
                <asp:TextBox ID="txtVISANo4" runat="server" MaxLength="1" Width="12px" Enabled="False"></asp:TextBox>
                <asp:Label ID="lblVISANoSymbol4" runat="server" CssClass="tableTitle"
                    Text=")"></asp:Label>                     
                <asp:Label ID="lblVISANo" runat="server"  Visible="False" CssClass="tableText"></asp:Label><asp:Label ID="lblVISANoError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
             <td valign="top">
                <asp:Label ID="lblPassportNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtPassportNo" runat="server" MaxLength="20" Width="200px"></asp:TextBox> 
                <asp:Label ID="lblPassportNoError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
            </td>
        </tr>       
        <tr>
            <td valign="top">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" Width="105px"></asp:TextBox><asp:Label ID="lblSurNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblSurname" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40"></asp:TextBox><asp:Label ID="lblGivenNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
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
        <tr>
            <td valign="top">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtDOB" runat="server" MaxLength="10" Width="75px" Enabled="False" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="lblDOBError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
