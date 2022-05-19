<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="invalidlink.aspx.vb" Inherits="HCSP.invalidlink" EnableEventValidation="False" %>

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
                <span style="font-size: 16pt;">Page not found.    
                    Please wait 15 seconds to return to the  <a href="../en/index.aspx">main page</a>.
                </span>
        </ContentTemplate>
    </asp:UpdatePanel>
       <script type="text/javascript">
           setTimeout("location.href='../en/index.aspx'", 15000);
    </script>
</asp:Content>
