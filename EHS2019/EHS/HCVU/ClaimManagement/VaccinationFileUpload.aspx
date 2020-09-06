<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="VaccinationFileUpload.aspx.vb" Inherits="HCVU.VaccinationFileUpload"
    Title="<%$ Resources: Title, VaccinationFileUpload %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/StudentFile/ucStudentFileDetail.ascx" TagName="ucStudentFileDetail" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/RVPHomeListSearch.ascx" TagName="RVPHomeListSearch" TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/SchoolListSearch.ascx" TagName="SchoolListSearch" TagPrefix="uc3" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        table.gvTable {
            width: 100%;
        }

            table.gvTable td {
                vertical-align: top;
            }

        table.tblSFD > tbody > tr {
            height: 20px;
        }


        table.tblSFCD{
            border: 1px solid #999999;
            border-collapse: collapse;
        }

            table.tblSFCD > tbody > tr {
                height: 20px;
            }

            table.tblSFCD td {
                border: 1px solid #999999;
                border-collapse: collapse;
            }

        table.tblSF {
            width: 100%;
        }

            table.tblSF > tbody > tr {
                height: 35px;
            }

            table.tblSF td {
                vertical-align: top;
            }

        table.tblI2DoseToInject td {
            padding-right: 15px;
        }

        table.tblSF2 {
            width: 100%;
        }

            table.tblSF2 > tbody > tr {
                height: 24px;
            }

            table.tblSF2 td {
                vertical-align: top;
            }

        table.tblDV > tbody > tr {
            height: 24px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <script type="text/javascript">
        function SaveFilePathToHiddenField() {
            document.getElementById('<%= hfIFile.ClientID %>').value = document.getElementById('<%= flIVaccinationFile.ClientID %>').value;
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, VaccinationFileUploadBanner %>"
        AlternateText="<%$ Resources: AlternateText, VaccinationFileUploadBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="ibtnI1Next" />--%>
            <asp:PostBackTrigger ControlID="ibtnGUploadFile" />
            <asp:PostBackTrigger ControlID="ibtnINext" />
            <asp:PostBackTrigger ControlID="ibtnCBack" />
        </Triggers>
        <ContentTemplate>
            <div style="height: 4px"></div>
            <asp:HiddenField ID="hfIFile" runat="server" />
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" style="width:950px;display:block" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" style="width:950px;display:block" />
            <asp:MultiView ID="mvCore" runat="server">
                <asp:View ID="vGrid" runat="server">
                    <asp:GridView ID="gvStudentFile" runat="server" CssClass="gvTable" AutoGenerateColumns="False" AllowPaging="True"
                        AllowSorting="True" OnRowDataBound="gvStudentFile_RowDataBound" OnPreRender="gvStudentFile_PreRender"
                        OnRowCommand="gvStudentFile_RowCommand" OnSorting="gvStudentFile_Sorting"
                        OnPageIndexChanging="gvStudentFile_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationFileID %>" SortExpression="Student_File_ID" ItemStyle-Width="130"><ItemTemplate><asp:LinkButton ID="lbtnGStudentFileID" runat="server" Text='<%# Eval("Student_File_ID") %>'></asp:LinkButton></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Scheme %>" SortExpression="Scheme_Code" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGSchemeCode" runat="server" Text='<%# Eval("Scheme_Display_Code") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SchoolRCHCode %>" SortExpression="School_Code" ItemStyle-Width="100"><ItemTemplate><asp:Label ID="lblGSchoolCode" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SPID %>" SortExpression="SP_ID" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGSPID" runat="server" Text='<%# Eval("SP_ID") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationDate %>" SortExpression="Service_Receive_Dtm" ItemStyle-Width="120"><ItemTemplate><asp:Label ID="lblGVaccinationDate" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationReportGenerationDate %>" SortExpression="Final_Checking_Report_Generation_Date"
                                ItemStyle-Width="120"><ItemTemplate><asp:Label ID="lblGVaccinationReportGenerationDate" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SubsidyDoseToInject %>" SortExpression="Dose" ItemStyle-Width="100"><ItemTemplate><asp:Label ID="lblGDoseToInject" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, UploadByAndTime %>" SortExpression="Upload_Dtm" ItemStyle-Width="140"><ItemTemplate><asp:Label ID="lblGUploadByAndTime" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" SortExpression="Record_Status"><ItemTemplate><asp:Label ID="lblGStatus" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ibtnGUploadFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, UploadFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, UploadFileBtn %>" OnClick="ibtnGUploadFile_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vDetail" runat="server">
                    <uc1:ucStudentFileDetail ID="udcStudentFileDetail" runat="server"></uc1:ucStudentFileDetail>
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnDRemoveFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveVaccinationFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RemoveVaccinationFileBtn %>" OnClick="ibtnDRemoveFile_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <%--<asp:View ID="vImport1" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblI1UploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadStudentFile %>"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <asp:Panel ID="panI1" runat="server" DefaultButton="ibtnI1Next">
                        <table class="tblSF">
                            <tr>
                                <td style="width: 180px">
                                    <asp:Label ID="lblI1SchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtI1SchoolCode" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                    <asp:Image ID="imgI1SchoolCodeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblI1ServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtI1ServiceProviderID" runat="server" Width="100px" MaxLength="8"></asp:TextBox>
                                    <asp:Image ID="imgI1ServiceProviderIDError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td style="width: 300px">
                                                <asp:ImageButton ID="ibtnI1Cancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnI1Cancel_Click" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="ibtnI1Next" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnI1Next_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>--%>
                <asp:View ID="vImport2" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblIUploadVaccinationFile" runat="server" Text="<%$ Resources: Text, UploadVaccinationFile %>"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <table class="tblSF">
                        <tr>
                            <td style="width: 260px">
                                <asp:Label ID="lblISchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIScheme" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
                                <asp:Image ID="imgISchemeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                <asp:HiddenField ID="hfScheme" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblIServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIServiceProviderID" runat="server" MaxLength="8" Width="80px" style="position:relative;top:-5px;left:1px;" />&nbsp;
                                <asp:Image ID="imgIServiceProviderIDError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />&nbsp;
                                <asp:ImageButton ID="ibtnSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, SearchSBtn %>" OnClick="ibtnSearchSP_Click" />&nbsp;
                                <asp:ImageButton ID="ibtnClearSearchSP" runat="server" AlternateText="<%$ Resources:AlternateText, ClearBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ClearSBtn %>" OnClick="ibtnClearSearchSP_Click" />&nbsp;
                                <cc1:FilteredTextBoxExtender ID="FilteredSearchSPID" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="txtIServiceProviderID"></cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblIServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIServiceProviderName" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblIPracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIPractice" runat="server" Width="400" style="position:relative;top:-2px" />
                                <asp:Image ID="imgIPracticeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:2px"  />
                            </td>
                        </tr>
                        <asp:Panel ID="panISchoolRCH" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblISchoolRCHCodeText" runat="server" Text="<%$ Resources: Text, SchoolRCHCode %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtISchoolRCHCode" runat="server" Width="150px" MaxLength="30" AutoPostBack="true"></asp:TextBox>
                                    <asp:Image ID="imgISchoolRCHCodeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                    <asp:ImageButton ID="btnSearchSchoolRCH" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png"
                                        ImageAlign="AbsMiddle" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblISchoolRCHNameText" runat="server" Text="<%$ Resources: Text, SchoolName %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblISchoolRCHName" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="panIVaccinationInfo" runat="server">
                            <tr style="height:10px"></tr>
                            <tr>
                                <td colspan="2">
                                    <table class="tblSF" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:264px"></td>
                                            <td style="width:150px;">
                                                <asp:Label ID="lblIOnlyDoseText" runat="server" Text="<%$ Resources: Text, OnlyOr1stDose %>" Class="tableText"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblI2ndDoseText" runat="server" Text="<%$ Resources: Text, 2ndDose %>" Class="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIVaccinationDate1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationDate1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgIVaccinationDate1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                                <cc1:CalendarExtender ID="calIVaccinationDate1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationDate1"
                                                    TargetControlID="txtIVaccinationDate1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationDate1" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationDate1" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIVaccinationDate2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationDate2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgIVaccinationDate2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                                <cc1:CalendarExtender ID="calIVaccinationDate2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationDate2"
                                                    TargetControlID="txtIVaccinationDate2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationDate2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationDate2" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIVaccinationReportGenerateDate1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerateDate1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgIVaccinationReportGenerationDate1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerateDate1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDate1"
                                                    TargetControlID="txtIVaccinationReportGenerateDate1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteVaccinationReportGenerateDate1" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerateDate1" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIVaccinationReportGenerateDate2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerateDate2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgIVaccinationReportGenerationDate2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerateDate2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDate2"
                                                    TargetControlID="txtIVaccinationReportGenerateDate2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationReportGenerateDate2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerateDate2" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
<%--                        
                            <tr>
                                <td>
                                    <asp:Label ID="lblI2DoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:RadioButtonList ID="rbtnI2DoseToInject" runat="server" RepeatDirection="Horizontal"
                                                    CellPadding="0" CellSpacing="0" CssClass="tblI2DoseToInject tableText">
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <asp:Image ID="imgErrorI2DoseToInject" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        </asp:Panel>
                        <asp:Panel ID="panIMMR" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblIDoseOfMMR" runat="server" Text="<%$ Resources: Text, DoseOfMMR %>" />
                                </td>
                                <td>
                                    <asp:dropdownlist ID="ddlIDoseOfMMR" runat="server" Width="150px" MaxLength="30" AutoPostBack="true">
                                        <asp:ListItem runat="server" Selected="True" Text="<%$ Resources: Text, PleaseSelect %>" Value ="" />
                                        <asp:ListItem Text="<%$ Resources: Text, 1stDose2 %>" Value ="1" />
                                        <asp:ListItem Text="<%$ Resources: Text, 2ndDose %>" Value ="2" />
                                        <asp:ListItem Text="<%$ Resources: Text, 3rdDose %>" Value ="3" />
                                    </asp:dropdownlist>
                                    <asp:Image ID="imgIddlIDoseOfMMRError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIGenerationDateMMR" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIVaccinationReportGenerateDateMMR" runat="server" Width="100" MaxLength="10" Style="position:relative;top:-3px" />
                                    <asp:ImageButton ID="ibtnIVaccinationReportGenerateDateMMR" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                    <asp:Image ID="imgIVaccinationReportGenerationDateMMRError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                    <cc1:CalendarExtender ID="calIVaccinationReportGenerateDateMMR" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDateMMR"
                                        TargetControlID="txtIVaccinationReportGenerateDateMMR" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="fteVaccinationReportGenerateDateMMR" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtIVaccinationReportGenerateDateMMR" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Label ID="lblIVaccinationFileText" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="flIVaccinationFile" runat="server" Width="460px" style="position:relative;top:-2px" />
                                <asp:Image ID="imgIVaccinationFileError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:1px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblIVaccineFilePwdText" runat="server" Text="<%$ Resources: Text, VaccinationFilePassword %>" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtIPassword" runat="server" Width="150px" TextMode="Password" style="position:relative;top:-2px" />
                                <asp:Image ID="imgIPasswordError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 300px">
                                            <asp:ImageButton ID="ibtnIBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnIBack_Click" Style="position:relative;left:-3px" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnINext" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnINext_Click"
                                                OnClientClick="SaveFilePathToHiddenField()" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vConfirm" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblCUploadVaccinationFile" runat="server" Text="<%$ Resources: Text, UploadVaccinationFile %>"></asp:Label>
                    </div>                    
                    <table class="tblSFD">
                        <tr>
                            <td style="vertical-align: top;padding-top:18px;" colspan="2">
                                <asp:Label ID="lblCIDHeading" runat="server" Font-Underline="True"
                                    Text="<%$ Resources: Text, InputDetail %>" CssClass="tableText" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 260px">
                                <asp:Label ID="lblCSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCScheme" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <asp:Panel ID="panCMMR" runat="server">
                            <tr>
                                <td style="width: 260px">
                                    <asp:Label ID="lblCDoseOfMMRText" runat="server" Text="<%$ Resources: Text, DoseOfMMR %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCDoseOfMMR" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 260px">
                                    <asp:Label ID="lblCVaccinationDateMMRText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCVaccinationDateMMR" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="panCVaccinationInfo" runat="server">
                            <tr style="height:10px"></tr>
                            <tr>
                                <td colspan="2">
                                    <table class="tblSFD" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:264px"></td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblCOnlyDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, OnlyOr1stDose %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblC2ndDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, 2ndDose %>"></asp:Label>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate1" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblCVaccinationDate1Remark" runat="server" ForeColor="red" class="tableText"/>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate2" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblCVaccinationDate2Remark" runat="server" ForeColor="red" class="tableText"/>                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDate1" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDate2" runat="server" CssClass="tableText"></asp:Label>

                                            </td>
                                        </tr>
                                        <asp:HiddenField ID="hfCDoseToInject" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationDate1" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate1" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationDate2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate2" runat="server" />
                                        <asp:HiddenField ID="hfCSchemeSeq" runat="server" />
                                    </table>                                    
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Label ID="lblCVaccinationFileText" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCVaccinationFile" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCServiceProviderID" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCServiceProviderName" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCMONameText" runat="server" Text="<%$ Resources: Text, MOName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCMOName" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCPracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCPractice" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Image ID="imgCPracticeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                            </td>
                        </tr>
                        <asp:Panel ID="panCSchoolRCH" runat="server">
                            <tr>
                                <td>                                
                                    <asp:Label ID="lblCSchoolRCHCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCSchoolRCHCode" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCSchoolRCHNameText" runat="server" Text="<%$ Resources: Text, SchoolName %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCSchoolRCHName" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td style="vertical-align: top;padding-top:18px;" colspan="2">
                                <asp:Label ID="lblCFDHeading" runat="server" Font-Underline="True"
                                    Text="<%$ Resources:Text, UploadFileDetail %>" CssClass="tableText" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 260px">
                                <asp:Label ID="lblCFDServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDServiceProviderID" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Label ID="lblCFDSPIDDifference" runat="server" ForeColor="red" class="tableText" />
                                <asp:Image ID="imgCFDSPIDError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCFDServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDServiceProviderName" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Label ID="lblCFDSPNameDifference" runat="server" ForeColor="red" class="tableText" />
                                <asp:Image ID="imgCFDSPNameError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCFDMONameText" runat="server" Text="<%$ Resources: Text, MOName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDMOName" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Label ID="lblCFDMONameDifference" runat="server" ForeColor="red" class="tableText" />
                                <asp:Image ID="imgCFDMONameError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCFDSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDScheme" runat="server" class="tableText"></asp:Label>
                                <asp:Image ID="imgCFDSchemeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                            </td>
                        </tr>
                        <tr id="trCFDSubsidiy" runat="server">
                            <td>
                                <asp:Label ID="lblCFDSubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDSubsidy" runat="server" class="tableText"></asp:Label>
                                <asp:Image ID="imgCFDSubsidyError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                <asp:HiddenField ID="hfCFDSubsidizeCode" runat="server" />
                            </td>
                        </tr>
                        <tr id="trCFDDose" runat="server">
                            <td>
                                <asp:Label ID="lblCFDDoseOfMMRText" runat="server" Text="<%$ Resources: Text, DoseOfMMR %>" />
                            </td>
                            <td>
                                <asp:Label ID="lblCFDDoseOfMMR" runat="server" class="tableText" />
                                <asp:Image ID="imgCFDDoseOfMMRError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                <asp:HiddenField ID="hfCFDDoseOfMMR" runat="server" />
                            </td>
                        </tr>

                        <tr id="trCFDSchoolCode" runat="server">
                            <td>
                                <asp:Label ID="lblCFDSchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDSchoolCode" runat="server" class="tableText"></asp:Label>
                                <asp:Label ID="lblCFDSchoolCodeDifference" runat="server" ForeColor="red" class="tableText" />
                                <asp:Image ID="imgCFDSchoolCodeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                            </td>
                        </tr>
                        <tr id="trCFDSchoolName" runat="server">
                            <td>
                                <asp:Label ID="lblCFDSchoolNameText" runat="server" Text="<%$ Resources: Text, SchoolName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDSchoolName" runat="server" class="tableText"></asp:Label>
                                <asp:Label ID="lblCFDSchoolNameDifference" runat="server" ForeColor="red" class="tableText" />
                                <asp:Image ID="imgCFDSchoolNameError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCFDNoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDNoOfClass" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCFDNoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCFDNoOfStudent" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblCFDClass" runat="server" Text="<%$ Resources: Text, Class %>"></asp:Label>
                            </td>
                            <td>
                                <asp:GridView ID="gvCFDClassDetail" runat="server" CssClass="gvTable" Width="330px" AutoGenerateColumns="False" AllowPaging="False"
                                    AllowSorting="False" OnRowDataBound="gvCFDClassDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, ClassName %>" ItemStyle-Width="180"><ItemTemplate><asp:Label ID="lblGClassName" runat="server" Text='<%# Eval("Class_Name") %>'></asp:Label><asp:Image ID="imgGClassNameError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" /></ItemTemplate></asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, NoOfStudent %>" ItemStyle-Width="120"><ItemTemplate><asp:Label ID="lblGClassNo" runat="server" Text='<%# Eval("No_Of_Student") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                </Columns>
                                </asp:GridView>                              
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 300px">
                                            <asp:ImageButton ID="ibtnCBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnCBack_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnCConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnCConfirm_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vErrorWarning" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblEUploadVaccinationFile" runat="server" Text="<%$ Resources: Text, UploadVaccinationFile %>"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <table class="tblSF2">
                        <tr>
                            <td style="width: 200px">
                                <asp:Label ID="lblENoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblENoOfClass" runat="server" class="tableText"></asp:Label>
                                <asp:HiddenField ID="hfEGenerationID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblENoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblENoOfStudent" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblENoOfSuccessfulRecordText" runat="server" Text="<%$ Resources: Text, NoOfSuccessfulRecord %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblENoOfSuccessfulRecord" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblENoOfErrorRecordText" runat="server" Text="<%$ Resources: Text, NoOfErrorRecord %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblENoOfErrorRecord" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblENoOfWarningRecordText" runat="server" Text="<%$ Resources: Text, NoOfWarningRecord %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblENoOfWarningRecord" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trEOverLimit" runat="server">
                            <td colspan="2" style="padding-top: 8px">
                                <asp:Label ID="lblEOverLimit" runat="server" ForeColor="Red" Text="<%$ Resources: Text, VaccinationFileErrorOverLimit %>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvE" runat="server" CssClass="gvTable" Width="1200px" AutoGenerateColumns="False" AllowPaging="True"
                        AllowSorting="False" OnRowDataBound="gvE_RowDataBound" OnPreRender="gvE_PreRender" OnPageIndexChanging="gvE_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ClassName %>" ItemStyle-Width="40"><ItemTemplate><asp:Label ID="lblGClassName" runat="server" Text='<%# Eval("Class_Name") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ClassNo %>" ItemStyle-Width="30"><ItemTemplate><asp:Label ID="lblGClassNo" runat="server" Text='<%# Eval("Class_No") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DocTypeIDNL %>" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGDocType" runat="server" Text='<%# Eval("Doc_Code") %>' /><br /><asp:Label ID="lblGDocNo" runat="server" Text='<%# Eval("Doc_No") %>' /></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ContactNo2 %>" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGContactNo" runat="server" Text='<%# Eval("Contact_No") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ChineseName %>" ItemStyle-Width="70"><ItemTemplate><asp:Label ID="lblGChineseName" runat="server" Text='<%# Eval("Name_CH_Excel") %>' Font-Names="HA_MingLiu"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishSurname %>" ItemStyle-Width="70"><ItemTemplate><asp:Label ID="lblGSurnameEN" runat="server" Text='<%# Eval("Surname_EN") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishGivenName %>" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGGivenNameEN" runat="server" Text='<%# Eval("Given_Name_EN") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" ItemStyle-Width="30"><ItemTemplate><asp:Label ID="lblGSex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DOB %>" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGDOB" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, OtherField %>" ItemStyle-Width="140"><ItemTemplate><asp:Label ID="lblGOtherField" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceDate %>" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGServiceDate" runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ErrorMessage %>" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGErrorMessage" runat="server" Text='<%# Eval("Upload_Error") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, WarningMessage %>" ItemStyle-Width="80"><ItemTemplate><asp:Label ID="lblGWarningMessage" runat="server" Text='<%# Eval("Upload_Warning") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnEReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnEReturn_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnEExportReport" runat="server" ImageUrl="<%$ Resources: ImageUrl, ExportReportBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ExportReportBtn %>" OnClick="ibtnEExportReport_Click" />
                                <asp:ImageButton ID="ibtnEConfirmAcceptWarning" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmAndAcceptWarningBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ConfirmAndAcceptWarningBtn %>" OnClick="ibtnEConfirmAcceptWarning_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vFinish" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnFReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnFReturn_Click" style="display:block" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vConcurrentUpdate" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCUReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnCUReturn_Click" style="display:block" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <%-- Pop up for Remove File --%>
            <asp:Button ID="btnRF" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeRemoveFile" runat="server" TargetControlID="btnRF"
                PopupControlID="panRF" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panRFHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panRF" runat="server" Style="display: none">
                <asp:Panel ID="panRFHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblRFHeader" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgRFIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblRFText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmToRemoveFileQ %>"></asp:Label></td>
                                    <td style="width: 40px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnRFConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnRFConfirm_Click" />
                                        <asp:ImageButton ID="ibtnRFCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnRFCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Remove File --%>
            <%-- Pop up for Export Warning Error Record --%>
            <asp:Button ID="btnER" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeExportReport" runat="server" TargetControlID="btnER"
                PopupControlID="panER" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panERHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panER" runat="server" Style="display: none">
                <asp:Panel ID="panERHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblERTitle" runat="server" Text="<%$ Resources: Text, ExportReport %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF; padding: 5px">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 45px; vertical-align: top">
                                        <asp:Image ID="imgERIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblERMessage" runat="server" Font-Bold="True" Text="<%$ Resources: Text, StudentErrorWarningReportPrompt %>"></asp:Label>
                                    </td>
                                    <td style="width: 10px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnERDownloadNow" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadNowBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, DownloadNowBtn %>" OnClick="ibtnERDownloadNow_Click" />
                                        <asp:ImageButton ID="ibtnERDownloadLater" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadLaterBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, DownloadLaterBtn %>" OnClick="ibtnERDownloadLater_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Export Warning Error Record --%>
            <%-- Pop up for Duplicate Vaccination Date --%>
            <asp:Button ID="btnDV" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeDV" runat="server" TargetControlID="btnDV"
                PopupControlID="panDV" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panDVHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panDV" runat="server" Style="display: none">
                <asp:Panel ID="panDVHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 700px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDVTitle" runat="server" Text="<%$ Resources: Text, Confirmation %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 700px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF; padding: 5px">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 45px; vertical-align: top">
                                        <asp:Image ID="imgDVIcon" runat="server" ImageUrl="~/Images/others/information.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblDVMessage1" runat="server" Font-Bold="True" Text="<%$ Resources: Text, DuplicateSchoolVaccinationMessage1 %>"></asp:Label>
                                        <table class="tblDV">
                                            <tr>
                                                <td style="width: 250px">
                                                    <asp:Label ID="lblDVVaccinationFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationFileID" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVScheme" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderID" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderName" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVPracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVPractice" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>                                                    
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationDate" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationReportGenerationDate" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVSubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVSubsidy" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVDoseToInject" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVUploadedByText" runat="server" Text="<%$ Resources: Text, UploadedBy %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVUploadedBy" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVStatus" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblDVMessage2" runat="server" Font-Bold="True" Text="<%$ Resources: Text, DuplicateSchoolVaccinationMessage2 %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDVConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnDVConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDVCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnDVCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Duplicate Vaccination Date --%>
            <%-- Pop up for RVP Home List --%>
            <asp:Button ID="btnModalPopupRVPHomeListSearch" runat="server" style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderRVPHomeListSearch" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupRVPHomeListSearch" PopupControlID="panPopupRVPHomeListSearch"
                PopupDragHandleControlID="panRCHSearchHomeListHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupRVPHomeListSearch" runat="server">
                <asp:Panel Style="cursor: move" ID="panRCHSearchHomeListHeading" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                <asp:Label ID="lblRCHSearchFormTitle" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 980px" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                                <asp:Panel ID="panRCHRecord" runat="server">
                                    <uc2:RVPHomeListSearch ID="udcRVPHomeListSearch" runat="server"></uc2:RVPHomeListSearch>
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                            </td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                <asp:ImageButton ID="ibtnPopupRVPHomeListSearchCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPopupRVPHomeListSearchCancel_Click">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnPopupRVPHomeListSearchSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" OnClick="ibtnPopupRVPHomeListSearchSelect_Click" />
                            </td>
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
                    </tbody>
                </table>
            </asp:Panel>
            <%-- End of Pop up for RVP Home List --%>            
            <%-- Pop up for School List --%>
            <asp:Button ID="btnModalPopupSchoolListSearch" runat="server" style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderSchoolListSearch" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupSchoolListSearch" PopupControlID="panPopupSchoolListSearch"
                PopupDragHandleControlID="panSearchSchoolListHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupSchoolListSearch" runat="server">
                <asp:Panel Style="cursor: move" ID="panSearchSchoolListHeading" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                <asp:Label ID="lblSearchSchoolTitle" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 980px" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                                <asp:Panel ID="panSchoolList" runat="server">
                                    <uc3:SchoolListSearch ID="udcSchoolListSearch" runat="server"></uc3:SchoolListSearch>
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                            </td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                <asp:ImageButton ID="ibtnPopupSchoolListSearchCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPopupSchoolListSearchCancel_Click">
                                </asp:ImageButton>
                                <asp:ImageButton ID="ibtnPopupSchoolListSearchSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" OnClick="ibtnPopupSchoolListSearchSelect_Click" />
                            </td>
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
                    </tbody>
                </table>
            </asp:Panel>
            <%-- End of Pop up for School List --%>
            <%-- Pop up for Warning Message --%>
            <asp:Button ID="btnModalPopupWarningMessage" runat="server" style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderWarningMessage" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupWarningMessage" PopupControlID="panPopupWarningMessage"
                PopupDragHandleControlID="panWarningMessageHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupWarningMessage" runat="server" Height="450px">
                <asp:Panel Style="cursor: move" ID="panWarningMessageHeading" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                <asp:Label ID="lblWarningMessageHeading" runat="server" Text="<%$ Resources:Text, Warning %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 680px" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">                            
                            <asp:Panel ID="pnlWarningMsgContent" runat="server">
                                <table width="638px">
                                    <tr>
                                        <td>
                                            <cc2:WarningMessageBox ID="udcWarningMessageBox" runat="server" ShowHeader="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle"
                                            colspan="2">
                                            <asp:ImageButton ID="ibtnWarningMessageConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnWarningMessageConfirm_Click" />
                                            <asp:ImageButton ID="ibtnWarningMessageCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnWarningMessageCancel_Click">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
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
            <%-- End of Pop up for Warning Message --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
