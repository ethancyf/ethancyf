<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="VaccineLotCreation.aspx.vb" Inherits="HCVU.VaccineLotCreation" Title="<%$ Resources:Title, VaccineLotCreation %>" %>

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
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, VaccineLotCreationBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, VaccineLotCreationBanner %>" /></td>
                </tr>
            </table>
            <cc1:InfoMessageBox ID="udcInfoBox" runat="server" Width="95%" />
            <cc1:MessageBox ID="msgBox" runat="server" Width="95%" />
            <asp:Panel ID="pnlEnquiry" runat="server">
                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">
                        <table>
                            <tr style="height: 25px;">
                                <td style="width:180px;">
                                    <asp:Label ID="lblBrandName" runat="server" Text="<%$ Resources:Text, VaccineName %>"></asp:Label>
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlBrand" runat="server" AppendDataBoundItems="True" width="560px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 25px;">
                                <td>
                                    <asp:Label ID="lblVaccLotNo" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtVaccLotNo" runat="server" MaxLength="20" onBlur="Upper(event,this)" Width="226px"></asp:TextBox></td>
                            </tr>
               
                            <tr style="height: 25px;">
                                <td>
                                    <asp:Label ID="lblExpiryDate" runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpiryFrom" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="imgExpiryFrom" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc2:CalendarExtender ID="CalendarExtender1" CssClass="ajax_cal" runat="server" PopupButtonID="imgExpiryFrom"
                                        TargetControlID="txtExpiryFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc2:CalendarExtender>
                                    <cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers" TargetControlID="txtExpiryFrom" ValidChars="-"></cc2:FilteredTextBoxExtender>
 
                                     To
                                    <asp:TextBox ID="txtExpiryTo" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="imgExpiryTo" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc2:CalendarExtender ID="CalendarExtender2" CssClass="ajax_cal" runat="server" PopupButtonID="imgExpiryTo"
                                        TargetControlID="txtExpiryTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc2:CalendarExtender>
                                    <cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom, Numbers" TargetControlID="txtExpiryTo" ValidChars="-"></cc2:FilteredTextBoxExtender>
                                    <asp:Image ID="imgExpiryDateFromErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -5px" />

                                </td>
                            </tr>
                             <tr style="height: 25px;">
                                <td>
                                    <asp:Label ID="lblRecordStatus" runat="server" Text="<%$ Resources:Text, VaccineLotRecordStatus %>" ></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlRecordStatus" runat="server" AppendDataBoundItems="True" Width="230px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                        <table style="width: 50%; padding-top: 15px;" >
                            <tr>
                                <td align="left" style="padding-right: 5px; padding-left: 180px; width: 100px;">
                                    <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="ibtnNew" runat="server" AlternateText="<%$ Resources:AlternateText, NewRecordBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CreateNewLotBtn %>" OnClick="ibtnNew_Click" />
                                </td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredVaccLotNo" runat="server" TargetControlID="txtVaccLotNo"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars='-_()'></cc2:FilteredTextBoxExtender>
                    </asp:View>
                    <asp:View ID="ViewSearchResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />

                        <uc3:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="pnlSearchCriteriaReview" />

                        <asp:Panel ID="pnlSearchCriteriaReview" runat="server">
                            <table>
                                <tr>
                                    <td valign="top" style="width: 200px">
                                        <asp:Label ID="lblResultBrandText" runat="server" Text="<%$ Resources:Text, VaccineName %>"></asp:Label>
                                     </td>
                                    <td valign="top" style="width: 750px">
                                        <asp:Label ID="lblResultBrand" runat="server" CssClass="tableText"></asp:Label>
                                       </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblResultLotNoText" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" ></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultLotNo" runat="server" CssClass="tableText" ></asp:Label></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblResultExpDtmText" runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultExpDtm" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td valign="top"> <asp:Label ID="lblNewResultRecordSText" runat="server" Text="<%$ Resources:Text, VaccineLotRecordStatus %>"></asp:Label></td>
                                    <td valign="top"> <asp:Label ID="lblNewResultRecordS" runat="server" CssClass="tableText"></asp:Label></td>
                                     <td valign="top"></td>
                                    <td valign="top"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="True" Width="1060px">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField SortExpression="Brand_Trade_Name" HeaderText="<%$ Resources:Text, VaccineName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLBrandName" runat="server" Text='<%# Eval("Brand_Trade_Name")%>'></asp:Label><br />
                                         <asp:Label ID="hfVLBrandId" runat="server" Text='<%# Eval("Brand_ID")%>' visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="600px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Vaccine_Lot_No" HeaderText="<%$ Resources:Text, VaccineLotNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnVLNo" runat="server" Text='<%# Eval("Vaccine_Lot_No")%>' CommandArgument='<%# Eval("Vaccine_Lot_No")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Expiry_Date" HeaderText="<%$ Resources:Text, VaccineLotExpiryDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLExpiryDtm" runat="server" Text='<%# Eval("Expiry_Date", "{0:dd MMM yyyy}")%>' CssClass="textChi"></asp:Label>
                                        <%--<asp:label ID="lblVLSymbol" runat="server" Text="<br>>>" Visible ="false" />--%>
                                        <%--<asp:Label ID="lblVLNewExpiryDtm" runat="server" Text='<%# Eval("New_Expiry_Date", "{0:dd MMM yyyy}")%>' CssClass="textChi" Visible="false" />--%>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="140px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Lot_Assign_Status" HeaderText="<%$ Resources:Text, VaccineLotAssignStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLLotAssignStatus" runat="server" Text='<%# Eval("Lot_Assign_Status")%>' CssClass="textChi"></asp:Label>
                                        <asp:label ID="lblVLSymbol" runat="server" Text="<br>>>" Visible ="false" />
                                        <asp:Label ID="lblVLNewLotAssignStatus" runat="server" Text='<%# Eval("New_Lot_Assign_Status")%>' CssClass="textChi" Visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="140px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Request_type" HeaderText="<%$ Resources:Text, RequestType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestType" runat="server" Text='<%# Eval("Request_Type")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, VaccineLotRecordStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLStatus" runat="server" Text='<%# Eval("Record_Status")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
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
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:ImageButton ID="ibtnSearchResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click" />
<%--                                       <asp:ImageButton ID="ibtnSearchResultNew" runat="server" AlternateText="<%$ Resources:AlternateText, NewRecordBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, NewRecordBtn %>" OnClick="ibtnNew_Click" />--%>

                                </td>
                            </tr>
                        </table>
                    </asp:View>
                   
                    <asp:View ID="ViewConfirm" runat="server">
                       
                        <div class="headingText">
                            <asp:Label ID="lblNewRecordConfirmHeading" runat="server" Text="<%$ Resources: Text, ConfirmDetail%>"></asp:Label>
                        </div>
                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineBrandName" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblBrandWarning" runat ="server" Text="(Cannot be modified)" style="color:red"  CssClass="tableText" visible="false"/>
                                   <asp:Label ID="hfConVaccineBrandId" runat="server" Text="" visible="false"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineLotNo" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblLotNoWarning" runat ="server" Text="(Cannot be modified)" style="color:red"  CssClass="tableText" visible="false"/>
                                      <%--<asp:Label ID="hfConVaccineLotNo" runat="server" visible="false"/>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineExpiryDateText" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblExpiryDateWarming" runat ="server" Text="(Cannot be modified)" style="color:red"  CssClass="tableText" visible="false"/>
                                     <%--<asp:Label ID="hfConVaccineExpiryDateText" runat="server" visible="false" />--%>
                                </td>
                            </tr>
                             <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotAssignStatus %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConLotAssignStatus" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblConLotAssignStatusItem" runat="server" Text="" visible="false"/>
                                    <asp:Label ID="lblConLotNewAssignStatus" runat="server" Text="" visible="false" CssClass="tableText"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnRecordBackToInput" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnRecordBackToInput_Click" />
                                </td>
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnRecordConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnRecordConfirm_Click" />
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

                    <!--New/Edit Lot Record-->
                    <asp:View ID="ViewNewEditLotRecord" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblNewEditRecordHeading" runat="server" Text="<%$ Resources: Text, VaccineLotRecord%>"></asp:Label>
                        </div>

                        <asp:Panel ID="panNewRecord" runat="server">

                            <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">

                                <tr>
                                    <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="lblVaccineBrandName" runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlVaccineBrandName" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 560px; position: relative; top: -1px" AutoPostBack="true" />
                                                     <asp:Label ID="lblEditBrandName" runat="server"   visible="false" CssClass="tableText" />
                                                    <asp:Label ID="lblEditBrandID" runat="server"   visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVaccineBrandNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr >
                                        <td  class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                            <asp:Label ID="lblVaccineLotNo" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                        </td>
                                       
                                         <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                          <asp:textbox ID="txtVaccineLotNo" runat="server"  MaxLength="20" onBlur="Upper(event,this)" Width="234px" />
                                               <asp:Label ID="lblEditVaccineLotNo" runat="server"   visible="false" CssClass="tableText"/>
                                          <asp:Image ID="imgVaccineLotNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                           </td>
                                  </tr>
                                        <tr  >
                                            <td  class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" >
                                                <asp:Label ID="lblVaccineExpiryDate" runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                            </td>
                                            <td  class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                             <asp:Label ID="lblEditExpiryDateText" runat="server"   visible="false" CssClass="tableText"/>
                                                            <asp:TextBox ID="txtVaccineExpiryDateText" runat="server" MaxLength="10" Width="75px" />
                                                            &nbsp;<asp:ImageButton ID="imgExpiry" runat="server" ImageAlign="AbsMiddle"
                                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                            <cc2:CalendarExtender ID="CalendarExtender3" CssClass="ajax_cal" runat="server" PopupButtonID="imgExpiry"
                                                                TargetControlID="txtVaccineExpiryDateText" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                                StartDate="01-01-2009"></cc2:CalendarExtender>
                                                            <cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom, Numbers" TargetControlID="txtVaccineExpiryDateText" ValidChars="-"></cc2:FilteredTextBoxExtender>

                                                            <asp:Image ID="imgVaccineExpiryDateTextErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                                        </td>
                                                    </tr>
                                        </table>
                                    </td>
                                </tr>
                                 <tr >
                                        <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                            <asp:Label ID="lblLotAssignStatus" runat="server" Text="<%$ Resources:Text, VaccineLotAssignStatus %>" Style="position: relative; top: 2px" />
                                        </td>
                                       
                                         <td  class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">  
                                          <asp:DropDownList ID="ddlVaccineLotAssignStatus" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 240px; position: relative; top: -1px"   />
                                              
                                          <asp:Image ID="imgLotAssignErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                           </td>
                                  </tr>
                                       
                              
                          
                               
                            </table>
                            <cc2:FilteredTextBoxExtender ID="txtVaccineLotNoFilter" runat="server" TargetControlID="txtVaccineLotNo"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="-_()"></cc2:FilteredTextBoxExtender>
                        </asp:Panel>

               

                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0"  >
                            <tr>
                                <td style="padding-top: 15px;" width="200px">
                                    <asp:ImageButton ID="ibtnRecordCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnRecordCancel_Click" />
                                </td>
                                <td style="padding-top: 15px;" >
                                    <asp:ImageButton ID="ibtnRecordSave" runat="server" ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, SaveBtn %>" OnClick="ibtnRecordSave_Click" />
                                </td>
                            </tr>
                        </table>

                    </asp:View>
                   
                    <!--Edit Lot Record Detail-->
                    <asp:View ID="ViewEditLotRecordDetail" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblERecordHeading" runat="server" Text="<%$ Resources: Text, VaccineLotRecord%>"></asp:Label>
                        </div>
                         <asp:Panel ID="panEditRecord" runat="server">
                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <asp:Label ID="hfEVaccineLotNo" runat="server" visible="false" />
                            <asp:Label ID="hfEVaccineLotBrand" runat="server" Visible="false"  />
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineBrandName" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" visible="false" />
                                    <asp:Label ID="lblEConVaccineBrandTradeName" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText"  />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineLotNo" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineExpiryDateText" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <%--<asp:Label ID="lblEConVaccineNewExpiryDateText" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" visible="false"/>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotAssignStatus %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineLotAssignStatus" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblEConNewVaccineLotAssignStatus" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" visible="false" />
                                    <asp:Label ID="lblEConVaccineLotAssignStatusItem" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" visible="false"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotRecordStatus %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineNewRecordStatus" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr id="trinfoApprovedBy"  runat="server">
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblETitleApprovedBy" runat="server" Text="<%$ Resources:Text, ApprovedBy %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEApprovedBy" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr id="trinfoRequestType"  runat="server">
                               <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top" >
                                    <asp:Label ID="lblETitleRequestType" runat="server" Text="<%$ Resources:Text, RequestType %>"></asp:Label></td>
                               <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblERequestType" runat="server" CssClass="tableText"></asp:Label>
                                    
                                    
                                </td>
                            </tr>
                            <tr id="trinfoRequestedBy" runat="server">
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label  ID="lblETitleRequestedBy" runat="server" Text="<%$ Resources:Text, RequestedBy %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblERequestedBy" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, CreateBy %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblCreatedBy" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                          
                        </table>
                       
                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0" border="0">
                              <tr>
                                <td style="padding-top: 15px;" width="200px" >
                                    <asp:ImageButton ID="ibtnEditRecordBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnEditRecordBack_Click" />
                                </td>
                              <td style="padding-top: 15px;" align="left" width="110px" >
                                    <asp:ImageButton ID="ibtnEditRecordEdit" runat="server" ImageUrl="<%$ Resources:ImageUrl, EditBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, EditBtn %>" OnClick="ibtnEditRecordEdit_Click" />
                                </td>
                                <td style="padding-top: 15px;" align="left" width="110px" >
                                    <asp:ImageButton ID="ibtnEditRecordRemove" runat="server" ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, RemoveBtn %>" OnClick="ibtnEditRecordRemove_Click" />
                                </td>

                                <td style="padding-top: 15px;" align="left" >
                                    <asp:ImageButton ID="ibtnEditRecordCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelRequestBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CancelRequestBtn %>" OnClick="ibtnEditRecordCancel_Click" />
                                </td>
                            </tr>
                        </table> 


                         </asp:Panel> 
                    </asp:View>






                </asp:MultiView>
            </asp:Panel>
            <asp:Button runat="server" ID="btnHiddenEditRecordRemove" Style="display: none" />
             <cc2:ModalPopupExtender ID="ModalPopupConfirmRemove" runat="server" BackgroundCssClass="modalBackgroundTransparent" 
                TargetControlID="btnHiddenEditRecordRemove" PopupControlID="panPopupConfirmRemove" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmRemove" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmRemove" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, ConfirmRemoveLotRecord %>" />
            </asp:Panel>
 
            <cc2:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnEditRecordCancel" PopupControlID="panPopupConfirmCancel" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmCancel" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmCancel" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, ConfirmCancelRequest %>" />
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
                                                <asp:Label ID="lblCentreName" runat="server" Text='<%# Eval("Centre_Name")%>'></asp:Label>                                      
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
