<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="InvalidOS.aspx.vb" Inherits="HCSP.ZH.InvalidOS"  EnableEventValidation="False" %>

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
            <table>
                <tr>
                    <td style="width:10px"></td>
                    <td><span style="font-size: 16pt;">醫健通（資助）系統已不支援微軟視窗XP。如你希望繼續使用醫健通（資助）系統，你必須使用可支援的操作系統（例如：微軟視窗7或以上版本）。</span></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

