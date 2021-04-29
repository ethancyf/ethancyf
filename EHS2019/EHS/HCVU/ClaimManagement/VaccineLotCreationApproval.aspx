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
                            AllowSorting="True" Width="1060px">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                                              
                                <asp:TemplateField SortExpression="Brand_Trade_Name" HeaderText="<%$ Resources:Text, VaccineName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLBrandName" runat="server" Text='<%# Eval("Brand_Trade_Name")%>'></asp:Label><br />
                                        <asp:hiddenField ID="hfVLBrandId" runat="server" value='<%# Eval("Brand_Id")%>'></asp:hiddenField>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="280px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Vaccine_Lot_No" HeaderText="<%$ Resources:Text, VaccineLotNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnVLNo" runat="server" Text='<%# Eval("Vaccine_Lot_No")%>' CommandArgument='<%# Eval("Vaccine_Lot_No")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="70px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Expiry_Date" HeaderText="<%$ Resources:Text, VaccineLotExpiryDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLExpiryDtm" runat="server" Text='<%# Eval("Expiry_Date", "{0:dd MMM yyyy}")%>' CssClass="textChi"></asp:Label>
                                       <%-- <asp:label ID="lblVLSymbol" runat="server" Text="<br>>>" Visible ="false" />--%>
                                        <%--<asp:Label ID="lblVLNewExpiryDtm" runat="server" Text='<%# Eval("New_Expiry_Date", "{0:dd MMM yyyy}")%>' CssClass="textChi"  Visible ="false" ></asp:Label>--%>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px"/>
                                </asp:TemplateField>
                                   <asp:TemplateField SortExpression="Lot_Assign_Status" HeaderText="<%$ Resources:Text, VaccineLotAssignStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLLotAssignStatus" runat="server" Text='<%# Eval("Lot_Assign_Status")%>'></asp:Label>
                                        <asp:label ID="lblVLSymbol" runat="server" Text="<br>>>" Visible ="false" />
                                        <asp:Label ID="lblVLNewLotAssignStatus" runat="server" Text='<%# Eval("New_Lot_Assign_Status")%>' CssClass="textChi" Visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="160px" />
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Request_Type" HeaderText="<%$ Resources:Text, RequestType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLRequestType" runat="server" Text='<%# Eval("Request_Type")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_status" HeaderText="<%$ Resources:Text, VaccineLotRecordStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLRecordStatus" runat="server" Text='<%# Eval("Record_status")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="140px" />
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Create_By" HeaderText="<%$ Resources:Text, CreateBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLCreateBy" runat="server" Text='<%# Eval("Create_By")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="60px" />
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Request_by" HeaderText="<%$ Resources:Text, RequestedBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLRequestBy" runat="server" Text='<%# Eval("Request_by")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="60px" />
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Request_Dtm" HeaderText="<%$ Resources:Text, RequestedDtm %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLRequestDtm" runat="server" Text='<%# Eval("Request_Dtm","{0:dd MMM yyyy HH:mm}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
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
                                    <asp:Label ID="lblDetailTitleBrandName" runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" ></asp:Label></td>
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
                                   <%--<asp:Label ID="lblDetailNewExpiryD" runat="server" CssClass="tableText" Visible="false" ></asp:Label>--%>
                                    </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleLotAssignStatus" runat="server" Text="<%$ Resources:Text, VaccineLotAssignStatus %>" Style="position: relative; top: 2px" ></asp:Label>
                                </td>
                                
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailLotAssignStatus" runat="server" CssClass="tableText"></asp:Label>
                                   <asp:Label ID="lblDetailNewLotAssignStatus" runat="server" CssClass="tableText" Visible="false" ></asp:Label>
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


            <%-- Popup for Remove Button, If the Vaccine Lot has been assigned to any Centres--%>
            <asp:Panel ID="panCentreList" runat="server" Style="display: none;">
                <asp:Panel ID="panCentreListHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 780px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblCentreListHeading" runat="server" Text="<%$ Resources:Text, Notice %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 780px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 10px 10px 10px 10px" align="left">
                            <asp:Panel ID="panCentreContent" runat="server"  Height="440px"  >                          
                                <%--<asp:Label ID="lblPopUpRemark" runat="server" style="font-weight: bold; font-size: 18px;" Text="<%$ Resources:Text, VaccineLotCreationPopupRemark %>"></asp:Label>--%>
                                 <cc1:MessageBox ID="popupMsgBox" runat="server" Width="740px" />
                                <br />                               
                                 <asp:GridView ID="gvCentre" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="False" Width="742px">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="10px" />
                                        </asp:TemplateField>
                                
                                        <asp:TemplateField SortExpression="Centre_Name" HeaderText="<%$ Resources:Text, CentreDHClinicName %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblVLBrandName" runat="server" Text='<%# Eval("Centre_Name")%>'></asp:Label>                                      
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="590px" />
                                        </asp:TemplateField>       
                                        
                                         <asp:TemplateField SortExpression="center_service_type" HeaderText="<%$ Resources:Text, CentreServiceType %>" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceType" runat="server" Text='<%# Eval("centre_service_type")%>'></asp:Label>                                      
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                        </asp:TemplateField>                              
                                    </Columns>
                               </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="ibtnCloseCentreList" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseCentreList_Click" /><br /><br /></td>
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
            <asp:Button runat="server" ID="btnHiddenCentreList" Style="display: none" />
            <cc2:ModalPopupExtender ID="popupCentreList" runat="server" TargetControlID="btnHiddenCentreList"
                PopupControlID="panCentreList" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panCentreListHeading">
            </cc2:ModalPopupExtender>
            <%-- End of  Popup for Remove Button, If the Vaccine Lot has been assigned to any Centrese --%>

        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>
