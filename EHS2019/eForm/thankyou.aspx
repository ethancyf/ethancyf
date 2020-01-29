<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
     CodeBehind="thankyou.aspx.vb" Inherits="eForm.thankyou"Title="<%$ Resources:Title, eForm %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 990px;">
            <tr style="height: 220px; width: 95%;">
                <td style="background-color: #fcfcfc">
                    <table cellpadding="2" cellspacing="2" style="height: 100%; width: 95%;" align="center">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="border-left-color: gray; border-right-color: gray; border-top-color: gray; border-bottom-color: gray; border-top-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-style: solid" align="center">
                                <asp:Label ID="lblThankEng" Text="Thank you for using eHealth System (Subsidies)" runat="server" Font-Bold="true" Font-Size="20px"></asp:Label><br /><br /><br />
                                <asp:Label ID="lblThankChi" Text="多謝使用醫健通(資助)系統" runat="server" Font-Bold="true" Font-Size="20px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
       </asp:Content> 