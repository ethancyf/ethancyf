<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputRVP.ascx.vb" Inherits="HCVU.ucInputRVP" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputRVPCOVID19.ascx" TagName="ucInputRVPCOVID19" TagPrefix="uc1" %>
<%--PS: Cannot remove panRVPDetail panel, for fix viewstate problem--%>
<asp:Panel ID="panRVPDetail" runat="server">
    <table runat="server" id="tblRVPDetail" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="2" valign="top">
                <asp:Panel ID="panClaimCategory" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top" style="width: 200px">
                                <asp:Label ID="lblCategoryText" runat="server" Height="25px" Width="160px" />
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rbCategorySelection" runat="server" AutoPostBack="True"
                                                CssClass="tableText" Style="position:relative;top:-3px;left:-5px">
                                            </asp:RadioButtonList></td>
                                        <td valign="top">
                                            <asp:Image ID="imgCategoryError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblCategory" runat="server" CssClass="tableText" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="panRCHCode" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="middle" style="width: 200px">
                    <asp:Label ID="lblRCHCodeText" runat="server" Width="160px" Height="25px" />
                </td>
                <td style="padding-bottom: 3px">
                    <asp:TextBox ID="txtRCHCodeText" runat="server" Width="100" MaxLength="6" AutoPostBack="true"></asp:TextBox>
                    <asp:Image ID="imgRCHCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                        ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                    <asp:ImageButton ID="btnSearchRCH" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                        ImageAlign="AbsMiddle" />
                    <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText" Style="display: none"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" style="width: 200px">
                    <asp:Label ID="lblRCHNameText" runat="server" Width="160px"
                        Height="25px"></asp:Label>
                </td>
                <td style="padding-bottom: 3px">
                    <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblRCHNameChi" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panRVPCategoryInput" runat="server">
        <asp:MultiView ID="mvCategory" runat="server" ActiveViewIndex="0" >
            <asp:View ID="vwDefault" runat="server" />

            <asp:View ID="vmCOVID19" runat="server">
                <uc1:ucInputRVPCOVID19 id="ucInputRVPCOVID19" runat="server" Visible="True" EnableViewState="True"/>
            </asp:view>

        </asp:MultiView>
    </asp:Panel>
</asp:Panel>

<cc1:ClaimVaccineInput ID="udcClaimVaccineInputRVP" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />

<asp:Panel ID="panRVPRecipientCondition" runat="server" Visible="false">
    <table style="border-collapse: collapse; border-spacing:0px">
        <tr id="trRecipientCondition" runat="server">
            <td class="tableCellStyle" style="vertical-align:top ; width: 204px; height: 22px;padding: 5px 0px 0px 5px">
                <asp:Label ID="lblRecipientConditionTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RecipientCondition%>" />
<%--                <asp:ImageButton ID="ImgBtnRecipientConditionHelp" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, HelpBtn%>"
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