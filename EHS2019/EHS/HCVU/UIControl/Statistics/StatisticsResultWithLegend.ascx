<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StatisticsResultWithLegend.ascx.vb" Inherits="HCVU.StatisticsResultWithLegend" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/ProfessionLegend.ascx" TagName="ucProfessionLegend"
    TagPrefix="uc4" %>
<%@ Register Src="~/UIControl/DistrictLegend.ascx" TagName="ucDistrictLegend" TagPrefix="uc5" %>

<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="left" style="width: 180px; vertical-align: top">
            <asp:Label ID="lblParameterTitle" runat="server" CssClass="tableTitle" />
        </td>
        <td align="left">
            <asp:Label ID="lblParameterValue" runat="server" CssClass="tableText" />
        </td>
        <td align="left" style="vertical-align: top; padding-left: 5px;">
            <asp:ImageButton ID="ibtnOpenLegend" runat="server" ImageUrl="<%$ Resources:ImageUrl, Infobtn %>" Visible = "False" />
            <asp:Button ID="btnHiddenProfessionLegend" runat="server" Style="display: none" />
            <cc1:modalpopupextender id="popupProfessionLegend" runat="server" targetcontrolid="btnHiddenProfessionLegend"
                popupcontrolid="panProfessionLegend" backgroundcssclass="modalBackgroundTransparent"
                dropshadow="False" repositionmode="RepositionOnWindowScroll" popupdraghandlecontrolid="panProfessionLegendHeading">
            </cc1:modalpopupextender>
            <asp:Button ID="btnHiddenDistrictLegend" runat="server" Style="display: none" />
            <cc1:modalpopupextender id="popupDistrictLegend" runat="server" targetcontrolid="btnHiddenDistrictLegend"
                popupcontrolid="panDistrictLegend" backgroundcssclass="modalBackgroundTransparent"
                dropshadow="False" repositionmode="RepositionOnWindowScroll" popupdraghandlecontrolid="panDistrictLegendHeading">
            </cc1:modalpopupextender>
        </td>
    </tr>
    <tr style="height: 4px">
    </tr>
</table>

<asp:Panel ID="panProfessionLegend" runat="server" Style="display: none;">
    <asp:Panel ID="panProfessionLegendHeading" runat="server" Style="cursor: move;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 485px">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblProfessionLegendHeading" runat="server" Text="<%$ Resources:Text, Legend %>">
                    </asp:Label></td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 485px">
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            </td>
            <td style="background-color: #ffffff; padding: 10px 10px 5px 10px" align="left">
                <asp:Panel ID="panProfessionLegendContent" runat="server" ScrollBars="none" Height="340px">
                    <uc4:ucProfessionLegend ID="udcProfessionLegend" runat="server" />
                </asp:Panel>
            </td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            </td>
            <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                <asp:ImageButton ID="ibtnCloseProfessionLegend" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseProfessionLegend_Click" /></td>
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
<asp:Panel ID="panDistrictLegend" runat="server" Style="display: none;">
    <asp:Panel ID="panDistrictLegendHeading" runat="server" Style="cursor: move;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 335px">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblDistrictLegendHeading" runat="server" Text="<%$ Resources:Text, Legend %>">
                    </asp:Label></td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 335px">
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            </td>
            <td style="background-color: #ffffff; padding: 10px 10px 5px 10px" align="left">
                <asp:Panel ID="panDistrictLegendContent" runat="server" ScrollBars="None" Height="520px">
                    <uc5:ucDistrictLegend ID="udcDistrictLegend" runat="server" />
                </asp:Panel>
            </td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            </td>
            <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                <asp:ImageButton ID="ibtnCloseDistrictLegend" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseDistrictLegend_Click" /></td>
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