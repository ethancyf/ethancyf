<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputRVPCOVID19.ascx.vb" Inherits="HCVU.ucInputRVPCOVID19" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Panel ID="panCOVID19Detail" runat="server">
    <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse;position:relative;left:-3px">
        <tr>
            <td style="width:205px;vertical-align:top">
                <asp:Label ID="lblCRecipientTypeText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, RecipientType%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:radiobuttonlist ID="rblCRecipientType" runat="server" AutoPostBack="true" RepeatDirection="Vertical" CssClass="tableText" Style="min-width:110px;position:relative;top:-3px;left:-9px" />
                        </td>
                        <td style="vertical-align:top">
                            <asp:Image ID="imgCRecipientTypeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width:200px;vertical-align:top">
                <asp:Label ID="lblCVaccineBrandText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, Vaccines%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:dropdownlist ID="ddlCVaccineBrandCovid19" runat="server" AutoPostBack="true" Style="width:660px;position:relative;top:-3px;left:-6px" />
                            <asp:TextBox ID="txtCVaccineBrand" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                        </td>
                        <td style="vertical-align:top">
                            <asp:Image ID="imgCVaccineBrandError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width:200px;vertical-align:top">
                <asp:Label ID="lblCVaccineLotNoText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, VaccineLotNumber%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:dropdownlist ID="ddlCVaccineLotNoCovid19" runat="server" AutoPostBack="false" Style="width:300px;position:relative;top:-3px;left:-6px" />
                            <asp:TextBox ID="txtCVaccineLotNo" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                        </td>
                        <td style="vertical-align:top">
                            <asp:Image ID="imgCVaccineLotNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trCContactNo" runat="server">
            <td style="width:200px;height:25px;vertical-align:top">
                <asp:Label ID="lblCContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2%>" Width="160px" />
            </td>
            <td style="height:25px;vertical-align:top">
                <asp:textbox ID="txtCContactNo" runat="server" MaxLength="8" style="position:relative;left:-5px;top:-2px" Width="100px"/>
                <asp:ImageButton ID="imgCContactNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
                <asp:Label ID="lblStep2aContactNoRecommendation" runat="server" Text="<%$ Resources:Text, ProvideContactNoWithSMS%>" style="font-size:14px;position:relative;top:-4px" />
                <cc2:FilteredTextBoxExtender ID="fteCContactNo" runat="server" TargetControlID="txtCContactNo"
                                            FilterType="Numbers" />
            </td>
        </tr>
        <tr>
            <td style="width:200px;height:25px;vertical-align:top;padding-bottom:5px">
                <asp:Label ID="lblCRemarkText" runat="server" Text="<%$ Resources:Text, Remarks%>" Width="160px" />
            </td>
            <td style="height:25px;vertical-align:top;padding-bottom:5px">
                <asp:textbox ID="txtCRemark" runat="server" MaxLength="200" style="position:relative;left:-5px" Width="660px"/>
                <asp:ImageButton ID="imgCRemarkError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
            </td>
        </tr>
        <tr id="trJoinEHRSS" runat="server">
            <td style="width:200px;height:25px;vertical-align:top;padding-bottom:5px">
                <asp:Label ID="lblCJoinEHRSSText" runat="server" Text="<%$ Resources:Text, JoinEHRSS%>" Width="160px" />
            </td>
            <td style="height:25px;vertical-align:top;padding-bottom:5px">
                <asp:checkbox ID="chkCJoinEHRSS" runat="server" AutoPostBack="false" style="position:relative;left:-8px"/>
            </td>
        </tr>
    </table>
</asp:Panel>