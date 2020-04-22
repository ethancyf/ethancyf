<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="CompletedEnrolment.aspx.vb" Inherits="eForm.CompletedEnrolment" Title="<%$ Resources:Title, eForm %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    //  keeps track of the delete button for the row
        //  that is going to be removed
        var _source;
        // keep track of the popup div
        var _popup;
        function showConfirm(source)
        {
            this._source = source;
            this._popup = $find('mdlPopup');
            //  find the confirm ModalPopup and show it    
            this._popup.show();
        }
        function okClick()
        {
            //  find the confirm ModalPopup and hide it    
            this._popup.hide();
            //  use the cached button as the postback source
            __doPostBack(this._source.name, '');
        }
        function cancelClick()
        {
            //  find the confirm ModalPopup and hide it 
            this._popup.hide();
            //  clear the event source
            this._source = null;
            this._popup = null;
        }
    </script>

    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eFormSpEnrolmentBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, eFormSpEnrolmentBanner %>" />
    <table border="0" cellpadding="0" cellspacing="0" width="600">
        <tr>
            <td>
                <asp:Panel ID="panStep1" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep1" Text="<%$ Resources:Text, eFormStep1 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep2" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep2" Text="<%$ Resources:Text, eFormStep2 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep3" runat="server" CssClass="highlightTimeline">
                    <asp:Label ID="lblStep3" Text="<%$ Resources:Text, eFormStep3 %>" runat="server"></asp:Label></asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 700px">
                        <tr>
                            <td style="background-image: url(Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 700px">
                    <tr>
                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label></td>
                                    <td align="center" style="width: 40px; height: 42px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, YesBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, YesBtn %>" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, NoBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NoBtn %>" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(Images/dialog/bottom-right.png); width: 7px; height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:InfoMessageBox ID="InfoMessageBox1" runat="server" Width="100%"></cc1:InfoMessageBox>
            <table>
                <tr>
                    <td style="width: 200px">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
            <div class="headingText">
                <asp:Label ID="lblDownloadFormText" runat="server" Text="<%$ Resources:Text, eHSEnrolmentCompletedHeader %>"></asp:Label>
                <asp:Label ID="Label3" runat="server" Text=" ( "></asp:Label>
                <asp:Label ID="lblCompleteSubmissionDateText" runat="server" Text="<%$ Resources:Text, SubmissionDtmTime %>"></asp:Label>
                <asp:Label ID="Label1" runat="server" Text=" : "></asp:Label>
                <asp:Label ID="lblCompleteSubmissionDate" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text=" ) "></asp:Label>
            </div>
            <br />
            <asp:GridView ID="gvPrintOut" runat="server" AutoGenerateColumns="False" ShowHeader="true"
                GridLines="Both" ShowFooter="false" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                        <ItemStyle VerticalAlign="Top" ForeColor="Black" />
                        <HeaderStyle VerticalAlign="Top" Width="15px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                        <ItemTemplate>
                            <asp:Label ID="lblERNIndex" runat="server" CssClass="tableText" Text='<%# Eval("Enrolment_Ref_No") %>'></asp:Label></ItemTemplate>
                        <ItemStyle VerticalAlign="Top" ForeColor="Black" />
                        <HeaderStyle VerticalAlign="Top" Width="140px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationName %>">
                        <ItemTemplate>
                            <asp:Label ID="lblMOEName" runat="server" CssClass="tableText" Text='<%# Eval("MO_Eng_Name") %>'></asp:Label><br />
                            <asp:Label ID="lblMOCName" runat="server" CssClass="TextChi" Text='<%# formatChineseString(Eval("MO_Chi_Name")) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                        <HeaderStyle VerticalAlign="Top" Width="350px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMOApp" runat="server" Text="<%$ Resources:Text, EnrolmentApplicationForm %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Image ID="imgMOApp" runat="server" ImageUrl="<%$ Resources:ImageUrl, SmallPrinterIcon %>"
                                            ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, PrinterImg %>" />
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnEngAppA" runat="server" Text="<%$ Resources:Text, English %>"></asp:LinkButton>
                            /
                            <asp:LinkButton ID="lnkBtnChiAppA" runat="server" Text="<%$ Resources:Text, Chinese %>"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" />
                        <HeaderStyle VerticalAlign="Top" Width="125px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEHRSS" runat="server" Text="<%$ Resources:Text, EHRSSConsentFom %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Image ID="imgEHRSS" runat="server" ImageUrl="<%$ Resources:ImageUrl, SmallPrinterIcon %>"
                                            AlternateText="<%$ Resources:AlternateText, PrinterImg %>" /></td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnEngEHRSS" runat="server" Text="<%$ Resources:Text, English %>"></asp:LinkButton>
                            /
                            <asp:LinkButton ID="lnkBtnChiEHRSS" runat="server" Text="<%$ Resources:Text, Chinese %>"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" />
                        <HeaderStyle VerticalAlign="Top" Width="195px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <li style="margin-left: 10pt">
                                <asp:LinkButton ID="lnkBtnCheckList" runat="server" Text="<%$ Resources:Text, CheckList  %>"></asp:LinkButton></li>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                        <HeaderStyle VerticalAlign="Top" Width="125px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:Panel ID="pnlPCD" runat="server">
                <div class="headingTextOtherSystemMultiLine">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblDownloadFormPCDText" runat="server" Text="<%$ Resources:Text, PCDEnrolmentCompletedHeader %>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <asp:GridView ID="gvPCDPrintOut" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                    GridLines="Both" ShowFooter="false">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblPCDERNHeader" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPCDERN" runat="server" CssClass="tableText" Text='<%# Eval("PCDERN") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" Width="210px" />
                            <HeaderStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPCDEnrolmentFormHeader" runat="server" Text="<%$ Resources:Text, EnrolmentInformation %>"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Image ID="imgPCDEnrolmentForm" runat="server" ImageUrl="<%$ Resources:ImageUrl, SmallPrinterIcon %>"
                                                AlternateText="<%$ Resources:AlternateText, PrinterImg %>" ToolTip="<%$ Resources:AlternateText, PrinterImg %>" /></td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnPCDEnrolmentFormEng" runat="server" Text="<%$ Resources:Text, English %>"></asp:LinkButton>
                                /
                                <asp:LinkButton ID="lnkBtnPCDEnrolmentFormChi" runat="server" Text="<%$ Resources:Text, Chinese %>"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle VerticalAlign="Top" Width="160px"/>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <br />
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibtnCompleteClose" runat="server" AlternateText="<%$ Resources:AlternateText, CompleteBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, CompleteBtn %>" OnClick="ibtnCompleteClose_Click" />
                    </td>
                </tr>
            </table>
            <cc2:ModalPopupExtender ID="ModalPopupExtenderConfirmClose" runat="server" TargetControlID="panConfirmMsg"
                PopupControlID="panConfirmMsg" BehaviorID="mdlPopup" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" OkControlID="ibtnDialogConfirm"
                CancelControlID="ibtnDialogCancel" OnOkScript="okClick();" OnCancelScript="cancelClick();"
                PopupDragHandleControlID="panConfirmMsgHeading">
            </cc2:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
