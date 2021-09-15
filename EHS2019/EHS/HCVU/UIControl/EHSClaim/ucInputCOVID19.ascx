<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputCOVID19.ascx.vb" Inherits="HCVU.ucInputCOVID19" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Panel ID="panCOVID19Category" runat="server">
    <table style="padding:0px;border-spacing:0px;border-collapse:collapse;padding-top:2px">
        <tr>
            <td style="vertical-align:top;width:205px">
                <asp:Label ID="lblCategoryText" runat="server" Height="25px"
                    Width="160px"></asp:Label></td>
            <td style="vertical-align:top" >
                <table style="padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rbCategorySelection" runat="server" AutoPostBack="True"
                                CssClass="tableText" Style="position:relative;top:-3px;left:-10px">
                            </asp:RadioButtonList></td>
                        <td style="vertical-align:top">
                            <asp:Image ID="imgCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                    </tr>
                </table>
                <asp:Label ID="lblCategory" runat="server" CssClass="tableText" style="position:relative;left:-5px;top:-3px"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panCOVID19Detail" runat="server">
    <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
        <asp:Panel ID ="panCategory" runat="server" Visible ="false">
        <tr>
            <td style="width:200px;vertical-align:top">
                <asp:Label ID="lblCCategoryText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, Category%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:dropdownlist ID="ddlCCategoryCovid19" runat="server" AutoPostBack="false" Style="width:300px;position:relative;top:-3px;left:-2px" />
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
            <td style="width:200px;vertical-align:top">
                <asp:Label ID="lblCVaccineBrandText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, Vaccines%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:dropdownlist ID="ddlCVaccineBrandCovid19" runat="server" AutoPostBack="true" Style="width:660px;position:relative;top:-3px;left:-2px" />
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
                            <asp:dropdownlist ID="ddlCVaccineLotNoCovid19" runat="server" AutoPostBack="false" Style="width:300px;position:relative;top:-3px;left:-2px" />
                            <asp:TextBox ID="txtCVaccineLotNo" runat="server"  AutoPostBack="false"  Style="display:none;"/>
                        </td>
                        <td style="vertical-align:top">
                            <asp:Image ID="imgCVaccineLotNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:Panel ID ="panDose" runat="server" Visible ="false">
        <tr>
            <td style="width:200px;vertical-align:top">
                <asp:Label ID="lblCDoseText" runat="server" Height="20px" Width="160px" Text ="<%$ Resources:Text, DoseSeq%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:dropdownlist ID="ddlCDoseCovid19" runat="server" AutoPostBack="false" Style="width:300px;position:relative;top:-3px;left:-2px" />
                        </td>
                        <td style="vertical-align:top">
                            <asp:Image ID="imgCDoseError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </asp:Panel>
        <tr id="trCContactNo" runat="server" visible="false">
            <td style="width:200px;height:25px;vertical-align:top">
                <asp:Label ID="lblCContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2%>" CssClass="tableTitle" Width="160px" />
            </td>
            <td style="height:25px;vertical-align:top">
                <asp:textbox ID="txtCContactNo" runat="server" MaxLength="8" style="position:relative;left:-1px" Width="100px"/>
                <asp:ImageButton ID="imgCContactNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
                <cc2:FilteredTextBoxExtender ID="fteCContactNo" runat="server" TargetControlID="txtCContactNo"
                                            FilterType="Numbers" />
            </td>
        </tr>
        <tr>
            <td style="width:200px;height:25px;vertical-align:top;padding-bottom:5px">
                <asp:Label ID="lblCRemarkText" runat="server" Text="<%$ Resources:Text, Remarks%>" Width="160px" />
            </td>
            <td style="height:25px;vertical-align:top;padding-bottom:5px">
                <asp:textbox ID="txtCRemark" runat="server" MaxLength="200" style="position:relative;left:-1px" Width="660px"/>
                <asp:ImageButton ID="imgCRemarkError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
            <cc2:FilteredTextBoxExtender ID="fteCRemark" runat="server" TargetControlID="txtCRemark"
                   FilterMode="InvalidChars"  InvalidChars="|\"></cc2:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr id="trJoinEHRSS" runat="server">
            <td style="width:200px;height:25px;vertical-align:top;padding-bottom:5px">
                <asp:Label ID="lblCJoinEHRSSText" runat="server" Text="<%$ Resources:Text, JoinEHRSS%>" Width="160px" />
            </td>
            <td style="height:25px;vertical-align:top;padding-bottom:5px">
                <asp:checkbox ID="chkCJoinEHRSS" runat="server" AutoPostBack="false" style="position:relative;left:-5px"/>
            </td>
        </tr>
        <tr id="trNonLocalRecoveredHistory" runat="server">
            <td style="width:200px;height:25px;vertical-align:top;padding-bottom:5px">
                <asp:Label ID="lblCNonLocalRecoveredHistory" runat="server" Text="<%$ Resources:Text, NonLocalRecoveredHistory%>" />
            </td>
            <td style="height:25px;vertical-align:top;padding-bottom:5px">
                <asp:checkbox ID="chkCNonLocalRecoveredHistory" runat="server" AutoPostBack="false" style="position:relative;left:-5px"/>
            </td>
        </tr>
    </table>
</asp:Panel>

<cc1:ClaimVaccineInput ID="udcClaimVaccineInputCOVID19" runat="server" CssTableText ="tableText" CssTableTitle ="tableTitle"/>

<div style="padding-bottom:10px" />
