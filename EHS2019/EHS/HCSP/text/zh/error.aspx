﻿<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CSRFMasterPage.Master" CodeBehind="error.aspx.vb" Inherits="HCSP.Text.ZH._error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <div>
                <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server">
                    <tr>
                        <td style="width: 100%">
                            <asp:Label ID="Label1" runat="server" Text="醫健通(資助)系統" Font-Underline="True" Font-Size="Medium"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <span style="font-size: 12pt;">系統發生錯誤。
                            <br />
                                請按 <a href="../login.aspx">此處</a> 重新登入。
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

