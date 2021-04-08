<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="VaccineLotCreationApproval.aspx.vb" Inherits="HCVU.VaccineLotCreationApproval" Title="<%$ Resources:Title, VaccineLotApproval %>" %>

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
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, VaccineLotCreationApprovalBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, VaccineLotCreationApprovalBanner %>" /></td>
                </tr>
            </table>
            <cc1:InfoMessageBox ID="udcInfoBox" runat="server" Width="95%" />
            <cc1:MessageBox ID="msgBox" runat="server" Width="95%" />
            <asp:Panel ID="pnlEnquiry" runat="server">
                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                   
                    <asp:View ID="ViewSearchResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />
                        
                      
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="True" Width="980">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Vaccine_Lot_No" HeaderText="<%$ Resources:Text, VaccineLotNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnVLNo" runat="server" Text='<%# Eval("Vaccine_Lot_No")%>' CommandArgument='<%# Eval("Vaccine_Lot_No")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="70px"/>
                                </asp:TemplateField>
                               
                                <asp:TemplateField SortExpression="BrandName" HeaderText="<%$ Resources:Text, VaccineBrandName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLBrandName" runat="server" Text='<%# Eval("BrandName")%>'></asp:Label><br />
                                        <asp:hiddenField ID="hfVLBrandId" runat="server" value='<%# Eval("BrandId")%>'></asp:hiddenField>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ExpiryDate" HeaderText="<%$ Resources:Text, VaccineLotExpiryDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLExpiryDtm" runat="server" Text='<%# Eval("ExpiryDate", "{0:dd MMM yyyy}")%>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="200px"/>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField SortExpression="Request_Type" HeaderText="<%$ Resources:Text, RequestType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLRequestType" runat="server" Text='<%# Eval("Request_Type")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_status" HeaderText="<%$ Resources:Text, VaccineLotRecordStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLRecordStatus" runat="server" Text='<%# Eval("Record_status")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                       
                    </asp:View>
                    <asp:View ID="ViewDetails" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblVaccineLotTitle" runat="server" Text="<%$ Resources:Text,VaccineLotRecord %>"></asp:Label>
                        </div>
                        <table  style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleBrandName" runat="server" Text="<%$ Resources:Text, VaccineBrandName %>" Style="position: relative; top: 2px" ></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailBrandName" runat="server" CssClass="tableText"></asp:Label>
                                     
                                     
                                   </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                 <asp:Label ID="lblDetailTitleVaccineLotNo" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" ></asp:Label>
                                </td>
                                 
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailVaccineLotNo" runat="server" CssClass="tableText"></asp:Label>
                                   <%--<asp:Label ID="lblDetailVaccineLotID" runat="server" CssClass="tableText" visible="false" ></asp:Label>--%>
                                   </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleExpiryD" runat="server" Text="<%$ Resources:Text, VaccineLotExpiryDate %>" Style="position: relative; top: 2px" ></asp:Label>
                                </td>
                                
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailExpiryD" runat="server" CssClass="tableText"></asp:Label>
                                   <asp:Label ID="lblDetailNewExpiryD" runat="server" CssClass="tableText" Visible="false" ></asp:Label>
                                    </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" Style="position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleRecordStatus" runat="server" Text="<%$ Resources:Text, VaccineLotRecordStatus %>"></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailRecordStatus" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
                                       </td>
                            </tr>
                            <tr>
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" Style="position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleRequestType" runat="server" Text="<%$ Resources:Text, RequestType %>"></asp:Label></td>
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailRequestType" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
                                </td>
                            </tr>
                            <tr>
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" Style="position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleRequestedBy" runat="server" Text="<%$ Resources:Text, RequestedBy %>"></asp:Label></td>
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailRequestedBy" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
                                </td>
                            </tr>
                            <tr>
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" Style="position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleCreatedBy" runat="server" Text="<%$ Resources:Text, CreateBy %>"></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailCreatedBy" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 210px">
                                                <asp:ImageButton ID="ibtnBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                                            <td align="left">
                                               <asp:ImageButton ID="ibtnApproval" runat="server" AlternateText="<%$ Resources:AlternateText, ApprovalBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ApproveBtn %>" OnClick="ibtnApproval_Click" />
                                                <asp:ImageButton ID="ibtnReject" runat="server" AlternateText="<%$ Resources:AlternateText, RejectBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" OnClick="ibtnReject_Click" />
                                                
                                                </td>
                                        </tr>
                                    </table>
                                  <%--  &nbsp;
                                    <asp:HiddenField ID="hfDetailTSMP" runat="server" />
                                    &nbsp; &nbsp;--%>
                                </td>
                            </tr>
                        </table>
                    

                    </asp:View>
                    
                    <asp:View ID="ViewMsg" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnMsgBack" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnMsgBack_Click" /></asp:View>
                    <asp:View ID="ViewError" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnErrorBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnErrorBack_Click" /></asp:View>
                </asp:MultiView>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmApproval" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnApproval" PopupControlID="panPopupConfirmApproval" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmApproval" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmApproval" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, ApprovalAlert %>" />
            </asp:Panel>
 
            <cc2:ModalPopupExtender ID="ModalPopupConfirmReject" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnReject" PopupControlID="panPopupConfirmReject" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmReject" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmReject" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, RejectAlert %>" />
            </asp:Panel>
           
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>
