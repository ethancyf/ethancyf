<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputEHSClaim.ascx.vb" Inherits="HCVU.ucInputEHSClaim" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputHCVS.ascx" TagName="ucInputHCVS" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputHCVSChina.ascx" TagName="ucInputHCVSChina" TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputVSS.ascx" TagName="ucInputVSS" TagPrefix="uc3" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputENHVSSO.ascx" TagName="ucInputENHVSSO" TagPrefix="uc4" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputPPP.ascx" TagName="ucInputPPP" TagPrefix="uc5" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputSSSCMC.ascx" TagName="ucInputSSSCMC" TagPrefix="uc6" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputCOVID19.ascx" TagName="ucInputCOVID19" TagPrefix="uc7" %>

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <uc1:ucInputHCVS id="ucInputEHSClaim_HCVS" runat="server" Visible="false" EnableViewState="true" />
            <uc2:ucInputHCVSChina id="ucInputEHSClaim_HCVSChina" runat="server" Visible="false" EnableViewState="true" />
            <uc3:ucInputVSS id="ucInputEHSClaim_VSS" runat="server" Visible="false" EnableViewState="true" />
            <uc4:ucInputENHVSSO id="ucInputEHSClaim_ENHVSSO" runat="server" Visible="false" EnableViewState="true" />
            <uc5:ucInputPPP id="ucInputEHSClaim_PPP" runat="server" Visible="false" EnableViewState="true" />
            <uc6:ucInputSSSCMC id="ucInputEHSClaim_SSSCMC" runat="server" Visible="false" EnableViewState="true" />
            <uc7:ucInputCOVID19 id="ucInputEHSClaim_COVID19" runat="server" Visible="false" EnableViewState="true" />
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
</table>
