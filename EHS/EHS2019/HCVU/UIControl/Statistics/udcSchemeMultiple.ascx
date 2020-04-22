<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcSchemeMultiple.ascx.vb"
    Inherits="HCVU.udcSchemeMultiple" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panScheme" runat="server">
        <tr class="SchemeMultiple_tr">
            <td align="left" style="width: 150px; vertical-align: top" class="SchemeMultiple_LabelWidth">
                <asp:Label ID="lblScheme" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
            </td>
            <td id="tdValue" runat="server" align="left" class="SchemeMultiple_ValueWidth">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px; vertical-align: top" class="SchemeMultiple_td_01">
                            <asp:RadioButtonList ID="rbtnScheme" runat="server" RepeatDirection="Horizontal" CssClass="SchemeMultiple_RadioButtonList"
                                 CellSpacing="0" CellPadding="0" AutoPostBack="True">
                                <asp:ListItem Text="<%$ Resources:Text, Any %>" Value="<%$ Resources:Text, Any %>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Text, Specific %>" Value="<%$ Resources:Text, Specific %>"></asp:ListItem>
                            </asp:RadioButtonList>                            
                        </td> 
                        <td style="width: 120px; vertical-align: top" class="SchemeMultiple_td_02">
                            <asp:ImageButton ID="imgAddScheme" runat="server" ImageUrl="<%$ Resources:ImageUrl, SelectSBtn %>" CssClass="SchemeMultiple_BtnAfterRadioButtonList"
                                            AlternateText="<%$ Resources:AlternateText, AddSBtn %>" OnClick="imgAddScheme_Click" />
                            <asp:Image ID="imgErrorScheme" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" CssClass="SchemeMultiple_Alert"
                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                        </td>
                        <td id="tdValue03" runat="server" style="width: 360px" class="SchemeMultiple_td_03">
                            <asp:Label ID="lblAddSchemeDisplay" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
    </asp:Panel>
</table>

<%-- Popup for multi selection, health profession (Start) --%>
<asp:Button ID="btnHiddenScheme" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="popupScheme" runat="server" TargetControlID="btnHiddenScheme"
    PopupControlID="panPopupScheme" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="None">
</cc1:ModalPopupExtender>
<asp:Panel ID="panPopupScheme" runat="server" Style="display: none;">
    <%-- Panel header --%>
    <asp:Panel ID="panPopupProfessionHeading" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: auto">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="vertical-align: middle; font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblRejectTitle" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
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
                                <asp:Label ID="lblPleaseSelectProfession" runat="server" Text="Please Select:" />
                            </td>
                            <td style="width: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: auto; height: 42px" valign="middle">
                                <%--<asp:Image ID="imgReject" runat="server" ImageUrl="~/Images/others/questionMark.png" />--%>
                                <%--<asp:Label ID="lblRemark" runat="server" Text="Reason:"></asp:Label>--%>
                                <asp:CheckBoxList ID="chkScheme" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
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
                                <asp:ImageButton ID="ibtnSchemePopupOK" runat="server" ImageUrl="<%$ Resources:ImageUrl, OKBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, OKBtn %>" OnClick="ibtnSchemePopupOK_Click" />
                                <asp:ImageButton ID="ibtnSchemePopupCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnSchemePopupCancel_Click" />
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
<%-- Popup for multi selection, health profession (End) --%>
