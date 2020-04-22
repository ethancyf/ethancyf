<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="spVetting.aspx.vb" Inherits="HCVU.spVetting" Title="<%$ Resources:Title, SPVetting %>" %>

<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc2" %>
<%@ Register Src="spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc3" %>
<%@ Register Src="~/UIControl/PCDIntegration/ucPCDWarningPopUp.ascx" TagName="ucPCDWarningPopUp" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panExistingSPProfile" runat="server" Style="display: none;">
                <asp:Panel ID="panExistingSPProfileHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblExistingSPProfileTitle" runat="server" Text="<%$ Resources:Text, ShowSPProfileTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panExistingSPProfileContent" ScrollBars="Vertical" Height="500px"
                                runat="server" Width="786px">
                                <uc1:spSummaryView ID="udcExistingSPProfile" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnExistingSPProfileClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnExistingSPProfileClose_Click" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                            height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td>
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, SPVettingBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, SPVettingBanner %>" /></td>
                    <td align="right">
                        <asp:ImageButton ID="ibtnExistingSPProfile" runat="server" OnClick="ibtnExistingSPProfile_Click"
                            AlternateText="<%$ Resources:AlternateText, OriginalRecordBtn %>" ImageUrl="<%$ Resources:ImageUrl, OriginalRecordBtn %>"
                            Style="display: none" />
                    </td>
                </tr>
            </table>
            <cc2:InfoMessageBox ID="CompleteMsgBox" runat="server" Width="90%" />
            <cc2:MessageBox ID="msgBox" runat="server" Width="90%" />
            <asp:Panel ID="pnlSPVetting" runat="server">
                <asp:MultiView ID="MultiViewSPVetting" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblEnrolRefNoText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtEnrolRefNo" runat="server" MaxLength="17" onBlur="Upper(event,this)"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtSPID" runat="server" MaxLength="8"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtSPHKID" runat="server" MaxLength="11" onBlur="formatHKID(this)"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtSPName" runat="server" MaxLength="40" ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>"
                                        onBlur="Upper(event,this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPPhone" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="20"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPHealthProf" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlSPHealthProf" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:DropDownList ID="ddlStatus" Width="155px" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                    </asp:DropDownList></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblScheme" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlScheme" Width="155px" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td align="center" style="padding-top: 15px">
                                    <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" /></td>
                            </tr>
                        </table>
                        <cc1:FilteredTextBoxExtender ID="FilteredERN" runat="server" TargetControlID="txtEnrolRefNo"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="-">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="FilteredSPID" runat="server" TargetControlID="txtSPID"
                            FilterType="Custom, Numbers">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="FilteredSPHKID" runat="server" TargetControlID="txtSPHKID"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="()">
                        </cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="FilteredSPName" runat="server" TargetControlID="txtSPName"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters " ValidChars="'.-, ">
                        </cc1:FilteredTextBoxExtender>
                    </asp:View>
                    <asp:View ID="ViewSerachResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />
                           
                        <uc2:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panSearchCriteriaReview" />
                          
                        <asp:Panel ID="panSearchCriteriaReview" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultERN" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblResultSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultSPHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblResultSPName" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultPhone" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblResultHealthProf" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:Label ID="lblResultStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblResultSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblResultScheme" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="true" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Enrolment_Ref_No" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnERN" runat="server" CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>                                        
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnRSPID" runat="server" Text='<%# Eval("SP_ID") %> ' CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="70px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Enter_Confirm_Dtm" HeaderText="Data Entry Confirm Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRRequestDtm" runat="server" Text='<%# Eval("Enter_Confirm_Dtm") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="90px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRSPHKID" runat="server" Text='<%# Eval("SP_HKID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="90px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_Eng_Name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                        <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>' CssClass="TextGridChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Phone_Daytime" HeaderText="<%$ Resources:Text, ContactNo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRPhone" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, Status %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRStatus" runat="server" Text='<%# Eval("Record_Status") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="60px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px"/>
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
                        <asp:HiddenField ID="hfEnrolmentAction" runat="server" />
                        <uc1:spSummaryView ID="SpSummaryView1" runat="server"></uc1:spSummaryView>
                        <br />
                        <table>
                            <asp:Panel ID="panEHRSS" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <div class="headingText">
                                                <asp:Label ID="lblEHRSSText" runat="server" Text="<%$ Resources:Text, EHRSSVU %>"></asp:Label></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHadJoinEHRSSText" runat="server" Text="<%$ Resources:Text, HadJoinedEHRSSQVU %>"></asp:Label>
                                            &nbsp; &nbsp;<asp:Label ID="lblHadJoinEHRSS" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>            
                            <asp:Panel ID="panPCD" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <div class="headingText">
                                                <asp:Label ID="lblPCDText" runat="server" Text="<%$ Resources:Text, PCD_Short %>"></asp:Label></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblWillJoinPCDText" runat="server" Text="<%$ Resources:Text, WillJoinPCD %>"></asp:Label>
                                            &nbsp; &nbsp;
                                            <asp:Label ID="lblWillJoinPCD" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                </table>
                            </asp:Panel> 
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="height: 10px" colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:ImageButton ID="ibtnBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                                <td align="center" valign="top">
                                    <asp:ImageButton ID="ibtnAccept" runat="server" AlternateText="<%$ Resources:AlternateText, AcceptBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, AcceptBtn %>" OnClick="ibtnAccept_Click" />
                                    <asp:ImageButton ID="ibtnDefer" runat="server" AlternateText="<%$ Resources:AlternateText, DeferBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, DeferBtn %>" OnClick="ibtnDefer_Click" />
                                    <asp:ImageButton ID="ibtnReject" runat="server" AlternateText="<%$ Resources:AlternateText, RejectBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" OnClick="ibtnReject_Click" />
                                    <asp:ImageButton ID="ibtnReturnForAmendment" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentBtn %>" OnClick="ibtnReturnForAmendment_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewComplete" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtnCompleteBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView></asp:Panel>
            <asp:HiddenField ID="hfERN" runat="server" />
            <asp:HiddenField ID="hfPCDWarningPopupType" runat="server" />
            <asp:Panel Style="display: none" ID="panPopupPCDWarning" runat="server" Width="600px">
                <uc4:ucPCDWarningPopup ID="ucPCDWarningPopup" runat="server" />
            </asp:Panel>
            <asp:Panel ID="panNoticePopup" runat="server" Width="550px" Height="100px" Style="display: none">
                <uc3:ucNoticePopUp ID="ucNoticePopup" runat="server" ButtonMode="OK" NoticeMode="Notification" />
            </asp:Panel>
            
            <asp:Button runat="server" ID="btnHiddenShowExistingSPProfile" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderSPProfile" runat="server" TargetControlID="btnHiddenShowExistingSPProfile"
                PopupControlID="panExistingSPProfile" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panExistingSPProfileHeading">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnHiddenShowPCDWarning" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModelPopupExtenderPCDWarning" runat="server" TargetControlID="btnHiddenShowPCDWarning"
                PopupControlID="panPopupPCDWarning" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnNoticePopupDummy" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="btnNoticePopupDummy"
                PopupControlID="panNoticePopup" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>

            <%-- Popup for Enrolment Action --%>
            <asp:Panel ID="panPopupEnrolmentAction" runat="server" Width="500px" Height="100px" Style="display: none">
                <uc3:ucNoticePopUp ID="ucEnrolmentActionPopup" runat="server" ButtonMode="ConfirmCancel" NoticeMode="Confirmation" MessageAlignment="Center" MessageWidth="95%" />
                <asp:Button ID="btnEnrolmentAction" runat="server" Style="display: none" />                 
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupEnrolmentAction" runat="server" TargetControlID="btnEnrolmentAction"
                PopupControlID="panPopupEnrolmentAction" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <%-- End of Popup for Reject Enrolment --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
