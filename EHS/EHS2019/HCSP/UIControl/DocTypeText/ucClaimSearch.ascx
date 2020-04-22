<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucClaimSearch.ascx.vb" Inherits="HCSP.UIControl.DocTypeText.ucClaimSearch" %>
<asp:Panel ID="panSearchEC" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td valign="top">
                <asp:Label ID="lblECHKIDText" runat="server" CssClass="tableTitle"></asp:Label>
                <asp:Button ID="lnkECDocIDTips" runat="server" Text="<%$ Resources:Text, InputTips %>" BackColor="White" BorderStyle="None" Font-Underline="True" ForeColor="Blue"></asp:Button>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtECHKID" runat="server" AutoCompleteType="Disabled" MaxLength="11" Width="85px"></asp:TextBox><asp:Label ID="ErrECHKID" runat="server" Text="*" CssClass="validateFailText"></asp:Label></td>
        </tr>
        <tr>
            <td rowspan="1" valign="top">
                <asp:Label ID="lblECDOBText" runat="server" CssClass="tableTitle"></asp:Label>
            </td>
        </tr>
        <tr>
            <td rowspan="1" valign="top">
                <asp:RadioButton ID="rbECDOB" runat="server" CssClass="tableTitle" GroupName="groupECDOA" Text="" />
                <asp:TextBox ID="txtECDOB" runat="server" AutoCompleteType="Disabled" MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="ErrECDOB" runat="server" Text="*" CssClass="validateFailText"></asp:Label>
                <asp:Label ID="lblECDOBOrText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td rowspan="1" valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbECDOA" runat="server" CssClass="tableTitle" GroupName="groupECDOA" />&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtECDOAAge" runat="server" AutoCompleteType="Disabled" MaxLength="3" Width="30px"></asp:TextBox><asp:Label ID="ErrECDOAAge" runat="server" CssClass="validateFailText" Text="*"></asp:Label>&nbsp;</td>
                        <td>
                            <asp:Label ID="lblECDOAOnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="padding-left: 20px">
                            <asp:TextBox ID="txtECDOADayEn" runat="server" AutoCompleteType="Disabled" MaxLength="2" Width="20px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:TextBox ID="txtECDOAYearChi" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="36px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox></td>
                        <td align="center">
                            <asp:DropDownList ID="ddlECDOAMonth" runat="server" >
                            </asp:DropDownList></td>
                        <td align="center">
                            <asp:TextBox ID="txtECDOAYearEn" runat="server" AutoCompleteType="Disabled" MaxLength="4" Width="36px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:TextBox ID="txtECDOADayChi" runat="server" AutoCompleteType="Disabled" MaxLength="2" Width="20px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="ErrECDOA" runat="server" Text="*" CssClass="validateFailText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-left: 20px">
                            <asp:Label ID="lblECDOAYearChiText" runat="server" CssClass="tableTitle">Year</asp:Label>
                            <asp:Label ID="lblECDOADayEnText" runat="server" CssClass="tableTitle">Day</asp:Label></td>
                        <td align="center">
                            <asp:Label ID="lblECDOAMonthChiText" runat="server" CssClass="tableTitle">Month</asp:Label>
                            <asp:Label ID="lblECDOAMonthEnText" runat="server" CssClass="tableTitle">Month</asp:Label></td>
                        <td align="center">
                            <asp:Label ID="lblECDOADayChiText" runat="server" CssClass="tableTitle">Day</asp:Label>
                            <asp:Label ID="lblECDOAYearEnText" runat="server" CssClass="tableTitle">Year</asp:Label></td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panSearchHKIC" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <asp:Panel ID="panHKICSymbol" runat="server" Visible="false">
        <tr>
            <td valign="top">
                <asp:Label ID="lblHKICSymbolText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, HKICSymbolLong %>"></asp:Label>
                <asp:Button ID="lnkSearchHKICwithSymbolTips" runat="server" Text="<%$ Resources:Text, InputTips %>" BackColor="White" BorderStyle="None" Font-Underline="True" ForeColor="Blue"></asp:Button>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:RadioButtonList ID="rblHKICSymbol" runat="server" AutoPostBack="true" RepeatDirection ="Horizontal" style="position:relative;left:-5px"/>
                <asp:Label ID="ErrHKICSymbol" runat="server" CssClass="validateFailText" Text="*" />
            </td>
        </tr>
        </asp:Panel>
        <tr>
            <td valign="top">
                <asp:Label ID="lblSearchHKICIdentityNo" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RegistrationNo %>"></asp:Label>
                <asp:Button ID="lnkSearchHKICTips" runat="server" Text="<%$ Resources:Text, InputTips %>" BackColor="White" BorderStyle="None" Font-Underline="True" ForeColor="Blue"></asp:Button>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtSearchHKICIdentityNo" runat="server" AutoCompleteType="Cellular" MaxLength="11" Width="85px"/>
                <asp:Label ID="ErrSearchHKICIdentityNo" runat="server" CssClass="validateFailText" Text="*" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblSearchHKICDOB" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBLong %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtSearchHKICDOB" runat="server" AutoCompleteType="Disabled" MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Label ID="ErrSearchHKICDOB" runat="server" CssClass="validateFailText" Text="*" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panSearchShortNo" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td valign="top">
                <asp:Label ID="lblSearchShortIdentityNo" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RegistrationNo %>"></asp:Label>
                <asp:Button ID="lnkSearchDocIDTips" runat="server" Text="<%$ Resources:Text, InputTips %>" BackColor="White" BorderStyle="None" Font-Underline="True" ForeColor="Blue"></asp:Button>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtSearchShortIdentityNo" runat="server" AutoCompleteType="Cellular" MaxLength="11" Width="85px"></asp:TextBox><asp:Label ID="ErrSearchShortIdentityNo" runat="server" CssClass="validateFailText" Text="*"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblSearchShortDOB" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBLong %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtSearchShortDOB" runat="server" AutoCompleteType="Disabled" MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="ErrSearchShortDOB" runat="server" CssClass="validateFailText" Text="*"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panSearchADOPC" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td valign="top">
                <asp:Label ID="lblADOPCIdentityNoText" runat="server" CssClass="tableTitle"></asp:Label>
                <asp:Button ID="lblADOPCDocIDTips" runat="server" Text="<%$ Resources:Text, InputTips %>" BackColor="White" BorderStyle="None" Font-Underline="True" ForeColor="Blue"></asp:Button>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtADOPCIdentityNoPrefix" runat="server" AutoCompleteType="Cellular" MaxLength="7" Width="65px"></asp:TextBox>
                <asp:Label ID="lblADOPCSlashText" runat="server" CssClass="tableTitle" Text="/"></asp:Label>
                <asp:TextBox ID="txtADOPCIdentityNo" runat="server" AutoCompleteType="Cellular" MaxLength="5" Width="45px"></asp:TextBox><asp:Label ID="ErrADOPCIdentityNo" runat="server" CssClass="validateFailText" Text="*"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lblADOPCDOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBLong %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtADOPCDOB" runat="server" AutoCompleteType="Disabled" MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label ID="ErrADOPCDOB" runat="server" CssClass="validateFailText" Text="*"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
