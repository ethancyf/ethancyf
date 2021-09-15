<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVSSCOVID19.ascx.vb" Inherits="HCVU.ucInputVSSCOVID19" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Panel ID="panOutreachCode" runat="server" Visible="false">
    <table style="padding:0px;border-spacing:0px;border-collapse:collapse;position:relative;left:-1px">
        <tr>
            <td style="width: 205px;vertical-align:middle">
                <asp:Label ID="lblOutreachCodeText" runat="server" Width="160px" Height="25px" />
            </td>
            <td style="padding-bottom: 3px">
                <asp:TextBox ID="txtOutreachCode" runat="server" Width="100" MaxLength="10" AutoPostBack="true" style="position:relative;left:-5px;top:-2px" />
                <asp:Image ID="imgOutreachCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-2px" />
                <asp:ImageButton ID="btnSearchOutreach" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                    ImageAlign="AbsMiddle" style="position:relative;top:-2px" />
                <asp:Label ID="lblOutreachCode" runat="server" CssClass="tableText" Style="display: none" />
            </td>
        </tr>
        <tr>
            <td style="width: 205px;vertical-align:top">
                <asp:Label ID="lblOutreachNameText" runat="server" Width="160px" Height="25px" />
            </td>
            <td style="padding-bottom: 3px">
                <asp:Label ID="lblOutreachName" runat="server" CssClass="tableText" style="position:relative;left:-6px;top:-2px" />
                <asp:Label ID="lblOutreachNameChi" runat="server" CssClass="tableTextChi" style="position:relative;left:-6px;top:-2px" />
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
                <asp:Label ID="lblCMainCategoryText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, Category%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:dropdownlist ID="ddlCMainCategory" runat="server" AutoPostBack="true" Style="width:660px;position:relative;top:-3px;left:-2px" />
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
            <td style="width:200px;vertical-align:top">
                <asp:Label ID="lblCSubCategoryText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, SubCategory%>" />
            </td>
            <td style="vertical-align:top">
                <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                    <tr>
                        <td style="vertical-align:top">
                            <asp:dropdownlist ID="ddlCSubCategory" runat="server" AutoPostBack="false" Style="width:660px;position:relative;top:-3px;left:-2px" />
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
<%--        <asp:Panel ID ="panDose" runat="server" Visible ="false">
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
        </asp:Panel>--%>
        <tr id="trCContactNo" runat="server">
            <td style="width:200px;height:25px;vertical-align:top">
                <asp:Label ID="lblCContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2%>" Width="160px" />
            </td>
            <td style="height:25px;vertical-align:top">
                <asp:textbox ID="txtCContactNo" runat="server" MaxLength="8" style="position:relative;left:-1px;top:-2px" Width="100px"/>
                <asp:ImageButton ID="imgCContactNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
                <asp:Label ID="lblStep2aContactNoRecommendation" runat="server" Text="<%$ Resources:Text, ProvideContactNoWithSMS%>" style="font-size:14px" />
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