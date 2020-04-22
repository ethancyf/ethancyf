<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKID.ascx.vb"
    Inherits="HCSP.UIControl.DocTypeText.ucInputHKID" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        <table cellpadding="0" cellspacing="0">
                <tr>
                    <td id="hkid_cell" runat="server" valign="top" enableviewstate="true" class="textVersionTable">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                            <td valign="top" >
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
                                    <asp:TextBox ID="txtENameFirstrname" runat="server" MaxLength="40"></asp:TextBox><asp:Label ID="lblGivenNameError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" >
                                    <asp:Label ID="lblGivenName" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="lblDOB" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtDOB" runat="server" AutoCompleteType="Disabled" Enabled="False" MaxLength="10" Width="75px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                </td>
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
                                    </asp:RadioButtonList>
                                    <asp:Label ID="lblGenderError" runat="server" CssClass="validateFailText" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" >
                                    <asp:Label ID="lblDOI" runat="server" CssClass="tableTitle"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtHKIDIssueDate" runat="server" MaxLength="8" Width="55px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="lblDOIError" runat="server" CssClass="validateFailText" Text="*" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="lblHKIDText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtHKID" runat="server" AutoCompleteType="Disabled" Enabled="False" MaxLength="11" Width="90px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                     
                    </td>
                </tr>
        </table>
                  
                 