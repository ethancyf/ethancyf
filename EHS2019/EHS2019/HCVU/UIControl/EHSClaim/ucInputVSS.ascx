<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVSS.ascx.vb" Inherits="HCVU.ucInputVSS" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputVSSDA.ascx" TagName="ucInputVSSDA" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputVSSPID.ascx" TagName="ucInputVSSPID" TagPrefix="uc2" %>

    <asp:Panel ID="panVSSCategory" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="top" style="width: 205px">
                    <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle" Height="25px"
                        Width="160px"></asp:Label></td>
                <td valign="top" >
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rbCategorySelection" runat="server" AutoPostBack="True"
                                    CssClass="tableText" Style="position:relative;top:-3px;left:-10px">
                                </asp:RadioButtonList></td>
                            <td valign="top">
                                <asp:Image ID="imgCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                        </tr>
                    </table>
                    <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="panVSSCategoryInput" runat="server">
        <asp:MultiView ID="mvCategory" runat="server" ActiveViewIndex="0" >
            <asp:View ID="vwDefault" runat="server" />

            <asp:View ID="vwPW" runat="server" />
                    
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
                    
            <asp:View ID="vwElders" runat="server" />
                    
            <asp:View ID="vwPID" runat="server">
                <uc2:ucInputVSSPID id="ucInputVSSPID" runat="server" Visible="True" EnableViewState="True"/>
            </asp:view>
                    
            <asp:View ID="vwDA" runat="server">
                <uc1:ucInputVSSDA id="ucInputVSSDA" runat="server" Visible="True" EnableViewState="True"/>
            </asp:view>

            <asp:View ID="vwAdult" runat="server" />

        </asp:MultiView>
    </asp:Panel>

    <asp:Panel ID="panVSSPlaceOfVaccination" runat="server" Visible="false">
        <table style="border-collapse: collapse; border-spacing:0px">
            <tr id="trPlaceOfVaccination" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 205px; height: 22px;">
                    <asp:Label ID="lblPlaceOfVaccinationTitle" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                    <asp:DropDownList ID="ddlPlaceOfVaccination" runat="server" Width="430px" AutoPostBack="true" style="position:relative;left:-6px"></asp:DropDownList>
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

    <cc1:ClaimVaccineInput ID="udcClaimVaccineInputVSS" runat="server" CssTableText ="tableText" CssTableTitle ="tableTitle"/>

    <asp:Panel ID="panVSSRecipientCondition" runat="server" Visible="false">
        <table style="border-collapse: collapse; border-spacing:0px">
            <tr id="trRecipientCondition" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 204px; height: 22px;padding: 5px 0px 0px 5px">
                    <asp:Label ID="lblRecipientConditionTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RecipientCondition%>" />
<%--                    <asp:ImageButton ID="ImgBtnRecipientConditionHelp" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, HelpBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, HelpIconBtn %>" Visible="true" Style="position:relative;top:2px"/>--%>
                </td>
                <td style="vertical-align:top;padding: 5px 0px 0px 0px;">
                      <asp:CheckBox ID="chkRecipientCondition" runat="server" AutoPostBack="true" Visible="false" Enabled="false" CssClass="tableText" Style="position:relative;left:-2px"></asp:CheckBox>
                      <asp:RadioButtonList ID="rblRecipientCondition" runat="server" AutoPostBack="true" Visible="false" Enabled="false" CssClass="inline-list" RepeatColumns="2"></asp:RadioButtonList>
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgRecipientConditionError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" Style="position:relative;left:-7px;top:3px"/>
                </td>
            </tr>
        </table>
    </asp:Panel>