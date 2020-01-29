<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputEHSClaimText.ascx.vb" Inherits="HCSP.ucInputEHSClaimText" %>
<%@ Register Src="~/UIControl/EHSClaim/ucInputHCVS.ascx" TagName="ucInputHCVS" TagPrefix="uc1" %>

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <uc1:ucInputHCVS id="ucInputHCVS" runat="server"  Visible="false"  EnableViewState="true"/>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
</table>
