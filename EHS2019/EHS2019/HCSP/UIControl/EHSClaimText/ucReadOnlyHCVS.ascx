<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHCVS.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyHCVS" %>

<asp:Panel ID="panDHCRelatedService" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblDHCRelatedServiceText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DHCRelatedService%>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDHCRelatedService" runat="server" CssClass="tableText"></asp:Label>
            </td>   
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panClaimDetailNormal" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td>
                <asp:Label ID="lblRedeemAmountText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRedeemAmount" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panClaimDetail" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td>
                    <asp:Label ID="lblRedeemAmountDetailText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherAvailText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherAvail" runat="server" CssClass="tableText" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRedeem" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRemainText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRemain" runat="server" CssClass="tableText" Width="100%"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>
<table id="tblCoPaymentFee" runat="server" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="lblCoPaymentFeeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <div style="display: block">
                <asp:Label ID="lblCoPaymentFee" runat="server" CssClass="tableText"></asp:Label>
            </div>
        </td>
    </tr>
</table>
<table cellspacing="0" cellpadding="0" class="textVersionTable">
    <tr>
        <td>
            <asp:Label ID="lblReasonVisitText" runat="server" CssClass="tableTitle" Width="200px"></asp:Label></td>
    </tr>
</table>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableCellStyle" style="width: 175px">
            <div style="display: block">
                <asp:Label ID="lblReasonForVisit" runat="server" CssClass="tableText" ForeColor="Red"></asp:Label></div>
        </td>
    </tr>
</table>
<table id="tblRFV" class="ReasonForVisitTable" cellpadding="0" cellspacing="1" runat="server">
    <tr id="trRFVPrincipal" runat="Server">
        <td>
            <table id="tblRFVPrincipal" cellpadding="3" cellspacing="0" class="ReasonForVisitTableHeading"
                runat="server">
                <tr id="trRFVPrincipalHeading" runat="Server">
                    <td class="VR">
                        <asp:Label ID="lblRFVPrincipalText" runat="server" CssClass="tableText" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr id="trRFVPrincipalContent" runat="Server" class="ReasonForVisitGroupTable1">
                    <td>
                        <table cellpadding="2" cellspacing="0" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVPrincipalL1" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVPrincipalL2" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trRFVSecondary" runat="Server">
        <td>
            <table id="tblRFVSecondary" cellpadding="3" cellspacing="0" class="ReasonForVisitTableHeading"
                runat="server">
                <tr id="trRFVSecondaryHeading" runat="Server">
                    <td class="VR">
                        <asp:Label ID="lblRFVSecondaryText" runat="server" CssClass="tableText" Font-Bold="true">></asp:Label>
                    </td>
                </tr>
                <tr id="trRFVSecondary1Content" runat="Server" class="ReasonForVisitGroupTable1">
                    <td id="trRFVSecondary1ContentOption">
                        <table cellpadding="2" cellspacing="0" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVSecondary1L1" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVSecondary1L2" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trRFVSecondary2Content" runat="Server" class="ReasonForVisitGroupTable2">
                    <td id="trRFVSecondary2ContentOption">
                        <table cellpadding="2" cellspacing="0" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVSecondary2L1" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVSecondary2L2" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trRFVSecondary3Content" runat="Server" class="ReasonForVisitGroupTable1"
                    style="border-bottom: dashed 1px gray">
                    <td id="trRFVSecondary3ContentOption">
                        <table cellpadding="2" cellspacing="0" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVSecondary3L1" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRFVSecondary3L2" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
