<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="VaccineLotApproval.aspx.vb" Inherits="HCVU.VaccineLotApproval" Title="<%$ Resources:Title, VaccineLotApproval %>" %>

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
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, VaccineLotApprovalBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, VaccineLotApprovalBanner %>" /></td>
                </tr>
            </table>
            <cc1:InfoMessageBox ID="udcInfoBox" runat="server" Width="95%" />
            <cc1:MessageBox ID="msgBox" runat="server" Width="95%" />
            <asp:Panel ID="pnlEnquiry" runat="server">
                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">
                        <table >
                            <tr>
                                <td style="width: 130px">
                                    <asp:Label ID="lblVaccCentre" runat="server" Text="<%$ Resources:Text, VaccineCentre %>"></asp:Label></td>
                                <td style="width: 350px">
                                    <asp:DropDownList ID="ddlVaccCentre" runat="server" AppendDataBoundItems="True" >
                                    </asp:DropDownList>
                                    <asp:Image ID="imgVaccCentreAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                                 
                            </tr>
                           
                            <tr>
                                <td style="width: 130px">
                                    </td>
                                <td style="width: 350px">
                                    
                                </td>
                            </tr>
                        </table>
                        <table style="width: 380px">
                            <tr>
                                <td align="center" style="padding-top: 10px">
                                    <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                   </td>
                            </tr>
                        </table>
                        
                    </asp:View>
                    <asp:View ID="ViewSearchResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />
                        
                        <uc3:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="pnlSearchCriteriaReview" />
                        
                        <asp:Panel ID="pnlSearchCriteriaReview" runat="server">
                            <table >
                                <tr>
                                    <td valign="top" style="width: 130px">
                                        <asp:Label ID="lblResultVaccineCentreText" runat="server" Text="<%$ Resources:Text, VaccineCentre %>"></asp:Label></td>
                                    <td valign="top" style="width: 650px">
                                        <asp:Label ID="lblResultCentreNo" runat="server" CssClass="tableText" ></asp:Label></td>
                                    <td valign="top" style="width: 130px">
                                         
                                        </td>
                                    <td valign="top" style="width: 650px">
                                         
                                        </td>
                                </tr>
                                
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="True" Width="1190px">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="5px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Request_ID" HeaderText="<%$ Resources:Text, RequestID %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnRequestID" runat="server" Text='<%# Eval("Request_ID")%>' CommandArgument='<%# Eval("Request_ID")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="50px"/>
                                </asp:TemplateField>
                                 
                                <asp:TemplateField SortExpression="Centre_Name" HeaderText="<%$ Resources:Text, VaccineCentre %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLCentreID" runat="server" Text='<%# Eval("Centre_Name")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="250px"/>
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Booth" HeaderText="<%$ Resources:Text, BoothOutreach %>" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLBooth" runat="server" Text='<%# Eval("Booth")%>'  width="70px" style="word-break: break-all" ></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="BrandName" HeaderText="<%$ Resources:Text, VaccineName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLBrandName" runat="server" Text='<%# Eval("BrandName")%>' Visible="false" ></asp:Label>
                                        <asp:Label ID="lblVLBrandTradeName" runat="server" Text='<%# Eval("Brand_Trade_Name")%>' ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="250px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Vaccine_Lot_No" HeaderText="<%$ Resources:Text, VaccineLotNo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLNo" runat="server" Text='<%# Eval("Vaccine_Lot_No")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="50px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Expiry_Date" HeaderText="<%$ Resources:Text, VaccineLotExpiryDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLExpDtm" runat="server" Text='<%# Eval("Expiry_Date", "{0:dd MMM yyyy}")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="105px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Request_EffectiveDateFrom" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLEffFrom" runat="server" Text='<%# Eval("Request_EffectiveDateFrom", "{0:dd MMM yyyy}")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="95px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Request_EffectiveDateTo" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateTo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLEffTo" runat="server" Text='<%# Eval("Request_EffectiveDateTo", "{0:dd MMM yyyy}")%>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="95px"/>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField SortExpression="Request_type" HeaderText="<%$ Resources:Text, RequestType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestType" runat="server" Text='<%# Eval("Request_type")%>'></asp:Label>
                                     </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="50px"/>
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, VaccineLotRecordStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLStatus" runat="server" Text='<%# Eval("Record_Status") %>'></asp:Label>
                                     </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="50px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Requested_By" HeaderText="<%$ Resources:Text, RequestedBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLRequestedBy" runat="server" Text='<%# Eval("Requested_By")%>'></asp:Label>
                                     </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="50px"/>
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
                            <asp:Label ID="lblVaccineLotTitle" runat="server" Text="<%$ Resources:Text,VaccineLotRecord %>"></asp:Label>
                        </div>
                        <table  style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <tr>
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleRequestID" runat="server" Text="<%$ Resources:Text, RequestID %>" Style="position: relative; top: 2px" ></asp:Label></td>
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailRequestID" runat="server" CssClass="tableText"></asp:Label>
                                   
                                </td>
                            </tr>
                            <tr>
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleVaccineCentre" runat="server" Text="<%$ Resources:Text, VaccineCentre %>" Style="position: relative; top: 2px" ></asp:Label></td>
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailVaccineCentre" runat="server" CssClass="tableText"></asp:Label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleBooth" runat="server" Text="<%$ Resources:Text, Booth%>" Style="position: relative; top: 2px" ></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailBooth" runat="server" CssClass="tableText"></asp:Label>
                                     
                                     
                                   </td>
                            </tr>
                            <tr id="trBrandName" runat="server" visible="false" >
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleBrandName" runat="server" Text="<%$ Resources:Text, VaccineBrandName %>" Style="position: relative; top: 2px" ></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailBrandName" runat="server" CssClass="tableText"></asp:Label>
                                     
                                     
                                   </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailTitleBrandTradeName" runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" ></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailBrandTradeName" runat="server" CssClass="tableText"></asp:Label>
                                     
                                     
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
                                    </td>
                            </tr>
                            <tr id="trEffectiveFrom" runat="server">
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top;position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleEffectiveFrom" runat="server" Text="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>"></asp:Label></td>
                                
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailEffectiveFrom" runat="server" CssClass="tableText"></asp:Label>
                                    <%--<asp:Label ID="lblDetailNewEffectiveFrom" runat="server" CssClass="tableText" Visible="false" ></asp:Label>--%>
                                   </td>
                            </tr>
                            <tr id="trEffectiveTo" runat="server">
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top;position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleEffectiveTo" runat="server" Text="<%$ Resources:Text, VaccineLotEffectiveDateTo %>"></asp:Label>
                                     
                                </td>
                               
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailEffectiveTo" runat="server" CssClass="tableText"></asp:Label>
                                   <asp:Label ID="lblDetailUpToExpiryDtm" runat="server" CssClass="tableText" Visible="false" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                     <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                            AllowSorting="False" Width="780">
                            <Columns>
                              <%--  <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSummaryIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField> --%>
                               <asp:TemplateField SortExpression="Booth_Name" HeaderText="<%$ Resources:Text, Booth %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSummaryBoothNo" runat="server" Text='<%# Eval("Booth_Name")%>'></asp:Label>
                                        <asp:Label ID="lblRequestType" runat="server" Text='<%# Eval("Request_Type")%>' visible="false" ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="80px"  HorizontalAlign="Center" />
                                </asp:TemplateField>
                                 
                                <asp:TemplateField SortExpression="Effective_From" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSummaryEffFrom" runat="server" Text='<%# Eval("Mapping_Service_Period_From", "{0:dd MMM yyyy}")%>'></asp:Label>
                                        <asp:Label ID="lblsymbolFrom" runat="server" Text=" >> " style="color : red " visible="false" />
                                        <asp:Label ID="lblSummaryNewEffFrom" runat="server" Text='<%# Eval("Request_Service_Period_From", "{0:dd MMM yyyy}")%>' style="color : red "></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="200px"  HorizontalAlign="Center"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Effective_To" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateTo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSummaryUseExpiryDtm" runat="server" Text='<%# Eval("Use_Of_ExpiryDtm")%>' visible="false" /> 
                                        <asp:Label ID="lblSummaryEffTo" runat="server" Text='<%# Eval("Mapping_Service_Period_To", "{0:dd MMM yyyy}")%>'></asp:Label>
                                        <asp:Label ID="lblsymbolTo" runat="server" Text=" >> " style="color : red " visible="false" />
                                         <asp:Label ID="lblSummaryNewEffTo" runat="server" Text='<%# Eval("Request_Service_Period_To", "{0:dd MMM yyyy}")%>' style="color : red "></asp:Label>
                                    </ItemTemplate>
                               
                                    <ItemStyle VerticalAlign="Top"  Width="250px"  HorizontalAlign="Center"/>
                                </asp:TemplateField>
                                
                                </Columns> 

                                        </asp:GridView> 
                                </td>
                            </tr>
                           <%-- <tr>
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" Style="position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleLotStatus" runat="server" Text="<%$ Resources:Text, VaccineLotStatus %>"></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailLotStatus" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
                                          </td>
                            </tr>--%>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" Style="position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleNewRecordStatus" runat="server" Text="<%$ Resources:Text, VaccineLotRecordStatus %>"></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailNewRecordStatus" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
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
                            <%--<tr>
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" Style="position: relative; top: 2px" >
                                    <asp:Label ID="lblDetailTitleCreatedBy" runat="server" Text="<%$ Resources:Text, CreateBy %>"></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblDetailCreatedBy" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
                                </td>
                            </tr>--%>
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
                                    &nbsp;
                                    <asp:HiddenField ID="hfDetailTSMP" runat="server" />
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                        </table>
                    

                    </asp:View>

                    <asp:View ID="ViewMsg" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnConfirmBack" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnConfirmBack_Click" />

                        &nbsp;<asp:ImageButton ID="ibtnNoRecordBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnNoRecordBack_Click" />
                    </asp:View>                    
                    <asp:View ID="ViewError" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnReturn" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnReturn_Click" /></asp:View>
                    
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
