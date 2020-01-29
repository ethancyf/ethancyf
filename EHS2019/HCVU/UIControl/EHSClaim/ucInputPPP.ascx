<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputPPP.ascx.vb" Inherits="HCVU.ucInputPPP" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>

    <asp:Panel ID="panPPPCategory" runat="server">
        <table border="0" style="padding:0px;border-collapse:collapse;border-spacing:0px">
            <tr style="vertical-align:top;height:25px">
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

    <asp:Panel ID="panPPPCategoryInput" runat="server">
        <asp:MultiView ID="mvCategory" runat="server" ActiveViewIndex="0" >
            <asp:View ID="vwDefault" runat="server" />
                    
            <asp:View ID="vwChildren" runat="server">
                <asp:Panel ID="panSchoolCode" runat="server" Visible="true">
                    <table border="0" style="padding:0px;border-collapse:collapse;border-spacing:0px">
                        <tr style="vertical-align:top;height:25px">
                            <td class="tableCellStyle" style="vertical-align:middle;width:200px">
                                <asp:Label ID="lblSchoolCodeText" runat="server" CssClass="tableTitle" Height="22px" Style="position:relative;left:-1px"  />
                            </td>
                            <td style="padding-bottom: 3px">
                                <asp:TextBox ID="txtSchoolCode" runat="server" Width="150" MaxLength="30" AutoPostBack="true" Style="position:relative;left:-1px" />
                                <asp:Image ID="imgSchoolCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                    ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                <asp:ImageButton ID="btnSearchSchool" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                                    ImageAlign="AbsMiddle" Style="position:relative;left:-2px"/>
                                <asp:Label ID="lblSchoolCode" runat="server" CssClass="tableText" Style="display: none;position:relative;left:-4px" />
                            </td>
                        </tr>
                        <tr style="vertical-align:top;height:25px">
                            <td class="tableCellStyle" style="vertical-align:middle;width:200px">
                                <asp:Label ID="lblSchoolNameText" runat="server" CssClass="tableTitle" Style="position:relative;left:-1px"  />
                            </td>
                            <td style="padding-bottom: 3px">
                                <asp:Label ID="lblSchoolName" runat="server" CssClass="tableText" Style="position:relative;top:2px" />
                                <asp:Label ID="lblSchoolNameChi" runat="server" CssClass="tableTextChi" Style="position:relative;top:2px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:view>
                    
        </asp:MultiView>
    </asp:Panel>

    <cc1:ClaimVaccineInput ID="udcClaimVaccineInputPPP" runat="server" CssTableText ="tableText" CssTableTitle ="tableTitle"/>
