<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="VaccineLotManagement.aspx.vb" Inherits="HCVU.VaccineLotManagement" Title="<%$ Resources:Title, VaccineLotManagement %>" %>

<%@ Register Src="~/ServiceProvider/spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>
    <link rel="stylesheet" href="../css/bootstrap.css" type="text/css">
    <link rel="stylesheet" href="../css/bootstrap-multiselect.css" type="text/css">
    <script type="text/javascript" src="../js/bootstrap.bundle-4.5.2.min.js"></script>
    <script type="text/javascript" src="../js/bootstrap-multiselect.js"></script>




    <script type="text/javascript">

        var lbVaccineCentreBooth_id = "<%=lbVaccineCentreBooth.ClientID%>";

        $(document).ready(function () {

            bindMultiselectForBooth();
            bindBoothSelectedLabel();
        

        });
        function bindMultiselectForBooth() {
            $("#" + lbVaccineCentreBooth_id).hide();
            
            
            $("#" + lbVaccineCentreBooth_id).multiselect({
                includeSelectAllOption: true,
                maxHeight: 350,
                numberDisplayed: 1,
                onChange: function (element, checked) {
                    var brands = $('#' + lbVaccineCentreBooth_id + ' option:selected');
                    var selected = 'Selected: ';
                    var i = 0;
                    $(brands).each(function (index, brand) {
                        selected += $(this).html();
                        if (i != (brands).length - 1) {
                            selected += ',';
                        } ++i;
                    });
                    $('#lbVaccineCentreSelectedLabel').html(selected);
                },
                onSelectAll: function (element, checked) {
                    var brands = $('#' + lbVaccineCentreBooth_id +' option:selected');
                    var selected = 'Selected: ';
                    var i = 0;
                    $(brands).each(function (index, brand) {
                        selected += $(this).html();
                        if (i != (brands).length - 1) {
                            selected += ',';
                        } ++i;
                    });
                    $('#lbVaccineCentreSelectedLabel').html(selected);
                },
                onDeselectAll: function (element, checked) {
                    var brands = $('#' + lbVaccineCentreBooth_id + ' option:selected');
                    var selected = 'Selected: ';
                    var i = 0;
                    $(brands).each(function (index, brand) {
                        selected += $(this).html();
                        if (i != (brands).length - 1) {
                            selected += ',';
                        } ++i;
                    });
                    $('#lbVaccineCentreSelectedLabel').html(selected);
                },
                buttonText: function (options, select) {
                    if (options.length > 0) {
                        return options.length + ' selected';
                    }
                    else {
                        return 'None selected';
                    }
                }
            });

        }
        function bindBoothSelectedLabel() {
            var brands = $('#' + lbVaccineCentreBooth_id + ' option:selected');
            var selected = 'Selected: ';
            var i = 0;
            $(brands).each(function (index, brand) {
                selected += $(this).html();
                if (i != (brands).length - 1) {
                    selected += ',';
                } ++i;
            });
            $('#lbVaccineCentreSelectedLabel').html(selected);
        }



        var parameter = Sys.WebForms.PageRequestManager.getInstance();

        parameter.add_endRequest(function () {
            bindMultiselectForBooth();
            bindBoothSelectedLabel();
        });


    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, VaccineLotManagementBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, VaccineLotManagementBanner %>" /></td>
                </tr>
            </table>
            <cc1:InfoMessageBox ID="udcInfoBox" runat="server" Width="95%" />
            <cc1:MessageBox ID="msgBox" runat="server" Width="95%" />
            <asp:Panel ID="pnlEnquiry" runat="server">
                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSummary" runat="server">
                         <%----- ---%>
                        <cc2:TabContainer ID="TabContainerCTM" runat="server" CssClass="m_ajax__tab_xp" Width="1120px" ActiveTabIndex="0"  AutoPostBack="true" >
                            <cc2:TabPanel ID="tabTransaction" runat="server" HeaderText="<%$ Resources:Text, CurrentRecord%>">
                                <ContentTemplate>
                        <table>
                            <tr style="height: 25px;">
                                <td style="width: 80px">
                                    <asp:Label ID="lblSearchVaccCentre" runat="server" Text="<%$ Resources:Text, VaccineCentre %>"></asp:Label></td>
                                <td style="width: 350px">

                                    <asp:DropDownList ID="ddlVaccCentre" runat="server" AppendDataBoundItems="True" AutoPostBack="true" OnSelectedIndexChanged="ddlVaccCentre_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvSummary" runat="server"  AutoGenerateColumns="False" AllowPaging="false"
                            AllowSorting="True" Width="1100px" >
                            <Columns>
                                <asp:TemplateField Visible ="false" >
                                    <ItemTemplate >
                                        <asp:Label ID="lblSResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>

                   
                                <asp:TemplateField SortExpression="Booth_Order" HeaderText="<%$ Resources:Text, BoothOutreach%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLBoothName" runat="server" Text='<%# Eval("Booth_name")%>'></asp:Label>
                                        
                                        <br />
                                        <asp:Label ID="lblSVLBooth" runat="server" Text='<%# Eval("Booth")%>' Visible ="false" ></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Brand_Name" HeaderText="<%$ Resources:Text, VaccineName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLBrandName" runat="server" Text='<%# Eval("Brand_Name")%>' visible="false" ></asp:Label>
                                        <asp:Label ID="lblSVLBrandTradeName" runat="server" Text='<%# Eval("Brand_Trade_Name")%>' />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="220px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Vaccine_Lot_No" HeaderText="<%$ Resources:Text, VaccineLotNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnSkbtnVLNo" runat="server" Text='<%# Eval("Vaccine_Lot_No")%>' CommandArgument='<%# Eval("Vaccine_Lot_ID")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="70px" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField SortExpression="VaccineLotExpiryDate" HeaderText="<%$ Resources:Text, VaccineLotExpiryDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLExpiryDtm" runat="server" Text='<%# Eval("VaccineLotExpiryDate","{0:dd MMM yyyy}")%>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="150px" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField SortExpression="Service_Period_From" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLEffFrom" runat="server" Text='<%# Eval("Service_Period_From", "{0:dd MMM yyyy}")%>'></asp:Label>
                                        <asp:Label ID="lblSVLEffFromSymbol" runat="server" Text="<br>>>"  Visible="false"></asp:Label>
                                        <asp:Label ID="lblSVLNewEffFrom" runat="server" Text='<%# Eval("New_Service_Period_From", "{0:dd MMM yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Service_Period_To" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateTo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLEffTo" runat="server" Text='<%# Eval("Service_Period_To", "{0:dd MMM yyyy}")%>'></asp:Label>
                                        <asp:Label ID="lblSVLEffToSymbol" runat="server" Text="<br>>>" Visible="false"></asp:Label>
                                        <asp:Label ID="lblSVLNewEffTo" runat="server"  Text='<%# Eval("New_Service_Period_To", "{0:dd MMM yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                              
                                <asp:TemplateField SortExpression="Request_type" HeaderText="<%$ Resources:Text, RequestType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSRequestType" runat="server" Text='<%# Eval("Request_type")%>'></asp:Label>
                                        <asp:Label ID="lblLotDetailRecordStatus" runat="server" Text='<%# Eval ("LotDetail_Record_Status") %>' visible="false"  />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Display_Record_Status" HeaderText="<%$ Resources:Text, VaccineLotRecordStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLStatus" runat="server" Text='<%# Eval("Display_Record_Status")%>'></asp:Label>                                        
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="110px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Create_By" HeaderText="<%$ Resources:Text, CreateBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLCreateBy" runat="server" Text='<%# Eval("Create_By")%>'></asp:Label>  
                                     <%--  (<asp:Label ID="lblSVLCreateDtm" runat="server" Text='<%# Eval("Create_Dtm")%>'></asp:Label>)  --%>                                      
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Requested_By" HeaderText="<%$ Resources:Text, RequestedBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSVLRequestedBy" runat="server" Text='<%# Eval("Requested_By")%>'></asp:Label>
                                       <%--<asp:Label ID="lblSVLRequestedDtm" runat="server" Text='<%# Eval("Requested_Dtm")%>'></asp:Label>--%>                                      
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="70px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table>
                               <tr>
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="btnBatchAssign" runat="server" ImageUrl="<%$ Resources:ImageUrl, BatchAssignBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, BatchAssignBtn %>" OnClick="ibtnBatchAssign_Click" />
                                </td>
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="btnBatchRemove" runat="server" ImageUrl="<%$ Resources:ImageUrl, BatchRemoveBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, BatchRemoveBtn %>" OnClick="ibtnBatchRemove_Click" />
                                </td>
                            </tr>
                             
                        </table>

                                    </ContentTemplate>
                            </cc2:TabPanel> 
                                    </cc2:TabContainer>
                    </asp:View>              

                    <asp:View ID="ViewConfirm" runat="server">

                        <div class="headingText">
                            <asp:Label ID="lblNewRecordConfirmHeading" runat="server" Text="<%$ Resources: Text, ConfirmDetail%>"></asp:Label>
                        </div>
                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineCentre %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineCentre" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="hfConVaccineCentreId" runat="server" Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, BoothOutreach%>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineBooth" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblConVaccineBoothID" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" Visible="false"  />
                                    <%--<asp:HiddenField ID="hfConVaccineBooth" runat="server" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineBrandName" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <%--<asp:HiddenField ID="hfConVaccineBrandID" runat="server" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineLotNo" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <%--<asp:HiddenField ID="hfConVaccineLotNo" runat="server" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineExpiryDateText" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr id ="trConEffectiveDateFrom" runat="server">
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    
                                    <asp:Label ID ="lblConEffectiveDateFromLabel"  Text="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>"  runat="server" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineLotEffectiveDateFrom" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="hfConVaccineLotEffectiveDateFrom" runat="server" visible="false" Text=""/>
                                    <%--<asp:Label ID="lblNewConVaccineLotEffectiveDateFrom" Text="" runat="server" ForeColor="red" Style="position: relative; top: 2px" CssClass="tableText"></asp:Label>--%>
                                    <asp:Label ID="hfNewConVaccineLotEffectiveDateFrom" runat="server"  Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr id ="trConEffectiveDateTo" runat="server" >
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    
                                     <asp:Label ID ="lblConEffectiveDateToLabel"  Text="<%$ Resources:Text, VaccineLotEffectiveDateTo %>"  runat="server" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblConVaccineLotEffectiveDateTo" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="hfConVaccineLotEffectiveDateTo" runat="server"  Text="" Visible="false" />
                                    <%--<asp:Label ID="lblNewConVaccineLotEffectiveDateTo" Text="" runat="server" ForeColor="red" Style="position: relative; top: 2px" CssClass="tableText"></asp:Label>--%>
                                    <asp:Label ID="hfNewConVaccineLotEffectiveDateTo" runat="server"  Text="" Visible="false" />
                                    <asp:Label ID="hfUpToVacExpDateTo" runat="server"  Text="" Visible="false" />
                                </td>
                            </tr>
                        
                        <tr>
                         <td colspan="2" style="vertical-align: middle;  text-align: center; ">
                                    <asp:GridView ID="gvConfirmDetail" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                            AllowSorting="false" Width="900">
                            <Columns>
                                <asp:TemplateField SortExpression="Booth" HeaderText="<%$ Resources:Text, BoothOutreach%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLDetailBooth" runat="server" Text='<%# Eval("Booth")%>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="VaccineLotEffectiveDateFrom" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLDetailEffFrom" runat="server" Text='<%# Eval(" Service_Period_From", "{0:dd MMM yyyy}")%>'></asp:Label>
                                        <asp:Label ID="lblNewVLDetailEffFrom" runat="server" CssClass="textChi" ForeColor="red"></asp:Label>
                                        <br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="400px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="VaccineLotEffectiveDateTo" HeaderText="<%$ Resources:Text, VaccineLotEffectiveDateTo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLDetailEffTo" runat="server" Text='<%# Eval("Service_Period_To", "{0:dd MMM yyyy}")%>' CssClass="textChi"></asp:Label>
                                        <asp:Label ID="lblNewVLDetailEffTo" runat="server" CssClass="textChi" ForeColor="red"></asp:Label>
                                        <asp:Label ID="hfUseOfEpiryDtm" runat="server" Text ='<%# Eval("Use_Of_ExpiryDtm")%>'    visible="false"  />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="400px" />
                                </asp:TemplateField>
                               <asp:TemplateField SortExpression="VaccineLotRecordStatus" HeaderText="<%$ Resources:Text, RecordStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVLDetailRecord_Status" runat="server" Text='<%# Eval("Record_Status")%>' CssClass="textChi"></asp:Label>
                                        <asp:Label ID="lblNewVLDetailRecord_Status" runat="server" CssClass="textChi" ForeColor="red"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="400px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
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
                    <!--New/Edit Lot Record-->
                    <asp:View ID="ViewNewEditLotRecord" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblNewEditRecordHeading" runat="server" Text="<%$ Resources: Text, VaccineLotRecord%>"></asp:Label>
                        </div>

                        <asp:Panel ID="panNewRecord" runat="server">

                            <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="lblResultVaccineCentreText" runat="server" Text="<%$ Resources:Text, VaccineCentre %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlVaccineCentre" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 560px; position: relative; top: -1px" AutoPostBack="true" OnSelectedIndexChanged="ddlVaccineCentre_SelectedIndexChanged" />
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVaccineCentreErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="lblVaccineCentreBoothText" runat="server" Text="<%$ Resources:Text, BoothOutreach%>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <%-- <asp:DropDownList ID="ddlVaccineCentreBooth" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 485px; position: relative; top: -1px" />--%>

                                                    <p>
                                                        <div>
                                                            <asp:ListBox ID="lbVaccineCentreBooth" runat="server" SelectionMode="Multiple" multiple="multiple">
                                                            </asp:ListBox>
                                                        </div>
                                                    </p>
                                                    <p>
                                                        <div id="lbVaccineCentreSelectedLabel">Selected: </div>
                                                    </p>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVaccineCentreBoothErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>


                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="lblVaccineBrandName" runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlVaccineBrandName" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 560px; position: relative; top: -1px" AutoPostBack="true" OnSelectedIndexChanged="ddlVaccineBrandName_SelectedIndexChanged" />
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVaccineBrandNameErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="lblVaccineLotNo" runat="server" Text="<%$ Resources:Text, VaccineLotNo %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlVaccineLotNo" runat="server" AppendDataBoundItems="True" Style="height: 22px; width: 560px; position: relative; top: -1px" AutoPostBack="true" OnSelectedIndexChanged="ddlVaccineLotNo_SelectedIndexChanged" />
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVaccineLotNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                            </table>

                        </asp:Panel>

                        <asp:Panel ID="panEditRecord" runat="server">
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
                                                    <asp:Label ID="hfPERConVaccineCentreID" runat="server"  Text="" Visible="false" />
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Text, BoothOutreach%>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPERConVaccineBooth" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                                    <asp:Label ID="hfPERConVaccineBoothId" runat="server"  Text="" Visible="false" />
                                                    <%--<asp:HiddenField ID="hfPERConVaccineBooth" runat="server" />--%>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>


                                <tr>
                                    <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" />
                                    </td>
                                    <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPERConVaccineBrand" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                                    <%--<asp:HiddenField ID="hfPERConVaccineBrand" runat="server" />--%>
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
                                                    <%--<asp:HiddenField ID="hfPERConVaccineLotNo" runat="server" />--%>
                                                </td>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>

                        <table style="width: 100%; padding-left: 22px" cellpadding="1" cellspacing="0">
                            <tr>
                                <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblVaccineExpiryDate" runat="server" Text="<%$ Resources:Text, VaccineExpiryDate %>" Style="position: relative; top: 2px" />
                                </td>
                                <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblVaccineExpiryDateText" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                                <asp:Image ID="imgVaccineExpiryDateTextErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -1px; left: 1px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <asp:Panel ID="penEffectiveDateCalender" runat="server">
                            <tr>
                                <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblVaccineLotEffectiveDateFrom" runat="server" Text="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>" Style="position: relative; top: 2px" />
                                </td>
                                <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:TextBox ID="txtVaccineLotEffectiveDateFrom" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="btnVaccineLotEffectiveDateFrom" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc2:CalendarExtender ID="CalExtVaccineLotEffectiveDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="btnVaccineLotEffectiveDateFrom"
                                        TargetControlID="txtVaccineLotEffectiveDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc2:CalendarExtender>
                                    <cc2:FilteredTextBoxExtender ID="txtVaccineLotEffectiveDateFromFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtVaccineLotEffectiveDateFrom" ValidChars="-"></cc2:FilteredTextBoxExtender>
                                    <asp:Image ID="imgVaccineLotEffectiveDateFromErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -5px" />

                                    <asp:Label ID="hfVaccineLotEffectiveDateFrom" runat="server"  Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblVaccineLotEffectiveDateTo" runat="server" Text="<%$ Resources:Text, VaccineLotEffectiveDateTo %>" Style="position: relative; top: 2px" />
                                </td>
                                <td style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:TextBox ID="txtVaccineLotEffectiveDateTo" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="btnVaccineLotEffectiveDateTo" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc2:CalendarExtender ID="CalVaccineLotEffectiveDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="btnVaccineLotEffectiveDateTo"
                                        TargetControlID="txtVaccineLotEffectiveDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc2:CalendarExtender>

                                    <asp:CheckBox ID="chkUpToExpiryDate" runat="server" AutoPostBack="True" OnCheckedChanged="chkUpToExpiryDate_click"  Text="<%$ Resources:Text, UpToVacExpDate%>"/>
 
                                    <cc2:FilteredTextBoxExtender ID="txtVaccineEffectiveDateToFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtVaccineLotEffectiveDateTo" ValidChars="-"></cc2:FilteredTextBoxExtender>
                                    <asp:Image ID="imgVaccineLotEffectiveDateToErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position: relative; top: -5px" />

                                    <asp:Label ID="hfVaccineLotEffectiveDateTo" runat="server"  Text="" Visible="false" />
                                </td>

                            </tr>
                          </asp:Panel>
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
                            <asp:Label ID="hfEVaccineLotId" runat="server" Text="" Visible="false"  />
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineCentre %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineCentre" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="hflblEConVaccineCentreId" runat="server"  Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, BoothOutreach%>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineBooth" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="hflEConVaccineBoothId" runat="server"  Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineName %>" Style="position: relative; top: 2px" />
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
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotEffectiveDateFrom %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineLotEffectiveDateFrom" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblENewConVaccineLotEffectiveDateFrom" runat="server" ForeColor="red" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotEffectiveDateTo %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineLotEffectiveDateTo" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblENewConVaccineLotEffectiveDateTo" runat="server" ForeColor="red" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                           <%-- <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotStatus %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineLotStatus" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                    <asp:Label ID="lblENewConVaccineLotStatus" Text="" runat="server" ForeColor="red" Style="position: relative; top: 2px" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label runat="server" Text="<%$ Resources:Text, VaccineLotRecordStatus %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEConVaccineNewRecordStatus" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr id="trinfoApprovedBy" runat="server">
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblETitleApprovedBy"  runat="server" Text="<%$ Resources:Text, ApprovedBy %>" Style="position: relative; top: 2px" />
                                </td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblEApprovedBy" runat="server" Text="" Style="position: relative; top: 2px" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr id="trinfoRequestType" runat="server">
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblETitleRequestType" runat="server" Text="<%$ Resources:Text, RequestType %>"></asp:Label></td>
                                <td class="fontBold" style="height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblERequestType" runat="server" CssClass="tableText"></asp:Label>


                                </td>
                            </tr>
                            <tr id="trinfoRequestedBy" runat="server">
                                <td class="fieldCaption" style="width: 200px; height: 25px; padding-top: 8px; vertical-align: top">
                                    <asp:Label ID="lblETitleRequestedBy" runat="server" Text="<%$ Resources:Text, RequestedBy %>" Style="position: relative; top: 2px" />
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
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnEditRecordEdit" runat="server" ImageUrl="<%$ Resources:ImageUrl, EditBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, EditBtn %>" OnClick="ibtnEditRecordEdit_Click" />
                                </td>
                                <td style="padding-top: 15px;">
                                    <asp:ImageButton ID="ibtnEditRecordRemove" runat="server" ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, RemoveBtn %>" OnClick="ibtnEditRecordRemove_Click" />
                                </td>

<%--                                <td style="padding-top: 15px;" >
                                    <asp:ImageButton ID="ibtnEditRecordCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelRequestBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CancelRequestBtn %>" OnClick="ibtnEditRecordCancel_Click" />
                                </td>--%>
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

<%--            <cc2:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnEditRecordCancel" PopupControlID="panPopupConfirmCancel" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmCancel" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmCancel" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, ConfirmCancelRequest %>" />
            </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>
