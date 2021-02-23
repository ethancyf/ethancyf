<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ReprintVaccinationRecord.aspx.vb" Inherits="HCVU.ReprintVaccinationRecord"
    EnableEventValidation="false" Title="<%$ Resources:Title, ReprintVaccinationRecord %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/Reimbursement/ClaimTransDetail.ascx" TagName="udcClaimTransDetail" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/IDEASCombo/IDEASCombo.ascx" TagName="IDEASCombo" TagPrefix="uc11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="300">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <uc11:IDEASCombo ID="ucIDEASCombo" runat="server" />
            <asp:Image ID="imgReprintVaccinationRecord" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReprintVaccinationRecordBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReprintVaccinationRecordBanner %>" />
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:MessageBox ID="udcErrorMessage" runat="server" Visible="False" Width="95%"></cc2:MessageBox>
                <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
            </asp:Panel>
            <asp:Panel ID="panReprintVaccinationRecord" runat="server">
                <asp:MultiView ID="MultiViewReprintVaccinationRecord" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearch" runat="server">
                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <tr>
                                <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lbleHSDocTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>" Style="position: relative; top: 2px" />
                                </td>
                                <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddleHSDocType" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 485px; position: relative; top: -1px" />
                                            </td>
                                            <td>
                                                <asp:Image ID="imgeHSDocTypeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lbleHSDocNoText" runat="server" Text="<%$ Resources:Text, IdentityDocNo %>" Style="position: relative; top: 2px" />
                                </td>
                                <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txteHSDocNo" runat="server" Width="176" onChange="convertToUpper(this)" MaxLength="30" />
                                            </td>
                                            <td>
                                                <asp:Image ID="imgeHSDocNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 3px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; padding-top: 15px" colspan="2">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="text-align: right; padding-right: 10px; width:300px">
                                                <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                            </td>
                                            <td style="text-align: left; padding-left: 10px">
                                                <asp:ImageButton ID="ibtnSearchByCard" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReadCardAndSearchBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ReadCardAndSearchBtn %>" OnClick="ibtnSearchByCard_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>


                        </table>

                    </asp:View>
                    <asp:View ID="ViewTransaction" runat="server">                        
                        <uc1:udcClaimTransDetail ID="udcClaimTransDetail" runat="server" />
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 210px;">
                                    <asp:ImageButton ID="ibtnDetailBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnDetailBack_Click" 
                                        style="position:relative;left:10px" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="ibtnReprintRecord" runat="server" AlternateText="<%$ Resources: AlternateText, ReprintBtn %>" ImageUrl="<%$ Resources: ImageUrl, ReprintBtn %>" OnClick="ibtnReprintRecord_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>

                </asp:MultiView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
