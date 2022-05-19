<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="InvalidOS.aspx.vb" Inherits="HCSP.Text.EN.InvalidOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <div>
                <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="eHealth System (Subsidies)"
                                Font-Underline="True" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-size: 12pt;">
                                Windows XP is not supported on eHealth System (Subsidies) (eHS(S)). If you wish to continue accessing eHS(S), you MUST use supported Operating System (OS) (for example, Windows 7 or above).
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

