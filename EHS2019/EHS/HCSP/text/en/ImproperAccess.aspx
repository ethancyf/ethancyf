<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="ImproperAccess.aspx.vb"
    Inherits="HCSP.Text.EN.ImproperAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <div>
                <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server" style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="eHealth System (Subsidies)" Font-Underline="True"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-size: 12pt;">Concurrent access or improper access detected, the action of this browser window is aborted. Please refer to FAQs for details.<br />
                                Please click <a href="../login.aspx">here</a> to back to login page,<br />
                                or click <a href="javascript:window.close();">here</a> to close this browser window.<br />
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
