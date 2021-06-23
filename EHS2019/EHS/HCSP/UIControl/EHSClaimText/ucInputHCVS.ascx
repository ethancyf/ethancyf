<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHCVS.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucInputHCVS" %>
<table cellpadding="0" cellspacing="0" class="textVersionTable">
    <asp:Panel ID="panDHCRelatedService" runat="server" Visible="true">        
        <tr id="trDHCRelatedService" runat="server">
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDHCRelatedServiceTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DHCRelatedService%>" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblDHCRelatedServiceRead" runat="server" cellpadding="0" cellspacing="0" style="display: none">
                    <tr>
                        <td>
                            <asp:Label ID="lblDHCRelatedService" runat="server" CssClass="tableText"></asp:Label>
                            <asp:Label ID="lblDHCRelatedServiceName" runat="server" CssClass="tableText"></asp:Label><!-- CRE20-006 DHC Intergation -->
                        </td>
                    </tr>
                </table>                           
                <table id="tblDHCRelatedServiceWrite" runat="server" cellpadding="0" cellspacing="0" style="display: block" border="0">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkDHCRelatedService" runat="server" AutoPostBack="True" CssClass="tableText" Style="position:relative;left:-2px"></asp:CheckBox>
                               <asp:Label ID="lblDHCDistrictCode" runat="server" CssClass="tableText"></asp:Label>
                               <asp:Label ID="lblDHCDistrict" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, District%>" /><asp:Label ID="ErrDistrictCode" runat="server" CssClass="validateFailText" Text="*"
                            Visible="False" />
                            <asp:RadioButtonList ID="rbDHCDistrictCode" runat="server" AutoPostBack="true" RepeatDirection="Vertical" enabled="false" ></asp:RadioButtonList>
                            
                        </td>
                    </tr>
                </table>
            </td>   
        </tr>
    </asp:Panel>
    <tr>
        <td>
            <asp:Label ID="lblAvailableVoucherText" runat="server" CssClass="tableTitle"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <asp:Panel ID="pnlAvailableQuota" runat="server" Visible="false">
    <tr>
        <td>
            <table style="padding:0px;border-collapse:collapse" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblAvailableQuotaText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAvailableQuota" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblAvailableQuotaUpTo" runat="server" CssClass="tableText"></asp:Label>                        
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table style="padding:0px;border-collapse:collapse" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblMaximumVoucherAmountText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMaximumVoucherAmount" runat="server" CssClass="tableText"></asp:Label>                   
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:panel>
    <tr>
        <td>
            <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
     <tr>
      <td>
              <table cellpadding="0" cellspacing="0" class="textVersionTable" id="tblVoucherRedeem" runat="server">
                <tr> 
                        <td style="width:220px"><%--
                            <asp:RadioButtonList ID="rbVoucherRedeem" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </td>
                        <td valign="bottom">--%>   
                         <asp:Label ID="Label1" runat="server" CssClass="tableText">$</asp:Label> 
                            <asp:TextBox ID="txtRedeemAmount" runat="server" AutoCompleteType="Disabled" AutoPostBack="false"
                        Width="42px"></asp:TextBox>
                            <asp:Label ID="ErrVoucherRedeem" runat="server" CssClass="validateFailText" Text="*"
                                Visible="False"></asp:Label>
                      </td>
                    </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="panClaimDetailNormal" runat="server">
                <table cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <%-- <asp:Label ID="lblNoOfvoucherredeemed" runat="server" CssClass="tableText"></asp:Label>--%>
                            <asp:Label ID="lblNomarlTotalAmount" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr id="trCoPaymentFee" runat="server">
        <td>
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr id="trCoPaymentFeeText" runat="server">
                    <td>
                        <asp:Label ID="lblCoPaymentFeeText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="tblCopaymentFeeRead" runat="server" cellpadding="0" cellspacing="0" style="display: none">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCoPaymentFee" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table id="tblCopaymentFeeWrite" runat="server" cellpadding="0" cellspacing="0" style="display: block">
                            <tr>
                                <td>
                                    <asp:Label ID="lblDollarSign" runat="server" CssClass="tableText">$</asp:Label>
                                    <asp:TextBox ID="txtCoPaymentFee" runat="server" MaxLength="4" Width="42px" AutoCompleteType="disabled"></asp:TextBox>
                                    <asp:Label ID="ErrCoPaymentFee" runat="server" CssClass="validateFailText" Text="*"
                                        Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblTotalReasonVisitText" runat="server" CssClass="tableTitle"></asp:Label>
                        <asp:Button ID="btnSelectReasonForVisit" runat="server" Text="<%$ Resources: AlternateText, Edit %>"
                            SkinID="TextOnlyVersionLinkButton" />
                        <asp:Label ID="ErrVisitReason" runat="server" CssClass="validateFailText" Text="*"
                            Visible="False" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table id="tblNoReasonForVisit" cellpadding="0" cellspacing="0" runat="server" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblNoReasonForVisit" runat="server" CssClass="tableText" Text="<%$ Resources:AlternateText, NoReasonForVisitSelected %>"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table id="tblReasonForVisit" class="ReasonForVisitTable" cellpadding="0" cellspacing="1" runat="server">
    <tr id="trReasonForVisitPrincipal" runat="Server">
        <td>
            <table cellpadding="3" cellspacing="0" id="tblReasonForVisitPrincipal" class="ReasonForVisitTableHeading" runat="server">
                <tr>
                    <td runat="server" class="VR">
                        <b>
                            <asp:Label ID="lblPrimaryText" runat="server" CssClass="tableText"></asp:Label>
                        </b>
                    </td>
                </tr>
                <tr id="trReasonForVisitGroupPrincipal" runat="server" class="ReasonForVisitGroupTable1">
                    <td>
                        <table cellpadding="2" cellspacing="0" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL1" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL2" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trReasonForVisitSecondary" runat="server">
        <td>
            <table cellpadding="3" cellspacing="0" id="tblReasonForVisitSecondary" class="ReasonForVisitTableHeading" runat="server">
                <tr>
                    <td runat="server" class="VR">
                        <b>
                            <asp:Label ID="lblSecondaryText" runat="server" CssClass="tableText"></asp:Label>
                        </b>
                    </td>
                </tr>
                <tr id="trReasonForVisitGroupSecondary1" runat="server" class="ReasonForVisitGroupTable1">
                    <td id="tdReasonForVisitGroupSecondary1" runat="server">
                        <table cellpadding="2" cellspacing="0" runat="server" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL1_S1" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL2_S1" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr  id="trReasonForVisitGroupSecondary2" runat="server" class="ReasonForVisitGroupTable2" >
                    <td id="tdReasonForVisitGroupSecondary2" runat="server">
                        <table cellpadding="2" cellspacing="0" runat="server" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL1_S2" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL2_S2" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr  id="trReasonForVisitGroupSecondary3" runat="server"  class="ReasonForVisitGroupTable1">
                    <td id="tdReasonForVisitGroupSecondary3" runat="server">
                        <table cellpadding="2" cellspacing="0" runat="server" class="rbSelectRFVGroupDisplay">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL1_S3" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReasonForVisitL2_S3" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
