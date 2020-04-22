<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Statistics.aspx.vb" MasterPageFile="~/MasterPage.Master"
    Inherits="HCVU.Statistics" Title="<%$ Resources:Title, StatisticEnquiry %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Statistics/StatisticsCriteriaBase.ascx" TagName="ucStatisticsCriteriaBase"
    TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/Statistics/StatisticsResultBase.ascx" TagName="ucStatisticsResultBase"
    TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="ucCollapsibleSearchCriteriaReview"
    TagPrefix="uc3" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, StatisticEnquiryBanner %>"
                ImageUrl="<%$ Resources:ImageUrl, StatisticEnquiryBanner %>" />
            <cc2:MessageBox ID="udcErrorMessage" runat="server" Width="95%"></cc2:MessageBox>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
            <asp:MultiView ID="mvTimePeriod" runat="server" ActiveViewIndex="0">
                <asp:View ID="viewOpeningHour" runat="server">
                    <!-- Main table start -->
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 930px">
                        <tr>
                            <td>
                                <!-- Tab Header -------------------------------------------------------------->
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 25px">
                                    <tr>
                                        <td id="tdTabHeaderInboxL" runat="server">
                                            <asp:Image ID="imgTabHeaderInboxL" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                        </td>
                                        <td id="tdTabHeaderInboxM" runat="server" valign="middle" style="background-repeat: repeat-x;
                                            text-align: center">
                                            <asp:LinkButton ID="lbtnTabHeaderStatList" runat="server" Text="<%$ Resources:Text, StatisticEnquiry %>"
                                                Style="display: block; width: 89px; font-size: 12px" OnClick="lbtnTabHeaderStatList_Click">
                                            </asp:LinkButton>
                                        </td>
                                        <td id="tdTabHeaderInboxR" runat="server">
                                            <asp:Image ID="imgTabHeaderInboxR" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                        </td>
                                        <td id="tdTabHeaderContentL" runat="server">
                                            <asp:Image ID="imgTabHeaderContentL" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                        </td>
                                        <td id="tdTabHeaderContentM" runat="server">
                                            <asp:LinkButton ID="lbtnTabHeaderContent" runat="server" Style="font-size: 12px"
                                                Text="" OnClick="lbtnTabHeaderContent_Click"></asp:LinkButton>
                                            <asp:ImageButton ID="ibtnTabHeaderContentClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, TabHeaderClose %>"
                                                OnClick="ibtnTabHeaderContentClose_Click" />
                                        </td>
                                        <td id="tdTabHeaderContentR" runat="server">
                                            <asp:Image ID="imgTabHeaderContentR" runat="server" ImageUrl="<%$ Resources:ImageUrl, Transparent %>" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 1px">
                                <!-- Tab Content -------------------------------------------------------------->
                                <table cellpadding="0" cellspacing="0" style="width: 100%; border: 1px solid #999999">
                                    <tr style="height: 12px">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr style="height: 355px" valign="top">
                                        <!-- Side Bar Menu -------------------------------------------------------------->
                                        <td style="width: 100px; border-top: 1px solid #BBBBBB; border-right: 1px solid #BBBBBB">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                <!-- Side Bar Menu Item - Inbox -------------------------------------------------------------->
                                                <tr>
                                                    <td id="tdSidebarStatList" runat="server">
                                                        <asp:LinkButton ID="lbtnSidebarStatList" runat="server" Text="<%$ Resources:Text, StatisticEnquiry %>"
                                                            Style="display: block; width: 100px; padding: 10px 0px 10px 5px;" OnClick="lbtnSidebarStatList_Click">
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <!-- Content -------------------------------------------------------------->
                                        <td valign="top" style="padding: 0px 10px 0px 10px">
                                            <!-- Multiview (main content) start -->
                                            <asp:MultiView ID="mvStatistics" runat="server" ActiveViewIndex="0">
                                                <asp:View ID="viewEnquiry" runat="server">
                                                    <asp:GridView ID="gvReportList" runat="server" Width="780px" AllowPaging="True" AllowSorting="True"
                                                        AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, StatisticsID %>" HeaderStyle-VerticalAlign="Top"
                                                                SortExpression="STSU_Statistic_ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReportID" runat="server" Text='<%# Eval("STSU_Statistic_ID") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="90px" />
                                                                <ItemStyle VerticalAlign="Top" Width="90px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, Description %>" HeaderStyle-VerticalAlign="Top"
                                                                SortExpression="STSU_Desc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("STSU_Desc") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>" HeaderStyle-VerticalAlign="Top"
                                                                SortExpression="STSU_Scheme">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheme" runat="server" Text='<%# Eval("STSU_Scheme") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="60px" />
                                                                <ItemStyle VerticalAlign="Top" Width="60px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                                                    </asp:GridView>
                                                    <br />
                                                </asp:View>
                                                <asp:View ID="viewCriteria" runat="server">
                                                    <asp:Panel ID="panEnquiryCriteria" runat="server">
                                                        <table cellpadding="3" cellspacing="0" style="width: auto; border: 1px solid #999999;
                                                            border-collapse: collapse;">
                                                            <tr>
                                                                <td align="left" style="width: 110px; vertical-align: middle; border: 1px solid #999999;
                                                                    border-collapse: collapse;">
                                                                    <asp:Label ID="lblEnquiryReportIDTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, StatisticsID %>"></asp:Label></td>
                                                                <td align="left" style="width: 630px; vertical-align: middle; border: 1px solid #999999;
                                                                    border-collapse: collapse;">
                                                                    <asp:Label ID="lblEnquiryReportID" runat="server" CssClass="tableText" Text=""></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 110px; vertical-align: middle; border: 1px solid #999999;
                                                                    border-collapse: collapse;">
                                                                    <asp:Label ID="lblEnquiryDescTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Description %>"></asp:Label></td>
                                                                <td align="left" style="width: 630px; vertical-align: middle; border: 1px solid #999999;
                                                                    border-collapse: collapse;">
                                                                    <div>
                                                                        <div style="float: left">
                                                                            <asp:Label ID="lblEnquiryDesc" runat="server" CssClass="tableText" Text=""></asp:Label>
                                                                        </div>
                                                                        <div style="float: left; padding-left: 5px">
                                                                            <asp:ImageButton ID="iBtnRemark" runat="server" ImageUrl="<%$ Resources:ImageUrl, RemarkSBtn %>" />
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 110px; vertical-align: middle; border: 1px solid #999999;
                                                                    border-collapse: collapse;">
                                                                    <asp:Label ID="lblEnquirySchemeTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                                                <td align="left" style="width: 630px; vertical-align: middle; border: 1px solid #999999;
                                                                    border-collapse: collapse;">
                                                                    <asp:Label ID="lblEnquiryScheme" runat="server" CssClass="tableText" Text=""></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                        <table>
                                                            <tr>
                                                                <td style="height: 20px">
                                                                    <br />
                                                                    <div class="headingText">
                                                                        <asp:Label ID="lblEnquiryReportCriteria" runat="server" Font-Bold="true" Text="<%$ Resources:Text, EnquiryCriteria %>"></asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 10px">
                                                                </td>
                                                            </tr>
                                                            <asp:Panel ID="panCutOffDate" runat="server" Visible="True">
                                                                <tr>
                                                                    <!-- Cut off date area -->
                                                                    <td style="width: 780px">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="left" style="width: 150px;">
                                                                                    <asp:Label ID="lblCutOffDateDesc" runat="server" Text="<%$ Resources:Text, CutoffDate %>"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblCutOffDate" runat="server" CssClass="tableText" Text=""></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </asp:Panel>
                                                            <tr>
                                                                <td style="width: 780px">
                                                                    <uc1:ucStatisticsCriteriaBase ID="ucStatisticsCriteriaBase" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">
                                                                </td>
                                                            </tr>
                                                            <asp:Panel ID="panRemarkResource" runat="server" Visible="False">
                                                                <tr>
                                                                    <!-- Remark area -->
                                                                    <td>
                                                                        <asp:Label ID="lblRemarkResource" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 20px">
                                                                    </td>
                                                                </tr>
                                                            </asp:Panel>
                                                        </table>
                                                        <table cellpadding="0" cellspacing="0" style="width: auto; margin-bottom: 20px">
                                                            <tr>
                                                                <td align="left" style="width: 150px">
                                                                    <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnCancel_Click" />
                                                                </td>
                                                                <td align="Center" style="width: 550px">
                                                                    <asp:ImageButton ID="ibtnSubmit" runat="server" ImageUrl="<%$ Resources:ImageUrl, SubmitBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, SubmitBtn %>" OnClick="ibtnSubmit_Click" />
                                                                </td>
                                                                <td style="width: 200px">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </asp:View>
                                                <asp:View ID="viewResult" runat="server">
                                                    <uc3:ucCollapsibleSearchCriteriaReview ID="ucCollapsibleSearchCriteriaReview" runat="server"
                                                        TargetControlID="panResultCriteria" HeaderText="<%$ Resources:Text, Result %>" />
                                                    <asp:Panel ID="panResultCriteria" runat="server">
                                                        <br />
                                                        <table cellpadding="0" cellspacing="0" style="width: auto">
                                                            <tr>
                                                                <td align="left" style="width: 180px; vertical-align: top">
                                                                    <asp:Label ID="lblResultReportIDTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, StatisticsID %>"></asp:Label></td>
                                                                <td align="left" style="width: 580px">
                                                                    <asp:Label ID="lblResultReportID" runat="server" CssClass="tableText" Text=""></asp:Label></td>
                                                            </tr>
                                                            <tr style="height: 4px">
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 180px; vertical-align: top">
                                                                    <asp:Label ID="lblResultDescTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Description %>"></asp:Label></td>
                                                                <td align="left" style="width: 580px">
                                                                    <asp:Label ID="lblResultDesc" runat="server" CssClass="tableText" Text=""></asp:Label></td>
                                                            </tr>
                                                            <tr style="height: 4px">
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 180px; vertical-align: top">
                                                                    <asp:Label ID="lblResultCutOffDateTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CutoffDate %>"></asp:Label></td>
                                                                <td align="left" style="width: 580px">
                                                                    <asp:Label ID="lblResultCutOffDate" runat="server" CssClass="tableText" Text=""></asp:Label></td>
                                                            </tr>
                                                            <tr style="height: 4px">
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left" style="width: auto; vertical-align: top">
                                                                    <uc2:ucStatisticsResultBase ID="ucStatisticsResultBase" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <br />
                                                    <div style="overflow: visible">
                                                        <div>
                                                            <asp:GridView ID="gvDynamicGrid" runat="server" AllowPaging="True" AllowSorting="False"
                                                                AutoGenerateColumns="False" Style="table-layout: fixed;">
                                                                <FooterStyle Wrap="False" />
                                                            </asp:GridView>
                                                        </div>
                                                        <div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <asp:Panel ID="panNoRecordsFound" Visible="False" runat="server">
                                                        <asp:Label ID="lblNoRecordsFound" runat="server" CssClass="tableText" Text="<%$ Resources:Text, NoRecordsFound %>" />
                                                        <p />
                                                    </asp:Panel>
                                                    <table cellpadding="0" cellspacing="0" style="width: auto">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblEnquiryTimeTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, EnquiryTimeColon %>"></asp:Label></td>
                                                            <td align="left" style="width: 580px; padding-left: 5px">
                                                                <asp:Label ID="lblEnquiryTimeText" runat="server" CssClass="tableText" Text=""></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table cellpadding="0" cellspacing="4" style="width: auto; margin-bottom: 20px">
                                                        <tr>
                                                            <td align="left" style="width: 150px">
                                                                <asp:ImageButton ID="ibtnBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBack_Click" />
                                                            </td>
                                                            <td align="Center" style="width: 550px">
                                                                <asp:ImageButton ID="ibtnExport" runat="server" ImageUrl="<%$ Resources:ImageUrl, ExportRecordBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ExportRecordBtn %>" OnClick="ibtnExport_Click" />
                                                            </td>
                                                            <td style="width: 200px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                            </asp:MultiView>
                                            <!-- Multiview (main content) end -->
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <!-- Main table end -->
                    <%-- Popup for Export redirect (Start) --%>
                    <asp:Button ID="btnHiddenRedirect" runat="server" Style="display: none" />
                    <asp:Panel ID="panNoticeExportRedirect" runat="server" Width="500px" Height="100px"
                        Style="display: none">
                        <uc4:ucNoticePopUp ID="ucNoticeExportRedirect" runat="server" ButtonMode="YesNo"
                            NoticeMode="Confirmation" MessageText="<%$ Resources:Text, ExportSuccessfully %>" />
                    </asp:Panel>
                    <cc1:ModalPopupExtender ID="popupNoticeExportRedirect" runat="server" TargetControlID="btnHiddenRedirect"
                        PopupControlID="panNoticeExportRedirect" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                    </cc1:ModalPopupExtender>
                    <%-- Popup for Export redirect (End) --%>
                    <%-- Popup for report remarks (Start) --%>
                    <asp:Panel ID="panNoticePopupRemark" runat="server" Width="500px" Height="100px"
                        Style="display: none">
                        <uc4:ucNoticePopUp ID="ucNoticePopupRemark" runat="server" ButtonMode="OK" NoticeMode="Remark"
                            MessageText="" />
                    </asp:Panel>
                    <cc1:ModalPopupExtender ID="popupNoticeRemark" runat="server" TargetControlID="iBtnRemark"
                        PopupControlID="panNoticePopupRemark" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                    </cc1:ModalPopupExtender>
                    <%-- Popup for report remarks (End) --%>
                </asp:View>
                <asp:View ID="viewClosingHour" runat="server">
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
