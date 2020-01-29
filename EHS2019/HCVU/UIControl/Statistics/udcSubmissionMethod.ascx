<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcSubmissionMethod.ascx.vb"
    Inherits="HCVU.udcSubmissionMethod" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panSubmissionMethod" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblSubmissionMethod" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SubmissionMethod %>" />
            </td>
            <td align="left" style="width: 600px;">
                <table style="width: 600px;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px; vertical-align: top">
                            <asp:RadioButtonList ID="rbtnSubmissionMethodType" runat="server" RepeatDirection="Horizontal" CellSpacing="0" CellPadding="0" AutoPostBack="True">
                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value="<%$ Resources:Text, Any %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Text, Specific %>" Value="<%$ Resources:Text, Specific %>"></asp:ListItem>
                            </asp:RadioButtonList>                            
                        </td> 
                        <td style="width: 120px; vertical-align: top">
                            <asp:ImageButton ID="imgAddSubmissionMethod" runat="server" ImageUrl="<%$ Resources:ImageUrl, SelectSBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, AddSBtn %>" OnClick="ibtnAddSubmissionMethod_Click" />
                            <asp:Image ID="imgErrorSubmissionMethod" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="vertical-align: top" />
                        </td>
                        <td style="width: 360px">
                            <asp:Label ID="lblAddSubmissionMethodDisplay" runat="server" />
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
    </asp:Panel>
</table>

<%-- Popup for multi selection, submission method (Start) --%>
<asp:Button ID="btnHiddenSubmissionMethod" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="popupSubmissionMethod" runat="server" TargetControlID="btnHiddenSubmissionMethod"
    PopupControlID="panPopupSubmissionMethod" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="None">
</cc1:ModalPopupExtender>
<asp:Panel ID="panPopupSubmissionMethod" runat="server" Style="display: none;">
    <%-- Panel header --%>
    <asp:Panel ID="panSubmissionMethodHeading" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: auto">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblRejectTitle" runat="server" Text="Submission Method"></asp:Label>
                </td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                </td>
            </tr>
            <%-- Panel body --%>
            <tr>
                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                </td>
                <td style="background-color: #ffffff">
                    <%-- Panel body content --%>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblPleaseSelectSubmissionMethod" runat="server" Text="Please Select:" />
                            </td>
                            <td style="width: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 250px; height: 42px" valign="middle">
                                <asp:CheckBoxList ID="chkSubmissionMethod" runat="server" RepeatColumns="1" RepeatDirection="Vertical">
                                </asp:CheckBoxList>
                            </td>
                            <td style="width: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:ImageButton ID="ibtnSubmissionMethodPopupOK" runat="server" ImageUrl="<%$ Resources:ImageUrl, OKBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, OKBtn %>" OnClick="ibtnSubmissionMethodPopupOK_Click" />
                                <asp:ImageButton ID="ibtnSubmissionMethodPopupCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnSubmissionMethodPopupCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                </td>
            </tr>
            <tr>
                <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                </td>
                <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                    height: 7px">
                </td>
                <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                    height: 7px">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
<%-- Popup for multi selection, submission method (End) --%>
