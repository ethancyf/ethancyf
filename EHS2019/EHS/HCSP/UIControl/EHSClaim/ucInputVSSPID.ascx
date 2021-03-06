<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVSSPID.ascx.vb" Inherits="HCSP.ucInputVSSPID" %>

<asp:Panel ID="panVSSPIDDocumentaryProof" runat="server">
    <table style="border-collapse: collapse; border-spacing:0px">
          <tr id="trDocumentaryProof" runat="server">
                <td class="tableCellStyle" style="vertical-align:top ; width: 205px; height: 22px;">
                    <asp:Label ID="lblDocumentaryProofTitle" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                      <asp:CheckBox ID="chkDocumentaryProof" runat="server" AutoPostBack="true" Visible="false" CssClass="tableText" Style="position:relative;left:-2px"></asp:CheckBox>
                      <asp:DropDownList ID="ddlDocumentaryProof" runat="server" Width="430px" AutoPostBack="true" Visible="false"></asp:DropDownList>
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgDocumentaryProofError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
          </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panPIDInstitutionCode" runat="server" Visible="false">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr style="vertical-align:top;height:22px">
            <td class="tableCellStyle" valign="middle" style="width: 205px">
                <asp:Label ID="lblPIDInstitutionCodeText" runat="server" CssClass="tableTitle" Width="160px"
                    Height="22px"></asp:Label>
            </td>
            <td style="padding-bottom: 3px">
                <asp:TextBox ID="txtPIDInstitutionCode" runat="server" Width="100" MaxLength="6" AutoPostBack="true"></asp:TextBox>
                <asp:Image ID="imgPIDInstitutionCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                <asp:ImageButton ID="btnSearchPID" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                    ImageAlign="AbsMiddle" />
                <asp:Label ID="lblPIDInstitutionCode" runat="server" CssClass="tableText" Style="display: none"></asp:Label></td>
        </tr>
        <tr style="vertical-align:top;height:28px">
            <td class="tableCellStyle" valign="top" style="width: 205px">
                <asp:Label ID="lblPIDInstitutionNameText" runat="server" CssClass="tableTitle" Width="160px"
                    ></asp:Label>
            </td>
            <td style="padding-bottom: 3px">
                <asp:Label ID="lblPIDInstitutionName" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblPIDInstitutionNameChi" runat="server" CssClass="tableTextChi"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>

