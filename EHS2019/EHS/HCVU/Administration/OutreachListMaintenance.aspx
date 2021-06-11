<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="OutreachListMaintenance.aspx.vb" Inherits="HCVU.OutreachListMaintenance" Title="<%$ Resources:Title, OutreachListMaintenance %>" %>


<%@ Register Src="~/ServiceProvider/spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, OutreachListMaintenanceBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, OutreachListMaintenanceBanner %>" /></td>
                </tr>
            </table>
            <cc1:InfoMessageBox ID="CompleteMsgBox" runat="server" Width="95%" />
            <cc1:MessageBox ID="msgBox" runat="server" Width="95%" />
            <asp:Panel ID="pnlEnquiry" runat="server">
                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">
                        <table>
                            <tr>
                                <td style="width: 130px">
                                    <asp:Label ID="lblOutreachCode" runat="server" Text="<%$ Resources:Text, OutreachCode %>"></asp:Label></td>
                                <td style="width: 350px">
                                    <asp:TextBox ID="txtOutreachCode" runat="server" MaxLength="10" onBlur="Upper(event,this)"></asp:TextBox>
                                    <asp:Image ID="imgOutreachCodeAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                                <td style="width: 130px">
                                    <asp:Label ID="lblOutreachName" runat="server" Text="<%$ Resources:Text, OutreachName %>"></asp:Label>
                                    <td>
                                        <asp:TextBox ID="txtOutreachName" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
                                    </td>
                            </tr>
                            <tr>
                                <td style="width: 130px">
                                    <asp:Label ID="lblOutreachAddr" runat="server" Text="<%$ Resources:Text, OutreachAddr %>"></asp:Label></td>
                                <td style="width: 350px">
                                    <asp:TextBox ID="txtOutreachAddr" runat="server" MaxLength="255" Width="300px"></asp:TextBox></td>
                                <td style="width: 130px">
                                    <asp:Label ID="lbllOutreachStatus" runat="server" Text="<%$ Resources:Text, OutreachStatus %>" Visible="false"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlOutreachStatus" runat="server" AppendDataBoundItems="True" Width="155px" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px"></td>
                                <td style="width: 350px"></td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td align="center" style="padding-top: 10px">
                                    <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                    <asp:ImageButton ID="ibtnNew" runat="server" AlternateText="<%$ Resources:AlternateText, NewOutreachRecordBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, NewOutreachRecordBtn %>" OnClick="ibtnNew_Click" /></td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredOutreachCode" runat="server" TargetControlID="txtOutreachCode"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"></cc2:FilteredTextBoxExtender>
                    </asp:View>
                    <asp:View ID="ViewSearchResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />

                        <uc3:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="pnlSearchCriteriaReview" />

                        <asp:Panel ID="pnlSearchCriteriaReview" runat="server">
                            <table>
                                <tr>
                                    <td valign="top" style="width: 130px">
                                        <asp:Label ID="lblResultOutreachCodeText" runat="server" Text="<%$ Resources:Text, OutreachCode %>"></asp:Label></td>
                                    <td valign="top" style="width: 350px">
                                        <asp:Label ID="lblResultOutreachCode" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td valign="top" style="width: 130px">
                                        <asp:Label ID="lblResultOutreachNameText" runat="server" Text="<%$ Resources:Text, OutreachName %>"></asp:Label>
                                        <td valign="top" style="width: 350px">
                                            <asp:Label ID="lblResultOutreachName" runat="server" CssClass="tableText"></asp:Label>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblResultOutreachAddrText" runat="server" Text="<%$ Resources:Text, OutreachAddr %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultOutreachAddr" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultOutreachStatusText" runat="server" Text="<%$ Resources:Text, OutreachStatus %>" Visible="False"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultOutreachStatus" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td valign="top"></td>
                                    <td valign="top"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="True" Width="1180">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Outreach_code" HeaderText="<%$ Resources:Text, OutreachCode %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnOutreachCode" runat="server" Text='<%# Eval("Outreach_code") %>' CommandArgument='<%# Eval("Outreach_code") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Outreach_Name_Eng" HeaderText="<%$ Resources:Text, OutreachNameEng %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutreachNameEng" runat="server" Text='<%# Eval("Outreach_Name_Eng")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Outreach_Name_Chi" HeaderText="<%$ Resources:Text, OutreachNameChi %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutreachNameChi" runat="server" Text='<%# Eval("Outreach_Name_Chi")%>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="170px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Address_Eng" HeaderText="<%$ Resources:Text, OutreachAddrEng %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutreachAddrEng" runat="server" Text='<%# Eval("Address_Eng") %>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Address_Chi" HeaderText="<%$ Resources:Text, OutreachAddrChi %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutreachAddrChi" runat="server" Text='<%# Eval("Address_Chi") %>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, OutreachStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutreachStatus" runat="server" Text='<%# Eval("Record_Status") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:ImageButton ID="ibtnSearchResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewDetails" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblDetailInfo" runat="server" Text="<%$ Resources:Text,OutreachRecord %>"></asp:Label>
                        </div>
                        <table>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Text, OutreachCode %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailOutreachCode" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 120px">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Text, OutreachName %>"></asp:Label>
                                </td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label14" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailOutreachNameEng" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailOutreachNameEng" runat="server" MaxLength="255"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgOutreachNameEngAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 120px"></td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label15" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailOutreachNameChi" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailOutreachNameChi" runat="server" MaxLength="255"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgOutreachNameChiAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 120px">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Text, OutreachAddr %>"></asp:Label></td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label7" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailOutreachAddrEng" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailOutreachAddrEng" runat="server" MaxLength="1000"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgOutreachAddrEngAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 120px"></td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label17" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailOutreachAddrChi" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailOutreachAddrChi" runat="server" MaxLength="255"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgOutreachAddrChiAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Text, OutreachStatus %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailOutreachStatus" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:DropDownList ID="ddlDetailOutreachStatus" runat="server" AppendDataBoundItems="True" Visible="False">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfDetailOutreachStatus" runat="server" />
                                    <asp:Image ID="imgOutreachStatusAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:ImageButton ID="ibtnBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="<%$ Resources:AlternateText, EditBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, EditBtn %>" OnClick="ibtnEdit_Click" />
                                                <asp:ImageButton ID="ibtnSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSave_Click" />
                                                <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnCancel_Click" />
                                                <asp:ImageButton ID="ibtnActive" runat="server" AlternateText="<%$ Resources:AlternateText, ActivateBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ActivateBtn %>" OnClick="ibtnActive_Click" />
                                                <asp:ImageButton ID="ibtnDeactive" runat="server" AlternateText="<%$ Resources:AlternateText, DeactivateBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, DeactivateBtn %>" OnClick="ibtnDeactive_Click" />
                                                <asp:ImageButton ID="ibtnRemove" runat="server" AlternateText="<%$ Resources:AlternateText, RemoveBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>" OnClick="ibtnRemove_Click" /></td>
                                            <asp:ImageButton ID="ibtnRemoveDummy" runat="server" Visible="false" />
                                        </tr>
                                    </table>
                                    &nbsp;
                                    <asp:HiddenField ID="hfDetailTSMP" runat="server" />
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredDetailOutreachNameEng" runat="server" TargetControlID="txtDetailOutreachNameEng"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;"></cc2:FilteredTextBoxExtender>
                        <cc2:FilteredTextBoxExtender ID="FilteredDetailOutreachAddrEng" runat="server" TargetControlID="txtDetailOutreachAddrEng"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;"></cc2:FilteredTextBoxExtender>

                    </asp:View>
                    <asp:View ID="ViewConfirm" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblConfirmInfo" runat="server" Text="<%$ Resources:Text,ConfirmDetail %>"></asp:Label>
                        </div>
                        <table>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Text, OutreachCode %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmOutreachCode" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 120px">
                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Text, OutreachName %>"></asp:Label></td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label12" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmOutreachNameEng" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">&nbsp;</td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label13" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmOutreachNameChi" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 120px">
                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Text, OutreachAddr %>"></asp:Label>
                                </td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label18" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmOutreachAddrEng" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px">&nbsp;</td>
                                <td style="width: 120px" valign="top">(<asp:Label ID="Label19" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmOutreachAddrChi" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Text, OutreachStatus %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmOutreachStatus" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:HiddenField ID="hfConfirmOutreachStatus" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnConfirm_Click" />
                                                <asp:ImageButton ID="ibtnConfirmBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnConfirmBack_Click" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewMsg" runat="server">
                        &nbsp;<asp:ImageButton ID="ibtnMsgBack" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnMsgBack_Click" />
                    </asp:View>
                    <asp:View ID="ViewError" runat="server">
                        &nbsp;<asp:ImageButton ID="ibtnErrorBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnErrorBack_Click" />
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnCancel" PopupControlID="panPopupConfirmCancel" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmCancel" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirm" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, CancelAlert %>" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmActivate" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnActive" PopupControlID="panPopupConfirmActivate" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmActivate" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmActivate" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, ActivateAlert %>" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmDeactivate" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnDeactive" PopupControlID="panPopupConfirmDeactivate" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmDeactivate" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmDeactivate" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, DeactivateAlert %>" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmRemove" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnRemoveDummy" PopupControlID="panPopupConfirmRemove" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmRemove" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmRemove" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, RemoveAlert %>" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>
