<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputENHVSSO.ascx.vb" Inherits="HCVU.ucInputENHVSSO" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>

    <asp:Panel ID="panENHVSSOCategory" runat="server">
        <table border="0" style="padding:0px;border-collapse:collapse;border-spacing:0px">
            <tr>
                <td style="vertical-align:top;width:200px">
                    <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle" Height="25px" Style="position:relative;left:-1px" />
                </td>
                <td style="vertical-align:top" >
                    <table border="0" style="padding:0px;border-collapse:collapse;border-spacing:0px">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbCategorySelection" runat="server" AutoPostBack="True"
                                    CssClass="tableText" Style="position:relative;top:-3px;left:-10px" />
                            </td>
                            <td style="vertical-align:top">
                                <asp:Image ID="imgCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblCategory" runat="server" CssClass="tableText" Style="position:relative;top:-2px" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="panENHVSSOCategoryInput" runat="server">
        <asp:MultiView ID="mvCategory" runat="server" ActiveViewIndex="0" >
            <asp:View ID="vwDefault" runat="server" />
                    
            <asp:View ID="vwChildren" runat="server">
                <asp:Panel ID="panReminder" runat="server" Visible="false">
<%--                    <table border="0" cellpadding="0" cellspacing="0" width="800">
                        <tr>
                            <td style="background-attachment: inherit; background-image: url(../Images/others/reminder_topleft.png);
                                width: 30px; background-repeat: no-repeat; height: 30px">
                            </td>
                            <td style="background-image: url(../Images/others/reminder_topmiddle.png); width: 740px;
                                background-repeat: repeat-x; height: 30px">
                            </td>
                            <td style="background-image: url(../Images/others/reminder_topright.png); width: 30px;
                                background-repeat: no-repeat; height: 30px">
                            </td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/others/reminder_left.png); width: 30px;
                                background-repeat: repeat-y">
                            </td>
                            <td style="line-height: 20px; background-color: #f9f9f9; width: 740px;">
                                <asp:Label ID="lblStep2aReminder" runat="server" CssClass="tableText"></asp:Label></td>
                            <td style="background-image: url(../Images/others/reminder_right.png); width: 30px;
                                background-repeat: repeat-y">
                            </td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/others/reminder_bottomleft.png); width: 30px;
                                background-repeat: no-repeat; height: 30px">
                            </td>
                            <td style="background-image: url(../Images/others/reminder_bottommiddle.png); width: 740px;
                                background-repeat: repeat-x; height: 30px">
                            </td>
                            <td style="background-image: url(../Images/others/reminder_bottomright.png); width: 30px;
                                background-repeat: no-repeat; height: 30px">
                            </td>
                        </tr>
                    </table>--%>
                </asp:Panel>
            </asp:view>
                    
        </asp:MultiView>
    </asp:Panel>

    <asp:Panel ID="panENHVSSOPlaceOfVaccination" runat="server" Visible="false">
        <table style="border-collapse: collapse; border-spacing:0px">
            <tr id="trPlaceOfVaccination" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 200px; height: 22px;">
                    <asp:Label ID="lblPlaceOfVaccinationTitle" runat="server" CssClass="tableTitle" Style="position:relative;left:-1px" />
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                    <asp:DropDownList ID="ddlPlaceOfVaccination" runat="server" Width="460px" AutoPostBack="false" />
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgPlaceOfVaccinationError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
            </tr>
            <tr id="trPlaceOfVaccinationOther" runat="server" style="display:none">
                <td class="tableCellStyle" style="vertical-align:top ; width: 205px; height: 22px;" />
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                    <asp:Label ID="lblPlaceOfVaccinationOtherTitle" runat="server" Width="110" CssClass="tableTitle" Text="<%$ Resources:Text, PlaceOfVaccinationOtherPleaseSpecify%>" style="position:relative;left:-6px"></asp:Label>
                    <asp:TextBox ID="txtPlaceOfVaccinationOther" runat="server" Width="312" AutoPostBack="false" MaxLength="255" style="position:relative;left:-6px"/>
                </td>
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgPlaceOfVaccinationErrorOther" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>   
            </tr>
        </table>
    </asp:Panel>

    <cc1:ClaimVaccineInput ID="udcClaimVaccineInputENHVSSO" runat="server" CssTableText ="tableText" CssTableTitle ="tableTitle"/>

