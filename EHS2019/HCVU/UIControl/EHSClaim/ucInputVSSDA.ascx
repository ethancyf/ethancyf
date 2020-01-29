<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVSSDA.ascx.vb" Inherits="HCVU.ucInputVSSDA" %>

<asp:Panel ID="panVSSDADocumentaryProof" runat="server">
    <table style="border-collapse: collapse; border-spacing:0px">
          <tr id="trDocumentaryProof" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 205px; height: 22px;">
                    <asp:Label ID="lblDocumentaryProofTitle" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                      <asp:CheckBox ID="chkDocumentaryProof" runat="server" AutoPostBack="false" Visible="false" CssClass="tableText" Style="position:relative;left:-9px"></asp:CheckBox>
                      <asp:DropDownList ID="ddlDocumentaryProof" runat="server" Width="430px" AutoPostBack="false" Visible="false" Style="position:relative;left:-7px"></asp:DropDownList>
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgDocumentaryProofError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
          </tr>
    </table>
</asp:Panel>