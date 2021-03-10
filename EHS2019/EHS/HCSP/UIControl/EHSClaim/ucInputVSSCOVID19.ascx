<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVSSCOVID19.ascx.vb" Inherits="HCSP.ucInputVSSCOVID19" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
    <asp:Panel ID ="panBooth" runat="server" Visible ="false">
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCBoothText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text="<%$ Resources:Text, Booth%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCBooth" runat="server" AutoPostBack="true" Style="width:300px;position:relative;top:-3px;left:-5px" />
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCBoothError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
    <asp:Panel ID ="panCategory" runat="server" Visible="false">
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblCCategoryText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, Category%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCCategoryCovid19" runat="server" AutoPostBack="false" Style="width:660px;position:relative;top:-3px;left:-5px" />
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
            <asp:Label ID="lblCMainCategoryText" runat="server" CssClass="tableTitle" Height="25px" Width="160px" Text ="<%$ Resources:Text, Category%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <asp:dropdownlist ID="ddlCMainCategoryCovid19" runat="server" AutoPostBack="false" Style="width:660px;position:relative;top:-3px;left:-5px" />
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
                        <asp:dropdownlist ID="ddlCSubCategoryCovid19" runat="server" AutoPostBack="false" Style="width:660px;position:relative;top:-3px;left:-5px" />
                        <asp:TextBox ID="txtCSubCategory" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                    </td>
                    <td style="vertical-align:top">
                        <asp:Image ID="imgCSubCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                </tr>
            </table>
        </td>
    </tr>

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
