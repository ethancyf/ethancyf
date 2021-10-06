<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputVSSPID.ascx.vb" Inherits="HCVU.ucInputVSSPID" %>

<asp:Panel ID="panVSSPIDDocumentaryProof" runat="server">
    <table style="border-collapse: collapse; border-spacing:0px">
          <tr id="trDocumentaryProof" runat="server" style="vertical-align:top;height:25px">
                <td class="tableCellStyle" style="width: 205px">
                    <asp:Label ID="lblDocumentaryProofTitle" runat="server" />
                </td>
                <td style="vertical-align:top;padding: 0px 0px 0px 0px;">
                      <asp:CheckBox ID="chkDocumentaryProof" runat="server" Width="430px" AutoPostBack="true" Visible="false" CssClass="tableText" Style="position:relative;left:-2px"></asp:CheckBox>
                      <asp:DropDownList ID="ddlDocumentaryProof" runat="server" Width="430px" AutoPostBack="true" Visible="false" Style="position:relative;left:-3px"></asp:DropDownList>
                </td>   
                <td style="vertical-align:top; padding-left: 4px;">
                    <asp:Image ID="imgDocumentaryProofError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false"/>
                </td>
          </tr>
    </table>
</asp:Panel>

<asp:Panel ID="panPIDInstitutionCode" runat="server" Visible="false">
    <table style="border-collapse: collapse; border-spacing:0px">
        <tr style="vertical-align:top;height:25px">
            <td class="tableCellStyle" style="width: 205px;vertical-align:middle">
                <asp:Label ID="lblPIDInstitutionCodeText" runat="server" Width="160px" Height="22px"></asp:Label>
            </td>
            <td style="padding-bottom: 3px">
                <asp:TextBox ID="txtPIDInstitutionCode" runat="server" Width="100" MaxLength="6" AutoPostBack="true" Style="position:relative;left:-4px"></asp:TextBox>
                <asp:Image ID="imgPIDInstitutionCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                <asp:ImageButton ID="btnSearchPID" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                    ImageAlign="AbsMiddle" style="position:relative;left:-2px"/>
                <asp:Label ID="lblPIDInstitutionCode" runat="server" CssClass="tableText" Style="display: none;position:relative;left:-4px"></asp:Label></td>
        </tr>
        <tr style="vertical-align:top;height:25px">
            <td class="tableCellStyle" style="width: 205px;vertical-align:top">
                <asp:Label ID="lblPIDInstitutionNameText" runat="server" Width="160px" />
            </td>
            <td style="padding-bottom: 3px">
                <asp:Label ID="lblPIDInstitutionName" runat="server" CssClass="tableText" Style="position:relative;left:-4px"></asp:Label>
                <asp:Label ID="lblPIDInstitutionNameChi" runat="server" CssClass="tableTextChi" Style="position:relative;left:-4px"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>

