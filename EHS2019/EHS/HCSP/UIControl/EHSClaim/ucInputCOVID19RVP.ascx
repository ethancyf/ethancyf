<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputCOVID19RVP.ascx.vb" Inherits="HCSP.ucInputCOVID19RVP" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
    <asp:Panel ID ="panCategory" runat="server" Visible ="false">
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCCategoryText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, Category%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCCategoryCovid19" runat="server" AutoPostBack="false" Style="width:300px;position:relative;top:-3px;left:-5px" />
                        <asp:TextBox ID="txtCCategory" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCRecipientTypeText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, RecipientType%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:radiobuttonlist ID="rblCRecipientType" runat="server" AutoPostBack="false" RepeatDirection="Horizontal" CssClass="tableText" Style="min-width:110px;position:relative;top:-3px;left:-9px" />
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCRecipientTypeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID="panRCHCode" runat="server">
    <tr>
        <td style="width: 185px;vertical-align:middle">
            <asp:Label ID="lblRCHCodeText" runat="server" CssClass="tableTitle" Width="160px"
                Height="25px"></asp:Label>
        </td>
        <td style="padding-bottom: 3px">
            <asp:TextBox ID="txtRCHCodeText" runat="server" Width="100" MaxLength="6" AutoPostBack="true" style="position:relative;left:-4px" />
            <asp:Image ID="imgRCHCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
            <asp:ImageButton ID="btnSearchRCH" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                ImageAlign="AbsMiddle" />
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText" Style="display: none" />
        </td>
    </tr>
    <tr>
        <td style="width: 185px;vertical-align:top">
            <asp:Label ID="lblRCHNameText" runat="server" CssClass="tableTitle" Width="160px" Height="25px" />
        </td>
        <td style="padding-bottom: 3px">
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText" style="position:relative;left:-3px" />
            <asp:Label ID="lblRCHNameChi" runat="server" CssClass="tableTextChi" style="position:relative;left:-3px" />
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCVaccineBrandText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, Vaccines%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCVaccineBrandCovid19" runat="server" AutoPostBack="false" Style="width:660px;position:relative;top:-3px;left:-5px" />
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
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCVaccineLotNoText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, VaccineLotNumber%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCVaccineLotNoCovid19" runat="server" AutoPostBack="false" Style="width:300px;position:relative;top:-3px;left:-5px" />
                        <asp:TextBox ID="txtCVaccineLotNo" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCVaccineLotNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID ="panVaccineName" runat="server" Visible ="false">
        <tr>
            <td style="width:205px;vertical-align:top">
                <asp:Label ID="lblCVaccineText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, Vaccines%>" />
            </td>
            <td style="vertical-align:central">
                <asp:Label ID="lblCVaccine" runat="server" CssClass="tableText" style="position:relative;top:-3px;left:-5px" />
            </td>
        </tr>
    </asp:Panel>
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCDoseText" runat="server" CssClass="tableTitle" Height="20px" Width="160px" Text ="<%$ Resources:Text, DoseSeq%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCDoseCovid19" runat="server" AutoPostBack="false" Style="width:300px;position:relative;top:-3px;left:-5px" />
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCDoseError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
