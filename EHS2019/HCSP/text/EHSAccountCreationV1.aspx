<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/text/ClaimVoucher.Master"
    Codebehind="EHSAccountCreationV1.aspx.vb" Inherits="HCSP.Text.EHSAccountCreationV1"
    Title="<%$ Resources:Title, Claim %>" %>

<%@ Register Src="../UIControl/ucInputTips.ascx" TagName="ucInputTips" TagPrefix="uc4" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
    <cc1:TextOnlyMessageBox ID="udcMsgBoxErr" runat="server" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
    <cc1:TextOnlyInfoMessageBox ID="udcMsgBoxInfo" runat="server" Width="100%" />
            </td>
        </tr>
    </table>
    <asp:MultiView ID="mvAccountCreation" runat="server">
        <asp:View ID="vStep1a1" runat="server">
            <asp:Panel runat="server" ID="panStep1Submit" DefaultButton="btnstep1a1CreateAccount">
                <table cellspacing="0" cellpadding="0" class="textVersionTable">
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1AcctInfoText" runat="server" Text="<%$ Resources:Text, SearchInfo %>"
                                CssClass="tableHeader"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1DocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"
                                CssClass="tableTitle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1DocumentType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1HKIDText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1HKID" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1DOBText" runat="server" Text="<%$ Resources:Text, DOB %>"
                                CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1DOB" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="SeparationLine">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblstep1a1DisclaimerNotice" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="bottom" align="left">
                            <asp:Button ID="btnstep1a1Cancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                            <asp:Button ID="btnstep1a1CreateAccount" runat="server" Text="<%$ Resources:AlternateText, CreateAccBtn %>" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vStep1a2" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblStep1a2AcctInfoText" runat="server" Text="<%$ Resources:Text, AccountInfo %>" CssClass="tableHeader"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <uc3:ucReadOnlyDocumnetType ID="udcStep1a2ReadOnlyDocumnetType" runat="server"></uc3:ucReadOnlyDocumnetType>
                    </td>
                </tr>
                <tr>
                    <td class="SeparationLine">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblStep1a2TempAccDisclaimerText" runat="server" CssClass="tableText" Text="<%$ Resources:Text, ConfirmTempAccDisclaim %>"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top" style="padding-top: 4px">
                        <asp:Button ID="btnStep1a2Back" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                        <asp:Button ID="btnStep1a2Confirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                        <asp:Button ID="btnStep1a2Modify" runat="server" Text="<%$ Resources:AlternateText, ModifyBtn %>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vStep1b1" runat="server">
            <asp:Panel runat="server" ID="panStep1bSubmit" DefaultButton="btnStep1b1Next">
                <table cellspacing="0" cellpadding="0" class="textVersionTable">
                    <tr>
                        <td style="width: 100%" valign="top">
                            <asp:Label ID="lblstep1b1InputAcctInfoText" runat="server" CssClass="tableHeader"
                                Text="<%$ Resources:Text, InputAccountInformation %>" Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%">
                            <asp:Label ID="lblStep1b1CurrentPracticeText" runat="server" Text="<%$ Resources:Text, CurrentPractice %>"
                                CssClass="tableTitle"></asp:Label>
                            <asp:Button ID="btnStep1b1ChangePractice" runat="server" Text="<%$ Resources:AlternateText, Change %>"
                                SkinID="TextOnlyVersionLinkButton" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%; height: 20px">
                            <asp:Label ID="lblStep1b1CurrentPractice" runat="server" CssClass="tableTextChi" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%">
                            <asp:Label ID="lblStep1b1InputInfoText" runat="server" CssClass="tableText"></asp:Label>
                            <asp:Button ID="lnkDocIDTips" runat="server" Text="<%$ Resources:Text, InputTips %>"
                                BackColor="White" BorderStyle="None" Font-Underline="True" ForeColor="Blue" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%">
                            <uc2:ucInputDocumentType ID="udcStep1b1InputDocumentType" runat="server"></uc2:ucInputDocumentType>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 100%">
                            <asp:Button ID="btnStep1b1Cancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                            <asp:Button ID="btnStep1b1Next" runat="server" Text="<%$ Resources:AlternateText, NextBtn%>" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vStep1b2" runat="server">
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblStep1b2CreationAcctInfotext" runat="server" Text="<%$ Resources:Text, ConfirmAccountInformation %>"
                            Width="100%" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top">
                        <uc3:ucReadOnlyDocumnetType ID="udcStep1b2ReadOnlyDocumnetType" runat="server"></uc3:ucReadOnlyDocumnetType>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Button ID="btnStep1b2Cancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                        <asp:Button ID="btnStep1b2Back" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="btnStep1b2Confirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vStep1c" runat="server">
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblStep1cAcctInfo" runat="server" Text="<%$ Resources:Text, AccountInfo %>"
                            Width="100%" CssClass="tableHeader"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <uc3:ucReadOnlyDocumnetType ID="udcStep1cReadOnlyDocumnetType" runat="server"></uc3:ucReadOnlyDocumnetType>
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td>
                            <asp:Button ID="btnStep1cProceedToClaim" runat="server" Text="<%$ Resources:AlternateText, ProceedClaimBtn%>" />
                            <asp:Button ID="btnStep1cNextCreation" runat="server" Text="<%$ Resources:AlternateText, NextCreationBtn%>" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:View>
        <asp:View ID="vConfirmCancelBox" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"
                            CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td class="tableRemark">
                        <asp:Label ID="lblConfirmCancelBoxMessage" runat="server" CssClass="tableText" Text="<%$ Resources:Text, CancelAlert %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="padding-top: 4px">
                        <asp:Button ID="btnConfirmCancelBoxCancel" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="btnConfirmCancelBoxConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vConfirmModifyBox" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="textVersionTable">
                        <asp:Label ID="lblConfirmModifyBoxMessageTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"
                            CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td class="tableRemark">
                        <asp:Label ID="lblConfirmModifyBoxMessage" runat="server" CssClass="tableText" Text="<%$ Resources:Text, ModifyTempAccDisclaim %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="padding-top: 4px">
                        <asp:Button ID="btnConfirmModifyBoxNo" runat="server" Text="<%$ Resources:AlternateText, NoBtn%>" />
                        <asp:Button ID="btnConfirmModifyBoxYes" runat="server" Text="<%$ Resources:AlternateText, YesBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vConfirmOnlyBox" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmOnlyBoxMessageTitle" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, ConfirmBoxTitle %>" Width="100%"></asp:Label></td>
                </tr>
                <tr>
                    <td class="tableRemark">
                        <asp:Label ID="lblConfirmOnlyBoxMessage" runat="server" CssClass="tableText" Text="<%$ Resources:Text, PracticeNoAvailSchemeForEHSAccountCreateion %>"
                            Width="100%"></asp:Label></td>
                </tr>
                <tr>
                    <td style="padding-top: 4px">
                        <asp:Button ID="btnConfirmOnlyBoxConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vSelectPractice" runat="server">
            <asp:Panel runat="server" ID="panStepSelectPractice" DefaultButton="btnStepSelectPracticeGO">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectPracticePracticeText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, SelectPractice %>" Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <cc2:PracticeRadioButtonGroupText runat="server" ID="ucSelectPracticeRadioButtonGroupText"
                                CssClass="tableTextChi" CellPadding="0" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnStepSelectPracticeGO" runat="server" Text="<%$ Resources:AlternateText, SelectBtn%>" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vConfirmDeclare" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td style="height: 19px">
                        <asp:Label ID="lblConfirmDeclareMessageTitle" runat="server" CssClass="tableHeader"
                            Text="<%$ Resources:Text, Declaration %>" Width="100%"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" class="tableRemarkHighlight">
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmDeclareMessage" runat="server" CssClass="tableText" Text="<%$ Resources:Text, ProvidedInfoTrueVAForm %>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 4px">
                        <asp:Button ID="btnConfirmDeclareReturn" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="btnConfirmDeclareConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vInputTips" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblInputTipsTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputTips %>"
                            Width="100%"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc4:ucInputTips runat="server" ID="ucInputTipsControl"></uc4:ucInputTips>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnInputTipBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vInternalError" runat="server">
        </asp:View>
        <asp:View ID="vExclamationConfirmationBox" runat="server">
        </asp:View>
        <asp:View ID="vPrintOption" runat="server">
        </asp:View>
        <asp:View ID="vAddHocPrint" runat="server">
        </asp:View>
        <asp:View ID="vStep1b3" runat="server">
            <table cellpadding="0" cellspacing="0" border="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblStep1b3Header" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top">
                        <uc2:ucInputDocumentType ID="udcStep1b3InputDocumentType" runat="server"></uc2:ucInputDocumentType>
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" border="0" class="textVersionTable">
                <tr>
                    <td style="padding-top: 4px">
                        <asp:Button ID="btnStep1b3Cancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                        <asp:Button ID="btnStep1b3Confirm" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
