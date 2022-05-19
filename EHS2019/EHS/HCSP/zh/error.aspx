<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="error.aspx.vb" Inherits="HCSP._error2" EnableEventValidation="False" %>

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
            <span style="font-size: 16pt;">&nbsp;系統發生錯誤，請按 <a href="../login.aspx">此處</a> 重新登入。</span>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
