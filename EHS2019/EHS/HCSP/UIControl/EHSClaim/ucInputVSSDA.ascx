<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVSSDA.ascx.vb" Inherits="HCSP.ucInputVSSDA" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<asp:Panel ID="panVSSDADocumentaryProof" runat="server">
    <table style="border-collapse: collapse; border-spacing:0px">
          <tr id="trDocumentaryProof" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 205px; height: 22px;">
                    <asp:Label ID="lblDocumentaryProofTitle" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                      <asp:CheckBox ID="chkDocumentaryProof" runat="server" AutoPostBack="false" Visible="false" CssClass="tableText" Style="position:relative;left:-2px"></asp:CheckBox>
                      <asp:DropDownList ID="ddlDocumentaryProof" runat="server" Width="430px" AutoPostBack="true" Visible="false"></asp:DropDownList>
                  
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgDocumentaryProofError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
          </tr>
    </table>
</asp:Panel>
 <!--CRE20-009 set panel to show the two checkbox [Start][Nichole]-->
    <asp:Panel ID="panVSSDAConfirm" runat="server" visible="false" >
        <table style="border-collapse: collapse; border-spacing:0px">
            <tr id="tr1" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width:199px; height: 22px;">
                    &nbsp;
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                    <table style="border-collapse: collapse; border-spacing:0px">
                        <tr><td style="width: 5px; vertical-align:top ">
                     <asp:CheckBox ID="chkDocProofCSSA" TabIndex="1" runat="server"      AutoPostBack="True"></asp:CheckBox></td>
                            <td style="text-align : justify ">
                    <asp:Label ID="lblDocProofCSSA" runat="server" CssClass="tableText"  AssociatedControlId="chkDocProofCSSA" Text="<%$ Resources:Text,  ProvidedInfoCSSA%>"></asp:Label>
                       </td> </tr>
                    </table>
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgVSSDAConfirmCSSAError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
            </tr>
            
            <tr id="tr2" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 199px; height: 22px;">
                    &nbsp;
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                     <table style="border-collapse: collapse; border-spacing:0px">
                        <tr><td style="width: 5px; vertical-align:top ">
                     <asp:CheckBox ID="chkDocProofAnnex" TabIndex="1" runat="server"  AutoPostBack="True"></asp:CheckBox>
                            </td>
                          <td style="text-align : justify ">
                    <asp:Label ID="lblDocProofAnnex" runat="server" CssClass="tableText" AssociatedControlId="chkDocProofAnnex" Text="<%$ Resources:Text,  ProvidedInfoAnnex%>"></asp:Label>
                       </td> </tr>
                    </table>

                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgVSSDAConfirmAnnexError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
        </table>
    </asp:Panel>
<!--CRE20-009 set panel to show the two checkbox [End][Nichole] -->