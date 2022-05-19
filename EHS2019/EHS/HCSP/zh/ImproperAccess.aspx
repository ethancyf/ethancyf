<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="ImproperAccess.aspx.vb" Inherits="HCSP.ZH.ImproperAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header_tw.jpg); width: 990px; background-repeat: no-repeat; height: 100px"
                    runat="server">
                    <tr>
                        <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironmentZH">
                            <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <span style="font-size: 16pt; width: 950pt;">發現多於一個瀏覽器同時操作或不正當的操作，已中止目前的瀏覽器操作。詳情請參考<asp:LinkButton ID="LinkButtonFAQs" runat="server">常見問題</asp:LinkButton>。<br />
                請按 <a href="../login.aspx">此處</a> 重新登入。
            <br />
                或請按 <a href="javascript:window.close();">此處</a> 關閉目前的瀏覽器。
            <br />
            </span>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



