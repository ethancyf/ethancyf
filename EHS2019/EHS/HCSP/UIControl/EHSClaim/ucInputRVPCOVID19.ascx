<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputRVPCOVID19.ascx.vb" Inherits="HCSP.ucInputRVPCOVID19" %>
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
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCOutreachTypeText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, OutreachType%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:radiobuttonlist ID="rblCOutreachType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" CssClass="tableText" Style="min-width:110px;position:relative;top:-3px;left:-9px" />
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCOutreachTypeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panRCHCode" runat="server">
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
    <tr>
        <td style="width: 185px;vertical-align:middle">
            <asp:Label ID="lblRCHCodeText" runat="server" CssClass="tableTitle" Width="160px"
                Height="25px"></asp:Label>
        </td>
        <td style="padding-bottom: 3px">
            <asp:TextBox ID="txtRCHCode" runat="server" Width="100" MaxLength="6" AutoPostBack="true" style="position:relative;left:-4px" />
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
    <asp:Panel ID="panOutreachCode" runat="server">
    <tr>
        <td style="width: 185px;vertical-align:middle">
            <asp:Label ID="lblOutreachCodeText" runat="server" CssClass="tableTitle" Width="160px"
                Height="25px"></asp:Label>
        </td>
        <td style="padding-bottom: 3px">
            <asp:TextBox ID="txtOutreachCode" runat="server" Width="100" MaxLength="6" AutoPostBack="true" style="position:relative;left:-4px;top:-2px" />
            <asp:Image ID="imgOutreachCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-4px" />
            <asp:ImageButton ID="btnSearchOutreach" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                ImageAlign="AbsMiddle" style="position:relative;top:-2px" />
            <asp:Label ID="lblOutreachCode" runat="server" CssClass="tableText" Style="display: none" />
        </td>
    </tr>
    <tr>
        <td style="width: 185px;vertical-align:top">
            <asp:Label ID="lblOutreachNameText" runat="server" CssClass="tableTitle" Width="160px" Height="25px" />
        </td>
        <td style="padding-bottom: 3px">
            <asp:Label ID="lblOutreachName" runat="server" CssClass="tableText" style="position:relative;left:-3px;top:-2px" />
            <asp:Label ID="lblOutreachNameChi" runat="server" CssClass="tableTextChi" style="position:relative;left:-3px;top:-2px" />
        </td>
    </tr>
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCMainCategoryText" runat="server" CssClass="tableTitle" Height="25px" Text ="<%$ Resources:Text, Category%>" />
            <asp:Image ID="imgCCategoryInfo" runat="server" AlternateText="<%$ Resources:Text, Category%>" ImageUrl="<%$ Resources:ImageUrl, Infobtn %>" 
                style="vertical-align:top;position:relative;top:0px" onclick="javascript:showCategoryInfo()" onmouseover="this.style.cursor='pointer'" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCMainCategoryCovid19" runat="server" CssClass="selectpicker1" AutoPostBack="false" Style="width:660px;position:relative;top:-3px;left:-5px" />
                        <asp:TextBox ID="txtCMainCategory" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCMainCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCSubCategoryText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, SubCategory%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCSubCategoryCovid19" runat="server" CssClass="selectpicker2" AutoPostBack="false" Style="width:660px;position:relative;top:-3px;left:-5px" />
                        <asp:TextBox ID="txtCSubCategory" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCSubCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                </tr>
            </table>
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
