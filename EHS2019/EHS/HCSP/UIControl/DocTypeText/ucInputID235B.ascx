<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputID235B.ascx.vb" Inherits="HCSP.UIControl.DocTypeText.ucInputID235B" %>

<script type="text/javascript">
        function convertToUpper(textbox) {    textbox.value = textbox.value.toUpperCase();}
</script>

<asp:Panel ID="panInputID235B" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td valign="top">
                <asp:Label ID="lblBENoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtBENo" runat="server" MaxLength="9" Width="75px" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblBENo" runat="server"  Visible="False" CssClass="tableText"></asp:Label>   
                <asp:Label ID="lblBENoError" runat="server" CssClass="validateFailText" Text="*"></asp:Label>
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
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText"
                    RepeatDirection="Horizontal" RepeatLayout="Flow">
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
        <tr>
           <td valign="top">
                <asp:Label ID="lblPermitRemain" runat="server" CssClass="tableTitle" Width="200px"></asp:Label></td>
        </tr>
        <tr>
           <td valign="top">  
                <asp:TextBox ID="txtPermitRemain" runat="server" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="lblPermitRemainError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
           </td>             
        </tr>
    </table>
</asp:Panel>
