<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PracticeSelection.ascx.vb" Inherits="HCSP.PracticeSelection" %>
<table style="width: 600px">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblSelectPracticeText" runat="server" Text="Please select Bank Account" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:RadioButtonList ID="rbPracticeSelection" runat="server" CssClass="tableText">
                        </asp:RadioButtonList></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
