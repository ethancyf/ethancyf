<%@ Control Language="vb" AutoEventWireup="false" Codebehind="spPrintFunction.ascx.vb"
    Inherits="HCVU.spPrintFunction" %>
<table>
    <tr>
        <td colspan="2">
            <div class="headingText">
                <asp:Label ID="lblHeader" runat="server" Text="(to be controlled code-behind from Resource)"></asp:Label></div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="pnlSchemeSelection" runat="server">
                <asp:CheckBoxList ID="cblScheme" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblScheme_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td style="height: 10px">
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:ImageButton ID="ibtnAction" runat="server" AlternateText="Print / Send (to be controlled code-behind)"
                OnClick="ibtnAction_Click" ImageUrl="<%$ Resources:ImageUrl, PrintBtn %>" />
            <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnCancel_Click" />
        </td>
    </tr>
</table>
<asp:HiddenField ID="hfActionCode" runat="server" />
<asp:HiddenField ID="hfActionType" runat="server" />
