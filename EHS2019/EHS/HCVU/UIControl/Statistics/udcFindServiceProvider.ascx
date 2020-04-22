<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcFindServiceProvider.ascx.vb"
    Inherits="HCVU.udcFindServiceProvider" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panInput" runat="server">
        <tr style="height:24px" class ="FindSP_tr">
            <td style="width: 150px;text-align:left;vertical-align:top" class ="FindSP_SPIDLabelWidth">
                <asp:Label ID="lblSPIDText" runat="server" CssClass="FindSP_SPIDLabel" Text="<%$ Resources: Text, ServiceProviderID %>"></asp:Label>
            </td>
            <td style="text-align:left; width: 600px; vertical-align:top" class ="FindSP_SPIDValueWidth">
                <div class="FindSP_SPIDValue">
                    <table style="border-collapse:collapse;padding:0px;border-spacing:0px">
                        <tr>
                            <td style="white-space: nowrap; padding-right: 5px">
                                <asp:TextBox ID="txtSPID" runat="server" MaxLength="8" CssClass="FindSP_SPIDTextBox">&nbsp;&nbsp;</asp:TextBox>
                                <asp:Image ID="imgSPIDError" runat="server" CssClass="FindSP_SPIDErrorImage" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                <cc1:FilteredTextBoxExtender ID="fteSPID" runat="server" TargetControlID="txtSPID" FilterType="Numbers" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnSPIDSearch" runat="server" CssClass="FindSP_SearchBtn"
                                    ImageUrl="<%$ Resources: ImageUrl, SearchSBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SearchSBtn %>" 
                                    OnClick="ibtnSPIDSearch_Click" />
                                <asp:ImageButton ID="ibtnSPIDClear" runat="server" CssClass="FindSP_ClearBtn"
                                    ImageUrl="<%$ Resources: ImageUrl, ClearSBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ClearBtn %>" 
                                    OnClick="ibtnSPIDClear_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr style="height:24px" class ="FindSP_tr">
            <td style="width: 150px;text-align:left;vertical-align:top" class ="FindSP_SPIDLabelWidth">
                <asp:Label ID="lblSPNameText" runat="server" CssClass="FindSP_NameLabel" Text="<%$ Resources: Text, ServiceProviderName %>" />
            </td>
            <td style="text-align:left; width: 600px; vertical-align:top" class ="FindSP_SPIDValueWidth">
                <asp:Label ID="lblSPName" runat="server" CssClass="FindSP_NameValue" />
            </td>
        </tr>
    </asp:Panel>
</table>
