<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="ImproperAccess.aspx.vb"
    Inherits="HCSP.Text.ZH.ImproperAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <div>
                <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server">
                    <tr>
                        <td style="width: 100%">
                            <asp:Label ID="Label1" runat="server" Text="醫健通(資助)系統" Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <span style="font-size: 12pt;">
                                發現多於一個瀏覽器同時操作或不正當的操作，已中止目前的瀏覽器操作。詳情請參考常見問題。<br />
                                請按 <a href="../login.aspx">此處</a> 重新登入。<br />
                                或請按 <a href="javascript:window.close();">此處</a> 關閉目前的瀏覽器。<br />
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
