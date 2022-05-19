<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="InvalidOS.aspx.vb" Inherits="HCSP.CN.InvalidOS" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header.jpg); width: 990px; background-repeat: no-repeat; height: 100px"
                    runat="server">
                    <tr>
                        <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironment">
                            <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <table>
                <tr>
                    <td style="width:10px"></td>
                    <td><span style="font-size: 16pt;">医健通（资助）系统已不支援微软视窗XP。 如你希望继续使用医健通（资助）系统，你必须使用可支援的操作系统（例如:微软视窗7或以上版本）。</span></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:content>
