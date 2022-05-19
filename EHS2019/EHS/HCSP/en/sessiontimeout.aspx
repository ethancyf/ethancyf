<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="sessiontimeout.aspx.vb" Inherits="HCSP.sessiontimeout1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            <span style="font-size: 16pt;">&nbsp;System timeout. Please click <a href="../login.aspx">here</a> to back to login again.</span>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
