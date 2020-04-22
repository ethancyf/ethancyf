<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputEVSS.ascx.vb"
    Inherits="HCSP.ucInputEVSS" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table runat="server" id="tblEVSSDetail" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="padding-bottom: 3px">
            <cc1:ClaimVaccineInput ID="udcClaimVaccineInputEVSS" runat="server" CssTableText="tableText"
                CssTableTitle="tableTitle" />
        </td>
    </tr>
</table>
