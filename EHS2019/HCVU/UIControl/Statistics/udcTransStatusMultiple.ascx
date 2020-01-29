<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcTransStatusMultiple.ascx.vb"
    Inherits="HCVU.udcTransStatusMultiple" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<table>
    <asp:Panel ID="panTransStatus" runat="server">
        <tr class="TransStatusMultiple_tr">
            <td style="width: 150px; text-align:left; vertical-align: top" class="TransStatusMultiple_LabelWidth">
                <asp:Label ID="lblTransStatus" runat="server" CssClass="TransStatusMultiple_Label" Text="<%$ Resources:Text, TransactionStatus %>" />
            </td>
            <td id="tdValue" runat="server" align="left" style="width: 600px;" class="TransStatusMultiple_ValueWidth">
                <div class="TransStatusMultiple_Value">
                    <table style="border-collapse:collapse;padding:0px;border-spacing:0px">
                        <tr>
                            <td style="width: 120px; vertical-align: top" class="TransStatusMultiple_td_01">
                                <asp:RadioButtonList ID="rbtnTransStatus" runat="server" RepeatDirection="Horizontal" CellSpacing="0" CellPadding="0"  CssClass="TransStatusMultiple_RadioButtonList" AutoPostBack="True" >
                                    <asp:ListItem Text="<%$ Resources:Text, Any %>" Value="<%$ Resources:Text, Any %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Text, Specific %>" Value="<%$ Resources:Text, Specific %>"></asp:ListItem>
                                </asp:RadioButtonList>
                                                        
                            </td> 
                            <td style="width: 120px; vertical-align: top" class="TransStatusMultiple_td_02">
                                <asp:ImageButton ID="imgAddTransStatus" runat="server" ImageUrl="<%$ Resources:ImageUrl, SelectSBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, AddSBtn %>" CssClass="TransStatusMultiple_BtnAfterRadioButtonList" OnClick="imgAddTransStatus_Click" />
                                <asp:Image ID="imgErrorTransStatus" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" CssClass="TransStatusMultiple_Alert" Visible="False" />
                            </td>
                            <td id="tdValue03" runat="server" style="width: 360px" class="TransStatusMultiple_td_03">
                                <asp:Label ID="lblAddTransStatusDisplay" runat="server" />
                            </td>
                        </tr>
                    </table>   
                </div>                 
            </td>
        </tr>
    </asp:Panel>
</table>

<%-- Popup for multi selection, health profession (Start) --%>
<asp:Button ID="btnHiddenTransStatus" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="popupTransStatus" runat="server" TargetControlID="btnHiddenTransStatus"
    PopupControlID="panPopupTransStatus" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="None">
</cc1:ModalPopupExtender>
<asp:Panel ID="panPopupTransStatus" runat="server" Style="display: none;">
    <%-- Panel header --%>
    <asp:Panel ID="panPopupProfessionHeading" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" style="width: auto">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="vertical-align: middle; font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblRejectTitle" runat="server" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label>
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
                                <asp:CheckBoxList ID="chkTransStatus" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
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
                                <asp:ImageButton ID="ibtnTransStatusPopupOK" runat="server" ImageUrl="<%$ Resources:ImageUrl, OKBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, OKBtn %>" OnClick="ibtnTransStatusPopupOK_Click" />
                                <asp:ImageButton ID="ibtnTransStatusPopupCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnTransStatusPopupCancel_Click" />
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
