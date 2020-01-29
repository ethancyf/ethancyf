<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputPIDVSS.ascx.vb" Inherits="HCVU.ucInputPIDVSS" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<asp:Panel ID="panPIDVSSDocumentaryProof" runat="server">
    <table style="border-collapse: collapse; border-spacing:0px">
          <tr id="trDocumentaryProof" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 200px; height: 25px;">
                    <asp:Label ID="lblDocumentaryProofTitle" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                      <asp:DropDownList ID="ddlDocumentaryProof" runat="server" Width="290px" AutoPostBack="false"></asp:DropDownList>
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgDocumentaryProofError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
          </tr>
    </table>
</asp:Panel>
<cc1:ClaimVaccineInput ID="udcClaimVaccineInputPIDVSS" runat="server" CssTableText ="tableText" CssTableTitle ="tableTitle"/>