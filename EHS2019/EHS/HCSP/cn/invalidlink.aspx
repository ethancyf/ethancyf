<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="invalidlink.aspx.vb" Inherits="HCSP.CN.InvalidLink" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header_cn.jpg);
                    width: 990px; background-repeat: no-repeat; height: 100px" runat="server">
                    <tr>
                        <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironmentZH">
                            <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <span style="font-size: 16pt;">未能找到网页。 <br /> 系统将于15秒后自动返回<a href="../cn/index.aspx">首页</a>。</span>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        setTimeout("location.href='../en/index.aspx'", 15000);
    </script>
</asp:content>
