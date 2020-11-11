<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputEHSClaim.ascx.vb" Inherits="HCSP.ucInputEHSClaim" %>

<%@ Register Src="~/UIControl/EHSClaim/ucInputHCVS.ascx" TagName="ucInputHCVS" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputHCVSChina.ascx" TagName="ucInputHCVSChina" TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputSSSCMC.ascx" TagName="ucInputSSSCMC" TagPrefix="uc3" %>

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <uc1:ucInputHCVS id="ucInputHCVS" runat="server"  Visible="false"  EnableViewState="true"/>
            <uc2:ucInputHCVSChina id="ucInputHCVSChina" runat="server"  Visible="false"  EnableViewState="true"/>
            <uc3:ucInputSSSCMC id="ucInputSSSCMC" runat="server"  Visible="false"  EnableViewState="true"/>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
</table>
