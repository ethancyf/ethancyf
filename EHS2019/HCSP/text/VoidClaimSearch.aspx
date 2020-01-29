<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/text/ClaimVoucher.Master" Codebehind="VoidClaimSearch.aspx.vb" Inherits="HCSP.VoidClaimSearch" Title="<%$Resources:Title, ReimbursementClaimTransMgt%>" EnableViewState="True" %>

<%@ Register Src="../UIControl/ucInputTips.ascx" TagName="ucInputTips" TagPrefix="uc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="../UIControl/DocTypeText/ucClaimSearch.ascx" TagName="ucClaimSearch" TagPrefix="uc1" %>
<%@ OutputCache Duration="1" Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <cc1:TextOnlyMessageBox ID="udcMsgBoxErr" runat="server" ></cc1:TextOnlyMessageBox>
    <asp:MultiView ID="mvVoidClaimTransaction" runat="server" EnableTheming="True">
        <asp:View ID="vSelectSearchType" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Button ID="btnSearchTranType" runat="server" SkinID="TextOnlyVersionLinkButton" Text="<%$ Resources:Text, SearchByTransactionNo %>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSearchTranTypeEHealthAccount" runat="server" SkinID="TextOnlyVersionLinkButton" Text="<%$ Resources:Text, SearchByEHealthAccount %>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vSearchTrasnaction" runat="server">
            <asp:Panel ID="pnlSearchTransactionContainer" runat="server" DefaultButton="btnSearchTranSearch">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblSearchTranHeader" runat="server" Text="<%$ Resources:Text, SearchInfo %>" CssClass="tableHeader"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="panSearchTranTransactionNo" runat="server">
                    <table id="tabSearchByTransactionNo" runat="server" cellpadding="0" cellspacing="0" style="width: 100%" visible="true">
                        <tr>
                            <td style="width: 100%">
                                <asp:Label ID="lblSearchTranTransactionNo" runat="server" Text="<%$ Resources:Text, TransactionNo %>" Font-Bold="True" Width="100%"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="height: 24px; width: 100%;">
                                <asp:TextBox ID="txtSearchTranTransactionNo" runat="server" MaxLength="18"></asp:TextBox><asp:Label ID="lblSearchTranNoError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panSearchTranDocType" runat="server">
                    <table runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchTranDocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                <asp:Button ID="btnSearchTranChangeDocType" runat="server" SkinID="TextOnlyVersionLinkButton" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSearchTranDocType" runat="server" CssClass="tableText" Width="100%"></asp:Label></td>
                        </tr>
                    </table>
                    <uc1:ucClaimSearch ID="udcSearchTranClaimSearch" runat="server"></uc1:ucClaimSearch>
                </asp:Panel>
                <asp:Button ID="btnSearchTranCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                <asp:Button ID="btnSearchTranSearch" runat="server" Text="<%$ Resources:AlternateText, SearchBtn %>" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vSelectDocType" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblStepSelectDocTypeDocTypeText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <cc1:DocumentTypeRadioButtonGroupText ID="ucDocumentTypeRadioButtonGroupText" runat="server" AutoPostBack="false" PracticeTextCss="tableText" Width="100%" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnStepSelectDocTypeSelect" runat="server" Text="" /></asp:View>
        <asp:View ID="vInputTips" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblInputTipTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputTips %>"></asp:Label>
                    </td>
                </tr>            
                <tr>
                    <td>
                        <uc2:ucInputTips ID="udcInputTips" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnInputTipBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
