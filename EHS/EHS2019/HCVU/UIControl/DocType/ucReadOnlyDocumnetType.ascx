<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyDocumnetType.ascx.vb"
    Inherits="HCVU.ucReadOnlyDocumnetType" %>
<%@ Register Src="ucReadOnlyHKIC.ascx" TagName="ucReadOnlyHKIC" TagPrefix="uc1" %>
<%@ Register Src="ucReadOnlyEC.ascx" TagName="ucReadOnlyEC" TagPrefix="uc2" %>
<%@ Register Src="ucReadOnlyHKBC.ascx" TagName="ucReadOnlyHKBC" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<asp:Label ID="lblInvalidAccount" runat="server" Style="font-size: 20px;" Text="<%$ Resources:Text, NotApplicable %>"></asp:Label>
<asp:ImageButton ID="ibtnShowOriginalAcc" runat="server" ImageUrl="<%$ Resources:ImageUrl, Infobtn_large %>"
    Style="width: 16px; height: 16px" ToolTip="<%$ Resources:Text, TraceRelatedEHAccount %>"
    OnClick="ibtnShowOriginalAcc_Click" />
<asp:Panel ID="pnlShowAccountID" runat="server" Visible="false">
    <table id="tblAccID" runat="server" style="padding-bottom:0px">
        <tr ID="trAccountIDType" runat="server">
            <td style="height:20px;vertical-align:top">
                <asp:Label ID="lblAccountIDTText" runat="server" Text="<%$ Resources:Text, AccountID %>" style="position:relative;top:4px;left:1px" />
            </td>
            <td style="vertical-align: middle">
                <cc2:CustomLinkButton ID="clbtnAccountIDT" runat="server" CssClass="tableText" ForeColor="Blue" OnClick="clbtnAccountID_Click" style="position:relative;top:-2px" />
                <asp:Label ID="lblAccountIDT" runat="server" CssClass="tableText" />
                <asp:Label ID="lblAccountStatusT" runat="server" CssClass="tableText" />
            </td>
            <td>
                <asp:Label ID="lblAccountTypeText" runat="server" Text="<%$ Resources: Text, AccountType %>"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAccountType" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr ID="trAccountID" runat="server">
            <td style="height:20px;vertical-align:top">
                <asp:Label ID="lblAccountIDText" runat="server" Text="<%$ Resources:Text, AccountID %>" style="position:relative;top:4px;left:1px" />
            </td>
            <td colspan="3" style="vertical-align: middle">
                <cc2:CustomLinkButton ID="clbtnAccountID" runat="server" CssClass="tableText" ForeColor="Blue" OnClick="clbtnAccountID_Click" style="position:relative;top:-2px" />
                <asp:Label ID="lblAccountID" runat="server" CssClass="tableText" />
                <asp:Label ID="lblAccountStatus" runat="server" CssClass="tableText" />
            </td>
        </tr>
        <tr ID="trAccountStatus" runat="server">
            <td style="height:20px;vertical-align:top">
                <asp:Label ID="lblEHealthAccountStatusText" runat="server" Text="<%$ Resources: Text, AccountStatus %>" style="position:relative;top:4px;left:1px" />
            </td>
            <td colspan="3">
                <asp:Label ID="lblEHealthAccountStatus" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:PlaceHolder ID="phDocumentType" runat="server"></asp:PlaceHolder>
<asp:Panel ID="panOriginalAcc" runat="server" Style="display: none;">
    <asp:Panel ID="panOriginalAccHeading" runat="server" Style="cursor: move;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 805px">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                </td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblOriginalAccHeading" runat="server" Text="<%$ Resources:Text, OriginaleHealthAccountRecord %>"></asp:Label></td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 805px">
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            </td>
            <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                <asp:Panel ID="panOriginalAccContent" runat="server" Height="210px">
                    <table id="tblInvalidAcc" runat="server">
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblOriginalAccIDText" runat="server" Text="Original Account ID"></asp:Label></td>
                            <td valign="top">
                                <asp:Label ID="lblOriginalAccID" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                    </table>
                    <asp:PlaceHolder ID="phOriginalAccDocumentType" runat="server"></asp:PlaceHolder>
                </asp:Panel>
            </td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            </td>
            <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                <asp:ImageButton ID="ibtnCloseOriginalAcc" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseOriginalAcc_Click" /></td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
            </td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
            </td>
            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                height: 7px">
            </td>
            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                height: 7px">
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="btnHiddenOriginalAcc" Style="display: none" />
<cc1:ModalPopupExtender ID="popupOriginalAcc" runat="server" TargetControlID="btnHiddenOriginalAcc"
    PopupControlID="panOriginalAcc" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panOriginalAccHeading">
</cc1:ModalPopupExtender>
