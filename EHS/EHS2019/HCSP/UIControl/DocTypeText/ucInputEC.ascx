<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputEC.ascx.vb"
    Inherits="HCSP.UIControl.DocTypeText.ucInputEC" %>
<asp:Panel ID="panEnterDetail" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr runat="server" id="trReferenceNoText">
            <td valign="top">
                <asp:Label ID="lblReferenceNoText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr runat="server" id="trReferenceNo">
            <td valign="top">
                <asp:Label ID="lblReferenceNo" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblECSerialNo" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtECSerialNo" runat="server" MaxLength="8" Width="100px"></asp:TextBox>
                <asp:Label ID="lblECSerialNoError" runat="server" CssClass="validateFailText" Text="*"
                    Visible="False"></asp:Label>
                <br />
                <asp:CheckBox ID="cboECSerialNoNotProvided" runat="server" CssClass="tableText" AutoPostBack="True"
                    OnCheckedChanged="cboECSerialNoNotProvided_CheckedChanged" /></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblECReference" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtECRefence1" runat="server" MaxLength="4" Width="60px"></asp:TextBox><asp:Label
                    ID="lblReferenceSep1" runat="server" CssClass="tableTitle" Text="-"></asp:Label><asp:TextBox
                        ID="txtECRefence2" runat="server" MaxLength="7" Width="50px"></asp:TextBox><asp:Label
                            ID="lblReferenceSep2" runat="server" CssClass="tableTitle" Text="-"></asp:Label><asp:TextBox
                                ID="txtECRefence3" runat="server" MaxLength="2" Width="20px"></asp:TextBox><asp:Label
                                    ID="lblReferenceSep3" runat="server" CssClass="tableTitle" Text="("></asp:Label><asp:TextBox
                                        ID="txtECRefence4" runat="server" MaxLength="1" Width="12px"></asp:TextBox><asp:Label
                                            ID="lblReferenceSep4" runat="server" CssClass="tableTitle" Text=")"> </asp:Label><asp:TextBox
                                                ID="txtECRefFree" runat="server" MaxLength="40" Width="200px" Visible="False"></asp:TextBox><asp:Label
                                                    ID="lblECReferenceError" runat="server" CssClass="validateFailText" Text="*"
                                                    Visible="False"></asp:Label>
                <br />
                <asp:Button ID="btnOtherFormat" runat="server" SkinID="TextOnlyVersionLinkButton"
                    OnClick="btnOtherFormat_Click" />
                <asp:Button ID="btnSpecificFormat" runat="server" SkinID="TextOnlyVersionLinkButton"
                    OnClick="btnSpecificFormat_Click" Visible="False" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblECDate" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtECDateDay" runat="server" MaxLength="2" Width="20px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:DropDownList
                        ID="ddlECDateMonth" runat="server">
                    </asp:DropDownList><asp:TextBox ID="txtECDateYear" runat="server" MaxLength="4" Width="36px"
                        onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                        onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                        onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="lblECDateError" runat="server"
                            CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblEName" runat="server" CssClass="tableTitle"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtENameSurname" runat="server" MaxLength="40" Width="105px"></asp:TextBox><asp:Label
                    ID="lblSurNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblSurname" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtENameFirstname" runat="server" MaxLength="40"></asp:TextBox><asp:Label
                    ID="lblGivenNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
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
                <asp:RadioButtonList ID="rbECGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList><asp:Label ID="lblGenderError" runat="server" CssClass="validateFailText"
                    Text="*" Visible="False"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblECHKID" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtECHKIC" runat="server" AutoCompleteType="Disabled" MaxLength="11"
                    Width="85px" Enabled="False"></asp:TextBox></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblECDOB" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtECDOB" runat="server" MaxLength="10" Enabled="False" Width="75px"
                    onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                    onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                    onblur="filterDateInput(this);"></asp:TextBox><asp:Panel ID="panECDOA" runat="server">
                        <asp:Label ID="lblAge" runat="server" CssClass="tableTitle"></asp:Label>
                        <asp:TextBox ID="txtECAge" runat="server" MaxLength="3" Width="40px" Enabled="False"></asp:TextBox>
                        <asp:Label ID="lblRegisterOn" runat="server" CssClass="tableTitle"></asp:Label>
                        <asp:TextBox ID="txtECDOAge" runat="server" Enabled="False" Width="150px"></asp:TextBox></asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="panECDOBType" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td valign="top">
                    &nbsp;<asp:Label ID="lblDOBType" runat="server" CssClass="tableTitle"></asp:Label><asp:Label
                        ID="lblECDOBTypeError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:RadioButtonList ID="rbDOBType" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" RepeatColumns="1">
                        <asp:ListItem Value="D">DOB</asp:ListItem>
                        <asp:ListItem Value="Y">Year of Birth reported</asp:ListItem>
                        <asp:ListItem Value="T">DOB reported on travel document</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
