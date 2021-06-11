<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChooseCCCode.ascx.vb" Inherits="HCVU.ChooseCCCode" %>
<table style="width: 100%">
    <tbody>
        <tr>
            <td align="center" >
                <asp:Label ID="lblChooseCCCode" runat="server" Text="<%$ Resources:Text, ChooseCCCode %>"></asp:Label>
                <table width = "200px" id="tbCCCode" runat="server">
                    <tbody>
                        <tr >
                            <td>
                                <asp:DropDownList ID="ddlCCCode1" runat="server" Width="50px" Font-Names="HA_MingLiu" Font-Size="18px">
                                </asp:DropDownList>
            
                                <asp:DropDownList ID="ddlCCCode2" runat="server" Width="50px" Font-Names="HA_MingLiu" Font-Size="18px">
                                </asp:DropDownList>
               
                                <asp:DropDownList ID="ddlCCCode3" runat="server" Width="50px" Font-Names="HA_MingLiu" Font-Size="18px">
                                </asp:DropDownList>
                           
                                <asp:DropDownList ID="ddlCCCode4" runat="server" Width="50px" Font-Names="HA_MingLiu" Font-Size="18px">
                                </asp:DropDownList>
                            
                                <asp:DropDownList ID="ddlCCCode5" runat="server" Width="50px" Font-Names="HA_MingLiu" Font-Size="18px">
                                </asp:DropDownList>
                          
                                <asp:DropDownList ID="ddlCCCode6" runat="server" Width="50px" Font-Names="HA_MingLiu" Font-Size="18px">
                                </asp:DropDownList>
                            </td>
                       </tr>
                    </tbody>
                </table>
                <br />
                <asp:Label ID="lblClick" runat="server" Text="<%$ Resources:Text, Click %>"></asp:Label>
                <asp:LinkButton ID="lnkHere" runat="server" Text="<%$ Resources:Text, here %>"></asp:LinkButton>
                <asp:Label ID="lblChkCCCode" runat="server" Text="<%$ Resources:Text, CheckInstallCCCode %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 42px" align="center">
                <asp:ImageButton ID="ibtnCCCodeConfirm" OnClick="ibtnCCCodeConfirm_Click" runat="server"
                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>">
                </asp:ImageButton>
                <asp:ImageButton ID="ibtnCCCodeCancel" OnClick="ibtnCCCodeCancel_Click" runat="server"
                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>">
                </asp:ImageButton></td>
        </tr>
    </tbody>
</table>