<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="spEnquirePCDStatus.aspx.vb" Inherits="HCVU.spEnquirePCDStatus" Title="<%$ Resources:Title, SPEnquirePCDStatusBanner %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, SPEnquirePCDStatusBanner %>" ToolTip="<%$ Resources:AlternateText, SPEnquirePCDStatusBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, SPEnquirePCDStatusBanner %>" /></td>
                </tr>
            </table>
            <cc1:InfoMessageBox ID="CompleteMsgBox" runat="server" Width="100%" />
            <cc1:MessageBox ID="msgBox" runat="server" Width="100%" />
            <asp:Panel ID="pnlEnquiry" runat="server">
                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">
                        <table>
                            <tr>
                                <td style="width: 160px">
                                    <asp:RadioButton ID="rdoSPID" runat="server" AutoPostBack="true" Checked="true" Text="<%$ Resources:Text, ServiceProviderID %>" />
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtSPID" runat="server" MaxLength="8"></asp:TextBox></td>
                                <td style="width: 80px;">
                                    <asp:Label ID="lblOr" runat="server">OR
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:RadioButton ID="rdoSPHKID" runat="server" AutoPostBack="true"  Text="<%$ Resources:Text, ServiceProviderHKID %>" />
                                <td>
                                    <asp:TextBox ID="txtSPHKID" runat="server" MaxLength="11" onBlur="formatHKID(this)" Enabled="false"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />
                        <div class="headingText">
                            <asp:Label ID="lblREnquireResult" runat="server" Text="<%$ Resources:Text, EnquiryResult %>"
                                Font-Bold="True"></asp:Label>
                        </div>
                        <table style="padding-top:6px">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblRSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>" />
                                <td style="width: 400px">
                                    <asp:Label ID="lblRSPID" runat="server" class="tableText">-</asp:Label></td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblRHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>" />
                                <td style="width: 400px">
                                    <asp:Label ID="lblRHKID" runat="server" class="tableText">-</asp:Label></td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblRPCDStatusText" runat="server" Text="<%$ Resources:Text, PCDStatus %>" />
                                <td style="width: 400px">
                                    <asp:Label ID="lblRPCDStatus" runat="server" class="tableText">-</asp:Label></td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblRPCDProfessionalText" runat="server" Text="<%$ Resources:Text, PCDProfessional %>" />
                                <td style="width: 400px">
                                    <asp:Label ID="lblRPCDProfessional" runat="server" class="tableText">-</asp:Label></td>
                                </td>
                            </tr>
                        <table>
                        </table>
                        <table  style="padding-top:10px;">
                            <tr>
                                <td style="padding-left:205px;">
                                    <asp:ImageButton ID="ibtnEnquire" runat="server" AlternateText="<%$ Resources:AlternateText, EnquireBtn %>" ToolTip="<%$ Resources:AlternateText, EnquireBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, EnquireBtn %>"/></td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredSPID" runat="server" TargetControlID="txtSPID"
                            FilterType="Custom, Numbers">
                        </cc2:FilteredTextBoxExtender>
                        <cc2:FilteredTextBoxExtender ID="FilteredSPHKID" runat="server" TargetControlID="txtSPHKID"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="()">
                        </cc2:FilteredTextBoxExtender>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>
