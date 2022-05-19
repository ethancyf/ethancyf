<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="ImproperAccess.aspx.vb" Inherits="HCSP.EN.ImproperAccess" %>


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
            <span style="font-size: 16pt; width: 950pt;">Concurrent access or improper access detected, the action of this browser window is aborted. Please refer
                <br />
                to 
                <asp:LinkButton ID="LinkButtonFAQs" runat="server">FAQs</asp:LinkButton>
                for details.
            <br />
                Please click <a href="../login.aspx">here</a> to back to login page,
            <br />
                or click <a href="javascript:window.close();">here</a> to close this browser window.
            <br />
            </span>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

