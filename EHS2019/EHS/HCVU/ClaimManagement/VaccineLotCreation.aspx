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
                                <td>
                                    <asp:Label ID="lblBrandName" runat="server" Text="<%$ Resources:Text, VaccineBrandName %>"></asp:Label>
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlBrand" runat="server" AppendDataBoundItems="True" width="238px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 25px;">
                                <td>
                                    <asp:Label ID="lblVaccLotNo" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtVaccLotNo" runat="server" MaxLength="20" onBlur="Upper(event,this)" Width="234px"></asp:TextBox></td>
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
                                    <asp:Label ID="lblNewRecordStatus" runat="server" Text="<%$ Resources:Text, VaccineLotRecordStatus %>" ></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlNewRecordStatus" runat="server" AppendDataBoundItems="True" Width="155px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                        <table style="width: 50%">
                            <tr>
                                <td align="center" style="padding-top: 10px; padding-right: 5px">
                                    <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" />
                               <%-- </td>
                                <td align="left" style="padding-top: 10px; padding-left: 5px">--%>
                                    <asp:ImageButton ID="ibtnNew" runat="server" AlternateText="<%$ Resources:AlternateText, NewRecordBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, NewRecordBtn %>" OnClick="ibtnNew_Click" />
                                </td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredVaccLotNo" runat="server" TargetControlID="txtVaccLotNo"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers"></cc2:FilteredTextBoxExtender>
                    </asp:View>
                    <asp:View ID="ViewSearchResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />

                        <uc3:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="pnlSearchCriteriaReview" />

                        <asp:Panel ID="pnlSearchCriteriaReview" runat="server">
                            <table>
                                <tr>
                                    <td valign="top" style="width: 200px">
                                        <asp:Label ID="lblResultBrandText" runat="server" Text="<%$ Resources:Text, VaccineBrandName %>"></asp:Label>
                                     </td>
                                    <td valign="top" style="width: 350px">
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
                            AllowSorting="True" Width="980">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Vaccine_Lot_No" HeaderText="<%$ Resources:Text, VaccineLotNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnVLNo" runat="server" Text='<%# Eval("Vaccine_Lot_No")%>' CommandArgument='<%# Eval("Vaccine_Lot_No")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="BrandName" HeaderText="<%$ Resources:Text, VaccineBrandName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLBrandName" runat="server" Text='<%# Eval("BrandName")%>'></asp:Label><br />
                                         <asp:hiddenField ID="hfVLBrandId" runat="server" value='<%# Eval("BrandID")%>'></asp:hiddenField>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ExpiryDate" HeaderText="<%$ Resources:Text, VaccineLotExpiryDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLExpiryDtm" runat="server" Text='<%# Eval("ExpiryDate", "{0:dd MMM yyyy}")%>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Request_type" HeaderText="<%$ Resources:Text, RequestType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestType" runat="server" Text='<%# Eval("Request_Type")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, VaccineLotRecordStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLStatus" runat="server" Text='<%# Eval("Record_Status")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="120px" />
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
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineBrandName %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineBrandName" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:HiddenField ID="hfConVaccineBrandId" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineLotNo" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                      <asp:HiddenField ID="hfConVaccineLotNo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineExpiryDateText" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                     <asp:HiddenField ID="hfConVaccineExpiryDateText" runat="server" />
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
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="lblVaccineBrandName" runat="server" Text="<%$ Resources:Text, VaccineBrandName %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlVaccineBrandName" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 240px; position: relative; top: -1px" AutoPostBack="true" />
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVaccineBrandNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <td >
                                            <asp:Label ID="lblVaccineLotNo" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                        </td>
                                       
                                         <td>
                                          <asp:textbox ID="txtVaccineLotNo" runat="server"  MaxLength="20" onBlur="Upper(event,this)" Width="234px" />

                                          <asp:Image ID="imgVaccineLotNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                           </td>
                                  </tr>
                                        <tr style="height: 25px; padding-top: 8px; vertical-align: top">
                                            <td style="width: 200px;" >
                                                <asp:Label ID="lblVaccineExpiryDate" runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                            </td>
                                            <td >
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
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

                                       
                              
                          
                               
                            </table>
                            <cc2:FilteredTextBoxExtender ID="txtVaccineLotNoFilter" runat="server" TargetControlID="txtVaccineLotNo"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=""></cc2:FilteredTextBoxExtender>
                        </asp:Panel>

                        <%--<asp:Panel ID="panEditRecord" runat="server">
                            <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Text, VaccineCentre %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPERConVaccineCentre" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                                  <asp:HiddenField ID="hfPERConVaccineCentre" runat="server" />
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Text, Booth %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPERConVaccineBooth" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                                     <asp:HiddenField ID="hfPERConVaccineBooth" runat="server" />
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>


                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Text, VaccineBrandName %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPERConVaccineBrand" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                                     <asp:HiddenField ID="hfPERConVaccineBrand" runat="server" />
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPERConVaccineLotNo" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                                    <asp:HiddenField ID="hfPERConVaccineLotNo" runat="server" />
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                  
                            </table>
                        </asp:Panel>--%>

                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <tr>
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnRecordCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnRecordCancel_Click" />
                                </td>
                                <td style="padding-top: 15px;">
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
                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <asp:HiddenField ID="hfEVaccineLotNo" runat="server" />
                            <asp:HiddenField ID="hfEVaccineLotBrand" runat="server" />
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineBrandName %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineBrandName" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
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

                        <table>
                              <tr>
                                <td width="220px" style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnEditRecordBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnEditRecordBack_Click" />
                                </td>
<%--                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnEditRecordEdit" runat="server" ImageUrl="<%$ Resources:ImageUrl, EditBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, EditBtn %>" OnClick="ibtnEditRecordEdit_Click" />
                                </td>--%>
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnEditRecordRemove" runat="server" ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, RemoveBtn %>" OnClick="ibtnEditRecordRemove_Click" />
                                </td>

                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnEditRecordCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelRequestBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CancelRequestBtn %>" OnClick="ibtnEditRecordCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>






                </asp:MultiView>
            </asp:Panel>

             <cc2:ModalPopupExtender ID="ModalPopupConfirmRemove" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnEditRecordRemove" PopupControlID="panPopupConfirmRemove" PopupDragHandleControlID="" RepositionMode="None">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>
