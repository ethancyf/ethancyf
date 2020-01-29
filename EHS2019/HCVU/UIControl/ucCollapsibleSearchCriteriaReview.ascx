<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucCollapsibleSearchCriteriaReview.ascx.vb" Inherits="HCVU.ucCollapsibleSearchCriteriaReview" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<cc1:CollapsiblePanelExtender ID="collapsiblePanelExtender" runat="server"
                              CollapseControlID="imgCollapsiblePanelController" ExpandControlID="imgCollapsiblePanelController" ImageControlID="imgCollapsiblePanelController"
                              AutoCollapse="false" AutoExpand="false" Collapsed="true" CollapsedSize="0" ExpandDirection="Vertical" ScrollContents="false"
                              CollapsedImage="<%$ Resources:ImageUrl, ShowCriteriaBtn %>" ExpandedImage="<%$ Resources:ImageUrl, HideCriteriaBtn %>">
</cc1:CollapsiblePanelExtender>

<asp:Panel ID="panSearchCriteriaReviewHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td id="tdHeaderText" runat="server" visible="true">
                <div class="headingText" style="vertical-align:middle">
                    <asp:Label ID="lblSearchCriteriaReviewHeader" runat="server" Text="<%$ Resources:Text, SearchResults %>"></asp:Label>
                </div>
            </td>
            <td style="padding-left:5px">
                <asp:Image ID="imgCollapsiblePanelController" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShowCriteriaBtn %>" onmouseover="this.style.cursor='pointer';"/>
            </td>
        </tr>
    </table>
</asp:Panel>
